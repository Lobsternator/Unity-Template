using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Physics
{
    [DisallowMultipleComponent]
    public class GroundedOverrideProximityTrigger : MonoBehaviour, IContactEventReceiver
    {
        private struct OverrideContact : IEquatable<OverrideContact>
        {
            public Collider Collider { get; }
            public ForceGroundedStateTallyCounter ForceGroundedStateTallyCounter { get; }
            public ForceGroundedStateMode ForceGroundedState { get; set; }

            public OverrideContact(Collider collider, ForceGroundedStateTallyCounter forceGroundedStateTallyCounter, ForceGroundedStateMode forceGroundedState)
            {
                Collider                       = collider;
                ForceGroundedStateTallyCounter = forceGroundedStateTallyCounter;
                ForceGroundedState             = forceGroundedState;
            }

            public bool Equals(OverrideContact other)
            {
                return Collider == other.Collider && ForceGroundedStateTallyCounter == other.ForceGroundedStateTallyCounter && ForceGroundedState == other.ForceGroundedState;
            }
        }

        public ContactEventSender CurrentContactEventSender { get; set; }

        [SerializeField] private bool _ignoreTriggerOverlaps = true;
        public bool IgnoreTriggerOverlaps
        {
            get => _ignoreTriggerOverlaps;
            set => _ignoreTriggerOverlaps = value;
        }

#if UNITY_EDITOR
        [SerializeField] private ForceGroundedStateMode _oldForceGroundedState = ForceGroundedStateMode.Either;
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
                UpdateTallies();
            }
        }

        private List<OverrideContact> _overrideContacts = new List<OverrideContact>();

        public void UpdateTallies()
        {
            for (int i = 0; i < _overrideContacts.Count; i++)
            {
                OverrideContact overrideContact              = _overrideContacts[i];
                ForceGroundedStateTallyCounter tallyCounter  = overrideContact.ForceGroundedStateTallyCounter;
                ForceGroundedStateMode oldForceGroundedState = overrideContact.ForceGroundedState;

                if (tallyCounter && oldForceGroundedState != _forceGroundedState)
                {
                    tallyCounter.AddForceGroundedStateTally(oldForceGroundedState, -1);
                    tallyCounter.AddForceGroundedStateTally(_forceGroundedState, 1);

                    overrideContact.ForceGroundedState = _forceGroundedState;
                    _overrideContacts[i]               = overrideContact;
                }
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if ((other.isTrigger && IgnoreTriggerOverlaps) || !enabled)
                return;

            ForceGroundedStateTallyCounter tallyCounter = other.GetComponent<ForceGroundedStateTallyCounter>();
            if (!tallyCounter)
                return;

            _overrideContacts.Add(new OverrideContact(other, tallyCounter, ForceGroundedState));
            tallyCounter.AddForceGroundedStateTally(ForceGroundedState, 1);
        }
        public void OnTriggerExit(Collider other)
        {
            if ((other.isTrigger && IgnoreTriggerOverlaps) || !enabled)
                return;

            ForceGroundedStateTallyCounter tallyCounter = other.GetComponent<ForceGroundedStateTallyCounter>();
            if (!tallyCounter)
                return;

            int findIndex = _overrideContacts.IndexOf(new OverrideContact(other, tallyCounter, ForceGroundedState));
            if (findIndex != -1)
            {
                tallyCounter.AddForceGroundedStateTally(ForceGroundedState, -1);
                _overrideContacts.RemoveAt(findIndex);
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _overrideContacts.Count; i++)
            {
                ForceGroundedStateTallyCounter tallyCounter = _overrideContacts[i].ForceGroundedStateTallyCounter;
                if (tallyCounter)
                    tallyCounter.AddForceGroundedStateTally(ForceGroundedState, -1);
            }

            _overrideContacts.Clear();
        }

        private void Update()
        {
            if (_overrideContacts.Count == 0)
                return;

            for (int i = 0; i < _overrideContacts.Count; i++)
            {
                Collider collider                           = _overrideContacts[i].Collider;
                ForceGroundedStateTallyCounter tallyCounter = _overrideContacts[i].ForceGroundedStateTallyCounter;

                if (!collider || !collider.enabled || !collider.gameObject.activeInHierarchy || !tallyCounter)
                {
                    if (tallyCounter)
                        tallyCounter.AddForceGroundedStateTally(ForceGroundedState, -1);

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
            UpdateTallies();
        }
#endif
    }
}
