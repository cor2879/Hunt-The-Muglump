#pragma warning disable CS0649
/**************************************************
 *  CustomModePanelBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System.Collections;

    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class CreditsWindowBehaviour : UIHelperBehaviour
    {
        private bool lockInput;

        public void FixedUpdate()
        {
            if (!this.lockInput && (InputExtension.IsCancelPressed() || InputExtension.IsMenuPressed()))
            {
                this.lockInput = true;

                StartCoroutine(
                    nameof(base.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        () => InputExtension.IsCancelPressed() || InputExtension.IsMenuPressed(),
                        () =>
                        {
                            this.lockInput = false;
                            this.Disable();
                        }));
            }
        }

        public override void Disable()
        {
            TitleScreenBehaviour.Instance.Enable();
            base.Disable();
        }
    }
}
