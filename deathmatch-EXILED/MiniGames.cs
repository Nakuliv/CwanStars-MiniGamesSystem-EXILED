using System.Collections.Generic;
using System.Linq;
using EMap = Exiled.API.Features.Map;
using Exiled.API.Enums;

using Exiled.API.Features;
using MEC;
using UnityEngine;
using Exiled.API.Features.Items;

namespace MiniGamesSystem
{
    public class MiniGames
    {
        static System.Random rnd = new System.Random();
        public static Dictionary<string, PlayerInfo> pInfoDict = new Dictionary<string, PlayerInfo>();
        public static string EventMsg = "[<color=blue>Tryb</color>]";
        public static List<DoorType> lockedCheckpointLcz = new List<DoorType>() { DoorType.CheckpointLczA, DoorType.CheckpointLczB };
        public static List<DoorType> EscapePrimary = new List<DoorType>() { DoorType.EscapePrimary };
        public static List<DoorType> SurfaceGate = new List<DoorType>() { DoorType.SurfaceGate };
        public static List<DoorType> GangWarDoors = new List<DoorType>() { DoorType.CheckpointEntrance, DoorType.GateA, DoorType.GateB };
        public static List<string> team1 = new List<string>();
        public CoroutineHandle Coroutine { get; set; }

        //DeathMatch
        public static void deathMatch()
        {
            Round.IsLocked = true;

            foreach (Door door in EMap.Doors)
            {
                if (door.Type == DoorType.CheckpointLczA || door.Type == DoorType.CheckpointLczB)
                {
                    door.ChangeLock(DoorLockType.SpecialDoorFeature);
                }
            }
            foreach (Player player in Player.List)
            {
                player.Role = RoleType.ClassD;
                Vector3 spawnPos = Extensions.GetRandomSpawnPoint(RoleType.Scientist);

                Timing.CallDelayed(0.5f, () => player.Position = spawnPos);
            }

            Map.Broadcast(5, "<b>Za 30 sekund wszyscy dostaną bronie i leczenie</b>");

            Timing.CallDelayed(25, () =>
            {
                Map.Broadcast(1, "5");
                Map.Broadcast(1, "4");
                Map.Broadcast(1, "3");
                Map.Broadcast(1, "2");
                Map.Broadcast(1, "1");
            });
            Timing.CallDelayed(30, () =>
            {
                Map.ShowHint("Otrzymałeś broń i leczenie, sprawdź ekwipunek!", 5);
                foreach (Player player in Player.Get(RoleType.ClassD))
                {
                    player.ResetInventory(new List<ItemType> { ItemType.GunCOM18, ItemType.Adrenaline, ItemType.Medkit });
                    player.Ammo[ItemType.Ammo9x19] = 200;
                    player.Ammo[ItemType.Ammo44cal] = 200;
                    player.Ammo[ItemType.Ammo12gauge] = 200;
                }
            });


                Timing.CallDelayed(300, () =>
            {
                    Map.Broadcast(1, "Checkpointy zostały otwarte!");

                    foreach (Door door in EMap.Doors)
                    {
                        if (door.Type == DoorType.CheckpointLczA || door.Type == DoorType.CheckpointLczB)
                        {
                            door.Unlock();
                            door.BreakDoor();
                        }
                    }
            });

            foreach (Pickup item in Map.Pickups)
                item.Destroy();

            Timing.RunCoroutine(DeathMatchCheck(), "dmcheck");
        }
        private static IEnumerator<float> DeathMatchCheck()
        {
            while (true)
            {
                if (Player.Get(RoleType.ClassD).Count() == 1)
                {
                    if (Round.ElapsedTime.Seconds > 1)
                    {
                        foreach (Player ply in Player.Get(RoleType.ClassD))
                        {
                            Map.Broadcast(5, $"<b><color=green>{ply.Nickname}</color> wygrał!</b>");
                            Round.IsLocked = false;
                            Timing.KillCoroutines("dmcheck");
                        }
                    }

                }
                yield return Timing.WaitForSeconds(5f);
            }
        }

