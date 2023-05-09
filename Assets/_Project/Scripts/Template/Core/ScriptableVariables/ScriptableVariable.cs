using System;
using UnityEngine;
using UnityEngine.Events;

namespace Template.Core
{
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
