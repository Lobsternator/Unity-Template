using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Physics
{
    [RequireComponent(typeof(Collider))]
    public class GroundedOverrideProximityTrigger : MonoBehaviour
    {
        [field: SerializeField] public bool InteractWithTriggers { get; set; } = false;
        [field: SerializeField] public ForceGroundedStateMode ForceGroundedState { get; private set; } = ForceGroundedStateMode.Either;

        private Dictionary<(Collider, PhysicsChecker), ForceGroundedStateMode> _touchingColliders = new Dictionary<(Collider, PhysicsChecker), ForceGroundedStateMode>();

        private void OnTriggerEnter(Collider other)
        {
            if ((other.isTrigger && !InteractWithTriggers) || !enabled)
                return;

            PhysicsChecker physicsChecker = other.GetComponent<PhysicsChecker>();
            if (!physicsChecker)
                return;

            if (_touchingColliders.ContainsKey((other, physicsChecker)))
                _touchingColliders[(other, physicsChecker)] = ForceGroundedState;
            else
                _touchingColliders.Add((other, physicsChecker), ForceGroundedState);

            physicsChecker.AddForceGroundedStateTally(ForceGroundedState, 1);
        }
        private void OnTriggerExit(Collider other)
        {
            if ((other.isTrigger && !InteractWithTriggers) || !enabled)
                return;

            PhysicsChecker physicsChecker = other.GetComponent<PhysicsChecker>();
            if (!physicsChecker)
                return;

            if (_touchingColliders.ContainsKey((other, physicsChecker)))
            {
                physicsChecker.AddForceGroundedStateTally(_touchingColliders[(other, physicsChecker)], -1);
                _touchingColliders.Remove((other, physicsChecker));
            }
        }

        private void OnDisable()
        {
            foreach (var (colliderPhysicsCheckerPair, forceGroundedState) in _touchingColliders)
                colliderPhysicsCheckerPair.Item2.AddForceGroundedStateTally(forceGroundedState, -1);

            _touchingColliders.Clear();
        }
        private void OnDestroy()
        {
            foreach (var (colliderPhysicsCheckerPair, forceGroundedState) in _touchingColliders)
                colliderPhysicsCheckerPair.Item2.AddForceGroundedStateTally(forceGroundedState, -1);

            _touchingColliders.Clear();
        }

        private void FixedUpdate()
        {
            if (_touchingColliders.Count == 0)
                return;

            List<(Collider, PhysicsChecker)> collidersToRemove = new List<(Collider, PhysicsChecker)>();
            foreach (var (physicsCheckerColliderPair, forceGroundedState) in _touchingColliders)
            {
                Collider collider             = physicsCheckerColliderPair.Item1;
                PhysicsChecker physicsChecker = physicsCheckerColliderPair.Item2;

                if (!collider || !collider.enabled || !collider.gameObject.activeInHierarchy || !physicsChecker)
                {
                    collidersToRemove.Add((collider, physicsChecker));

                    if (physicsChecker)
                        physicsChecker.AddForceGroundedStateTally(forceGroundedState, -1);
                }
            }

            foreach (var colliderPhysicsCheckerPair in collidersToRemove)
                _touchingColliders.Remove(colliderPhysicsCheckerPair);
        }
    }
}