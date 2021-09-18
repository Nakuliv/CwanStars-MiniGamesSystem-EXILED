using MEC;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Exiled.API.Features;
using DummyAPI;

namespace MiniGamesSystem.Pets
{
    public class Pet : Dummy
    {
        public static bool SpawnPet(Player owner, string Nick, PetType type, out Pet pet)
        {
            pet = null;
            PetOwnerScript pos;
            if (!owner.GameObject.TryGetComponent(out pos))
            {
                owner.GameObject.AddComponent<PetOwnerScript>();
            }
            if(type == PetType.Custom)
            {
                if (owner.GetPetOwnerScript().SpawnedPets.Contains(type))
                    pet.Despawn();
            }
            if (owner.GetPetOwnerScript().SpawnedPets.Contains(type)) return false;

            pet = new Pet(owner, Nick, type);

            return true;
        }

        public Player Owner { get; }

        //public Pet(Player player, string Nick) : this(player, Nick) { }

        public Pet(Player player, string Nick, PetType Type) :
            base(player.Position, player.GameObject.transform.localRotation)
        {
            player.GetPetOwnerScript().SpawnedPets.Add(Type);
            switch (Type)
            {
                case PetType.Amogus:
                    Player.ClassManager().CurClass = RoleType.ClassD;
                    if (Handler.pInfoDict[player.UserId].custompetSize != new Vector3(1, 1, 1))
                    {
                        Player.Scale = new Vector3(1f, 0.5f, 1f);
                    }
                    Player.RankName = "Pet";
                    Player.RankColor = "yellow";
                    break;
                case PetType.Doggo:
                    Player.ClassManager().CurClass = RoleType.Scp93953;
                    if (Handler.pInfoDict[player.UserId].custompetSize != new Vector3(1, 1, 1))
                    {
                        Player.Scale = new Vector3(0.5f, 0.5f, 0.5f);
                    }
                    Player.RankName = "Pet";
                    Player.RankColor = "yellow";
                    break;
                case PetType.Custom:
                    Player.ClassManager().CurClass = Handler.pInfoDict[player.UserId].custompetClass;
                    if (Handler.pInfoDict[player.UserId].custompetSize != new Vector3(1, 1, 1))
                    {
                        Player.Scale = Handler.pInfoDict[player.UserId].custompetSize;
                    }
                    //Player.CurrentItem = new Exiled.API.Features.Items.Item(Handler.pInfoDict[player.UserId].custompetItem);
                    Player.RankName = "Custom Pet";
                    Player.RankColor = "yellow";
                    break;
            }
            Owner = player;
            Player.IsGodModeEnabled = false;
            Player.Health = 150;
            Player.CustomInfo = $"<color=white>[</color><color=blue>Nazwa</color><color=white>]</color> {Nick}\n<color=white>[</color><color=#ff7518>Właściciel</color><color=white>]</color> <color=green>{Owner.Nickname}</color>\n<color=white>[</color><color=#EFC01A>Typ</color><color=white>]</color> <color=brown>{Type}</color>";

            Player.RemoveDisplayInfo(PlayerInfoArea.Nickname);
            Player.RemoveDisplayInfo(PlayerInfoArea.Role);
            Player.RemoveDisplayInfo(PlayerInfoArea.PowerStatus);
            Player.RemoveDisplayInfo(PlayerInfoArea.UnitName);

            Movement = PlayerMovementState.Sprinting;
            Timing.RunCoroutine(Walk());
        }

        private IEnumerator<float> Walk()
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(0.1f);

                if (Owner == null) Destroy();
                if (GameObject == null) yield break;
                RotateToPosition(Owner.Position);

                var distance = Vector3.Distance(Owner.Position, Position);

                if ((PlayerMovementState)Owner.AnimationController().Network_curMoveState == PlayerMovementState.Sneaking) Movement = PlayerMovementState.Sneaking;
                else Movement = PlayerMovementState.Sprinting;

                if (Movement == PlayerMovementState.Sneaking)
                {
                    if (distance > 5f) Position = Owner.Position;

                    else if (distance > 1f) Direction = MovementDirection.Forward;

                    else if (distance <= 1f) Direction = MovementDirection.Stop;

                    continue;
                }

                if (distance > 10f)
                    Position = Owner.Position;

                else if (distance > 2f)
                    Direction = MovementDirection.Forward;

                else if (distance <= 1.25f)
                    Direction = MovementDirection.Stop;

            }
        }
    }
}