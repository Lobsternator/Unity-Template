using System;
using System.Collections;
using System.Collections.Generic;
using Template.Core;
using UnityEngine;

namespace Template
{
    [Serializable, CreateAssetMenu(fileName = "New ObjectReference", menuName = "ScriptableReference/Object")]
    public class ObjectReference : ScriptableReference<UnityEngine.Object> { }
}
