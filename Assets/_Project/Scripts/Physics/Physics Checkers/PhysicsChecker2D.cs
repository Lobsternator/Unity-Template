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
        private class ForceGroundedStateTallyCountContainer
        {
            public int groundedTally;
            public int airbornTally;
            public int eitherTally;
        }

        [field: Range(0.0f, 90.0f)]
        [field: Tooltip("When the ground steepness is below this threshold the object will be considered to be \"grounded\".")]
        [field: SerializeField] public float MaxGroundSteepness { get; set; }  = 85.0f;

        [field: Tooltip("When the velocity is above this threshold the object will be considered to be \"moving\".")]
        [field: SerializeField] public float MinVelocity { get; set; }         = 0.1f;

        private ForceGroundedStateTallyCountContainer _forceGroundedStateTallyCount = new ForceGroundedStateTallyCountContainer();
        private ForceGroundedStateMode _cachedForceGroundedState                    = ForceGroundedStateMode.Either;
        public ForceGroundedStateMode ForceGroundedState
        {
            get
            {
                if (_forceGroundedStateTallyCount.eitherTally > 0)
                    return ForceGroundedStateMode.Either;

                else if (_forceGroundedStateTallyCount.groundedTally > 0 && _forceGroundedStateTallyCount.airbornTally > 0)
                    return ForceGroundedStateMode.Either;

                else if (_forceGroundedStateTallyCount.groundedTally > 0)
                    return ForceGroundedStateMode.Grounded;

                else if (_forceGroundedStateTallyCount.airbornTally > 0)
                    return ForceGroundedStateMode.Airborn;

                else
                    return ForceGroundedStateMode.Either;
            }
        }

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

        private bool ShouldBecomeGrounded => _touchingColliders.Count > 0 && !IsGrounded && _isBelowMaxSteepness                      && _cachedForceGroundedState != ForceGroundedStateMode.Airborn  || (!IsGrounded && _cachedForceGroundedState == ForceGroundedStateMode.Grounded);
        private bool ShouldBecomeAirborn  => ((_touchingColliders.Count == 0 && IsGrounded) || (IsGrounded && !_isBelowMaxSteepness)) && _cachedForceGroundedState != ForceGroundedStateMode.Grounded || ( IsGrounded && _cachedForceGroundedState == ForceGroundedStateMode.Airborn);

        private Rigidbody2D _rigidbody;
        private HashSet<Collider2D> _touchingColliders      = new HashSet<Collider2D>();
        private List<FrozenCollision2D> _collisionsToHandle = new List<FrozenCollision2D>();
        private bool _isBelowMaxSteepness;
        private bool _hasDoneInitialStateCheck;

        public int GetForceGroundedStateTally(ForceGroundedStateMode forceGroundedState)
        {
            if (forceGroundedState == ForceGroundedStateMode.Grounded)
                return _forceGroundedStateTallyCount.groundedTally;
            else if (forceGroundedState == ForceGroundedStateMode.Airborn)
                return _forceGroundedStateTallyCount.airbornTally;
            else if (forceGroundedState == ForceGroundedStateMode.Either)
                return _forceGroundedStateTallyCount.eitherTally;
            else
                throw new NotImplementedException();
        }
        public void SetForceGroundedStateTally(ForceGroundedStateMode forceGroundedState, int tally)
        {
            if (forceGroundedState == ForceGroundedStateMode.Grounded)
                _forceGroundedStateTallyCount.groundedTally = tally;
            else if (forceGroundedState == ForceGroundedStateMode.Airborn)
                _forceGroundedStateTallyCount.airbornTally  = tally;
            else if (forceGroundedState == ForceGroundedStateMode.Either)
                _forceGroundedStateTallyCount.eitherTally   = tally;
            else
                throw new NotImplementedException();

            if (_hasDoneInitialStateCheck)
                UpdateGroundedState();
        }
        public void AddForceGroundedStateTally(ForceGroundedStateMode forceGroundedState, int tally)
        {
            if (forceGroundedState == ForceGroundedStateMode.Grounded)
                _forceGroundedStateTallyCount.groundedTally += tally;
            else if (forceGroundedState == ForceGroundedStateMode.Airborn)
                _forceGroundedStateTallyCount.airbornTally  += tally;
            else if (forceGroundedState == ForceGroundedStateMode.Either)
                _forceGroundedStateTallyCount.eitherTally   += tally;
            else
                throw new NotImplementedException();

            if (_hasDoneInitialStateCheck)
                UpdateGroundedState();
        }

        private void UpdateGroundedState()
        {
            _cachedForceGroundedState = ForceGroundedState;

            if (_cachedForceGroundedState == ForceGroundedStateMode.Grounded && !IsGrounded)
                OnBecameGrounded();
            else if (_cachedForceGroundedState == ForceGroundedStateMode.Airborn && IsGrounded)
                OnBecameAirborn();
            else if (_cachedForceGroundedState == ForceGroundedStateMode.Either)
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
            IsGrounded     = false;
            GroundNormal   = Vector2.up;
            GroundTangent  = Vector2.right;
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

            foreach (FrozenCollision2D collision in _collisionsToHandle)
            {
                Vector2 closestContactNormal = collision.GetClosestContactNormal(Vector2.up);
                float steepness              = Vector2.Dot(closestContactNormal, Vector2.up);

                if (steepness > finalSteepness)
                {
                    finalSteepness     = steepness;
                    finalContactNormal = closestContactNormal;
                }
            }

            _isBelowMaxSteepness = finalSteepness > Mathf.Cos(MaxGroundSteepness * Mathf.Deg2Rad);
            if (_isBelowMaxSteepness)
            {
                GroundNormal   = finalContactNormal;
                GroundTangent  = Vector3.Cross(finalContactNormal, Vector3.forward);
            }
            else
            {
                GroundNormal   = Vector3.up;
                GroundTangent  = Vector3.right;
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

            _cachedForceGroundedState = ForceGroundedState;
            CollisionChecking();

            if (_touchingColliders.Count > 0 || _cachedForceGroundedState == ForceGroundedStateMode.Grounded)
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
            _collisionsToHandle.Add(new FrozenCollision2D(collision));
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            _collisionsToHandle.Add(new FrozenCollision2D(collision));
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
