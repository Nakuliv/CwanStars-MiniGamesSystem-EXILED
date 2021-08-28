using System;
using Exiled.API.Features;
using ServerEv = Exiled.Events.Handlers.Server;
using PlayerEv = Exiled.Events.Handlers.Player;
using UnityEngine;
using System.IO;

namespace MiniGamesSystem
{
    public class MiniGamesSystem : Plugin<Config>
    {
        public static GameObject workstationObj;
        public static string DataPath = Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED"), "Plugins"), "MiniGamesSystemData");
        private static readonly Lazy<MiniGamesSystem> LazyInstance = new Lazy<MiniGamesSystem>(() => new MiniGamesSystem());
        public static MiniGamesSystem Instance => LazyInstance.Value;

        public override string Name { get; } = "MiniGamesSystem";
        public override string Author { get; } = "Cwaniak U.G";
        public override System.Version Version => new System.Version(1, 0, 0);


        private MiniGamesSystem() { }
        private Handler handler;

        public override void OnEnabled()
        {
            base.OnEnabled();
            handler = new Handler();

            if (!Directory.Exists(DataPath)) Directory.CreateDirectory(DataPath);

            ServerEv.WaitingForPlayers += handler.OnWTP;
            PlayerEv.Verified += handler.OnJoin;
            ServerEv.RoundStarted += handler.OnRS;
            ServerEv.SendingConsoleCommand += handler.OnConsoleCMD;
            ServerEv.SendingRemoteAdminCommand += handler.OnRAcmd;
            PlayerEv.PickingUpItem += handler.OnPickingUp;
            ServerEv.RestartingRound += handler.OnRoundRestart;
            PlayerEv.Shooting += handler.OnShooting;
            ServerEv.EndingRound += handler.OnRndEnd;
            ServerEv.RespawningTeam += handler.OnRespawning;
            Exiled.Events.Handlers.Warhead.Stopping += handler.OnWarheadCancel;
            PlayerEv.Died += handler.OnPlyDied;
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
            ServerEv.WaitingForPlayers -= handler.OnWTP;
            PlayerEv.Verified -= handler.OnJoin;
            ServerEv.RoundStarted -= handler.OnRS;
            ServerEv.SendingConsoleCommand -= handler.OnConsoleCMD;
            ServerEv.SendingRemoteAdminCommand -= handler.OnRAcmd;
            PlayerEv.PickingUpItem -= handler.OnPickingUp;
            ServerEv.RestartingRound -= handler.OnRoundRestart;
            PlayerEv.Shooting -= handler.OnShooting;
            ServerEv.EndingRound -= handler.OnRndEnd;
            ServerEv.RespawningTeam -= handler.OnRespawning;
            Exiled.Events.Handlers.Warhead.Stopping -= handler.OnWarheadCancel;
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
            bench.GetComponent<WorkStation>().Network_playerConnected = null;
            return bench;
        }
    }
}
