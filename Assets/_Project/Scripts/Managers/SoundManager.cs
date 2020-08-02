using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUF.Sound;
using UnityEngine;
using UnityEngine.Audio;

namespace TUF.Sound
{
    public class SoundManager : MonoBehaviour
    {
        private static Dictionary<AudioSource, SoundDefinition> sounds = new Dictionary<AudioSource, SoundDefinition>();

        public static bool IsPlaying(SoundDefinition sound)
        {
            foreach (AudioSource audio in sounds.Keys)
            {
                if (sounds[audio] == sound)
                {
                    return true;
                }
            }
            return false;
        }

        double t = 0;
        public void Update()
        {
            t = 0;
            foreach (var sound in sounds)
            {
                AudioSource aSource = sound.Key;
                if (sound.Value.doesLoop)
                {
                    SoundDefinition musicDefinition = sound.Value;
                    t = (double)aSource.timeSamples / (double)aSource.clip.frequency;
                    if (t >= musicDefinition.loopPoint)
                    {
                        aSource.Play();
                        aSource.timeSamples = (int)(musicDefinition.loopBackTo * ((double)aSource.clip.frequency));
                    }
                }
                else
                {
                    if (aSource.time > aSource.clip.length)
                    {
                        Stop(aSource);
                    }
                }
            }
        }

        public static GameObject Play(SoundDefinition sound, float startTime = 0, Transform parent = null)
        {
            if(!sound)
            {
                return null;
            }
            GameObject go = new GameObject(sound.name);
            go.transform.SetParent(parent);
            AudioSource aSource = go.AddComponent<AudioSource>();
            aSource.clip = sound.sound;
            aSource.volume = sound.volume;
            aSource.loop = sound.doesLoop;
            aSource.outputAudioMixerGroup = sound.mixerGroup;
            aSource.time = startTime == 0 ? sound.startTime : startTime;
            aSource.Play();
            sounds.Add(aSource, sound);
            return go;
        }

        public static void Pause(AudioSource audioSource)
        {
            audioSource.Pause();
        }

        public static void Resume(AudioSource audioSource)
        {
            audioSource.UnPause();
        }

        public static void Stop(AudioSource audioSource)
        {
            audioSource.Stop();
            if (sounds.ContainsKey(audioSource))
            {
                sounds.Remove(audioSource);
            }
            Destroy(audioSource.gameObject);
        }

        /*
        public static void FadeOutAndStop(AudioSource audioSource, float time)
        {
            StartCoroutine(FadeOutAndStop(audioSource, audioSource.volume, 0, time));
        }

        private static IEnumerator FadeOutAndStop(AudioSource audioSource, float start, float end, float time)
        {
            while (audioSource.volume > 0)
            {
                audioSource.volume -= start * Time.deltaTime / time;
                yield return null;
            }
            Stop(audioSource);
        }*/
    }
}