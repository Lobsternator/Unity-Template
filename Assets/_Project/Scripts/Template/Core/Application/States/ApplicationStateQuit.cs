using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Serializable = System.SerializableAttribute;

namespace Template.Core
{
    [Serializable]
    public class ApplicationStateQuit : ApplicationStateBase
    {
        [Tooltip("If the application stalls when shutting down, the state will check for responsiveness this many times before forcefully shutting down the application.")]
        public int failsafeKillRetryCount    = 10;
        [Tooltip("The state will wait this long (in milliseconds) between each check.")]
        public int failsafeKillRetryInterval = 100;

        public override IEnumerator OnEnable()
        {
            yield return CoroutineUtility.WaitForFrames(1);

#if !UNITY_EDITOR
            Task.Run(FailsafeKill);
            Application.Quit();
#endif
        }

#if !UNITY_EDITOR
        private async Task FailsafeKill()
        {
            var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            int retries        = 0;

            while (retries < failsafeKillRetryCount)
            {
                await Task.Delay(failsafeKillRetryInterval);

                if (!currentProcess.Responding)
                    retries++;
                else
                    retries = 0;
            }

            currentProcess.Kill();
        }
#endif
    }
}
