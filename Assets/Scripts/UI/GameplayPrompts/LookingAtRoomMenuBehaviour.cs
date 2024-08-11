#pragma warning disable CS0649
/**************************************************
 *  LookingAtRoomMenuBehaviour.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI.GameplayPrompts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.UI;

    using BeautifulInterface = Interface.Elements.Scripts;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.GameplayManagement;
    using OldSchoolGames.HuntTheMuglump.Scripts.UI;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    [RequireComponent(typeof(CanvasGroup))]
    public class LookingAtRoomMenuBehaviour : PanelBehaviour
    {
        #region private child references

        [SerializeField]
        private GameplayPromptBehaviour backPrompt;

        #endregion

        #region public child accessors

        public GameplayPromptBehaviour BackPrompt
        {
            get
            {
                this.ValidateUnityEditorParameter(this.backPrompt, nameof(this.backPrompt));

                return this.backPrompt;
            }
        }

        #endregion

        public bool Active { get; private set; }

        public void SetActive(bool activeState)
        {
            this.gameObject.SetActive(activeState);
            this.Active = activeState;
        }

        private void Start()
        {
            //this.BackPrompt.Button.onClick.AddListener(this.OnBackButtonClicked);
        }

        private void OnBackButtonClicked()
        {
            GameplayMenuManagerBehaviour.Instance.MenuState.GoBack();
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
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(LookingAtRoomMenuBehaviour));
        }
    }
}
