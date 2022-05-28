namespace CustomEffects.Configs
{
    using CustomEffects.Effects;
    
    public class Effects
    {
        public Deceleration Deceleration { get; set; } = new Deceleration();

        public Acceleration Acceleration { get; set; } = new Acceleration();
    }
}