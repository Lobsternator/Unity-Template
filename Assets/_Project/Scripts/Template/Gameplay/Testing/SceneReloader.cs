using System.Collections;
using Template.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Template.Gameplay
{
    /// <summary>
    /// Test script that simply reloads the scene according to <see cref="reloadInterval"/>.
    /// </summary>
    public class SceneReloader : MonoBehaviour
    {
        [SerializeField]
        private float reloadInterval = 1.0f;

        private IEnumerator ReloadScene()
        {
            while (true)
            {
                yield return new WaitForSeconds(reloadInterval);

                Scene activeScene = ExtendedSceneManager.GetActiveScene();
                if (!activeScene.isLoaded)
                    continue;

                ExtendedSceneManager.LoadSceneAsync(activeScene.buildIndex, LoadSceneMode.Single);
            }
        }

        private void OnEnable()
        {
            StartCoroutine(ReloadScene());
        }
        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}
