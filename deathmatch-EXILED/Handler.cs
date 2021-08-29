using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Interactables.Interobjects.DoorUtils;
using MEC;
using EMap = Exiled.API.Features.Map;
using UnityEngine;
using Mirror;
using RemoteAdmin;
using YamlDotNet.Serialization;
using System.IO;
using Newtonsoft.Json;
using Respawning;
using Respawning.NamingRules;
using Exiled.Permissions.Extensions;
using System.Net;
using Exiled.API.Features.Items;
using MiniGamesSystem.Hats;

namespace MiniGamesSystem
{
    public class Handler
    {
        public static string EventMsg = "[<color=blue>Tryb</color>]";
        public MiniGamesSystem.ObjectType ObjectType { get; set; } = MiniGamesSystem.ObjectType.WorkStation;
        static System.Random rnd = new System.Random();
        StringBuilder message = new StringBuilder();
        public List<Vector3> PossibleSpawnsPos = new List<Vector3>();
        Vector3 ChoosedSpawnPos;
        internal static Dictionary<string, Tuple<HatInfo, HatInfo>> HatPlayers = new Dictionary<string, Tuple<HatInfo, HatInfo>>();
        public List<CoroutineHandle> coroutines = new List<CoroutineHandle>();
        public static Dictionary<string, PlayerInfo> pInfoDict = new Dictionary<string, PlayerInfo>();
        private List<DoorType> lockedCheckpointLcz = new List<DoorType>() { DoorType.CheckpointLczA, DoorType.CheckpointLczB };
        private List<DoorType> EscapePrimary = new List<DoorType>() { DoorType.EscapePrimary };
        private List<DoorType> SurfaceGate = new List<DoorType>() { DoorType.SurfaceGate };
        private List<DoorType> GangWarDoors = new List<DoorType>() { DoorType.CheckpointEntrance, DoorType.GateA, DoorType.GateB };
        public static HashSet<string> props = new HashSet<string>();
        [YamlIgnore]
        public GameObject orginalPrefab;

        public static int Deathmatch = 0;
        public static int GangWar = 0;
        public static int hideAndSeek = 0;
        public static int peanutRun = 0;
        public static int dgball = 0;

        public static string AktualnyEvent = "";

        /*public void OnRAcmd(SendingRemoteAdminCommandEventArgs ev)
        {
            if (ev.Name.ToLower() == "coin")
            {
                ev.IsAllowed = false;
                if (!ev.Sender.CheckPermission("MiniGamesSystem.addcoins"))
                {
                    ev.ReplyMessage = "<color=red>Bruh (Brak uprawnień).</color>";
                    return;
                }
                else
                {
                    if (ev.Arguments.Count == 0 || ev.Arguments.Count == 0)
                    {
                        ev.ReplyMessage = "<color=red>Musisz wpisać: coin add [id gracza] [ilość] lub coin remove [id gracza] [ilość]</color>";
                    }
                    else if (ev.Arguments.Count == 3)
                    {
                        if (ev.Arguments[0] == "add")
                        {
                            int coiny = int.Parse(ev.Arguments[2]);
                            pInfoDict[Player.Get(ev.Arguments[1]).UserId].Coins = (pInfoDict[Player.Get(ev.Arguments[1]).UserId].Coins + coiny);
                            ev.ReplyMessage = $"<color=green>Pomyślnie dodano {coiny} coinów graczowi {Player.Get(ev.Arguments[1]).Nickname}!</color>";
                        }
                        else if (ev.Arguments[0] == "remove")
                        {
                            int coiny = int.Parse(ev.Arguments[2]);
                            pInfoDict[Player.Get(ev.Arguments[1]).UserId].Coins = (pInfoDict[Player.Get(ev.Arguments[1]).UserId].Coins - coiny);
                            ev.ReplyMessage = $"<color=green>Pomyślnie usunięto {coiny} coinów graczowi {Player.Get(ev.Arguments[1]).Nickname}!</color>";
                        }
                    }
                }
                
                if (ev.Name.ToLower() == "spawntests")
                {
                    ev.IsAllowed = false;

                    if (ev.Arguments.Count == 0)
                    {
                        if (!Round.IsStarted)
                        {
                            ev.ReplyMessage = "<color=red>nie możesz tego zrobić przed startem rundy!</color>";
                        }
                        else if (Round.IsStarted)
                        {
                            {
                                UnityEngine.Vector3 location3 = Map.Rooms.First(x => x.Type == RoomType.Surface).Position;
                                location3.y = location3.y + 1;
                                orginalPrefab = UnityEngine.Object.Instantiate(MiniGamesSystem.GetWorkStationObject(ObjectType));
                                orginalPrefab.transform.position = new Vector3(location3.x, location3.y, location3.z);
                                orginalPrefab.transform.localScale = new Vector3(2, 2, 2);
                                orginalPrefab.transform.rotation = Quaternion.Euler(new Vector3(1, 1, 1));
                                Pickup Item = Exiled.API.Extensions.Item.Spawn(ItemType.WeaponManagerTablet, 0, location3);
                                GameObject gameObject = Item.gameObject;
                                gameObject.transform.localScale = new Vector3(15, 5, 25);
                                gameObject.transform.rotation = new UnityEngine.Quaternion(3, 1, 1, 1);
                                NetworkServer.UnSpawn(gameObject);
                                NetworkServer.Spawn(Item.gameObject);
                                ev.ReplyMessage = "<color=green>OK</color>";

                                //shrek dummy

                                GameObject dummy = GameObject.Instantiate(NetworkManager.singleton.spawnPrefabs.FirstOrDefault(p => p.gameObject.name == "Player"));
                                CharacterClassManager dummyClassManager = dummy.GetComponent<CharacterClassManager>();
                                dummyClassManager.CurClass = RoleType.ClassD;
                                NicknameSync nicknameSync = dummy.GetComponent<NicknameSync>();
                                nicknameSync.Network_myNickSync = "Shrek";
                                dummy.transform.localScale = new Vector3(2, 1, 2);
                                nicknameSync.Network_customPlayerInfoString = "<color=green>Shrek</color>";
                                dummy.GetComponent<QueryProcessor>().PlayerId = 9999;
                                dummy.GetComponent<QueryProcessor>().NetworkPlayerId = 9999;
                                dummy.transform.localScale = new Vector3(2, 1, 2);
                                var test = dummy.GetComponent<AnimationController>();
                                test.Network_curMoveState = (byte)PlayerMovementState.Sprinting;
                                dummy.transform.position = new Vector3(dummy.transform.position.x, dummy.transform.position.y, dummy.transform.position.z + 1);
                                var position = ev.Sender.Position;
                                dummy.transform.position = position;
                                test.Networkspeed = new Vector2(10, 0);
                                NetworkServer.Spawn(dummy);
                            }
                        };
                    }
                }
            }
        }*/
        internal static bool RemoveHat(HatPlayerComponent playerComponent)
        {
            if (playerComponent.item == null) return false;

            UnityEngine.Object.Destroy(playerComponent.item.gameObject);
            playerComponent.item = null;
            return true;
        }
        public string listaczapek(Player ply)
        {
            return string.Join("\n", pInfoDict[ply.UserId].ListaCzapek);
        }

