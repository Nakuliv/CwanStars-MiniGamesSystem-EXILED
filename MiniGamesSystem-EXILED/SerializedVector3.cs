using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MiniGamesSystem
{
    [Serializable]
    public class SerializedVector3
    {
        public SerializedVector3(Vector3 vector)
        {
            X = vector.x;
            Y = vector.y;
            Z = vector.z;
        }

        public SerializedVector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public SerializedVector3() { }

        public Vector3 Parse() => new Vector3(X, Y, Z);

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public static implicit operator Vector3(SerializedVector3 vector) => vector.Parse();
        public static implicit operator SerializedVector3(Vector3 vector) => new SerializedVector3(vector);
    }
}
