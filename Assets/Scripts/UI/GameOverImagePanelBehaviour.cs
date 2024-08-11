#pragma warning disable CS0649
/**************************************************
 *  GameOverImagePanelBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines the behaviours for the panel that contains the primary image for the GameOver window.
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.UI.UIHelperBehaviour" />
    public class GameOverImagePanelBehaviour : UIHelperBehaviour
    {
        /// <summary>
        /// The victory image
        /// </summary>
        [SerializeField]
        private Image victoryImage;

        /// <summary>
        /// The eaten image
        /// </summary>
        [SerializeField]
        private Image eatenImage;

        /// <summary>
        /// The fallen image
        /// </summary>
        [SerializeField]
        private Image fallenImage;

        /// <summary>
        /// The game over condition
        /// </summary>
        [SerializeField, ReadOnly]
        private GameOverCondition gameOverCondition;

        /// <summary>
        /// Gets or sets the game over condition.
        /// </summary>
        /// <value>
        /// The game over condition.
        /// </value>
        public GameOverCondition GameOverCondition
        {
            get => this.gameOverCondition;
            set => this.gameOverCondition = value;
        }

        /// <summary>
        /// Enables the image based on the specified game over condition.
        /// </summary>
        /// <param name="gameOverCondition">The game over condition.</param>
        public void Enable(GameOverCondition gameOverCondition)
        {
            this.GameOverCondition = gameOverCondition;

            this.victoryImage.gameObject.SetActive(this.GameOverCondition.Equals(GameOverCondition.Victory));
            this.eatenImage.gameObject.SetActive(this.GameOverCondition.Equals(GameOverCondition.Eaten) || this.GameOverCondition.Equals(GameOverCondition.Quit));
            this.fallenImage.gameObject.SetActive(this.GameOverCondition.Equals(GameOverCondition.Fallen));

            this.Enable();
        }
    }
}
