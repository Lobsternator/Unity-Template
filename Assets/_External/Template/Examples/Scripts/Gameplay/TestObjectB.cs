using System.Collections.ObjectModel;
using Template.Saving;
using Template.Saving.Serialization;
using UnityEngine;

namespace Template.Gameplay
{
    /// <summary>
    /// Complimentary test script for <see cref="TestObjectA"/>.
    /// </summary>
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class TestObjectB : MonoBehaviour, ISavableObject
    {
        [field: SerializeField]
        public DataKey DataKey { get; set; }

        private Rigidbody _rigidbody;

        public void Launch(float force)
        {
            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
        }

        private void OnEnable()
        {
            SaveManager.Saving  += GetSaveData;
            SaveManager.Loading += LoadSaveData;
        }
        private void OnDisable()
        {
            SaveManager.Loading -= LoadSaveData;
            SaveManager.Saving  -= GetSaveData;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
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

            Vector3 position = Vector3.zero;
            if (dataContainer.GetItem(nameof(_rigidbody.position), ref position))
                _rigidbody.MovePosition(position);

            Vector3 velocity = Vector3.zero;
            if (dataContainer.GetItem(nameof(_rigidbody.velocity), ref velocity))
                _rigidbody.velocity = velocity;

            var interpolation        = _rigidbody.interpolation;
            _rigidbody.interpolation = RigidbodyInterpolation.None;
            _rigidbody.interpolation = interpolation;
        }
    }
}
