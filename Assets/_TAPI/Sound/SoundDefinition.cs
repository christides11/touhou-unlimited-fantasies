using UnityEngine;
using UnityEngine.Audio;

namespace TAPI.Sound
{
    /// <summary>
    /// Defines the parameters needed for a sound.
    /// </summary>
    [CreateAssetMenu(fileName = "SoundDefinition", menuName = "TAPI/Sound Definition")]
    public class SoundDefinition : ScriptableObject
    {
        // The name of the sound.
        public string soundName;
        // The mixer group this sound belongs to.
        public AudioMixerGroup mixerGroup;
        // The sound itself.
        public AudioClip sound;
        // The priority of the audio source.
        [Range(0, 256)] public byte priority;
        // The volume of the audio source.
        [Range(0, 1)] public float volume;
    }
}