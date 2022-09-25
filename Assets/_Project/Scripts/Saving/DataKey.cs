using System;
using UnityEngine;

namespace Template.Saving
{
    [Serializable]
    public class DataKey : IEquatable<DataKey>
    {
        public static DataKey CreateNew()
        {
            DataKey dataKey = new DataKey();
            dataKey._value = Guid.NewGuid().ToString();

            return dataKey;
        }

        [SerializeField]
        private string _value = Guid.NewGuid().ToString();
        public string Value => _value;

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public bool Equals(DataKey other)
        {
            return Value == other.Value;
        }
    }
}
