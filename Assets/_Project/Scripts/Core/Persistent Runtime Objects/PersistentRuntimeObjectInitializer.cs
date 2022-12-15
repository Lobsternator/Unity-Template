using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Template.Core
{
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

    public static class PersistentRuntimeObjectInitializer
    {
        private static readonly Type[] _assemblyTypes                          = Assembly.GetAssembly(typeof(IPersistentRuntimeObject)).GetTypes();
        private static readonly BindingFlags _createObjectInstanceBindingFlags = BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy;

        private static Type[] GetInheritedTypes(RuntimeInitializeLoadType runtimeInitializeLoadType)
        {
            return _assemblyTypes.Where(t =>
            {
                PersistentRuntimeObjectAttribute attribute = t.GetCustomAttribute(typeof(PersistentRuntimeObjectAttribute)) as PersistentRuntimeObjectAttribute;
                RuntimeInitializeLoadType loadType         = attribute is not null ? attribute.RuntimeInitializeLoadType : RuntimeInitializeLoadType.AfterSceneLoad;

                bool isClass      = t.IsClass;
                bool isAbstract   = t.IsAbstract;
                bool hasInterface = t.GetInterfaces().Contains(typeof(IPersistentRuntimeObject));
                bool hasLoadType  = loadType == runtimeInitializeLoadType;

                return isClass && !isAbstract && hasInterface && hasLoadType;
            }).OrderBy(t =>
            {
                PersistentRuntimeObjectAttribute attribute = t.GetCustomAttribute(typeof(PersistentRuntimeObjectAttribute)) as PersistentRuntimeObjectAttribute;
                return attribute is not null ? attribute.LoadOrder : 0;
            }).ToArray();
        }

        private static void CreateObjectInstances_Internal(RuntimeInitializeLoadType loadType)
        {
            Type[] inheritedTypes = GetInheritedTypes(loadType);
            foreach (Type type in inheritedTypes)
            {
                PersistentRuntimeObjectAttribute attribute = type.GetCustomAttribute(typeof(PersistentRuntimeObjectAttribute)) as PersistentRuntimeObjectAttribute;
                string objectName                          = attribute?.Name is not null ? attribute.Name : type.Name;

                object[] parameters = new object[] { objectName };
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
