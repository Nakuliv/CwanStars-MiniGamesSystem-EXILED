using System;
using Exiled.API.Features;
using ServerEv = Exiled.Events.Handlers.Server;
using PlayerEv = Exiled.Events.Handlers.Player;
using MapEv = Exiled.Events.Handlers.Map;
using WarheadEv = Exiled.Events.Handlers.Warhead;
using UnityEngine;
using System.IO;
using Exiled.API.Enums;

namespace MiniGamesSystem
{
    public class MiniGamesSystem : Plugin<Config>
    {
        public static GameObject workstationObj;
        public static string DataPath = Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED"), "Plugins"), "MiniGamesSystemData");
        private static readonly Lazy<MiniGamesSystem> LazyInstance = new Lazy<MiniGamesSystem>(() => new MiniGamesSystem());
        public static MiniGamesSystem Instance => LazyInstance.Value;

        public override string Name { get; } = "MiniGamesSystem";
        public override string Author { get; } = "Naku (Cwaniaak.)";
        public override Version Version => new Version(2, 0, 0);

        public override PluginPriority Priority => PluginPriority.Highest;
        private MiniGamesSystem() { }
        private Handler handler;

        public override void OnEnabled()
        {
            base.OnEnabled();
            handler = new Handler();

            if (!Directory.Exists(DataPath)) Directory.CreateDirectory(DataPath);

            WarheadEv.Detonated += handler.OnWarheadDetonated;
            MapEv.SpawningItem += handler.OnSpawningItems;
            ServerEv.WaitingForPlayers += handler.OnWTP;
            PlayerEv.Verified += handler.OnJoin;
            ServerEv.RoundStarted += handler.OnRS;
            PlayerEv.PickingUpItem += handler.OnPickingUp;
            ServerEv.RestartingRound += handler.OnRoundRestart;
            PlayerEv.Shooting += handler.OnShooting;
            ServerEv.EndingRound += handler.OnRndEnd;
            ServerEv.RespawningTeam += handler.OnRespawning;
            WarheadEv.Stopping += handler.OnWarheadCancel;
            PlayerEv.Died += handler.OnPlyDied;
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
            WarheadEv.Detonated -= handler.OnWarheadDetonated;
            MapEv.SpawningItem -= handler.OnSpawningItems;
            ServerEv.WaitingForPlayers -= handler.OnWTP;
            PlayerEv.Verified -= handler.OnJoin;
            ServerEv.RoundStarted -= handler.OnRS;
            PlayerEv.PickingUpItem -= handler.OnPickingUp;
            ServerEv.RestartingRound -= handler.OnRoundRestart;
            PlayerEv.Shooting -= handler.OnShooting;
            ServerEv.EndingRound -= handler.OnRndEnd;
            ServerEv.RespawningTeam -= handler.OnRespawning;
            WarheadEv.Stopping -= handler.OnWarheadCancel;
            PlayerEv.Died -= handler.OnPlyDied;
            handler = null;
        }

        public enum ObjectType
        {
            WorkStation,
            DoorLCZ,
            DoorHCZ,
            DoorEZ
        }

        public static GameObject GetWorkStationObject(ObjectType type)
        {
            GameObject bench = null;
            bench = UnityEngine.Object.Instantiate(workstationObj);
            return bench;
        }
    }
}
