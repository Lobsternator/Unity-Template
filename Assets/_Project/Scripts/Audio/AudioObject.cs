using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public void Play()
        {
            enabled = true;
        }
        public void Stop()
        {
            enabled = false;
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
