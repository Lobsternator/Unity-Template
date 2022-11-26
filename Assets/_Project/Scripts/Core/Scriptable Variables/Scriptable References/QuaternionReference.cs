using System;
using UnityEngine;

namespace Template.Core
{
    [Serializable, CreateAssetMenu(fileName = "New QuaternionReference", menuName = "ScriptableReference/Quaternion")]
    public class QuaternionReference : ScriptableReference<Quaternion> { }
}
