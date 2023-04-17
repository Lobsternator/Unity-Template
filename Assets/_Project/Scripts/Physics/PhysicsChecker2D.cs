using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Core;

namespace Template.Physics
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D), typeof(ContactChecker2D))]
    public class PhysicsChecker2D : MonoBehaviour, IContactEventReceiver2D
    {
        private class CollisionInfo
        {
            public Vector2 ContactNormal { get; }

            public CollisionInfo(Collision2D collision, Vector2 worldUp)
            {
                ContactNormal = collision.GetClosestContactNormal(worldUp);
            }
        }

        public ContactEventSender2D CurrentContactEventSender { get; set; }

        [field: Range(0.0f, 90.0f)]
        [field: Tooltip("When the ground steepness is below this threshold the object will be considered to be \"grounded\".")]
        [field: SerializeField] public float MaxGroundSteepness { get; set; } = 85.0f;

        [field: Tooltip("When the velocity is above this threshold the object will be considered to be \"moving\".")]
        [field: SerializeField] public float MinVelocity { get; set; } = 0.1f;

        [field: SerializeField] public WorldUpOverride WorldUpOverride { get; set; } = new WorldUpOverride();

        public bool IsMoving { get; private set; }   = false;
        public bool IsGrounded { get; private set; } = false;
        public Vector2 GroundNormal { get; private set; }  = Vector2.up;
        public Vector2 GroundTangent { get; private set; } = Vector2.right;
        public bool HasDoneInitialStateCheck { get; private set; } = false;

        public float GroundSteepness => 1.0f - Mathf.Clamp01(Vector2.Dot(GroundNormal, WorldUpOverride.up));

        private ForceGroundedStateMode _forceGroundedState = ForceGroundedStateMode.Either;
        public ForceGroundedStateMode ForceGroundedState
        {
            get => _forceGroundedState;
            set
            {
                if (_forceGroundedState == value)
                    return;

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
                int touchingColliderCount = _contactChecker.Contacts.Count((c) => c.ContactType == ContactType.Collision);

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
                int touchingColliderCount = _contactChecker.Contacts.Count((c) => c.ContactType == ContactType.Collision);

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
            if (!HasDoneInitialStateCheck)
                return;
            
            _contactChecker.ClearDeadContacts();

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
            GroundNormal  = WorldUpOverride.up;
            GroundTangent = WorldUpOverride.right;
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
                float steepness = Vector2.Dot(collision.ContactNormal, WorldUpOverride.up);

                if (steepness > finalSteepness)
                {
                    finalSteepness     = steepness;
                    finalContactNormal = collision.ContactNormal;
                }
            }

            _isBelowMaxSteepness = finalSteepness > Mathf.Cos(MaxGroundSteepness * Mathf.Deg2Rad);
            if (_isBelowMaxSteepness)
            {
                GroundNormal  = finalContactNormal;
                GroundTangent = Vector3.Cross(finalContactNormal, WorldUpOverride.forward);
            }
            else
            {
                GroundNormal  = WorldUpOverride.up;
                GroundTangent = WorldUpOverride.right;
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
            yield return CoroutineUtility.WaitForFixedFrames(1);

            _contactChecker.ClearDeadContacts();
            int touchingColliderCount = _contactChecker.Contacts.Count((c) => c.ContactType == ContactType.Collision);

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
            UpdateGroundedState();
        }
#endif

        public void OnCollisionEnter2D(Collision2D collision)
        {
            _collisionsToHandle.Add(new CollisionInfo(collision, WorldUpOverride.up));
        }
        public void OnCollisionStay2D(Collision2D collision)
        {
            _collisionsToHandle.Add(new CollisionInfo(collision, WorldUpOverride.up));
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
