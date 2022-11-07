using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Gameplay
{
    [RequireComponent(typeof(DamageableObject))]
    public class DestroyOnDamageAmount : MonoBehaviour
    {
        public float targetAmount;

        private DamageableObject _damageableObject;

        private void OnDamageChanged(DamageEventArgs eventArgs)
        {
            float currentAmount = eventArgs.NewValue;

            if (Mathf.Approximately(currentAmount, targetAmount) || eventArgs.NewValue > targetAmount)
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
