using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Template.Core;

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

                Scene activeScene = SceneManager.GetActiveScene();
                if (!activeScene.isLoaded)
                    continue;

                SceneManager.LoadScene(activeScene.buildIndex);
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
