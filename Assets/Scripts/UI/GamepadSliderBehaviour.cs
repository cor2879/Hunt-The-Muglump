#pragma warning disable CS0649
/**************************************************
 *  GamepadSliderBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System;
    using System.Collections;

    using UnityEngine;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    [RequireComponent(typeof(Slider))]
    [RequireComponent(typeof(SelectableStateBehaviour))]
    public class GamepadSliderBehaviour : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private Slider slider;

        private static bool lockInput = false;

        public Slider Slider
        {
            get
            {
                if (this.slider == null)
                {
                    this.slider = this.GetComponent<Slider>();
                }

                return this.slider;
            }
        }

        public float PageSize
        {
            get => this.Slider.maxValue * 0.1f;
        }

        public SelectableStateBehaviour SelectableState
        {
            get => this.Slider.GetComponent<SelectableStateBehaviour>();
        }

        public void FixedUpdate()
        {
            if (this.SelectableState.IsSelected)
            {
                var scrollDelta = InputExtension.GetHorizontalScrollDelta();

                if (scrollDelta > float.Epsilon && this.Slider.value < this.Slider.maxValue)
                {
                    this.Slider.value = Mathf.Min(this.Slider.value + scrollDelta, this.Slider.maxValue);
                }
                else if (scrollDelta < -float.Epsilon && this.Slider.value > this.Slider.minValue)
                {
                    this.Slider.value = Mathf.Max(this.Slider.value + scrollDelta, this.Slider.minValue);
                }

                if (!lockInput && InputExtension.IsPageForwardPressed())
                {
                    lockInput = true;

                    StartCoroutine(
                        nameof(this.WaitForPredicateToBeFalseThenDoAction),
                        new WaitAction(
                            () => InputExtension.IsPageForwardPressed(),
                            () =>
                            {
                                lockInput = false;
                                this.PageForward();
                            }));
                }

                if (!lockInput && InputExtension.IsPageBackPressed())
                {
                    lockInput = true;

                    StartCoroutine(
                        nameof(this.WaitForPredicateToBeFalseThenDoAction),
                        new WaitAction(
                            () => InputExtension.IsPageBackPressed(),
                            () =>
                            {
                                lockInput = false;
                                this.PageBack();
                            }));
                }
            }
        }

        public void PageForward()
        {
            this.Slider.value += this.PageSize;
        }

        public void PageBack()
        {
            this.Slider.value -= this.PageSize;
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
