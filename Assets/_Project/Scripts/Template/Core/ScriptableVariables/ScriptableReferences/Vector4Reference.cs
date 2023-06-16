using System;
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Specialized ScriptableReference for <see cref="Vector4"/>.
    /// </summary>
    [Serializable, CreateAssetMenu(fileName = "New Vector4Reference", menuName = "ScriptableReference/Vector4")]
    public class Vector4Reference : ScriptableReference<Vector4> { }
}
