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

            set
            {
                _value = value;

                if (notifyChanges)
                    valueChanged?.Invoke(_value);
            }
        }

        [Space(7)] public bool notifyChanges;
        [Space(2)] public UnityEvent<TValue> valueChanged;
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
            get { return useConstant ? constant : reference.Value; }
            set 
            { 
                if (useConstant)
                    constant        = value;
                else
                    reference.Value = value;
            }
        }
    }
}
