using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Template.Gameplay
{
    public interface IDamageableObject
    {
        public DamageManager DamageManager { get; }

        public void OnDamageChanged(float oldDamage, float newDamage);
        public void OnMinDamageChanged(float oldMinDamage, float newMinDamage);
        public void OnMaxDamageChanged(float oldMaxDamage, float newMaxDamage);
        public void OnDamageMaxed(float oldDamage, float newDamage);
    }

    [Serializable]
    public class DamageManager
    {
        public IDamageableObject Owner { get; }
        public bool IsDestroyed => Mathf.Approximately(_damage, _maxDamage);

        private float _oldDamage;
        [SerializeField] private float _damage;
        public float Damage
        {
            get => _damage;
            set
            {
                _damage = Mathf.Clamp(value, _minDamage, _maxDamage);

                if (!Mathf.Approximately(_oldDamage, _damage))
                {
                    Owner.OnDamageChanged(_oldDamage, _damage);

                    if (Mathf.Approximately(_damage, _maxDamage))
                        Owner.OnDamageMaxed(_oldDamage, _damage);
                }
                
                _oldDamage = Mathf.Clamp(_damage, _minDamage, _maxDamage);
            }
        }

        private float _oldMinDamage;
        [SerializeField] private float _minDamage;
        public float MinDamage
        {
            get => _minDamage;
            set
            {
                _minDamage = Mathf.Min(value, _maxDamage);
                _damage    = Mathf.Clamp(_damage, _minDamage, _maxDamage);

                if (!Mathf.Approximately(_oldMinDamage, _minDamage))
                    Owner.OnMinDamageChanged(_oldMinDamage, _minDamage);

                if (!Mathf.Approximately(_oldDamage, _damage))
                {
                    Owner.OnDamageChanged(_oldDamage, _damage);

                    if (Mathf.Approximately(_damage, _maxDamage))
                        Owner.OnDamageMaxed(_oldDamage, _damage);
                }

                _oldDamage    = _damage;
                _oldMinDamage = _minDamage;
            }
        }

        private float _oldMaxDamage;
        [SerializeField] private float _maxDamage;
        public float MaxDamage
        {
            get => _maxDamage;
            set
            {
                _maxDamage = Mathf.Max(_minDamage, value);
                _damage    = Mathf.Clamp(_damage, _minDamage, _maxDamage);

                if (!Mathf.Approximately(_oldMaxDamage, _maxDamage))
                    Owner.OnMaxDamageChanged(_oldMaxDamage, _maxDamage);

                if (!Mathf.Approximately(_oldDamage, _damage))
                {
                    Owner.OnDamageChanged(_oldDamage, _damage);

                    if (Mathf.Approximately(_damage, _maxDamage))
                        Owner.OnDamageMaxed(_oldDamage, _damage);
                }

                _oldDamage    = _damage;
                _oldMaxDamage = _maxDamage;
            }
        }

        public DamageManager(IDamageableObject owner)
        {
            Owner     = owner;
            Damage    = 0;
            MinDamage = 0;
            MaxDamage = 100;

            ClearOldValues();
        }
        public DamageManager(IDamageableObject owner, float damage, float minDamage, float maxDamage)
        {
            Owner     = owner;
            Damage    = damage;
            MinDamage = minDamage;
            MaxDamage = maxDamage;

            ClearOldValues();
        }

        public void ClearOldValues()
        {
            _oldMinDamage = _minDamage;
            _oldMaxDamage = _maxDamage;
            _oldDamage    = _damage;
        }

        public void Validate()
        {
            MinDamage = MinDamage;
            MaxDamage = MaxDamage;
            Damage    = Damage;
        }
    }

    public class DamageableObject : MonoBehaviour, IDamageableObject
    {
        [Serializable]
        public class DamageEvents
        {
            public UnityEvent<float, float> DamageChanged;
            public UnityEvent<float, float> MinDamageChanged;
            public UnityEvent<float, float> MaxDamageChanged;
            public UnityEvent<float, float> DamageMaxed;
        }

        [field: SerializeField] public DamageManager DamageManager { get; private set; }

        [field: Space(7)]
        [field: SerializeField] public DamageEvents Events { get; private set; }

        public DamageableObject()
        {
            Events        = new DamageEvents();
            DamageManager = new DamageManager(this);
        }

        /// <summary>
        /// Internal function, don't use unless you know what you're doing.
        /// </summary>
        public virtual void OnDamageChanged(float oldDamage, float newDamage)
        {
            Events.DamageChanged?.Invoke(oldDamage, newDamage);
        }
        /// <summary>
        /// Internal function, don't use unless you know what you're doing.
        /// </summary>
        public virtual void OnMinDamageChanged(float oldMinDamage, float newMinDamage)
        {
            Events.MinDamageChanged?.Invoke(oldMinDamage, newMinDamage);
        }
        /// <summary>
        /// Internal function, don't use unless you know what you're doing.
        /// </summary>
        public virtual void OnMaxDamageChanged(float oldMaxDamage, float newMaxDamage)
        {
            Events.MaxDamageChanged?.Invoke(oldMaxDamage, newMaxDamage);
        }
        /// <summary>
        /// Internal function, don't use unless you know what you're doing.
        /// </summary>
        public virtual void OnDamageMaxed(float oldDamage, float newDamage)
        {
            Events.DamageMaxed?.Invoke(oldDamage, newDamage);
        }

        protected virtual void Awake()
        {
            DamageManager.ClearOldValues();
            DamageManager.Validate();
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (!gameObject.scene.isLoaded)
                DamageManager.ClearOldValues();

            DamageManager.Validate();
        }
#endif
    }
}
