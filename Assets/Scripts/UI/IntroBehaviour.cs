#pragma warning disable CS0649
/**************************************************
 *  IntroBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;

    /// <summary>
    /// Defines the behaviours for the game intro
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class IntroBehaviour : ExitEarlyBehaviour
    {
        /// <summary>
        /// The muglump prefab
        /// </summary>
        [SerializeField]
        private GameObject muglumpPrefab;

        /// <summary>
        /// The player prefab
        /// </summary>
        [SerializeField]
        private GameObject playerPrefab;

        /// <summary>
        /// The player behaviour
        /// </summary>
        private IntroPlayerBehaviour playerBehaviour;

        /// <summary>
        /// The muglump behaviour
        /// </summary>
        private MuglumpBehaviour muglumpBehaviour;

        /// <summary>
        /// The title screen behaviour
        /// </summary>
        private TitleScreenBehaviour titleScreenBehaviour;

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <value>
        /// The player.
        /// </value>
        public IntroPlayerBehaviour Player
        {
            get
            {
                if (this.playerBehaviour == null)
                {
                    this.playerBehaviour = Instantiate(playerPrefab).GetComponent<IntroPlayerBehaviour>();
                    this.playerBehaviour.OnComplete = this.FinishIntro;
                }

                return this.playerBehaviour;
            }
        }

        /// <summary>
        /// Gets the muglump.
        /// </summary>
        /// <value>
        /// The muglump.
        /// </value>
        public MuglumpBehaviour Muglump
        {
            get
            {
                if (this.muglumpBehaviour == null)
                {
                    this.muglumpBehaviour = Instantiate(muglumpPrefab).GetComponent<MuglumpBehaviour>();
                }

                return this.muglumpBehaviour;
            }
        }

        /// <summary>
        /// Gets the title screen behaviour.
        /// </summary>
        /// <value>
        /// The title screen behaviour.
        /// </value>
        public TitleScreenBehaviour TitleScreenBehaviour
        {
            get
            {
                if (this.titleScreenBehaviour == null)
                {
                    this.titleScreenBehaviour = TitleScreenBehaviour.Instance;
                }

                return this.titleScreenBehaviour;
            }
        }

        /// <summary>
        /// Begins the intro.
        /// </summary>
        public void BeginIntro()
        {
            this.Muglump.gameObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            this.Player.gameObject.transform.SetPositionAndRotation(new Vector3(10, -0.6f, 0), Quaternion.identity);
            this.Player.Move(this.Muglump.Position);
        }

        /// <summary>
        /// Finishes the intro.
        /// </summary>
        public void FinishIntro()
        {
            Destroy(this.Muglump.gameObject);
            Destroy(this.Player.gameObject);

            this.Disable();
            this.TitleScreenBehaviour.FinishGameIntro();
        }

        protected override void Exit()
        {
            this.FinishIntro();
        }
    }
}
