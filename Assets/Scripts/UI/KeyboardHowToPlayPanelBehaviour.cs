#pragma warning disable CS0649
/**************************************************
 *  KeyboardPanelBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System;

    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    [RequireComponent(typeof(DisplayPanelBehaviour))]
    public class KeyboardHowToPlayPanelBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Text actionColumnTextBox;

        [SerializeField]
        private Text keyColumnTextBox;

        private DisplayPanelBehaviour displayPanelBehaviour;

        private Text ActionColumnTextBox
        {
            get
            {
                if (this.actionColumnTextBox == null)
                {
                    throw new UIException($"The parameter {nameof(this.actionColumnTextBox)} needs to be set in the Unity Editor.");
                }

                return this.actionColumnTextBox;
            }
        }

        private Text KeyColumnTextBox
        {
            get
            {
                if (this.keyColumnTextBox == null)
                {
                    throw new UIException($"The parameter {nameof(this.keyColumnTextBox)} needs to be set in the Unity Editor.");
                }

                return this.keyColumnTextBox;
            }
        }

        public DisplayPanelBehaviour DisplayPanelBehaviour
        {
            get
            {
                if (this.displayPanelBehaviour == null)
                {
                    this.displayPanelBehaviour = this.GetComponent<DisplayPanelBehaviour>();
                }

                return this.displayPanelBehaviour;
            }
        }

        public string ActionColumnText
        {
            get => this.ActionColumnTextBox.text;
            set => this.ActionColumnTextBox.text = value;
        }

        public string KeyColumnText
        {
            get => this.KeyColumnTextBox.text;
            set => this.KeyColumnTextBox.text = value;
        }

        public void Start()
        {
            this.ActionColumnText = string.Join(Environment.NewLine, StringContent.KeyboardActions);
            this.KeyColumnText = string.Join(Environment.NewLine, StringContent.KeyboardCommandKeys);
        }
    }
}
