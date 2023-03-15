using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Gameplay
{
    public interface IDamageEventArgs<TDamageType> where TDamageType : IDamageType
    {
        public float OldDamage { get; }
        public float NewDamage { get; }
        public TDamageType DamageType { get; }

        public DamagePool DamagePool { get; }
        public MonoBehaviour EventInstigator { get; }
        public MonoBehaviour DamageCauser { get; }
    }

    public class DamageEventArgs : EventArgs, IDamageEventArgs<DamageType>
    {
        public DamageEventArgs(float oldDamage, float newDamage, DamageType damageType, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            OldDamage       = oldDamage;
            NewDamage       = newDamage;
            DamageType      = damageType;
            DamagePool      = damagePool;
            EventInstigator = eventInstigator;
            DamageCauser    = damageCauser;
        }

        public float OldDamage { get; }
        public float NewDamage { get; }
        public DamageType DamageType { get; }

        public DamagePool DamagePool { get; }
        public MonoBehaviour EventInstigator { get; }
        public MonoBehaviour DamageCauser { get; }
    }

    public class MinDamageEventArgs : EventArgs, IDamageEventArgs<MinDamageType>
    {
        public MinDamageEventArgs(float oldMinDamage, float newMinDamage, MinDamageType damageType, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            OldDamage       = oldMinDamage;
            NewDamage       = newMinDamage;
            DamageType      = damageType;
            DamagePool      = damagePool;
            EventInstigator = eventInstigator;
            DamageCauser    = damageCauser;
        }

        public float OldDamage { get; }
        public float NewDamage { get; }
        public MinDamageType DamageType { get; }

        public DamagePool DamagePool { get; }
        public MonoBehaviour EventInstigator { get; }
        public MonoBehaviour DamageCauser { get; }
    }

    public class MaxDamageEventArgs : EventArgs, IDamageEventArgs<MaxDamageType>
    {
        public MaxDamageEventArgs(float oldMaxDamage, float newMaxDamage, MaxDamageType damageType, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            OldDamage       = oldMaxDamage;
            NewDamage       = newMaxDamage;
            DamageType      = damageType;
            DamagePool      = damagePool;
            EventInstigator = eventInstigator;
            DamageCauser    = damageCauser;
        }

        public float OldDamage { get; }
        public float NewDamage { get; }
        public MaxDamageType DamageType { get; }

        public DamagePool DamagePool { get; }
        public MonoBehaviour EventInstigator { get; }
        public MonoBehaviour DamageCauser { get; }
    }

    [Serializable]
    public class DamagePool
    {
        public MonoBehaviour Owner { get; }
        public bool IsAtMinDamage => Mathf.Approximately(Damage, MinDamage);
        public bool IsAtMaxDamage => Mathf.Approximately(Damage, MaxDamage);
        [field: SerializeField] public bool IsDamageable { get; set; } = true;

        private float _oldDamage;
        [field: Space(7)]
        [field: SerializeField] public float Damage { get; private set; } = 0.0f;

        private float _oldMinDamage;
        [field: SerializeField] public float MinDamage { get; private set; } = 0.0f;

        private float _oldMaxDamage;
        [field: SerializeField] public float MaxDamage { get; private set; } = 100.0f;

        public event Action<DamageEventArgs> DamageChanged;
        public event Action<MinDamageEventArgs> MinDamageChanged;
        public event Action<MaxDamageEventArgs> MaxDamageChanged;

        public DamagePool(MonoBehaviour owner)
        {
            Owner     = owner;
            Damage    = 0.0f;
            MinDamage = 0.0f;
            MaxDamage = 100.0f;

            _oldMinDamage = MinDamage;
            _oldMaxDamage = MaxDamage;
            _oldDamage    = Damage;

            Validate();
        }
        public DamagePool(MonoBehaviour owner, float damage, float minDamage, float maxDamage)
        {
            Owner     = owner;
            Damage    = damage;
            MinDamage = minDamage;
            MaxDamage = maxDamage;

            _oldMinDamage = MinDamage;
            _oldMaxDamage = MaxDamage;
            _oldDamage    = Damage;

            Validate();
        }

        public void SetDamage(float damage, DamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            if (!IsDamageable)
                return;

            Damage = Mathf.Clamp(damage, MinDamage, MaxDamage);

            if (!Mathf.Approximately(_oldDamage, Damage))
            {
                var eventArgs = new DamageEventArgs(_oldDamage, Damage, damageType, this, eventInstigator, damageCauser);
                _oldDamage    = Mathf.Clamp(Damage, MinDamage, MaxDamage);

                DamageChanged?.Invoke(eventArgs);
            }
        }
        public void ApplyDamage(float baseDamage, DamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            if (!IsDamageable)
                return;

            float modifiedDamage = damageType is not null ? damageType.GetModifiedDamage(baseDamage, this, eventInstigator, damageCauser) : baseDamage;
            SetDamage(Damage + modifiedDamage, damageType, eventInstigator, damageCauser);
        }

        public void SetMinDamage(float minDamage, MinDamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            MinDamage = Mathf.Min(minDamage, MaxDamage);
            Damage    = Mathf.Clamp(Damage, MinDamage, MaxDamage);

            if (!Mathf.Approximately(_oldMinDamage, MinDamage))
            {
                var eventArgs = new MinDamageEventArgs(_oldMinDamage, MinDamage, damageType, this, eventInstigator, damageCauser);
                _oldMinDamage = MinDamage;

                MinDamageChanged?.Invoke(eventArgs);
            }

            if (!Mathf.Approximately(_oldDamage, Damage))
            {
                var eventArgs = new DamageEventArgs(_oldDamage, Damage, DamageUtility.GetBuiltinDamageType<MinDamageChange_DamageType>(), this, eventInstigator, damageCauser);
                _oldDamage    = Damage;

                DamageChanged?.Invoke(eventArgs);
            }
        }
        public void ApplyMinDamage(float baseMinDamage, MinDamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            float modifiedMinDamage = damageType is not null ? damageType.GetModifiedDamage(baseMinDamage, this, eventInstigator, damageCauser) : baseMinDamage;
            SetMinDamage(MinDamage + modifiedMinDamage, damageType, eventInstigator, damageCauser);
        }

        public void SetMaxDamage(float maxDamage, MaxDamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            MaxDamage = Mathf.Max(MinDamage, maxDamage);
            Damage    = Mathf.Clamp(Damage, MinDamage, MaxDamage);

            if (!Mathf.Approximately(_oldMaxDamage, MaxDamage))
            {
                var eventArgs = new MaxDamageEventArgs(_oldMaxDamage, MaxDamage, damageType, this, eventInstigator, damageCauser);
                _oldMaxDamage = MaxDamage;

                MaxDamageChanged?.Invoke(eventArgs);
            }

            if (!Mathf.Approximately(_oldDamage, Damage))
            {
                var eventArgs = new DamageEventArgs(_oldDamage, Damage, DamageUtility.GetBuiltinDamageType<MaxDamageChange_DamageType>(), this, eventInstigator, damageCauser);
                _oldDamage    = Damage;

                DamageChanged?.Invoke(eventArgs);
            }
        }
        public void ApplyMaxDamage(float baseMaxDamage, MaxDamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            float modifiedMaxDamage = damageType is not null ? damageType.GetModifiedDamage(baseMaxDamage, this, eventInstigator, damageCauser) : baseMaxDamage;
            SetMaxDamage(MaxDamage + modifiedMaxDamage, damageType, eventInstigator, damageCauser);
        }

        public void Validate()
        {
            SetMinDamage(MinDamage, DamageUtility.GetBuiltinDamageType<ValidateMinDamage_MinDamageType>(), null, null);
            SetMaxDamage(MaxDamage, DamageUtility.GetBuiltinDamageType<ValidateMaxDamage_MaxDamageType>(), null, null);
            SetDamage(Damage, DamageUtility.GetBuiltinDamageType<ValidateDamage_DamageType>(), null, null);
        }
    }
}
