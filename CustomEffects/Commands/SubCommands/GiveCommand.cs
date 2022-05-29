namespace CustomEffects.Commands.SubCommands
{
    using System;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public sealed class GiveCommand : ICommand
    {
        public string Command => "give";
        public string[] Aliases { get; } = { "gv" };
        public string Description => "Gives a custom effect to a player";

        public const string Usage = "Usage: ce give (playerID) (CustomEffect)";
        
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
                response = "Player was not found";
                return false;
            }

            switch (effectId)
            {
                default:
                    response = "Effect was not found";
                    return false;
            }
        }
    }
}