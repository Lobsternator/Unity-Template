using System.Collections.Generic;
using Serializable = System.SerializableAttribute;

namespace Template.Saving.Serialization
{
    /// <summary>
    /// Container for storing and retrieving serializable data.
    /// </summary>
    [Serializable]
    public class SerializableObjectDataContainer
    {
        private Dictionary<string, object> _items = new Dictionary<string, object>();

        /// <summary>
        /// If itemValue is not serializable; tries to convert it to a corresponding serializable type, then adds it to the container.
        /// </summary>
        public bool AddItem(string itemName, object itemValue)
        {
            if (_items.ContainsKey(itemName))
                return false;

            SerializationUtility.TryConvertToKnownSerializableType(itemValue, itemValue.GetType(), out itemValue);

            _items.Add(itemName, itemValue);
            return true;
        }
        /// <summary>
        /// If itemValue is not serializable; tries to convert it to a corresponding serializable type, then sets it in the container.
        /// </summary>
        public bool SetItem(string itemName, object itemValue)
        {
            if (!_items.ContainsKey(itemName))
                return false;

            SerializationUtility.TryConvertToKnownSerializableType(itemValue, itemValue.GetType(), out itemValue);

            _items[itemName] = itemValue;
            return true;
        }

        /// <summary>
        /// If itemValue was converted to a serializable to when being added; tries to convert it back to original unserializable type, then returns.
        /// </summary>
        public bool GetItem<T>(string itemName, ref T itemValue)
        {
            if (!_items.TryGetValue(itemName, out var serializedValue))
                return false;

            SerializationUtility.TryConvertToKnownUnserializableType(serializedValue, serializedValue.GetType(), out serializedValue);

            itemValue = (T)serializedValue;
            return true;
        }

        public bool RemoveItem(string itemName)
        {
            return _items.Remove(itemName);
        }

        public bool ContainsItem(string itemName)
        {
            return _items.ContainsKey(itemName);
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}
