using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Stores information about an ongoing hitstop.
    /// </summary>
    public class Hitstop
    {
        public Hitstop(float durationMultiplier, AnimationCurve timeScaleCurve, float originalTimeScale)
        {
            StartTime          = Time.unscaledTime;
            DurationMultiplier = durationMultiplier;
            _originalTimeScale = originalTimeScale;
            TimeScaleCurve     = timeScaleCurve;
        }

        public float ModifiedTimeScale => EvaluateModifiedTimeScale(Mathf.Approximately(DurationMultiplier, 0.0f) ? Mathf.Infinity : (TimeScaleCurve.GetStartTime() + (Time.unscaledTime - StartTime) / DurationMultiplier));

        public float Duration => TimeScaleCurve.GetDuration() * DurationMultiplier;
        public float StartTime { get; }
        public float EndTime => StartTime + Duration;

        public float DurationMultiplier { get; }

        private float _originalTimeScale;
        public float OriginalTimeScale
        {
            get => _originalTimeScale;
            set
            {
                _originalTimeScale = value;
                TimeManager.SetTimeScale(ModifiedTimeScale, HitstopInteraction.Ignore);
            }
        }

        public AnimationCurve TimeScaleCurve { get; }

        public float EvaluateModifiedTimeScale(float time)
        {
            return _originalTimeScale * TimeScaleCurve.Evaluate(Mathf.Clamp(time, TimeScaleCurve.GetStartTime(), TimeScaleCurve.GetEndTime()));
        }
    }

    public enum HitstopInteraction
    {
        Ignore,
        Multiply,
        Cancel
    }

    /// <summary>
    /// Singleton that manages time in various ways.
    /// <b>Automatically created at the start of the program.</b>
    /// </summary>
    [PersistentRuntimeObject(RuntimeInitializeLoadType.BeforeSceneLoad, -500)]
    public class TimeManager : PersistentRuntimeSingleton<TimeManager>
    {
        public static bool IsDoingHitstop => CurrentHitstop is not null;
        public static Hitstop CurrentHitstop { get; private set; }

        public static event Action TimeScaleChanged;
        public static event Action TimeFroze;
        public static event Action TimeUnfroze;
        public static event Action<Hitstop> HitstopStarted;
        public static event Action HitstopEnded;

        private static bool _isTimeFrozen;

        private static void OnTimeFroze()
        {
            _isTimeFrozen = true;
            TimeFroze?.Invoke();
        }
        private static void OnTimeUnfroze()
        {
            _isTimeFrozen = false;
            TimeUnfroze?.Invoke();
        }

        public static void SetTimeScale(float timeScale) => SetTimeScale(timeScale, HitstopInteraction.Multiply);
        public static void SetTimeScale(float timeScale, HitstopInteraction hitstopInteraction)
        {
            float oldTimeScale = Time.timeScale;
            float newTimeScale;

            if (hitstopInteraction == HitstopInteraction.Ignore || CurrentHitstop is null)
                newTimeScale = timeScale;

            else if (hitstopInteraction == HitstopInteraction.Multiply)
            {
                CurrentHitstop.OriginalTimeScale = timeScale;
                newTimeScale                     = CurrentHitstop.ModifiedTimeScale;
            }
            else if (hitstopInteraction == HitstopInteraction.Cancel)
            {
                if (Instance)
                    Instance.StopAllCoroutines();

                CurrentHitstop = null;
                newTimeScale   = timeScale;
                HitstopEnded?.Invoke();
            }
            else
                throw new NotImplementedException();

            if (Mathf.Approximately(oldTimeScale, newTimeScale))
                return;

            Time.timeScale = Mathf.Max(newTimeScale, 0.0f);
            TimeScaleChanged?.Invoke();

            if (Mathf.Approximately(newTimeScale, 0.0f) && !Mathf.Approximately(oldTimeScale, 0.0f))
                OnTimeFroze();

            else if (Mathf.Approximately(oldTimeScale, 0.0f) && !Mathf.Approximately(newTimeScale, 0.0f))
                OnTimeUnfroze();
        }

        private static Hitstop DoHitstop_Internal(float durationMultiplier, AnimationCurve timeScaleCurve)
        {
            float originalTimeScale;
            if (CurrentHitstop is not null)
            {
                originalTimeScale = CurrentHitstop.OriginalTimeScale;
                CancelHitstop();
            }
            else
                originalTimeScale = Time.timeScale;

            CurrentHitstop = new Hitstop(Mathf.Max(durationMultiplier, 0.0f), timeScaleCurve, originalTimeScale);

            SetTimeScale(CurrentHitstop.ModifiedTimeScale, HitstopInteraction.Ignore);
            Instance.StartCoroutine(UpdateHitstop());

            HitstopStarted?.Invoke(CurrentHitstop);
            return CurrentHitstop;
        }

        private static bool CanDoHitstop()
        {
#if UNITY_EDITOR
            if (!Instance)
            {
                Debug.LogWarning($"{nameof(TimeManager)} instance needs to exist in order to do a hitstop!");
                return false;
            }
#endif

            return true;
        }

        public static Hitstop DoHitstop(HitstopSettings hitstopSettings)
        {
            if (!CanDoHitstop())
                return null;

            return DoHitstop_Internal(hitstopSettings.durationMultiplier, hitstopSettings.timeScaleCurve);
        }
        public static Hitstop DoHitstop(float duration, float timeScale)
        {
            if (!CanDoHitstop())
                return null;

            return DoHitstop_Internal(duration, AnimationCurve.Constant(0, 1.0f, timeScale));
        }

        public static void CancelHitstop()
        {
            if (CurrentHitstop is null)
                return;

            SetTimeScale(CurrentHitstop.OriginalTimeScale, HitstopInteraction.Cancel);
        }

        private static IEnumerator UpdateHitstop()
        {
            float duration = CurrentHitstop.Duration;

            while (Time.unscaledTime - CurrentHitstop.StartTime < duration)
            {
                yield return CoroutineUtility.WaitForFrames(1);

                SetTimeScale(CurrentHitstop.ModifiedTimeScale, HitstopInteraction.Ignore);
            }

            SetTimeScale(CurrentHitstop.OriginalTimeScale, HitstopInteraction.Cancel);
        }

        protected override void Awake()
        {
            base.Awake();
            if (IsDuplicate)
                return;

            _isTimeFrozen = false;
        }
        private void Start()
        {
            if (Mathf.Approximately(Time.timeScale, 0.0f))
                OnTimeFroze();
        }

        private void LateUpdate()
        {
            if (Mathf.Approximately(Time.timeScale, 0.0f) && !_isTimeFrozen)
                OnTimeFroze();

            else if (!Mathf.Approximately(Time.timeScale, 0.0f) && _isTimeFrozen)
                OnTimeUnfroze();
        }

#if UNITY_EDITOR
        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();

            TimeScaleChanged = null;
            TimeFroze        = null;
            TimeUnfroze      = null;
            HitstopStarted   = null;
            HitstopEnded     = null;
        }
#endif
    }
}
