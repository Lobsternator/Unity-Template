using System;
using UnityEngine;

namespace Template.Core
{
    [Serializable, CreateAssetMenu(fileName = "New StringReference", menuName = "ScriptableReference/String")]
    public class StringReference : ScriptableReference<string> { }
}
