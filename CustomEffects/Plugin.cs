namespace CustomEffects
{
    using System;
    using CustomEffects.Configs;
    using Exiled.API.Features;
    
    /// <inheritdoc />
    public sealed class Plugin : Plugin<Config>
    {
        /// <inheritdoc />
        public override string Author => "ficus-x";

        /// <inheritdoc />
        public override string Name => "CustomEffects";

        /// <inheritdoc />
        public override string Prefix => "CustomEffects";

        /// <inheritdoc />
        public override Version RequiredExiledVersion { get; } = new Version(5, 2, 1);

        /// <summary>
        /// Gets the <see cref="Plugin"/> Instance
        /// </summary>
        public static Plugin Instance { get; private set; }

        public override void OnEnabled()
        {
            Instance = this;
            
            Config.LoadConfigs();
            
            Config.EffectsConfigs.ContusionEffect.Register();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Instance = null;
            
            Config.EffectsConfigs.ContusionEffect.Unregister();
            
            base.OnDisabled();
        }
    }
}