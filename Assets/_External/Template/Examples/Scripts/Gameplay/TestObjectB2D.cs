using System.Collections.ObjectModel;
using Template.Saving;
using Template.Saving.Serialization;
using UnityEngine;

namespace Template.Gameplay
{
    /// <summary>
    /// Complimentary test script for <see cref="TestObjectA2D"/>.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class TestObjectB2D : MonoBehaviour, ISavableObject
    {
        [field: SerializeField]
        public DataKey DataKey { get; set; }

        private Rigidbody2D _rigidbody;

        public void Launch(float force)
        {
            _rigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
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
            _rigidbody = GetComponent<Rigidbody2D>();
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
        }
    }
}
