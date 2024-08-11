#pragma warning disable CS0649
/**************************************************
 *  DataFieldBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System.Collections;
    using System.Reflection;

    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class DataFieldBehaviour
        : UIHelperBehaviour
    {
        [SerializeField]
        private Text textBox;

        public DataRowBehaviour Row { get; set; }

        public Text Textbox
        {
            get
            {
                if (this.textBox == null)
                {
                    throw new UIException($"The parameter {nameof(this.textBox)} needs to be set in the Unity Editor.");
                }

                return this.textBox;
            }
        }
    }
}
