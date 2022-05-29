namespace CustomEffects.Commands
{
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;
    using CommandSystem;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public sealed class CustomEffectCommand : ParentCommand
    {
        public override string Command => "CustomEffects";
        public override string[] Aliases { get; } = { "ce" };
        public override string Description => "The CustomEffects parent command";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new SubCommands.GiveCommand());
            RegisterCommand(new SubCommands.RemoveCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            
            response = "Please enter a sub command";

            foreach (ICommand command in AllCommands)
            {
                if (player.CheckPermission($"ce.{command.Command}"))
                    response += $"<color=yellow><b>- {command.Command} ({string.Join(", ", command.Aliases)})</b></color>\n<color=white>{command.Description}</color>\n\n";
            }
            
            return false;
        }
    }
}