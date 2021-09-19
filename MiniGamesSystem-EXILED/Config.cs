using Exiled.API.Interfaces;
using System.ComponentModel;

namespace MiniGamesSystem
{
    public class Config : IConfig
    {
        [Description("Display the list of commands?")]
        public bool DisplayCommandList { get; set; } = true;
        public string CommandListTop { get; set; } = "Dostępne komendy";

        [Description("Determines the position of the Hint on the users screen (32 = Top, 0 = Middle, -15 = Below)")]
        public int HintVertPos { get; set; } = 25;

        [Description("Text traslations:")]
        public string TopMessage { get; set; } = "\nWitaj w lobby!</color></size><size=25>\nWpisz <color=yellow>.vote <id eventu></color> w konsoli pod `(~) żeby zagłosować na tryb</size><size=50>\nGra rozpocznie się za: %seconds</B></size>";
        public string BottomMessage { get; set; } = "<size=30><i>%players</i></size>";
        public string ServerIsPaused { get; set; } = "<color=red>Runda jest zatrzymana</color>";
        public string RoundIsBeingStarted { get; set; } = "<color=green>Runda się rozpoczyna...</color>";
        public string OneSecondRemain { get; set; } = "<color=red>Potrzebna jest jeszcze jedna osoba</color>";
        public string XSecondsRemains { get; set; } = "sekund";
        public string OnePlayerConnected { get; set; } = "połączony gracz";
        public string XPlayersConnected { get; set; } = "połączonych graczy";
        public bool IsEnabled { get; set; } = true;
    }
}