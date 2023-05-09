using System.Collections;
using System.Collections.Generic;
using Template.Saving;
using UnityEngine;

namespace Template.UI
{
    public class SaveForwarding : MonoBehaviour
    {
        public void SaveToSlot(int slot)
        {
            SaveManager.SaveToSlot(slot);
        }
        public void LoadFromSlot(int slot)
        {
            SaveManager.LoadFromSlot(slot);
        }

        public void ClearSlot(int slot)
        {
            SaveManager.ClearSaveSlot(slot);
        }
        public void ClearAllSlots()
        {
            SaveManager.ClearAllSaveSlots();
        }
    }
}
