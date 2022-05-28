namespace CustomEffects.Effects
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    
    public sealed class Acceleration : CustomEffect
    {
        public override uint Id { get; set; } = 2;

        public override Dictionary<EffectType, float> GivenEffects { get; set; } = new Dictionary<EffectType, float>()
        {
            {EffectType.MovementBoost, 5f}
        };

        public override float NeededDamage { get; set; } = 0;

        public override List<RoleType> NeededTargetRoles { get; set; } = new List<RoleType>()
        {
            RoleType.Scp93953, RoleType.Scp93989
        };

        public override List<DamageType> NeededDamageTypes { get; set; } = new List<DamageType>()
        {
            DamageType.Com15, DamageType.Com18, DamageType.Crossvec, DamageType.Firearm,
            DamageType.Fsp9, DamageType.Logicer, DamageType.Revolver, DamageType.Shotgun, DamageType.E11Sr
        };

        public override void ApplyAfterEffects(Player player)
            => player.ShowHint("Acceleration!");
    }
}