namespace CustomEffects.Commands.SubCommands
{
    using System.Linq;
    using CustomEffects.Effects;
    using System;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    
    public sealed class Remove : ICommand
    {
        public string Command => "remove";
        public string[] Aliases => new[]{ "rm" };
        public string Description => "Removes a custom effect from a player";

        public const string Usage = "Usage: ce remove (Player ID) (Custom effect ID)";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission($"ce.{Command}"))
            {
                response = $"You do not have permission to execute this command. Permission: ce.{Command}";
                return false;
            }

            if (arguments.Count != 2 || !int.TryParse(arguments.At(0), out int playerId) || !uint.TryParse(arguments.At(1), out uint effectId))
            {
                response = Usage;
                return false;
            }

            Player effectedPlayer = Player.Get(playerId);

            if (effectedPlayer == null)
            {
                response = "Player was not found.";
                return false;
            }

            CustomEffect customEffect = CustomEffect.Registered.FirstOrDefault(ce => ce.Id == effectId);
            
            if (customEffect == null)
            {
                response = "Custom effect was not found.";
                return false;
            }

            customEffect.DisableEffects(effectedPlayer);

            response = "This custom effect was removed successfully.";
            return true;
        }
    }
}