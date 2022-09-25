using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Template.Core;
using Template.Events;

namespace Template.Audio
{
    [PersistentRuntimeObject(RuntimeInitializeLoadType.BeforeSceneLoad, -1000)]
    public class AudioManager : PersistentRuntimeSingleton<AudioManager, AudioManagerData>
    {
        private ObjectPool<AudioObject> _audioObjectPool;

        public AudioObject PlaySound(AudioClip audioClip, AudioObjectSettings settings)
        {
            AudioObject audioObject = _audioObjectPool.Get();
            AudioSource audioSource = audioObject.AudioSource;

            audioSource.volume = settings.volume;
            audioSource.clip   = audioClip;

            audioSource.Play();

            return audioObject;
        }
        public AudioObject PlaySound(AudioClip audioClip)
        {
            return PlaySound(audioClip, PersistentData.defaultAudioSettings);
        }

        private AudioObject OnAudioObjectCreate()
        {
            return Instantiate(PersistentData.audioObjectPrefab, transform).GetComponent<AudioObject>();
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
            Destroy(audioObject.gameObject);
        }

        private void OnAudioObjectFinishedPlaying(AudioObject audioObject)
        {
            _audioObjectPool.Release(audioObject);
        }

        private void OnEnable()
        {
            EventManager.Instance.ApplicationEvents.AudioObjectFinishedPlaying += OnAudioObjectFinishedPlaying;
        }
        private void OnDisable()
        {
            if (EventManager.IsApplicationQuitting)
                return;

            EventManager.Instance.ApplicationEvents.AudioObjectFinishedPlaying -= OnAudioObjectFinishedPlaying;
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
