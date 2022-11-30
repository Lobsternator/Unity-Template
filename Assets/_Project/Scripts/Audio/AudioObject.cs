using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Core;

namespace Template.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioObject : MonoBehaviour
    {
        private AudioSource _audioSource;
        public AudioSource AudioSource
        {
            get
            {
                if (!_audioSource)
                    _audioSource = GetComponent<AudioSource>();

                return _audioSource;
            }
        }

        public event Action<AudioObject> StartedPlaying;
        public event Action<AudioObject> StoppedPlaying;

        private Coroutine _fadeoutCoroutine;
        private float _fadeoutStartVolume;

        private void StopFadeout()
        {
            _audioSource.volume = _fadeoutStartVolume;

            if (_fadeoutCoroutine is not null)
                StopCoroutine(_fadeoutCoroutine);

            _fadeoutCoroutine = null;
        }
        private IEnumerator Fadeout(float duration)
        {
            while (!Mathf.Approximately(_audioSource.volume, 0.0f))
            {
                yield return CoroutineUtility.WaitForFrames(1);

                _audioSource.volume += Time.deltaTime / duration * _fadeoutStartVolume;
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
            StopFadeout();
            enabled = false;
        }
        public void Stop(float fadeout)
        {
            if (_fadeoutCoroutine is null)
                _fadeoutCoroutine = StartCoroutine(Fadeout(fadeout));
        }

        private void OnEnable()
        {
            _audioSource.Play();
            StartedPlaying?.Invoke(this);
        }
        private void OnDisable()
        {
            _audioSource.Stop();
            StoppedPlaying?.Invoke(this);
        }

        private void Awake()
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!_audioSource.isPlaying)
                enabled = false;
        }
    }
}
