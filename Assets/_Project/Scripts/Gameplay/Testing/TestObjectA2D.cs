using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Audio;
using Template.Core;
using Template.Events;
using Template.Physics;

namespace Template.Gameplay
{
    [RequireComponent(typeof(PhysicsChecker2D), typeof(Collider2D))]
    public class TestObjectA2D : MonoBehaviour
    {
        [field: SerializeField] public float LaunchForce { get; private set; }
        [field: SerializeField] public AudioClip ImpactSound { get; private set; }
        [field: SerializeField] public HitstopSettings ImpactHitstopSettings { get; private set; }

        private PhysicsChecker2D _physicsChecker;

        private void OnBecameGrounded()
        {
            AudioManager.Instance.PlaySound(ImpactSound);
            TimeManager.Instance.DoHitstop(ImpactHitstopSettings);
            EventManager.Instance.GameplayEvents.Test2D?.Invoke(this);
        }

        private void OnEnable()
        {
            _physicsChecker.BecameGrounded += OnBecameGrounded;
        }
        private void OnDisable()
        {
            _physicsChecker.BecameGrounded -= OnBecameGrounded;
        }

        private void Awake()
        {
            _physicsChecker = GetComponent<PhysicsChecker2D>();
        }
    }
}
