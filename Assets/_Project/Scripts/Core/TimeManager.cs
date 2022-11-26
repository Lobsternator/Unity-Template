using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    public enum HitstopInteraction
    {
        Ignore,
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
        public bool IsDoingHitstop => _hitstop is not null;

        public event Action<float> TimeScaleChanged;
        public event Action TimeFroze;
        public event Action<float> TimeUnfroze;

        private class Hitstop
        {
            public Hitstop(float duration, AnimationCurve timeScaleCurve, float returnTimeScale)
            {
                this.startTime       = Time.unscaledTime;
                this.duration        = duration;
                this.returnTimeScale = returnTimeScale;
                this.timeScaleCurve  = timeScaleCurve;
            }

            public float CurrentModifiedTimeScale => EvaluateModifiedTimeScale((Time.unscaledTime - startTime) / duration);

            public float startTime;
            public float duration;
            public float returnTimeScale;
            public AnimationCurve timeScaleCurve;

            public float EvaluateModifiedTimeScale(float normalizedTime)
            {
                return returnTimeScale * timeScaleCurve.Evaluate(normalizedTime);
            }
        }

        private Hitstop _hitstop;

        public void SetTimeScale(float timeScale, HitstopInteraction hitstopInteraction)
        {
            float oldTimeScale = Time.timeScale;

            switch (hitstopInteraction)
            {
                case HitstopInteraction.Ignore:
                    Time.timeScale = timeScale;
                    break;

                case HitstopInteraction.UpdateReturnScale:

                    if (_hitstop is not null)
                    {
                        _hitstop.returnTimeScale = timeScale;
                        Time.timeScale           = _hitstop.CurrentModifiedTimeScale;
                    }
                    else
                        Time.timeScale           = timeScale;

                    break;

                case HitstopInteraction.Cancel:
                    _hitstop       = null;
                    Time.timeScale = timeScale;
                    break;

                default:
                    break;
            }

            if (Mathf.Approximately(oldTimeScale, timeScale))
                return;

            TimeScaleChanged?.Invoke(timeScale);

            if (Mathf.Approximately(timeScale, 0.0f) && !Mathf.Approximately(oldTimeScale, 0.0f))
            {
                IsTimeFrozen = true;
                TimeFroze?.Invoke();
            }
            else if (Mathf.Approximately(oldTimeScale, 0.0f) && !Mathf.Approximately(timeScale, 0.0f))
            {
                IsTimeFrozen = false;
                TimeUnfroze?.Invoke(timeScale);
            }
        }

        public void DoHitstop_Internal(float duration, AnimationCurve timeScaleCurve)
        {
            float returnTimeScale = _hitstop is not null ? _hitstop.returnTimeScale : Time.timeScale;
            _hitstop              = new Hitstop(duration, timeScaleCurve, returnTimeScale);

            SetTimeScale(_hitstop.EvaluateModifiedTimeScale(0.0f), HitstopInteraction.Ignore);
        }

        public void DoHitstop(HitstopSettings hitstopSettings)
        {
            DoHitstop_Internal(hitstopSettings.duration, hitstopSettings.timeScaleCurve);
        }
        public void DoHitstop(float duration, float timeScale)
        {
            DoHitstop_Internal(duration, AnimationCurve.Constant(0, 1.0f, timeScale));
        }

        private void Update()
        {
            if (_hitstop is null)
                return;

            if (Time.unscaledTime - _hitstop.startTime < _hitstop.duration)
                SetTimeScale(_hitstop.CurrentModifiedTimeScale, HitstopInteraction.Ignore);

            else
                SetTimeScale(_hitstop.returnTimeScale, HitstopInteraction.Cancel);
        }
    }
}
