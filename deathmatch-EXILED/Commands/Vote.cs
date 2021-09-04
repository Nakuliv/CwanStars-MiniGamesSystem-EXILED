using System;
using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;

namespace MiniGamesSystem.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Vote : ParentCommand
    {
        public Vote() => LoadGeneratedCommands();

        public override string Command { get; } = "vote";

        public override string[] Aliases { get; } = new string[] {"vt" };

        public override string Description { get; } = "Vote MiniGames.";

        public override void LoadGeneratedCommands() { }

        public static HashSet<string> vote = new HashSet<string>();

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var ply = Player.Get(((PlayerCommandSender)sender).ReferenceHub);
            if (arguments.Count == 0)
            {
                if (!Round.IsStarted)
                {
                    response = "<color=red>musisz wpisać numer eventu!</color>";
                    return false;
                }
                else if (Round.IsStarted)
                {
                    response = "<color=red>Nie możesz głosować w trakcie rundy!</color>";
                    return false;
                }
            }
            else if (arguments.Count != 0)
            {
                switch (arguments.At(0).ToLower())
                {
                    case "1":
                        if (vote.Contains($"HAS{ply.UserId}"))
                        {
                            Handler.hideAndSeek--;
                            Handler.Deathmatch++;
                            vote.Remove($"HAS{ply.UserId}");
                            vote.Add($"DM{ply.UserId}");
                            response = "Pomyślnie zmieniono głos na DeathMatch!";
                            return true;
                        }
                        else if (vote.Contains($"GW{ply.UserId}"))
                        {
                            Handler.GangWar--;
                            Handler.Deathmatch++;
                            vote.Remove($"GW{ply.UserId}");
                            vote.Add($"DM{ply.UserId}");
                            response = "Pomyślnie zmieniono głos na DeathMatch!";
                            return true;
                        }
                        else if (vote.Contains($"DG{ply.UserId}"))
                        {
                            Handler.dgball--;
                            Handler.Deathmatch++;
                            vote.Remove($"DG{ply.UserId}");
                            vote.Add($"DM{ply.UserId}");
                            response = "Pomyślnie zmieniono głos na DeathMatch!";
                            return true;
                        }
                        else if (vote.Contains($"PE{ply.UserId}"))
                        {
                            Handler.peanutRun--;
                            Handler.Deathmatch++;
                            vote.Remove($"PE{ply.UserId}");
                            vote.Add($"DM{ply.UserId}");
                            response = "Pomyślnie zmieniono głos na DeathMatch!";
                            return true;
                        }
                        else if (!Round.IsStarted && !vote.Contains($"DM{ply.UserId}"))
                        {
                            Handler.Deathmatch++;
                            vote.Add($"DM{ply.UserId}");
                            response = "Pomyślnie zagłosowano na DeathMatch!";
                            return true;
                        }
                        else if (vote.Contains($"DM{ply.UserId}"))
                        {
                            response = "<color=red>Możesz zagłosować tylko raz!</color>";
                            return false;
                        }

                        else if (Round.IsStarted)
                        {
                            response = "<color=red>Nie możesz głosować w trakcie rundy!</color>";
                            return false;
                        }

                        break;
                    case "2":
                        if (vote.Contains($"DM{ply.UserId}"))
                        {
                            Handler.Deathmatch--;
                            Handler.GangWar++;
                            vote.Remove($"DM{ply.UserId}");
                            vote.Add($"GW{ply.UserId}");
                            response = "Pomyślnie zmieniono głos na GangWar!";
                            return true;
                        }
                        else if (vote.Contains($"PE{ply.UserId}"))
                        {
                            Handler.peanutRun--;
                            Handler.GangWar++;
                            vote.Remove($"PE{ply.UserId}");
                            vote.Add($"GW{ply.UserId}");
                            response = "Pomyślnie zmieniono głos na GangWar!";
                            return true;
                        }
                        else if (vote.Contains($"DG{ply.UserId}"))
                        {
                            Handler.dgball--;
                            Handler.GangWar++;
                            vote.Remove($"DG{ply.UserId}");
                            vote.Add($"GW{ply.UserId}");
                            response = "Pomyślnie zmieniono głos na GangWar!";
                            return true;
                        }
                        else if (vote.Contains($"HAS{ply.UserId}"))
                        {
                            Handler.hideAndSeek--;
                            Handler.GangWar++;
                            vote.Remove($"HAS{ply.UserId}");
                            vote.Add($"GW{ply.UserId}");
                            response = "Pomyślnie zmieniono głos na GangWar!";
                        }
                        else if (!Round.IsStarted && !vote.Contains($"GW{ply.UserId}"))
                        {
                            Handler.GangWar++;
                            vote.Add($"GW{ply.UserId}");
                            response = "Pomyślnie zagłosowano na GangWar!";
                            return true;
                        }
                        else if (vote.Contains($"GW{ply.UserId}"))
                        {
                            response = "<color=red>Możesz zagłosować tylko raz!</color>";
                            return false;
                        }

                        else if (Round.IsStarted)
                        {
                            response = "<color=red>Nie możesz głosować w trakcie rundy!</color>";
                            return false;
                        }
                        break;
                    case "3":
                        if (vote.Contains($"DM{ply.UserId}"))
                        {
                            Handler.Deathmatch--;
                            Handler.hideAndSeek++;
                            vote.Remove($"DM{ply.UserId}");
                            vote.Add($"HAS{ply.UserId}");
                            response = "Pomyślnie zmieniono głos na HideAndSeek!";
                            return true;
                        }
                        else if (vote.Contains($"GW{ply.UserId}"))
                        {
                            Handler.GangWar--;
                            Handler.hideAndSeek++;
                            vote.Remove($"GW{ply.UserId}");
                            vote.Add($"HAS{ply.UserId}");
                            response = "Pomyślnie zmieniono głos na HideAndSeek!";
                            return true;
                        }
                        else if (vote.Contains($"DG{ply.UserId}"))
                        {
                            Handler.dgball--;
                            Handler.hideAndSeek++;
                            vote.Remove($"DG{ply.UserId}");
                            vote.Add($"HAS{ply.UserId}");
                            response = "Pomyślnie zmieniono głos na HideAndSeek!";
                            return true;
                        }
                        else if (vote.Contains($"PE{ply.UserId}"))
                        {
                            Handler.peanutRun--;
                            Handler.hideAndSeek++;
                            vote.Remove($"PE{ply.UserId}");
                            vote.Add($"HAS{ply.UserId}");
                            response = "Pomyślnie zmieniono głos na HideAndSeek!";
                            return true;
                        }
                        else if (!Round.IsStarted && !vote.Contains($"HAS{ply.UserId}"))
                        {
                            Handler.hideAndSeek++;
                            vote.Add($"HAS{ply.UserId}");
                            response = "Pomyślnie zagłosowano na HideAndSeek!";
                            return true;
                        }
                        else if (vote.Contains($"HAS{ply.UserId}"))
                        {
                            response = "<color=red>Możesz zagłosować tylko raz!</color>";
                            return false;
                        }

                        else if (Round.IsStarted)
                        {
                            response = "<color=red>Nie możesz głosować w trakcie rundy!</color>";
                            return false;
                        }
                        break;
                    case "4":
                        if (vote.Contains($"DM{ply.UserId}"))
                        {
                            Handler.Deathmatch--;
                            Handler.peanutRun++;
                            vote.Remove($"DM{ply.UserId}");
                            vote.Add($"PE{ply.UserId}");
                            response = "Pomyślnie zmieniono głos na PeanutRun!";
                            return true;
                        }
                        else if (vote.Contains($"HAS{ply.UserId}"))
                        {
                            Handler.hideAndSeek--;
                            Handler.peanutRun++;
                            vote.Remove($"HAS{ply.UserId}");
                            vote.Add($"PE{ply.UserId}");
                            response = "Pomyślnie zmieniono głos na PeanutRun!";
                            return true;
                        }
                        else if (vote.Contains($"DG{ply.UserId}"))
                        {
                            Handler.dgball--;
                            Handler.peanutRun++;
                            vote.Remove($"DG{ply.UserId}");
                            vote.Add($"PE{ply.UserId}");
                            response = "Pomyślnie zmieniono głos na PeanutRun!";
                            return true;
                        }
                        else if (vote.Contains($"GW{ply.UserId}"))
                        {
                            Handler.GangWar--;
                            Handler.peanutRun++;
                            vote.Remove($"GW{ply.UserId}");
                            vote.Add($"PE{ply.UserId}");
                            response = "Pomyślnie zmieniono głos na PeanutRun!";
                            return true;
                        }
                        else if (!Round.IsStarted && !vote.Contains($"PE{ply.UserId}"))
                        {
                            Handler.peanutRun++;
                            vote.Add($"PE{ply.UserId}");
                            response = "Pomyślnie zagłosowano na PeanutRun!";
                            return true;
                        }
                        else if (vote.Contains($"PE{ply.UserId}"))
                        {
                            response = "<color=red>Możesz zagłosować tylko raz!</color>";
                            return false;
                        }

                        else if (Round.IsStarted)
                        {
                            response = "<color=red>Nie możesz głosować w trakcie rundy!</color>";
                            return false;
                        }
                        break;
                    case "5":
                        if (vote.Contains($"DM{ply.UserId}"))
                        {
                            Handler.Deathmatch--;
                            Handler.dgball++;
                            vote.Remove($"DM{ply.UserId}");
                            vote.Add($"DG{ply.UserId}");
                            response = "Pomyślnie zmieniono głos na Dodgeball!";
                            return true;
                        }
                        else if (vote.Contains($"PE{ply.UserId}"))
                        {
                            Handler.peanutRun--;
                            Handler.dgball++;
                            vote.Remove($"PE{ply.UserId}");
                            vote.Add($"DG{ply.UserId}");
                            response = "Pomyślnie zmieniono głos na Dodgeball!";
                            return true;
                        }
                        else if (vote.Contains($"HAS{ply.UserId}"))
                        {
                            Handler.hideAndSeek--;
                            Handler.dgball++;
                            vote.Remove($"HAS{ply.UserId}");
                            vote.Add($"DG{ply.UserId}");
                            response = "Pomyślnie zmieniono głos na Dodgeball!";
                            return true;
                        }
                        else if (vote.Contains($"GW{ply.UserId}"))
                        {
                            Handler.GangWar--;
                            Handler.dgball++;
                            vote.Remove($"GW{ply.UserId}");
                            vote.Add($"DG{ply.UserId}");
                            response = "Pomyślnie zmieniono głos na Dodgeball!";
                            return true;
                        }
                        else if (!Round.IsStarted && !vote.Contains($"DG{ply.UserId}"))
                        {
                            Handler.dgball++;
                            vote.Add($"DG{ply.UserId}");
                            response = "Pomyślnie zagłosowano na Dodgeball!";
                            return true;
                        }
                        else if (vote.Contains($"DG{ply.UserId}"))
                        {
                            response = "<color=red>Możesz zagłosować tylko raz!</color>";
                            return false;
                        }

                        else if (Round.IsStarted)
                        {
                            response = "<color=red>Nie możesz głosować w trakcie rundy!</color>";
                            return false;
                        }
                        break;
                }
            }
            response = "";
            return true;
        }
    }
}
