using System;
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Specialized ScriptableReference for <see cref="bool"/>.
    /// </summary>
    [Serializable, CreateAssetMenu(fileName = "New BoolReference", menuName = "ScriptableReference/Bool")]
    public class BoolReference : ScriptableReference<bool> { }
}
