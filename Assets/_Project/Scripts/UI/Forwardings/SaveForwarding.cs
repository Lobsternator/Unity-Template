using System.Collections;
using System.Collections.Generic;
using Template.Saving;
using UnityEngine;

namespace Template.UI
{
    public class SaveForwarding : MonoBehaviour
    {
        public void Save(int slot)
        {
            SaveManager.Instance.SaveToSlot(slot);
        }
        public void Load(int slot)
        {
            SaveManager.Instance.LoadFromSlot(slot);
        }
    }
}
