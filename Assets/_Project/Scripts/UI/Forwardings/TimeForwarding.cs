using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Audio;
using Template.Core;

namespace Template.UI
{
    public class TimeForwarding : MonoBehaviour
    {
        public HitstopInteraction hitstopInteraction = HitstopInteraction.Multiply;

        public void SetHitstopInteraction(int hitstopInteraction)
        {
            this.hitstopInteraction = (HitstopInteraction)hitstopInteraction;
        }

        public void SetTimeScale(float timeScale)
        {
            TimeManager.SetTimeScale(timeScale, hitstopInteraction);
        }

        public void DoHitstop(HitstopSettingsReference hitstopSettings)
        {
            TimeManager.DoHitstop(hitstopSettings.Value);
        }

        public void CancelHitstop()
        {
            TimeManager.CancelHitstop();
        }

        public void ResetHitstopInteraction()
        {
            hitstopInteraction = HitstopInteraction.Multiply;
        }

        public void Reset()
        {
            ResetHitstopInteraction();
        }
    }
}
