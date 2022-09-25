using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Template.Core;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Template.Scenes
{
    public static class SceneManagerExtensions
    {
        private static SceneManagerData _sceneManagerData = PersistentRuntimeObjectUtility.GetPersistentData<SceneManagerData>();

        private static string GetFullScenePath(string localScenePath)
        {
            return Path.Combine(_sceneManagerData.pathToSceneFolder, localScenePath + ".unity").Replace('\\', '/');
        }

        public static bool IsSceneLoaded(this SceneManager sceneManager, string localScenePath)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return sceneManager.IsSceneLoaded(buildIndex);
        }
        public static bool IsSceneLoading(this SceneManager sceneManager, string localScenePath)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return sceneManager.IsSceneLoading(buildIndex);
        }

        public static bool AreScenesLoaded(this SceneManager sceneManager, params int[] buildIndices)
        {
            bool result = true;

            foreach (int buildIndex in buildIndices)
                result = result && sceneManager.IsSceneLoaded(buildIndex);

            return result;
        }
        public static bool AreScenesLoading(this SceneManager sceneManager, params int[] buildIndices)
        {
            bool result = true;

            foreach (int buildIndex in buildIndices)
                result = result && sceneManager.IsSceneLoading(buildIndex);

            return result;
        }

        public static bool AreScenesLoaded(this SceneManager sceneManager, params string[] localScenePaths)
        {
            bool result = true;

            foreach (string localScenePath in localScenePaths)
                result = result && IsSceneLoaded(sceneManager, localScenePath);

            return result;
        }
        public static bool AreScenesLoading(this SceneManager sceneManager, params string[] localScenePaths)
        {
            bool result = true;

            foreach (string localScenePath in localScenePaths)
                result = result && IsSceneLoading(sceneManager, localScenePath);

            return result;
        }

        public static bool IsSceneUnloaded(this SceneManager sceneManager, string localScenePath)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return sceneManager.IsSceneUnloaded(buildIndex);
        }
        public static bool IsSceneUnloading(this SceneManager sceneManager, string localScenePath)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return sceneManager.IsSceneUnloading(buildIndex);
        }

        public static bool AreScenesUnoaded(this SceneManager sceneManager, params int[] buildIndices)
        {
            bool result = true;

            foreach (int buildIndex in buildIndices)
                result = result && sceneManager.IsSceneUnloaded(buildIndex);

            return result;
        }
        public static bool AreScenesUnloading(this SceneManager sceneManager, params int[] buildIndices)
        {
            bool result = true;

            foreach (int buildIndex in buildIndices)
                result = result && sceneManager.IsSceneUnloading(buildIndex);

            return result;
        }

        public static bool AreScenesUnoaded(this SceneManager sceneManager, params string[] localScenePaths)
        {
            bool result = true;

            foreach (string localScenePath in localScenePaths)
                result = result && IsSceneUnloaded(sceneManager, localScenePath);

            return result;
        }
        public static bool AreScenesUnloading(this SceneManager sceneManager, params string[] localScenePaths)
        {
            bool result = true;

            foreach (string localScenePath in localScenePaths)
                result = result && IsSceneUnloading(sceneManager, localScenePath);

            return result;
        }

        public static AsyncOperation LoadSceneAsync(this SceneManager sceneManager, string localScenePath) => LoadSceneAsync(sceneManager, localScenePath, LoadSceneMode.Single);
        public static AsyncOperation LoadSceneAsync(this SceneManager sceneManager, string localScenePath, LoadSceneMode loadMode)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return sceneManager.LoadSceneAsync(buildIndex, loadMode);
        }

        public static AsyncOperationCollection LoadScenesAsync(this SceneManager sceneManager, params int[] buildIndices) => LoadScenesAsync(sceneManager, LoadSceneMode.Single, buildIndices);
        public static AsyncOperationCollection LoadScenesAsync(this SceneManager sceneManager, LoadSceneMode loadMode, params int[] buildIndices)
        {
            AsyncOperationCollection operations = new AsyncOperationCollection(buildIndices.Length);

            foreach (int buildIndex in buildIndices)
                operations.AddOperation(sceneManager.LoadSceneAsync(buildIndex, loadMode));

            return operations;
        }

        public static AsyncOperationCollection LoadScenesAsync(this SceneManager sceneManager, params string[] localScenePaths) => LoadScenesAsync(sceneManager, LoadSceneMode.Single, localScenePaths);
        public static AsyncOperationCollection LoadScenesAsync(this SceneManager sceneManager, LoadSceneMode loadMode, params string[] localScenePaths)
        {
            AsyncOperationCollection operations = new AsyncOperationCollection(localScenePaths.Length);

            foreach (string localScenePath in localScenePaths)
                operations.AddOperation(LoadSceneAsync(sceneManager, localScenePath, loadMode));

            return operations;
        }

        public static AsyncOperation UnloadSceneAsync(this SceneManager sceneManager, string localScenePath) => UnloadSceneAsync(sceneManager, localScenePath, UnloadSceneOptions.None);
        public static AsyncOperation UnloadSceneAsync(this SceneManager sceneManager, string localScenePath, UnloadSceneOptions unloadOptions)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return sceneManager.UnloadSceneAsync(buildIndex, unloadOptions);
        }

        public static AsyncOperationCollection UnloadScenesAsync(this SceneManager sceneManager, params int[] buildIndices) => UnloadScenesAsync(sceneManager, UnloadSceneOptions.None, buildIndices);
        public static AsyncOperationCollection UnloadScenesAsync(this SceneManager sceneManager, UnloadSceneOptions unloadOptions, params int[] buildIndices)
        {
            AsyncOperationCollection operations = new AsyncOperationCollection(buildIndices.Length);

            foreach (int buildIndex in buildIndices)
                operations.AddOperation(sceneManager.UnloadSceneAsync(buildIndex, unloadOptions));

            return operations;
        }

        public static AsyncOperationCollection UnloadScenesAsync(this SceneManager sceneManager, params string[] localScenePaths) => UnloadScenesAsync(sceneManager, UnloadSceneOptions.None, localScenePaths);
        public static AsyncOperationCollection UnloadScenesAsync(this SceneManager sceneManager, UnloadSceneOptions unloadOptions, params string[] localScenePaths)
        {
            AsyncOperationCollection operations = new AsyncOperationCollection(localScenePaths.Length);

            foreach (string localScenePath in localScenePaths)
                operations.AddOperation(UnloadSceneAsync(sceneManager, localScenePath, unloadOptions));

            return operations;
        }

        public static AsyncOperation WaitForSceneToLoad(this SceneManager sceneManager, string localScenePath)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return sceneManager.WaitForSceneToLoad(buildIndex);
        }

        public static AsyncOperationCollection WaitForScenesToLoad(this SceneManager sceneManager, params int[] buildIndices)
        {
            AsyncOperationCollection operations = new AsyncOperationCollection(buildIndices.Length);

            foreach (int buildIndex in buildIndices)
                operations.AddOperation(sceneManager.WaitForSceneToLoad(buildIndex));

            return operations;
        }
        public static AsyncOperationCollection WaitForScenesToLoad(this SceneManager sceneManager, params string[] localScenePaths)
        {
            AsyncOperationCollection operations = new AsyncOperationCollection(localScenePaths.Length);

            foreach (string localScenePath in localScenePaths)
                operations.AddOperation(WaitForSceneToLoad(sceneManager, localScenePath));

            return operations;
        }

        public static AsyncOperation WaitForSceneToUnload(this SceneManager sceneManager, string localScenePath)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GetFullScenePath(localScenePath));
            return sceneManager.WaitForSceneToUnload(buildIndex);
        }

        public static AsyncOperationCollection WaitForScenesToUnload(this SceneManager sceneManager, params int[] buildIndices)
        {
            AsyncOperationCollection operations = new AsyncOperationCollection(buildIndices.Length);

            foreach (int buildIndex in buildIndices)
                operations.AddOperation(sceneManager.WaitForSceneToUnload(buildIndex));

            return operations;
        }
        public static AsyncOperationCollection WaitForScenesToUnload(this SceneManager sceneManager, params string[] localScenePaths)
        {
            AsyncOperationCollection operations = new AsyncOperationCollection(localScenePaths.Length);

            foreach (string localScenePath in localScenePaths)
                operations.AddOperation(WaitForSceneToUnload(sceneManager, localScenePath));

            return operations;
        }
    }
}
