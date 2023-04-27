using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Template.Audio;
using Template.Core;
using Template.Physics;

namespace Template.Gameplay
{
    [RequireComponent(typeof(PhysicsChecker), typeof(Collider))]
    public class TestObjectA : MonoBehaviour
    {
        [field: SerializeField] public float LaunchForce { get; private set; }
        [field: SerializeField] public EventReference ImpactSound { get; private set; }
        [field: SerializeField] public AudioEventSettings ImpactSoundSettings { get; private set; }
        [field: SerializeField] public HitstopSettings ImpactHitstopSettings { get; private set; }

        private PhysicsChecker _physicsChecker;

        private void OnBecameGrounded()
        {
            AudioManager.PlaySound(ImpactSound, ImpactSoundSettings);
            TimeManager.DoHitstop(ImpactHitstopSettings);

            foreach (TestObjectB testObjectB in FindObjectsOfType<TestObjectB>())
                testObjectB.Launch(LaunchForce);
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
            _physicsChecker = GetComponent<PhysicsChecker>();
        }
    }
}
