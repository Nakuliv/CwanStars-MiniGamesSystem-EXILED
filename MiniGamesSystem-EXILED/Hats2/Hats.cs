﻿using Exiled.API.Features;
using Mirror;
using UnityEngine;

namespace MiniGamesSystem
{
    internal static class Hats
    {
        
        public static void SpawnHat(this Player p, ItemType item, bool force = true)
        {
            HatPlayerComponent playerComponent;
            
            if (!p.GameObject.TryGetComponent(out playerComponent))
            {
                playerComponent = p.GameObject.AddComponent<HatPlayerComponent>();
            }

            if (force && playerComponent.item != null)
            {
                Object.Destroy(playerComponent.item.gameObject);
                playerComponent.item = null;
            }

            if (item == ItemType.None) return;

            var pos = GetHatPosForRole(p.Role);
            var itemOffset = Vector3.zero;
            var rot = Quaternion.Euler(0, 0, 0);
            
            var gameObject = UnityEngine.Object.Instantiate<GameObject>(Server.Host.Inventory.pickupPrefab);
            
            switch (item)
            {
                case ItemType.KeycardScientist:
                    gameObject.transform.localScale += new Vector3(1.5f, 20f, 1.5f);
                    rot = Quaternion.Euler(0, 90, 0);
                    itemOffset = new Vector3(0, .1f, 0);
                    break;
                
                case ItemType.KeycardNTFCommander:
                    gameObject.transform.localScale += new Vector3(1.5f, 200f, 1.5f);
                    rot = Quaternion.Euler(0, 90, 0);
                    itemOffset = new Vector3(0, .9f, 0);
                    break;
                
                case ItemType.SCP268:
                    gameObject.transform.localScale += new Vector3(-.1f, -.1f, -.1f);
                    rot = Quaternion.Euler(-90, 0, 90);
                    break;
                
                case ItemType.Ammo556x45:
                    gameObject.transform.localScale += new Vector3(-.03f, -.03f, -.03f);
                    var position2 = gameObject.transform.position;
                    gameObject.transform.position = new Vector3(position2.x, position2.y, position2.z);
                    rot = Quaternion.Euler(-90, 0, 90);
                    item = ItemType.SCP268;
                    break;
                
                case ItemType.Ammo762x39:
                    gameObject.transform.localScale += new Vector3(-.1f, 10f, -.1f);
                    rot = Quaternion.Euler(-90, 0, 90);
                    item = ItemType.SCP268;
                    break;
                
                case ItemType.Ammo9x19:
                    gameObject.transform.localScale += new Vector3(-.1f, -.1f, 5f);
                    rot = Quaternion.Euler(-90, 0, -90);
                    itemOffset = new Vector3(0, -.15f, .1f);
                    item = ItemType.SCP268;
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
                    break;
            }

            NetworkServer.Spawn(gameObject);
            gameObject.GetComponent<Pickup>().SetupPickup(item, 0, Server.Host.Inventory.gameObject, new Pickup.WeaponModifiers(true, 0, 0, 0), p.CameraTransform.position+pos, p.CameraTransform.rotation * rot);
            
            var pickup = gameObject.GetComponent<Pickup>();

            var rigidbody = pickup.gameObject.GetComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            var collider = pickup.gameObject.GetComponent<Collider>();
            collider.enabled = false;

            playerComponent.item = pickup.gameObject.AddComponent<HatItemComponent>();
            playerComponent.item.player = playerComponent;
            playerComponent.item.pos = pos;
            playerComponent.item.itemOffset = itemOffset;
            playerComponent.item.rot = rot;
        }

        internal static Vector3 GetHatPosForRole(RoleType role)
        {
            switch (role)
            {
                case RoleType.Scp173:
                    return new Vector3(0, .7f, -.05f);
                case RoleType.Scp106:
                    return new Vector3(0, .45f, .13f);
                case RoleType.Scp096:
                    return new Vector3(.15f, .45f, .225f);
                case RoleType.Scp93953:
                    return new Vector3(0, -.5f, 1.125f);
                case RoleType.Scp93989:
                    return new Vector3(0, -.3f, 1.1f);
                case RoleType.Scp049:
                    return new Vector3(0, .125f, -.05f);
                case RoleType.None:
                    return new Vector3(-1000, -1000, -1000);
                case RoleType.Spectator:
                    return new Vector3(-1000, -1000, -1000);
                case RoleType.Scp0492:
                    return new Vector3(0, 0f, -.06f);
                default:
                    return new Vector3(0, .15f, -.07f);
            }
        }

        internal static void Reset()
        {
            foreach (var component in Object.FindObjectsOfType<HatPlayerComponent>())
            {
                if (component.item)
                {
                    Object.Destroy(component.item.gameObject);
                }

                Object.Destroy(component);
            }

            foreach (var component in Object.FindObjectsOfType<HatItemComponent>())
            {
                Object.Destroy(component.gameObject);
            }
        }
    }
}