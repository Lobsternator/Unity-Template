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
            audioObject.Attacher!.enabled = true;
            audioObject.Attacher!.Update();

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

        private static AudioParameter[] CombineParameters(ICollection<AudioParameter> parameterCollectionA, ICollection<AudioParameter> parameterCollectionB)
        {
            AudioParameter[] allParameters = new AudioParameter[parameterCollectionA.Count + parameterCollectionB.Count];

            parameterCollectionA.CopyTo(allParameters, 0);
            parameterCollectionB.CopyTo(allParameters, parameterCollectionA.Count);

            return allParameters;
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

            AudioObject audioObject                 = _audioObjectPool.Get();
            Camera mainCamera                       = Camera.main;
            audioObject.Attacher!.Follow            = mainCamera ? mainCamera.transform : null;
            audioObject.Attacher!.LocalFollowOffset = Vector3.zero;

            return PlaySound_Internal(audioObject, eventReference, volume, pitch, parameters);
        }
        public static AudioObject PlaySound(EventReference eventReference, float volume, params AudioParameter[] parameters)
        {
            if (!CanPlaySound())
                return null;

            AudioObject audioObject                 = _audioObjectPool.Get();
            Camera mainCamera                       = Camera.main;
            audioObject.Attacher!.Follow            = mainCamera ? mainCamera.transform : null;
            audioObject.Attacher!.LocalFollowOffset = Vector3.zero;

            return PlaySound_Internal(audioObject, eventReference, volume, 1.0f, parameters);
        }
        public static AudioObject PlaySound(EventReference eventReference, params AudioParameter[] parameters)
        {
            if (!CanPlaySound())
                return null;

            AudioObject audioObject                 = _audioObjectPool.Get();
            Camera mainCamera                       = Camera.main;
            audioObject.Attacher!.Follow            = mainCamera ? mainCamera.transform : null;
            audioObject.Attacher!.LocalFollowOffset = Vector3.zero;

            return PlaySound_Internal(audioObject, eventReference, 1.0f, 1.0f, parameters);
        }
        public static AudioObject PlaySound(EventReference eventReference, AudioEventSettings settings, params AudioParameter[] parameters)
        {
            if (!CanPlaySound())
                return null;

            AudioObject audioObject                 = _audioObjectPool.Get();
            Camera mainCamera                       = Camera.main;
            audioObject.Attacher!.Follow            = mainCamera ? mainCamera.transform : null;
            audioObject.Attacher!.LocalFollowOffset = Vector3.zero;

            return PlaySound_Internal(audioObject, eventReference, settings.volume, settings.pitch, CombineParameters(settings.parameters, parameters));
        }

        public static AudioObject PlaySoundAttached(EventReference eventReference, float volume, float pitch, Transform follow, Vector3 followOffset, params AudioParameter[] parameters)
        {
            if (!CanPlaySound())
                return null;

            AudioObject audioObject                 = _audioObjectPool.Get();
            audioObject.Attacher!.Follow            = follow;
            audioObject.Attacher!.LocalFollowOffset = followOffset;

            return PlaySound_Internal(audioObject, eventReference, volume, pitch, parameters);
        }
        public static AudioObject PlaySoundAttached(EventReference eventReference, float volume, Transform follow, Vector3 followOffset, params AudioParameter[] parameters)
        {
            if (!CanPlaySound())
                return null;

            AudioObject audioObject                 = _audioObjectPool.Get();
            audioObject.Attacher!.Follow            = follow;
            audioObject.Attacher!.LocalFollowOffset = followOffset;

            return PlaySound_Internal(audioObject, eventReference, volume, 1.0f, parameters);
        }
        public static AudioObject PlaySoundAttached(EventReference eventReference, Transform follow, Vector3 followOffset, params AudioParameter[] parameters)
        {
            if (!CanPlaySound())
                return null;

            AudioObject audioObject                 = _audioObjectPool.Get();
            audioObject.Attacher!.Follow            = follow;
            audioObject.Attacher!.LocalFollowOffset = followOffset;

            return PlaySound_Internal(audioObject, eventReference, 1.0f, 1.0f, parameters);
        }
        public static AudioObject PlaySoundAttached(EventReference eventReference, AudioEventSettings settings, Transform follow, Vector3 followOffset, params AudioParameter[] parameters)
        {
            if (!CanPlaySound())
                return null;

            AudioObject audioObject                 = _audioObjectPool.Get();
            audioObject.Attacher!.Follow            = follow;
            audioObject.Attacher!.LocalFollowOffset = followOffset;

            return PlaySound_Internal(audioObject, eventReference, settings.volume, settings.pitch, CombineParameters(settings.parameters, parameters));
        }

        public static AudioObject PlaySoundAttached(EventReference eventReference, float volume, float pitch, Transform follow, params AudioParameter[] parameters)
        {
            if (!CanPlaySound())
                return null;

            AudioObject audioObject                 = _audioObjectPool.Get();
            audioObject.Attacher!.Follow            = follow;
            audioObject.Attacher!.LocalFollowOffset = Vector3.zero;

            return PlaySound_Internal(audioObject, eventReference, volume, pitch, parameters);
        }
        public static AudioObject PlaySoundAttached(EventReference eventReference, float volume, Transform follow, params AudioParameter[] parameters)
        {
            if (!CanPlaySound())
                return null;

            AudioObject audioObject                 = _audioObjectPool.Get();
            audioObject.Attacher!.Follow            = follow;
            audioObject.Attacher!.LocalFollowOffset = Vector3.zero;

            return PlaySound_Internal(audioObject, eventReference, volume, 1.0f, parameters);
        }
        public static AudioObject PlaySoundAttached(EventReference eventReference, Transform follow, params AudioParameter[] parameters)
        {
            if (!CanPlaySound())
                return null;

            AudioObject audioObject                 = _audioObjectPool.Get();
            audioObject.Attacher!.Follow            = follow;
            audioObject.Attacher!.LocalFollowOffset = Vector3.zero;

            return PlaySound_Internal(audioObject, eventReference, 1.0f, 1.0f, parameters);
        }
        public static AudioObject PlaySoundAttached(EventReference eventReference, AudioEventSettings settings, Transform follow, params AudioParameter[] parameters)
        {
            if (!CanPlaySound())
                return null;

            AudioObject audioObject                 = _audioObjectPool.Get();
            audioObject.Attacher!.Follow            = follow;
            audioObject.Attacher!.LocalFollowOffset = Vector3.zero;

            return PlaySound_Internal(audioObject, eventReference, settings.volume, settings.pitch, CombineParameters(settings.parameters, parameters));
        }

        public static AudioObject PlaySoundAttachedLocal(EventReference eventReference, float volume, float pitch, Transform follow, Vector3 localFollowOffset, params AudioParameter[] parameters)
        {
            if (!CanPlaySound())
                return null;

            AudioObject audioObject                 = _audioObjectPool.Get();
            audioObject.Attacher!.Follow            = follow;
            audioObject.Attacher!.LocalFollowOffset = localFollowOffset;

            return PlaySound_Internal(audioObject, eventReference, volume, pitch, parameters);
        }
        public static AudioObject PlaySoundAttachedLocal(EventReference eventReference, float volume, Transform follow, Vector3 localFollowOffset, params AudioParameter[] parameters)
        {
            if (!CanPlaySound())
                return null;

            AudioObject audioObject                 = _audioObjectPool.Get();
            audioObject.Attacher!.Follow            = follow;
            audioObject.Attacher!.LocalFollowOffset = localFollowOffset;

            return PlaySound_Internal(audioObject, eventReference, volume, 1.0f, parameters);
        }
        public static AudioObject PlaySoundAttachedLocal(EventReference eventReference, Transform follow, Vector3 localFollowOffset, params AudioParameter[] parameters)
        {
            if (!CanPlaySound())
                return null;

            AudioObject audioObject                 = _audioObjectPool.Get();
            audioObject.Attacher!.Follow            = follow;
            audioObject.Attacher!.LocalFollowOffset = localFollowOffset;

            return PlaySound_Internal(audioObject, eventReference, 1.0f, 1.0f, parameters);
        }
        public static AudioObject PlaySoundAttachedLocal(EventReference eventReference, AudioEventSettings settings, Transform follow, Vector3 localFollowOffset, params AudioParameter[] parameters)
        {
            if (!CanPlaySound())
                return null;

            AudioObject audioObject                 = _audioObjectPool.Get();
            audioObject.Attacher!.Follow            = follow;
            audioObject.Attacher!.LocalFollowOffset = localFollowOffset;

            return PlaySound_Internal(audioObject, eventReference, settings.volume, settings.pitch, CombineParameters(settings.parameters, parameters));
        }

        private static AudioObject OnAudioObjectCreate()
        {
            AudioObject audioObject     = Instantiate(AudioManagerData.Instance.audioObjectPrefab, Instance.transform).AudioObject;
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

#if UNITY_EDITOR
        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();

            OnAudioStartedPlaying = null;
            OnAudioStoppedPlaying = null;
        }
#endif
    }
}
