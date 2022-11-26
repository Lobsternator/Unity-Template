using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Template.Core;

namespace Template.Audio
{
    [PersistentRuntimeObject(RuntimeInitializeLoadType.BeforeSceneLoad, -1000)]
    public class AudioManager : PersistentRuntimeSingleton<AudioManager, AudioManagerData>
    {
        private ObjectPool<AudioObject> _audioObjectPool;

        public event Action<AudioObject> OnAudioStartedPlaying;
        public event Action<AudioObject> OnAudioStoppedPlaying;

        private AudioObject PlaySound_Internal(AudioObject audioObject, AudioClip audioClip, AudioObjectSettings settings)
        {
            AudioSource audioSource = audioObject.AudioSource;
            audioSource.volume      = settings.volume;
            audioSource.clip        = audioClip;

            audioObject.Play();
            OnAudioStartedPlaying?.Invoke(audioObject);

            return audioObject;
        }

        public AudioObject PlaySound(AudioClip audioClip, AudioObjectSettings settings)
        {
            AudioObject audioObject = _audioObjectPool.Get();

            return PlaySound_Internal(audioObject, audioClip, settings);
        }
        public AudioObject PlaySound(AudioClip audioClip)
        {
            return PlaySound(audioClip, PersistentData.defaultAudioSettings);
        }

        private AudioObject OnAudioObjectCreate()
        {
            AudioObject audioObject     = Instantiate(PersistentData.audioObjectPrefab, transform).GetComponent<AudioObject>();
            audioObject.StoppedPlaying += OnAudioObjectStoppedPlaying;

            return audioObject;
        }
        private void OnAudioObjectGet(AudioObject audioObject)
        {
            audioObject.gameObject.SetActive(true);
        }
        private void OnAudioObjectRelease(AudioObject audioObject)
        {
            audioObject.gameObject.SetActive(false);
        }
        private void OnAudioObjectDestroy(AudioObject audioObject)
        {
            audioObject.StoppedPlaying -= OnAudioObjectStoppedPlaying;
            Destroy(audioObject.gameObject);
        }

        private void OnAudioObjectStoppedPlaying(AudioObject audioObject)
        {
            _audioObjectPool.Release(audioObject);
            OnAudioStoppedPlaying?.Invoke(audioObject);
        }

        protected override void Awake()
        {
            base.Awake();
            if (IsDuplicate)
                return;

            _audioObjectPool = new ObjectPool<AudioObject>(
                OnAudioObjectCreate,
                OnAudioObjectGet,
                OnAudioObjectRelease,
                OnAudioObjectDestroy,
                defaultCapacity: PersistentData.maxNumAudioObjects,
                maxSize: PersistentData.maxNumAudioObjects);
        }
    }
}
