namespace CustomEffects.Commands.SubCommands
{
    using System;
    using System.Linq;
    using CommandSystem;
    using CustomEffects.Effects;
    using Exiled.Permissions.Extensions;
    
    public sealed class List : ICommand
    {
        public string Command => "list";
        public string[] Aliases => Array.Empty<string>();
        public string Description => "Gives a list of registered custom effects";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            // if (!((CommandSender) sender).CheckPermission($"ce.{Command}"))
            // {
            //     response = $"You do not have permission to execute this command. Permission: ce.{Command}";
            //     return false;
            // }

            if (CustomEffect.Registered.Count == 0)
            {
                response = "There are no custom effects currently on this server.";
                return false;
            }

            response = string.Empty;
            
            foreach (var customEffect in CustomEffect.Registered.OrderBy(e => e.Id))
            {
                response += $"{customEffect.Name} ({customEffect.Id}) - {customEffect.Description}\n";
            }

            return true;
        }
    }
}