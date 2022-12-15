using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Type = System.Type;

namespace Template.Core
{
    public static class PersistentRuntimeObjectUtility
    {
        private static Dictionary<Type, Object> _cachedDatas = new Dictionary<Type, Object>();

        public static TData GetPersistentData<TData>() where TData : PersistentRuntimeObjectData
        {
            if (_cachedDatas.TryGetValue(typeof(TData), out var persistentData))
            {
                if (!persistentData)
                {
                    persistentData              = Resources.LoadAll("", typeof(TData)).FirstOrDefault();
                    _cachedDatas[typeof(TData)] = persistentData;
                }

                return persistentData as TData;
            }
            else
            {
                persistentData = Resources.LoadAll("", typeof(TData)).FirstOrDefault();
                _cachedDatas.Add(typeof(TData), persistentData);

                return persistentData as TData;
            }
        }
    }
}
