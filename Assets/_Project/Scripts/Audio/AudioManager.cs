using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using FMOD.Studio;
using FMODUnity;
using Template.Core;

namespace Template.Audio
{
    [Serializable]
    public struct ParameterValue
    {
        public string name;
        public float value;

        public ParameterValue(string name, float value)
        {
            this.name  = name;
            this.value = value;
        }
    }

    [PersistentRuntimeObject(RuntimeInitializeLoadType.BeforeSceneLoad, -1000)]
    public class AudioManager : PersistentRuntimeSingleton<AudioManager, AudioManagerData>
    {
        private ObjectPool<AudioObject> _audioObjectPool;

        public event Action<AudioObject> OnAudioStartedPlaying;
        public event Action<AudioObject> OnAudioStoppedPlaying;

        private AudioObject PlaySound_Internal(AudioObject audioObject, EventReference eventReference, float volume, float pitch, params ParameterValue[] parameters)
        {
            StudioEventEmitter eventEmitter = audioObject.EventEmitter;
            eventEmitter.EventReference     = eventReference;

            audioObject.Play();
            OnAudioStartedPlaying?.Invoke(audioObject);

            eventEmitter.EventInstance.setVolume(volume);
            eventEmitter.EventInstance.setPitch(pitch);

            for (int i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                eventEmitter.SetParameter(parameter.name, parameter.value);
            }

            return audioObject;
        }

        public AudioObject PlaySound(EventReference eventReference, float volume, float pitch, params ParameterValue[] parameters)
        {
            AudioObject audioObject = _audioObjectPool.Get();

            return PlaySound_Internal(audioObject, eventReference, volume, pitch, parameters);
        }
        public AudioObject PlaySound(EventReference eventReference, float volume, params ParameterValue[] parameters)
        {
            return PlaySound(eventReference, volume, 1.0f, parameters);
        }
        public AudioObject PlaySound(EventReference eventReference, params ParameterValue[] parameters)
        {
            return PlaySound(eventReference, 1.0f, 1.0f, parameters);
        }
        public AudioObject PlaySound(EventReference eventReference, AudioEventSettings settings, params ParameterValue[] parameters)
        {
            settings.parameters.AddRange(parameters);

            return PlaySound(eventReference, settings.volume, settings.pitch, settings.parameters.ToArray());
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
