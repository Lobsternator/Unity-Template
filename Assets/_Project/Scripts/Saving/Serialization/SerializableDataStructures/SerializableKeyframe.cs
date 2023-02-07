using UnityEngine;
using Serializable = System.SerializableAttribute;

namespace Template.Saving.Serialization
{
    [Serializable]
    public struct SerializableKeyframe
    {
        public float time;
        public float value;
        public float inTangent;
        public float outTangent;
        public float inWeight;
        public float outWeight;
        public WeightedMode weightedMode;

        public SerializableKeyframe(float time, float value, float inTangent, float outTangent, float inWeight, float outWeight, WeightedMode weightedMode)
        {
            this.time         = time;
            this.value        = value;
            this.inTangent    = inTangent;
            this.outTangent   = outTangent;
            this.inWeight     = inWeight;
            this.outWeight    = outWeight;
            this.weightedMode = weightedMode;
        }

        public static implicit operator SerializableKeyframe(Keyframe keyframe)
        {
            return new SerializableKeyframe()
            {
                time         = keyframe.time,
                value        = keyframe.value,
                inTangent    = keyframe.inTangent,
                outTangent   = keyframe.outTangent,
                inWeight     = keyframe.inWeight,
                outWeight    = keyframe.outWeight,
                weightedMode = keyframe.weightedMode,
            };
        }
        public static implicit operator Keyframe(SerializableKeyframe serializableKeyframe)
        {
            return new Keyframe()
            {
                time         = serializableKeyframe.time,
                value        = serializableKeyframe.value,
                inTangent    = serializableKeyframe.inTangent,
                outTangent   = serializableKeyframe.outTangent,
                inWeight     = serializableKeyframe.inWeight,
                outWeight    = serializableKeyframe.outWeight,
                weightedMode = serializableKeyframe.weightedMode,
            };
        }
    }
}
