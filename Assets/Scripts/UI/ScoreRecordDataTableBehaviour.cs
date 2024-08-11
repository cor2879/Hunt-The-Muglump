#pragma warning disable CS0649
/**************************************************
 *  ScoreRecordDataTableBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
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

    [RequireComponent(typeof(DataTableBehaviour))]
    [RequireComponent(typeof(ButtonsPanelBehaviour))]
    public class ScoreRecordDataTableBehaviour
        : UIHelperBehaviour, IDataBoundTable<ScoreRecord>
    {
        private const float RowHeight = 60.0f;

        private const float ViewPortHeight = 475.0f;

        [SerializeField]
        private GameOverScreenBehaviour gameOverPanel;

        [SerializeField]
        private Scrollbar scrollbar;

        private DataTableBehaviour dataTable;

        private ButtonsPanelBehaviour buttonsPanel;

        public Scrollbar Scrollbar
        {
            get
            {
                this.ValidateUnityEditorParameter(this.scrollbar, nameof(this.scrollbar));

                return this.scrollbar;
            }
        }

        public DataTableBehaviour DataTable
        {
            get
            {
                if (this.dataTable == null)
                {
                    this.dataTable = this.GetComponent<DataTableBehaviour>();
                }

                return this.dataTable;
            }
        }

        public ButtonsPanelBehaviour ButtonsPanel
        {
            get
            {
                if (this.buttonsPanel == null)
                {
                    this.buttonsPanel = this.GetComponent<ButtonsPanelBehaviour>();
                }

                return this.buttonsPanel;
            }
        }

        public GameOverScreenBehaviour GameOverPanel
        {
            get
            {
                if (this.gameOverPanel == null)
                {
                    throw new UIException($"The {nameof(this.gameOverPanel)} parameter needs to be set in the Unity Editor for the {nameof(ScoreRecordDataTableBehaviour)}.");
                }

                return this.gameOverPanel;
            }
        }

        public void DataBind(IEnumerable<ScoreRecord> collection)
        {
            Validator.ArgumentIsNotNull(collection, nameof(collection));

            if (!collection.Any())
            {
                return;
            }

            var indexer = 0;

            foreach (var scoreRecord in collection.Take(1))
            {
                this.AddScoreRecordRow(scoreRecord, indexer);
                indexer++;
            }

            foreach (var scoreRecord in collection.Skip(1))
            {
                this.AddScoreRecordRow(scoreRecord, indexer);
                indexer++;
            }

            if (indexer > 1)
            {
                this.DataTable.Rows[indexer].GetComponent<Selectable>().navigation = new Navigation()
                {
                    mode = Navigation.Mode.Explicit,
                    selectOnUp = this.DataTable.Rows[indexer - 1].GetComponent<Selectable>(),
                    selectOnDown = TitleScreenBehaviour.Instance.ScoreRecordHistoryPanel.BackButton
                };

                TitleScreenBehaviour.Instance.ScoreRecordHistoryPanel.BackButton.navigation = new Navigation()
                {
                    mode = Navigation.Mode.Explicit,
                    selectOnUp = this.DataTable.Rows[indexer].GetComponent<Selectable>()
                };
            }

            for (--indexer; indexer > 1; indexer--)
            {
                BuildNavigation(
                    this.DataTable.Rows[indexer + 1].GetComponent<Selectable>(),
                    this.DataTable.Rows[indexer].GetComponent<Selectable>(), 
                    this.DataTable.Rows[indexer - 1].GetComponent<Selectable>());
            }

            if (indexer > 0)
            {
                this.DataTable.Rows[indexer].GetComponent<Selectable>().navigation = new Navigation()
                {
                    mode = Navigation.Mode.Explicit,
                    selectOnDown = this.DataTable.Rows[indexer + 1].GetComponent<Selectable>()
                };
            }

            this.ButtonsPanel.DefaultButton = this.DataTable.Rows[1].GetComponent<SelectableStateBehaviour>();
            this.DataTable.Rows[1].GetComponent<SelectableStateBehaviour>().Select();
            this.SetUpOnSelectScroll();
        }

        private void AddScoreRecordRow(ScoreRecord scoreRecord, int scrollMapPosition)
        {
            var row = Instantiate(this.DataTable.DataRowPrefab, this.DataTable.transform);
            row.DataTable = this.DataTable;
            row.transform.localPosition = new Vector3(this.DataTable.Rows.First().XPosition, this.DataTable.Rows.Last().YPosition - this.DataTable.Rows.First().Height, 0.0f);

            row.GetBindingInterface<ScoreRecord>().DataBind(scoreRecord);
            this.DataTable.Rows.Add(row);
            this.ButtonsPanel.Buttons.Add(row.GetComponent<SelectableStateBehaviour>());
            row.GetComponent<SelectableStateBehaviour>().ButtonsPanel = this.ButtonsPanel;
            row.GetComponent<ScrollToViewBehaviour>().ScrollMapPosition = scrollMapPosition;
            row.GetComponent<ScrollToViewBehaviour>().Scrollbar = this.Scrollbar.GetComponent<OnSelectScrollBehaviour>();
        }

        private void BuildNavigation(Selectable bottom, Selectable middle, Selectable top)
        {
            if (bottom == null || top == null)
            {
                return;
            }

            middle.navigation = new Navigation()
            {
                mode = Navigation.Mode.Explicit,
                selectOnUp = top,
                selectOnDown = bottom
            };
        }

        public void Awake()
        {
            this.OnEnabled.AddListener(this.Enabled);
        }

        private void Enabled()
        {
            if (TitleScreenBehaviour.Instance != null && (!this.DataTable.Rows.Any() || this.DataTable.Rows.Count == 1))
            {
                this.DataBind(Settings.ScoreRecordHistory);
                this.ButtonsPanel.Buttons.Add(TitleScreenBehaviour.Instance.ScoreRecordHistoryPanel.BackButton.GetComponent<SelectableStateBehaviour>());
            }
        }

        private void SetUpOnSelectScroll()
        {
            var onSelectScrollBehaviour = this.Scrollbar.GetComponent<OnSelectScrollBehaviour>();

            onSelectScrollBehaviour.StepHeight = ScoreRecordDataTableBehaviour.RowHeight;
            onSelectScrollBehaviour.ViewPortHeight = ScoreRecordDataTableBehaviour.ViewPortHeight;
            onSelectScrollBehaviour.RowCount = this.DataTable.Rows.Count;

            this.Scrollbar.value = 1.0f;
        }

        private void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(ScoreRecordDataTableBehaviour));
        }
    }
}
