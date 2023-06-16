using System;
using UnityEngine;
using UnityEngine.Events;

namespace Template.Core
{
    /// <summary>
    /// ScriptableObject storing a single value.
    /// </summary>
    [Serializable]
    public class ScriptableReference<TValue> : ScriptableObject
    {
        [SerializeField]
        private TValue _value;
        public TValue Value
        {
            get => _value;
            set => _value = value;
        }
    }

    /// <summary>
    /// Wrapper around <see cref="ScriptableReference{TValue}"/> that also allows for use of a constant value.
    /// </summary>
    [Serializable]
    public class ScriptableVariable<TValue>
    {
        public bool useConstant = true;
        public TValue constant;
        public ScriptableReference<TValue> reference;

        public bool HasReference => reference != null;

        public TValue Value
        {
            get { return useConstant ? constant : reference != null ? reference.Value : default; }
            set 
            { 
                if (useConstant)
                    constant = value;

                else if (HasReference)
                    reference.Value = value;
            }
        }
    }
}