        //voting system
        /*public void OnConsoleCMD(SendingConsoleCommandEventArgs ev)
        {
            string cmd = ev.Name.ToLower();
            if (cmd == "portfel")
            {
                ev.IsAllowed = false;
                Player player = ev.Arguments.Count == 0 ? ev.Player : Player.Get(ev.Arguments[0]);
                string nick;
                bool hasData = pInfoDict.ContainsKey(player.UserId);
                if (player != null) nick = player.Nickname;
                else nick = hasData ? pInfoDict[ev.Player.UserId].Coins.ToString() : "[BRAK DANYCH]";
                ev.ReturnMessage =
                    "\n=================== Portfel ===================\n" +
                    $"Gracz: {nick} ({player.UserId})\n" +
                    $"Coiny: {(hasData ? pInfoDict[player.UserId].Coins.ToString() : "[BRAK DANYCH]")}\n";
            }
            else if (cmd == "sklep")
            {
                if (ev.Arguments.Count == 0)
                {
                    ev.IsAllowed = false;
                    bool hasData = pInfoDict.ContainsKey(ev.Player.UserId);
                    ev.ReturnMessage =
                        "\n=================== Sklep ===================\n" +
                        $"twoje Coiny: {(hasData ? pInfoDict[ev.Player.UserId].Coins.ToString() : "[BRAK DANYCH]")}\n" +
                        "---------------------------\n" +
                        "<color=#EFC01A>Czapki:</color>\n" +
                        "Coin - 50 coinów\n" +
                        "Piłka - 100 Coinów\n" +
                        "Cola - 150 Coinów\n" +
                        "Beret - 250 Coinów\n" +
                        "Ser - 1000 Coinów\n" +
                        "---------------------------\n" +
                        "<color=#EFC01A>Pety (BETA):</color>\n" +
                        "Amogus - 450 coinów\n" +
                        "---------------------------\n" +
                        "<color=#EFC01A>Rangi:</color>\n" +
                        "VIP na miesiąc - 10000 Coinów\n" +
                        "Admin - 999999999 Coinów\n" +
                        "---------------------------\n" +
                        "<color=cyan>Aby kupić jakiś item, wpisz:</color> <color=yellow>.sklep kup [nazwa itemu]</color>";
                }
                else if (ev.Arguments.Count > 0)
                {
                    ev.IsAllowed = false;
                    if (ev.Arguments[0] == "kup")
                    {
                        if (ev.Arguments[1] == "Amogus")
                        {
                            if (pInfoDict[ev.Player.UserId].Coins > 449)
                            {

                                if (pInfoDict[ev.Player.UserId].ListaCzapek.Contains("Amogus"))
                                {
                                    ev.ReturnMessage = "<color=red>Masz już tego peta!</color>";
                                }
                                else
                                {
                                    pInfoDict[ev.Player.UserId].Coins = (pInfoDict[ev.Player.UserId].Coins - 450);
                                    pInfoDict[ev.Player.UserId].ListaCzapek.Add("Amogus");
                                    ev.ReturnMessage = "<color=green>Kupiłeś Amogus, wpisz .eq aby zobaczyć listę twoich czapek i petów, lub nałożyć czapkę!</color>";
                                    foreach (KeyValuePair<string, PlayerInfo> info in pInfoDict)
                                    {
                                        File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                    }
                                }
                            }
                            else
                            {
                                ev.ReturnMessage = "<color=red>Nie stać cię na to!</color>";
                            }
                        }
                        else if (ev.Arguments[1] == "Vip")
                        {
                            if (pInfoDict[ev.Player.UserId].Coins > 9999)
                            {

                                if (pInfoDict[ev.Player.UserId].ListaCzapek.Contains("Vip"))
                                {
                                    ev.ReturnMessage = "<color=red>Masz już tę rangę!</color>";
                                }
                                else
                                {d
                                    pInfoDict[ev.Player.UserId].Coins = (pInfoDict[ev.Player.UserId].Coins - 10000);
                                    pInfoDict[ev.Player.UserId].ListaCzapek.Add("Vip");
                                    ev.ReturnMessage = "<color=green>Kupiłeś rangę VIP na miesiąc!</color>";
                                    foreach (KeyValuePair<string, PlayerInfo> info in pInfoDict)
                                    {
                                        File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                    }

                                    //webhook
                                    string token = "https://canary.discord.com/api/webhooks/875161096959442996/rKIGIZ0L7O7PR76rd-wuGsB9mE6OPC-lCqGiZtVMJXF8ShigAzbOD9wxyZy-RhTiMgBg";
                                    WebRequest wr = (HttpWebRequest)WebRequest.Create(token);
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
                            description = $"Gracz {ev.Player.Nickname} (steamid: {ev.Player.UserId}) kupił VIPa na miesiąc za 10000 coinów!",
                            title = "Nowy VIP!",
                                                              image = new {
                    url = "https://cdn.discordapp.com/attachments/844546533507727380/875720054921105418/nowyvip2.png"
      },
                            color = 65417
            }
                    }
                                        });

                                        sw.Write(json);
                                    }

                                    var response = (HttpWebResponse)wr.GetResponse();
                                }
                            }
                            else
                            {
                                ev.ReturnMessage = "<color=red>Nie stać cię na to!</color>";
                            }
                        }
                        else if (ev.Arguments[1] == "Coin")
                        {
                            if (pInfoDict[ev.Player.UserId].Coins > 49)
                            {
                                if (pInfoDict[ev.Player.UserId].ListaCzapek.Contains("Coin"))
                                {
                                    ev.ReturnMessage = "<color=red>Masz już tę czapkę!</color>";
                                }
                                else
                                {
                                    pInfoDict[ev.Player.UserId].Coins = (pInfoDict[ev.Player.UserId].Coins - 50);
                                    pInfoDict[ev.Player.UserId].ListaCzapek.Add("Coin");
                                    ev.ReturnMessage = "<color=green>Kupiłeś Coin, wpisz .eq aby zobaczyć listę twoich czapek, lub nałożyć czapkę!</color>";
                                    foreach (KeyValuePair<string, PlayerInfo> info in pInfoDict)
                                    {
                                        File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                    }
                                }
                            }
                            else
                            {
                                ev.ReturnMessage = "<color=red>Nie stać cię na to!</color>";
                            }
                        }
                        else if (ev.Arguments[1] == "Piłka")
                        {
                            if (pInfoDict[ev.Player.UserId].Coins > 99)
                            {
                                if (pInfoDict[ev.Player.UserId].ListaCzapek.Contains("Piłka"))
                                {
                                    ev.ReturnMessage = "<color=red>Masz już tę czapkę!</color>";
                                }
                                else
                                {
                                    pInfoDict[ev.Player.UserId].Coins = (pInfoDict[ev.Player.UserId].Coins - 100);
                                    pInfoDict[ev.Player.UserId].ListaCzapek.Add("Piłka");
                                    ev.ReturnMessage = "<color=green>Kupiłeś Piłkę, wpisz .eq aby zobaczyć listę twoich czapek, lub nałożyć czapkę!</color>";
                                    foreach (KeyValuePair<string, PlayerInfo> info in pInfoDict)
                                    {
                                        File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                    }
                                }
                            }
                            else
                            {
                                ev.ReturnMessage = "<color=red>Nie stać cię na to!</color>";
                            }
                        }
                        else if (ev.Arguments[1] == "Cola")
                        {
                            if (pInfoDict[ev.Player.UserId].Coins > 149)
                            {
                                if (pInfoDict[ev.Player.UserId].ListaCzapek.Contains("Cola"))
                                {
                                    ev.ReturnMessage = "<color=red>Masz już tę czapkę!</color>";
                                }
                                else
                                {
                                    pInfoDict[ev.Player.UserId].Coins = (pInfoDict[ev.Player.UserId].Coins - 150);
                                    pInfoDict[ev.Player.UserId].ListaCzapek.Add("Cola");
                                    ev.ReturnMessage = "<color=green>Kupiłeś Colę, wpisz .eq aby zobaczyć listę twoich czapek, lub nałożyć czapkę!</color>";
                                    foreach (KeyValuePair<string, PlayerInfo> info in pInfoDict)
                                    {
                                        File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                    }
                                }
                            }
                            else
                            {
                                ev.ReturnMessage = "<color=red>Nie stać cię na to!</color>";
                            }
                        }
                        else if (ev.Arguments[1] == "Beret")
                        {
                            if (pInfoDict[ev.Player.UserId].Coins > 249)
                            {
                                if (pInfoDict[ev.Player.UserId].ListaCzapek.Contains("Beret"))
                                {
                                    ev.ReturnMessage = "<color=red>Masz już tę czapkę!</color>";
                                }
                                else
                                {
                                    pInfoDict[ev.Player.UserId].Coins = (pInfoDict[ev.Player.UserId].Coins - 250);
                                    pInfoDict[ev.Player.UserId].ListaCzapek.Add("Beret");
                                    ev.ReturnMessage = "<color=green>Kupiłeś Beret, wpisz .eq aby zobaczyć listę twoich czapek, lub nałożyć czapkę!</color>";
                                    foreach (KeyValuePair<string, PlayerInfo> info in pInfoDict)
                                    {
                                        File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                    }
                                }
                            }
                            else
                            {
                                ev.ReturnMessage = "<color=red>Nie stać cię na to!</color>";
                            }
                        }
                        else if (ev.Arguments[1] == "Ser")
                        {
                            if (pInfoDict[ev.Player.UserId].Coins > 999)
                            {
                                if (pInfoDict[ev.Player.UserId].ListaCzapek.Contains("Ser"))
                                {
                                    ev.ReturnMessage = "<color=red>Masz już tę czapkę!</color>";
                                }
                                else
                                {
                                    pInfoDict[ev.Player.UserId].Coins = (pInfoDict[ev.Player.UserId].Coins - 1000);
                                    pInfoDict[ev.Player.UserId].ListaCzapek.Add("Ser");
                                    ev.ReturnMessage = "<color=green>Kupiłeś Ser, wpisz .eq aby zobaczyć listę twoich czapek, lub nałożyć czapkę!</color>";
                                    foreach (KeyValuePair<string, PlayerInfo> info in pInfoDict)
                                    {
                                        File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                    }
                                }
                            }
                            else
                            {
                                ev.ReturnMessage = "<color=red>Nie stać cię na to!</color>";
                            }
                        }
                        else
                        {
                            ev.ReturnMessage = "<color=red>Taki item nie istnieje, sprawdź czy wpisałeś nazwę itemu poprawnie!</color>";
                        }
                    }
                }

            }
            else if (cmd == "ekwipunek" || cmd == "eq")
            {
                if (ev.Arguments.Count == 0)
                {
                    ev.IsAllowed = false;
                    bool hasData = pInfoDict.ContainsKey(ev.Player.UserId);
                    ev.ReturnMessage =
                    "\n=================== Ekwipunek ===================\n" +
                    "<color=#EFC01A>Twoje Czapki:</color>\n" +
                    $"{(hasData ? listaczapek(ev.Player) : "[NIE MASZ ŻADNYCH CZAPEK]")}\n" +
                    "---------------------------\n" +
                    "<color=cyan>Aby wziąć jakąś czapkę wpisz: </color><color=yellow>.eq wez [nazwa czapki]</color>\n" +
                    "<color=cyan>Aby zdjąć czapkę wpisz: </color><color=yellow>.eq odloz</color>";
                }
                else if (ev.Arguments[0] == "wez")
                {
                    ev.IsAllowed = false;
                    if (ev.Arguments[1] == "Coin")
                    {
                        if (pInfoDict[ev.Player.UserId].ListaCzapek.Contains("Coin"))
                        {
                            if (ev.Player.Role != RoleType.None && ev.Player.Role != RoleType.Spectator) ev.Player.SpawnHat(ItemType.Coin);
                        }
                        else
                        {
                            ev.ReturnMessage = "<color=red>Nie masz takiej czapki w ekwipunku!<color>";
                        }
                    }
                    else if (ev.Arguments[1] == "Piłka")
                    {
                        if (pInfoDict[ev.Player.UserId].ListaCzapek.Contains("Piłka"))
                        {
                            if (ev.Player.Role != RoleType.None && ev.Player.Role != RoleType.Spectator) ev.Player.SpawnHat(ItemType.SCP018);
                        }
                        else
                        {
                            ev.ReturnMessage = "<color=red>Nie masz takiej czapki w ekwipunku!</color>";
                        }
                    }
                    else if (ev.Arguments[1] == "Cola")
                    {
                        if (pInfoDict[ev.Player.UserId].ListaCzapek.Contains("Cola"))
                        {
                            if (ev.Player.Role != RoleType.None && ev.Player.Role != RoleType.Spectator)
                            {
                                ev.Player.SpawnHat(ItemType.SCP207);
                            }
                        }
                        else
                        {
                            ev.ReturnMessage = "<color=red>Nie masz takiej czapki w ekwipunku!</color>";
                        }
                    }
                    else if (ev.Arguments[1] == "Beret")
                    {
                        if (pInfoDict[ev.Player.UserId].ListaCzapek.Contains("Beret"))
                        {
                            if (ev.Player.Role != RoleType.None && ev.Player.Role != RoleType.Spectator) ev.Player.SpawnHat(ItemType.SCP268);
                        }
                        else
                        {
                            ev.ReturnMessage = "<color=red>Nie masz takiej czapki w ekwipunku!</color>";
                        }
                    }
                    else if (ev.Arguments[1] == "Ser")
                    {
                        if (pInfoDict[ev.Player.UserId].ListaCzapek.Contains("Ser"))
                        {
                            if (ev.Player.Role != RoleType.None && ev.Player.Role != RoleType.Spectator) ev.Player.SpawnHat(ItemType.KeycardScientist);
                        }
                        else
                        {
                            ev.ReturnMessage = "<color=red>Nie masz takiej czapki w ekwipunku!</color>";
                        }
                    }
                    else
                    {
                        ev.ReturnMessage = "<color=red>Nie masz takiej czapki!</color>";
                    }
                }
                else if (ev.Arguments[0] == "odloz")
                {
                    ev.IsAllowed = false;
                    HatPlayerComponent playerComponent;
                    if (!ev.Player.GameObject.TryGetComponent(out playerComponent))
                    {
                        playerComponent = ev.Player.GameObject.AddComponent<HatPlayerComponent>();
                    }
                    RemoveHat(playerComponent);
                    ev.ReturnMessage = "<color=green>Odłożyłeś czapkę!</color>";
                }
            }
            else if (cmd == "topka" || cmd == "top")
            {
                ev.IsAllowed = false;
                string output;
                int num = 5;
                if (ev.Arguments.Count > 0 && int.TryParse(ev.Arguments[0], out int n)) num = n;
                if (num > 15)
                {
                    ev.Color = "red";
                    ev.ReturnMessage = "Leaderboards can be no larger than 15.";
                    return;
                }
                if (pInfoDict.Count != 0)
                {
                    output = $"\n========== Top {num} najbogatszych graczy: ==========\n";

                    for (int i = 0; i < num; i++)
                    {
                        if (pInfoDict.Count == i) break;
                        string userid = pInfoDict.ElementAt(i).Key;
                        PlayerInfo info = pInfoDict[userid];
                        output += $"{i + 1}) <color=#EFC01A>{info.nick}</color> ({userid}) | Coiny: {info.Coins}";
                        if (i != pInfoDict.Count - 1) output += "\n";
                        else break;
                    }
                    ev.ReturnMessage = output;
                }
                else
                {
                    ev.Color = "red";
                    ev.ReturnMessage = "Error: Tak.";
                }
            }

            switch (ev.Name.ToLower())
            {
                /*case "prophunt":
                    ev.IsAllowed = false;
                    MiniGames.PropHunt();
                    ev.ReturnMessage = "<color=green>PropHunt Został włączony!</color>";
                    break;
                case "prophunt add":
                    ev.IsAllowed = false;
                    Player Ply = Player.Get(ev.Arguments[0]);
                    props.Add(Ply.UserId);
                    ev.ReturnMessage = $"<color=green>Gracz {Ply.Nickname} został zmieniony w propa!</color>";
                    break;

                case "prophunt remove":
                    ev.IsAllowed = false;
                    Player Plys = Player.Get(ev.Arguments[0]);
                    props.Remove(Plys.UserId);
                    ev.ReturnMessage = $"<color=green>Gracz {Plys.Nickname} został usunięty z propów!</color>";
                    break;

                case "vote":
                    ev.IsAllowed = false;
                    if (ev.Arguments.Count == 0)
                    {
                        if (!Round.IsStarted)
                        {
                            ev.ReturnMessage = "<color=red>musisz wpisać numer eventu!</color>";
                        }
                        else if (Round.IsStarted)
                        {
                            ev.ReturnMessage = "<color=red>Nie możesz głosować w trakcie rundy!</color>";
                        }
                        return;
                    }
                    else if (ev.Arguments.Count != 0)
                    {
                        switch (ev.Arguments[0].ToLower())
                        {
                            case "1":
                                if (vote.Contains($"HAS{ev.Player.UserId}"))
                                {
                                    hideAndSeek--;
                                    Deathmatch++;
                                    vote.Remove($"HAS{ev.Player.UserId}");
                                    vote.Add($"DM{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zmieniono głos na DeathMatch!";
                                }
                                else if (vote.Contains($"GW{ev.Player.UserId}"))
                                {
                                    GangWar--;
                                    Deathmatch++;
                                    vote.Remove($"GW{ev.Player.UserId}");
                                    vote.Add($"DM{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zmieniono głos na DeathMatch!";
                                }
                                else if (vote.Contains($"DG{ev.Player.UserId}"))
                                {
                                    dgball--;
                                    Deathmatch++;
                                    vote.Remove($"DG{ev.Player.UserId}");
                                    vote.Add($"DM{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zmieniono głos na DeathMatch!";
                                }
                                else if (vote.Contains($"PE{ev.Player.UserId}"))
                                {
                                    peanutRun--;
                                    Deathmatch++;
                                    vote.Remove($"PE{ev.Player.UserId}");
                                    vote.Add($"DM{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zmieniono głos na DeathMatch!";
                                }
                                else if (!Round.IsStarted && !vote.Contains($"DM{ev.Player.UserId}"))
                                {
                                    Deathmatch++;
                                    vote.Add($"DM{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zagłosowano na DeathMatch!";
                                }
                                else if (vote.Contains($"DM{ev.Player.UserId}"))
                                {
                                    ev.ReturnMessage = "<color=red>Możesz zagłosować tylko raz!</color>";
                                }

                                else if (Round.IsStarted)
                                {
                                    ev.ReturnMessage = "<color=red>Nie możesz głosować w trakcie rundy!</color>";
                                }

                                break;
                            case "2":
                                if (vote.Contains($"DM{ev.Player.UserId}"))
                                {
                                    Deathmatch--;
                                    GangWar++;
                                    vote.Remove($"DM{ev.Player.UserId}");
                                    vote.Add($"GW{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zmieniono głos na GangWar!";
                                }
                                else if (vote.Contains($"PE{ev.Player.UserId}"))
                                {
                                    peanutRun--;
                                    GangWar++;
                                    vote.Remove($"PE{ev.Player.UserId}");
                                    vote.Add($"GW{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zmieniono głos na GangWar!";
                                }
                                else if (vote.Contains($"DG{ev.Player.UserId}"))
                                {
                                    dgball--;
                                    GangWar++;
                                    vote.Remove($"DG{ev.Player.UserId}");
                                    vote.Add($"GW{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zmieniono głos na GangWar!";
                                }
                                else if (vote.Contains($"HAS{ev.Player.UserId}"))
                                {
                                    hideAndSeek--;
                                    GangWar++;
                                    vote.Remove($"HAS{ev.Player.UserId}");
                                    vote.Add($"GW{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zmieniono głos na GangWar!";
                                }
                                else if (!Round.IsStarted && !vote.Contains($"GW{ev.Player.UserId}"))
                                {
                                    GangWar++;
                                    vote.Add($"GW{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zagłosowano na GangWar!";
                                }
                                else if (vote.Contains($"GW{ev.Player.UserId}"))
                                {
                                    ev.ReturnMessage = "<color=red>Możesz zagłosować tylko raz!</color>";
                                }

                                else if (Round.IsStarted)
                                {
                                    ev.ReturnMessage = "<color=red>Nie możesz głosować w trakcie rundy!</color>";
                                }
                                break;
                            case "3":
                                if (vote.Contains($"DM{ev.Player.UserId}"))
                                {
                                    Deathmatch--;
                                    hideAndSeek++;
                                    vote.Remove($"DM{ev.Player.UserId}");
                                    vote.Add($"HAS{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zmieniono głos na HideAndSeek!";
                                }
                                else if (vote.Contains($"GW{ev.Player.UserId}"))
                                {
                                    GangWar--;
                                    hideAndSeek++;
                                    vote.Remove($"GW{ev.Player.UserId}");
                                    vote.Add($"HAS{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zmieniono głos na HideAndSeek!";
                                }
                                else if (vote.Contains($"DG{ev.Player.UserId}"))
                                {
                                    dgball--;
                                    hideAndSeek++;
                                    vote.Remove($"DG{ev.Player.UserId}");
                                    vote.Add($"HAS{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zmieniono głos na HideAndSeek!";
                                }
                                else if (vote.Contains($"PE{ev.Player.UserId}"))
                                {
                                    peanutRun--;
                                    hideAndSeek++;
                                    vote.Remove($"PE{ev.Player.UserId}");
                                    vote.Add($"HAS{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zmieniono głos na HideAndSeek!";
                                }
                                else if (!Round.IsStarted && !vote.Contains($"HAS{ev.Player.UserId}"))
                                {
                                    hideAndSeek++;
                                    vote.Add($"HAS{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zagłosowano na HideAndSeek!";
                                }
                                else if (vote.Contains($"HAS{ev.Player.UserId}"))
                                {
                                    ev.ReturnMessage = "<color=red>Możesz zagłosować tylko raz!</color>";
                                }

                                else if (Round.IsStarted)
                                {
                                    ev.ReturnMessage = "<color=red>Nie możesz głosować w trakcie rundy!</color>";
                                }
                                break;
                            case "4":
                                if (vote.Contains($"DM{ev.Player.UserId}"))
                                {
                                    Deathmatch--;
                                    peanutRun++;
                                    vote.Remove($"DM{ev.Player.UserId}");
                                    vote.Add($"PE{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zmieniono głos na PeanutRun!";
                                }
                                else if (vote.Contains($"HAS{ev.Player.UserId}"))
                                {
                                    hideAndSeek--;
                                    peanutRun++;
                                    vote.Remove($"HAS{ev.Player.UserId}");
                                    vote.Add($"PE{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zmieniono głos na PeanutRun!";
                                }
                                else if (vote.Contains($"DG{ev.Player.UserId}"))
                                {
                                    dgball--;
                                    peanutRun++;
                                    vote.Remove($"DG{ev.Player.UserId}");
                                    vote.Add($"PE{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zmieniono głos na PeanutRun!";
                                }
                                else if (vote.Contains($"GW{ev.Player.UserId}"))
                                {
                                    GangWar--;
                                    peanutRun++;
                                    vote.Remove($"GW{ev.Player.UserId}");
                                    vote.Add($"PE{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zmieniono głos na PeanutRun!";
                                }
                                else if (!Round.IsStarted && !vote.Contains($"PE{ev.Player.UserId}"))
                                {
                                    peanutRun++;
                                    vote.Add($"PE{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zagłosowano na PeanutRun!";
                                }
                                else if (vote.Contains($"PE{ev.Player.UserId}"))
                                {
                                    ev.ReturnMessage = "<color=red>Możesz zagłosować tylko raz!</color>";
                                }

                                else if (Round.IsStarted)
                                {
                                    ev.ReturnMessage = "<color=red>Nie możesz głosować w trakcie rundy!</color>";
                                }
                                break;
                            case "5":
                                if (vote.Contains($"DM{ev.Player.UserId}"))
                                {
                                    Deathmatch--;
                                    dgball++;
                                    vote.Remove($"DM{ev.Player.UserId}");
                                    vote.Add($"DG{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zmieniono głos na Dodgeball!";
                                }
                                else if (vote.Contains($"PE{ev.Player.UserId}"))
                                {
                                    peanutRun--;
                                    dgball++;
                                    vote.Remove($"PE{ev.Player.UserId}");
                                    vote.Add($"DG{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zmieniono głos na Dodgeball!";
                                }
                                else if (vote.Contains($"HAS{ev.Player.UserId}"))
                                {
                                    hideAndSeek--;
                                    dgball++;
                                    vote.Remove($"HAS{ev.Player.UserId}");
                                    vote.Add($"DG{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zmieniono głos na Dodgeball!";
                                }
                                else if (vote.Contains($"GW{ev.Player.UserId}"))
                                {
                                    GangWar--;
                                    dgball++;
                                    vote.Remove($"GW{ev.Player.UserId}");
                                    vote.Add($"DG{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zmieniono głos na Dodgeball!";
                                }
                                else if (!Round.IsStarted && !vote.Contains($"DG{ev.Player.UserId}"))
                                {
                                    dgball++;
                                    vote.Add($"DG{ev.Player.UserId}");
                                    ev.ReturnMessage = "Pomyślnie zagłosowano na Dodgeball!";
                                }
                                else if (vote.Contains($"DG{ev.Player.UserId}"))
                                {
                                    ev.ReturnMessage = "<color=red>Możesz zagłosować tylko raz!</color>";
                                }

                                else if (Round.IsStarted)
                                {
                                    ev.ReturnMessage = "<color=red>Nie możesz głosować w trakcie rundy!</color>";
                                }
                                break;
                        }
                        return;
                    }
                    break;
            }
            switch (ev.Name.ToLower())
            {
                case "randomtp":
                    ev.IsAllowed = false;
                    if (ev.Arguments.Count == 0)
                    {
                        if (!Round.IsStarted)
                        {
                            ev.ReturnMessage = "<color=red>nie możesz tego zrobić przed startem rundy!</color>";
                        }
                        else if (Round.IsStarted)
                        {
                            if (ev.Player.Role == RoleType.Tutorial)
                            {
                                List<Player> PlysToTeleport = Player.List.Where(p => p.Team != Team.RIP).ToList();
                                if (PlysToTeleport.Count == 0)
                                {
                                    ev.Player.ShowHint("brak graczy do których można się teleportować");
                                }
                                else
                                {
                                    Player chosen = PlysToTeleport.ElementAt(rnd.Next(PlysToTeleport.Count));
                                    ev.Player.Broadcast(5, $"<B>Przeteleportowano do: <color=blue>{chosen.Nickname}</color> Który gra jako: <color={chosen.RoleColor.ToHex()}>{chosen.Role}</color></B>");

                                    ev.Player.Position = chosen.Position + new Vector3(0, 2, 0);
                                }
                            }
                        }
                        return;
                    }
                    break;
            }
        }*/
        public void OnWTP()
        {
            SpawnManager();
            GameObject.Find("StartRound").transform.localScale = new Vector3(0, 0, 0);
            pInfoDict.Clear();
            foreach (FileInfo file in new DirectoryInfo(MiniGamesSystem.DataPath).GetFiles())
            {
                PlayerInfo info = JsonConvert.DeserializeObject<PlayerInfo>(File.ReadAllText(file.FullName));
                if (info.Coins == 0)
                {
                    File.Delete(file.FullName);
                    continue;
                }
                string userid = file.Name.Replace(".json", "");
                if (!pInfoDict.ContainsKey(userid) && !pInfoDict.ContainsValue(info))
                    pInfoDict.Add(userid, info);
            }
            pInfoDict = pInfoDict.OrderByDescending(x => x.Value.Coins).ToDictionary(x => x.Key, x => x.Value);

            var Unit = new SyncUnit();
            Unit.UnitName =
                "<color=#EFC01A>Witaj na</color> <color=green>Cwan</color><color=yellow>Stars</color>";
            Unit.SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox;

            var Unit1 = new SyncUnit();
            Unit1.UnitName = $"<color=#666699>Wejdź na naszego Discorda!</color>";
            Unit1.SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox;

            var Unit2 = new SyncUnit();
            Unit2.UnitName =
                "<color=#EFC01A>== Dostępne komendy ==</color>";
            Unit2.SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox;

            var Unit3 = new SyncUnit();
            Unit3.UnitName = $"<color=white>-</color> <color=#FAFF86>.vote</color>";
            Unit3.SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox;

            var Unit4 = new SyncUnit();
            Unit4.UnitName = $"<color=white>-</color> <color=#FAFF86>.sklep</color>";
            Unit4.SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox;

            var Unit5 = new SyncUnit();
            Unit5.UnitName = $"<color=white>-</color> <color=#FAFF86>.portfel</color>";
            Unit5.SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox;

            var Unit6 = new SyncUnit();
            Unit6.UnitName = $"<color=white>-</color> <color=#FAFF86>.eq</color>";
            Unit6.SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox;

            var Unit7 = new SyncUnit();
            Unit7.UnitName = $"<color=white>-</color> <color=#FAFF86>.top</color>";
            Unit7.SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox;

            var Space = new SyncUnit();
            Space.UnitName = $" ";
            Space.SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox;

            Timing.CallDelayed(0.001f, () => { RespawnManager.Singleton.NamingManager.AllUnitNames.Clear(); });
            Timing.CallDelayed(0.01f, () =>
            {
                RespawnManager.Singleton.NamingManager.AllUnitNames.Add(Unit);
                RespawnManager.Singleton.NamingManager.AllUnitNames.Add(Unit1);
                RespawnManager.Singleton.NamingManager.AllUnitNames.Add(Space);
                RespawnManager.Singleton.NamingManager.AllUnitNames.Add(Unit2);
                RespawnManager.Singleton.NamingManager.AllUnitNames.Add(Unit3);
                RespawnManager.Singleton.NamingManager.AllUnitNames.Add(Unit4);
                RespawnManager.Singleton.NamingManager.AllUnitNames.Add(Unit5);
                RespawnManager.Singleton.NamingManager.AllUnitNames.Add(Unit6);
                RespawnManager.Singleton.NamingManager.AllUnitNames.Add(Unit7);
            });
            foreach (CoroutineHandle coroutine in coroutines)
            {
                Timing.KillCoroutines(coroutine);
            }
            coroutines.Clear();

            coroutines.Add(Timing.RunCoroutine(LobbyTimer()));

            foreach (Door door in EMap.Doors)
            {
                if (EscapePrimary.Contains(door.Type))
                {
                    door.DoorLockType = DoorLockType.SpecialDoorFeature;
                }

                if (SurfaceGate.Contains(door.Type))
                {
                    door.DoorLockType = DoorLockType.SpecialDoorFeature;
                    door.Open = false;
                }

            }

        }

