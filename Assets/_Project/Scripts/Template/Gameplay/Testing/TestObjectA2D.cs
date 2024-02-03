using FMODUnity;
using Template.Audio;
using Template.Core;
using Template.Physics;
using UnityEngine;

namespace Template.Gameplay
{
    /// <summary>
    /// Test script for showcasing the <see cref="PhysicsChecker2D"/>, <see cref="AudioManager"/>, and <see cref="TimeManager"/>.
    /// </summary>
    [RequireComponent(typeof(PhysicsChecker2D), typeof(Collider2D))]
    public class TestObjectA2D : MonoBehaviour
    {
        [field: SerializeField] public float LaunchForce { get; private set; }
        [field: SerializeField] public EventReference ImpactSound { get; private set; }
        [field: SerializeField] public AudioEventSettings ImpactSoundSettings { get; private set; }
        [field: SerializeField] public HitstopSettings ImpactHitstopSettings { get; private set; }

        private PhysicsChecker2D _physicsChecker;

        private void OnBecameGrounded()
        {
            AudioManager.PlaySoundAttached(ImpactSound, ImpactSoundSettings, transform);
            TimeManager.DoHitstop(ImpactHitstopSettings);

            foreach (TestObjectB2D testObjectB in FindObjectsOfType<TestObjectB2D>())
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
            _physicsChecker = GetComponent<PhysicsChecker2D>();
        }
    }
}
