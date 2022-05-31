namespace CustomEffects.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.CustomItems.API.Features;
    using Exiled.Events.EventArgs;
    using PlayerStatsSystem;
    
    /// <summary>
    /// Abstract class for making custom effects.
    /// </summary>
    public abstract class CustomEffect
    {
        /// <summary>
        /// Gets a list of all registered custom effects.
        /// </summary>
        public static HashSet<CustomEffect> Registered { get; } = new HashSet<CustomEffect>();

        /// <summary>
        /// Gets or sets the id of custom effect.
        /// </summary>
        public abstract uint Id { get; set; }

        /// <summary>
        /// Gets or sets the name of this custom effect.
        /// </summary>
        public abstract string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the description of this custom effect.
        /// </summary>
        public abstract string Description { get; set; }
        
        /// <summary>
        /// Gets or sets effects to be given to player. Set 0 to make effect endless.
        /// </summary>
        public virtual Dictionary<EffectType, float> GivenEffects { get; set; }

        /// <summary>
        /// Gets or sets the damage to be needed to activate effects.
        /// </summary>
        public virtual float NeededDamage { get; set; } = 0;

        /// <summary>
        /// Gets or sets the chance of activating effects.
        /// </summary>
        public virtual byte Chance { get; set; } = 100;
        
        /// <summary>
        /// Gets or sets the hint to be showed when player gets effects.
        /// </summary>
        public virtual string Hint { get; set; }

        /// <summary>
        /// Gets or sets the duration hint.
        /// </summary>
        public virtual float HintDuration { get; set; } = 3f;

        /// <summary>
        /// Gets or sets the damage types to be need to activate effects.
        /// </summary>
        public virtual List<DamageType> NeededDamageTypes { get; set; }

        /// <summary>
        /// Gets or sets the role types to be needed target player to activate effects.
        /// </summary>
        public virtual List<RoleType> NeededTargetRoles { get; set; }

        /// <summary>
        /// Gets or sets the role types to be needed target player to activate effects.
        /// </summary>
        public virtual List<RoleType> NeededAttackerRoles { get; set; }

        /// <summary>
        /// Gets or sets the hitBox types to be need to activate effects.
        /// </summary>
        public virtual List<HitboxType> NeededHitBoxTypes { get; set; }

        /// <summary>
        /// Gets or sets custom items to stop activating effects.
        /// </summary>
        public virtual List<uint> IgnoredCustomItems { get; set; }

        /// <summary>
        /// Adds custom prediction to make player get custom effect. if returns false, the custom effect will not be given
        /// </summary>
        public virtual bool CustomConditions(HurtingEventArgs ev)
            => true;

        public void ProceedDamage(HurtingEventArgs ev)
        {
            if (Chance != 0 && new Random().Next(0, 100) > Chance)
                return;
            
            if (NeededDamage != 0 && !(ev.Amount >= NeededDamage))
                return;
            
            if (NeededDamageTypes != null && NeededDamageTypes.All(damage => damage != ev.Handler.Type))
                return;
            
            if (NeededHitBoxTypes != null && ev.Handler.Is(out UniversalDamageHandler hitBox) && NeededHitBoxTypes.All(hb => hb != hitBox.Hitbox))
                return;
            
            if (NeededTargetRoles != null && NeededTargetRoles.All(rt => rt != ev.Target.Role.Type))
                return;
            
            if (NeededAttackerRoles != null && (ev.Attacker == null || NeededAttackerRoles.All(rna => rna != ev.Attacker.Role.Type)))
                return;
            
            if (CustomItem.TryGet(ev.Target, out CustomItem item) && IgnoredCustomItems != null && IgnoredCustomItems.All(ci => ci != item.Id))
                return;
            
            if (!CustomConditions(ev))
                return;
            
            EnableEffects(ev.Target);
        }

        public virtual void SubscribeEvents()
            => Exiled.Events.Handlers.Player.Hurting += ProceedDamage;

        public virtual void UnsubscribeEvents()
            => Exiled.Events.Handlers.Player.Hurting -= ProceedDamage;

        public virtual void EnableEffects(Player player)
        {
            if (GivenEffects != null)
            {
                foreach (var effect in GivenEffects)
                    player.EnableEffect(effect.Key, effect.Value);
            }
            
            if (Hint != string.Empty)
                player.ShowHint(Hint, HintDuration);
        }

        public virtual void DisableEffects(Player player)
        {
            if (GivenEffects != null)
            {
                foreach (var effect in GivenEffects)
                    player.DisableEffect(effect.Key);
            }
        }
        
        internal void TryRegister()
        {
            if (!Plugin.Instance.Config.IsEnabled)
                return;

            if (!Registered.Contains(this))
            {
                if (Registered.Any(r => (int) r.Id == (int) Id))
                {
                    Log.Warn($"{Name} ({Id}) has tried to register with the same effect ID as another effect. It will not be registered!");
                    return;
                }

                Registered.Add(this);
                Log.Debug($"Custom effect {Name} ({Id}) has been successfully registered.", Plugin.Instance.Config.Debug);
                
                SubscribeEvents();

                return;
            }
            
            Log.Warn($"Couldn't register {Name} ({Id}) as it already exists.");
        }

        internal void TryUnregister()
        {
            UnsubscribeEvents();
            
            if (CustomEffect.Registered.Remove(this))
                return;

            Log.Warn($"Cannot unregister CustomEffect {Id}, it hasn't been registered yet.");
        }
    }
}