using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Template.Core;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Template.Scenes
{
    [PersistentRuntimeObject(RuntimeInitializeLoadType.BeforeSceneLoad, -250)]
    public class ExtendedSceneManager : PersistentRuntimeSingleton<ExtendedSceneManager, ExtendedSceneManagerData>
    {
        private Dictionary<int, AsyncSceneOperation> _loadingSceneOperations   = new Dictionary<int, AsyncSceneOperation>();
        private Dictionary<int, AsyncSceneOperation> _unloadingSceneOperations = new Dictionary<int, AsyncSceneOperation>();

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
            return _loadingSceneOperations.TryGetValue(buildIndex, out var operation) && operation.keepWaiting;
        }
        public bool IsSceneUnloading(int buildIndex)
        {
            return _unloadingSceneOperations.TryGetValue(buildIndex, out var operation) && operation.keepWaiting;
        }

        public Scene GetActiveScene()
        {
            return UnitySceneManager.GetActiveScene();
        }
        public bool SetActiveScene(Scene scene)
        {
            return UnitySceneManager.SetActiveScene(scene);
        }

        public Scene GetSceneByPath(string scenePath)
        {
            return UnitySceneManager.GetSceneByPath(scenePath);
        }
        public Scene GetSceneByName(string name)
        {
            return UnitySceneManager.GetSceneByName(name);
        }
        public Scene GetSceneByBuildIndex(int buildIndex)
        {
            return UnitySceneManager.GetSceneByBuildIndex(buildIndex);
        }
        public Scene GetSceneAt(int index)
        {
            return UnitySceneManager.GetSceneAt(index);
        }

        public Scene CreateScene(string sceneName, CreateSceneParameters parameters)
        {
            return UnitySceneManager.CreateScene(sceneName, parameters);
        }
        public Scene CreateScene(string sceneName)
        {
            return UnitySceneManager.CreateScene(sceneName);
        }

        public void MergeScenes(Scene sourceScene, Scene destinationScene)
        {
            UnitySceneManager.MergeScenes(sourceScene, destinationScene);
        }

        public void MoveGameObjectToScene(GameObject go, Scene scene)
        {
            UnitySceneManager.MoveGameObjectToScene(go, scene);
        }

        private AsyncSceneOperation LoadSceneAsync_Single(int buildIndex, bool sceneLoaded, bool sceneLoading, bool sceneUnloaded, bool sceneUnloading)
        {
            for (int i = 0; i < UnitySceneManager.sceneCount; i++)
            {
                int otherBuildIndex = UnitySceneManager.GetSceneAt(i).buildIndex;
                if (otherBuildIndex == buildIndex || IsSceneUnloading(otherBuildIndex))
                    continue;

                if (IsSceneLoading(otherBuildIndex))
                {
                    AsyncSceneOperation loadOperation = GetLoadingOperation(otherBuildIndex);
                    loadOperation.completed          += (o) =>
                    {
                        if (IsSceneUnloading(o.BuildIndex))
                            return;

                        AsyncSceneOperation unloadOperation = new AsyncSceneOperation(UnitySceneManager.UnloadSceneAsync(otherBuildIndex), otherBuildIndex);
                        _unloadingSceneOperations.Add(otherBuildIndex, unloadOperation);
                    };
                }
                else
                {
                    AsyncSceneOperation unloadOperation = new AsyncSceneOperation(UnitySceneManager.UnloadSceneAsync(otherBuildIndex), otherBuildIndex);
                    _unloadingSceneOperations.Add(otherBuildIndex, unloadOperation);
                }
            }

            foreach (AsyncSceneOperation loadOperation in _loadingSceneOperations.Values)
            {
                loadOperation.completed += (o) =>
                {
                    if (IsSceneUnloading(o.BuildIndex))
                        return;

                    AsyncSceneOperation delayedOperation = new AsyncSceneOperation(UnitySceneManager.UnloadSceneAsync(o.BuildIndex), o.BuildIndex);
                    _unloadingSceneOperations.Add(buildIndex, delayedOperation);
                };
            }

            AsyncSceneOperation operation = null;

            if (sceneUnloading)
            {
                operation            = GetUnloadingOperation(buildIndex);
                operation.completed += (o) =>
                {
                    if (IsSceneLoading(buildIndex))
                        return;

                    AsyncSceneOperation delayedOperation = new AsyncSceneOperation(UnitySceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Single), buildIndex);
                    _loadingSceneOperations.Add(buildIndex, delayedOperation);
                };
            }
            else if (sceneLoaded)
                operation = new AsyncSceneOperation(UnitySceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Single), buildIndex);

            return operation;
        }
        private AsyncSceneOperation LoadSceneAsync_Additive(int buildIndex, bool sceneLoaded, bool sceneLoading, bool sceneUnloaded, bool sceneUnloading)
        {
            AsyncSceneOperation operation = null;
            if (sceneLoaded || sceneLoading)
                return operation;

            if (sceneUnloading)
            {
                operation            = GetUnloadingOperation(buildIndex);
                operation.completed += (o) =>
                {
                    if (IsSceneLoading(buildIndex))
                        return;

                    AsyncSceneOperation delayedOperation = new AsyncSceneOperation(UnitySceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive), buildIndex);
                    _loadingSceneOperations.Add(buildIndex, delayedOperation);
                };
            }
            else if (sceneUnloaded)
            {
                operation = new AsyncSceneOperation(UnitySceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive), buildIndex);
                _loadingSceneOperations.Add(buildIndex, operation);
            }

            return operation;
        }

        public AsyncSceneOperation LoadSceneAsync(int buildIndex) => LoadSceneAsync(buildIndex, LoadSceneMode.Single);
        public AsyncSceneOperation LoadSceneAsync(int buildIndex, LoadSceneMode loadMode)
        {
            AsyncSceneOperation operation = null;
            bool sceneLoaded              = IsSceneLoaded(buildIndex);
            bool sceneLoading             = IsSceneLoading(buildIndex);
            bool sceneUnloaded            = IsSceneUnloaded(buildIndex);
            bool sceneUnloading           = IsSceneUnloading(buildIndex);

            if (loadMode == LoadSceneMode.Single)
                operation = LoadSceneAsync_Single(buildIndex, sceneLoaded, sceneLoading, sceneUnloaded, sceneUnloading);

            else if (loadMode == LoadSceneMode.Additive)
                operation = LoadSceneAsync_Additive(buildIndex, sceneLoaded, sceneLoading, sceneUnloaded, sceneUnloading);

            return operation is null ? GetLoadingOperation(buildIndex) : operation;
        }

        public AsyncSceneOperation UnloadSceneAsync(int buildIndex) => UnloadSceneAsync(buildIndex, UnloadSceneOptions.None);
        public AsyncSceneOperation UnloadSceneAsync(int buildIndex, UnloadSceneOptions unloadOptions)
        {
            AsyncSceneOperation operation = null;
            bool sceneUnloaded            = IsSceneUnloaded(buildIndex);
            bool sceneUnloading           = IsSceneUnloading(buildIndex);
            bool sceneLoading             = IsSceneLoading(buildIndex);

            if (sceneLoading)
            {
                operation            = GetLoadingOperation(buildIndex);
                operation.completed += (o) =>
                {
                    if (IsSceneUnloading(buildIndex))
                        return;

                    AsyncSceneOperation delayedOperation = new AsyncSceneOperation(UnitySceneManager.UnloadSceneAsync(buildIndex, unloadOptions), buildIndex);
                    _loadingSceneOperations.Add(buildIndex, delayedOperation);
                };
            }
            else if (!sceneUnloaded && !sceneUnloading)
            {
                operation = new AsyncSceneOperation(UnitySceneManager.UnloadSceneAsync(buildIndex, unloadOptions), buildIndex);
                _unloadingSceneOperations.Add(buildIndex, operation);
            }

            return operation is null ? GetUnloadingOperation(buildIndex) : operation;
        }

        public AsyncSceneOperation GetLoadingOperation(int buildIndex)
        {
            return _loadingSceneOperations.TryGetValue(buildIndex, out var operation) ? operation : null;
        }
        public AsyncSceneOperation GetUnloadingOperation(int buildIndex)
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
        protected override void OnDestroy()
        {
            base.OnDestroy();

            UnitySceneManager.sceneLoaded   -= OnSceneLoaded;
            UnitySceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }
}
