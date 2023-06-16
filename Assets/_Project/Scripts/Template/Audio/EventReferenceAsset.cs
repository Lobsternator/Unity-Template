using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Template.Core;

namespace Template.Audio
{
    /// <summary>
    /// <see cref="ScriptableReference{TValue}"/> wrapper around <see cref="EventReference"/>.
    /// </summary>
    [CreateAssetMenu(fileName = "new EventReference", menuName = "Audio/EventReference")]
    public class EventReferenceAsset : ScriptableReference<EventReference>
    {

    }
}
