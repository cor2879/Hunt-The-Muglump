#pragma warning disable CS0649
/**************************************************
 *  ScoreRecordDataRowBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Interfaces;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    [RequireComponent(typeof(DataRowBehaviour))]
    public class ScoreRecordDataRowBehaviour
        : UIHelperBehaviour, IDataBoundRow<ScoreRecord>
    {
        [SerializeField]
        private DataFieldBehaviour playerNameField;

        [SerializeField]
        private DataFieldBehaviour scoreField;

        [SerializeField]
        private DataFieldBehaviour timeStampField;

        private DataRowBehaviour dataRow;

        private ScoreRecordDataTableBehaviour scoreRecordDataTable;

        private Selectable selectable;

        private SelectableStateBehaviour selectableState;

        public DataRowBehaviour DataRow
        {
            get
            {
                if (this.dataRow == null)
                {
                    this.dataRow = this.GetComponent<DataRowBehaviour>();
                }

                return this.dataRow;
            }
        }

        public ScoreRecordDataTableBehaviour ScoreRecordDataTable
        {
            get
            {
                if (this.scoreRecordDataTable == null)
                {
                    this.scoreRecordDataTable = this.DataRow.DataTable.GetComponent<ScoreRecordDataTableBehaviour>();
                }

                return this.scoreRecordDataTable;
            }
        }

        public ScoreRecord ScoreRecord { get; private set; }

        public DataFieldBehaviour PlayerNameField
        {
            get
            {
                if (this.playerNameField == null)
                {
                    throw new UIException($"The parameter {nameof(this.playerNameField)} needs to be set for the DataRow Prefab in the Unity Editor.");
                }

                return this.playerNameField;
            }
        }

        public DataFieldBehaviour ScoreField
        {
            get
            {
                if (this.scoreField == null)
                {
                    throw new UIException($"The parameter {nameof(this.scoreField)} needs to be set for the DataRow Prefab in the Unity Editor.");
                }

                return this.scoreField;
            }
        }

        public DataFieldBehaviour TimeStampField
        {
            get
            {
                if (this.timeStampField == null)
                {
                    throw new UIException($"The parameter {nameof(this.timeStampField)} needs to be set for the DataRow PRefab in the Unity Editor.");
                }

                return this.timeStampField;
            }
        }

        public Selectable Selectable
        {
            get
            {
                if (this.selectable == null)
                {
                    this.selectable = this.GetComponent<Selectable>();
                }

                return this.selectable;
            }
        }

        public SelectableStateBehaviour SelectableState
        {
            get
            {
                if (this.IsSelectable && this.selectableState == null)
                {
                    this.selectableState = this.GetComponent<SelectableStateBehaviour>();
                }

                return this.selectableState;
            }
        }

        public bool IsSelectable { get => this.Selectable != null; }

        public bool IsSelected { get => this.SelectableState != null ? this.SelectableState.IsSelected : false; }

        public void DataBind(ScoreRecord item)
        {
            this.PlayerNameField.Textbox.text = item.Player.Name;
            this.ScoreField.Textbox.text = item.Score.ToString();
            this.TimeStampField.Textbox.text = item.TimeStamp.ToString("g");
            this.ScoreRecord = item;
        }

        public void Awake()
        {
            if (!this.DataRow.Fields.Any())
            {
                this.DataRow.Fields.Add(this.PlayerNameField);
                this.DataRow.Fields.Add(this.ScoreField);
                this.DataRow.Fields.Add(this.TimeStampField);
            }

            if (this.SelectableState != null)
            {
                this.SelectableState.Selected += this.OnSelect;
                this.SelectableState.Deselected += this.OnDeselect;
            }
        }

        public void Start()
        {

        }

        public void Update()
        {
            if (this.IsSelected && (InputExtension.IsSubmitPressed() || Input.GetMouseButtonDown(InputConfiguration.LeftMouseButton)))
            {
                StartCoroutine(nameof(this.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        () => InputExtension.IsSubmitPressed() || Input.GetMouseButtonDown(InputConfiguration.LeftMouseButton),
                        () =>
                        {
                            //if (!this.IsSelected)
                            //{
                            //    return;
                            //}

                            this.ScoreRecordDataTable.GameOverPanel.GameOver(this.ScoreRecord.GameOverSettings);
                            // TitleScreenBehaviour.Instance.ScoreRecordHistoryPanel.Disable();
                            this.ScoreRecordDataTable.ButtonsPanel.DefaultButton = this.SelectableState;
                        }));

                this.SelectableState.ButtonsPanel.Deactivate();

            }
        }

        private void OnSelect()
        {
            foreach (var field in this.DataRow.Fields)
            {
                field.Textbox.color = this.Selectable.colors.selectedColor;
            }
        }

        private void OnDeselect()
        {
            foreach (var field in this.DataRow.Fields)
            {
                field.Textbox.color = this.Selectable.colors.normalColor;
            }
        }
    }
}
