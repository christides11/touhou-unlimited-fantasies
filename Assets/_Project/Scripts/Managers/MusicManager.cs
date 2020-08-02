using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUF.Sound;
using UnityEngine;
using UnityEngine.Audio;

namespace TUF.Core
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager instance;

        private Dictionary<AudioSource, SoundDefinition> currentlyPlaying = new Dictionary<AudioSource, SoundDefinition>();

        public void Awake()
        {
            instance = this;
        }

        public bool IsPlaying(SoundDefinition music)
        {
            foreach(AudioSource audio in currentlyPlaying.Keys)
            {
                if(currentlyPlaying[audio] == music)
                {
                    return true;
                }
            }
            return false;
        }

        public void Update()
        {
            double t = 0;
            foreach (var song in currentlyPlaying)
            {
                AudioSource aSource = song.Key;
                if (song.Value.doesLoop)
                {
                    SoundDefinition musicDefinition = song.Value;
                    t = (double)aSource.timeSamples / (double)aSource.clip.frequency;
                    if (t >= musicDefinition.loopPoint)
                    {
                        aSource.Play();
                        aSource.timeSamples = (int)(musicDefinition.loopBackTo * ((double)aSource.clip.frequency));
                    }
                }
                else
                {
                    if(aSource.time > aSource.clip.length)
                    {
                        Stop(aSource);
                    }
                }
            }
        }

        public AudioSource Play(SoundDefinition song, float startTime = 0)
        {
            AudioSource aSource = gameObject.AddComponent<AudioSource>();
            aSource.clip = song.sound;
            aSource.volume = song.volume;
            aSource.loop = song.doesLoop;
            aSource.outputAudioMixerGroup = song.mixerGroup;
            aSource.time = startTime;
            aSource.Play();
            currentlyPlaying.Add(aSource, song);
            return aSource;
        }

        public void Pause(AudioSource audioSource)
        {
            audioSource.Pause();
        }

        public void Resume(AudioSource audioSource)
        {
            audioSource.UnPause();
        }

        public void Stop(AudioSource audioSource)
        {
            audioSource.Stop();
            if (currentlyPlaying.ContainsKey(audioSource))
            {
                currentlyPlaying.Remove(audioSource);
            }
        }

        public void FadeOutAndStop(AudioSource audioSource, float time)
        {
            StartCoroutine(FadeOutAndStop(audioSource, audioSource.volume, 0, time));
        }

        private IEnumerator FadeOutAndStop(AudioSource audioSource, float start, float end, float time)
        {
            while (audioSource.volume > 0)
            {
                audioSource.volume -= start * Time.deltaTime / time;
                yield return null;
            }
            Stop(audioSource);
        }
    }
}