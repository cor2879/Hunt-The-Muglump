#pragma warning disable CS0649
/**************************************************
 *  TextScrollingBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Defines the behaviours for UI Text elements that are intended to interact with
    /// a Scrollbar or ScrollView
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(Text))]
    [RequireComponent(typeof(LayoutElement))]
    [RequireComponent(typeof(RectTransform))]
    public class TextScrollingHelperBehaviour : MonoBehaviour
    {
        /// <summary>
        /// The vertical scroll bar
        /// </summary>
        [SerializeField]
        private Scrollbar verticalScrollBar;

        /// <summary>
        /// The text control
        /// </summary>
        private Text textControl;

        /// <summary>
        /// The layout element
        /// </summary>
        private LayoutElement layoutElement;

        /// <summary>
        /// The rect transform
        /// </summary>
        private RectTransform rectTransform;

        /// <summary>
        /// Gets the text control.
        /// </summary>
        /// <value>
        /// The text control.
        /// </value>
        public Text TextControl
        {
            get
            {
                if (this.textControl == null)
                {
                    this.textControl = this.GetComponent<Text>();
                }

                return this.textControl;
            }
        }

        /// <summary>
        /// Gets the rect transform.
        /// </summary>
        /// <value>
        /// The rect transform.
        /// </value>
        public RectTransform RectTransform
        {
            get
            {
                if (this.rectTransform == null)
                {
                    this.rectTransform = this.GetComponent<RectTransform>();
                }

                return this.rectTransform;
            }
        }

        /// <summary>
        /// Gets the layout element.
        /// </summary>
        /// <value>
        /// The layout element.
        /// </value>
        public LayoutElement LayoutElement
        {
            get
            {
                if (this.layoutElement == null)
                {
                    this.layoutElement = this.GetComponent<LayoutElement>();
                }

                return this.layoutElement;
            }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text
        {
            get
            {
                return this.TextControl.text;
            }

            set
            {
                this.TextControl.text = value;

                if (string.IsNullOrEmpty(value))
                {
                    this.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.LayoutElement.minHeight);
                    this.verticalScrollBar.size = 1.0f;
                }
            }
        }
    }
}
