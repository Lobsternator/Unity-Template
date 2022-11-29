using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Template.Gameplay
{
    [Serializable]
    public class DamageEventArgs : EventArgs
    {
        public DamageEventArgs(float oldDamage, float newDamage, DamageType damageType, IDamageableObject damageReciever, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            OldDamage       = oldDamage;
            NewDamage       = newDamage;
            DamageType      = damageType;
            DamageReciever  = damageReciever;
            EventInstigator = eventInstigator;
            DamageCauser    = damageCauser;
        }

        public float OldDamage { get; }
        public float NewDamage { get; }
        public DamageType DamageType { get; }

        public IDamageableObject DamageReciever { get; }
        public MonoBehaviour EventInstigator { get; }
        public MonoBehaviour DamageCauser { get; }
    }

    [Serializable]
    public abstract class DamageType : ScriptableObject
    {
        public abstract float GetModifiedDamage(float baseDamage, IDamageableObject damageReciever, MonoBehaviour eventInstigator, MonoBehaviour damageCauser);
    }

    public interface IDamageableObject
    {
        public DamageManager DamageManager { get; }

        public void OnDamageChanged(DamageEventArgs eventArgs);
        public void OnMinDamageChanged(DamageEventArgs eventArgs);
        public void OnMaxDamageChanged(DamageEventArgs eventArgs);
    }

    [Serializable]
    public class DamageManager
    {
        public IDamageableObject Owner { get; }
        public bool IsDestroyed => Mathf.Approximately(Damage, MaxDamage);
        [field: SerializeField] public bool IsDamageable { get; set; } = true;

        private float _oldDamage;
        [field: Space(7)]
        [field: SerializeField] public float Damage { get; private set; }

        private float _oldMinDamage;
        [field: SerializeField] public float MinDamage { get; private set; }

        private float _oldMaxDamage;
        [field: SerializeField] public float MaxDamage { get; private set; }

        public DamageManager(IDamageableObject owner)
        {
            Owner     = owner;
            Damage    = 0;
            MinDamage = 0;
            MaxDamage = 100;

            UpdateValues();
        }
        public DamageManager(IDamageableObject owner, float damage, float minDamage, float maxDamage)
        {
            Owner     = owner;
            Damage    = damage;
            MinDamage = minDamage;
            MaxDamage = maxDamage;

            UpdateValues();
        }

        public void ApplyDamage(float baseDamage, DamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            if (!IsDamageable)
                return;

            float modifiedDamage = damageType is not null ? damageType.GetModifiedDamage(baseDamage, Owner, eventInstigator, damageCauser) : baseDamage;
            Damage               = Mathf.Clamp(Damage + modifiedDamage, MinDamage, MaxDamage);

            if (!Mathf.Approximately(_oldDamage, Damage))
                Owner.OnDamageChanged(new DamageEventArgs(_oldDamage, Damage, damageType, Owner, eventInstigator, damageCauser));

            _oldDamage = Mathf.Clamp(Damage, MinDamage, MaxDamage);
        }

        public void SetMinDamage(float minDamage, MonoBehaviour eventInstigator, MonoBehaviour causer)
        {
            MinDamage = Mathf.Min(minDamage, MaxDamage);
            Damage    = Mathf.Clamp(Damage, MinDamage, MaxDamage);

            if (!Mathf.Approximately(_oldMinDamage, MinDamage))
                Owner.OnMinDamageChanged(new DamageEventArgs(_oldMinDamage, MinDamage, null, Owner, eventInstigator, causer));

            if (!Mathf.Approximately(_oldDamage, Damage))
                Owner.OnDamageChanged(new DamageEventArgs(_oldDamage, Damage, null, Owner, eventInstigator, causer));

            _oldDamage    = Damage;
            _oldMinDamage = MinDamage;
        }
        public void SetMaxDamage(float maxDamage, MonoBehaviour eventInstigator, MonoBehaviour causer)
        {
            MaxDamage = Mathf.Max(MinDamage, maxDamage);
            Damage    = Mathf.Clamp(Damage, MinDamage, MaxDamage);

            if (!Mathf.Approximately(_oldMaxDamage, MaxDamage))
                Owner.OnMaxDamageChanged(new DamageEventArgs(_oldMaxDamage, MaxDamage, null, Owner, eventInstigator, causer));

            if (!Mathf.Approximately(_oldDamage, Damage))
                Owner.OnDamageChanged(new DamageEventArgs(_oldDamage, Damage, null, Owner, eventInstigator, causer));

            _oldDamage    = Damage;
            _oldMaxDamage = MaxDamage;
        }

        public void UpdateValues()
        {
            _oldMinDamage = MinDamage;
            _oldMaxDamage = MaxDamage;
            _oldDamage    = Damage;
        }
        public void Validate()
        {
            SetMinDamage(MinDamage, null, null);
            SetMaxDamage(MaxDamage, null, null);
            ApplyDamage(0, null, null, null);
        }
    }

    [DisallowMultipleComponent]
    public class DamageableObject : MonoBehaviour, IDamageableObject
    {
        [Serializable]
        public class EventsContainer
        {
            public UnityEvent<DamageEventArgs> damageChanged;
            public UnityEvent<DamageEventArgs> minDamageChanged;
            public UnityEvent<DamageEventArgs> maxDamageChanged;
        }

        [field: SerializeField] public DamageManager DamageManager { get; private set; }

        [field: Space(7)]
        [field: SerializeField] public EventsContainer DamageEvents { get; private set; }

        public DamageableObject()
        {
            DamageEvents  = new EventsContainer();
            DamageManager = new DamageManager(this);
        }

        /// <summary>
        /// Internal function, don't use unless you know what you're doing.
        /// </summary>
        public virtual void OnDamageChanged(DamageEventArgs eventArgs)
        {
            DamageEvents.damageChanged?.Invoke(eventArgs);
        }
        /// <summary>
        /// Internal function, don't use unless you know what you're doing.
        /// </summary>
        public virtual void OnMinDamageChanged(DamageEventArgs eventArgs)
        {
            DamageEvents.minDamageChanged?.Invoke(eventArgs);
        }
        /// <summary>
        /// Internal function, don't use unless you know what you're doing.
        /// </summary>
        public virtual void OnMaxDamageChanged(DamageEventArgs eventArgs)
        {
            DamageEvents.maxDamageChanged?.Invoke(eventArgs);
        }

        protected virtual void Awake()
        {
            DamageManager.UpdateValues();
            DamageManager.Validate();
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (!gameObject.scene.isLoaded)
                DamageManager.UpdateValues();

            DamageManager.Validate();
        }
#endif
    }
}
