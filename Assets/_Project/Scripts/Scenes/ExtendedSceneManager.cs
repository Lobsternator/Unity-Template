using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Template.Core;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Template.Scenes
{
    public static class ExtendedSceneManager
    {
        public static int sceneCount => UnitySceneManager.sceneCount;
        public static int sceneCountInBuildSettings => UnitySceneManager.sceneCountInBuildSettings;

        private static Dictionary<int, AsyncSceneOperation> _loadingSceneOperations   = new Dictionary<int, AsyncSceneOperation>();
        private static Dictionary<int, AsyncSceneOperation> _unloadingSceneOperations = new Dictionary<int, AsyncSceneOperation>();

        static ExtendedSceneManager()
        {
            UnitySceneManager.sceneLoaded   -= OnSceneLoaded;
            UnitySceneManager.sceneLoaded   -= OnSceneLoaded;
            UnitySceneManager.sceneUnloaded -= OnSceneUnloaded;
            UnitySceneManager.sceneUnloaded -= OnSceneUnloaded;

            UnitySceneManager.sceneLoaded   += OnSceneLoaded;
            UnitySceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        public static string GetFullScenePath(string localScenePath)
        {
            return Path.Combine(PersistentPathData.Instance.SceneFolderPath, localScenePath + ".unity").Replace('\\', '/');
        }

        public static bool IsSceneLoaded(int buildIndex)
        {
            return UnitySceneManager.GetSceneByBuildIndex(buildIndex).isLoaded;
        }
        public static bool IsSceneUnloaded(int buildIndex)
        {
            return !UnitySceneManager.GetSceneByBuildIndex(buildIndex).isLoaded;
        }

        public static bool IsSceneLoading(int buildIndex)
        {
            return _loadingSceneOperations.TryGetValue(buildIndex, out var operation) && operation.keepWaiting;
        }
        public static bool IsSceneUnloading(int buildIndex)
        {
            return _unloadingSceneOperations.TryGetValue(buildIndex, out var operation) && operation.keepWaiting;
        }

        public static bool IsSceneLoaded(string localScenePath)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return IsSceneLoaded(buildIndex);
        }
        public static bool IsSceneLoading(string localScenePath)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return IsSceneLoading(buildIndex);
        }

        public static bool AreScenesLoaded(params int[] buildIndices)
        {
            foreach (int buildIndex in buildIndices)
                if (!IsSceneLoaded(buildIndex))
                    return false;

            return true;
        }
        public static bool AreScenesLoading(params int[] buildIndices)
        {
            foreach (int buildIndex in buildIndices)
                if (IsSceneLoading(buildIndex))
                    return true;

            return false;
        }

        public static bool AreScenesLoaded(params string[] localScenePaths)
        {
            foreach (string localScenePath in localScenePaths)
                if (!IsSceneLoaded(localScenePath))
                    return false;

            return true;
        }
        public static bool AreScenesLoading(params string[] localScenePaths)
        {
            foreach (string localScenePath in localScenePaths)
                if (IsSceneLoading(localScenePath))
                    return true;

            return false;
        }

        public static bool IsSceneUnloaded(string localScenePath)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return IsSceneUnloaded(buildIndex);
        }
        public static bool IsSceneUnloading(string localScenePath)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return IsSceneUnloading(buildIndex);
        }

        public static bool AreScenesUnloaded(params int[] buildIndices)
        {
            foreach (int buildIndex in buildIndices)
                if (!IsSceneUnloaded(buildIndex))
                    return false;

            return true;
        }
        public static bool AreScenesUnloading(params int[] buildIndices)
        {
            foreach (int buildIndex in buildIndices)
                if (IsSceneUnloading(buildIndex))
                    return true;

            return false;
        }

        public static bool AreScenesUnloaded(params string[] localScenePaths)
        {
            foreach (string localScenePath in localScenePaths)
                if (!IsSceneUnloaded(localScenePath))
                    return false;

            return true;
        }
        public static bool AreScenesUnloading(params string[] localScenePaths)
        {
            foreach (string localScenePath in localScenePaths)
                if (IsSceneUnloading(localScenePath))
                    return true;

            return false;
        }

        public static Scene GetActiveScene()
        {
            return UnitySceneManager.GetActiveScene();
        }
        public static bool SetActiveScene(Scene scene)
        {
            return UnitySceneManager.SetActiveScene(scene);
        }

        public static Scene GetSceneByPath(string scenePath)
        {
            return UnitySceneManager.GetSceneByPath(scenePath);
        }
        public static Scene GetSceneByName(string name)
        {
            return UnitySceneManager.GetSceneByName(name);
        }
        public static Scene GetSceneByBuildIndex(int buildIndex)
        {
            return UnitySceneManager.GetSceneByBuildIndex(buildIndex);
        }
        public static Scene GetSceneAt(int index)
        {
            return UnitySceneManager.GetSceneAt(index);
        }

        public static Scene CreateScene(string sceneName, CreateSceneParameters parameters)
        {
            return UnitySceneManager.CreateScene(sceneName, parameters);
        }
        public static Scene CreateScene(string sceneName)
        {
            return UnitySceneManager.CreateScene(sceneName);
        }

        public static void MergeScenes(Scene sourceScene, Scene destinationScene)
        {
            UnitySceneManager.MergeScenes(sourceScene, destinationScene);
        }

        public static void MoveGameObjectToScene(GameObject go, Scene scene)
        {
            UnitySceneManager.MoveGameObjectToScene(go, scene);
        }

        private static AsyncSceneOperation LoadSceneAsync_Single(int buildIndex, bool sceneLoaded, bool sceneLoading, bool sceneUnloaded, bool sceneUnloading)
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
        private static AsyncSceneOperation LoadSceneAsync_Additive(int buildIndex, bool sceneLoaded, bool sceneLoading, bool sceneUnloaded, bool sceneUnloading)
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

        public static AsyncSceneOperation LoadSceneAsync(int buildIndex) => LoadSceneAsync(buildIndex, LoadSceneMode.Single);
        public static AsyncSceneOperation LoadSceneAsync(int buildIndex, LoadSceneMode loadMode)
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

        public static AsyncSceneOperation UnloadSceneAsync(int buildIndex) => UnloadSceneAsync(buildIndex, UnloadSceneOptions.None);
        public static AsyncSceneOperation UnloadSceneAsync(int buildIndex, UnloadSceneOptions unloadOptions)
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

        public static AsyncSceneOperation LoadSceneAsync(string localScenePath) => LoadSceneAsync(localScenePath, LoadSceneMode.Single);
        public static AsyncSceneOperation LoadSceneAsync(string localScenePath, LoadSceneMode loadMode)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return LoadSceneAsync(buildIndex, loadMode);
        }

        public static AsyncSceneOperationCollection LoadScenesAsync(params int[] buildIndices) => LoadScenesAsync(LoadSceneMode.Single, buildIndices);
        public static AsyncSceneOperationCollection LoadScenesAsync(LoadSceneMode loadMode, params int[] buildIndices)
        {
            AsyncSceneOperationCollection operations = new AsyncSceneOperationCollection(buildIndices.Length);

            foreach (int buildIndex in buildIndices)
                operations.AddOperation(LoadSceneAsync(buildIndex, loadMode));

            return operations;
        }

        public static AsyncSceneOperationCollection LoadScenesAsync(params string[] localScenePaths) => LoadScenesAsync(LoadSceneMode.Single, localScenePaths);
        public static AsyncSceneOperationCollection LoadScenesAsync(LoadSceneMode loadMode, params string[] localScenePaths)
        {
            AsyncSceneOperationCollection operations = new AsyncSceneOperationCollection(localScenePaths.Length);

            foreach (string localScenePath in localScenePaths)
                operations.AddOperation(LoadSceneAsync(localScenePath, loadMode));

            return operations;
        }

        public static AsyncSceneOperation UnloadSceneAsync(string localScenePath) => UnloadSceneAsync(localScenePath, UnloadSceneOptions.None);
        public static AsyncSceneOperation UnloadSceneAsync(string localScenePath, UnloadSceneOptions unloadOptions)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return UnloadSceneAsync(buildIndex, unloadOptions);
        }

        public static AsyncSceneOperationCollection UnloadScenesAsync(params int[] buildIndices) => UnloadScenesAsync(UnloadSceneOptions.None, buildIndices);
        public static AsyncSceneOperationCollection UnloadScenesAsync(UnloadSceneOptions unloadOptions, params int[] buildIndices)
        {
            AsyncSceneOperationCollection operations = new AsyncSceneOperationCollection(buildIndices.Length);

            foreach (int buildIndex in buildIndices)
                operations.AddOperation(UnloadSceneAsync(buildIndex, unloadOptions));

            return operations;
        }

        public static AsyncSceneOperationCollection UnloadScenesAsync(params string[] localScenePaths) => UnloadScenesAsync(UnloadSceneOptions.None, localScenePaths);
        public static AsyncSceneOperationCollection UnloadScenesAsync(UnloadSceneOptions unloadOptions, params string[] localScenePaths)
        {
            AsyncSceneOperationCollection operations = new AsyncSceneOperationCollection(localScenePaths.Length);

            foreach (string localScenePath in localScenePaths)
                operations.AddOperation(UnloadSceneAsync(localScenePath, unloadOptions));

            return operations;
        }

        public static AsyncSceneOperation GetLoadingOperation(int buildIndex)
        {
            return _loadingSceneOperations.TryGetValue(buildIndex, out var operation) ? operation : null;
        }
        public static AsyncSceneOperation GetUnloadingOperation(int buildIndex)
        {
            return _unloadingSceneOperations.TryGetValue(buildIndex, out var operation) ? operation : null;
        }

        public static AsyncSceneOperation GetLoadingOperation(string localScenePath)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return GetLoadingOperation(buildIndex);
        }

        public static AsyncSceneOperationCollection GetLoadingOperations(params int[] buildIndices)
        {
            AsyncSceneOperationCollection operations = new AsyncSceneOperationCollection(buildIndices.Length);

            foreach (int buildIndex in buildIndices)
                operations.AddOperation(GetLoadingOperation(buildIndex));

            return operations;
        }
        public static AsyncSceneOperationCollection GetLoadingOperations(params string[] localScenePaths)
        {
            AsyncSceneOperationCollection operations = new AsyncSceneOperationCollection(localScenePaths.Length);

            foreach (string localScenePath in localScenePaths)
                operations.AddOperation(GetLoadingOperation(localScenePath));

            return operations;
        }

        public static AsyncSceneOperation GetUnloadingOperation(string localScenePath)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return GetUnloadingOperation(buildIndex);
        }

        public static AsyncSceneOperationCollection GetUnloadingOperations(params int[] buildIndices)
        {
            AsyncSceneOperationCollection operations = new AsyncSceneOperationCollection(buildIndices.Length);

            foreach (int buildIndex in buildIndices)
                operations.AddOperation(GetUnloadingOperation(buildIndex));

            return operations;
        }
        public static AsyncSceneOperationCollection GetUnloadingOperations(params string[] localScenePaths)
        {
            AsyncSceneOperationCollection operations = new AsyncSceneOperationCollection(localScenePaths.Length);

            foreach (string localScenePath in localScenePaths)
                operations.AddOperation(GetUnloadingOperation(localScenePath));

            return operations;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            _loadingSceneOperations.Remove(scene.buildIndex);
            _unloadingSceneOperations.Remove(scene.buildIndex);
        }
        private static void OnSceneUnloaded(Scene scene)
        {
            _loadingSceneOperations.Remove(scene.buildIndex);
            _unloadingSceneOperations.Remove(scene.buildIndex);
        }
    }
}