        public void OnWarheadCancel(StoppingEventArgs ev)
        {
            if (AktualnyEvent == "PeanutRun")
            {
                ev.IsAllowed = false;
            }
        }

        public void OnPlyDied(DiedEventArgs ev)
        {
            if (AktualnyEvent == "deathMatch")
            {
                if (pInfoDict.ContainsKey(ev.Killer.UserId))
                {
                    PlayerInfo info = pInfoDict[ev.Killer.UserId];
                    info.Coins++;

                    pInfoDict[ev.Killer.UserId] = info;
                }
            }
        }

        public void SpawnManager()
        {
            UnityEngine.Vector3 location = Map.Rooms.First(x => x.Type == RoomType.Surface).Position;
            location.y = location.y + 1;

            PossibleSpawnsPos.Clear();

            PossibleSpawnsPos.Add(new Vector3(location.x, location.y, location.z + 10));

            PossibleSpawnsPos.ShuffleList();

            ChoosedSpawnPos = PossibleSpawnsPos[0];
        }

        public void OnRS()
        {
            foreach (Player ply in Player.List)
            {
                ply.IsGodModeEnabled = false;
                ply.DisableEffect<CustomPlayerEffects.Scp207>();
                ply.SetRank("", "default");
                if (Extensions.hasTag) ply.RefreshTag();
                if (Extensions.isHidden) ply.ReferenceHub.characterClassManager.CmdRequestHideTag();
            }

            if (Deathmatch > (GangWar + hideAndSeek + dgball + peanutRun))
            {
                AktualnyEvent = "deathMatch";
                MiniGames.deathMatch();
            }
            else if (peanutRun > (Deathmatch + hideAndSeek + GangWar + dgball))
            {
                AktualnyEvent = "PeanutRun";
                MiniGames.PeanutRunn();
            }
            else if (GangWar > (Deathmatch + hideAndSeek + peanutRun + dgball))
            {
                AktualnyEvent = "WojnaGangow";
                MiniGames.WojnaGangow();
            }
            else if (hideAndSeek > (GangWar + Deathmatch + peanutRun + dgball))
            {
                AktualnyEvent = "HideAndSeek";
                MiniGames.HideAndSeek();
            }
            else if (dgball > (GangWar + Deathmatch + peanutRun + hideAndSeek))
            {
                AktualnyEvent = "DodgeBall";
                MiniGames.DgBall();
            }
            else
            {
                switch (rnd.Next(1, 6))
                {
                    case 1:
                        AktualnyEvent = "deathMatch";
                        MiniGames.deathMatch();
                        break;
                    case 2:
                        AktualnyEvent = "WojnaGangow";
                        MiniGames.WojnaGangow();
                        break;
                    case 3:
                        AktualnyEvent = "HideAndSeek";
                        MiniGames.HideAndSeek();
                        break;
                    case 4:
                        AktualnyEvent = "PeanutRun";
                        MiniGames.PeanutRunn();
                        break;
                    case 5:
                        AktualnyEvent = "DodgeBall";
                        MiniGames.DgBall();
                        break;
                }
                return;
            }
            Map.Broadcast(5, $"{EventMsg} <b><color>{AktualnyEvent}</color></b>");
        }

