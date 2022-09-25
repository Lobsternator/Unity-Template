using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Events;
using Serializable = System.SerializableAttribute;

namespace Template.Core
{
    public enum HitstopInteraction
    {
        UpdateTimeScale,
        UpdateReturnScale,
        Cancel
    }

    [Serializable]
    public class HitstopSettings
    {
        public float duration;
        [Tooltip("Always goes between 0 and 1 on the time axis, and is later scaled by 'duration'.")]
        public AnimationCurve timeScaleCurve;
    }

    [PersistentRuntimeObject(RuntimeInitializeLoadType.BeforeSceneLoad, -500)]
    public class TimeManager : PersistentRuntimeSingleton<TimeManager>
    {
        public bool IsTimeFrozen { get; private set; }
        public bool IsDoingHitstop => !(_hitstop is null);

        private class Hitstop
        {
            public Hitstop(float duration, AnimationCurve timeScaleCurve, float returnTimeScale)
            {
                this.startTime       = Time.unscaledTime;
                this.duration        = duration;
                this.returnTimeScale = returnTimeScale;
                this.timeScaleCurve  = timeScaleCurve;
            }

            public float startTime;
            public float duration;
            public float returnTimeScale;
            public AnimationCurve timeScaleCurve;
        }

        private Hitstop _hitstop;

        public void SetTimeScale(float timeScale, HitstopInteraction hitstopInteraction)
        {
            float oldTimeScale = Time.timeScale;
            Time.timeScale     = timeScale;

            if (Mathf.Approximately(oldTimeScale, timeScale))
            {
                if (hitstopInteraction == HitstopInteraction.Cancel)
                    _hitstop = null;

                return;
            }

            EventManager.Instance.ApplicationEvents.TimeScaleChanged?.Invoke(timeScale);

            switch (hitstopInteraction)
            {
                case HitstopInteraction.UpdateReturnScale: _hitstop.returnTimeScale = timeScale; break;
                case HitstopInteraction.Cancel:            _hitstop                 = null;      break;
            }

            if (Mathf.Approximately(timeScale, 0.0f) && !Mathf.Approximately(oldTimeScale, 0.0f))
            {
                IsTimeFrozen = true;
                EventManager.Instance.ApplicationEvents.TimeFroze?.Invoke();
            }
            else if (Mathf.Approximately(oldTimeScale, 0.0f) && !Mathf.Approximately(timeScale, 0.0f))
            {
                IsTimeFrozen = false;
                EventManager.Instance.ApplicationEvents.TimeUnfroze?.Invoke(timeScale);
            }
        }

        public void DoHitstop(HitstopSettings hitstopSettings)
        {
            _hitstop        = new Hitstop(hitstopSettings.duration, hitstopSettings.timeScaleCurve, Time.timeScale);
            float timeScale = hitstopSettings.timeScaleCurve.Evaluate(0.0f);

            SetTimeScale(Time.timeScale * timeScale, HitstopInteraction.UpdateTimeScale);
        }
        public void DoHitstop(float duration, float timeScale)
        {
            _hitstop = new Hitstop(duration, AnimationCurve.Constant(0, 1.0f, timeScale), Time.timeScale);

            SetTimeScale(Time.timeScale * timeScale, HitstopInteraction.UpdateTimeScale);
        }

        private void Update()
        {
            if (_hitstop is null)
                return;

            if (Time.unscaledTime - _hitstop.startTime < _hitstop.duration)
            {
                float timeScale = _hitstop.timeScaleCurve.Evaluate((Time.unscaledTime - _hitstop.startTime) / _hitstop.duration);
                SetTimeScale(_hitstop.returnTimeScale * timeScale, HitstopInteraction.UpdateTimeScale);
            }

            else
                SetTimeScale(_hitstop.returnTimeScale, HitstopInteraction.Cancel);
        }
    }
}
