namespace CustomEffects.Commands
{
    using System;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    
    [CommandHandler(typeof(ClientCommandHandler))]
    public sealed class CustomEffectCommand : ICommand
    {
        public string Command => "CustomEffects";
        public string[] Aliases { get; } = { "ce" };
        public string Description => "Commands related to CustomEffects";

        public const string GiveUsage = "Usage: ce give (playerID) (EffectID)";
        public const string EffectedSuccessfully = "This player was effected successfully";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender) sender).CheckPermission("ce.effects"))
            {
                response = "You do not have permissions to use this command";
                return false;
            }
            
            if (arguments.Count == 3 && arguments.At(1).Equals("give") && int.TryParse(arguments.At(2), out int playerId) && uint.TryParse(arguments.At(3), out uint effectId))
            {
                Player effectedPlayer = Player.Get(playerId);

                if (effectedPlayer == null)
                {
                    response = EffectedSuccessfully;
                    return false;
                }
                
                if (effectId == Plugin.Instance.Config.EffectsConfigs.Acceleration.Id)
                {
                    foreach (var effects in Plugin.Instance.Config.EffectsConfigs.Acceleration.GivenEffects)
                        effectedPlayer.EnableEffect(effects.Key, effects.Value);

                    response = EffectedSuccessfully;
                    return true;
                }

                if (effectId == Plugin.Instance.Config.EffectsConfigs.Deceleration.Id)
                {
                    foreach (var effects in Plugin.Instance.Config.EffectsConfigs.Deceleration.GivenEffects)
                        effectedPlayer.EnableEffect(effects.Key, effects.Value);

                    response = EffectedSuccessfully;
                    return true;
                }

                response = GiveUsage;
                return false;
            }

            response = GiveUsage;
            return false;
        }
    }
}