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

    [RequireComponent(typeof(GameplayPromptBehaviour))]
    public class DirectionalActionGameplayPromptBehaviour : PanelBehaviour
    {
        #region private child references

        [SerializeField]
        private Image leftImage;

        [SerializeField]
        private Image upImage;

        [SerializeField]
        private Image rightImage;

        [SerializeField]
        private Image downImage;

        [SerializeField]
        private BeautifulInterface.ButtonUI button;

        #endregion

        #region public child accessors

        public Image LeftImage
        {
            get
            {
                this.ValidateUnityEditorParameter(this.leftImage, nameof(this.leftImage));

                return this.leftImage;
            }
        }

        public Image UpImage
        {
            get
            {
                this.ValidateUnityEditorParameter(this.upImage, nameof(this.upImage));

                return this.upImage;
            }
        }

        public Image RightImage
        {
            get
            {
                this.ValidateUnityEditorParameter(this.rightImage, nameof(this.rightImage));

                return this.rightImage;
            }
        }

        public Image DownImage
        {
            get
            {
                this.ValidateUnityEditorParameter(this.downImage, nameof(this.downImage));

                return this.downImage;
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

        [SerializeField, ReadOnly]
        private bool[] activeDirections = new bool[] { false, false, false, false };

        public bool[] ActiveDirections { get => this.activeDirections; set => this.activeDirections = value; }

        protected override void Update()
        {
            this.LeftImage.gameObject.SetActive(activeDirections[0]);
            this.UpImage.gameObject.SetActive(activeDirections[1]);
            this.RightImage.gameObject.SetActive(activeDirections[2]);
            this.DownImage.gameObject.SetActive(activeDirections[3]);

            base.Update();
        }

        public IEnumerator WaitForDurationThenDoAction(WaitDuration waitDuration)
        {
            while (waitDuration.Duration >= float.Epsilon)
            {
                waitDuration.Duration -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            waitDuration.DoAction.Invoke();
        }

        /// <summary>
        /// Waits for predicate to be false then does the action.
        /// </summary>
        /// <param name="waitAction">The wait action.</param>
        /// <returns></returns>
        public IEnumerator WaitForPredicateToBeFalseThenDoAction(WaitAction waitAction)
        {
            while (waitAction.Predicate())
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            waitAction.DoAction.Invoke();
        }

        public void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(DirectionalActionGameplayPromptBehaviour));
        }
    }
}
