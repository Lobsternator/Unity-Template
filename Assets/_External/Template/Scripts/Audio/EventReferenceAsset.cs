using FMODUnity;
using Template.Core;
using UnityEngine;

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
