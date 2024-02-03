using UnityEngine;
using Serializable = System.SerializableAttribute;

namespace Template.Core
{
    /// <summary>
    /// Stores various settings related to <see cref="Hitstop"/>s.
    /// </summary>
    [Serializable]
    public class HitstopSettings
    {
        [Min(0.0f)]
        public float durationMultiplier;
        public AnimationCurve timeScaleCurve;
    }

    /// <summary>
    /// <see cref="ScriptableReference{TValue}"/> wrapper around <see cref="HitstopSettings"/>
    /// </summary>
    [CreateAssetMenu(fileName = "new HitstopSettingsReference", menuName = "Gameplay/HitstopSettings")]
    public class HitstopSettingsReference : ScriptableReference<HitstopSettings>
    {

    }
}
