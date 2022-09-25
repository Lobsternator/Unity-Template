using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    public static class PersistentRuntimeObjectUtility
    {
        public static TData GetPersistentData<TData>() where TData : PersistentRuntimeObjectData
        {
            return Resources.Load($"PersistentRuntimeObjectData/{typeof(TData).Name}") as TData;
        }
    }
}
