using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Saving;
using Template.Saving.Serialization;

namespace Template.Gameplay
{
    public class TestSavableObject : MonoBehaviour, ISavableObject
    {
        [field: SerializeField] public DataKey DataKey { get; set; } = new DataKey();
        public Vector3 test;

        public SerializableObjectDataContainer GetSaveData()
        {
            SerializableObjectDataContainer dataContainer = new SerializableObjectDataContainer();

            dataContainer.AddItem(nameof(test), test);

            return dataContainer;
        }

        public void LoadSaveData(SerializableObjectDataContainer dataContainer)
        {
            dataContainer.GetItem(nameof(test), ref test);
        }
    }
}
