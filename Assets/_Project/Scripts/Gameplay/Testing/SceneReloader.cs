using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Template.Scenes;

namespace Template.Gameplay
{
    public class SceneReloader : MonoBehaviour
    {
        [SerializeField] private float reloadInterval = 1.0f;

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
