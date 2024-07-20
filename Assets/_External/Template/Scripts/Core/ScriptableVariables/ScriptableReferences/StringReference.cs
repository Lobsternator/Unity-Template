using System;
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Specialized ScriptableReference for <see cref="string"/>.
    /// </summary>
    [Serializable, CreateAssetMenu(fileName = "New StringReference", menuName = "ScriptableReference/String")]
    public class StringReference : ScriptableReference<string> { }
}
