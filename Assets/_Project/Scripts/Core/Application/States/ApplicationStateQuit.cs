using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serializable = System.SerializableAttribute;

namespace Template.Core
{
    [Serializable]
    public class ApplicationStateQuit : State<ApplicationStateMachine>
    {
        public override IEnumerator OnEnable()
        {
            yield return CoroutineUtility.WaitForFrames(1);

            Application.Quit();
        }
    }
}
