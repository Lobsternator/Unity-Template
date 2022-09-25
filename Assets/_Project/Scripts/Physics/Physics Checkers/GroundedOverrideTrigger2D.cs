using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Physics
{
    [RequireComponent(typeof(Collider2D))]
    public class GroundedOverrideTrigger2D : MonoBehaviour
    {
        [field: SerializeField] public PhysicsChecker2D PhysicsChecker { get; private set; }
        [field: SerializeField] public ForceGroundedStateMode ForceGroundedState { get; private set; } = ForceGroundedStateMode.Either;

        private HashSet<Collider2D> _touchingColliders = new HashSet<Collider2D>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            _touchingColliders.Add(other);
            if (!PhysicsChecker)
                return;

            PhysicsChecker.ForceGroundedState = ForceGroundedState;
        }
        private void OnTriggerExit2D(Collider2D other)
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
