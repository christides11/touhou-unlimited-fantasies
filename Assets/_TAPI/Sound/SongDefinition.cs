using UnityEngine;
using UnityEngine.Audio;

namespace TAPI.Sound
{
    /// <summary>
    /// Defines the parameters needed for a song.
    /// </summary>
    [CreateAssetMenu(fileName = "SongDefinition", menuName = "TAPI/Song Definition")]
    public class SongDefinition : ScriptableObject
    {
        // The name of the song.
        public string songName;
        // The mixer group this song belongs to.
        public AudioMixerGroup mixerGroup;
        // The song itself.
        public AudioClip song;
        // The priority of the audio source.
        [Range(0, 256)] public byte priority;
        // The volume of the audio source.
        [Range(0, 1)] public float volume;
        // If the song loops.
        public bool doesLoop;
        // The time at which it loops back.
        public double loopPoint;
        // The time at which it loops back to.
        public double loopBackTo;
    }
}