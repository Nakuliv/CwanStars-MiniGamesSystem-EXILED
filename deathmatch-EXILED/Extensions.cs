using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using MiniGamesSystem.Hats;
using Mirror;
using RemoteAdmin;
using UnityEngine;

namespace MiniGamesSystem
{
    public static class Extensions
    {
        internal static bool hasTag;
        internal static bool isHidden;
        public static Dictionary<Player, List<GameObject>> DumHubs = new Dictionary<Player, List<GameObject>>();

        public static void SpawnDummyModel(Player ply, Vector3 position, Quaternion rotation, RoleType role, float x, float y, float z, out int dummyIndex)
        {
            dummyIndex = 0;
            GameObject obj = Object.Instantiate(NetworkManager.singleton.playerPrefab);
            CharacterClassManager ccm = obj.GetComponent<CharacterClassManager>();
            if (ccm == null)
                Log.Error("CCM is null, this can cause problems!");
            ccm.CurClass = role;
            ccm.GodMode = true;
            obj.GetComponent<NicknameSync>().Network_myNickSync = " ";
            obj.GetComponent<NicknameSync>().Network_customPlayerInfoString = "<color=white>[</color><color=blue>Pet</color><color=white>]</color> Test\n<color=white>[</color><color=#ff7518>Właściciel</color><color=white>]</color> <color=green>Cwan</color><color=yellow>Stars</color>\n<color=white>[</color><color=#EFC01A>INFO</color><color=white>]</color> Przez najnowszy update gry pety aktulnie nie działają.";
            obj.GetComponent<QueryProcessor>().PlayerId = 9999;
            obj.GetComponent<QueryProcessor>().NetworkPlayerId = 9999;
            obj.transform.localScale = new Vector3(x, y, z);
            obj.GetComponent<AnimationController>().Network_curMoveState = (byte)PlayerMovementState.Walking;
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            NetworkServer.Spawn(obj);
            if (DumHubs.TryGetValue(ply, out List<GameObject> objs))
            {
                objs.Add(obj);
            }
            else
            {
                DumHubs.Add(ply, new List<GameObject>());
                DumHubs[ply].Add(obj);
                dummyIndex = DumHubs[ply].Count();
            }
            if (dummyIndex != 1)
                dummyIndex = objs.Count();
        }

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
                Object.Destroy(playerComponent.item.gameObject);
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
