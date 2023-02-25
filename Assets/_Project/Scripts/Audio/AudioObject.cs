using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Template.Core;

namespace Template.Audio
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(StudioEventEmitter))]
    public class AudioObject : MonoBehaviour
    {
        private StudioEventEmitter _eventEmitter;
        public StudioEventEmitter EventEmitter
        {
            get
            {
                if (!_eventEmitter)
                    _eventEmitter = GetComponent<StudioEventEmitter>();

                return _eventEmitter;
            }
        }

        public event Action<AudioObject> StartedPlaying;
        public event Action<AudioObject> StoppedPlaying;

        private Coroutine _fadeoutCoroutine;

        public void StopFadeout()
        {
            if (_fadeoutCoroutine is not null)
                StopCoroutine(_fadeoutCoroutine);

            _fadeoutCoroutine = null;
        }
        private IEnumerator Fadeout(float duration)
        {
            float instanceVolume;
            EventEmitter.EventInstance.getVolume(out instanceVolume);
            float instanceOriginalVolume = instanceVolume;

            while (!Mathf.Approximately(instanceVolume, 0.0f))
            {
                yield return CoroutineUtility.WaitForFrames(1);

                EventEmitter.EventInstance.getVolume(out instanceVolume);
                EventEmitter.EventInstance.setVolume(instanceVolume - Time.deltaTime / duration * instanceOriginalVolume);
            }

            enabled = false;
        }

        public void Play()
        {
            enabled = true;
        }
        public void Stop()
        {
            enabled = false;
        }
        public void Stop(float fadeout)
        {
            if (enabled && _fadeoutCoroutine is null)
                _fadeoutCoroutine = StartCoroutine(Fadeout(fadeout));
        }

        public RESULT GetParameter(string name, out float value)
        {
            return EventEmitter.EventInstance.getParameterByName(name, out value);
        }
        public RESULT GetParameter(ref AudioParameter parameter)
        {
            return GetParameter(parameter.name, out parameter.value);
        }
        public RESULT SetParameter(string name, float value)
        {
            return EventEmitter.EventInstance.setParameterByName(name, value);
        }
        public RESULT SetParameter(AudioParameter parameter)
        {
            return SetParameter(parameter.name, parameter.value);
        }

        public RESULT GetVolume(out float volume)
        {
            return EventEmitter.EventInstance.getVolume(out volume);
        }
        public RESULT SetVolume(float volume)
        {
            return EventEmitter.EventInstance.setVolume(volume);
        }

        public RESULT GetPitch(out float pitch)
        {
            return EventEmitter.EventInstance.getPitch(out pitch);
        }
        public RESULT SetPitch(float pitch)
        {
            return EventEmitter.EventInstance.setPitch(pitch);
        }

        public RESULT GetPlaybackState(out PLAYBACK_STATE playbackState)
        {
            return EventEmitter.EventInstance.getPlaybackState(out playbackState);
        }

        private void OnEnable()
        {
            EventEmitter.Play();
            StartedPlaying?.Invoke(this);
        }
        private void OnDisable()
        {
            StopFadeout();
            EventEmitter.Stop();
            StoppedPlaying?.Invoke(this);
        }

        private void OnDestroy()
        {
            EventEmitter.EventInstance.release();
        }

        private void Update()
        {
            PLAYBACK_STATE playbackState;
            EventEmitter.EventInstance.getPlaybackState(out playbackState);

            if (playbackState == PLAYBACK_STATE.STOPPED)
                enabled = false;
        }
    }
}
