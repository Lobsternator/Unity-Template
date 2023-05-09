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

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        private AudioObjectAttacher? _attacher;
        public AudioObjectAttacher? Attacher
        {
            get
            {
                if (!_attacher)
                    _attacher = GetComponent<AudioObjectAttacher>();

                return _attacher;
            }
        }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

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
            GetVolume(out var instanceVolume);
            float instanceOriginalVolume = instanceVolume;

            while (!Mathf.Approximately(instanceVolume, 0.0f))
            {
                yield return CoroutineUtility.WaitForFrames(1);

                GetVolume(out instanceVolume);
                SetVolume(instanceVolume - Time.deltaTime / duration * instanceOriginalVolume);
            }

            SetVolume(instanceOriginalVolume);
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

        public float GetParameter(string name)
        {
            EventEmitter.EventInstance.getParameterByName(name, out var value);
            return value;
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

        public float GetVolume()
        {
            EventEmitter.EventInstance.getVolume(out var volume);
            return volume;
        }
        public RESULT GetVolume(out float volume)
        {
            return EventEmitter.EventInstance.getVolume(out volume);
        }
        public RESULT SetVolume(float volume)
        {
            return EventEmitter.EventInstance.setVolume(volume);
        }

        public float GetPitch()
        {
            EventEmitter.EventInstance.getPitch(out var pitch);
            return pitch;
        }
        public RESULT GetPitch(out float pitch)
        {
            return EventEmitter.EventInstance.getPitch(out pitch);
        }
        public RESULT SetPitch(float pitch)
        {
            return EventEmitter.EventInstance.setPitch(pitch);
        }

        public PLAYBACK_STATE GetPlaybackState()
        {
            EventEmitter.EventInstance.getPlaybackState(out var playbackState);
            return playbackState;
        }
        public RESULT GetPlaybackState(out PLAYBACK_STATE playbackState)
        {
            return EventEmitter.EventInstance.getPlaybackState(out playbackState);
        }

        private void Awake()
        {
            _eventEmitter = GetComponent<StudioEventEmitter>();
            _attacher     = GetComponent<AudioObjectAttacher>();
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
            if (GetPlaybackState() == PLAYBACK_STATE.STOPPED)
                enabled = false;
        }
    }
}
