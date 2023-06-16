using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Gameplay
{
    /// <summary>
    /// Event arguments for when a <see cref="Gameplay.ValuePool"/>'s value changes in some way.
    /// </summary>
    public class ValueEventArgs : EventArgs
    {
        public ValueEventArgs(float oldValue, float newValue, ValuePool valuePool, MonoBehaviour eventInstigator, MonoBehaviour changeCauser)
        {
            OldValue        = oldValue;
            NewValue        = newValue;
            ValuePool       = valuePool;
            EventInstigator = eventInstigator;
            ChangeCauser    = changeCauser;
        }

        public float OldValue { get; }
        public float NewValue { get; }
        public float ValueDifference => NewValue - OldValue;

        public ValuePool ValuePool { get; }
        public MonoBehaviour EventInstigator { get; }
        public MonoBehaviour ChangeCauser { get; }
    }

    /// <summary>
    /// Object for managing a pool of arbitrary "value".
    /// </summary>
    [Serializable]
    public class ValuePool
    {
        public MonoBehaviour Owner { get; set; }
        public bool IsAtMinValue => Mathf.Approximately(Value, MinValue);
        public bool IsAtMaxValue => Mathf.Approximately(Value, MaxValue);

        private float _oldValue;
        [field: SerializeField] public float Value { get; private set; } = 0.0f;

        private float _oldMinValue;
        [field: SerializeField] public float MinValue { get; private set; } = 0.0f;

        private float _oldMaxValue;
        [field: SerializeField] public float MaxValue { get; private set; } = 100.0f;

        public event Action<ValueEventArgs> ValueChanged;
        public event Action<ValueEventArgs> MinValueChanged;
        public event Action<ValueEventArgs> MaxValueChanged;

        public ValuePool()
        {
            Owner    = null;
            Value    = 0.0f;
            MinValue = 0.0f;
            MaxValue = 100.0f;

            _oldMinValue = MinValue;
            _oldMaxValue = MaxValue;
            _oldValue    = Value;
        }
        public ValuePool(float value, float minValue, float maxValue)
        {
            Owner    = null;
            Value    = value;
            MinValue = minValue;
            MaxValue = maxValue;

            _oldMinValue = MinValue;
            _oldMaxValue = MaxValue;
            _oldValue    = Value;

            Validate();
        }
        public ValuePool(MonoBehaviour owner)
        {
            Owner    = owner;
            Value    = 0.0f;
            MinValue = 0.0f;
            MaxValue = 100.0f;

            _oldMinValue = MinValue;
            _oldMaxValue = MaxValue;
            _oldValue    = Value;
        }
        public ValuePool(MonoBehaviour owner, float value, float minValue, float maxValue)
        {
            Owner    = owner;
            Value    = value;
            MinValue = minValue;
            MaxValue = maxValue;

            _oldMinValue = MinValue;
            _oldMaxValue = MaxValue;
            _oldValue    = Value;

            Validate();
        }

        public void SetValue(float value, MonoBehaviour eventInstigator, MonoBehaviour changeCauser)
        {
            Value = Mathf.Clamp(value, MinValue, MaxValue);

            if (!Mathf.Approximately(_oldValue, Value))
            {
                var eventArgs = new ValueEventArgs(_oldValue, Value, this, eventInstigator, changeCauser);
                _oldValue     = Mathf.Clamp(Value, MinValue, MaxValue);

                ValueChanged?.Invoke(eventArgs);
            }
        }
        public void SetValue(float value, MonoBehaviour eventInstigator) => SetValue(value, eventInstigator, null);
        public void SetValue(float value) => SetValue(value, null, null);

        public void AddValue(float value, MonoBehaviour eventInstigator, MonoBehaviour changeCauser)
        {
            SetValue(Value + value, eventInstigator, changeCauser);
        }
        public void AddValue(float value, MonoBehaviour eventInstigator) => AddValue(value, eventInstigator, null);
        public void AddValue(float value) => AddValue(value, null, null);

        public void SetMinValue(float minValue, MonoBehaviour eventInstigator, MonoBehaviour changeCauser)
        {
            MinValue = Mathf.Min(minValue, MaxValue);
            Value    = Mathf.Clamp(Value, MinValue, MaxValue);

            if (!Mathf.Approximately(_oldMinValue, MinValue))
            {
                var eventArgs = new ValueEventArgs(_oldMinValue, MinValue, this, eventInstigator, changeCauser);
                _oldMinValue  = MinValue;

                MinValueChanged?.Invoke(eventArgs);
            }

            if (!Mathf.Approximately(_oldValue, Value))
            {
                var eventArgs = new ValueEventArgs(_oldValue, Value, this, eventInstigator, changeCauser);
                _oldValue     = Value;

                ValueChanged?.Invoke(eventArgs);
            }
        }
        public void SetMinValue(float minValue, MonoBehaviour eventInstigator) => SetMinValue(minValue, eventInstigator, null);
        public void SetMinValue(float minValue) => SetMinValue(minValue, null, null);

        public void AddMinValue(float minValue, MonoBehaviour eventInstigator, MonoBehaviour changeCauser)
        {
            SetMinValue(MinValue + minValue, eventInstigator, changeCauser);
        }
        public void AddMinValue(float minValue, MonoBehaviour eventInstigator) => AddMinValue(minValue, eventInstigator, null);
        public void AddMinValue(float minValue) => AddMinValue(minValue, null, null);

        public void SetMaxValue(float maxValue, MonoBehaviour eventInstigator, MonoBehaviour changeCauser)
        {
            MaxValue = Mathf.Max(MinValue, maxValue);
            Value    = Mathf.Clamp(Value, MinValue, MaxValue);

            if (!Mathf.Approximately(_oldMaxValue, MaxValue))
            {
                var eventArgs = new ValueEventArgs(_oldMaxValue, MaxValue, this, eventInstigator, changeCauser);
                _oldMaxValue  = MaxValue;

                MaxValueChanged?.Invoke(eventArgs);
            }

            if (!Mathf.Approximately(_oldValue, Value))
            {
                var eventArgs = new ValueEventArgs(_oldValue, Value, this, eventInstigator, changeCauser);
                _oldValue     = Value;

                ValueChanged?.Invoke(eventArgs);
            }
        }
        public void SetMaxValue(float maxValue, MonoBehaviour eventInstigator) => SetMaxValue(maxValue, eventInstigator, null);
        public void SetMaxValue(float maxValue) => SetMaxValue(maxValue, null, null);

        public void AddMaxValue(float maxValue, MonoBehaviour eventInstigator, MonoBehaviour changeCauser)
        {
            SetMaxValue(MaxValue + maxValue, eventInstigator, changeCauser);
        }
        public void AddMaxValue(float maxValue, MonoBehaviour eventInstigator) => AddMaxValue(maxValue, eventInstigator, null);
        public void AddMaxValue(float maxValue) => AddMaxValue(maxValue, null, null);

        public void Validate()
        {
            SetMinValue(MinValue);
            SetMaxValue(MaxValue);
            SetValue(Value);
        }
    }
}
