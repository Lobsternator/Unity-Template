using System.Collections;
using System.Collections.ObjectModel;
using Template.Core;
using Template.Saving;
using Template.Saving.Serialization;
using UnityEngine;

namespace Template.Gameplay
{
    /// <summary>
    /// Test script for showcasing the damage system.
    /// </summary>
    public class TestDamageableObject : MonoBehaviour, ISavableObject
    {
        [field: SerializeField]
        public DataKey DataKey { get; set; }

        [field: SerializeField]
        public DamagePool Damage { get; private set; } = new DamagePool(0.0f, 0.0f, 100.0f);

        private float _timeUntilNextDamage;

        private void OnDamageChanged(DamageChangeEventArgs damageChangeEventArgs)
        {
            Debug.Log($"Damage changed by {damageChangeEventArgs.DamageChange}!");
        }

        private IEnumerator DamageSelf(float damage, float interval)
        {
            _timeUntilNextDamage = interval;

            while (true)
            {
                yield return CoroutineUtility.WaitForFrames(1);
                _timeUntilNextDamage -= Time.deltaTime;

                if (_timeUntilNextDamage >= Mathf.Epsilon)
                    continue;

                Damage.ApplyDamageChange(damage, null, this, this);
                _timeUntilNextDamage = interval;
            }
        }

        private void OnEnable()
        {
            Damage.DamageChanged += OnDamageChanged;
            SaveManager.Saving   += GetSaveData;
            SaveManager.Loading  += LoadSaveData;
        }
        private void OnDisable()
        {
            SaveManager.Loading  -= LoadSaveData;
            SaveManager.Saving   -= GetSaveData;
            Damage.DamageChanged -= OnDamageChanged;
        }

        private void Awake()
        {
            Damage.Owner = this;
            StartCoroutine(DamageSelf(10.0f, 0.25f));
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Damage.Validate();
        }
#endif

        public SaveData GetSaveData()
        {
            var saveData = new SaveData(this);

            saveData.Data.AddItem(nameof(Damage.Damage), Damage.Damage);
            saveData.Data.AddItem(nameof(_timeUntilNextDamage), _timeUntilNextDamage);

            return saveData;
        }

        public void LoadSaveData(ReadOnlyDictionary<DataKey, SerializableObjectDataContainer> data)
        {
            if (!data.TryGetValue(DataKey, out var dataContainer))
                return;

            float damage = 0.0f;
            if (dataContainer.GetItem(nameof(Damage.Damage), ref damage))
                Damage.SetDamage(damage);

            dataContainer.GetItem(nameof(_timeUntilNextDamage), ref _timeUntilNextDamage);
        }
    }
}
