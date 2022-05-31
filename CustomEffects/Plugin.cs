namespace CustomEffects
{
    using System;
    using CustomEffects.Configs;
    using Exiled.API.Features;
    
    /// <inheritdoc />
    public sealed class Plugin : Plugin<Config>
    {
        /// <inheritdoc />
        public override string Author => "Ficus-x";

        /// <inheritdoc />
        public override string Name => "CustomEffects";

        /// <inheritdoc />
        public override string Prefix => "CustomEffects";

        /// <inheritdoc />
        public override Version Version { get; } = new Version(1, 0, 0);

        /// <inheritdoc />
        public override Version RequiredExiledVersion { get; } = new Version(5, 2, 1);
        
        /// <summary>
        /// Gets the <see cref="Plugin"/> Instance
        /// </summary>
        public static Plugin Instance { get; private set; }

        /// <inheritdoc />
        public override void OnEnabled()
        {
            Instance = this;
            
            Config.LoadConfigs();
            
            Config.EffectsConfigs.ContusionEffects.Register();

            base.OnEnabled();
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            Instance = null;
            
            Config.EffectsConfigs.ContusionEffects.Unregister();
            
            base.OnDisabled();
        }
    }
}