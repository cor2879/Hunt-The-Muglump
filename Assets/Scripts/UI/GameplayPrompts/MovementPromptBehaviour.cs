#pragma warning disable CS0649
/**************************************************
 *  MovementPromptBehaviour.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI.GameplayPrompts
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.UI;

    using BeautifulInterface = Interface.Elements.Scripts;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;
    using OldSchoolGames.HuntTheMuglump.Scripts.Platform;

    [RequireComponent(typeof(GameplayPromptBehaviour))]
    public class MovementPromptBehaviour : MonoBehaviour
    {
        #region private child references

        [SerializeField]
        private Image moveLeftImage;

        [SerializeField]
        private Image moveUpImage;

        [SerializeField]
        private Image moveRightImage;

        [SerializeField]
        private Image moveDownImage;

        [SerializeField]
        private BeautifulInterface.ButtonUI button;

        #endregion

        #region public child accessors

        public Image MoveLeftImage
        {
            get
            {
                // this.ValidateUnityEditorParameter(this.moveLeftImage, nameof(this.moveLeftImage));

                return this.moveLeftImage;
            }
        }

        public Image MoveUpImage
        {
            get
            {
                // this.ValidateUnityEditorParameter(this.moveUpImage, nameof(this.moveUpImage));

                return this.moveUpImage;
            }
        }

        public Image MoveRightImage
        {
            get
            {
                // this.ValidateUnityEditorParameter(this.moveRightImage, nameof(this.moveRightImage));

                return this.moveRightImage;
            }
        }

        public Image MoveDownImage
        {
            get
            {
                // this.ValidateUnityEditorParameter(this.moveDownImage, nameof(this.moveDownImage));

                return this.moveDownImage;
            }
        }

        public BeautifulInterface.ButtonUI Button
        {
            get
            {
                this.ValidateUnityEditorParameter(this.button, nameof(this.button));

                return this.button;
            }
        }

        #endregion

        public PlayerBehaviour Player { get => PlayerBehaviour.Instance; }

        private void Update()
        {
            if (Player == null)
            {
                return;
            }

            this.MoveLeftImage?.gameObject.SetActive(Player.CurrentRoom?.GetAdjacentRoom(Direction.West) != null);
            this.MoveUpImage?.gameObject.SetActive(Player.CurrentRoom?.GetAdjacentRoom(Direction.North) != null);
            this.MoveRightImage?.gameObject.SetActive(Player.CurrentRoom?.GetAdjacentRoom(Direction.East) != null);
            this.MoveDownImage?.gameObject.SetActive(Player.CurrentRoom?.GetAdjacentRoom(Direction.South) != null);
        }

        public void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(NewGameMenuBehaviour));
        }
    }
}
