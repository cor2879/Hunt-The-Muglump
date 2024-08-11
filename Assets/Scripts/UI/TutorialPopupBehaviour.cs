#pragma warning disable CS0649
/**************************************************
 *  TutorialManagerBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System.Collections;

    using UnityEngine;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class TutorialPopupBehaviour : UIHelperBehaviour
    {
        [SerializeField]
        private Text textBox;

        [SerializeField]
        private new string name;

        public Text TextBox
        {
            get => this.textBox;
        }

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(this.name))
                {
                    throw new UIException($"The {nameof(this.name)} parameter needs to be set in the Unity Editor.");
                }

                return this.name;
            }
        }

        public override void Enable()
        {
            GameManager.Instance.PauseAction = true;
            base.Enable();
        }

        public override void Disable()
        {
            GameManager.Instance.PauseAction = false;
            base.Disable();
        }
    }
}
