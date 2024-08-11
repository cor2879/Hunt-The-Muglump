#pragma warning disable CS0649
/**************************************************
 *  ArrowCountTextManagerBehaviour.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.GameplayManagement.ChildReferenceManagers
{
    using System.Collections;

    using UnityEngine;

    using BeautifulInterface = Interface.Elements.Scripts;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.GameplayManagement;
    using OldSchoolGames.HuntTheMuglump.Scripts.UI;

    public class ArrowCountTextManagerBehaviour : MonoBehaviour
    {
        private static ArrowCountTextManagerBehaviour instance;

        #region private child references

        [SerializeField]
        private BeautifulInterface.TextUI[] arrowCountTexts;

        [SerializeField]
        private BeautifulInterface.TextUI[] flashArrowCountTexts;

        [SerializeField]
        private BeautifulInterface.TextUI[] netArrowCountTexts;

        #endregion

        #region public child accessors

        public BeautifulInterface.TextUI[] ArrowCountText
        {
            get
            {
                return this.arrowCountTexts;
            }
        }

        public BeautifulInterface.TextUI[] FlashArrowCountText
        {
            get
            {
                return this.flashArrowCountTexts;
            }
        }

        public BeautifulInterface.TextUI[] NetArrowCountText
        {
            get
            {
                return this.netArrowCountTexts;
            }
        }

        #endregion

        public static ArrowCountTextManagerBehaviour Instance { get => instance; }

        public IEnumerator WaitForDurationThenDoAction(WaitDuration waitDuration)
        {
            while (waitDuration.Duration >= float.Epsilon)
            {
                waitDuration.Duration -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            waitDuration.DoAction.Invoke();
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
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

        private void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(GameplayMenuManagerBehaviour));
        }
    }
}
