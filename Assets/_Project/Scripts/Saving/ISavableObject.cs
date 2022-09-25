using Template.Saving.Serialization;

namespace Template.Saving
{
    public interface ISavableObject
    {
        public DataKey DataKey { get; set; }

        public SerializableObjectDataContainer GetSaveData();
        public void LoadSaveData(SerializableObjectDataContainer dataContainer);
    }
}