        public void OnRespawning(RespawningTeamEventArgs ev)
        {
            ev.IsAllowed = false;
        }

        public void OnRndEnd(EndingRoundEventArgs ev)
        {
            if (AktualnyEvent == "PeanutRun")
            {
                if (Player.Get(RoleType.Scp173).Count() > 1)
                {
                    foreach (Player ply in Player.Get(RoleType.Scp173))
                    {
                        if (pInfoDict.ContainsKey(ply.UserId))
                        {
                            PlayerInfo info = pInfoDict[ply.UserId];
                            info.Coins = info.Coins + 1;

                            pInfoDict[ply.UserId] = info;
                        }

                        Map.Broadcast(5, $"{ply.Nickname} wygrali!");
                    }
                }
                if (Player.Get(RoleType.Scp173).Count() == 1)
                {
                    foreach (Player ply in Player.Get(RoleType.Scp173))
                    {
                        if (pInfoDict.ContainsKey(ply.UserId))
                        {
                            PlayerInfo info = pInfoDict[ply.UserId];
                            info.Coins = info.Coins + 1;

                            pInfoDict[ply.UserId] = info;
                        }

                        Map.Broadcast(5, $"{ply.Nickname} wygrał!");
                    }
                }
            }
            else if (AktualnyEvent == "deathMatch")
            {
                foreach (Player ply in Player.Get(RoleType.ClassD))
                {
                    if (pInfoDict.ContainsKey(ply.UserId))
                    {
                        PlayerInfo info = pInfoDict[ply.UserId];
                        info.Coins = info.Coins + 2;

                        pInfoDict[ply.UserId] = info;
                    }
                }
            }
            else if (AktualnyEvent == "WojnaGangow")
            {
                var team1 = Player.Get(RoleType.ClassD);
                var team2 = Player.Get(RoleType.Scientist);
                if (team1.Count() == 0 && team2.Count() > 0)
                {
                    foreach (Player ply in team2)
                    {
                        if (pInfoDict.ContainsKey(ply.UserId))
                        {
                            if (pInfoDict.ContainsKey(ply.UserId))
                            {
                                PlayerInfo info = pInfoDict[ply.UserId];
                                info.Coins++;

                                pInfoDict[ply.UserId] = info;
                            }
                        }
                    }
                    Map.Broadcast(5, "<b><color=#FAFF86>Naukowcy</color> wygrali!</b>");
                }
                else if (team2.Count() == 0 && team1.Count() > 0)
                {
                    foreach (Player ply in team1)
                    {
                        if (pInfoDict.ContainsKey(ply.UserId))
                        {
                            PlayerInfo info = pInfoDict[ply.UserId];
                            info.Coins++;

                            pInfoDict[ply.UserId] = info;
                        }
                    }
                    Map.Broadcast(5, "<b><color=#EE7600>Klasa-D</color> wygrała!</b>");
                }
            }
            else if (AktualnyEvent == "DodgeBall")
            {
                foreach (Player ply in Player.Get(RoleType.ClassD))
                {
                    if (pInfoDict.ContainsKey(ply.UserId))
                    {
                        PlayerInfo info = pInfoDict[ply.UserId];
                        info.Coins++;

                        pInfoDict[ply.UserId] = info;
                    }
                }
            }
        }
        public void OnRoundRestart()
        {
            Deathmatch = 0;
            GangWar = 0;
            hideAndSeek = 0;
            peanutRun = 0;
            dgball = 0;
            Commands.Vote.vote.Clear();
            AktualnyEvent = "";

            if (AktualnyEvent == "PeanutRun")
            {
                PeanutRun.Plugin.Singleton.Methods.DisableGamemode(true);
            }
            else if (AktualnyEvent == "DodgeBall")
            {
                DodgeBall.Plugin.Singleton.Methods.DisableGamemode(true);
            }

            foreach (CoroutineHandle coroutine in coroutines)
            {
                Timing.KillCoroutines(coroutine);
            }
            coroutines.Clear();

            Timing.KillCoroutines("dmCR");
            Timing.KillCoroutines("dmcheck");
            Timing.KillCoroutines("hascheck");
            Timing.KillCoroutines();
            MiniGames.team1.Clear();

            foreach (KeyValuePair<string, PlayerInfo> info in pInfoDict)
            {
                File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
            }
        }

