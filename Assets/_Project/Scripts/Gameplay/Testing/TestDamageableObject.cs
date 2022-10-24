using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Gameplay
{
    [RequireComponent(typeof(DamageableObject))]
    public class TestDamageableObject : MonoBehaviour
    {
        private DamageableObject _damageableObject;

        public void OnDamageChanged(DamageEventArgs eventArgs)
        {
            Debug.Log("Damaged");
        }
        public void OnMinDamageChanged(DamageEventArgs eventArgs)
        {
            Debug.Log("Min Damage Changed!");
        }
        public void OnMaxDamageChanged(DamageEventArgs eventArgs)
        {
            Debug.Log("Max Damage Changed!");
        }
        public void OnDamageMaxed(DamageEventArgs eventArgs)
        {
            Debug.Log("Max Destruction!");
        }

        IEnumerator DamageSelf(float amount, float interval)
        {
            while (this)
            {
                yield return new WaitForSeconds(interval);
                _damageableObject.ApplyDamage(amount, null, this, this);
            }
        }

        public void Awake()
        {
            _damageableObject = GetComponent<DamageableObject>();

            StartCoroutine(DamageSelf(10, 0.1f));
        }
    }
}
