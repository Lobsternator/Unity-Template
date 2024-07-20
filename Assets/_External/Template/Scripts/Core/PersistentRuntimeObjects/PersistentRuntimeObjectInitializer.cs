using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// For providing additional information about <see cref="IPersistentRuntimeObject"/>s.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class PersistentRuntimeObjectAttribute : Attribute
    {
        public RuntimeInitializeLoadType RuntimeInitializeLoadType { get; }
        public int LoadOrder { get; }
        public string Name { get; }

        public PersistentRuntimeObjectAttribute()
        {
            RuntimeInitializeLoadType = RuntimeInitializeLoadType.AfterSceneLoad;
            LoadOrder                 = 0;
            Name                      = null;
        }
        public PersistentRuntimeObjectAttribute(RuntimeInitializeLoadType runtimeInitializeLoadType)
        {
            RuntimeInitializeLoadType = runtimeInitializeLoadType;
            LoadOrder                 = 0;
            Name                      = null;
        }
        public PersistentRuntimeObjectAttribute(int loadOrder)
        {
            RuntimeInitializeLoadType = RuntimeInitializeLoadType.AfterSceneLoad;
            LoadOrder                 = loadOrder;
            Name                      = null;
        }
        public PersistentRuntimeObjectAttribute(string name)
        {
            RuntimeInitializeLoadType = RuntimeInitializeLoadType.AfterSceneLoad;
            LoadOrder                 = 0;
            Name                      = name;
        }
        public PersistentRuntimeObjectAttribute(RuntimeInitializeLoadType runtimeInitializeLoadType, int loadOrder)
        {
            RuntimeInitializeLoadType = runtimeInitializeLoadType;
            LoadOrder                 = loadOrder;
            Name                      = null;
        }
        public PersistentRuntimeObjectAttribute(RuntimeInitializeLoadType runtimeInitializeLoadType, string name)
        {
            RuntimeInitializeLoadType = runtimeInitializeLoadType;
            LoadOrder                 = 0;
            Name                      = name;
        }
        public PersistentRuntimeObjectAttribute(RuntimeInitializeLoadType runtimeInitializeLoadType, int loadOrder, string name)
        {
            RuntimeInitializeLoadType = runtimeInitializeLoadType;
            LoadOrder                 = loadOrder;
            Name                      = name;
        }
    }

    /// <summary>
    /// Static class responsible for creating any <see cref="IPersistentRuntimeObject"/>s in the application domain.
    /// </summary>
    public static class PersistentRuntimeObjectInitializer
    {
        private static readonly List<Type> _persistentRuntimeObjectTypes;
        private static readonly BindingFlags _createObjectInstanceBindingFlags = BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy;

        static PersistentRuntimeObjectInitializer()
        {
            _persistentRuntimeObjectTypes = new List<Type>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) 
            {
                foreach (Type type in assembly.GetTypes())
                {
                    bool isClass      = type.IsClass;
                    bool isAbstract   = type.IsAbstract;
                    bool hasInterface = type.HasInterface<IPersistentRuntimeObject>();

                    if (isClass && !isAbstract && hasInterface)
                        _persistentRuntimeObjectTypes.Add(type);
                }
            }
        }

        private static Type[] GetInheritedTypes(RuntimeInitializeLoadType runtimeInitializeLoadType)
        {
            return _persistentRuntimeObjectTypes.Where(t =>
            {
                PersistentRuntimeObjectAttribute attribute = t.GetCustomAttribute<PersistentRuntimeObjectAttribute>();
                RuntimeInitializeLoadType loadType         = attribute is not null ? attribute.RuntimeInitializeLoadType : RuntimeInitializeLoadType.AfterSceneLoad;

                return loadType == runtimeInitializeLoadType;
            }).OrderBy(t =>
            {
                PersistentRuntimeObjectAttribute attribute = t.GetCustomAttribute<PersistentRuntimeObjectAttribute>();
                return attribute is not null ? attribute.LoadOrder : 0;
            }).ToArray();
        }

        private static void CreateObjectInstances_Internal(RuntimeInitializeLoadType loadType)
        {
            Type[] inheritedTypes = GetInheritedTypes(loadType);
            foreach (Type type in inheritedTypes)
            {
                PersistentRuntimeObjectAttribute attribute = type.GetCustomAttribute<PersistentRuntimeObjectAttribute>();
                string defaultObjectName                   = attribute?.Name is not null ? attribute.Name : type.Name;

                object[] parameters = new object[] { defaultObjectName };
                type.GetMethod("CreateObjectInstance", _createObjectInstanceBindingFlags).Invoke(null, parameters);
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void CreateObjectInstances_SubsystemRegistration()
        {
            CreateObjectInstances_Internal(RuntimeInitializeLoadType.SubsystemRegistration);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void CreateObjectInstances_AfterAssembliesLoaded()
        {
            CreateObjectInstances_Internal(RuntimeInitializeLoadType.AfterAssembliesLoaded);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void CreateObjectInstances_BeforeSplashScreen()
        {
            CreateObjectInstances_Internal(RuntimeInitializeLoadType.BeforeSplashScreen);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CreateObjectInstances_BeforeSceneLoad()
        {
            CreateObjectInstances_Internal(RuntimeInitializeLoadType.BeforeSceneLoad);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CreateObjectInstances_AfterSceneLoad()
        {
            CreateObjectInstances_Internal(RuntimeInitializeLoadType.AfterSceneLoad);
        }
    }
}
