#pragma warning disable CS0649
/**************************************************
 *  ArrowSelectionMenuButton.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;

    public class ArrowSelectionMenuButton : UIHelperBehaviour
    {
        #region private members

        [SerializeField]
        private ArrowType arrowType; 

        [SerializeField]
        private Image highlightImage;

        private PlayerBehaviour player;

        #endregion

        #region public accessors

        public ArrowType ArrowType { get => this.arrowType; private set => this.arrowType = value; }

        public Image HighlightImage
        {
            get
            {
                this.ValidateUnityEditorParameter(this.highlightImage, nameof(this.highlightImage));

                return this.highlightImage;
            }
        }

        public PlayerBehaviour Player
        {
            get
            {
                if (this.player == null && PlayerBehaviour.Instance != null)
                {
                    this.player = PlayerBehaviour.Instance;
                }
            
                return this.player;
            }
        }

        #endregion

        private void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(ArrowSelectionMenuButton));
        }

        private void Update()
        {
            this.HighlightImage.enabled = (this.Player != null && this.Player.SelectedArrowType == this.ArrowType);
        }
    }
}