        public void OnShooting(ShootingEventArgs ev)
        {
            foreach (Player ply in Player.List)
            {
                if (props.Contains(ply.UserId))
                {
                    if (ev.ShotPosition == ply.Position)
                    {
                        ply.Kill(DamageTypes.Com18);
                    }
                }
            }
        }

        public void OnPickingUp(PickingUpItemEventArgs ev)
        {
            if (ev.Pickup.Base.gameObject.TryGetComponent<HatItemComponent>(out var hat))
            {
                ev.Player.ShowHint("<color=red>Nie możesz podnieść czapki!</color>");
                ev.IsAllowed = false;
            }
            if (props.Contains(ev.Player.UserId))
            {
                Item item = new Item(ev.Pickup.Type);
                
                item.Spawn(ev.Pickup.Position, default);

                ev.Player.IsInvisible = true;
                ev.IsAllowed = false;

                if (ev.Pickup.Type == ItemType.SCP018 || ev.Pickup.Type == ItemType.Medkit)
                {
                    ev.Player.Scale = new Vector3(0.01f, 0.01f, 0.01f);

                }
                else if (ev.Pickup.Type == ItemType.GunE11SR)
                {
                    ev.Player.Scale = new Vector3(0.1f, 0.1f, 0.1f);
                }
                Timing.RunCoroutine(TpProps(ev.Pickup));
            }
        }

