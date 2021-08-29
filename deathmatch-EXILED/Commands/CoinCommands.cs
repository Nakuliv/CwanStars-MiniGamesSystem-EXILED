using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using RemoteAdmin;
using UnityEngine;

namespace MiniGamesSystem.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class CoinCommands : ParentCommand
    {
        public CoinCommands() => LoadGeneratedCommands();

        public override string Command { get; } = "coin";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Coiny do minigames.";

        public override void LoadGeneratedCommands() { }

        public static Dictionary<string, PlayerInfo> pInfoDict = new Dictionary<string, PlayerInfo>();

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var p = Player.Get(((PlayerCommandSender)sender).ReferenceHub);

            if (!p.CheckPermission("MiniGamesSystem.addcoins"))
            {
                response = "<color=red>Nie masz uprawnień do tej komendy!</color>";
                return false;
            }
            else
            {
                if (arguments.Count == 0 || arguments.Count == 0)
                {
                    response = "<color=red>Musisz wpisać: coin add [id gracza] [ilość] lub coin remove [id gracza] [ilość]</color>";
                    return false;
                }
                else if (arguments.Count == 3)
                {
                    if (arguments.At(0) == "add")
                    {
                        int coiny = int.Parse(arguments.At(2));
                        pInfoDict[Player.Get(arguments.At(1)).UserId].Coins = (pInfoDict[Player.Get(arguments.At(1)).UserId].Coins + coiny);
                        response = $"<color=green>Pomyślnie dodano {coiny} coinów graczowi {Player.Get(arguments.At(1)).Nickname}!</color>";
                        return true;
                    }
                    else if (arguments.At(0) == "remove")
                    {
                        int coiny = int.Parse(arguments.At(2));
                        pInfoDict[Player.Get(arguments.At(1)).UserId].Coins = (pInfoDict[Player.Get(arguments.At(1)).UserId].Coins - coiny);
                        response = $"<color=green>Pomyślnie usunięto {coiny} coinów graczowi {Player.Get(arguments.At(1)).Nickname}!</color>";
                        return true;
                    }
                }
                response = $"";
                return true;
            }
        }
    }
}
