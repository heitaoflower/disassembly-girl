using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Utils;
namespace Manager
{
    public class SoundManager : Singleton<SoundManager>
    {
        public override void Initialize()
        {
            base.Initialize();

            availableSounds = new Stack<SKSound>(maxCapacity);
            playingSounds = new List<SKSound>();

            for (int index = 0; index < initialCapacity; index++)
            {
                availableSounds.Push(new SKSound(this));
            }
        }

        public override void Release()
        {
            base.Release();
        }

        public float soundEffectVolume = 1.0f;
        public int initialCapacity = 10;
        public int maxCapacity = 15;
        public bool dontDestroyOnLoad = true;
        public bool clearAllAudioClipsOnLevelLoad = true;
        [NonSerialized]
        public SKSound backgroundSound = null;
        private SKSound oneShotSound = null;

        private Stack<SKSound> availableSounds = null;
        private List<SKSound> playingSounds = null;

        #region MonoBehaviour

        void OnLevelWasLoaded(int level)
        {
            if (dontDestroyOnLoad && clearAllAudioClipsOnLevelLoad)
            {
                for (int index = playingSounds.Count - 1; index >= 0; index--)
                {
                    SKSound sound = playingSounds[index];
                    sound.audioSource.clip = null;

                    availableSounds.Push(sound);
                    playingSounds.RemoveAt(index);
                }
            }
        }

        void Update()
        {
            for (int index = playingSounds.Count - 1; index >= 0; index--)
            {
                SKSound sound = playingSounds[index];

                if (sound.playingLoopingAudio)
                {
                    continue;
                }

                sound.elapsedTime += Time.deltaTime;
                if (sound.elapsedTime > sound.audioSource.clip.length)
                {
                    sound.Stop();
                }
            }
        }

        #endregion

        private SKSound NextAvailableSound()
        {
            SKSound sound = null;

            if (availableSounds.Count > 0)
            {
                sound = availableSounds.Pop();
            }

            if (sound == null)
            {
                sound = new SKSound(this);
            }

            playingSounds.Add(sound);

            return sound;

        }

        public void PlayBackgroundMusic(AudioClip audioClip, float volume, bool loop = true)
        {
            if (backgroundSound == null)
            {
                backgroundSound = new SKSound(this);
            }

            backgroundSound.PlayAudioClip(audioClip, volume, 1.0f, 0.0f);
            backgroundSound.SetLoop(loop);
        }

        public void PlayOneShot(AudioClip audioClip, float volumeScale = 1.0f)
        {
            if (oneShotSound == null)
            {
                oneShotSound = new SKSound(this);
            }

            oneShotSound.audioSource.PlayOneShot(audioClip, volumeScale * soundEffectVolume);
        }

        public SKSound PlaySound(AudioClip audioClip)
        {
            return PlaySound(audioClip, 1.0f);
        }

        public SKSound PlaySound(AudioClip audioClip, float volume)
        {
            return PlaySound(audioClip, volume, 1.0f, 0.0f);
        }

        public SKSound PlayPitchedSound(AudioClip audioClip, float pitch)
        {
            return PlaySound(audioClip, 1f, pitch, 0f);
        }

        public SKSound PlayPannedSound(AudioClip audioClip, float pan)
        {
            return PlaySound(audioClip, 1f, 1f, pan);
        }

        public SKSound PlaySound(AudioClip audioClip, float volumeScale, float pitch, float pan)
        {
            SKSound sound = NextAvailableSound();
            sound.PlayAudioClip(audioClip, volumeScale * soundEffectVolume, pitch, pan);

            return sound;
        }

        public SKSound PlaySoundLooped(AudioClip audioClip)
        {
            SKSound sound = NextAvailableSound();
            sound.PlayAudioClip(audioClip, soundEffectVolume, 1f, 0f);
            sound.SetLoop(true);

            return sound;
        }

        public void RecycleSound(SKSound sound)
        {
            if (sound == backgroundSound)
            {
                return;
            }

            int index = 0;

            while (index < playingSounds.Count)
            {
                if (playingSounds[index] == sound)
                {
                    break;
                }

                index++;
            }

            playingSounds.RemoveAt(index);

            if (availableSounds.Count + playingSounds.Count >= maxCapacity)
            {
                Destroy(sound.audioSource);
            }
            else
            {
                availableSounds.Push(sound);
            }
        }

        #region SKSound inner class

        public class SKSound
        {
            private SoundManager manager = null;

            public AudioSource audioSource = null;
            internal Action CompletionHandler = null;
            internal bool playingLoopingAudio = false;
            internal float elapsedTime = default(float);

            public SKSound(SoundManager manager)
            {
                this.manager = manager;
                audioSource = this.manager.gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }

            private IEnumerator FadeOut(float duration, Action OnComplete)
            {
                float startingVolum = audioSource.volume;

                while (audioSource.volume > 0.0f && elapsedTime < audioSource.clip.length)
                {
                    audioSource.volume -= Time.deltaTime * startingVolum / duration;
                    yield return null;
                }

                Stop();

                if (OnComplete != null)
                {
                    OnComplete();
                }
            }

            public SKSound SetLoop(bool loop)
            {
                playingLoopingAudio = true;
                audioSource.loop = loop;

                return this;
            }

            public SKSound SetCompletionHandler(Action Handler)
            {
                CompletionHandler = Handler;

                return this;
            }

            public void Stop()
            {
                audioSource.Stop();

                if (CompletionHandler != null)
                {
                    CompletionHandler();
                    CompletionHandler = null;
                }

                manager.RecycleSound(this);
            }

            public void FadeOutAndStop(float duration, Action Handler = null)
            {
                manager.StartCoroutine
                (
                    FadeOut(duration, () =>
                    {
                        if (Handler != null)
                        {
                            Handler();
                        }
                    })
                );
            }

            internal void PlayAudioClip(AudioClip audioClip, float volum, float pitch, float pan)
            {
                playingLoopingAudio = false;
                elapsedTime = 0;

                audioSource.clip = audioClip;
                audioSource.volume = volum;
                audioSource.pitch = pitch;
                audioSource.panStereo = pan;

                audioSource.loop = false;
                audioSource.mute = false;

                audioSource.Play();
            }
        }
        #endregion

    }
}