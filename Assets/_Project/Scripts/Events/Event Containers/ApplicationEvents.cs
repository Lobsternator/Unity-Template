using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Audio;
using Template.Core;

namespace Template.Events
{
    public class ApplicationEvents : EventContainer
    {
        public Action<State<ApplicationStateMachine>> ApplicationStateDisabled;
        public Action<State<ApplicationStateMachine>> ApplicationStateEnabled;
        public Action<AudioObject>                    AudioObjectFinishedPlaying;
        public Action<State<ApplicationStateMachine>> BeforeApplicationStateDisabled;
        public Action<State<ApplicationStateMachine>> BeforeApplicationStateEnabled;
        public Action<float>                          TimeScaleChanged;
        public Action                                 TimeFroze;
        public Action<float>                          TimeUnfroze;
    }
}
