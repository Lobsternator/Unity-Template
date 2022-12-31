using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    public static class CoroutineUtility
    {
        private static WaitForEndOfFrame _waitForEndOfFrame    = new WaitForEndOfFrame();
        private static WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

        public static IEnumerator WaitForFrames(int frames)
        {
            for (int i = 0; i < frames; i++)
                yield return _waitForEndOfFrame;
        }
        public static IEnumerator WaitForFixedFrames(int frames)
        {
            for (int i = 0; i < frames; i++)
                yield return _waitForFixedUpdate;
        }
    }
}
