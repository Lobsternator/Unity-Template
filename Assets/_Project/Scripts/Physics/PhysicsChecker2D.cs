using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Core;

namespace Template.Physics
{
    [RequireComponent(typeof(ContactChecker2D))]
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

        public bool IsMoving { get; private set; }   = false;
        public bool IsGrounded { get; private set; } = false;
        public Vector2 GroundNormal { get; private set; }  = Vector2.up;
        public Vector2 GroundTangent { get; private set; } = Vector2.right;
        public bool HasDoneInitialStateCheck { get; private set; } = false;

        public float GroundSteepness => 1.0f - Mathf.Clamp01(Vector2.Dot(GroundNormal, Vector2.up));

        private ForceGroundedStateMode _forceGroundedState = ForceGroundedStateMode.Either;
        public ForceGroundedStateMode ForceGroundedState
        {
            get => _forceGroundedState;
            set
            {
                _forceGroundedState = value;

                if (HasDoneInitialStateCheck)
                    UpdateGroundedState();
            }
        }

        public event Action StartedMoving;
        public event Action StoppedMoving;
        public event Action BecameGrounded;
        public event Action BecameAirborn;
        public event Action PhysicsFrameProcessed;

        private bool ShouldBecomeGrounded
        {
            get
            {
                int touchingColliderCount = _contactChecker.TouchingColliders.Count((c) => !c.isTrigger);

                return touchingColliderCount > 0                             &&
                       !IsGrounded                                           &&
                       _isBelowMaxSteepness                                  &&
                       _forceGroundedState != ForceGroundedStateMode.Airborn ||
                       (!IsGrounded && _forceGroundedState == ForceGroundedStateMode.Grounded);
            }
        }
        private bool ShouldBecomeAirborn
        {
            get
            {
                int touchingColliderCount = _contactChecker.TouchingColliders.Count((c) => !c.isTrigger);

                return ((touchingColliderCount == 0 && IsGrounded)            ||
                       (IsGrounded && !_isBelowMaxSteepness))                 &&
                       _forceGroundedState != ForceGroundedStateMode.Grounded ||
                       (IsGrounded && _forceGroundedState == ForceGroundedStateMode.Airborn);
            }
        }

        private Rigidbody2D _rigidbody;
        private ContactChecker2D _contactChecker;
        private List<CollisionInfo> _collisionsToHandle = new List<CollisionInfo>();
        private bool _isBelowMaxSteepness;

        public void UpdateGroundedState()
        {
            if (_forceGroundedState == ForceGroundedStateMode.Grounded && !IsGrounded)
                OnBecameGrounded();
            else if (_forceGroundedState == ForceGroundedStateMode.Airborn && IsGrounded)
                OnBecameAirborn();
            else if (_forceGroundedState == ForceGroundedStateMode.Either)
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

            int touchingColliderCount = _contactChecker.TouchingColliders.Count((c) => !c.isTrigger);

            CollisionChecking();

            if (touchingColliderCount > 0 || _forceGroundedState == ForceGroundedStateMode.Grounded)
                OnBecameGrounded();
            else
                OnBecameAirborn();

            float velocitySqrMagnitude = _rigidbody.velocity.sqrMagnitude;
            if (velocitySqrMagnitude >= MinVelocity * MinVelocity)
                OnStartedMoving();
            else
                OnStoppedMoving();

            HasDoneInitialStateCheck = true;
            PhysicsFrameProcessed?.Invoke();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (HasDoneInitialStateCheck)
                UpdateGroundedState();
        }
#endif

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _collisionsToHandle.Add(new CollisionInfo(collision));
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            _collisionsToHandle.Add(new CollisionInfo(collision));
        }

        private void OnEnable()
        {
            HasDoneInitialStateCheck = false;
            StartCoroutine(InitialStateCheck());
        }
        private void OnDisable()
        {
            StopCoroutine(nameof(InitialStateCheck));
        }

        private void Awake()
        {
            _rigidbody      = GetComponent<Rigidbody2D>();
            _contactChecker = GetComponent<ContactChecker2D>();
        }

        private void FixedUpdate()
        {
            if (!HasDoneInitialStateCheck)
                return;

            CollisionChecking();
            GroundChecking();
            MovementChecking();

            PhysicsFrameProcessed?.Invoke();
        }
    }
}
