using System;
using UnityEngine;

namespace Template.Core
{
    [Serializable, CreateAssetMenu(fileName = "New ObjectReference", menuName = "ScriptableReference/Object")]
    public class ObjectReference : ScriptableReference<UnityEngine.Object> { }
}
