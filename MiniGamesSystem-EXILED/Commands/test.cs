using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;

namespace MiniGamesSystem.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Test : ParentCommand
    {
        public Test() => LoadGeneratedCommands();

        public override string Command { get; } = "test";

        public override string[] Aliases { get; } = new string[] { "test" };

        public override string Description { get; } = "Test MiniGames.";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            foreach(Player ply in Player.List)
            {
                ply.Role = RoleType.Scp173;
                response = $"test {ply.Role}";
                return true;
            }
            response = $"";
            return true;
        }
    }
}
