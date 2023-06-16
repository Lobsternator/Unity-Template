using System;
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Specialized ScriptableReference for <see cref="Vector3"/>.
    /// </summary>
    [Serializable, CreateAssetMenu(fileName = "New Vector3Reference", menuName = "ScriptableReference/Vector3")]
    public class Vector3Reference : ScriptableReference<Vector3> { }
}
