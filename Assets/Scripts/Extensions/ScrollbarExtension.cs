/**************************************************
 *  scrollbarExtension.cs
 *  
 *  copyright (c) 2021 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Extensions
{
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public static class ScrollbarExtension
    {
        /// <summary>
        /// Scrolls to the bottom.
        /// </summary>
        public static void ScrollToBottom(this Scrollbar scrollbar)
        {
            Validator.ArgumentIsNotNull(scrollbar, nameof(scrollbar));

            scrollbar.value = 0;
        }

        public static  void ScrollToTop(this Scrollbar scrollbar)
        {
            Validator.ArgumentIsNotNull(scrollbar, nameof(scrollbar));

            scrollbar.value = 1.0f;
        }
    }
}
