#pragma warning disable CS0649
/**************************************************
 *  DirectionalActionSubMenuBehaviour.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI.GameplayPrompts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.UI;

    using BeautifulInterface = Interface.Elements.Scripts;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.GameplayManagement;
    using OldSchoolGames.HuntTheMuglump.Scripts.UI;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class DirectionalActionSubMenuBehaviour : PanelBehaviour
    {
        #region private child references

        [SerializeField]
        private DirectionalActionGameplayPromptBehaviour directionalGameplayPrompt;

        [SerializeField]
        private DirectionPanelBehaviour upPanel;

        [SerializeField]
        private DirectionPanelBehaviour leftPanel;

        [SerializeField]
        private DirectionPanelBehaviour downPanel;

        [SerializeField]
        private DirectionPanelBehaviour rightPanel;

        [SerializeField]
        private GameplayPromptBehaviour stairsPrompt;

        private PlayerBehaviour player;

        #endregion

        #region public child accessors

        public PlayerBehaviour Player
        {
            get
            {
                if (PlayerBehaviour.Instance == null)
                {
                    return null;
                }

                if (this.player == null)
                {
                    this.player = PlayerBehaviour.Instance;
                }

                return player;
            }
        }
        public DirectionalActionGameplayPromptBehaviour DirectionalGameplayPrompt
        {
            get
            {
                return this.directionalGameplayPrompt;
            }
        }

        public DirectionPanelBehaviour UpPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.upPanel, nameof(this.upPanel));

                return this.upPanel;
            }
        }

        public DirectionPanelBehaviour LeftPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.leftPanel, nameof(this.leftPanel));

                return this.leftPanel;
            }
        }

        public DirectionPanelBehaviour RightPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.rightPanel, nameof(this.rightPanel));

                return this.rightPanel;
            }
        }

        public DirectionPanelBehaviour DownPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.downPanel, nameof(this.downPanel));

                return this.downPanel;
            }
        }

        public GameplayPromptBehaviour StairsPrompt
        {
            get => this.stairsPrompt;
        }

        #endregion

        public GameplayMenuManagerBehaviour GameplayMenuManager { get => GameplayMenuManagerBehaviour.Instance; }

        public bool Active { get; private set; }

        public void SetActive(bool active)
        {
            this.Active = active;
            this.gameObject.SetActive(active);

            if (!active)
            {
                this.SetValidDirections(new[] { false, false, false, false });
            }
        }

        private bool ShowDirectionPrompts()
        {
            return this.Active && PlayerBehaviour.Instance.HasCameraFocus;
        }

        public void SetValidDirections(bool[] directions)
        {
            if (directions == null || !(directions.Length == 4))
            {
                throw new ArgumentException($"An unexpected problem occured when setting the valid directions array: {(directions != null ? directions.ToString() : "null")}");
            }

            if (this.DirectionalGameplayPrompt != null)
            {
                this.DirectionalGameplayPrompt.ActiveDirections = directions;
            }

            this.LeftPanel.SetActive(this.ShowDirectionPrompts() && directions[0]);
            this.UpPanel.SetActive(this.ShowDirectionPrompts() && directions[1]);
            this.RightPanel.SetActive(this.ShowDirectionPrompts() && directions[2]);
            this.DownPanel.SetActive(this.ShowDirectionPrompts() && directions[3]);
        }

        private void Start()
        {
            this.SetValidDirections(new[] { false, false, false, false });
        }

        protected override void Update()
        {
            if (this.Player != null && this.Player.CurrentRoom != null && this.Player.IsIdle && this.Player.CurrentRoom.IsEntrance)
            {
                this.StairsPrompt?.Show();
            }
            else
            {
                this.StairsPrompt?.Hide();
            }

            base.Update();
        }

        public IEnumerator WaitForDurationThenDoAction(WaitDuration waitDuration)
        {
            while (waitDuration.Duration >= float.Epsilon)
            {
                waitDuration.Duration -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            waitDuration.DoAction.Invoke();
        }

        /// <summary>
        /// Waits for predicate to be false then does the action.
        /// </summary>
        /// <param name="waitAction">The wait action.</param>
        /// <returns></returns>
        public IEnumerator WaitForPredicateToBeFalseThenDoAction(WaitAction waitAction)
        {
            while (waitAction.Predicate())
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            waitAction.DoAction.Invoke();
        }

        public void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(DirectionalActionSubMenuBehaviour));
        }
    }
}
