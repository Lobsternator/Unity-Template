using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Physics
{
    [RequireComponent(typeof(PhysicsChecker2D))]
    public class ForceGroundedStateTallyCounter2D : MonoBehaviour
    {
        public delegate ForceGroundedStateMode ForceGroundedStateModeFromTallyDelegate(int groundedTally, int airbornTally, int eitherTally);
        public ForceGroundedStateModeFromTallyDelegate ForceGroundedStateModeFromTallyCallback { get; set; }

        private PhysicsChecker2D _physicsChecker;
        private Dictionary<ForceGroundedStateMode, int> _forceGroundedStateTallyCount = new Dictionary<ForceGroundedStateMode, int>();

        public ForceGroundedStateTallyCounter2D()
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
            UpdateGroundedState();
        }
        public void AddForceGroundedStateTally(ForceGroundedStateMode forceGroundedState, int tally)
        {
            _forceGroundedStateTallyCount[forceGroundedState] += tally;
            UpdateGroundedState();
        }

        public void UpdateGroundedState()
        {
            _physicsChecker.ForceGroundedState = GetForceGroundedState();
        }

        private void Awake()
        {
            _physicsChecker = GetComponent<PhysicsChecker2D>();
        }
    }
}
