using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    public enum HitstopInteraction
    {
        Ignore,
        Multiply,
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

        public static event Action<float> TimeScaleChanged;
        public static event Action TimeFroze;
        public static event Action<float> TimeUnfroze;
        public static event Action HitstopBegan;
        public static event Action HitstopEnded;

        private class Hitstop
        {
            public Hitstop(float duration, AnimationCurve timeScaleCurve, float returnTimeScale)
            {
                this.startTime         = Time.unscaledTime;
                this.duration          = duration;
                this.originalTimeScale = returnTimeScale;
                this.timeScaleCurve    = timeScaleCurve;
            }

            public float CurrentModifiedTimeScale => EvaluateModifiedTimeScale((Time.unscaledTime - startTime) / duration);

            public float startTime;
            public float duration;
            public float originalTimeScale;
            public AnimationCurve timeScaleCurve;

            public float EvaluateModifiedTimeScale(float normalizedTime)
            {
                return originalTimeScale * timeScaleCurve.Evaluate(normalizedTime);
            }
        }

        private Hitstop _hitstop;

        public void SetTimeScale(float timeScale, HitstopInteraction hitstopInteraction)
        {
            float oldTimeScale = Time.timeScale;
            float newTimeScale;

            if (hitstopInteraction == HitstopInteraction.Ignore || _hitstop is null)
                newTimeScale = timeScale;

            else if (hitstopInteraction == HitstopInteraction.Multiply)
            {
                _hitstop.originalTimeScale = timeScale;
                newTimeScale               = _hitstop.CurrentModifiedTimeScale;
            }
            else if (hitstopInteraction == HitstopInteraction.Cancel)
            {
                StopAllCoroutines();
                _hitstop     = null;
                newTimeScale = timeScale;
                HitstopEnded?.Invoke();
            }
            else
                throw new NotImplementedException();

            if (Mathf.Approximately(oldTimeScale, newTimeScale))
                return;

            Time.timeScale = newTimeScale;
            TimeScaleChanged?.Invoke(newTimeScale);

            if (Mathf.Approximately(newTimeScale, 0.0f) && !Mathf.Approximately(oldTimeScale, 0.0f))
            {
                IsTimeFrozen = true;
                TimeFroze?.Invoke();
            }
            else if (Mathf.Approximately(oldTimeScale, 0.0f) && !Mathf.Approximately(newTimeScale, 0.0f))
            {
                IsTimeFrozen = false;
                TimeUnfroze?.Invoke(newTimeScale);
            }
        }

        private IEnumerator UpdateHitstop()
        {
            while (Time.unscaledTime - _hitstop.startTime < _hitstop.duration)
            {
                yield return CoroutineUtility.WaitForFrames(1);

                SetTimeScale(_hitstop.CurrentModifiedTimeScale, HitstopInteraction.Ignore);
            }

            SetTimeScale(_hitstop.originalTimeScale, HitstopInteraction.Cancel);
        }

        private void DoHitstop_Internal(float duration, AnimationCurve timeScaleCurve)
        {
            float returnTimeScale = _hitstop is not null ? _hitstop.originalTimeScale : Time.timeScale;
            _hitstop              = new Hitstop(duration, timeScaleCurve, returnTimeScale);

            SetTimeScale(_hitstop.EvaluateModifiedTimeScale(0.0f), HitstopInteraction.Ignore);

            HitstopBegan?.Invoke();

            StartCoroutine(UpdateHitstop());
        }

        public void DoHitstop(HitstopSettings hitstopSettings)
        {
            CancelHitstop();
            DoHitstop_Internal(hitstopSettings.duration, hitstopSettings.timeScaleCurve);
        }
        public void DoHitstop(float duration, float timeScale)
        {
            CancelHitstop();
            DoHitstop_Internal(duration, AnimationCurve.Constant(0, 1.0f, timeScale));
        }

        public void CancelHitstop()
        {
            if (_hitstop is null)
                return;

            SetTimeScale(_hitstop.originalTimeScale, HitstopInteraction.Cancel);
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();

            TimeScaleChanged = null;
            TimeFroze        = null;
            TimeUnfroze      = null;
            HitstopBegan     = null;
            HitstopEnded     = null;
        }
    }
}
