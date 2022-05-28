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
    
    public abstract class CustomEffect
    {
        /// <summary>
        /// Gets or sets the id of custom effect
        /// </summary>
        public abstract uint Id { get; set; }
        
        /// <summary>
        /// Gets or sets effects to be given to player. Type 0 to make effect endless
        /// </summary>
        public abstract Dictionary<EffectType, float> GivenEffects { get; set; }

        /// <summary>
        /// Gets or sets the damage to be needed to activate effects
        /// </summary>
        public virtual float NeededDamage { get; set; } = 0;

        /// <summary>
        /// Gets or sets the chance of activating effects
        /// </summary>
        public virtual byte Chance { get; set; } = 100;
        
        /// <summary>
        /// Gets or sets the damage types to be need to activate effects
        /// </summary>
        public virtual List<DamageType> NeededDamageTypes { get; set; }
        
        /// <summary>
        /// Gets or sets the role types to be needed target player to activate effects
        /// </summary>
        public virtual List<RoleType> NeededTargetRoles { get; set; }
        
        /// <summary>
        /// Gets or sets the role types to be needed target player to activate effects
        /// </summary>
        public virtual List<RoleType> NeededAttackerRoles { get; set; }
        
        /// <summary>
        /// Gets or sets the hitBox types to be need to activate effects
        /// </summary>
        public virtual List<HitboxType> NeededHitBoxTypes { get; set; }
        
        /// <summary>
        /// Gets or sets custom items to stop activating effects
        /// </summary>
        public virtual List<uint> IgnoredCustomItems { get; set; }
        
        /// <summary>
        /// The method to be called when the player get effects
        /// </summary>
        /// <param name="player">The player to be effected</param>
        public virtual void ApplyAfterEffects(Player player) {}
        
        public void ProceedDamage(HurtingEventArgs ev)
        {
            if (Chance == 0 || new Random().Next(0, 100) <= Chance)
            {
                if (NeededDamage == 0 || ev.Amount >= NeededDamage)
                {
                    if (NeededDamageTypes == null || NeededDamageTypes.Any(d => d == ev.Handler.Type))
                    {
                        if (NeededHitBoxTypes == null || !ev.Handler.Is(out UniversalDamageHandler hitBox) || NeededHitBoxTypes.Any(h => h == hitBox.Hitbox))
                        {
                            if (NeededTargetRoles == null || NeededTargetRoles.Any(r => r == ev.Target.Role.Type))
                            {
                                if (ev.Attacker != null && (NeededAttackerRoles == null || NeededAttackerRoles.Any(r => r == ev.Attacker.Role.Type)))
                                {
                                    if (!CustomItem.TryGet(ev.Target, out CustomItem item) || IgnoredCustomItems == null || IgnoredCustomItems.Any(c => c == item.Id))
                                    {
                                        foreach (var effect in GivenEffects)
                                            ev.Target.EnableEffect(effect.Key, effect.Value);
                                        
                                        ApplyAfterEffects(ev.Target);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Register()
            => Exiled.Events.Handlers.Player.Hurting += ProceedDamage;

        public void Unregister()
            => Exiled.Events.Handlers.Player.Hurting -= ProceedDamage;
    }
}