using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Exiled.API.Features;
using Exiled.Loader;

namespace MiniGamesSystem
{
    internal static class Helper
    {
        private static MethodInfo IsGhost = null;
        private static MethodInfo IsNpc = null;

        internal static bool IsPlayerGhost(Player p)
        {
            if (IsGhost == null) return false;

            return (bool)(IsGhost.Invoke(null, new object[] { p }) ?? false);
        }

        internal static bool IsPlayerNPC(Player p)
        {
            return (bool)(IsNpc?.Invoke(null, new object[] { p }) ?? false) || p.Id == 9999 || p.IPAddress == "127.0.0.WAN";
        }
    }
}