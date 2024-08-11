#pragma warning disable CS0649
/**************************************************
 *  ItemCountTextManagerBehaviour.cs
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

    public class ItemCountTextManagerBehaviour : MonoBehaviour
    {
        private static ItemCountTextManagerBehaviour instance;

        #region private child references

        [SerializeField]
        private BeautifulInterface.TextUI[] eauDuMuglumpCountTexts;

        [SerializeField]
        private BeautifulInterface.TextUI[] bearTrapCountTexts;

        #endregion

        #region public child accessors

        public BeautifulInterface.TextUI[] EauDuMuglumpCountText
        {
            get
            {
                return this.eauDuMuglumpCountTexts;
            }
        }

        public BeautifulInterface.TextUI[] BearTrapCountText
        {
            get
            {
                return this.bearTrapCountTexts;
            }
        }

        #endregion

        public static ItemCountTextManagerBehaviour Instance { get => instance; }

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

        private void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(GameplayMenuManagerBehaviour));
        }
    }
}
