using System;
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Specialized ScriptableReference for <see cref="int"/>.
    /// </summary>
    [Serializable, CreateAssetMenu(fileName = "New IntReference", menuName = "ScriptableReference/Int")]
    public class IntReference : ScriptableReference<int> { }
}