        public IEnumerator<float> TpProps(Pickup type)
        {
            while (true)
            {
                foreach (Player ply in Player.List)
                {
                    if (props.Contains(ply.UserId))
                    {
                        type.Position = ply.Position;
                    }
                }
                yield return Timing.WaitForSeconds(0f);
            }
        }

        public IEnumerator<float> Czapki(Player ply, ItemType type)
        {
            Item item = new Item(type);
            
            Pickup Item = item.Spawn(ply.Position, default);
            while (true)
            {
                switch (ply.Role)
                {
                    case RoleType.Scp173:
                        Item.Position = new Vector3(0, .7f, -.05f) + ply.Position;
                        break;
                    case RoleType.Scp106:
                        Item.Position = new Vector3(0, .45f, .13f) + ply.Position;
                        break;
                    case RoleType.Scp096:
                        Item.Position = new Vector3(.15f, .45f, .225f) + ply.Position;
                        break;
                    case RoleType.Scp93953:
                        Item.Position = new Vector3(0, -.4f, 1.3f) + ply.Position;
                        break;
                    case RoleType.Scp93989:
                        Item.Position = new Vector3(0, -.3f, 1.3f) + ply.Position;
                        break;
                    case RoleType.Scp049:
                        Item.Position = new Vector3(0, .125f, -.05f) + ply.Position;
                        break;
                    case RoleType.None:
                        Item.Position = new Vector3(-1000, -1000, -1000) + ply.Position;
                        break;
                    case RoleType.Spectator:
                        Item.Position = new Vector3(-1000, -1000, -1000) + ply.Position;
                        break;
                    case RoleType.Scp0492:
                        Item.Position = new Vector3(0, 0f, -.06f) + ply.Position;
                        break;
                    default:
                        Item.Position = new Vector3(0, .15f, -.07f) + ply.Position;
                        break;
                }
                yield return Timing.WaitForSeconds(0.5f);
            }
        }

