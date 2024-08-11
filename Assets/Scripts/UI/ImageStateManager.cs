#pragma warning disable CS0649
/**************************************************
 *  ImageStateManager.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System.Linq;

    using UnityEngine;
    using UnityEngine.UI;

    public class ImageStateManager : UIHelperBehaviour
    {
        [SerializeField]
        private Image[] images;

        public void SetImageStates(params bool[] enabled)
        {
            if (images.Any())
            {
                for (var i = 0; i < enabled.Length && i < images.Length; i++)
                {
                    images[i].gameObject.SetActive(enabled[i]);
                }
            }
        }

        public void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(SettingsPanelBehaviourBase));
        }
    }
}
