#pragma warning disable CS0649
/**************************************************
 *  TutorialManagerBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class TutorialManagerBehaviour : UIHelperBehaviour
    {
        private Dictionary<string, TutorialPopupBehaviour> Popups { get; } = new Dictionary<string, TutorialPopupBehaviour>();

        private Queue<TutorialPopupBehaviour> PopupQueue { get; } = new Queue<TutorialPopupBehaviour>();

        private TutorialPopupBehaviour CurrentPopup { get; set; }

        private void AddPopup(TutorialPopupBehaviour popup)
        {
            this.Popups.Add(popup.Name, popup);
        }

        private void AddPopupToQueue(TutorialPopupBehaviour popup)
        {
            this.PopupQueue.Enqueue(popup);
            this.ShowNextPopup();
        }

        private void ShowNextPopup()
        {
            if (!this.PopupQueue.Any())
            {
                return;
            }

            this.CurrentPopup = this.PopupQueue.Dequeue();
            this.CurrentPopup.Enable();
        }

        public void Start()
        {
            // TODO: Add popups

            foreach (var popup in this.Popups.Values)
            {
                popup.Disable();
            }
        }

        public void FixedUpdate()
        {
            if (this.CurrentPopup != null)
            {
                if (InputExtension.IsSubmitPressed())
                {
                    StartCoroutine(
                        nameof(this.WaitForPredicateToBeFalseThenDoAction),
                        new WaitAction(
                            () => InputExtension.IsSubmitPressed(),
                            this.HidePopup));
                }
            }
        }

        public void ShowPopup(string name)
        {
            if (!this.Popups.ContainsKey(name))
            {
                throw new UIException($"'{name}' is an invalid popup name.");
            }

            this.AddPopupToQueue(this.Popups[name]);
        }

        public void HidePopup()
        {
            this.CurrentPopup?.Disable();
            this.ShowNextPopup();
        }
    }
}
