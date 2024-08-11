#pragma warning disable CS0649
/**************************************************
 *  CustomModeParameterContainerBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class CustomModeParameterContainerBehaviour : UIHelperBehaviour
    {
        public const float Height = 648.0f;
        public const float ViewportHeight = 500.0f;
        public const float RowHeight = 54.0f;
        public const int ParametersCount = 12;

        #region UnityEditor Private Fields

        [SerializeField]
        private UISliderPanelBehaviour minimumRoomCountPanel;

        [SerializeField]
        private UISliderPanelBehaviour muglumpCountPanel;

        [SerializeField]
        private UISliderPanelBehaviour blackMuglumpCountPanel;

        [SerializeField]
        private UISliderPanelBehaviour blueMuglumpCountPanel;

        [SerializeField]
        private UISliderPanelBehaviour silverbackMuglumpCountPanel;

        [SerializeField]
        private UISliderPanelBehaviour batCountPanel;

        [SerializeField]
        private UISliderPanelBehaviour pitCountPanel;

        [SerializeField]
        private UISliderPanelBehaviour arrowCountPanel;

        [SerializeField]
        private UISliderPanelBehaviour flashArrowCountPanel;

        [SerializeField]
        private UISliderPanelBehaviour netArrowCountPanel;

        [SerializeField]
        private UISliderPanelBehaviour coverScentCountPanel;

        [SerializeField]
        private UISliderPanelBehaviour bearTrapCountPanel;

        [SerializeField]
        private Scrollbar scrollbar;

        #endregion

        #region Controls

        public Scrollbar Scrollbar
        {
            get
            {
                if (this.scrollbar == null)
                {
                    throw new UIException($"{nameof(this.scrollbar)} needs to be set in the Unity Editor.");
                }

                return this.scrollbar;
            }
        }

        public UISliderPanelBehaviour MinimumRoomCountPanel
        {
            get
            {
                if (this.minimumRoomCountPanel == null)
                {
                    throw new UIException($"{nameof(this.minimumRoomCountPanel)} needs to be set in the Unity Editor.");
                }

                return this.minimumRoomCountPanel;
            }
        }

        public UISliderPanelBehaviour MuglumpCountPanel
        {
            get
            {
                if (this.muglumpCountPanel == null)
                {
                    throw new UIException($"{nameof(this.muglumpCountPanel)} needs to be set in the Unity Editor.");
                }

                return this.muglumpCountPanel;
            }
        }

        public UISliderPanelBehaviour BlackMuglumpCountPanel
        {
            get
            {
                if (this.blackMuglumpCountPanel == null)
                {
                    throw new UIException($"{nameof(this.blackMuglumpCountPanel)} needs to be set in the Unity Editor.");
                }

                return this.blackMuglumpCountPanel;
            }
        }
        
        public UISliderPanelBehaviour BlueMuglumpCountPanel
        {
            get
            {
                if (this.blueMuglumpCountPanel == null)
                {
                    throw new UIException($"{nameof(this.blueMuglumpCountPanel)} needs to be set in the Unity Editor.");
                }

                return this.blueMuglumpCountPanel;
            }
        }

        public UISliderPanelBehaviour SilverbackMuglumpCountPanel
        {
            get
            {
                if (this.silverbackMuglumpCountPanel == null)
                {
                    throw new UIException($"{nameof(this.silverbackMuglumpCountPanel)} needs to be set in the Unity Editor.");
                }

                return this.silverbackMuglumpCountPanel;
            }
        }
        
        public UISliderPanelBehaviour BatCountPanel
        {
            get
            {
                if (this.batCountPanel == null)
                {
                    throw new UIException($"{nameof(this.batCountPanel)} needs to be set in the Unity Editor.");
                }

                return this.batCountPanel;
            }
        }

        public UISliderPanelBehaviour PitCountPanel
        {
            get
            {
                if (this.pitCountPanel == null)
                {
                    throw new UIException($"{nameof(this.pitCountPanel)} needs to be set in the Unity Editor.");
                }

                return this.pitCountPanel;
            }
        }

        public UISliderPanelBehaviour ArrowCountPanel
        {
            get
            {
                if (this.arrowCountPanel == null)
                {
                    throw new UIException($"{nameof(this.arrowCountPanel)} needs to be set in the Unity Editor.");
                }

                return this.arrowCountPanel;
            }
        }

        public UISliderPanelBehaviour FlashArrowCountPanel
        {
            get
            {
                if (this.flashArrowCountPanel == null)
                {
                    throw new UIException($"{nameof(this.flashArrowCountPanel)} needs to be set in the Unity Editor.");
                }

                return this.flashArrowCountPanel;
            }
        }

        public UISliderPanelBehaviour NetArrowCountPanel
        {
            get
            {
                if (this.netArrowCountPanel == null)
                {
                    throw new UIException($"{nameof(this.netArrowCountPanel)} needs to be set in the Unity Editor.");
                }

                return this.netArrowCountPanel;
            }
        }

        public UISliderPanelBehaviour CoverScentCountPanel
        {
            get
            {
                if (this.coverScentCountPanel == null)
                {
                    throw new UIException($"{nameof(this.coverScentCountPanel)} needs to be set in the Unity Editor.");
                }

                return this.coverScentCountPanel;
            }
        }

        public UISliderPanelBehaviour BearTrapCountPanel
        {
            get
            {
                if (this.bearTrapCountPanel == null)
                {
                    throw new UIException($"{nameof(this.bearTrapCountPanel)} needs to be set in the Unity Editor.");
                }

                return this.bearTrapCountPanel;
            }
        }

        #endregion

        public UISliderPanelBehaviour[] OccupantCountParameters { get; } = new UISliderPanelBehaviour[11];

        public int RoomCount
        {
            get => this.MinimumRoomCountPanel.Value;
        }

        public int MaximumOccupantCount
        {
            get => (int)Mathf.Floor(this.RoomCount * 0.75f);
        }

        public void Awake()
        {
            this.SetUpOnSelectScroll();
            this.SetUpDynamicMaximumOccupantCounts();
        }

        private void SetUpOnSelectScroll()
        {
            var onSelectScrollBehaviour = this.Scrollbar.GetComponent<OnSelectScrollBehaviour>();

            onSelectScrollBehaviour.StepHeight = CustomModeParameterContainerBehaviour.RowHeight;
            onSelectScrollBehaviour.ViewPortHeight = CustomModeParameterContainerBehaviour.ViewportHeight;
            onSelectScrollBehaviour.RowCount = CustomModeParameterContainerBehaviour.ParametersCount;

            this.MinimumRoomCountPanel.Scrollbar = onSelectScrollBehaviour;
            this.MuglumpCountPanel.Scrollbar = onSelectScrollBehaviour;
            this.BlackMuglumpCountPanel.Scrollbar = onSelectScrollBehaviour;
            this.BlueMuglumpCountPanel.Scrollbar = onSelectScrollBehaviour;
            this.SilverbackMuglumpCountPanel.Scrollbar = onSelectScrollBehaviour;
            this.BatCountPanel.Scrollbar = onSelectScrollBehaviour;
            this.PitCountPanel.Scrollbar = onSelectScrollBehaviour;
            this.ArrowCountPanel.Scrollbar = onSelectScrollBehaviour;
            this.FlashArrowCountPanel.Scrollbar = onSelectScrollBehaviour;
            this.NetArrowCountPanel.Scrollbar = onSelectScrollBehaviour;
            this.CoverScentCountPanel.Scrollbar = onSelectScrollBehaviour;
            this.BearTrapCountPanel.Scrollbar = onSelectScrollBehaviour;
        }

        private void SetUpDynamicMaximumOccupantCounts()
        {
            this.OccupantCountParameters[0] = MuglumpCountPanel;
            this.OccupantCountParameters[1] = BlackMuglumpCountPanel;
            this.OccupantCountParameters[2] = BlueMuglumpCountPanel;
            this.OccupantCountParameters[3] = this.SilverbackMuglumpCountPanel;
            this.OccupantCountParameters[4] = BatCountPanel;
            this.OccupantCountParameters[5] = PitCountPanel;
            this.OccupantCountParameters[6] = ArrowCountPanel;
            this.OccupantCountParameters[7] = FlashArrowCountPanel;
            this.OccupantCountParameters[8] = NetArrowCountPanel;
            this.OccupantCountParameters[9] = CoverScentCountPanel;
            this.OccupantCountParameters[10] = BearTrapCountPanel;

            this.MinimumRoomCountPanel.OnValueChanged.AddListener(this.UpdateDynamicMaximumOccupantCounts);

            foreach (var uiParameterPanel in this.OccupantCountParameters)
            {
                uiParameterPanel.OnValueChanged.AddListener(this.UpdateDynamicMaximumOccupantCounts);
                uiParameterPanel.DefaultMaxValue = this.MaximumOccupantCount;
                uiParameterPanel.GetComponent<CustomModeParameterSliderBehaviour>().Initialize();
            }
        }

        private void UpdateDynamicMaximumOccupantCounts(float value)
        {
            foreach (var uiParameterPanel in this.OccupantCountParameters)
            {
                uiParameterPanel.DefaultMaxValue = this.MaximumOccupantCount;
                uiParameterPanel.MaxValue = Mathf.Max(Mathf.Min(
                    this.MaximumOccupantCount - MathfExtension.Sum(
                                                    this.OccupantCountParameters.
                                                    Except(new List<UISliderPanelBehaviour> { uiParameterPanel }).
                                                    Select(parameter => parameter.Value).ToArray()), 
                    uiParameterPanel.DefaultMaxValue), 0);
            }
        }
    }
}
