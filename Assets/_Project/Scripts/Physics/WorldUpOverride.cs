using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Physics
{
    [Serializable]
    public class WorldUpOverride
    {
        public Transform transform;

        public Vector3 up => transform ? transform.up : Vector3.up;
        public Vector3 right => transform ? transform.right : Vector3.right;
        public Vector3 forward => transform ? transform.forward : Vector3.forward;
    }
}
