using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serializable = System.SerializableAttribute;

namespace Template.Core
{
    [Serializable]
    public class ApplicationStateTest : State<ApplicationStateMachine>
    {
        public Vector3 test;

        public override IEnumerator OnEnable()
        {
            Debug.Log("Application state test message.");

            yield break;
        }
    }
}
