using UnityEngine;
using UnityEngine.SceneManagement;

namespace Template.UI
{
    /// <summary>
    /// Attach to UI elements to allow use of <see cref="SceneManager"/> functions.
    /// </summary>
    public class SceneForwarding : MonoBehaviour
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

        public void LoadScene(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex);
        }
        public void LoadScene(string scenePath)
        {
            SceneManager.LoadScene(scenePath);
        }

        public void LoadSceneAsync(int buildIndex)
        {
            SceneManager.LoadSceneAsync(buildIndex);
        }
        public void LoadSceneAsync(string scenePath)
        {
            SceneManager.LoadSceneAsync(scenePath);
        }

        public void UnloadSceneAsync(int buildIndex)
        {
            SceneManager.UnloadSceneAsync(buildIndex);
        }
        public void UnloadSceneAsync(string scenePath)
        {
            SceneManager.UnloadSceneAsync(scenePath);
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
