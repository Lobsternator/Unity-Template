using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using FMODUnity;
using Template.Core;

namespace Template.Audio
{
    [Serializable]
    public struct AudioParameter
    {
        public string name;
        public float value;

        public AudioParameter(string name, float value)
        {
            this.name  = name;
            this.value = value;
        }
    }

    [PersistentRuntimeObject(RuntimeInitializeLoadType.BeforeSceneLoad, -1000)]
    public class AudioManager : PersistentRuntimeSingleton<AudioManager, AudioManagerData>
    {
        public static event Action<AudioObject> OnAudioStartedPlaying;
        public static event Action<AudioObject> OnAudioStoppedPlaying;

        private static ObjectPool<AudioObject> _audioObjectPool;

        private static AudioObject PlaySound_Internal(AudioObject audioObject, EventReference eventReference, float volume, float pitch, params AudioParameter[] parameters)
        {
            StudioEventEmitter eventEmitter = audioObject.EventEmitter;
            eventEmitter.EventReference     = eventReference;

            audioObject.Play();
            OnAudioStartedPlaying?.Invoke(audioObject);

            audioObject.SetVolume(volume);
            audioObject.SetPitch(pitch);

            for (int i = 0; i < parameters.Length; i++)
                audioObject.SetParameter(parameters[i]);

            return audioObject;
        }

        private static bool CanPlaySound()
        {
#if UNITY_EDITOR
            if (!Instance)
            {
                Debug.LogWarning($"{nameof(AudioManager)} instance needs to exist in order to play sounds!");
                return false;
            }
#endif

            return Instance.isActiveAndEnabled;
        }

        public static AudioObject PlaySound(EventReference eventReference, float volume, float pitch, params AudioParameter[] parameters)
        {
            if (!CanPlaySound())
                return null;

            AudioObject audioObject = _audioObjectPool.Get();

            return PlaySound_Internal(audioObject, eventReference, volume, pitch, parameters);
        }
        public static AudioObject PlaySound(EventReference eventReference, float volume, params AudioParameter[] parameters)
        {
            if (!CanPlaySound())
                return null;

            AudioObject audioObject = _audioObjectPool.Get();

            return PlaySound_Internal(audioObject, eventReference, volume, 1.0f, parameters);
        }
        public static AudioObject PlaySound(EventReference eventReference, params AudioParameter[] parameters)
        {
            if (!CanPlaySound())
                return null;

            AudioObject audioObject = _audioObjectPool.Get();

            return PlaySound_Internal(audioObject, eventReference, 1.0f, 1.0f, parameters);
        }
        public static AudioObject PlaySound(EventReference eventReference, AudioEventSettings settings, params AudioParameter[] parameters)
        {
            if (!CanPlaySound())
                return null;

            AudioObject audioObject        = _audioObjectPool.Get();
            AudioParameter[] allParameters = new AudioParameter[settings.parameters.Count + parameters.Length];

            settings.parameters.CopyTo(allParameters);
            parameters.CopyTo(allParameters, settings.parameters.Count);

            return PlaySound_Internal(audioObject, eventReference, settings.volume, settings.pitch, allParameters);
        }

        private static AudioObject OnAudioObjectCreate()
        {
            AudioObject audioObject     = Instantiate(AudioManagerData.Instance.audioObjectPrefab, Instance.transform).GetComponent<AudioObject>();
            audioObject.StoppedPlaying += OnAudioObjectStoppedPlaying;

            return audioObject;
        }
        private static void OnAudioObjectGet(AudioObject audioObject)
        {
            audioObject.gameObject.SetActive(true);
        }
        private static void OnAudioObjectRelease(AudioObject audioObject)
        {
            audioObject.gameObject.SetActive(false);
        }
        private static void OnAudioObjectDestroy(AudioObject audioObject)
        {
            audioObject.StoppedPlaying -= OnAudioObjectStoppedPlaying;
            Destroy(audioObject.gameObject);
        }

        private static void OnAudioObjectStoppedPlaying(AudioObject audioObject)
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

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();

            OnAudioStartedPlaying = null;
            OnAudioStoppedPlaying = null;
        }
    }
}
