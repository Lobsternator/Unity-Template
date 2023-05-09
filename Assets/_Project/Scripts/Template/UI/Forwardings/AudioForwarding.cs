using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Template.Audio;

namespace Template.UI
{
    public class AudioForwarding : MonoBehaviour
    {
        public float Volume { get; set; } = 1.0f;
        public float Pitch { get; set; }  = 1.0f;
        public Transform Follow { get; set; }
        public Dictionary<string, AudioParameter> parameters = new Dictionary<string, AudioParameter>();

        private string _selectedParameter = string.Empty;

        public void AddParameter(string name)
        {
            parameters.Add(name, new AudioParameter(name, 0.0f));
            SelectParameter(name);
        }
        public void SelectParameter(string name)
        {
            _selectedParameter = name;
        }
        public void SetParameterValue(float value)
        {
            AudioParameter audioParameter  = parameters[_selectedParameter];
            audioParameter.value           = value;
            parameters[_selectedParameter] = audioParameter;
        }

        public void ResetVolume()
        {
            Volume = 1.0f;
        }
        public void ResetPitch()
        {
            Pitch = 1.0f;
        }
        public void ClearFollow()
        {
            Follow = null;
        }
        public void ClearParameters()
        {
            parameters.Clear();
        }

        public void Reset()
        {
            ResetVolume();
            ResetPitch();
            ClearFollow();
            ClearParameters();
        }

        public void PlaySound(ScriptableEventReference eventReference)
        {
            AudioManager.PlaySound(eventReference.Value, Volume, Pitch, parameters.Values.ToArray());
        }
        public void PlaySoundAttached(ScriptableEventReference eventReference)
        {
            AudioManager.PlaySoundAttached(eventReference.Value, Volume, Pitch, Follow, parameters.Values.ToArray());
        }
    }
}
