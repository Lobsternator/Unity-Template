using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Physics
{
    [RequireComponent(typeof(Collider2D))]
    public class GroundedOverrideProximityTrigger2D : MonoBehaviour
    {
        [field: SerializeField] public bool InteractWithTriggers { get; set; } = false;
        [field: SerializeField] public ForceGroundedStateMode ForceGroundedState { get; private set; } = ForceGroundedStateMode.Either;

        private Dictionary<(Collider2D, ForceGroundedStateTallyCounter2D), ForceGroundedStateMode> _touchingColliders = new Dictionary<(Collider2D, ForceGroundedStateTallyCounter2D), ForceGroundedStateMode>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((other.isTrigger && !InteractWithTriggers) || !enabled)
                return;

            ForceGroundedStateTallyCounter2D tallyCounter = other.GetComponent<ForceGroundedStateTallyCounter2D>();
            if (!tallyCounter)
                return;

            if (_touchingColliders.ContainsKey((other, tallyCounter)))
                _touchingColliders[(other, tallyCounter)] = ForceGroundedState;
            else
                _touchingColliders.Add((other, tallyCounter), ForceGroundedState);

            tallyCounter.AddForceGroundedStateTally(ForceGroundedState, 1);
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if ((other.isTrigger && !InteractWithTriggers) || !enabled)
                return;

            ForceGroundedStateTallyCounter2D tallyCounter = other.GetComponent<ForceGroundedStateTallyCounter2D>();
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

            List<(Collider2D, ForceGroundedStateTallyCounter2D)> collidersToRemove = new List<(Collider2D, ForceGroundedStateTallyCounter2D)>();
            foreach (var (physicsCheckerColliderPair, forceGroundedState) in _touchingColliders)
            {
                Collider2D collider                           = physicsCheckerColliderPair.Item1;
                ForceGroundedStateTallyCounter2D tallyCounter = physicsCheckerColliderPair.Item2;

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
