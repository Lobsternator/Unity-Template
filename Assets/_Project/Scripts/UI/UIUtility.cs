using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Template.UI
{
    public class UIUtility : MonoBehaviour
    {
        public void SetSelectedObject(GameObject obj)
        {
            EventSystem eventSystem = EventSystem.current;
            if (!eventSystem)
                return;

            eventSystem.SetSelectedGameObject(obj);
        }
    }
}
