namespace CustomEffects.Effects
{
    using Exiled.API.Features.DamageHandlers;
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
        /// Gets or sets the id of custom effect.
        /// </summary>
        public abstract uint Id { get; set; }
        
        /// <summary>
        /// Gets or sets effects to be given to player. Set 0 to make effect endless.
        /// </summary>
        public abstract Dictionary<EffectType, float> GivenEffects { get; set; }

        /// <summary>
        /// Gets or sets the damage to be needed to activate effects.
        /// </summary>
        public virtual float NeededDamage { get; set; } = 0;

        /// <summary>
        /// Gets or sets the chance of activating effects.
        /// </summary>
        public virtual byte Chance { get; set; } = 100;
        
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
        /// Gets or sets the team to be needed target player to activate effects.
        /// </summary>
        public virtual List<Team> NeededTargetTeams { get; set; }
        
        /// <summary>
        /// Gets or sets the team to be needed target player to activate effects.
        /// </summary>
        public virtual List<Team> NeededAttackerTeams { get; set; }
        
        /// <summary>
        /// Gets or sets the side to be needed target player to activate effects.
        /// </summary>
        public virtual List<Side> NeededTargetSides { get; set; }
        
        /// <summary>
        /// Gets or sets the side to be needed target player to activate effects.
        /// </summary>
        public virtual List<Side> NeededAttackerSides { get; set; }
        
        /// <summary>
        /// Gets or sets the hitBox types to be need to activate effects.
        /// </summary>
        public virtual List<HitboxType> NeededHitBoxTypes { get; set; }
        
        /// <summary>
        /// Gets or sets custom items to stop activating effects.
        /// </summary>
        public virtual List<uint> IgnoredCustomItems { get; set; }
        
        /// <summary>
        /// The method to be called before the player get effects.
        /// </summary>
        /// <param name="player">The player to be effected</param>
        public virtual void ApplyBeforeEffects(Player player) {}
        
        /// <summary>
        /// The method to be called when the player got effects.
        /// </summary>
        /// <param name="player">The player to be effected</param>
        public virtual void ApplyAfterEffects(Player player) {}

        /// <summary>
        /// Adds another predictions to make player get custom effect. if returns false the custom effect will not be given
        /// </summary>
        /// <param name="attacker">Gets the attacker player.</param>
        /// <param name="target">Gets the target player, who is going to be hurt.</param>
        /// <param name="handler">Gets the <see cref="T:Exiled.API.Features.DamageHandlers.CustomDamageHandler" /> for the event.</param>
        /// <param name="amount">Gets the amount of inflicted damage.</param>
        public virtual bool AddPrediction(Player attacker, Player target, CustomDamageHandler handler, float amount) { return true; }

        public void ProceedDamage(HurtingEventArgs ev)
        {
            if (Chance == 0 || new Random().Next(0, 100) <= Chance)
            {
                if (NeededDamage == 0 || ev.Amount >= NeededDamage)
                {
                    if (NeededDamageTypes == null || NeededDamageTypes.Any(damage => damage == ev.Handler.Type))
                    {
                        if (NeededHitBoxTypes == null || !ev.Handler.Is(out UniversalDamageHandler hitBox) || NeededHitBoxTypes.Any(hb => hb == hitBox.Hitbox))
                        {
                            if (NeededTargetRoles == null || NeededTargetRoles.Any(rt => rt == ev.Target.Role.Type))
                            {
                                if (NeededAttackerRoles == null || (ev.Attacker != null && NeededAttackerRoles.Any(rna => rna == ev.Attacker.Role.Type)))
                                {
                                    if (!CustomItem.TryGet(ev.Target, out CustomItem item) || IgnoredCustomItems == null || IgnoredCustomItems.Any(ci => ci == item.Id))
                                    {
                                        if (NeededAttackerTeams == null || NeededAttackerTeams.Any(at => at == ev.Attacker.Role.Team))
                                        {
                                            if (NeededTargetTeams == null || NeededTargetTeams.Any(tt => tt == ev.Target.Role.Team))
                                            {
                                                if (NeededAttackerSides == null || NeededAttackerSides.Any(nas => nas == ev.Attacker.Role.Side))
                                                {
                                                    if (NeededTargetSides == null || NeededTargetSides.Any(nts => nts == ev.Target.Role.Side))
                                                    {
                                                        if (AddPrediction(ev.Attacker, ev.Target, ev.Handler, ev.Amount))
                                                        {
                                                            ApplyBeforeEffects(ev.Target);
                                        
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