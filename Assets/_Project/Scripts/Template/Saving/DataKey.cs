using System;
using UnityEngine;

namespace Template.Saving
{
    /// <summary>
    /// Unique handle for identifying specific save data.
    /// </summary>
    [Serializable]
    public class DataKey : IEquatable<DataKey>
    {
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
