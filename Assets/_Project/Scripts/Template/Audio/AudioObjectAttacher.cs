using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Audio
{
    [RequireComponent(typeof(AudioObject))]
    public class AudioObjectAttacher : MonoBehaviour
    {
        [field: SerializeField] public Transform Follow { get; set; }
        [field: SerializeField] public Vector3 LocalFollowOffset { get; set; }
        public Vector3 FollowOffset
        {
            get => Follow ? Follow.TransformVector(LocalFollowOffset) : LocalFollowOffset;
            set => LocalFollowOffset = Follow ? Follow.InverseTransformVector(value) : value;
        }

        private AudioObject _audioObject;
        public AudioObject AudioObject
        {
            get
            {
                if (!_audioObject)
                    _audioObject = GetComponent<AudioObject>();

                return _audioObject;
            }
        }

        private void OnAudioObjectStoppedPlaying(AudioObject audioObject)
        {
            transform.localPosition = Vector3.zero;
            enabled                 = false;
        }

        private void Awake()
        {
            _audioObject = GetComponent<AudioObject>();
        }

        private void OnEnable()
        {
            _audioObject.StoppedPlaying += OnAudioObjectStoppedPlaying;
        }
        private void OnDisable()
        {
            _audioObject.StoppedPlaying -= OnAudioObjectStoppedPlaying;
        }

        public void Update()
        {
            if (Follow) transform.position = Follow.position + Follow.rotation * LocalFollowOffset;
        }
    }
}
