/**************************************************
 *  DarknessBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using UnityEngine;

    /// <summary>
    /// Defines the behaviours for the Darkness object
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(SpriteRenderer))]
    public class DarknessBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Shows this instance.
        /// </summary>
        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// Hides this instance.
        /// </summary>
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// Sets the scale.
        /// </summary>
        /// <param name="xScale">The x scale.</param>
        /// <param name="yScale">The y scale.</param>
        public void SetScale(float xScale, float yScale)
        {
            this.gameObject.transform.localScale = new Vector3(xScale, yScale, 1.0f);
        }
    }
}
