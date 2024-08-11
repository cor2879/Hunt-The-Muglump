#pragma warning disable CS0649
/**************************************************
 *  TextContainerBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Defines the behaviours for a custom UI Element that contains a Text
    /// element that needs to be modified or retrieved at runtime.
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.UIHelperBehaviour" />
    public class TextContainerBehaviour : UIHelperBehaviour
    {
        /// <summary>
        /// The value text box
        /// </summary>
        [SerializeField]
        private Text valueTextBox;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get => this.valueTextBox.text; set => this.valueTextBox.text = value; }
    }
}
