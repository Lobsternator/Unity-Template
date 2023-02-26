using System.Collections;
using System.Collections.Generic;
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

            Application.Quit();
        }
    }
}
