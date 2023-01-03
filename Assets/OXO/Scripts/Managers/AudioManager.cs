using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace OxoGames
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : Singleton<AudioManager>
    {
        public Sound[] sounds;
        private AudioSource _audioSource;
        public bool isMuted;
        public GameObject soundButton;

        [TextArea, DisableIf("@1>0"), SerializeField]
        private string documentation = $"AudioManager.Play('name');";

        public Sprite mutedSprite;
        public Sprite unMutedSprite;
        public Image muteImage;

        private void Awake() => Init();

        private void Init()
        {
            isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1 ? true : false;
            Mute(isMuted);

            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }

            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
            }
        }
        public void ToogleMute() // call via onclick
        {
            isMuted = !isMuted;

            int volume = isMuted ? 0 : 1;
            AudioListener.volume = volume;
            PlayerPrefs.SetInt("IsMuted", isMuted  ? 1 : 0);
            SetSprites();
            PlayerPrefs.Save();
        }
        public void SetSprites()
        {
            if(!unMutedSprite || !mutedSprite || !muteImage) { return; }
            muteImage.sprite = isMuted ? mutedSprite : unMutedSprite;
        }

        public void Mute(bool mute)
        {
            int volume = mute ? 0 : 1;
            AudioListener.volume = volume;
            SetSprites();
        }
        public static AudioSource Play(string name)
        {
            Sound s = Array.Find(Instance.sounds, sound => sound.name == name);
            s.source.Play();
            return s.source;
        }

        public static AudioSource GetSource(string name)
        {
            Sound s = Array.Find(Instance.sounds, sound => sound.name == name);
            return s.source;
        }
        protected void Stop()
        {
            foreach (var audioSource in GetComponentsInChildren<AudioSource>())
            {
                audioSource.Stop();
            }
        }


        [Serializable]
        public class Sound
        {
            public string name = "ExampleHitAudio ";
            public AudioClip clip;



            [Range(0f, 1f),] public float volume = 0.3f;

            [Range(0f, 1f)] public float pitch = 1f;

            [HideInInspector] public AudioSource source;
            private static AudioManager _audioManager => Instance;

            #region InfoBox
            [ShowIf("@volume>0.5f", InfoMessageType.Warning),
             InfoBox("Volume is higher than 0.5 be careful! .", InfoMessageType.Warning)]
            [SerializeField]
            private string messageText = "Volume is high.";
            #endregion


            [Sirenix.OdinInspector.Button]
            public void Play()
            {
                if (source == null)
                {
                    AudioSource audioSource = _audioManager.GetComponent<AudioSource>();
                    audioSource.clip = clip;
                    audioSource.volume = volume;
                    audioSource.pitch = pitch;
                    audioSource.Play();
                }
                else
                {
                    source?.Play();
                }
            }

            [Sirenix.OdinInspector.Button]
            public void Stop()
            {
                if (source != null)
                {
                    source.Stop();
                }
                else
                {
                    _audioManager.Stop();
                }
            }
        }
    }
}