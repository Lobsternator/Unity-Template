using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using FMODUnity;
using Template.Core;

namespace Template.Audio
{
    [PersistentRuntimeObject(RuntimeInitializeLoadType.BeforeSceneLoad, -1000)]
    public class AudioManager : PersistentRuntimeSingleton<AudioManager, AudioManagerData>
    {
        private ObjectPool<AudioObject> _audioObjectPool;

        public event Action<AudioObject> OnAudioStartedPlaying;
        public event Action<AudioObject> OnAudioStoppedPlaying;

        private AudioObject PlaySound_Internal(AudioObject audioObject, EventReference eventReference, float volume, float pitch)
        {
            StudioEventEmitter eventEmitter = audioObject.EventEmitter;
            eventEmitter.EventReference     = eventReference;

            audioObject.Play();
            OnAudioStartedPlaying?.Invoke(audioObject);

            eventEmitter.EventInstance.setVolume(volume);
            eventEmitter.EventInstance.setPitch(pitch);

            return audioObject;
        }

        public AudioObject PlaySound(EventReference eventReference, float volume, float pitch)
        {
            AudioObject audioObject = _audioObjectPool.Get();

            return PlaySound_Internal(audioObject, eventReference, volume, pitch);
        }
        public AudioObject PlaySound(EventReference eventReference, float volume)
        {
            return PlaySound(eventReference, volume, 1.0f);
        }
        public AudioObject PlaySound(EventReference eventReference)
        {
            return PlaySound(eventReference, 1.0f, 1.0f);
        }
        public AudioObject PlaySound(EventReference eventReference, AudioEventSettings settings)
        {
            return PlaySound(eventReference, settings.volume, settings.pitch);
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

            Instantiate(PersistentData.bankLoaderPrefab, transform);
        }
    }
}
