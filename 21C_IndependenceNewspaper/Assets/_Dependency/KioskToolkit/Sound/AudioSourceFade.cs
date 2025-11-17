using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arc9.Unity.KioskToolkit.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceFade : MonoBehaviour
    {
        public enum AudioState
        {
            Stop = 0,
            Playing,
            FadeIn,
            FadeOut,
        }

        private AudioSource audioSource;
        private AudioState state = 0;

        private float fadeTime = 1; // second
        private float elapsedFadeTime = 0.0f;

        public AudioClip Clip
        {
            get { return audioSource.clip; }
            set { audioSource.clip = value; }
        }

        public bool PlayOnAwake
        {
            get { return audioSource.playOnAwake; }
            set
            {
                audioSource.playOnAwake = value;
                if (audioSource.playOnAwake)
                {
                    audioSource.Play();
                }
            }
        }

        public float targetVolume = 1.0f;
        public float Volume
        {
            get { return audioSource.volume; }
            set { audioSource.volume = value; }
        }

        public bool Loop
        {
            get { return audioSource.loop; }
            set { audioSource.loop = value; }
        }

        public AudioSource AudioSource
        {
            get { return audioSource; }
        }

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void Play(bool imm = false)
        {
            if (imm)
            {
                state = AudioState.Playing;
                audioSource.volume = Volume;
                audioSource.Play();
            }
            else
            {
                elapsedFadeTime = 0.0f;
                state = AudioState.FadeIn;
                audioSource.volume = 0.0f;
                audioSource.Play();
            }
        }

        public void Stop(bool imm = false)
        {
            if (imm)
            {
                state = AudioState.Stop;
                audioSource.Stop();
            }
            else
            {
                elapsedFadeTime = 0.0f;
                state = AudioState.FadeOut;
            }
        }

        private void Update()
        {
            if (audioSource.isPlaying)
            {
                if (state == AudioState.FadeOut)
                {
                    elapsedFadeTime += Time.deltaTime;

                    audioSource.volume = targetVolume - ((elapsedFadeTime / fadeTime) * targetVolume);
                    if (audioSource.volume <= 0.0f)
                    {
                        state = AudioState.Stop;
                        audioSource.volume = 0.0f;
                        audioSource.Stop();
                    }
                }
                else if (state == AudioState.FadeIn)
                {
                    elapsedFadeTime += Time.deltaTime;
                    audioSource.volume = (elapsedFadeTime / fadeTime) * targetVolume;
                    if (audioSource.volume >= targetVolume)
                    {
                        state = AudioState.Playing;
                        audioSource.volume = targetVolume;
                    }
                }
            }
        }
    }
}