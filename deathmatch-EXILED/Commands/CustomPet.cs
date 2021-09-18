using System;
using System.Collections.Generic;
using System.IO;
using CommandSystem;
using Exiled.API.Features;
using MiniGamesSystem.Pets;
using Newtonsoft.Json;
using RemoteAdmin;

namespace MiniGamesSystem.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class CustomPet : ParentCommand
    {
        public CustomPet() => LoadGeneratedCommands();

        public override string Command { get; } = "custompet";

        public override string[] Aliases { get; } = new string[] { "cpet", "cp" };

        public override string Description { get; } = "CustomPet MiniGames.";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var ply = Player.Get(((PlayerCommandSender)sender).ReferenceHub);
            if (Handler.pInfoDict[ply.UserId].ListaPetow.Contains(PetType.Custom))
            {
                switch (arguments.At(0))
                {
                    case "help":
                        response =
                    "\n=================== CUSTOM PET MENU <color=#EFC01A>(BETA)</color> ===================\n" +
                    "<color=#EFC01A>Dostępne Komendy:</color>\n" +
                    ".custompet nazwa [nazwa peta] - ustawia nazwę peta\n" +
                    ".custompet klasa [id klasy] - zmienia klasę peta\n" +
                    ".custompet item [id itemu] - ustawia jaki item ma trzymać pet\n" +
                    ".custompet rozmiar [x y z] - ustawia rozmiar peta\n" +
                    ".custompet spawn - spawnuje peta\n" +
                    "---------------------------\n" +
                    "<color=#EFC01A>Aktualne Dane Peta:</color>\n" +
                    $"Nazwa:{Handler.pInfoDict[ply.UserId].custompetName}\n" +
                    $"Klasa:{Handler.pInfoDict[ply.UserId].custompetClass}\n" +
                    $"Item: {Handler.pInfoDict[ply.UserId].custompetItem}\n" +
                    $"Rozmiar: {Handler.pInfoDict[ply.UserId].custompetSize}\n";
                        return true;
                    case "nazwa":
                        Handler.pInfoDict[ply.UserId].custompetName = arguments.At(1);
                        foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                        {
                            File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                        }
                        response = $"Zmieniono nazwę CustomPeta na {arguments.At(1)}";
                        return true;
                    case "klasa":
                        var klasa = Convert.ToInt32(arguments.At(1));
                        Handler.pInfoDict[ply.UserId].custompetClass = (RoleType)klasa;
                        foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                        {
                            File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                        }
                        response = $"Zmieniono nazwę klasę CustomPeta na {Handler.pInfoDict[ply.UserId].custompetClass}";
                        return true;
                    case "item":
                        var item = Convert.ToInt32(arguments.At(1));
                        Handler.pInfoDict[ply.UserId].custompetItem = (ItemType)item;
                        foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                        {
                            File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                        }
                        response = $"Zmieniono item CustomPeta na {Handler.pInfoDict[ply.UserId].custompetItem}";
                        return true;
                    case "rozmiar":
                        float x = float.Parse(arguments.At(1));
                        float y = float.Parse(arguments.At(2));
                        float z = float.Parse(arguments.At(3));
                        Handler.pInfoDict[ply.UserId].custompetSize = new UnityEngine.Vector3(x, y, z);
                        foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                        {
                            File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                        }
                        response = $"Zmieniono rozmiar CustomPeta na x: {arguments.At(1)} y: {arguments.At(2)} z: {arguments.At(3)}";
                        return true;
                    case "spawn":
                        if (Pet.SpawnPet(ply, Handler.pInfoDict[ply.UserId].custompetName, PetType.Custom, out var pet))
                        {
                            response = "Zespawnowałeś Custom Peta!";
                            return true;
                        }
                        break;
                }
                response = "Error: coś zjebałeś";
                return false;
            }
            response = "Error: nie masz custom peta w eq.";
            return false;
        }
    }
}
