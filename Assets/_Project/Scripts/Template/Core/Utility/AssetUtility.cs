using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Type = System.Type;

namespace Template.Core
{
    /// <summary>
    /// Various utilities related to assets.
    /// </summary>
    public static class AssetUtility
    {
        private static Dictionary<Type, Object> _cachedSingletonAssets = new Dictionary<Type, Object>();

        public static TSingleton GetSingletonAsset<TSingleton>() where TSingleton : ScriptableObject
        {
            if (typeof(TSingleton).GetCustomAttribute(typeof(SingletonAssetAttribute), true) is null)
                throw new System.ArgumentException($"{typeof(TSingleton).Name} does not have the necessary attribute: \'{nameof(SingletonAssetAttribute)}\'!");

            if (_cachedSingletonAssets.TryGetValue(typeof(TSingleton), out var singletonAsset))
            {
                if (!singletonAsset)
                {
                    singletonAsset                             = Resources.LoadAll("", typeof(TSingleton)).FirstOrDefault();
                    _cachedSingletonAssets[typeof(TSingleton)] = singletonAsset;
                }

                return singletonAsset as TSingleton;
            }
            else
            {
                singletonAsset = Resources.LoadAll("", typeof(TSingleton)).FirstOrDefault();
                if (!_cachedSingletonAssets.TryAdd(typeof(TSingleton), singletonAsset))
                    _cachedSingletonAssets[typeof(TSingleton)] = singletonAsset;

                //_cachedSingletonAssets.Add(typeof(TSingleton), singletonAsset); // SOMEHOW CAUSES AN ERROR WHEN BUILT?????????????????????

                return singletonAsset as TSingleton;
            }
        }
    }
}
