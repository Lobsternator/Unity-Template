using System;
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Specialized ScriptableReference for <see cref="UnityEngine.Object"/>.
    /// </summary>
    [Serializable, CreateAssetMenu(fileName = "New ObjectReference", menuName = "ScriptableReference/Object")]
    public class ObjectReference : ScriptableReference<UnityEngine.Object> { }
}
