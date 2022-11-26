using System;
using UnityEngine;

namespace Template.Core
{
    [Serializable, CreateAssetMenu(fileName = "New BoolReference", menuName = "ScriptableReference/Bool")]
    public class BoolReference : ScriptableReference<bool> { }
}
