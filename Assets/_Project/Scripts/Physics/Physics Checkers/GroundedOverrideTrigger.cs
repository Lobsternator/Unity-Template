using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Physics
{
    [RequireComponent(typeof(Collider))]
    public class GroundedOverrideTrigger : MonoBehaviour
    {
        [field: SerializeField] public PhysicsChecker PhysicsChecker { get; private set; }
        [field: SerializeField] public ForceGroundedStateMode ForceGroundedState { get; private set; } = ForceGroundedStateMode.Either;

        private HashSet<Collider> _touchingColliders = new HashSet<Collider>();

        private void OnTriggerEnter(Collider other)
        {
            _touchingColliders.Add(other);
            if (!PhysicsChecker)
                return;

            PhysicsChecker.ForceGroundedState = ForceGroundedState;
        }
        private void OnTriggerExit(Collider other)
        {
            _touchingColliders.Remove(other);
            if (!PhysicsChecker)
                return;

            if (_touchingColliders.Count == 0)
                PhysicsChecker.ForceGroundedState = ForceGroundedStateMode.Either;
        }

        private void FixedUpdate()
        {
            if (!PhysicsChecker)
                return;

            if (_touchingColliders.Count > 0)
                PhysicsChecker.ForceGroundedState = ForceGroundedState;
            else
                PhysicsChecker.ForceGroundedState = ForceGroundedStateMode.Either;
        }
    }
}
