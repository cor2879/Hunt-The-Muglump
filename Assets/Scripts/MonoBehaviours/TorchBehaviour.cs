#pragma warning disable CS0649
/**************************************************
 *  TorchBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines the behaviour for torch objects
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class TorchBehaviour : MonoBehaviour
    {
        /// <summary>
        /// The flame behaviour
        /// </summary>
        [SerializeField]
        private FlameBehaviour flameBehaviour;

        /// <summary>
        /// Gets or sets the value indicating whether or not this torch is lit.
        /// </summary>
        public bool IsLit { get; private set; }

        /// <summary>
        /// Gets the flame behaviour.
        /// </summary>
        /// <value>
        /// The flame behaviour.
        /// </value>
        public FlameBehaviour FlameBehaviour
        {
            get => this.flameBehaviour;
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            this.FlameBehaviour.Extinguish();
        }

        /// <summary>
        /// Ignites this instance.
        /// </summary>
        public void Ignite()
        {
            this.FlameBehaviour.Ignite();
            this.IsLit = true;
        }

        /// <summary>
        /// Extinguishes this instance.
        /// </summary>
        public void Extinguish()
        {
            this.FlameBehaviour.Extinguish();
            this.IsLit = false;
        }
    }
}
