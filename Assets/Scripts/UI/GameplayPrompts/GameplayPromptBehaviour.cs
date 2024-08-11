#pragma warning disable CS0649
/**************************************************
 *  GameplayPromptBehaviour.cs
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
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class GameplayPromptBehaviour : PanelBehaviour
    {
        [SerializeField]
        private Image[] images;

        [SerializeField]
        private BeautifulInterface.ButtonUI button;

        public Image[] Images
        {
            get
            {
                if (!images.Any())
                {
                    Debug.Log($"There are no images loaded for {this}.  Did you forget to make an association in the Unity Editor?");
                }

                return this.images;
            }
        }

        public override void Hide()
        {
            this.transform.localScale = Vector3.zero;
        }

        public override void Show()
        {
            this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        public BeautifulInterface.ButtonUI Button
        {
            get
            {
                this.ValidateUnityEditorParameter(this.button, nameof(this.button));

                return this.button;
            }
        }

        public void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(GameplayPromptBehaviour));
        }
    }
}
