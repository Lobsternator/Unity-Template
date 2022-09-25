using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Template.Core;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Template.Scenes
{
    [PersistentRuntimeObject(RuntimeInitializeLoadType.BeforeSceneLoad, -250)]
    public class SceneManager : PersistentRuntimeSingleton<SceneManager, SceneManagerData>
    {
        private Dictionary<int, AsyncOperation> _loadingSceneOperations   = new Dictionary<int, AsyncOperation>();
        private Dictionary<int, AsyncOperation> _unloadingSceneOperations = new Dictionary<int, AsyncOperation>();

        public bool IsSceneLoaded(int buildIndex)
        {
            return UnitySceneManager.GetSceneByBuildIndex(buildIndex).isLoaded;
        }
        public bool IsSceneUnloaded(int buildIndex)
        {
            return !UnitySceneManager.GetSceneByBuildIndex(buildIndex).isLoaded;
        }

        public bool IsSceneLoading(int buildIndex)
        {
            return _loadingSceneOperations.TryGetValue(buildIndex, out var operation) && !operation.isDone;
        }
        public bool IsSceneUnloading(int buildIndex)
        {
            return _unloadingSceneOperations.TryGetValue(buildIndex, out var operation) && !operation.isDone;
        }

        public AsyncOperation LoadSceneAsync(int buildIndex) => LoadSceneAsync(buildIndex, LoadSceneMode.Single);
        public AsyncOperation LoadSceneAsync(int buildIndex, LoadSceneMode loadMode)
        {
            bool sceneLoaded    = IsSceneLoaded(buildIndex);
            bool sceneLoading   = IsSceneLoading(buildIndex);
            bool sceneUnloading = IsSceneUnloading(buildIndex);

            if (!sceneLoaded && !sceneLoading && !sceneUnloading)
            {
                AsyncOperation operation = UnitySceneManager.LoadSceneAsync(buildIndex, loadMode);
                _loadingSceneOperations.Add(buildIndex, operation);

                return operation;
            }
            else
                return sceneLoading ? _loadingSceneOperations[buildIndex] : null;
        }

        public AsyncOperation UnloadSceneAsync(int buildIndex) => UnloadSceneAsync(buildIndex, UnloadSceneOptions.None);
        public AsyncOperation UnloadSceneAsync(int buildIndex, UnloadSceneOptions unloadOptions)
        {
            bool sceneUnloaded  = IsSceneUnloaded(buildIndex);
            bool sceneUnloading = IsSceneUnloading(buildIndex);
            bool sceneLoading   = IsSceneLoading(buildIndex);

            if (!sceneUnloaded && !sceneUnloading && !sceneLoading)
            {
                AsyncOperation operation = UnitySceneManager.UnloadSceneAsync(buildIndex, unloadOptions);
                _unloadingSceneOperations.Add(buildIndex, operation);

                return operation;
            }
            else
                return sceneUnloading ? _unloadingSceneOperations[buildIndex] : null;
        }

        public AsyncOperation WaitForSceneToLoad(int buildIndex)
        {
            return _loadingSceneOperations.TryGetValue(buildIndex, out var operation) ? operation : null;
        }
        public AsyncOperation WaitForSceneToUnload(int buildIndex)
        {
            return _unloadingSceneOperations.TryGetValue(buildIndex, out var operation) ? operation : null;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            _loadingSceneOperations.Remove(scene.buildIndex);
            _unloadingSceneOperations.Remove(scene.buildIndex);
        }
        private void OnSceneUnloaded(Scene scene)
        {
            _loadingSceneOperations.Remove(scene.buildIndex);
            _unloadingSceneOperations.Remove(scene.buildIndex);
        }

        protected override void Awake()
        {
            base.Awake();
            if (IsDuplicate)
                return;

            UnitySceneManager.sceneLoaded   += OnSceneLoaded;
            UnitySceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        private void OnDestroy()
        {
            UnitySceneManager.sceneLoaded   -= OnSceneLoaded;
            UnitySceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }
}
