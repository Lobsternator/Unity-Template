using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Template.Core;

namespace Template.Events
{
    [PersistentRuntimeObject(RuntimeInitializeLoadType.BeforeSceneLoad, -1000000)]
    public class EventManager : PersistentRuntimeSingleton<EventManager>
    {
        public static bool IsApplicationQuitting { get; private set; }

        public ApplicationEvents ApplicationEvents { get; private set; }
        public GameplayEvents GameplayEvents { get; private set; }

        private void CreateEventContainers()
        {
            foreach (PropertyInfo property in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (property.PropertyType.IsSubclassOf(typeof(EventContainer)))
                    property.SetValue(this, ScriptableObject.CreateInstance(property.PropertyType));
            }
        }

        protected override void Awake()
        {
            base.Awake();
            if (IsDuplicate)
                return;

            IsApplicationQuitting = false;
            Application.wantsToQuit += () =>
            {
                IsApplicationQuitting = true;
                return true;
            };

            CreateEventContainers();
        }
    }
}
