using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;

namespace MiniGamesSystem.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Top : ParentCommand
    {
        public Top() => LoadGeneratedCommands();

        public override string Command { get; } = "topka";

        public override string[] Aliases { get; } = new string[] {"top" };

        public override string Description { get; } = "Top MiniGames.";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var ply = Player.Get(((PlayerCommandSender)sender).ReferenceHub);

            string output;
            int num = 5;
            if (arguments.Count > 0 && int.TryParse(arguments.At(0), out int n)) num = n;
            if (num > 15)
            {
                response = "<color=red>Leaderboards can be no larger than 15.</color>";
                return false;
            }
            if (Handler.pInfoDict.Count != 0)
            {
                output = $"\n========== Top {num} najbogatszych graczy: ==========\n";

                for (int i = 0; i < num; i++)
                {
                    if (Handler.pInfoDict.Count == i) break;
                    string userid = Handler.pInfoDict.ElementAt(i).Key;
                    PlayerInfo info = Handler.pInfoDict[userid];
                    output += $"{i + 1}) <color=#EFC01A>{info.nick}</color> ({userid}) | Coiny: {info.Coins}";
                    if (i != Handler.pInfoDict.Count - 1) output += "\n";
                    else break;
                }
                response = output;
                return true;
            }
            else
            {
                response = "<color=red>Error: Tak.</color>";
                return false;
            }
        }
    }
}
