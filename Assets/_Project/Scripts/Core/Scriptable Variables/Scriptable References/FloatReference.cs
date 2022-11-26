using System;
using UnityEngine;

namespace Template.Core
{
    [Serializable, CreateAssetMenu(fileName = "New FloatReference", menuName = "ScriptableReference/Float")]
    public class FloatReference : ScriptableReference<float> { }
}
