using Template.Saving;
using UnityEngine;

namespace Template.UI
{
    /// <summary>
    /// Attach to UI elements to allow use of <see cref="SaveManager"/> functions.
    /// </summary>
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
