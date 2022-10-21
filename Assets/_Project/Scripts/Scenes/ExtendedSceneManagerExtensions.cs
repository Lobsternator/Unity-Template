using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Template.Core;

namespace Template.Scenes
{
    public static class ExtendedSceneManagerExtensions
    {
        private static ExtendedSceneManagerData _sceneManagerData = PersistentRuntimeObjectUtility.GetPersistentData<ExtendedSceneManagerData>();

        private static string GetFullScenePath(string localScenePath)
        {
            return Path.Combine(_sceneManagerData.pathToSceneFolder, localScenePath + ".unity").Replace('\\', '/');
        }

        public static bool IsSceneLoaded(this ExtendedSceneManager sceneManager, string localScenePath)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return sceneManager.IsSceneLoaded(buildIndex);
        }
        public static bool IsSceneLoading(this ExtendedSceneManager sceneManager, string localScenePath)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return sceneManager.IsSceneLoading(buildIndex);
        }

        public static bool AreScenesLoaded(this ExtendedSceneManager sceneManager, params int[] buildIndices)
        {
            bool result = true;

            foreach (int buildIndex in buildIndices)
                result = result && sceneManager.IsSceneLoaded(buildIndex);

            return result;
        }
        public static bool AreScenesLoading(this ExtendedSceneManager sceneManager, params int[] buildIndices)
        {
            bool result = true;

            foreach (int buildIndex in buildIndices)
                result = result && sceneManager.IsSceneLoading(buildIndex);

            return result;
        }

        public static bool AreScenesLoaded(this ExtendedSceneManager sceneManager, params string[] localScenePaths)
        {
            bool result = true;

            foreach (string localScenePath in localScenePaths)
                result = result && IsSceneLoaded(sceneManager, localScenePath);

            return result;
        }
        public static bool AreScenesLoading(this ExtendedSceneManager sceneManager, params string[] localScenePaths)
        {
            bool result = true;

            foreach (string localScenePath in localScenePaths)
                result = result && IsSceneLoading(sceneManager, localScenePath);

            return result;
        }

        public static bool IsSceneUnloaded(this ExtendedSceneManager sceneManager, string localScenePath)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return sceneManager.IsSceneUnloaded(buildIndex);
        }
        public static bool IsSceneUnloading(this ExtendedSceneManager sceneManager, string localScenePath)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return sceneManager.IsSceneUnloading(buildIndex);
        }

        public static bool AreScenesUnoaded(this ExtendedSceneManager sceneManager, params int[] buildIndices)
        {
            bool result = true;

            foreach (int buildIndex in buildIndices)
                result = result && sceneManager.IsSceneUnloaded(buildIndex);

            return result;
        }
        public static bool AreScenesUnloading(this ExtendedSceneManager sceneManager, params int[] buildIndices)
        {
            bool result = true;

            foreach (int buildIndex in buildIndices)
                result = result && sceneManager.IsSceneUnloading(buildIndex);

            return result;
        }

        public static bool AreScenesUnoaded(this ExtendedSceneManager sceneManager, params string[] localScenePaths)
        {
            bool result = true;

            foreach (string localScenePath in localScenePaths)
                result = result && IsSceneUnloaded(sceneManager, localScenePath);

            return result;
        }
        public static bool AreScenesUnloading(this ExtendedSceneManager sceneManager, params string[] localScenePaths)
        {
            bool result = true;

            foreach (string localScenePath in localScenePaths)
                result = result && IsSceneUnloading(sceneManager, localScenePath);

            return result;
        }

        public static AsyncSceneOperation LoadSceneAsync(this ExtendedSceneManager sceneManager, string localScenePath) => LoadSceneAsync(sceneManager, localScenePath, LoadSceneMode.Single);
        public static AsyncSceneOperation LoadSceneAsync(this ExtendedSceneManager sceneManager, string localScenePath, LoadSceneMode loadMode)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return sceneManager.LoadSceneAsync(buildIndex, loadMode);
        }

        public static AsyncSceneOperationCollection LoadScenesAsync(this ExtendedSceneManager sceneManager, params int[] buildIndices) => LoadScenesAsync(sceneManager, LoadSceneMode.Single, buildIndices);
        public static AsyncSceneOperationCollection LoadScenesAsync(this ExtendedSceneManager sceneManager, LoadSceneMode loadMode, params int[] buildIndices)
        {
            AsyncSceneOperationCollection operations = new AsyncSceneOperationCollection(buildIndices.Length);

            foreach (int buildIndex in buildIndices)
                operations.AddOperation(sceneManager.LoadSceneAsync(buildIndex, loadMode));

            return operations;
        }

        public static AsyncSceneOperationCollection LoadScenesAsync(this ExtendedSceneManager sceneManager, params string[] localScenePaths) => LoadScenesAsync(sceneManager, LoadSceneMode.Single, localScenePaths);
        public static AsyncSceneOperationCollection LoadScenesAsync(this ExtendedSceneManager sceneManager, LoadSceneMode loadMode, params string[] localScenePaths)
        {
            AsyncSceneOperationCollection operations = new AsyncSceneOperationCollection(localScenePaths.Length);

            foreach (string localScenePath in localScenePaths)
                operations.AddOperation(LoadSceneAsync(sceneManager, localScenePath, loadMode));

            return operations;
        }

        public static AsyncSceneOperation UnloadSceneAsync(this ExtendedSceneManager sceneManager, string localScenePath) => UnloadSceneAsync(sceneManager, localScenePath, UnloadSceneOptions.None);
        public static AsyncSceneOperation UnloadSceneAsync(this ExtendedSceneManager sceneManager, string localScenePath, UnloadSceneOptions unloadOptions)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return sceneManager.UnloadSceneAsync(buildIndex, unloadOptions);
        }

        public static AsyncSceneOperationCollection UnloadScenesAsync(this ExtendedSceneManager sceneManager, params int[] buildIndices) => UnloadScenesAsync(sceneManager, UnloadSceneOptions.None, buildIndices);
        public static AsyncSceneOperationCollection UnloadScenesAsync(this ExtendedSceneManager sceneManager, UnloadSceneOptions unloadOptions, params int[] buildIndices)
        {
            AsyncSceneOperationCollection operations = new AsyncSceneOperationCollection(buildIndices.Length);

            foreach (int buildIndex in buildIndices)
                operations.AddOperation(sceneManager.UnloadSceneAsync(buildIndex, unloadOptions));

            return operations;
        }

        public static AsyncSceneOperationCollection UnloadScenesAsync(this ExtendedSceneManager sceneManager, params string[] localScenePaths) => UnloadScenesAsync(sceneManager, UnloadSceneOptions.None, localScenePaths);
        public static AsyncSceneOperationCollection UnloadScenesAsync(this ExtendedSceneManager sceneManager, UnloadSceneOptions unloadOptions, params string[] localScenePaths)
        {
            AsyncSceneOperationCollection operations = new AsyncSceneOperationCollection(localScenePaths.Length);

            foreach (string localScenePath in localScenePaths)
                operations.AddOperation(UnloadSceneAsync(sceneManager, localScenePath, unloadOptions));

            return operations;
        }

        public static AsyncSceneOperation GetLoadingOperation(this ExtendedSceneManager sceneManager, string localScenePath)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return sceneManager.GetLoadingOperation(buildIndex);
        }

        public static AsyncSceneOperationCollection GetLoadingOperations(this ExtendedSceneManager sceneManager, params int[] buildIndices)
        {
            AsyncSceneOperationCollection operations = new AsyncSceneOperationCollection(buildIndices.Length);

            foreach (int buildIndex in buildIndices)
                operations.AddOperation(sceneManager.GetLoadingOperation(buildIndex));

            return operations;
        }
        public static AsyncSceneOperationCollection GetLoadingOperations(this ExtendedSceneManager sceneManager, params string[] localScenePaths)
        {
            AsyncSceneOperationCollection operations = new AsyncSceneOperationCollection(localScenePaths.Length);

            foreach (string localScenePath in localScenePaths)
                operations.AddOperation(GetLoadingOperation(sceneManager, localScenePath));

            return operations;
        }

        public static AsyncSceneOperation GetUnloadingOperation(this ExtendedSceneManager sceneManager, string localScenePath)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return sceneManager.GetUnloadingOperation(buildIndex);
        }

        public static AsyncSceneOperationCollection GetUnloadingOperations(this ExtendedSceneManager sceneManager, params int[] buildIndices)
        {
            AsyncSceneOperationCollection operations = new AsyncSceneOperationCollection(buildIndices.Length);

            foreach (int buildIndex in buildIndices)
                operations.AddOperation(sceneManager.GetUnloadingOperation(buildIndex));

            return operations;
        }
        public static AsyncSceneOperationCollection GetUnloadingOperations(this ExtendedSceneManager sceneManager, params string[] localScenePaths)
        {
            AsyncSceneOperationCollection operations = new AsyncSceneOperationCollection(localScenePaths.Length);

            foreach (string localScenePath in localScenePaths)
                operations.AddOperation(GetUnloadingOperation(sceneManager, localScenePath));

            return operations;
        }
    }
}
