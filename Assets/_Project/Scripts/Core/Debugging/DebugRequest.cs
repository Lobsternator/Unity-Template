#if UNITY_EDITOR
using System.Reflection;
using UnityEngine;

namespace Template.Core
{
    public class DebugRequest
    {
        public MethodInfo drawMethod;
        public object[] parameters;
        public Color color;

        public float startTime;
        public float duration;

        public bool HasExpired         => Time.time - startTime > duration;
        public bool HasExpiredInEditor => Time.realtimeSinceStartup - startTime > duration;
    }
}
#endif
