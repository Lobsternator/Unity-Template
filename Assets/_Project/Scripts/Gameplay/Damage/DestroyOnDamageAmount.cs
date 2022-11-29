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
            float currentAmount = eventArgs.NewDamage;

            if (Mathf.Approximately(currentAmount, targetAmount) || currentAmount > targetAmount)
                Destroy(gameObject);
        }

        private void OnEnable()
        {
            float currentAmount = _damageableObject.GetDamage();

            if (Mathf.Approximately(currentAmount, targetAmount) || currentAmount > targetAmount)
                Destroy(gameObject);
            else
                _damageableObject.DamageEvents.damageChanged.AddListener(OnDamageChanged);
        }
        private void OnDisable()
        {
            _damageableObject.DamageEvents.damageChanged.RemoveListener(OnDamageChanged);
        }

        private void Awake()
        {
            _damageableObject = GetComponent<DamageableObject>();
        }
    }
}
