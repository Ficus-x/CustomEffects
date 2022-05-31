namespace CustomEffects.Effects
{
    using System.Collections.Generic;
    using Exiled.API.Enums;

    public sealed class ContusionEffect : CustomEffect
    {
        public override uint Id { get; set; } = 1;

        public override string Name { get; set; } = "Contusion";
        
        public override Dictionary<EffectType, float> GivenEffects { get; set; } = new Dictionary<EffectType, float>()
        {
            {EffectType.Poisoned, 6f}
        };

        public override byte Chance { get; set; } = 60;

        public override List<RoleType> NeededTargetRoles { get; set; } = new List<RoleType>()
        {
            RoleType.ClassD
        };

        public override List<DamageType> NeededDamageTypes { get; set; } = new List<DamageType>()
        {
            DamageType.Scp018
        };

        public override string Hint { get; set; } = "You got contusion.";
    }
}