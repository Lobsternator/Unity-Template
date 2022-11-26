using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Gameplay
{
    [RequireComponent(typeof(DamageableObject))]
    public class DestroyOnDamageFraction : MonoBehaviour
    {
        [Range(0f, 1f)]
        public float targetFraction;

        private DamageableObject _damageableObject;

        private void OnDamageChanged(DamageEventArgs eventArgs)
        {
            float currentFraction = eventArgs.NewDamage / _damageableObject.GetMaxDamage();

            if (Mathf.Approximately(currentFraction, targetFraction) || currentFraction > targetFraction)
                Destroy(gameObject);
        }

        private void OnEnable()
        {
            float currentFraction = _damageableObject.GetDamage() / _damageableObject.GetMaxDamage();

            if (Mathf.Approximately(currentFraction, targetFraction) || currentFraction > targetFraction)
                Destroy(gameObject);
            else
                _damageableObject.DamageEvents.DamageChanged.AddListener(OnDamageChanged);
        }
        private void OnDisable()
        {
            _damageableObject.DamageEvents.DamageChanged.RemoveListener(OnDamageChanged);
        }

        private void Awake()
        {
            _damageableObject = GetComponent<DamageableObject>();
        }
    }
}
