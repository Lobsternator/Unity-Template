using System;
using UnityEngine;

namespace Template.Core
{
    [Serializable, CreateAssetMenu(fileName = "New Vector4Reference", menuName = "ScriptableReference/Vector4")]
    public class Vector4Reference : ScriptableReference<Vector4> { }
}
