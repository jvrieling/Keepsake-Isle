using System.Collections;
using UnityEngine;

namespace GBTemplate
{
    public class GBSoundController : MonoBehaviour
    {
        public float GlobalAudioVolume = 1;
        public float CurrentMusicVolume = 1;
        public float CurrentSoundVolume = 1;

        public AudioSource[] soundSources;
        public AudioSource musicSource;

        public void PlaySound(AudioClip clip)
        {
            if (clip != null)
            {
                soundSources[0].loop = false;
                soundSources[0].PlayOneShot(clip);
            }
        }

        public void PlaySound(AudioClip clip, int channel)
        {
            if (clip != null)
            {
                soundSources[channel].loop = false;
                soundSources[channel].PlayOneShot(clip);
            }
        }

        public void LoopSound(AudioClip clip)
        {
            if (clip != null)
            {
                soundSources[0].loop = true;
                soundSources[0].clip = clip;
                soundSources[0].Play();
            }
        }

        public void LoopSound(AudioClip clip, int channel)
        {
            if (clip != null)
            {
                soundSources[channel].loop = true;
                soundSources[channel].clip = clip;
                soundSources[channel].Play();
            }
        }

        public void PlayMusic(AudioClip clip)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }

        public void PlayMusicOneShot(AudioClip clip)
        {
            musicSource.PlayOneShot(clip);
        }

        public void UpdateSoundVolume(float newVolume)
        {
            CurrentSoundVolume = newVolume;

            for (int i = 0; i < soundSources.Length; i++)
            {
                soundSources[i].volume = CurrentSoundVolume * GlobalAudioVolume;
            }
        }

        public void UpdateMusicVolume(float newVolume)
        {
            CurrentMusicVolume = newVolume;
            musicSource.volume = CurrentMusicVolume * GlobalAudioVolume;
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }

        public void StopAllSounds()
        {
            for (int i = 0; i < soundSources.Length; i++)
            {
                soundSources[i].pitch = 1;
                soundSources[i].Stop();
            }
        }

        public void UpdateGlobalVolume(float newVolume)
        {
            GlobalAudioVolume = newVolume;

            for (int i = 0; i < soundSources.Length; i++)
            {
                soundSources[i].volume = CurrentSoundVolume * GlobalAudioVolume;
            }

            musicSource.volume = CurrentMusicVolume * GlobalAudioVolume;
        }

        public void FadeOutSoundChannel(int channel, float fadeTime)
        {
            StartCoroutine(FadeOut(soundSources[channel], fadeTime));
        }

        public void FadeOutMusic(float fadeTime)
        {
            StartCoroutine(FadeOut(musicSource, fadeTime));
        }

        public IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
        {
            float startVolume = audioSource.volume;
            //FadingOut = true;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

                yield return null;
            }

            //FadingOut = false;
            audioSource.Stop();
            audioSource.volume = startVolume;
        }
    }
}