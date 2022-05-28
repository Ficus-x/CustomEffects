namespace CustomEffects.Effects
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    
    public sealed class Deceleration : CustomEffect
    {
        public override uint Id { get; set; } = 1;

        public override Dictionary<EffectType, float> GivenEffects { get; set; } = new Dictionary<EffectType, float>()
        {
            {EffectType.Disabled, 2f}
        };

        public override List<DamageType> NeededDamageTypes { get; set; } = new List<DamageType>()
        {
            DamageType.Scp939
        };

        public override float NeededDamage { get; set; } = 30;

        public override byte Chance { get; set; } = 100;

        public override void ApplyAfterEffects(Player player)
            => player.ShowHint("Deceleration!");
    }
}