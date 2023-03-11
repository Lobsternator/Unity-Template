using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Gameplay
{
    [Serializable]
    public class DamageEventArgs : EventArgs
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

    [Serializable]
    public class MinDamageEventArgs : EventArgs
    {
        public MinDamageEventArgs(float oldMinDamage, float newMinDamage, MinDamageType damageType, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            OldMinDamage    = oldMinDamage;
            NewMinDamage    = newMinDamage;
            DamageType      = damageType;
            DamagePool      = damagePool;
            EventInstigator = eventInstigator;
            DamageCauser    = damageCauser;
        }

        public float OldMinDamage { get; }
        public float NewMinDamage { get; }
        public MinDamageType DamageType { get; }

        public DamagePool DamagePool { get; }
        public MonoBehaviour EventInstigator { get; }
        public MonoBehaviour DamageCauser { get; }
    }

    [Serializable]
    public class MaxDamageEventArgs : EventArgs
    {
        public MaxDamageEventArgs(float oldMaxDamage, float newMaxDamage, MaxDamageType damageType, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            OldMaxDamage    = oldMaxDamage;
            NewMaxDamage    = newMaxDamage;
            DamageType      = damageType;
            DamagePool      = damagePool;
            EventInstigator = eventInstigator;
            DamageCauser    = damageCauser;
        }

        public float OldMaxDamage { get; }
        public float NewMaxDamage { get; }
        public MaxDamageType DamageType { get; }

        public DamagePool DamagePool { get; }
        public MonoBehaviour EventInstigator { get; }
        public MonoBehaviour DamageCauser { get; }
    }

    [Serializable]
    public abstract class DamageType : ScriptableObject
    {
        public abstract float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser);
    }

    [Serializable]
    public abstract class MinDamageType : ScriptableObject
    {
        public abstract float GetModifiedMinDamage(float baseMinDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser);
    }

    [Serializable]
    public abstract class MaxDamageType : ScriptableObject
    {
        public abstract float GetModifiedMaxDamage(float baseMaxDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser);
    }

    public sealed class MinDamageChangeDamageType : DamageType
    {
        public static MinDamageChangeDamageType StaticInstance { get; } = CreateInstance<MinDamageChangeDamageType>();

        public override float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            return baseDamage;
        }
    }

    public sealed class MaxDamageChangeDamageType : DamageType
    {
        public static MaxDamageChangeDamageType StaticInstance { get; } = CreateInstance<MaxDamageChangeDamageType>();

        public override float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            return baseDamage;
        }
    }

    [Serializable]
    public class DamagePool
    {
        public MonoBehaviour Owner { get; }
        public DamageManager DamageManager { get; }

        public DamagePool(MonoBehaviour owner)
        {
            Owner         = owner;
            DamageManager = new DamageManager(this);
        }
    }

    [Serializable]
    public class DamageManager
    {
        public DamagePool DamagePool { get; }
        public bool IsAtMinDamage => Mathf.Approximately(Damage, MinDamage);
        public bool IsAtMaxDamage => Mathf.Approximately(Damage, MaxDamage);
        [field: SerializeField] public bool IsDamageable { get; set; } = true;

        private float _oldDamage;
        [field: Space(7)]
        [field: SerializeField] public float Damage { get; private set; }

        private float _oldMinDamage;
        [field: SerializeField] public float MinDamage { get; private set; }

        private float _oldMaxDamage;
        [field: SerializeField] public float MaxDamage { get; private set; }

        public event Action<DamageEventArgs> DamageChanged;
        public event Action<MinDamageEventArgs> MinDamageChanged;
        public event Action<MaxDamageEventArgs> MaxDamageChanged;

        public DamageManager(DamagePool damagePool)
        {
            DamagePool = damagePool;
            Damage     = 0;
            MinDamage  = 0;
            MaxDamage  = 100;

            _oldMinDamage = MinDamage;
            _oldMaxDamage = MaxDamage;
            _oldDamage    = Damage;

            Validate();
        }
        public DamageManager(DamagePool damagePool, float damage, float minDamage, float maxDamage)
        {
            DamagePool = damagePool;
            Damage     = damage;
            MinDamage  = minDamage;
            MaxDamage  = maxDamage;

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
                DamageChanged?.Invoke(new DamageEventArgs(_oldDamage, Damage, damageType, DamagePool, eventInstigator, damageCauser));

            _oldDamage = Mathf.Clamp(Damage, MinDamage, MaxDamage);
        }
        public void ApplyDamage(float baseDamage, DamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            if (!IsDamageable)
                return;

            float modifiedDamage = damageType is not null ? damageType.GetModifiedDamage(baseDamage, DamagePool, eventInstigator, damageCauser) : baseDamage;
            SetDamage(modifiedDamage, damageType, eventInstigator, damageCauser);
        }

        public void SetMinDamage(float minDamage, MinDamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            MinDamage = Mathf.Min(minDamage, MaxDamage);
            Damage    = Mathf.Clamp(Damage, MinDamage, MaxDamage);

            if (!Mathf.Approximately(_oldMinDamage, MinDamage))
                MinDamageChanged?.Invoke(new MinDamageEventArgs(_oldMinDamage, MinDamage, damageType, DamagePool, eventInstigator, damageCauser));

            if (!Mathf.Approximately(_oldDamage, Damage))
                DamageChanged?.Invoke(new DamageEventArgs(_oldDamage, Damage, MinDamageChangeDamageType.StaticInstance, DamagePool, eventInstigator, damageCauser));

            _oldDamage    = Damage;
            _oldMinDamage = MinDamage;
        }
        public void ApplyMinDamage(float baseMinDamage, MinDamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            float modifiedMinDamage = damageType is not null ? damageType.GetModifiedMinDamage(baseMinDamage, DamagePool, eventInstigator, damageCauser) : baseMinDamage;
            SetMinDamage(modifiedMinDamage, damageType, eventInstigator, damageCauser);
        }

        public void SetMaxDamage(float maxDamage, MaxDamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            MaxDamage = Mathf.Max(MinDamage, maxDamage);
            Damage    = Mathf.Clamp(Damage, MinDamage, MaxDamage);

            if (!Mathf.Approximately(_oldMaxDamage, MaxDamage))
                MaxDamageChanged?.Invoke(new MaxDamageEventArgs(_oldMaxDamage, MaxDamage, damageType, DamagePool, eventInstigator, damageCauser));

            if (!Mathf.Approximately(_oldDamage, Damage))
                DamageChanged?.Invoke(new DamageEventArgs(_oldDamage, Damage, MaxDamageChangeDamageType.StaticInstance, DamagePool, eventInstigator, damageCauser));

            _oldDamage    = Damage;
            _oldMaxDamage = MaxDamage;
        }
        public void ApplyMaxDamage(float baseMaxDamage, MaxDamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            float modifiedMaxDamage = damageType is not null ? damageType.GetModifiedMaxDamage(baseMaxDamage, DamagePool, eventInstigator, damageCauser) : baseMaxDamage;
            SetMaxDamage(modifiedMaxDamage, damageType, eventInstigator, damageCauser);
        }

        public void Validate()
        {
            SetMinDamage(MinDamage, null, null, null);
            SetMaxDamage(MaxDamage, null, null, null);
            SetDamage(Damage, null, null, null);
        }
    }
}
