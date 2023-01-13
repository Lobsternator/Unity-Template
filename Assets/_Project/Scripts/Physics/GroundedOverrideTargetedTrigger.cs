using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Physics
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider))]
    public class GroundedOverrideTargetedTrigger : MonoBehaviour
    {
        [field: SerializeField] public ForceGroundedStateTallyCounter TallyCounter { get; private set; }
        [field: SerializeField] public bool IgnoreTriggerOverlaps { get; set; } = true;
        [field: SerializeField] public ForceGroundedStateMode ForceGroundedState { get; private set; } = ForceGroundedStateMode.Either;

        private Dictionary<Collider, ForceGroundedStateMode> _touchingColliders = new Dictionary<Collider, ForceGroundedStateMode>();

        private void OnTriggerEnter(Collider other)
        {
            if (!TallyCounter || (other.isTrigger && IgnoreTriggerOverlaps) || !enabled)
                return;

            if (_touchingColliders.ContainsKey(other))
                _touchingColliders[other] = ForceGroundedState;
            else
                _touchingColliders.Add(other, ForceGroundedState);

            TallyCounter.AddForceGroundedStateTally(ForceGroundedState, 1);
        }
        private void OnTriggerExit(Collider other)
        {
            if (!TallyCounter || (other.isTrigger && IgnoreTriggerOverlaps) || !enabled)
                return;
            
            if (_touchingColliders.ContainsKey(other))
            {
                TallyCounter.AddForceGroundedStateTally(_touchingColliders[other], -1);
                _touchingColliders.Remove(other);
            }
        }

        private void OnDisable()
        {
            foreach (var (collider, forceGroundedState) in _touchingColliders)
                TallyCounter.AddForceGroundedStateTally(forceGroundedState, -1);

            _touchingColliders.Clear();
        }
        private void OnDestroy()
        {
            foreach (var (collider, forceGroundedState) in _touchingColliders)
                TallyCounter.AddForceGroundedStateTally(forceGroundedState, -1);

            _touchingColliders.Clear();
        }

        private void FixedUpdate()
        {
            if (!TallyCounter)
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
                    TallyCounter.AddForceGroundedStateTally(forceGroundedState, -1);
                }
            }

            foreach (Collider collider in collidersToRemove)
                _touchingColliders.Remove(collider);
        }
    }
}
