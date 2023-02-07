using UnityEngine;
using Serializable = System.SerializableAttribute;

namespace Template.Saving.Serialization
{
    [Serializable]
    public struct SerializableQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public SerializableQuaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static implicit operator SerializableQuaternion(Quaternion quaternion)
        {
            return new SerializableQuaternion()
            {
                x = quaternion.x,
                y = quaternion.y,
                z = quaternion.z,
                w = quaternion.w,
            };
        }
        public static implicit operator Quaternion(SerializableQuaternion serializableQuaternion)
        {
            return new Quaternion()
            {
                x = serializableQuaternion.x,
                y = serializableQuaternion.y,
                z = serializableQuaternion.z,
                w = serializableQuaternion.w,
            };
        }
    }
}
