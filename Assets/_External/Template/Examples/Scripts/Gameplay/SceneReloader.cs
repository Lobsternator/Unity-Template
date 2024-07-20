using System.Collections;
using System.Collections.ObjectModel;
using Template.Core;
using Template.Saving;
using Template.Saving.Serialization;
using Template.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Template.Gameplay
{
    /// <summary>
    /// Test script that simply reloads the scene according to <see cref="reloadInterval"/>.
    /// </summary>
    public class SceneReloader : MonoBehaviour, ISavableObject
    {
        [field: SerializeField]
        public DataKey DataKey { get; set; }

        [SerializeField]
        private float reloadInterval = 1.0f;

        private float _timeUntilNextReload;

        private IEnumerator ReloadScene()
        {
            _timeUntilNextReload = reloadInterval;

            while (true)
            {
                yield return CoroutineUtility.WaitForFrames(1);
                _timeUntilNextReload -= Time.deltaTime;

                if (_timeUntilNextReload >= Mathf.Epsilon)
                    continue;

                Scene activeScene = ExtendedSceneManager.GetActiveScene();
                if (!activeScene.isLoaded)
                    continue;

                ExtendedSceneManager.LoadSceneAsync(activeScene.buildIndex, LoadSceneMode.Single);
                _timeUntilNextReload = reloadInterval;
            }
        }

        private void OnEnable()
        {
            SaveManager.Saving  += GetSaveData;
            SaveManager.Loading += LoadSaveData;

            StartCoroutine(ReloadScene());
        }
        private void OnDisable()
        {
            StopAllCoroutines();

            SaveManager.Loading -= LoadSaveData;
            SaveManager.Saving  -= GetSaveData;
        }

        public SaveData GetSaveData()
        {
            var saveData = new SaveData(this);

            saveData.Data.AddItem(nameof(_timeUntilNextReload), _timeUntilNextReload);

            return saveData;
        }

        public void LoadSaveData(ReadOnlyDictionary<DataKey, SerializableObjectDataContainer> data)
        {
            if (!data.TryGetValue(DataKey, out var dataContainer))
                return;

            dataContainer.GetItem(nameof(_timeUntilNextReload), ref _timeUntilNextReload);
        }
    }
}
