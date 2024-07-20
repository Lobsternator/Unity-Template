#if UNITY_EDITOR
using System.Reflection;
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// A single debug draw request made by <see cref="DebugManager"/>.
    /// </summary>
    public class DebugRequest
    {
        public MethodInfo drawMethod;
        public object[] parameters;
        public Color color;

        public float startTime;
        public float duration;

        public bool HasExpired => Time.time - startTime > duration;
    }
}
#endif
