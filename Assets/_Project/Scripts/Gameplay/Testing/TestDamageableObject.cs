using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Gameplay
{
    [RequireComponent(typeof(DamageableObject))]
    public class TestDamageableObject : MonoBehaviour
    {
        private DamageableObject _damageableObject;

        public void OnDamageChanged(float oldHealth, float newHealth)
        {
            Debug.Log("Damaged");
        }
        public void OnMinDamageChanged(float oldHealth, float newHealth)
        {
            Debug.Log("Min Damage Changed!");
        }
        public void OnMaxDamageChanged(float oldHealth, float newHealth)
        {
            Debug.Log("Max Damage Changed!");
        }
        public void OnDamageMaxed(float oldDamage, float newDamage)
        {
            Debug.Log("Max Destruction!");
        }

        IEnumerator DamageSelf(float amount, float interval)
        {
            while (this)
            {
                yield return new WaitForSeconds(interval);
                _damageableObject.AddDamage(amount);
            }
        }

        public void Awake()
        {
            _damageableObject = GetComponent<DamageableObject>();

            StartCoroutine(DamageSelf(10, 0.1f));
        }
    }
}
