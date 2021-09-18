using System;
using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Features;
using MEC;
using MiniGamesSystem.Hats;
using MiniGamesSystem.Pets;
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

        public string listaczapek(Player ply)
        {
            return string.Join("\n", Handler.pInfoDict[ply.UserId].ListaCzapek);
        }

        public string listapetow(Player ply)
        {
            return string.Join("\n", Handler.pInfoDict[ply.UserId].ListaPetow);
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
                bool hasData = Handler.pInfoDict.ContainsKey(ply.UserId);
                response =
                "\n=================== Ekwipunek ===================\n" +
                "<color=#EFC01A>Twoje Czapki:</color>\n" +
                $"{(hasData ? listaczapek(ply) : "[NIE MASZ ŻADNYCH CZAPEK]")}\n" +
                "<color=#EFC01A>Twoje Pety:</color>\n" +
                $"{(hasData ? listapetow(ply) : "[NIE MASZ ŻADNYCH PETÓW]")}\n" +
                "---------------------------\n" +
                "<color=cyan>Aby wziąć jakąś czapkę wpisz: </color><color=yellow>.eq wez [nazwa czapki]</color>\n" +
                "<color=cyan>Aby zdjąć czapkę wpisz: </color><color=yellow>.eq odloz</color>";
                return true;
            }
            else if (arguments.At(0) == "wez")
            {
                if (arguments.At(1) == "Coin")
                {
                    if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Coin"))
                    {
                        if (ply.Role != RoleType.None && ply.Role != RoleType.Spectator)
                        {
                            //ply.SpawnHat(new HatInfo(ItemType.Coin, new UnityEngine.Vector3(1, 1, 1), ply.Position));
                            Hats.Hats.SpawnHat(ply, new HatInfo(ItemType.Coin));
                            response = "<color=green>Założyłeś czapkę!</color>";
                            return true;
                        }
                    }
                    else
                    {
                        response = "<color=red>Nie masz takiej czapki w ekwipunku!</color>";
                        return false;
                    }
                }
                else if (arguments.At(1) == "Piłka")
                {
                    if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Piłka"))
                    {
                        if (ply.Role != RoleType.None && ply.Role != RoleType.Spectator)
                        {
                            ply.SpawnHat(new HatInfo(ItemType.SCP018));
                            response = "<color=green>Założyłeś czapkę!</color>";
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
                    if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Cola"))
                    {
                        if (ply.Role != RoleType.None && ply.Role != RoleType.Spectator)
                        {
                            ply.SpawnHat(new HatInfo(ItemType.SCP207));
                            response = "<color=green>Założyłeś czapkę!</color>";
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
                    if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Beret"))
                    {
                        if (ply.Role != RoleType.None && ply.Role != RoleType.Spectator)
                        {
                            ply.SpawnHat(new HatInfo(ItemType.SCP268));
                            response = "<color=green>Założyłeś czapkę!</color>";
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
                    if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Ser"))
                    {
                        if (ply.Role != RoleType.None && ply.Role != RoleType.Spectator)
                        {
                            ply.SpawnHat(new HatInfo(ItemType.KeycardScientist, new UnityEngine.Vector3(2,3,2)));
                            response = "<color=green>Założyłeś czapkę!</color>";
                            return true;
                        }
                    }
                    else
                    {
                        response = "<color=red>Nie masz takiej czapki w ekwipunku!</color>";
                        return false;
                    }
                }
                else if (arguments.At(1) == "Amogus")
                {
                    if (Handler.pInfoDict[ply.UserId].ListaPetow.Contains(PetType.Amogus))
                    {
                        if (ply.Role != RoleType.None && ply.Role != RoleType.Spectator)
                        {
                            if(Pet.SpawnPet(ply, arguments.At(2), PetType.Amogus, out var pet))
                            {
                                response = "<color=green>Zrespiono Peta!</color>";
                                return true;
                            }
                        }
                    }
                    else
                    {
                        response = "<color=red>Nie masz takiego peta w ekwipunku!</color>";
                        return false;
                    }
                }
                else if (arguments.At(1) == "Doggo")
                {
                    if (Handler.pInfoDict[ply.UserId].ListaPetow.Contains(PetType.Doggo))
                    {
                        if (ply.Role != RoleType.None && ply.Role != RoleType.Spectator)
                        {
                            if (Pet.SpawnPet(ply, arguments.At(2), PetType.Doggo, out var pet))
                            {
                                response = "<color=green>Zrespiono Peta!</color>";
                                return true;
                            }
                        }
                    }
                    else
                    {
                        response = "<color=red>Nie masz takiego peta w ekwipunku!</color>";
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