        public void OnJoin(VerifiedEventArgs ev)
        {
            if (!File.Exists(Path.Combine(MiniGamesSystem.DataPath, $"{ev.Player.UserId}.json")))
            {
                pInfoDict.Add(ev.Player.UserId, new PlayerInfo(ev.Player.Nickname));
            }

            if (!Round.IsStarted)
            {
                Timing.CallDelayed(0.5f, () =>
                {
                    ev.Player.Role = RoleType.NtfCaptain;
                    ev.Player.ClearInventory(true);
                    ev.Player.IsGodModeEnabled = true;
                    ev.Player.RankName = "W lobby";
                    ev.Player.RankColor = "pumpkin";
                });

                Timing.CallDelayed(1f, () =>
                {
                    ev.Player.ReferenceHub.playerEffectsController.EnableEffect<CustomPlayerEffects.Scp207>(999f, false);
                    ev.Player.ReferenceHub.playerEffectsController.ChangeEffectIntensity<CustomPlayerEffects.Scp207>(4);
                    ev.Player.Position = ChoosedSpawnPos;
                });

            }
            else if (Round.IsStarted)
            {
                ev.Player.Broadcast(5, $"<b><i>Aktualny Tryb: <color=green>{AktualnyEvent}</color></i></b>");
            }
        }

