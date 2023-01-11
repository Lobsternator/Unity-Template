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

        private Dictionary<(Collider, ForceGroundedStateTallyCounter), ForceGroundedStateMode> _touchingColliders = new Dictionary<(Collider, ForceGroundedStateTallyCounter), ForceGroundedStateMode>();

        private void OnTriggerEnter(Collider other)
        {
            if ((other.isTrigger && !InteractWithTriggers) || !enabled)
                return;

            ForceGroundedStateTallyCounter tallyCounter = other.GetComponent<ForceGroundedStateTallyCounter>();
            if (!tallyCounter)
                return;

            if (_touchingColliders.ContainsKey((other, tallyCounter)))
                _touchingColliders[(other, tallyCounter)] = ForceGroundedState;
            else
                _touchingColliders.Add((other, tallyCounter), ForceGroundedState);

            tallyCounter.AddForceGroundedStateTally(ForceGroundedState, 1);
        }
        private void OnTriggerExit(Collider other)
        {
            if ((other.isTrigger && !InteractWithTriggers) || !enabled)
                return;

            ForceGroundedStateTallyCounter tallyCounter = other.GetComponent<ForceGroundedStateTallyCounter>();
            if (!tallyCounter)
                return;

            if (_touchingColliders.ContainsKey((other, tallyCounter)))
            {
                tallyCounter.AddForceGroundedStateTally(_touchingColliders[(other, tallyCounter)], -1);
                _touchingColliders.Remove((other, tallyCounter));
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

            List<(Collider, ForceGroundedStateTallyCounter)> collidersToRemove = new List<(Collider, ForceGroundedStateTallyCounter)>();
            foreach (var (physicsCheckerColliderPair, forceGroundedState) in _touchingColliders)
            {
                Collider collider                           = physicsCheckerColliderPair.Item1;
                ForceGroundedStateTallyCounter tallyCounter = physicsCheckerColliderPair.Item2;

                if (!collider || !collider.enabled || !collider.gameObject.activeInHierarchy || !tallyCounter)
                {
                    collidersToRemove.Add((collider, tallyCounter));

                    if (tallyCounter)
                        tallyCounter.AddForceGroundedStateTally(forceGroundedState, -1);
                }
            }

            foreach (var colliderTallyCounterPair in collidersToRemove)
                _touchingColliders.Remove(colliderTallyCounterPair);
        }
    }
}
