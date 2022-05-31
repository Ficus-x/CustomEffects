namespace CustomEffects.Commands
{
    using Exiled.Permissions.Extensions;
    using System;
    using CommandSystem;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public sealed class CustomEffectCommand : ParentCommand
    {
        public override string Command => "CustomEffects";
        public override string[] Aliases { get; } = { "ce" };
        public override string Description => "The CustomEffects parent command";

        public CustomEffectCommand() => LoadGeneratedCommands();
        
        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new SubCommands.Give());
            RegisterCommand(new SubCommands.Remove());
            RegisterCommand(new SubCommands.List());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "\nPlease enter a sub command\n\n";

            foreach (ICommand command in AllCommands)
            {
                if (sender.CheckPermission($"ce.{command.Command}"))
                    response += $"<color=yellow><b>- {command.Command} ({string.Join(", ", command.Aliases)})</b></color>\n<color=white>{command.Description}</color>\n\n";
            }
            
            return false;
        }
    }
}