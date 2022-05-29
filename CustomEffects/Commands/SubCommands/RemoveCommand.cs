namespace CustomEffects.Commands.SubCommands
{
    using System;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public sealed class RemoveCommand : ICommand
    {
        public string Command => "remove";
        public string[] Aliases { get; } = { "rm" };
        public string Description => "Removes a custom effect from a player";

        public const string Usage = "Usage: ce remove (PlayerID)";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission($"ce.{Command}"))
            {
                response = $"You do not have permission to execute this command. Permission: ce.{Command}";
                return false;
            }

            if (arguments.Count != 2 || int.TryParse(arguments.At(0), out int playerId) || uint.TryParse(arguments.At(1), out uint effectId))
            {
                response = Usage;
                return false;
            }

            Player player = Player.Get(playerId);

            if (player == null)
            {
                response = "Player was not found";
                return false;
            }

            switch (effectId)
            {
                default:
                    response = "Custom effect was not found";
                    return false;
            }
        }
    }
}