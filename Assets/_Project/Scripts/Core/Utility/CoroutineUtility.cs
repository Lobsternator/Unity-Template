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

        public static IEnumerator WaitForSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }
        public static IEnumerator WaitForSecondsRealtime(float seconds)
        {
            yield return new WaitForSecondsRealtime(seconds);
        }

        public static IEnumerator WaitUntil(params Func<bool>[] predicates)
        {
            foreach (Func<bool> predicate in predicates)
                yield return new WaitUntil(predicate);
        }
        public static IEnumerator WaitWhile(params Func<bool>[] predicates)
        {
            foreach (Func<bool> predicate in predicates)
                yield return new WaitWhile(predicate);
        }
    }
}
