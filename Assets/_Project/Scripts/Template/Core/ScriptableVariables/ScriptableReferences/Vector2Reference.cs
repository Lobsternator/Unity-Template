using System;
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Specialized ScriptableReference for <see cref="Vector2"/>.
    /// </summary>
    [Serializable, CreateAssetMenu(fileName = "New Vector2Reference", menuName = "ScriptableReference/Vector2")]
    public class Vector2Reference : ScriptableReference<Vector2> { }
}
