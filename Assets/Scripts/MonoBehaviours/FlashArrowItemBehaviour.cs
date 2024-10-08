﻿#pragma warning disable CS0649
/**************************************************
 *  FlashArrowItemBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using System;
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines behaviour for the flash arrow item
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.ArrowItemBehaviour" />
    public class FlashArrowItemBehaviour : ArrowItemBehaviour
    {
        [SerializeField]
        private FlameBehaviour flameBehaviour;

        /// <summary>
        /// Gets the type of the arrow.
        /// </summary>
        /// <value>
        /// The type of the arrow.
        /// </value>
        public override ArrowType ArrowType
        {
            get => ArrowType.FlashArrow;
        }

        public void Start()
        {
            this.flameBehaviour.Ignite(false);
        }
    }
}
