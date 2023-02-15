using System.Collections.Generic;
using System.Collections.ObjectModel;
using Template.Saving.Serialization;

namespace Template.Saving
{
    public class SaveData
    {
        public ISavableObject Owner { get; }
        public SerializableObjectDataContainer Data { get; }

        public SaveData(ISavableObject owner, SerializableObjectDataContainer data)
        {
            Owner = owner;
            Data  = data;
        }
        public SaveData(ISavableObject owner)
        {
            Owner = owner;
            Data  = new SerializableObjectDataContainer();
        }
        public SaveData()
        {
            Owner = null;
            Data  = new SerializableObjectDataContainer();
        }
    }

    public interface ISavableObject
    {
        public DataKey DataKey { get; set; }

        public SaveData GetSaveData();
        public void LoadSaveData(ReadOnlyDictionary<DataKey, SerializableObjectDataContainer> data);
    }
}
