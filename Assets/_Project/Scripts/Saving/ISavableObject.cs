using Template.Saving.Serialization;

namespace Template.Saving
{
    public enum SaveMethod
    {
        Automatic,
        Manual
    }

    public interface ISavableObject
    {
        public DataKey DataKey { get; set; }
        public SaveMethod SaveMethod { get; }
        public SaveMethod LoadMethod { get; }

        public SerializableObjectDataContainer GetSaveData();
        public void LoadSaveData(SerializableObjectDataContainer dataContainer);
    }
}
