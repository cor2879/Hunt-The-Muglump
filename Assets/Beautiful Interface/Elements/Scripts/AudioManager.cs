#pragma warning disable CS0109
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using OldSchoolGames.HuntTheMuglump.Scripts.Interfaces;
using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
using OldSchoolGames.HuntTheMuglump.Scripts.UI;

namespace Interface.Elements.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        private new AudioSource audio;
        public List<Sound> sounds;
        private static AudioManager instance;

        private void Awake()
        {
            audio = GetComponent<AudioSource>();
            if (instance)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public static IList<Sound> Sounds { get => instance.sounds; }

        private static OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.AudioManager SoundEffectManager
        {
            get
            {
                if (TitleScreenBehaviour.Instance != null)
                {
                    return TitleScreenBehaviour.Instance.SoundEffectManager;
                }

                return GameManager.Instance.SoundEffectManager;
            }
            
        }

        /// <summary>
        /// Play sound by referencing an AudioClip
        /// </summary>
        /// <param name="clip"></param>
        public static void Play(AudioClip clip)
        {
            if (!clip) return;
            
            if (SoundEffectManager == null)
            {
                Debug.LogError("No AudioManager instance running");
                return;
            }

            SoundEffectManager.PlayAudioOnce(clip);
        }

        /// <summary>
        /// Play sound by searching for SoundEffect enum in sounds
        /// </summary>
        /// <param name="effect"></param>
        public static void Play(SoundEffects effect)
        {
            Play(instance.sounds.ElementAtOrDefault((int)effect).Clip);
        }
    }

    [Serializable]
    public struct Sound
    {
        public SoundEffects Effect;
        public AudioClip Clip;
    }

    public enum SoundEffects
    {
        Success = 0,
        Error = 1,
        Hover = 2,
        Click = 3,
        Logout = 4
    }
}