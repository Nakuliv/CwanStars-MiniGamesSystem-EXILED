using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;

namespace MiniGamesSystem
{
    public static class Extensions
    {
        internal static bool hasTag;
        internal static bool isHidden;

        public static void SetRank(this Player player, string rank, string color = "default")
        {
            player.ReferenceHub.serverRoles.Network_myText = rank;
            player.ReferenceHub.serverRoles.Network_myColor = color;
        }

        public static void RefreshTag(this Player player)
        {
            player.ReferenceHub.serverRoles.HiddenBadge = null; player.ReferenceHub.serverRoles.RpcResetFixed(); player.ReferenceHub.serverRoles.RefreshPermissions(true);
        }
    }
}
