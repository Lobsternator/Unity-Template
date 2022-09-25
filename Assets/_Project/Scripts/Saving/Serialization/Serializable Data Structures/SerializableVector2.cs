using UnityEngine;
using Serializable = System.SerializableAttribute;

namespace Template.Saving.Serialization
{
    [Serializable]
    public struct SerializableVector2
    {
        public float x;
        public float y;

        public SerializableVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator SerializableVector2(Vector2 vector)
        {
            return new SerializableVector2()
            {
                x = vector.x,
                y = vector.y,
            };
        }
        public static implicit operator Vector2(SerializableVector2 serializableVector)
        {
            return new Vector2()
            {
                x = serializableVector.x,
                y = serializableVector.y,
            };
        }
    }
}
