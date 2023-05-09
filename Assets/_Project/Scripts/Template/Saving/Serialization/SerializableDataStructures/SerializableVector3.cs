using UnityEngine;
using Serializable = System.SerializableAttribute;

namespace Template.Saving.Serialization
{
    [Serializable]
    public struct SerializableVector3
    {
        public float x;
        public float y;
        public float z;

        public SerializableVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator SerializableVector3(Vector3 vector)
        {
            return new SerializableVector3()
            {
                x = vector.x,
                y = vector.y,
                z = vector.z,
            };
        }
        public static implicit operator Vector3(SerializableVector3 serializableVector)
        {
            return new Vector3()
            {
                x = serializableVector.x,
                y = serializableVector.y,
                z = serializableVector.z,
            };
        }
    }
}
