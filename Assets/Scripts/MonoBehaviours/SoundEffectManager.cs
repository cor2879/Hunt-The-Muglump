/**************************************************
 *  SoundEffectManager.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Singleton class which manages all audio features of a game.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(AudioManager))]
    public class SoundEffectManager : MonoBehaviour
    {
        private AudioManager audioManager;

        private AudioManager AudioManager
        {
            get
            {
                if (this.audioManager == null)
                {
                    this.audioManager = this.GetComponent<AudioManager>();
                }

                return this.audioManager;
            }
        }
    }
}
