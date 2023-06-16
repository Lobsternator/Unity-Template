using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Template.UI
{
    /// <summary>
    /// Attach to UI elements to allow for use of various UI utilities.
    /// </summary>
    public class UIUtility : MonoBehaviour
    {
        public void SetSelectedObject(GameObject obj)
        {
            EventSystem eventSystem = EventSystem.current;
            if (!eventSystem)
                return;

            eventSystem.SetSelectedGameObject(obj);
        }
        public void SetSelectedObject(Selectable selectable)
        {
            selectable.Select();
        }
    }
}
