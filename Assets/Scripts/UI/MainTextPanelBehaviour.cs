#pragma warning disable CS0649
/**************************************************
 *  MainTextPanelBehaviour.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    using BeautifulInterface = Interface.Elements.Scripts;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Extensions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    [RequireComponent(typeof(CanvasGroup))]
    public class MainTextPanelBehaviour : UIHelperBehaviour
    {
        #region private child references

        [SerializeField]
        private BeautifulInterface.TextUI mainPanelText;

        #endregion

        #region public child accessors

        public BeautifulInterface.TextUI MainPanelText
        {
            get
            {
                this.ValidateUnityEditorParameter(this.mainPanelText, nameof(this.mainPanelText));

                return this.mainPanelText;
            }
        }

        #endregion

        public string Text { get => this.MainPanelText.text; set => this.MainPanelText.text = value; }

        public void StartAnimation()
        {
            //if (!this.MainPanelText.IsAnimating)
            //{
            //    this.MainPanelText.StartAnimation();
            //}
        }

        public void SetActive(bool active)
        {
            this.gameObject.SetActive(active);
        }

        public void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(MainTextPanelBehaviour));
        }
    }
}