        private IEnumerator<float> LobbyTimer()
        {
            while (!Round.IsStarted)
            {
                message.Clear();

                if (MiniGamesSystem.Instance.Config.HintVertPos != 0 && MiniGamesSystem.Instance.Config.HintVertPos < 0)
                {
                    for (int i = MiniGamesSystem.Instance.Config.HintVertPos; i < 0; i++)
                    {
                        message.Append("\n");
                    }
                }


                message.Append($"<size=25><B><color=#00fe0f>DeathMatch</color> [id: 1 | głosy: {Deathmatch}]  |  <color=#00fe0f>WojnaGangów</color> [id: 2 | głosy: {GangWar}]  |  <color=#00fe0f>HideAndSeek</color> [id: 3 | głosy: {hideAndSeek}]     <color=#00fe0f>PeanutRun</color> [id: 4 | głosy: {peanutRun}]  |  <color=#00fe0f>Dodgeball</color> [id: 5 | głosy: {dgball}]</size><size=100><color=yellow>" + MiniGamesSystem.Instance.Config.TopMessage);

                short NetworkTimer = GameCore.RoundStart.singleton.NetworkTimer;

                switch (NetworkTimer)
                {
                    case -2: message.Replace("%seconds", MiniGamesSystem.Instance.Config.ServerIsPaused); break;

                    case -1: message.Replace("%seconds", MiniGamesSystem.Instance.Config.RoundIsBeingStarted); break;

                    case 1: message.Replace("%seconds", $"<color=green>{NetworkTimer}</color> {MiniGamesSystem.Instance.Config.OneSecondRemain}"); break;

                    case 0: message.Replace("%seconds", MiniGamesSystem.Instance.Config.RoundIsBeingStarted); break;

                    default: message.Replace("%seconds", $"<color=green>{NetworkTimer}</color> {MiniGamesSystem.Instance.Config.XSecondsRemains}"); break;
                }

                int NumOfPlayers = Player.List.Count();

                message.Append($"\n{MiniGamesSystem.Instance.Config.BottomMessage}");

                if (NumOfPlayers == 1) message.Replace("%players", $"<color=green>{NumOfPlayers}</color> {MiniGamesSystem.Instance.Config.OnePlayerConnected}");
                else message.Replace("%players", $"<color=green>{NumOfPlayers}</color> {MiniGamesSystem.Instance.Config.XPlayersConnected}");


                if (MiniGamesSystem.Instance.Config.HintVertPos != 0 && MiniGamesSystem.Instance.Config.HintVertPos > 0)
                {
                    for (int i = 0; i < MiniGamesSystem.Instance.Config.HintVertPos; i++)
                    {
                        message.Append("\n");
                    }
                }


                foreach (Player ply in Player.List)
                {
                    if (MiniGamesSystem.Instance.Config.UseHints) ply.ShowHint(message.ToString(), 1f);
                    else ply.Broadcast(1, message.ToString());
                }

                yield return Timing.WaitForSeconds(1f);
            }
        }
    }
}
