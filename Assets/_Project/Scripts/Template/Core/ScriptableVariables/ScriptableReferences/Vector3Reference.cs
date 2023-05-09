using System;
using UnityEngine;

namespace Template.Core
{
    [Serializable, CreateAssetMenu(fileName = "New Vector3Reference", menuName = "ScriptableReference/Vector3")]
    public class Vector3Reference : ScriptableReference<Vector3> { }
}
