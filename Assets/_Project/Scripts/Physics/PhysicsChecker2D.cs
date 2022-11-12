using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Core;

namespace Template.Physics
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PhysicsChecker2D : MonoBehaviour
    {
        private class CollisionInfo
        {
            public Vector2 contactNormal;

            public CollisionInfo(Collision2D collision)
            {
                contactNormal = collision.GetClosestContactNormal(Vector2.up);
            }
        }

        [field: Range(0.0f, 90.0f)]
        [field: Tooltip("When the ground steepness is below this threshold the object will be considered to be \"grounded\".")]
        [field: SerializeField] public float MaxGroundSteepness { get; set; } = 85.0f;

        [field: Tooltip("When the velocity is above this threshold the object will be considered to be \"moving\".")]
        [field: SerializeField] public float MinVelocity { get; set; } = 0.1f;

        public bool IsMoving { get; private set; }
        public bool IsGrounded { get; private set; }
        public Vector2 GroundNormal { get; private set; }  = Vector2.up;
        public Vector2 GroundTangent { get; private set; } = Vector2.right;

        public float GroundSteepness => 1.0f - Mathf.Clamp01(Vector2.Dot(GroundNormal, Vector2.up));

        public event Action StartedMoving;
        public event Action StoppedMoving;
        public event Action BecameGrounded;
        public event Action BecameAirborn;
        public event Action PhysicsFrameProcessed;

        public delegate ForceGroundedStateMode ForceGroundedStateModeFromTallyDelegate(int groundedTally, int airbornTally, int eitherTally);
        public ForceGroundedStateModeFromTallyDelegate ForceGroundedStateModeFromTallyCallback { get; set; }

        private Dictionary<ForceGroundedStateMode, int> _forceGroundedStateTallyCount = new Dictionary<ForceGroundedStateMode, int>();

        private bool ShouldBecomeGrounded
        {
            get
            {
                ForceGroundedStateMode forceGroundedState = GetForceGroundedState();

                return _touchingColliders.Count > 0                         &&
                       !IsGrounded                                          &&
                       _isBelowMaxSteepness                                 &&
                       forceGroundedState != ForceGroundedStateMode.Airborn ||
                       (!IsGrounded && forceGroundedState == ForceGroundedStateMode.Grounded);
            }
        }
        private bool ShouldBecomeAirborn
        {
            get
            {
                ForceGroundedStateMode forceGroundedState = GetForceGroundedState();

                return ((_touchingColliders.Count == 0 && IsGrounded)        ||
                       (IsGrounded && !_isBelowMaxSteepness))                &&
                       forceGroundedState != ForceGroundedStateMode.Grounded ||
                       ( IsGrounded && forceGroundedState == ForceGroundedStateMode.Airborn);
            }
        }

        private Rigidbody2D _rigidbody;
        private HashSet<Collider2D> _touchingColliders  = new HashSet<Collider2D>();
        private List<CollisionInfo> _collisionsToHandle = new List<CollisionInfo>();
        private bool _isBelowMaxSteepness;
        private bool _hasDoneInitialStateCheck;

        public PhysicsChecker2D()
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

            if (_hasDoneInitialStateCheck)
                UpdateGroundedState();
        }
        public void AddForceGroundedStateTally(ForceGroundedStateMode forceGroundedState, int tally)
        {
            _forceGroundedStateTallyCount[forceGroundedState] += tally;

            if (_hasDoneInitialStateCheck)
                UpdateGroundedState();
        }

        private void UpdateGroundedState()
        {
            ForceGroundedStateMode forceGroundedState = GetForceGroundedState();

            if (forceGroundedState == ForceGroundedStateMode.Grounded && !IsGrounded)
                OnBecameGrounded();
            else if (forceGroundedState == ForceGroundedStateMode.Airborn && IsGrounded)
                OnBecameAirborn();
            else if (forceGroundedState == ForceGroundedStateMode.Either)
            {
                if (ShouldBecomeGrounded)
                    OnBecameGrounded();
                else if (ShouldBecomeAirborn)
                    OnBecameAirborn();
            }
        }

        private void OnBecameGrounded()
        {
            IsGrounded = true;
            BecameGrounded?.Invoke();
        }
        private void OnBecameAirborn()
        {
            IsGrounded    = false;
            GroundNormal  = Vector2.up;
            GroundTangent = Vector2.right;
            BecameAirborn?.Invoke();
        }

        private void OnStartedMoving()
        {
            IsMoving = true;
            StartedMoving?.Invoke();
        }
        private void OnStoppedMoving()
        {
            IsMoving = false;
            StoppedMoving?.Invoke();
        }

        private void HandleCollisions()
        {
            Vector2 finalContactNormal = Vector2.zero;
            float finalSteepness       = float.MinValue;

            foreach (CollisionInfo collision in _collisionsToHandle)
            {
                float steepness = Vector2.Dot(collision.contactNormal, Vector2.up);

                if (steepness > finalSteepness)
                {
                    finalSteepness     = steepness;
                    finalContactNormal = collision.contactNormal;
                }
            }

            _isBelowMaxSteepness = finalSteepness > Mathf.Cos(MaxGroundSteepness * Mathf.Deg2Rad);
            if (_isBelowMaxSteepness)
            {
                GroundNormal  = finalContactNormal;
                GroundTangent = Vector3.Cross(finalContactNormal, Vector3.forward);
            }
            else
            {
                GroundNormal  = Vector2.up;
                GroundTangent = Vector2.right;
            }
        }

        private void CollisionChecking()
        {
            if (_collisionsToHandle.Count > 0)
                HandleCollisions();

            _collisionsToHandle.Clear();
        }
        private void GroundChecking()
        {
            _touchingColliders.RemoveWhere(c => !c || !c.enabled || !c.gameObject.activeInHierarchy);

            if (ShouldBecomeGrounded)
                OnBecameGrounded();

            else if (ShouldBecomeAirborn)
                OnBecameAirborn();
        }
        private void MovementChecking()
        {
            float velocitySqrMagnitude = _rigidbody.velocity.sqrMagnitude;

            if (velocitySqrMagnitude >= MinVelocity * MinVelocity && !IsMoving)
                OnStartedMoving();

            else if (velocitySqrMagnitude < MinVelocity * MinVelocity && IsMoving)
                OnStoppedMoving();
        }

        private IEnumerator InitialStateCheck()
        {
            yield return new WaitForFixedUpdate();

            ForceGroundedStateMode forceGroundedState = GetForceGroundedState();
            CollisionChecking();

            if (_touchingColliders.Count > 0 || forceGroundedState == ForceGroundedStateMode.Grounded)
                OnBecameGrounded();
            else
                OnBecameAirborn();

            float velocitySqrMagnitude = _rigidbody.velocity.sqrMagnitude;
            if (velocitySqrMagnitude >= MinVelocity * MinVelocity)
                OnStartedMoving();
            else
                OnStoppedMoving();

            _hasDoneInitialStateCheck = true;
            PhysicsFrameProcessed?.Invoke();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_hasDoneInitialStateCheck)
                UpdateGroundedState();
        }
#endif

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _touchingColliders.Add(collision.collider);
            _collisionsToHandle.Add(new CollisionInfo(collision));
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            _collisionsToHandle.Add(new CollisionInfo(collision));
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            _touchingColliders.Remove(collision.collider);
        }

        private void OnEnable()
        {
            _hasDoneInitialStateCheck = false;
            StartCoroutine(InitialStateCheck());
        }
        private void OnDisable()
        {
            StopCoroutine(nameof(InitialStateCheck));
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!_hasDoneInitialStateCheck)
                return;

            CollisionChecking();
            GroundChecking();
            MovementChecking();

            PhysicsFrameProcessed?.Invoke();
        }
    }
}
