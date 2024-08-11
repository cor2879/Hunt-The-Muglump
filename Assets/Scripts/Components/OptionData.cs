/**************************************************
 *  OptionData.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Components
{
    using UnityEngine.UI;

    /// <summary>
    /// Defines an option value that can carry additional data which may be retrieved
    /// when the option is selected.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <seealso cref="UnityEngine.UI.Dropdown.OptionData" />
    public class OptionData<TValue> : Dropdown.OptionData
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public TValue Value { get; set; }
    }
}
