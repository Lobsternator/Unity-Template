using System;
using UnityEngine;

namespace Template.Core
{
    [Serializable, CreateAssetMenu(fileName = "New IntReference", menuName = "ScriptableReference/Int")]
    public class IntReference : ScriptableReference<int> { }
}
