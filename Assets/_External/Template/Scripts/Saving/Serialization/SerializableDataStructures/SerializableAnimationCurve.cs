using System.Linq;
using UnityEngine;
using Serializable = System.SerializableAttribute;

namespace Template.Saving.Serialization
{
    /// <summary>
    /// Serializable version of <see cref="AnimationCurve"/>.
    /// </summary>
    [Serializable]
    public struct SerializableAnimationCurve
    {
        public SerializableKeyframe[] keys;
        public WrapMode preWrapMode;
        public WrapMode postWrapMode;

        public SerializableAnimationCurve(SerializableKeyframe[] keys, WrapMode preWrapMode, WrapMode postWrapMode)
        {
            this.keys         = keys;
            this.preWrapMode  = preWrapMode;
            this.postWrapMode = postWrapMode;
        }

        public static implicit operator SerializableAnimationCurve(AnimationCurve animationCurve)
        {
            return new SerializableAnimationCurve()
            {
                keys         = animationCurve.keys.Cast<SerializableKeyframe>().ToArray(),
                preWrapMode  = animationCurve.preWrapMode,
                postWrapMode = animationCurve.postWrapMode,
            };
        }
        public static implicit operator AnimationCurve(SerializableAnimationCurve serializableAnimationCurve)
        {
            return new AnimationCurve()
            {
                keys         = serializableAnimationCurve.keys.Cast<Keyframe>().ToArray(),
                preWrapMode  = serializableAnimationCurve.preWrapMode,
                postWrapMode = serializableAnimationCurve.postWrapMode,
            };
        }
    }
}
