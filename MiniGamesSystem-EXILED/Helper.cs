using System.Reflection;
using Exiled.API.Features;
using UnityEngine;

namespace MiniGamesSystem
{
    internal static class Helper
    {
        private static MethodInfo IsGhost = null;
        private static MethodInfo IsNpc = null;
        internal static bool IsZero(this Quaternion rot) => rot.x == 0 && rot.y == 0 && rot.z == 0;
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