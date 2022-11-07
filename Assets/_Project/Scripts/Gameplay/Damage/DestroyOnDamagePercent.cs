using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Gameplay
{
    [RequireComponent(typeof(DamageableObject))]
    public class DestroyOnDamagePercent : MonoBehaviour
    {
        [Range(0f, 1f)]
        public float targetPercent;

        private DamageableObject _damageableObject;

        private void OnDamageChanged(DamageEventArgs eventArgs)
        {
            float currentPercent = eventArgs.NewValue / _damageableObject.DamageManager.MaxDamage;

            if (Mathf.Approximately(currentPercent, targetPercent) || currentPercent > targetPercent)
                Destroy(gameObject);
        }

        private void OnEnable()
        {
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
