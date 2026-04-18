#pragma warning disable CS0649
/**************************************************
 *  CoverScentButton.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class CoverScentButton : UIHelperBehaviour
    {
        [SerializeField]
        private Image activeImage;

        public Image ActiveImage
        {
            get 
            {
                this.ValidateUnityEditorParameter(this.activeImage, nameof(this.activeImage));

                return activeImage; 
            }
        }

        public bool ShowActiveEffect
        {
            get
            {
                if (PlayerBehaviour.Instance != null)
                {
                    return PlayerBehaviour.Instance.IsCoverScentActive ?
                        Utility.Between(PlayerBehaviour.Instance.CoverScentBehaviour.ActiveTurns, 1, 2) ?
                            Blinkronizer.Instance.BlinkOn : true : false;
                }

                return false;
            }
        }

        public void Update()
        {
            this.ActiveImage.gameObject.SetActive(this.ShowActiveEffect);
        }

        public void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(CoverScentButton));
        }
    }
}
