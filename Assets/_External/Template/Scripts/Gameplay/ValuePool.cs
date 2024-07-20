using System;
using UnityEngine;

namespace Template.Gameplay
{
    /// <summary>
    /// Event arguments for when a <see cref="Gameplay.ValuePool"/>'s value changes in some way.
    /// </summary>
    public class ValueChangeEventArgs : EventArgs
    {
        public float OldValue { get; }
        public float NewValue { get; }
        public float ValueChange => NewValue - OldValue;

        public ValuePool ValuePool { get; }
        public MonoBehaviour EventInstigator { get; }
        public MonoBehaviour ChangeCauser { get; }

        public ValueChangeEventArgs(float oldValue, float newValue, ValuePool valuePool, MonoBehaviour eventInstigator, MonoBehaviour changeCauser)
        {
            OldValue        = oldValue;
            NewValue        = newValue;
            ValuePool       = valuePool;
            EventInstigator = eventInstigator;
            ChangeCauser    = changeCauser;
        }
    }

    /// <summary>
    /// Object for managing a pool of arbitrary "value," within a restricted range.
    /// </summary>
    [Serializable]
    public class ValuePool
    {
        public MonoBehaviour Owner { get; set; }
        public bool IsAtMinValue => Mathf.Approximately(Value, MinValue);
        public bool IsAtMaxValue => Mathf.Approximately(Value, MaxValue);

        [field: SerializeField]
        public float Value { get; private set; } = 0.0f;
        private float _oldValue;

        [field: SerializeField]
        public float MinValue { get; private set; } = 0.0f;
        private float _oldMinValue;

        [field: SerializeField]
        public float MaxValue { get; private set; } = 100.0f;
        private float _oldMaxValue;

        public delegate float ModifyValueDelegate(float baseValue, MonoBehaviour eventInstigator, MonoBehaviour changeCauser);
        public ModifyValueDelegate ModifyValueChange { get; set; }
        public ModifyValueDelegate ModifyMinValueChange { get; set; }
        public ModifyValueDelegate ModifyMaxValueChange { get; set; }

        public event Action<ValueChangeEventArgs> ValueChanged;
        public event Action<ValueChangeEventArgs> MinValueChanged;
        public event Action<ValueChangeEventArgs> MaxValueChanged;

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
                var eventArgs = new ValueChangeEventArgs(_oldValue, Value, this, eventInstigator, changeCauser);
                _oldValue     = Mathf.Clamp(Value, MinValue, MaxValue);

                ValueChanged?.Invoke(eventArgs);
            }
        }
        public void SetValue(float value, MonoBehaviour eventInstigator) => SetValue(value, eventInstigator, null);
        public void SetValue(float value) => SetValue(value, null, null);

        public void ApplyValueChange(float value, MonoBehaviour eventInstigator, MonoBehaviour changeCauser)
        {
            if (ModifyValueChange is not null)
                value = ModifyValueChange(value, eventInstigator, changeCauser);

            SetValue(Value + value, eventInstigator, changeCauser);
        }
        public void ApplyValueChange(float value, MonoBehaviour eventInstigator) => ApplyValueChange(value, eventInstigator, null);
        public void ApplyValueChange(float value) => ApplyValueChange(value, null, null);

        public void SetMinValue(float minValue, MonoBehaviour eventInstigator, MonoBehaviour changeCauser)
        {
            MinValue = Mathf.Min(minValue, MaxValue);
            Value    = Mathf.Clamp(Value, MinValue, MaxValue);

            if (!Mathf.Approximately(_oldMinValue, MinValue))
            {
                var eventArgs = new ValueChangeEventArgs(_oldMinValue, MinValue, this, eventInstigator, changeCauser);
                _oldMinValue  = MinValue;

                MinValueChanged?.Invoke(eventArgs);
            }

            if (!Mathf.Approximately(_oldValue, Value))
            {
                var eventArgs = new ValueChangeEventArgs(_oldValue, Value, this, eventInstigator, changeCauser);
                _oldValue     = Value;

                ValueChanged?.Invoke(eventArgs);
            }
        }
        public void SetMinValue(float minValue, MonoBehaviour eventInstigator) => SetMinValue(minValue, eventInstigator, null);
        public void SetMinValue(float minValue) => SetMinValue(minValue, null, null);

        public void ApplyMinValueChange(float minValue, MonoBehaviour eventInstigator, MonoBehaviour changeCauser)
        {
            if (ModifyMinValueChange is not null)
                minValue = ModifyMinValueChange(minValue, eventInstigator, changeCauser);

            SetMinValue(MinValue + minValue, eventInstigator, changeCauser);
        }
        public void ApplyMinValueChange(float minValue, MonoBehaviour eventInstigator) => ApplyMinValueChange(minValue, eventInstigator, null);
        public void ApplyMinValueChange(float minValue) => ApplyMinValueChange(minValue, null, null);

        public void SetMaxValue(float maxValue, MonoBehaviour eventInstigator, MonoBehaviour changeCauser)
        {
            MaxValue = Mathf.Max(MinValue, maxValue);
            Value    = Mathf.Clamp(Value, MinValue, MaxValue);

            if (!Mathf.Approximately(_oldMaxValue, MaxValue))
            {
                var eventArgs = new ValueChangeEventArgs(_oldMaxValue, MaxValue, this, eventInstigator, changeCauser);
                _oldMaxValue  = MaxValue;

                MaxValueChanged?.Invoke(eventArgs);
            }

            if (!Mathf.Approximately(_oldValue, Value))
            {
                var eventArgs = new ValueChangeEventArgs(_oldValue, Value, this, eventInstigator, changeCauser);
                _oldValue     = Value;

                ValueChanged?.Invoke(eventArgs);
            }
        }
        public void SetMaxValue(float maxValue, MonoBehaviour eventInstigator) => SetMaxValue(maxValue, eventInstigator, null);
        public void SetMaxValue(float maxValue) => SetMaxValue(maxValue, null, null);

        public void ApplyMaxValueChange(float maxValue, MonoBehaviour eventInstigator, MonoBehaviour changeCauser)
        {
            if (ModifyMaxValueChange is not null)
                maxValue = ModifyMaxValueChange(maxValue, eventInstigator, changeCauser);

            SetMaxValue(MaxValue + maxValue, eventInstigator, changeCauser);
        }
        public void ApplyMaxValueChange(float maxValue, MonoBehaviour eventInstigator) => ApplyMaxValueChange(maxValue, eventInstigator, null);
        public void ApplyMaxValueChange(float maxValue) => ApplyMaxValueChange(maxValue, null, null);

        public void Validate()
        {
            SetMinValue(MinValue);
            SetMaxValue(MaxValue);
            SetValue(Value);
        }
    }
}
