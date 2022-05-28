namespace CustomEffects.Configs
{
    using System.ComponentModel;
    using System.IO;
    using Exiled.API.Features;
    using Exiled.API.Interfaces;
    using Exiled.Loader;
    
    public sealed class Config : IConfig
    {
        public Effects EffectsConfigs;
        
        [Description("Whether or not this plugin is enabled")]
        public bool IsEnabled { get; set; } = true;
        
        [Description("The folder path where effect configs will be stored.")]
        public string EffectsFolder { get; set; } = Path.Combine(Paths.Configs, "CustomEffects");

        [Description("The file name to load effect configs from.")]
        public string EffectsFile { get; set; } = "global.yml";
        
        public void LoadConfigs()
        {
            if (!Directory.Exists(EffectsFolder))
                Directory.CreateDirectory(EffectsFolder);
        
            string filePath = Path.Combine(EffectsFolder, EffectsFile);
            
            if (!File.Exists(filePath))
            {
                EffectsConfigs = new Effects();
                File.WriteAllText(filePath, Loader.Serializer.Serialize(EffectsConfigs));
            }
            else
            {
                EffectsConfigs = Loader.Deserializer.Deserialize<Effects>(File.ReadAllText(filePath));
                File.WriteAllText(filePath, Loader.Serializer.Serialize(EffectsConfigs));
            }
        }
    }
}