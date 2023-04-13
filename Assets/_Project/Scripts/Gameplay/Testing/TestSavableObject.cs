using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Template.Saving;
using Template.Saving.Serialization;
using Template.Core;

namespace Template.Gameplay
{
    public class TestSavableObject : MonoBehaviour, ISavableObject
    {
        [field: SerializeField] public DataKey DataKey { get; set; } = new DataKey();

        public Vector3 test;

        public SaveData GetSaveData()
        {
            SerializableObjectDataContainer dataContainer = new SerializableObjectDataContainer();

            dataContainer.AddItem(nameof(test), test);

            return new SaveData(this, dataContainer);
        }
        public void LoadSaveData(ReadOnlyDictionary<DataKey, SerializableObjectDataContainer> data)
        {
            if (!data.TryGetValue(DataKey, out SerializableObjectDataContainer dataContainer))
                return;

            dataContainer.GetItem(nameof(test), ref test);
        }

        private void OnEnable()
        {
            SaveManager.Saving  += GetSaveData;
            SaveManager.Loading += LoadSaveData;
        }
        private void OnDisable()
        {
            SaveManager.Saving  -= GetSaveData;
            SaveManager.Loading -= LoadSaveData;
        }
    }
}
