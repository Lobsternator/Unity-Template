using FMODUnity;
using System.Collections.ObjectModel;
using Template.Audio;
using Template.Core;
using Template.Physics;
using Template.Saving;
using Template.Saving.Serialization;
using UnityEngine;

namespace Template.Gameplay
{
    /// <summary>
    /// Test script for showcasing the <see cref="PhysicsChecker2D"/>, <see cref="AudioManager"/>, and <see cref="TimeManager"/>.
    /// </summary>
    [RequireComponent(typeof(PhysicsChecker2D), typeof(Collider2D))]
    public class TestObjectA2D : MonoBehaviour, ISavableObject
    {
        [field: SerializeField]
        public DataKey DataKey { get; set; }

        [field: SerializeField]
        public float LaunchForce { get; private set; }

        [field: SerializeField]
        public EventReference ImpactSound { get; private set; }

        [field: SerializeField]
        public AudioEventSettings ImpactSoundSettings { get; private set; }

        [field: SerializeField]
        public HitstopSettings ImpactHitstopSettings { get; private set; }

        private PhysicsChecker2D _physicsChecker;
        private Rigidbody2D _rigidbody;

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
            SaveManager.Saving             += GetSaveData;
            SaveManager.Loading            += LoadSaveData;
        }
        private void OnDisable()
        {
            SaveManager.Loading            -= LoadSaveData;
            SaveManager.Saving             -= GetSaveData;
            _physicsChecker.BecameGrounded -= OnBecameGrounded;
        }

        private void Awake()
        {
            _physicsChecker = GetComponent<PhysicsChecker2D>();
            _rigidbody      = GetComponent<Rigidbody2D>();
        }

        public SaveData GetSaveData()
        {
            var saveData = new SaveData(this);

            saveData.Data.AddItem(nameof(_rigidbody.position), _rigidbody.position);
            saveData.Data.AddItem(nameof(_rigidbody.velocity), _rigidbody.velocity);

            return saveData;
        }

        public void LoadSaveData(ReadOnlyDictionary<DataKey, SerializableObjectDataContainer> data)
        {
            if (!data.TryGetValue(DataKey, out var dataContainer))
                return;

            Vector2 position = Vector2.zero;
            if (dataContainer.GetItem(nameof(_rigidbody.position), ref position))
                _rigidbody.MovePosition(position);

            Vector2 velocity = Vector2.zero;
            if (dataContainer.GetItem(nameof(_rigidbody.velocity), ref velocity))
                _rigidbody.velocity = velocity;

            var interpolation        = _rigidbody.interpolation;
            _rigidbody.interpolation = RigidbodyInterpolation2D.None;
            _rigidbody.interpolation = interpolation;
        }
    }
}
