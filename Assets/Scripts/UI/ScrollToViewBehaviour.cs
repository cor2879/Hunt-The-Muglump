﻿/**************************************************
 *  ScrollToViewBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Adds standard behaviour to selectable UI elements
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    /// <seealso cref="UnityEngine.EventSystems.ISelectHandler" />
    [RequireComponent(typeof(Selectable))]
    public class ScrollToViewBehaviour : MonoBehaviour, ISelectHandler
    {
        public OnSelectScrollBehaviour Scrollbar
        {
            get; set;
        }

        public int ScrollMapPosition { get; set; }

        /// <summary>
        /// Called when the element is selected.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        public void OnSelect(BaseEventData eventData)
        {
            this.Scrollbar?.ScrollToView(this.ScrollMapPosition);
        }
    }
}
