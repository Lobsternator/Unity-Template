using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Physics
{
    [RequireComponent(typeof(Collider))]
    public class GroundedOverrideTargetedTrigger : MonoBehaviour
    {
        [field: SerializeField] public PhysicsChecker PhysicsChecker { get; private set; }
        [field: SerializeField] public bool InteractWithTriggers { get; set; } = false;
        [field: SerializeField] public ForceGroundedStateMode ForceGroundedState { get; private set; } = ForceGroundedStateMode.Either;

        private Dictionary<Collider, ForceGroundedStateMode> _touchingColliders = new Dictionary<Collider, ForceGroundedStateMode>();

        private void OnTriggerEnter(Collider other)
        {
            if (!PhysicsChecker || (other.isTrigger && !InteractWithTriggers) || !enabled)
                return;

            if (_touchingColliders.ContainsKey(other))
                _touchingColliders[other] = ForceGroundedState;
            else
                _touchingColliders.Add(other, ForceGroundedState);

            PhysicsChecker.AddForceGroundedStateTally(ForceGroundedState, 1);
        }
        private void OnTriggerExit(Collider other)
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

            List<Collider> collidersToRemove = new List<Collider>();
            foreach (var (collider, forceGroundedState) in _touchingColliders)
            {
                if (!collider || !collider.enabled || !collider.gameObject.activeInHierarchy)
                {
                    collidersToRemove.Add(collider);
                    PhysicsChecker.AddForceGroundedStateTally(forceGroundedState, -1);
                }
            }

            foreach (Collider collider in collidersToRemove)
                _touchingColliders.Remove(collider);
        }
    }
}
