using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serializable = System.SerializableAttribute;

namespace Template.Core
{
    [Serializable]
    public class HitstopSettings
    {
        [Min(0.0f)]
        public float durationMultiplier;
        public AnimationCurve timeScaleCurve;
    }

    [CreateAssetMenu(fileName = "new HitstopSettingsReference", menuName = "Time/HitstopSettings")]
    public class HitstopSettingsReference : ScriptableReference<HitstopSettings>
    {

    }
}
