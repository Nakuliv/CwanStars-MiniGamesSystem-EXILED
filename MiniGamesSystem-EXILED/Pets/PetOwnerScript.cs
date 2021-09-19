using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MiniGamesSystem.Pets
{
    public class PetOwnerScript : MonoBehaviour
    {
        public List<PetType> SpawnedPets { get; } = new List<PetType>();
    }
}
