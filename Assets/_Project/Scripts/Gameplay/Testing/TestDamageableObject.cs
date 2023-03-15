using System.Collections;
using System.Collections.Generic;
using Template.Gameplay;
using UnityEngine;

namespace Template
{
    public class TestDamageableObject : MonoBehaviour
    {
        [field: SerializeField] public DamagePool Damage { get; private set; }

        private void OnDamageChanged(DamageEventArgs damageEventArgs)
        {
            Debug.Log($"Damaged for {damageEventArgs.GetDamageDifference()}!");
        }

        private IEnumerator DamageSelf(float damage, float interval)
        {
            while (true)
            {
                yield return new WaitForSeconds(interval);

                Damage.ApplyDamage(damage, null, this, this);
            }
        }

        private void OnEnable()
        {
            Damage.DamageChanged += OnDamageChanged;
        }
        private void OnDisable()
        {
            Damage.DamageChanged -= OnDamageChanged;
        }

        private void Awake()
        {
            Damage = new DamagePool(this);

            StartCoroutine(DamageSelf(10.0f, 0.25f));
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Damage.Validate();
        }
#endif
    }
}
