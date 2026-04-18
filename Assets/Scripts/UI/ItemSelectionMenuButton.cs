namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;

    public class ItemSelectionMenuButton : UIHelperBehaviour
    {
        [SerializeField]
        private ItemType itemType;

        [SerializeField]
        private Image highlightImage;

        private PlayerBehaviour player;

        public ItemType ItemType => this.itemType;

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

        private void Update()
        {
            this.HighlightImage.enabled =
                (this.Player != null && this.Player.SelectedItemType == this.ItemType);
        }

        private void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(ItemSelectionMenuButton));
        }
    }
}