using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Physics
{
    [RequireComponent(typeof(Collider2D))]
    public class GroundedOverrideTargetedTrigger2D : MonoBehaviour
    {
        [field: SerializeField] public PhysicsChecker2D PhysicsChecker { get; private set; }
        [field: SerializeField] public bool InteractWithTriggers { get; set; } = false;
        [field: SerializeField] public ForceGroundedStateMode ForceGroundedState { get; private set; } = ForceGroundedStateMode.Either;

        private Dictionary<Collider2D, ForceGroundedStateMode> _touchingColliders = new Dictionary<Collider2D, ForceGroundedStateMode>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!PhysicsChecker || (other.isTrigger && !InteractWithTriggers) || !enabled)
                return;

            if (_touchingColliders.ContainsKey(other))
                _touchingColliders[other] = ForceGroundedState;
            else
                _touchingColliders.Add(other, ForceGroundedState);

            PhysicsChecker.AddForceGroundedStateTally(ForceGroundedState, 1);
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!PhysicsChecker || (other.isTrigger && !InteractWithTriggers) || !enabled)
                return;

            if (_touchingColliders.ContainsKey(other))
            {
                PhysicsChecker.AddForceGroundedStateTally(_touchingColliders[other], -1);
                _touchingColliders.Remove(other);
            }
        }

        private void OnDisable()
        {
            foreach (var (collider, forceGroundedState) in _touchingColliders)
                PhysicsChecker.AddForceGroundedStateTally(forceGroundedState, -1);

            _touchingColliders.Clear();
        }
        private void OnDestroy()
        {
            foreach (var (collider, forceGroundedState) in _touchingColliders)
                PhysicsChecker.AddForceGroundedStateTally(forceGroundedState, -1);

            _touchingColliders.Clear();
        }

        private void FixedUpdate()
        {
            if (!PhysicsChecker)
            {
                _touchingColliders.Clear();
                return;
            }

            if (_touchingColliders.Count == 0)
                return;

            List<Collider2D> collidersToRemove = new List<Collider2D>();
            foreach (var (collider, forceGroundedState) in _touchingColliders)
            {
                if (!collider || !collider.enabled || !collider.gameObject.activeInHierarchy)
                {
                    collidersToRemove.Add(collider);
                    PhysicsChecker.AddForceGroundedStateTally(forceGroundedState, -1);
                }
            }

            foreach (Collider2D collider in collidersToRemove)
                _touchingColliders.Remove(collider);
        }
    }
}