using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Physics
{
    [DisallowMultipleComponent]
    public class GroundedOverrideTargetedTrigger2D : MonoBehaviour, IContactEventReceiver2D
    {
        private struct OverrideContact : IEquatable<OverrideContact>
        {
            public Collider2D Collider { get; }
            public ForceGroundedStateMode ForceGroundedState { get; set; }

            public OverrideContact(Collider2D collider, ForceGroundedStateMode forceGroundedState)
            {
                Collider           = collider;
                ForceGroundedState = forceGroundedState;
            }

            public bool Equals(OverrideContact other)
            {
                return Collider == other.Collider && ForceGroundedState == other.ForceGroundedState;
            }
        }

        public ContactEventSender2D ActiveSender { get; set; }

        [field: SerializeField] public ForceGroundedStateTallyCounter2D TallyCounter { get; private set; }
        [field: SerializeField] public bool IgnoreTriggerOverlaps { get; set; } = true;

#if UNITY_EDITOR
        [SerializeField, HideInInspector] private ForceGroundedStateMode _oldForceGroundedState = ForceGroundedStateMode.Either;
#endif

        [SerializeField] private ForceGroundedStateMode _forceGroundedState = ForceGroundedStateMode.Either;
        public ForceGroundedStateMode ForceGroundedState
        {
            get => _forceGroundedState;
            set
            {
                if (value == _forceGroundedState)
                    return;

                _forceGroundedState = value;
                UpdateTally();
            }
        }

        private List<OverrideContact> _overrideContacts = new List<OverrideContact>();

        public void UpdateTally()
        {
            for (int i = 0; i < _overrideContacts.Count; i++)
            {
                OverrideContact overrideContact              = _overrideContacts[i];
                ForceGroundedStateMode oldForceGroundedState = overrideContact.ForceGroundedState;

                if (TallyCounter && oldForceGroundedState != _forceGroundedState)
                {
                    TallyCounter.AddForceGroundedStateTally(oldForceGroundedState, -1);
                    TallyCounter.AddForceGroundedStateTally(_forceGroundedState, 1);

                    overrideContact.ForceGroundedState = _forceGroundedState;
                    _overrideContacts[i]               = overrideContact;
                }
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!TallyCounter || (other.isTrigger && IgnoreTriggerOverlaps))
                return;

            _overrideContacts.Add(new OverrideContact(other, ForceGroundedState));
            TallyCounter.AddForceGroundedStateTally(ForceGroundedState, 1);
        }
        public void OnTriggerExit2D(Collider2D other)
        {
            if (!TallyCounter || (other.isTrigger && IgnoreTriggerOverlaps))
                return;

            int findIndex = _overrideContacts.IndexOf(new OverrideContact(other, ForceGroundedState));
            if (findIndex != -1)
            {
                TallyCounter.AddForceGroundedStateTally(ForceGroundedState, -1);
                _overrideContacts.RemoveAt(findIndex);
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _overrideContacts.Count; i++)
            {
                if (TallyCounter)
                    TallyCounter.AddForceGroundedStateTally(ForceGroundedState, -1);
            }

            _overrideContacts.Clear();
        }

        private void Update()
        {
            if (_overrideContacts.Count == 0)
                return;

            if (!TallyCounter)
            {
                _overrideContacts.Clear();
                return;
            }

            for (int i = 0; i < _overrideContacts.Count; i++)
            {
                Collider2D collider = _overrideContacts[i].Collider;

                if (!collider || !collider.enabled || !collider.gameObject.activeInHierarchy)
                {
                    TallyCounter.AddForceGroundedStateTally(ForceGroundedState, -1);
                    _overrideContacts.RemoveAt(i--);
                }
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_forceGroundedState == _oldForceGroundedState)
                return;

            _oldForceGroundedState = _forceGroundedState;
            UpdateTally();
        }
#endif
    }
}
