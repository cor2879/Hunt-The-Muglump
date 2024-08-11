#pragma warning disable CS0649, CS0414
/**************************************************
 *  ButtonsPanelBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    [RequireComponent(typeof(Button))]
    public class TouchButton : MonoBehaviour
    {
        private bool lockInput;

        [SerializeField, ReadOnly]
        private Button button;

        private Button Button
        {
            get
            {
                if (this.button == null)
                {
                    this.button = this.GetComponent<Button>();
                }

                return this.button;
            }
        }

        public bool IsTouched
        {
            get
            {
                return false;
                // return InputExtension.IsTouchDetected() && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
            }
        } 

        private void Update()
        {
            if (this.IsTouched)
            {
                this.lockInput = true;

                this.StartCoroutine(
                    nameof(this.WaitForPredicateToBeFalseThenDoAction), 
                    new WaitAction(
                        () => this.IsTouched,
                        () =>
                        {
                            this.Click();
                            this.lockInput = false;
                        }));
            }
        }

        public void Click()
        {
            ExecuteEvents.Execute(this.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
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
    }
}
