using Exiled.API.Features;
using Exiled.API.Features.Items;
using MiniGamesSystem.Hats;
using UnityEngine;

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

        public static Vector3 GetRandomSpawnPoint(RoleType roleType)
        {
            GameObject randomPosition = UnityEngine.Object.FindObjectOfType<SpawnpointManager>().GetRandomPosition(roleType);

            return randomPosition == null ? Vector3.zero : randomPosition.transform.position;
        }

        public static void SpawnHat(Player player, HatInfo hat)
        {
            if (hat.Item == ItemType.None) return;

            var pos = Hats.Hats.GetHatPosForRole(player.Role);
            var itemOffset = Vector3.zero;
            var rot = Quaternion.Euler(0, 0, 0);
            var scale = Vector3.one;
            var item = hat.Item;

            switch (item)
            {
                case ItemType.KeycardScientist:
                    scale += new Vector3(1.5f, 20f, 1.5f);
                    rot = Quaternion.Euler(0, 90, 0);
                    itemOffset = new Vector3(0, .1f, 0);
                    break;

                case ItemType.KeycardNTFCommander:
                    scale += new Vector3(1.5f, 200f, 1.5f);
                    rot = Quaternion.Euler(0, 90, 0);
                    itemOffset = new Vector3(0, .9f, 0);
                    break;

                case ItemType.SCP268:
                    scale += new Vector3(-.1f, -.1f, -.1f);
                    rot = Quaternion.Euler(-90, 0, 90);
                    itemOffset = new Vector3(0, 0, .1f);
                    break;

                case ItemType.Adrenaline:
                case ItemType.Medkit:
                case ItemType.Coin:
                case ItemType.SCP018:
                    itemOffset = new Vector3(0, .1f, 0);
                    break;

                case ItemType.SCP500:
                    itemOffset = new Vector3(0, .075f, 0);
                    break;

                case ItemType.SCP207:
                    itemOffset = new Vector3(0, .225f, 0);
                    rot = Quaternion.Euler(-90, 0, 0);
                    break;
            }

            if (hat.Scale != Vector3.one) scale = hat.Scale;
            if (hat.Position != Vector3.zero) itemOffset = hat.Position;
            if (!hat.Rotation.IsZero()) rot = hat.Rotation;
            if (hat.Scale != Vector3.one || hat.Position != Vector3.zero || !hat.Rotation.IsZero()) item = hat.Item;

            var itemObj = new Item(Server.Host.Inventory.CreateItemInstance(item, false)) { Scale = scale };

            var pickup = itemObj.Spawn(Vector3.zero, Quaternion.identity);

            SpawnHat(player, pickup, itemOffset, rot);
        }

        public static void SpawnHat(Player player, Pickup pickup, Vector3 posOffset, Quaternion rotOffset)
        {
            HatPlayerComponent playerComponent;

            if (!player.GameObject.TryGetComponent(out playerComponent))
            {
                playerComponent = player.GameObject.AddComponent<HatPlayerComponent>();
            }

            if (playerComponent.item != null)
            {
                UnityEngine.Object.Destroy(playerComponent.item.gameObject);
                playerComponent.item = null;
            }

            var rigidbody = pickup.Base.gameObject.GetComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            playerComponent.item = pickup.Base.gameObject.AddComponent<HatItemComponent>();
            playerComponent.item.item = pickup.Base;
            playerComponent.item.player = playerComponent;
            playerComponent.item.pos = Hats.Hats.GetHatPosForRole(player.Role);
            playerComponent.item.itemOffset = posOffset;
            playerComponent.item.rot = rotOffset;
        }

    }
}
