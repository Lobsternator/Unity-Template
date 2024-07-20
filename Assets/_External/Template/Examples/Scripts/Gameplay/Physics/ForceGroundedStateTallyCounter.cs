using System;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Physics
{
    /// <summary>
    /// Tallies up how many <see cref="ForceGroundedStateMode"/>s have accumulated, and determines what the final <see cref="ForceGroundedStateMode"/> should be.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PhysicsChecker))]
    public class ForceGroundedStateTallyCounter : MonoBehaviour
    {
        public delegate ForceGroundedStateMode ForceGroundedStateModeFromTallyDelegate(int groundedTally, int airbornTally, int eitherTally);
        public ForceGroundedStateModeFromTallyDelegate ForceGroundedStateModeFromTallyCallback { get; set; }

        private PhysicsChecker _physicsChecker;
        private Dictionary<ForceGroundedStateMode, int> _forceGroundedStateTallyCount = new Dictionary<ForceGroundedStateMode, int>();

        public ForceGroundedStateTallyCounter()
        {
            ForceGroundedStateModeFromTallyCallback = DefaultForceGroundedStateFromTallyCallback;

            foreach (ForceGroundedStateMode forceGroundedStateMode in Enum.GetValues(typeof(ForceGroundedStateMode)))
                _forceGroundedStateTallyCount.Add(forceGroundedStateMode, 0);
        }

        public ForceGroundedStateMode DefaultForceGroundedStateFromTallyCallback(int groundedTally, int airbornTally, int eitherTally)
        {
            if (eitherTally > 0)
                return ForceGroundedStateMode.Either;

            else if (groundedTally > 0 && airbornTally > 0)
                return ForceGroundedStateMode.Either;

            else if (groundedTally > 0)
                return ForceGroundedStateMode.Grounded;

            else if (airbornTally > 0)
                return ForceGroundedStateMode.Airborn;

            else
                return ForceGroundedStateMode.Either;
        }

        public ForceGroundedStateMode GetForceGroundedState()
        {
            return ForceGroundedStateModeFromTallyCallback.Invoke(
                _forceGroundedStateTallyCount[ForceGroundedStateMode.Grounded],
                _forceGroundedStateTallyCount[ForceGroundedStateMode.Airborn],
                _forceGroundedStateTallyCount[ForceGroundedStateMode.Either]);
        }

        public int GetForceGroundedStateTally(ForceGroundedStateMode forceGroundedState)
        {
            return _forceGroundedStateTallyCount[forceGroundedState];
        }
        public void SetForceGroundedStateTally(ForceGroundedStateMode forceGroundedState, int tally)
        {
            _forceGroundedStateTallyCount[forceGroundedState] = tally;

            if (enabled)
                UpdateGroundedState();
        }
        public void AddForceGroundedStateTally(ForceGroundedStateMode forceGroundedState, int tally)
        {
            _forceGroundedStateTallyCount[forceGroundedState] += tally;

            if (enabled)
                UpdateGroundedState();
        }

        public void UpdateGroundedState()
        {
            _physicsChecker.ForceGroundedState = GetForceGroundedState();
        }

        private void OnEnable()
        {
            UpdateGroundedState();
        }

        private void Awake()
        {
            _physicsChecker = GetComponent<PhysicsChecker>();
        }
    }
}
