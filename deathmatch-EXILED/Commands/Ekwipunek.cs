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
    public class Ekwipunek : ParentCommand
    {
        public Ekwipunek() => LoadGeneratedCommands();

        public override string Command { get; } = "ekwipunek";

        public override string[] Aliases { get; } = new string[] { "eq" };

        public override string Description { get; } = "Ekwipunek MiniGames.";

        public override void LoadGeneratedCommands() { }

        public static Dictionary<string, PlayerInfo> pInfoDict = new Dictionary<string, PlayerInfo>();

        public string listaczapek(Player ply)
        {
            return string.Join("\n", pInfoDict[ply.UserId].ListaCzapek);
        }

        internal static bool RemoveHat(HatPlayerComponent playerComponent)
        {
            if (playerComponent.item == null) return false;

            UnityEngine.Object.Destroy(playerComponent.item.gameObject);
            playerComponent.item = null;
            return true;
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var ply = Player.Get(((PlayerCommandSender)sender).ReferenceHub);
            if (arguments.Count == 0)
            {
                bool hasData = pInfoDict.ContainsKey(ply.UserId);
                response =
                "\n=================== Ekwipunek ===================\n" +
                "<color=#EFC01A>Twoje Czapki:</color>\n" +
                $"{(hasData ? listaczapek(ply) : "[NIE MASZ ŻADNYCH CZAPEK]")}\n" +
                "---------------------------\n" +
                "<color=cyan>Aby wziąć jakąś czapkę wpisz: </color><color=yellow>.eq wez [nazwa czapki]</color>\n" +
                "<color=cyan>Aby zdjąć czapkę wpisz: </color><color=yellow>.eq odloz</color>";
                return true;
            }
            else if (arguments.At(0) == "wez")
            {
                if (arguments.At(1) == "Coin")
                {
                    if (pInfoDict[ply.UserId].ListaCzapek.Contains("Coin"))
                    {
                        if (ply.Role != RoleType.None && ply.Role != RoleType.Spectator)
                        {
                            ply.SpawnHat(ItemType.Coin);
                            response = "<color=green>Założyłeś czapkę!<color>";
                            return true;
                        }
                    }
                    else
                    {
                        response = "<color=red>Nie masz takiej czapki w ekwipunku!<color>";
                        return false;
                    }
                }
                else if (arguments.At(1) == "Piłka")
                {
                    if (pInfoDict[ply.UserId].ListaCzapek.Contains("Piłka"))
                    {
                        if (ply.Role != RoleType.None && ply.Role != RoleType.Spectator)
                        {
                            ply.SpawnHat(ItemType.SCP018);
                            response = "<color=green>Założyłeś czapkę!<color>";
                            return true;
                        }
                    }
                    else
                    {
                        response = "<color=red>Nie masz takiej czapki w ekwipunku!</color>";
                        return false;
                    }
                }
                else if (arguments.At(1) == "Cola")
                {
                    if (pInfoDict[ply.UserId].ListaCzapek.Contains("Cola"))
                    {
                        if (ply.Role != RoleType.None && ply.Role != RoleType.Spectator)
                        {
                            ply.SpawnHat(ItemType.SCP207);
                            response = "<color=green>Założyłeś czapkę!<color>";
                            return true;
                        }
                    }
                    else
                    {
                        response = "<color=red>Nie masz takiej czapki w ekwipunku!</color>";
                        return false;
                    }
                }
                else if (arguments.At(1) == "Beret")
                {
                    if (pInfoDict[ply.UserId].ListaCzapek.Contains("Beret"))
                    {
                        if (ply.Role != RoleType.None && ply.Role != RoleType.Spectator)
                        {
                            ply.SpawnHat(ItemType.SCP268);
                            response = "<color=green>Założyłeś czapkę!<color>";
                            return true;
                        }
                    }
                    else
                    {
                        response = "<color=red>Nie masz takiej czapki w ekwipunku!</color>";
                        return false;
                    }
                }
                else if (arguments.At(1) == "Ser")
                {
                    if (pInfoDict[ply.UserId].ListaCzapek.Contains("Ser"))
                    {
                        if (ply.Role != RoleType.None && ply.Role != RoleType.Spectator)
                        {
                            ply.SpawnHat(ItemType.KeycardScientist);
                            response = "<color=green>Założyłeś czapkę!<color>";
                            return true;
                        }
                    }
                    else
                    {
                        response = "<color=red>Nie masz takiej czapki w ekwipunku!</color>";
                        return false;
                    }
                }
                else
                {
                    response = "<color=red>Nie masz takiej czapki!</color>";
                    return false;
                }
            }
            else if (arguments.At(0) == "odloz")
            {
                HatPlayerComponent playerComponent;
                if (!ply.GameObject.TryGetComponent(out playerComponent))
                {
                    playerComponent = ply.GameObject.AddComponent<HatPlayerComponent>();
                }
                RemoveHat(playerComponent);
                response = "<color=green>Odłożyłeś czapkę!</color>";
                return true;
            }
            response = "";
            return true;
        }
    }
}