using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Events;

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

        private void Update()
        {
            if (!AudioSource.isPlaying)
                EventManager.Instance.ApplicationEvents.AudioObjectFinishedPlaying?.Invoke(this);
        }
    }
}
