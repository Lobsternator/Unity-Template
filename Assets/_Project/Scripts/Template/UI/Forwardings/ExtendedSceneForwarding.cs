using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Template.Scenes;

namespace Template.UI
{
    /// <summary>
    /// Attach to UI elements to allow use of <see cref="ExtendedSceneManager"/> functions.
    /// </summary>
    public class ExtendedSceneForwarding : MonoBehaviour
    {
        public LoadSceneMode loadSceneMode;
        public UnloadSceneOptions unloadSceneOptions;

        public void SetLoadSceneMode(int loadSceneMode)
        {
            this.loadSceneMode = (LoadSceneMode)loadSceneMode;
        }
        public void SetUnloadSceneOptions(int unloadSceneOptions)
        {
            this.unloadSceneOptions = (UnloadSceneOptions)unloadSceneOptions;
        }

        public void LoadSceneAsync(int buildIndex)
        {
            ExtendedSceneManager.LoadSceneAsync(buildIndex, loadSceneMode);
        }
        public void LoadSceneAsync(string scenePath)
        {
            ExtendedSceneManager.LoadSceneAsync(scenePath, loadSceneMode);
        }

        public void UnloadSceneAsync(int buildIndex)
        {
            ExtendedSceneManager.UnloadSceneAsync(buildIndex, unloadSceneOptions);
        }
        public void UnloadSceneAsync(string scenePath)
        {
            ExtendedSceneManager.UnloadSceneAsync(scenePath, unloadSceneOptions);
        }

        public void ResetLoadSceneMode()
        {
            loadSceneMode = LoadSceneMode.Additive;
        }
        public void ResetUnloadSceneOptions()
        {
            unloadSceneOptions = UnloadSceneOptions.None;
        }

        public void Reset()
        {
            ResetLoadSceneMode();
            ResetUnloadSceneOptions();
        }
    }
}
