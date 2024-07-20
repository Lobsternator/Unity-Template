using System;
using UnityEngine;

namespace Template.Physics
{
    /// <summary>
    /// Wrapper around <see cref="Transform"/>. Used for shifting reference frames.
    /// </summary>
    [Serializable]
    public class WorldUpOverride
    {
        public Transform transform;

        public Vector3 up => transform ? transform.up : Vector3.up;
        public Vector3 right => transform ? transform.right : Vector3.right;
        public Vector3 forward => transform ? transform.forward : Vector3.forward;
    }
}
