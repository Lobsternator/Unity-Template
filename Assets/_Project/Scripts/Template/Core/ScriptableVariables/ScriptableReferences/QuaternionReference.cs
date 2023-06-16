using System;
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Specialized ScriptableReference for <see cref="Quaternion"/>.
    /// </summary>
    [Serializable, CreateAssetMenu(fileName = "New QuaternionReference", menuName = "ScriptableReference/Quaternion")]
    public class QuaternionReference : ScriptableReference<Quaternion> { }
}
