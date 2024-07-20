using UnityEngine;
using Serializable = System.SerializableAttribute;

namespace Template.Saving.Serialization
{
    /// <summary>
    /// Serializable version of <see cref="Vector4"/>.
    /// </summary>
    [Serializable]
    public struct SerializableVector4
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public SerializableVector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static implicit operator SerializableVector4(Vector4 vector)
        {
            return new SerializableVector4()
            {
                x = vector.x,
                y = vector.y,
                z = vector.z,
                w = vector.w,
            };
        }

        public static implicit operator Vector4(SerializableVector4 serializableVector)
        {
            return new SerializableVector4()
            {
                x = serializableVector.x,
                y = serializableVector.y,
                z = serializableVector.z,
                w = serializableVector.w,
            };
        }
    }
}
