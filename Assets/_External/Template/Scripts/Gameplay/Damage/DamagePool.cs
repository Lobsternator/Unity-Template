using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Core;

namespace Template.Gameplay
{
    public interface IDamageType
    {
        public float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser);
    }
    public abstract class DamageType : ScriptableObject, IDamageType
    {
        public abstract float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser);
    }

    /// <summary>
    /// Event arguments for when a <see cref="Gameplay.DamagePool"/>'s damage changes in some way.
    /// </summary>
    public class DamageChangeEventArgs : EventArgs
    {
        public float OldDamage { get; }
        public float NewDamage { get; }
        public float DamageChange => NewDamage - OldDamage;
        public IDamageType DamageType { get; }

        public DamagePool DamagePool { get; }
        public MonoBehaviour EventInstigator { get; }
        public MonoBehaviour DamageCauser { get; }

        public DamageChangeEventArgs(float oldDamage, float newDamage, IDamageType damageType, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            OldDamage       = oldDamage;
            NewDamage       = newDamage;
            DamageType      = damageType;
            DamagePool      = damagePool;
            EventInstigator = eventInstigator;
            DamageCauser    = damageCauser;
        }
    }

    /// <summary>
    /// Object for managing a pool of arbitrary "damage," within a restricted range.
    /// </summary>
    [Serializable]
    public class DamagePool
    {
        public MonoBehaviour Owner { get; set; }
        public bool IsAtMinDamage => Mathf.Approximately(Damage, MinDamage);
        public bool IsAtMaxDamage => Mathf.Approximately(Damage, MaxDamage);

        [field: Space(7)]
        [field: SerializeField]
        public float Damage { get; private set; } = 0.0f;
        private float _oldDamage;

        [field: SerializeField]
        public float MinDamage { get; private set; } = 0.0f;
        private float _oldMinDamage;

        [field: SerializeField]
        public float MaxDamage { get; private set; } = 100.0f;
        private float _oldMaxDamage;

        public delegate float ModifyDamageDelegate(float baseDamage, IDamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser);
        public ModifyDamageDelegate ModifyDamageChange { get; set; }
        public ModifyDamageDelegate ModifyMinDamageChange { get; set; }
        public ModifyDamageDelegate ModifyMaxDamageChange { get; set; }

        public event Action<DamageChangeEventArgs> DamageChanged;
        public event Action<DamageChangeEventArgs> MinDamageChanged;
        public event Action<DamageChangeEventArgs> MaxDamageChanged;

        public DamagePool()
        {
            Owner     = null;
            Damage    = 0.0f;
            MinDamage = 0.0f;
            MaxDamage = 100.0f;

            _oldMinDamage = MinDamage;
            _oldMaxDamage = MaxDamage;
            _oldDamage    = Damage;
        }
        public DamagePool(float damage, float minDamage, float maxDamage)
        {
            Owner     = null;
            Damage    = damage;
            MinDamage = minDamage;
            MaxDamage = maxDamage;

            _oldMinDamage = MinDamage;
            _oldMaxDamage = MaxDamage;
            _oldDamage    = Damage;

            Validate();
        }
        public DamagePool(MonoBehaviour owner)
        {
            Owner     = owner;
            Damage    = 0.0f;
            MinDamage = 0.0f;
            MaxDamage = 100.0f;

            _oldMinDamage = MinDamage;
            _oldMaxDamage = MaxDamage;
            _oldDamage    = Damage;
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

        public void SetDamage(float damage, IDamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            Damage = Mathf.Clamp(damage, MinDamage, MaxDamage);

            if (!Mathf.Approximately(_oldDamage, Damage))
            {
                var eventArgs = new DamageChangeEventArgs(_oldDamage, Damage, damageType, this, eventInstigator, damageCauser);
                _oldDamage    = Mathf.Clamp(Damage, MinDamage, MaxDamage);

                DamageChanged?.Invoke(eventArgs);
            }
        }
        public void SetDamage(float damage, IDamageType damageType, MonoBehaviour eventInstigator) => SetDamage(damage, damageType, eventInstigator, null);
        public void SetDamage(float damage, IDamageType damageType) => SetDamage(damage, damageType, null, null);
        public void SetDamage(float damage) => SetDamage(damage, null, null, null);

        public void ApplyDamageChange(float baseDamage, IDamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            float modifiedDamage = damageType is not null ? damageType.GetModifiedDamage(baseDamage, this, eventInstigator, damageCauser) : baseDamage;

            if (ModifyDamageChange is not null)
                modifiedDamage = ModifyDamageChange(modifiedDamage, damageType, eventInstigator, damageCauser);

            SetDamage(Damage + modifiedDamage, damageType, eventInstigator, damageCauser);
        }
        public void ApplyDamageChange(float baseDamage, IDamageType damageType, MonoBehaviour eventInstigator) => ApplyDamageChange(baseDamage, damageType, eventInstigator, null);
        public void ApplyDamageChange(float baseDamage, IDamageType damageType) => ApplyDamageChange(baseDamage, damageType, null, null);
        public void ApplyDamageChange(float baseDamage) => ApplyDamageChange(baseDamage, null, null, null);

        public void SetMinDamage(float minDamage, IDamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            MinDamage = Mathf.Min(minDamage, MaxDamage);
            Damage    = Mathf.Clamp(Damage, MinDamage, MaxDamage);

            if (!Mathf.Approximately(_oldMinDamage, MinDamage))
            {
                var eventArgs = new DamageChangeEventArgs(_oldMinDamage, MinDamage, damageType, this, eventInstigator, damageCauser);
                _oldMinDamage = MinDamage;

                MinDamageChanged?.Invoke(eventArgs);
            }

            if (!Mathf.Approximately(_oldDamage, Damage))
            {
                var eventArgs = new DamageChangeEventArgs(_oldDamage, Damage, new DamageType_MinDamageChange(), this, eventInstigator, damageCauser);
                _oldDamage    = Damage;

                DamageChanged?.Invoke(eventArgs);
            }
        }
        public void SetMinDamage(float minDamage, IDamageType damageType, MonoBehaviour eventInstigator) => SetMinDamage(minDamage, damageType, eventInstigator, null);
        public void SetMinDamage(float minDamage, IDamageType damageType) => SetMinDamage(minDamage, damageType, null, null);
        public void SetMinDamage(float minDamage) => SetMinDamage(minDamage, null, null, null);

        public void ApplyMinDamageChange(float baseMinDamage, IDamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            float modifiedMinDamage = damageType is not null ? damageType.GetModifiedDamage(baseMinDamage, this, eventInstigator, damageCauser) : baseMinDamage;

            if (ModifyMinDamageChange is not null)
                modifiedMinDamage = ModifyMinDamageChange(modifiedMinDamage, damageType, eventInstigator, damageCauser);

            SetMinDamage(MinDamage + modifiedMinDamage, damageType, eventInstigator, damageCauser);
        }
        public void ApplyMinDamageChange(float baseMinDamage, IDamageType damageType, MonoBehaviour eventInstigator) => ApplyMinDamageChange(baseMinDamage, damageType, eventInstigator, null);
        public void ApplyMinDamageChange(float baseMinDamage, IDamageType damageType) => ApplyMinDamageChange(baseMinDamage, damageType, null, null);
        public void ApplyMinDamageChange(float baseMinDamage) => ApplyMinDamageChange(baseMinDamage, null, null, null);

        public void SetMaxDamage(float maxDamage, IDamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            MaxDamage = Mathf.Max(MinDamage, maxDamage);
            Damage    = Mathf.Clamp(Damage, MinDamage, MaxDamage);

            if (!Mathf.Approximately(_oldMaxDamage, MaxDamage))
            {
                var eventArgs = new DamageChangeEventArgs(_oldMaxDamage, MaxDamage, damageType, this, eventInstigator, damageCauser);
                _oldMaxDamage = MaxDamage;

                MaxDamageChanged?.Invoke(eventArgs);
            }

            if (!Mathf.Approximately(_oldDamage, Damage))
            {
                var eventArgs = new DamageChangeEventArgs(_oldDamage, Damage, new DamageType_MaxDamageChange(), this, eventInstigator, damageCauser);
                _oldDamage    = Damage;

                DamageChanged?.Invoke(eventArgs);
            }
        }
        public void SetMaxDamage(float maxDamage, IDamageType damageType, MonoBehaviour eventInstigator) => SetMaxDamage(maxDamage, damageType, eventInstigator, null);
        public void SetMaxDamage(float maxDamage, IDamageType damageType) => SetMaxDamage(maxDamage, damageType, null, null);
        public void SetMaxDamage(float maxDamage) => SetMaxDamage(maxDamage, null, null, null);

        public void ApplyMaxDamageChange(float baseMaxDamage, IDamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            float modifiedMaxDamage = damageType is not null ? damageType.GetModifiedDamage(baseMaxDamage, this, eventInstigator, damageCauser) : baseMaxDamage;

            if (ModifyMaxDamageChange is not null)
                modifiedMaxDamage = ModifyMaxDamageChange(modifiedMaxDamage, damageType, eventInstigator, damageCauser);

            SetMaxDamage(MaxDamage + modifiedMaxDamage, damageType, eventInstigator, damageCauser);
        }
        public void ApplyMaxDamageChange(float baseMaxDamage, IDamageType damageType, MonoBehaviour eventInstigator) => ApplyMaxDamageChange(baseMaxDamage, damageType, eventInstigator, null);
        public void ApplyMaxDamageChange(float baseMaxDamage, IDamageType damageType) => ApplyMaxDamageChange(baseMaxDamage, damageType, null, null);
        public void ApplyMaxDamageChange(float baseMaxDamage) => ApplyMaxDamageChange(baseMaxDamage, null, null, null);

        public void Validate()
        {
            SetMinDamage(MinDamage, new DamageType_ValidateMinDamage());
            SetMaxDamage(MaxDamage, new DamageType_ValidateMaxDamage());
            SetDamage(Damage,       new DamageType_ValidateDamage());
        }
    }
}
