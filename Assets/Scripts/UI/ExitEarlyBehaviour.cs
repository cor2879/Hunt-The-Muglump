﻿#pragma warning disable CS0649
/**************************************************
 *  ExitEarlyBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System.Collections;

    using UnityEngine;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class ExitEarlyBehaviour
        : UIHelperBehaviour
    {
        public bool Enabled { get; private set; }

        public void Update()
        {
            if (this.Enabled && (InputExtension.IsAnyKeyPressed() || InputExtension.IsAnyButtonPressed() || InputExtension.IsAnyTouchActive()))
            {
                StartCoroutine($"{nameof(this.WaitForPredicateToBeFalseThenDoAction)}",
                    new WaitAction(
                        () => InputExtension.IsAnyKeyPressed(),
                        () => this.Exit()
                        ));
            }
        }

        public override void Enable()
        {
            this.Enabled = true;
            base.Enable();
        }

        public override void Disable()
        {
            this.Enabled = false;
            base.Disable();
        }

        protected virtual void Exit()
        {
            this.Disable();
        }
    }
}
