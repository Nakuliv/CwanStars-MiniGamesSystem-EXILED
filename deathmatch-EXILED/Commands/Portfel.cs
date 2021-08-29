﻿using System;
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
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Portfel : ParentCommand
    {
        public Portfel() => LoadGeneratedCommands();

        public override string Command { get; } = "portfel";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Portfel MiniGames.";

        public override void LoadGeneratedCommands() { }

        public static Dictionary<string, PlayerInfo> pInfoDict = new Dictionary<string, PlayerInfo>();

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
                    var ply = Player.Get(((PlayerCommandSender)sender).ReferenceHub);

                    Player player = arguments.Count == 0 ? ply : Player.Get(arguments.At(0));
                    string nick;
                    bool hasData = pInfoDict.ContainsKey(player.UserId);
                    if (player != null) nick = player.Nickname;
                    else nick = hasData ? pInfoDict[ply.UserId].Coins.ToString() : "[BRAK DANYCH]";
                    response =
                        "\n=================== Portfel ===================\n" +
                        $"Gracz: {nick} ({player.UserId})\n" +
                        $"Coiny: {(hasData ? pInfoDict[player.UserId].Coins.ToString() : "[BRAK DANYCH]")}\n";
                    return true;
        }
    }
}