        //Wojna Gangów
        public static void WojnaGangow()
        {
            foreach (Pickup item in Map.Pickups)
                item.Destroy();

            List<Player> players = Player.List.ToList();
            for (int i = 0; i < players.Count / 2; i++)
            {
                UnityEngine.Vector3 location3 = Map.Rooms.First(x => x.Type == RoomType.EzCollapsedTunnel).Position;
                location3.y = location3.y + 1;

                team1.Add(players[i].UserId);
                players[i].Role = RoleType.Scientist;
                players[i].ClearInventory();
            }
            foreach (Door door in EMap.Doors)
            {
                if (GangWarDoors.Contains(door.Type))
                {
                    door.ChangeLock(DoorLockType.SpecialDoorFeature);
                }
            }
            foreach (Player player in players)
            {
                UnityEngine.Vector3 location3 = Map.Rooms.First(x => x.Type == RoomType.EzDownstairsPcs).Position;
                location3.y = location3.y + 1;

                UnityEngine.Vector3 location2 = Map.Rooms.First(x => x.Type == RoomType.EzConference).Position;
                location2.y = location2.y + 1;

                Timing.CallDelayed(0.1f, () =>
                {
                    if (!team1.Contains(player.UserId))
                    {
                        player.Role = RoleType.ClassD;
                        player.ClearInventory();
                    }
                });
                Timing.CallDelayed(0.5f, () =>
                {
                    if (!team1.Contains(player.UserId))
                    {
                        player.Ammo[ItemType.Ammo9x19] = 200;
                        player.Ammo[ItemType.Ammo44cal] = 200;
                        player.Ammo[ItemType.Ammo12gauge] = 200;
                        player.AddItem(ItemType.GunCOM15);
                        player.AddItem(ItemType.Medkit);
                        player.AddItem(ItemType.SCP500);
                        player.AddItem(ItemType.Adrenaline);
                    }
                    else
                    {
                        player.Ammo[ItemType.Ammo9x19] = 200;
                        player.Ammo[ItemType.Ammo44cal] = 200;
                        player.Ammo[ItemType.Ammo12gauge] = 200;
                        player.AddItem(ItemType.GunCOM15);
                        player.AddItem(ItemType.Medkit);
                        player.AddItem(ItemType.SCP500);
                        player.AddItem(ItemType.Adrenaline);
                    }
                });
                Timing.CallDelayed(0.6f, () =>
                {
                    if (!team1.Contains(player.UserId))
                    {
                        player.Position = new Vector3(location2.x, location2.y, location2.z);
                    }
                    else
                    {
                        player.Position = new Vector3(location3.x, location3.y, location3.z);
                    }
                });
            }
        }

        //Hide And Seek
        public static void HideAndSeek()
        {
            Timing.RunCoroutine(HideAndSeekCheck(), "hascheck");
            int counter = 0;
            foreach (Player player in Player.List)
            {
                if (Player.List.Count() < 5)
                {
                    if (counter > 0)
                    {
                        player.Role = RoleType.ClassD;
                        player.Broadcast(5, "<b>Jesteś <color=#32CD32>chowającym</color>, masz 30 sekund na schowanie się!</b>");
                    }
                    else
                    {
                        player.Role = RoleType.Scp93953;
                        Timing.CallDelayed(0.5f, () =>
                        {
                            player.Position = Extensions.GetRandomSpawnPoint(RoleType.Scp106);
                        });

                        player.Broadcast(5, "<b>Jesteś <color=red>szukającym</color>, za 30 sekund będziesz mógł szukać!</b>");
                        counter++;
                        Timing.CallDelayed(30, () =>
                        {
                            player.Position = Extensions.GetRandomSpawnPoint(RoleType.Scp173);
                            Map.Broadcast(5, "<b><color=red>Szukający</color> zostali wypuszczeni!</b>");
                        });
                    }
                }
                else
                {
                    if (counter > 2)
                    {
                        player.Role = RoleType.ClassD;
                        player.Broadcast(5, "<b>Jesteś <color=#32CD32>chowającym</color>, masz 30 sekund na schowanie się!</b>");
                    }
                    else
                    {
                        player.Role = RoleType.Scp93953;
                        Timing.CallDelayed(0.5f, () =>
                        {
                            player.Position = Extensions.GetRandomSpawnPoint(RoleType.Scp106);
                        });
                        player.Broadcast(5, "<b>Jesteś <color=red>szukającym</color>, za 30 sekund będziesz mógł szukać!</b>");
                        counter++;
                        Timing.CallDelayed(30, () =>
                        {
                            player.Position = Extensions.GetRandomSpawnPoint(RoleType.Scp173);
                        });
                    }
                }

                Timing.CallDelayed(900, () =>
            {
                    if (Player.Get(RoleType.ClassD).Count() > 0)
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
                        foreach (Player scps in Player.Get(RoleType.Scp93953))
                        {
                            scps.Kill();
                        }
                        Map.Broadcast(5, "<b><color=orange>Klasa-D</color> wygrała!</b>");
                    }
                    else if (Player.Get(RoleType.Scp93953).Count() > 0)
                    {
                        foreach (Player ply in Player.Get(RoleType.Scp93953))
                        {
                            if (pInfoDict.ContainsKey(ply.UserId))
                            {
                                PlayerInfo info = pInfoDict[ply.UserId];
                                info.Coins = info.Coins + 1;

                                pInfoDict[ply.UserId] = info;
                            }
                        }
                        Map.Broadcast(5, "<b><color=red>SCP</color> wygrały!</b>");
                    }
            });
            }
        }

