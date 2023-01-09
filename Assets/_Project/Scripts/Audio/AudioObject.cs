using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using Template.Core;

namespace Template.Audio
{
    [RequireComponent(typeof(StudioEventEmitter))]
    public class AudioObject : MonoBehaviour
    {
        public StudioEventEmitter EventEmitter { get; private set; }

        public event Action<AudioObject> StartedPlaying;
        public event Action<AudioObject> StoppedPlaying;

        private Coroutine _fadeoutCoroutine;
        private float _fadeoutStartVolume;

        private void StopFadeout()
        {
            EventEmitter.EventInstance.setVolume(_fadeoutStartVolume);

            if (_fadeoutCoroutine is not null)
                StopCoroutine(_fadeoutCoroutine);

            _fadeoutCoroutine = null;
        }
        private IEnumerator Fadeout(float duration)
        {
            float instanceVolume;
            EventEmitter.EventInstance.getVolume(out instanceVolume);

            while (!Mathf.Approximately(instanceVolume, 0.0f))
            {
                yield return CoroutineUtility.WaitForFrames(1);

                EventEmitter.EventInstance.getVolume(out instanceVolume);
                EventEmitter.EventInstance.setVolume(instanceVolume - Time.deltaTime / duration * _fadeoutStartVolume);
            }

            enabled = false;
            StopFadeout();
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
            if (_fadeoutCoroutine is null)
                _fadeoutCoroutine = StartCoroutine(Fadeout(fadeout));
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

        private void Awake()
        {
            EventEmitter = GetComponent<StudioEventEmitter>();
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
