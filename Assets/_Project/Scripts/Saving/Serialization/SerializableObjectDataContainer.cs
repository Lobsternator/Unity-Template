using System.Collections.Generic;
using Serializable = System.SerializableAttribute;

namespace Template.Saving.Serialization
{
    [Serializable]
    public class SerializableObjectDataContainer
    {
        private Dictionary<string, object> _items = new Dictionary<string, object>();

        public bool AddItem(string itemName, object itemValue)
        {
            if (_items.ContainsKey(itemName))
                return false;

            SerializationUtility.TryConvertToKnownSerializableType(itemValue, itemValue.GetType(), out itemValue);

            _items.Add(itemName, itemValue);
            return true;
        }
        public bool SetItem(string itemName, object itemValue)
        {
            if (!_items.ContainsKey(itemName))
                return false;

            SerializationUtility.TryConvertToKnownSerializableType(itemValue, itemValue.GetType(), out itemValue);

            _items[itemName] = itemValue;
            return true;
        }

        public bool GetItem<T>(string itemName, ref T itemValueRef)
        {
            if (!_items.TryGetValue(itemName, out var itemValue))
                return false;

            SerializationUtility.TryConvertToKnownUnserializableType(itemValue, itemValue.GetType(), out itemValue);

            itemValueRef = (T)itemValue;
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
