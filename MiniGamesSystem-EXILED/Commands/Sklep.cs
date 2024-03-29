﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using CommandSystem;
using Exiled.API.Features;
using MiniGamesSystem.Pets;
using Newtonsoft.Json;
using RemoteAdmin;

namespace MiniGamesSystem.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Sklep : ParentCommand
    {
        public Sklep() => LoadGeneratedCommands();

        public override string Command { get; } = "Sklep";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Sklep MiniGames.";

        public override void LoadGeneratedCommands()
        {
        }


        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender,
            out string response)
        {
            var ply = Player.Get(((PlayerCommandSender) sender).ReferenceHub);
            if (arguments.Count == 0)
            {
                bool hasData = Handler.pInfoDict.ContainsKey(ply.UserId);
                response =
                    "\n=================== Sklep ===================\n" +
                    $"twoje Coiny: {(hasData ? Handler.pInfoDict[ply.UserId].Coins.ToString() : "[BRAK DANYCH]")}\n" +
                    (MiniGamesSystem.Instance.Config.EnableHats ? "---------------------------\n" : null) +
                    (MiniGamesSystem.Instance.Config.EnableHats ? "<color=#EFC01A>Czapki:</color>\n" : null) +
                    (MiniGamesSystem.Instance.Config.EnableHats ? "Coin - <color=yellow>50</color> coinów\n" : null) +
                    (MiniGamesSystem.Instance.Config.EnableHats ? "Piłka - <color=yellow>100</color> Coinów\n" : null) +
                    (MiniGamesSystem.Instance.Config.EnableHats ? "Cola - <color=yellow>150</color> Coinów\n" : null) +
                    (MiniGamesSystem.Instance.Config.EnableHats ? "Beret - <color=yellow>250</color> Coinów\n" : null) +
                    (MiniGamesSystem.Instance.Config.EnableHats ? "Ser - <color=yellow>1000</color> Coinów\n" : null) +
                    (MiniGamesSystem.Instance.Config.EnablePets ? "---------------------------\n" : null) +
                    (MiniGamesSystem.Instance.Config.EnablePets ? "<color=#EFC01A>Pety (BETA):</color>\n" : null) +
                    (MiniGamesSystem.Instance.Config.EnablePets ? "Amogus - <color=yellow>500</color> coinów\n" : null) +
                    (MiniGamesSystem.Instance.Config.EnablePets ? "Doggo - <color=yellow>1000</color> coinów\n" : null) +
                    (MiniGamesSystem.Instance.Config.EnablePets ? "(<color=#EFC01A>NOWOŚĆ!</color>)Custom - <color=yellow>20000</color> coinów\n" : null) +
                    "---------------------------\n" +
                    "<color=#EFC01A>Rangi:</color>\n" +
                    "VIP na miesiąc - <color=yellow>10000</color> Coinów\n" +
                    "Admin - <color=yellow>999999999</color> Coinów\n" +
                    "---------------------------\n" +
                    "<color=cyan>Aby kupić jakiś item, wpisz:</color> <color=yellow>.sklep kup [nazwa itemu]</color>";
                return true;
            }
            else if (arguments.Count > 0)
            {
                if (arguments.At(0) == "kup")
                {
                    if (MiniGamesSystem.Instance.Config.EnablePets)
                    {
                        if (arguments.At(1) == "Amogus")
                        {
                            if (Handler.pInfoDict[ply.UserId].Coins > 449)
                            {
                                if (Handler.pInfoDict[ply.UserId].ListaPetow.Contains(PetType.Amogus))
                                {
                                    response = "<color=red>Masz już tego peta!</color>";
                                    return false;
                                }
                                else
                                {
                                    Handler.pInfoDict[ply.UserId].Coins = (Handler.pInfoDict[ply.UserId].Coins - 500);
                                    Handler.pInfoDict[ply.UserId].ListaPetow.Add(PetType.Amogus);
                                    foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                                    {
                                        File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"),
                                            JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                    }

                                    response =
                                        "<color=green>Kupiłeś Amogus, wpisz .eq aby zobaczyć listę twoich czapek i petów, lub nałożyć czapkę!</color>";
                                    return true;
                                }
                            }
                            else
                            {
                                response = "<color=red>Nie stać cię na to!</color>";
                                return false;
                            }
                        }
                        else if (arguments.At(1) == "Doggo")
                        {
                            if (Handler.pInfoDict[ply.UserId].Coins > 999)
                            {
                                if (Handler.pInfoDict[ply.UserId].ListaPetow.Contains(PetType.Doggo))
                                {
                                    response = "<color=red>Masz już tego peta!</color>";
                                    return false;
                                }
                                else
                                {
                                    Handler.pInfoDict[ply.UserId].Coins = (Handler.pInfoDict[ply.UserId].Coins - 1000);
                                    Handler.pInfoDict[ply.UserId].ListaPetow.Add(PetType.Doggo);
                                    foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                                    {
                                        File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"),
                                            JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                    }

                                    response =
                                        "<color=green>Kupiłeś Doggo, wpisz .eq aby zobaczyć listę twoich czapek i petów, lub nałożyć czapkę!</color>";
                                    return true;
                                }
                            }
                            else
                            {
                                response = "<color=red>Nie stać cię na to!</color>";
                                return false;
                            }
                        }
                        else if (arguments.At(1) == "Custom")
                        {
                            if (Handler.pInfoDict[ply.UserId].Coins > 19999)
                            {
                                if (Handler.pInfoDict[ply.UserId].ListaPetow.Contains(PetType.Custom))
                                {
                                    response = "<color=red>Masz już tego peta!</color>";
                                    return false;
                                }
                                else
                                {
                                    Handler.pInfoDict[ply.UserId].Coins = (Handler.pInfoDict[ply.UserId].Coins - 20000);
                                    Handler.pInfoDict[ply.UserId].ListaPetow.Add(PetType.Custom);
                                    foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                                    {
                                        File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"),
                                            JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                    }

                                    response =
                                        "<color=green>Kupiłeś Custom Peta, wpisz .custompet aby go skonfigurować!</color>";
                                    return true;
                                }
                            }
                            else
                            {
                                response = "<color=red>Nie stać cię na to!</color>";
                                return false;
                            }
                        }
                    }
                    else if (arguments.At(1) == "Vip")
                    {
                        if (Handler.pInfoDict[ply.UserId].Coins > 9999)
                        {
                            if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Vip"))
                            {
                                response = "<color=red>Masz już tę rangę!</color>";
                                return false;
                            }
                            else
                            {
                                Handler.pInfoDict[ply.UserId].Coins = (Handler.pInfoDict[ply.UserId].Coins - 10000);
                                Handler.pInfoDict[ply.UserId].ListaCzapek.Add("Vip");
                                response = "<color=green>Kupiłeś rangę VIP na miesiąc!</color>";
                                foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                                {
                                    File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"),
                                        JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                }

                                //webhook
                                string token =
                                    "https://canary.discord.com/api/webhooks/875161096959442996/rKIGIZ0L7O7PR76rd-wuGsB9mE6OPC-lCqGiZtVMJXF8ShigAzbOD9wxyZy-RhTiMgBg";
                                WebRequest wr = (HttpWebRequest) WebRequest.Create(token);
                                wr.ContentType = "application/json";
                                wr.Method = "POST";
                                using (var sw = new StreamWriter(wr.GetRequestStream()))
                                {
                                    string json = JsonConvert.SerializeObject(new
                                    {
                                        username = "CwanStars Ban",
                                        embeds = new[]
                                        {
                                            new
                                            {
                                                description =
                                                    $"Gracz {ply.Nickname} (steamid: {ply.UserId}) kupił VIPa na miesiąc za 10000 coinów!",
                                                title = "Nowy VIP!",
                                                image = new
                                                {
                                                    url =
                                                        "https://cdn.discordapp.com/attachments/844546533507727380/875720054921105418/nowyvip2.png"
                                                },
                                                color = 65417
                                            }
                                        }
                                    });

                                    sw.Write(json);
                                }

                                var responsee = (HttpWebResponse) wr.GetResponse();
                            }
                        }
                        else
                        {
                            response = "<color=red>Nie stać cię na to!</color>";
                            return false;
                        }
                    }
                    else if (arguments.At(1) == "Coin")
                    {
                        if (Handler.pInfoDict[ply.UserId].Coins > 49)
                        {
                            if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Coin"))
                            {
                                response = "<color=red>Masz już tę czapkę!</color>";
                                return false;
                            }
                            else
                            {
                                Handler.pInfoDict[ply.UserId].Coins = (Handler.pInfoDict[ply.UserId].Coins - 50);
                                Handler.pInfoDict[ply.UserId].ListaCzapek.Add("Coin");
                                foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                                {
                                    File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"),
                                        JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                }

                                response =
                                    "<color=green>Kupiłeś Coin, wpisz .eq aby zobaczyć listę twoich czapek, lub nałożyć czapkę!</color>";
                                return true;
                            }
                        }
                        else
                        {
                            response = "<color=red>Nie stać cię na to!</color>";
                            return false;
                        }
                    }
                    else if (arguments.At(1) == "Piłka")
                    {
                        if (Handler.pInfoDict[ply.UserId].Coins > 99)
                        {
                            if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Piłka"))
                            {
                                response = "<color=red>Masz już tę czapkę!</color>";
                                return false;
                            }
                            else
                            {
                                Handler.pInfoDict[ply.UserId].Coins = (Handler.pInfoDict[ply.UserId].Coins - 100);
                                Handler.pInfoDict[ply.UserId].ListaCzapek.Add("Piłka");
                                foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                                {
                                    File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"),
                                        JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                }

                                response =
                                    "<color=green>Kupiłeś Piłkę, wpisz .eq aby zobaczyć listę twoich czapek, lub nałożyć czapkę!</color>";
                                return true;
                            }
                        }
                        else
                        {
                            response = "<color=red>Nie stać cię na to!</color>";
                            return false;
                        }
                    }
                    else if (arguments.At(1) == "Cola")
                    {
                        if (Handler.pInfoDict[ply.UserId].Coins > 149)
                        {
                            if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Cola"))
                            {
                                response = "<color=red>Masz już tę czapkę!</color>";
                                return false;
                            }
                            else
                            {
                                Handler.pInfoDict[ply.UserId].Coins = (Handler.pInfoDict[ply.UserId].Coins - 150);
                                Handler.pInfoDict[ply.UserId].ListaCzapek.Add("Cola");
                                foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                                {
                                    File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"),
                                        JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                }

                                response =
                                    "<color=green>Kupiłeś Colę, wpisz .eq aby zobaczyć listę twoich czapek, lub nałożyć czapkę!</color>";
                                return true;
                            }
                        }
                        else
                        {
                            response = "<color=red>Nie stać cię na to!</color>";
                            return false;
                        }
                    }
                    else if (arguments.At(1) == "Beret")
                    {
                        if (Handler.pInfoDict[ply.UserId].Coins > 249)
                        {
                            if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Beret"))
                            {
                                response = "<color=red>Masz już tę czapkę!</color>";
                                return false;
                            }
                            else
                            {
                                Handler.pInfoDict[ply.UserId].Coins = (Handler.pInfoDict[ply.UserId].Coins - 250);
                                Handler.pInfoDict[ply.UserId].ListaCzapek.Add("Beret");
                                foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                                {
                                    File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"),
                                        JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                }

                                response =
                                    "<color=green>Kupiłeś Beret, wpisz .eq aby zobaczyć listę twoich czapek, lub nałożyć czapkę!</color>";
                                return true;
                            }
                        }
                        else
                        {
                            response = "<color=red>Nie stać cię na to!</color>";
                            return false;
                        }
                    }
                    else if (arguments.At(1) == "Ser")
                    {
                        if (Handler.pInfoDict[ply.UserId].Coins > 999)
                        {
                            if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Ser"))
                            {
                                response = "<color=red>Masz już tę czapkę!</color>";
                                return false;
                            }
                            else
                            {
                                Handler.pInfoDict[ply.UserId].Coins = (Handler.pInfoDict[ply.UserId].Coins - 1000);
                                Handler.pInfoDict[ply.UserId].ListaCzapek.Add("Ser");
                                foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                                {
                                    File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"),
                                        JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                }

                                response =
                                    "<color=green>Kupiłeś Ser, wpisz .eq aby zobaczyć listę twoich czapek, lub nałożyć czapkę!</color>";
                                return true;
                            }
                        }
                        else
                        {
                            response = "<color=red>Nie stać cię na to!</color>";
                            return false;
                        }
                    }
                    else
                    {
                        response =
                            "<color=red>Taki item nie istnieje, sprawdź czy wpisałeś nazwę itemu poprawnie!</color>";
                        return false;
                    }
                }
            }

            response = "";
            return true;
        }
    }
}