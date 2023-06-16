using System;
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Specialized ScriptableReference for <see cref="float"/>.
    /// </summary>
    [Serializable, CreateAssetMenu(fileName = "New FloatReference", menuName = "ScriptableReference/Float")]
    public class FloatReference : ScriptableReference<float> { }
}
