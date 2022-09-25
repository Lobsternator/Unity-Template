using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Core;
using Action = System.Action;

namespace Template.Physics
{
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicsChecker : MonoBehaviour
    {
        [field: Range(0.0f, 90.0f)]
        [field: Tooltip("When the ground steepness is below this threshold the object will be considered to be \"grounded\".")]
        [field: SerializeField] public float MaxGroundSteepness { get; set; }  = 85.0f;

        [field: Tooltip("When the velocity is above this threshold the object will be considered to be \"moving\".")]
        [field: SerializeField] public float MinVelocity { get; set; }         = 0.1f;

        [Tooltip("Force the objects state to be either \"grounded\" or \"airborn\", or either of the two; decided normally.")]
        [SerializeField] private ForceGroundedStateMode _forceGroundedState    = ForceGroundedStateMode.Either;
        public ForceGroundedStateMode ForceGroundedState
        {
            get => _forceGroundedState;
            set
            {
                _forceGroundedState = value;
                if (!_hasDoneInitialStateCheck)
                    return;

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
        }

        public bool IsMoving { get; private set; }
        public bool IsGrounded { get; private set; }
        public Vector3 GroundNormal { get; private set; }   = Vector3.up;
        public Vector3 GroundBinormal { get; private set; } = Vector3.right;
        public Vector3 GroundTangent { get; private set; }  = Vector3.forward;

        public float GroundSteepness => 1.0f - Mathf.Clamp01(Vector3.Dot(GroundNormal, Vector3.up));

        public event Action StartedMoving;
        public event Action StoppedMoving;
        public event Action BecameGrounded;
        public event Action BecameAirborn;
        public event Action PhysicsFrameProcessed;

        private bool ShouldBecomeGrounded => _touchingColliders.Count > 0 && !IsGrounded && _isBelowMaxSteepness                      && ForceGroundedState != ForceGroundedStateMode.Airborn  || (!IsGrounded && ForceGroundedState == ForceGroundedStateMode.Grounded);
        private bool ShouldBecomeAirborn  => ((_touchingColliders.Count == 0 && IsGrounded) || (IsGrounded && !_isBelowMaxSteepness)) && ForceGroundedState != ForceGroundedStateMode.Grounded || ( IsGrounded && ForceGroundedState == ForceGroundedStateMode.Airborn);

        private Rigidbody _rigidbody;
        private HashSet<Collider> _touchingColliders      = new HashSet<Collider>();
        private List<FrozenCollision> _collisionsToHandle = new List<FrozenCollision>();
        private bool _isBelowMaxSteepness;
        private bool _hasDoneInitialStateCheck;

        private void OnBecameGrounded()
        {
            IsGrounded = true;
            BecameGrounded?.Invoke();
        }
        private void OnBecameAirborn()
        {
            IsGrounded     = false;
            GroundNormal   = Vector3.up;
            GroundBinormal = Vector3.right;
            GroundTangent  = Vector3.forward;
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
            Vector3 finalContactNormal = Vector3.zero;
            float finalSteepness       = float.MinValue;

            foreach (FrozenCollision collision in _collisionsToHandle)
            {
                Vector3 closestContactNormal = collision.GetClosestContactNormal(Vector3.up);
                float steepness              = Vector3.Dot(closestContactNormal, Vector3.up);

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
                GroundBinormal = Vector3.Cross(finalContactNormal, Vector3.up);
                GroundTangent  = Vector3.Cross(finalContactNormal, GroundBinormal);
            }
            else
            {
                GroundNormal   = Vector3.up;
                GroundBinormal = Vector3.right;
                GroundTangent  = Vector3.forward;
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

            CollisionChecking();

            if (_touchingColliders.Count > 0 || ForceGroundedState == ForceGroundedStateMode.Grounded)
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
            if (!_hasDoneInitialStateCheck)
                return;

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
#endif

        private void OnCollisionEnter(Collision collision)
        {
            _touchingColliders.Add(collision.collider);
            _collisionsToHandle.Add(new FrozenCollision(collision));
        }
        private void OnCollisionStay(Collision collision)
        {
            _collisionsToHandle.Add(new FrozenCollision(collision));
        }
        private void OnCollisionExit(Collision collision)
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
            _rigidbody = GetComponent<Rigidbody>();
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