        private static IEnumerator<float> HideAndSeekCheck()
        {
            while (true)
            {
                if(Round.ElapsedTime.Seconds > 5)
                {
                    if (Player.Get(RoleType.ClassD).Count() > 0 && Player.Get(RoleType.Scp93953).Count() == 0)
                    {
                        foreach (Player ply in Player.List)
                        {
                            if (ply.Role == RoleType.ClassD)
                            {
                                if (pInfoDict.ContainsKey(ply.UserId))
                                {
                                    PlayerInfo info = pInfoDict[ply.UserId];
                                    info.Coins = info.Coins + 1;

                                    pInfoDict[ply.UserId] = info;
                                }
                            }
                            else if (ply.Role == RoleType.Scp93953)
                            {
                                ply.Kill();
                            }
                        }
                        Map.Broadcast(5, "<b><color=orange>Klasa-D</color> wygrała!</b>");
                        Timing.KillCoroutines("hascheck");
                    }
                    else if (Player.Get(RoleType.Scp93953).Count() > 0 && Player.Get(RoleType.ClassD).Count() == 0)
                    {
                        foreach (Player ply in Player.List)
                        {
                            if (ply.Role == RoleType.Scp93953)
                            {
                                if (pInfoDict.ContainsKey(ply.UserId))
                                {
                                    PlayerInfo info = pInfoDict[ply.UserId];
                                    info.Coins = info.Coins + 1;

                                    pInfoDict[ply.UserId] = info;
                                }
                            }
                        }
                        Map.Broadcast(5, "<b><color=red>SCP</color> wygrały!</b>");
                        Timing.KillCoroutines("hascheck");
                    }
                }
                yield return Timing.WaitForSeconds(5f);
            }
        }

        //PropHunt

        public static void PropHunt()
        {
            List<Player> players = Player.List.ToList();
            for (int i = 0; i < players.Count / 2; i++)
            {
                players[i].Role = RoleType.ClassD;
                Handler.props.Add(players[i].UserId);
                players.Remove(players[i]);
            }

            foreach (Player player in players)
            {
                player.Role = RoleType.NtfCaptain;
                player.ClearInventory();

                Timing.CallDelayed(0.1f, () =>
                {
                    player.AddItem(ItemType.GunCOM18);
                    player.Ammo[ItemType.Ammo9x19] = 200;
                    player.Ammo[ItemType.Ammo44cal] = 200;
                    player.Ammo[ItemType.Ammo12gauge] = 200;
                    player.Position = Map.Rooms.First(x => x.Type == RoomType.LczCafe).Position;
                });
            }
        }
        //PeanutRun
        public static void PeanutRunn()
        {
            Warhead.Start();
                foreach (Player player in Player.List)
                    player.Role = RoleType.Scp173;

            Round.IsLocked = true;
        }

        public static void DgBall()
        {
            Round.IsLocked = true;
            Vector3 spawnPos = Extensions.GetRandomSpawnPoint(RoleType.Scp173);
            foreach (Player player in Player.List)
            {
                player.Role = RoleType.ClassD;
                Timing.CallDelayed(0.5f, () => player.Position = spawnPos);
            }

            foreach (Door door in EMap.Doors)
            {
                if (door.Type == DoorType.Scp173Gate)
                {

                    door.ChangeLock(DoorLockType.SpecialDoorFeature);
                }
            }
            Timing.RunCoroutine(DodgeballLoop(), "dgballLoop");
        }

        static IEnumerator<float> DodgeballLoop()
        {
            yield return Timing.WaitForSeconds(10f);

            for (; ; )
            {
                int count = 0;
                Player winner = null;
                foreach (Player player in Player.List)
                    if (player.IsAlive)
                    {
                        count++;
                        winner = player;
                    }

                if (count <= 1)
                {
                    Round.IsLocked = false;
                    Map.Broadcast(10, $"<B><color=green>{winner.Nickname}</color> wygrał!</B>");
                    yield break;
                }

                foreach (Player player in Player.List)
                    if (rnd.Next(100) >= 50)
                        new ExplosiveGrenade(ItemType.SCP018).SpawnActive(player.Position, player);

                yield return Timing.WaitForSeconds(5.5f);
            }
        }
    }
}
