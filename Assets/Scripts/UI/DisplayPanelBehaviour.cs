#pragma warning disable CS0649
/**************************************************
 *  DisplayPanelBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class DisplayPanelBehaviour : UIHelperBehaviour
    {
        private bool lockInput = false;

        public override void Enable()
        {
            base.Enable();
            this.OnEnabled?.Invoke();
        }

        public override void Disable()
        {
            base.Disable();
            this.OnDisabled?.Invoke();
        }

        public void FixedUpdate()
        {
            if (!this.lockInput && InputExtension.IsCancelPressed())
            {
                this.lockInput = true;

                StartCoroutine(
                    nameof(this.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        () => InputExtension.IsCancelPressed(),
                        () =>
                        {
                            this.lockInput = false;
                            this.Disable();
                        }));
            }
        }
    }
}
