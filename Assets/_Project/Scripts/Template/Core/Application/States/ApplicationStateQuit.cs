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

            while (retries < 10)
            {
                await Task.Delay(100);

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
