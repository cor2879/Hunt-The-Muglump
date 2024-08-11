#pragma warning disable CS0649
/**************************************************
 *  MinimapBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.InputSystem.EnhancedTouch;
    using UnityEngine.UI;


    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

    /// <summary>
    /// Defines the behaviours for the Minimap
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.UIWindowBehaviour" />
    public class MinimapBehaviour : UIWindowBehaviour
    {
        private float lastMultitouchDistance = 0.0f;

        /// <summary>
        /// The minimap z position
        /// </summary>
        private const float MinimapZPosition = -10.0f;

        /// <summary>
        /// The field of view maximum
        /// </summary>
        private const float FieldOfViewMax = 165.0f;

        /// <summary>
        /// The field of view minimum
        /// </summary>
        private const float FieldOfViewMin = 75.0f;

        /// <summary>
        /// The field of view default
        /// </summary>
        private const float FieldOfViewDefault = 150.0f;

        /// <summary>
        /// The left mouse button down
        /// </summary>
        private bool leftMouseButtonDown = false;

        /// <summary>
        /// The mouse over
        /// </summary>
        [SerializeField, ReadOnly]
        private bool mouseOver = false;

        /// <summary>
        /// The lock input
        /// </summary>
        [SerializeField, ReadOnly]
        private bool lockInput = false;

        /// <summary>
        /// The camera movement speed
        /// </summary>
        [SerializeField]
        private float cameraMovementSpeed;

        /// <summary>
        /// The minimap camera
        /// </summary>
        [SerializeField]
        private Camera minimapCamera;

        [SerializeField]
        private MonoBehaviour minimapBackground;

        /// <summary>
        /// The zoom speed
        /// </summary>
        [SerializeField]
        private float zoomSpeed = 2.5f;

        /// <summary>
        /// The map scroll multiplier
        /// </summary>
        [SerializeField, ReadOnly]
        private float mapScrollMultiplier = -0.5f;

        /// <summary>
        /// The dungeon
        /// </summary>
        [SerializeField]
        private DungeonBehaviour dungeon;

        [SerializeField]
        private RawImage rawImage;

        /// <summary>
        /// The muglump panel
        /// </summary>
        [SerializeField]
        private TextContainerBehaviour redMuglumpPanel;

        [SerializeField]
        private TextContainerBehaviour blackMuglumpPanel;

        [SerializeField]
        private TextContainerBehaviour blueMuglumpPanel;

        [SerializeField]
        private TextContainerBehaviour goldMuglumpPanel;

        [SerializeField]
        private TextContainerBehaviour silverbackMuglumpPanel;

        /// <summary>
        /// The pit panel
        /// </summary>
        [SerializeField]
        private TextContainerBehaviour pitPanel;

        /// <summary>
        /// The bat panel
        /// </summary>
        [SerializeField]
        private TextContainerBehaviour batPanel;

        /// <summary>
        /// The arrow panel
        /// </summary>
        [SerializeField]
        private TextContainerBehaviour arrowPanel;

        [SerializeField]
        private TextContainerBehaviour flashArrowPanel;

        [SerializeField]
        private TextContainerBehaviour netArrowPanel;

        [SerializeField]
        private TextContainerBehaviour muglumpOilPanel;

        [SerializeField]
        private TextContainerBehaviour bearTrapPanel;

        [SerializeField]
        private TextContainerBehaviour crownPanel;

        /// <summary>
        /// The close button
        /// </summary>
        [SerializeField]
        private Button closeButton;

        /// <summary>
        /// Gets the dungeon.
        /// </summary>
        /// <value>
        /// The dungeon.
        /// </value>
        public DungeonBehaviour Dungeon
        {
            get => this.dungeon;
        }

        public RectTransform MinimapBackground
        {
            get
            {
                this.ValidateUnityEditorParameter(this.minimapBackground, nameof(this.minimapBackground));

                return this.minimapBackground.GetComponent<RectTransform>();
            }
        }

        /// <summary>
        /// Gets the muglump panel.
        /// </summary>
        /// <value>
        /// The muglump panel.
        /// </value>
        public TextContainerBehaviour RedMuglumpPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.redMuglumpPanel, nameof(this.redMuglumpPanel));

                return this.redMuglumpPanel;
            }
        }

        public TextContainerBehaviour BlackMuglumpPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.blackMuglumpPanel, nameof(this.blackMuglumpPanel));

                return this.blackMuglumpPanel;
            }
        }

        public TextContainerBehaviour BlueMuglumpPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.blueMuglumpPanel, nameof(this.blueMuglumpPanel));

                return this.blueMuglumpPanel;
            }
        }

        public TextContainerBehaviour GoldMuglumpPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.goldMuglumpPanel, nameof(this.goldMuglumpPanel));

                return this.goldMuglumpPanel;
            }
        }

        public TextContainerBehaviour SilverbackMuglumpPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.silverbackMuglumpPanel, nameof(this.silverbackMuglumpPanel));

                return this.silverbackMuglumpPanel;
            }
        }

        /// <summary>
        /// Gets the pit panel.
        /// </summary>
        /// <value>
        /// The pit panel.
        /// </value>
        public TextContainerBehaviour PitPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.pitPanel, nameof(this.pitPanel));

                return this.pitPanel;
            }
        }

        /// <summary>
        /// Gets the bat panel.
        /// </summary>
        /// <value>
        /// The bat panel.
        /// </value>
        public TextContainerBehaviour BatPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.batPanel, nameof(this.batPanel));

                return this.batPanel;
            }
        }

        /// <summary>
        /// Gets the arrow panel.
        /// </summary>
        /// <value>
        /// The arrow panel.
        /// </value>
        public TextContainerBehaviour ArrowPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.arrowPanel, nameof(this.arrowPanel));

                return this.arrowPanel;
            }
        }

        public TextContainerBehaviour NetArrowPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.netArrowPanel, nameof(this.netArrowPanel));

                return this.netArrowPanel;
            }
        }

        public TextContainerBehaviour FlashArrowPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.flashArrowPanel, nameof(this.flashArrowPanel));

                return this.flashArrowPanel;
            }
        }

        public TextContainerBehaviour MuglumpOilPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.muglumpOilPanel, nameof(this.muglumpOilPanel));

                return this.muglumpOilPanel;
            }
        }

        public TextContainerBehaviour BearTrapPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.bearTrapPanel, nameof(this.bearTrapPanel));

                return this.bearTrapPanel;
            }
        }

        public TextContainerBehaviour CrownPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.crownPanel, nameof(this.crownPanel));

                return this.crownPanel;
            }
        }

        public RawImage MinimapRawImage
        {
            get
            {
                this.ValidateUnityEditorParameter(this.rawImage, nameof(this.rawImage));

                return this.rawImage;
            }
        }

        /// <summary>
        /// Gets the close button.
        /// </summary>
        /// <value>
        /// The close button.
        /// </value>
        public Button CloseButton
        {
            get => this.closeButton;
        }

        /// <summary>
        /// Gets the map collider.
        /// </summary>
        /// <value>
        /// The map collider.
        /// </value>
        public BoxCollider2D MapCollider
        {
            get => this.GetComponentInChildren<BoxCollider2D>();
        }

        /// <summary>
        /// Gets the minimap camera.
        /// </summary>
        /// <value>
        /// The minimap camera.
        /// </value>
        public Camera MinimapCamera
        {
            get => this.minimapCamera;
        }

        /// <summary>
        /// Gets the camera movement speed.
        /// </summary>
        /// <value>
        /// The camera movement speed.
        /// </value>
        public float CameraMovementSpeed
        {
            get => this.cameraMovementSpeed;
        }

        public void Start()
        {
            //this.MinimapRawImage.mainTexture.width = Screen.width;
            //this.MinimapRawImage.mainTexture.height = Screen.height;
            EnhancedTouchSupport.Enable();

            if (this.MapCollider != null)
            {
                this.MapCollider.size = new Vector2(this.MinimapBackground.rect.width, this.MinimapBackground.rect.height);
            }
        }

        /// <summary>
        /// Updates this instance when the Unity Engine updates each frame.
        /// </summary>
        public override void Update()
        {
            if (this.MapCollider != null && this.MapCollider.OverlapPoint(InputExtension.MousePosition))
            {
                this.mouseOver = true;
            }
            else
            {
                this.mouseOver = false;
            }

            if (this.mouseOver && Input.GetMouseButtonDown(InputConfiguration.LeftMouseButton))
            {
                this.leftMouseButtonDown = true;
                InputExtension.HideMouse();
                InputExtension.ClampMouse();
                InputExtension.LockMouse();
            }
            else if (!this.mouseOver || Input.GetMouseButtonUp(InputConfiguration.LeftMouseButton))
            {
                this.leftMouseButtonDown = false;
                InputExtension.UnlockMouse();
                InputExtension.UnClampMouse();
                InputExtension.ShowMouse();
            }

            base.Update();
        }

        /// <summary>
        /// Executes at a fixed interval which is determined by the Unity Engine at runtime.
        /// </summary>
        public void FixedUpdate()
        {
            this.MoveCamera();
            this.ZoomCamera();

            if (InputExtension.IsOpenMinimapPressed() && !this.lockInput)
            {
                this.lockInput = true;

                StartCoroutine(nameof(base.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        InputExtension.IsOpenMinimapPressed,
                        () =>
                        {
                            this.CloseButton.onClick.Invoke();
                            this.lockInput = false;
                        }));
            }
        }

        /// <summary>
        /// Called when this instance is enabled.
        /// </summary>
        public override void OnEnable()
        {
            if (PlayerBehaviour.Instance == null)
            {
                return;
            }

            var minimapPosition = new Vector3(PlayerBehaviour.Instance.Position.x, PlayerBehaviour.Instance.Position.y, MinimapZPosition);
            
            this.MinimapCamera.transform.position = minimapPosition;
            this.MinimapCamera.fieldOfView = FieldOfViewDefault;

            this.RedMuglumpPanel.Value = this.Dungeon.Dungeon.Count(room => room.GetRedMuglumpBehaviour() != null).ToString();
            this.BlackMuglumpPanel.Value = this.Dungeon.Dungeon.Count(room => room.GetBlackMuglumpBehaviour() != null).ToString();
            this.BlueMuglumpPanel.Value = this.Dungeon.Dungeon.Count(room => room.GetBlueMuglumpBehaviour() != null).ToString();
            this.GoldMuglumpPanel.Value = this.Dungeon.Dungeon.Count(room => room.GetGoldMuglumpBehaviour() != null).ToString();
            this.SilverbackMuglumpPanel.Value = this.Dungeon.Dungeon.Count(room => room.GetSilverbackMuglumpBehaviour() != null).ToString();

            this.PitPanel.Value = this.Dungeon.Dungeon.Count(room =>
            {
                var pit = room.GetPitBehaviour();

                return pit != null && pit.Net == null;
            }).ToString();

            this.BatPanel.Value = this.Dungeon.Dungeon.Count(room => room.GetBats() != null).ToString();
            this.ArrowPanel.Value = this.Dungeon.Dungeon.Count(room => room.GetArrowItemBehaviour() != null).ToString();
            this.FlashArrowPanel.Value = this.Dungeon.Dungeon.Count(room => room.GetFlashArrowItemBehaviour() != null).ToString();
            this.NetArrowPanel.Value = this.Dungeon.Dungeon.Count(room => room.GetNetArrowItemBehaviour() != null).ToString();
            this.MuglumpOilPanel.Value = this.Dungeon.Dungeon.Count(room => room.GetCoverScentItemBehaviour() != null).ToString();
            this.BearTrapPanel.Value = this.Dungeon.Dungeon.Count(room => room.GetBearTrapItemBehaviour() != null).ToString();
            this.CrownPanel.Value = this.Dungeon.Dungeon.Count(room => room.GetCrownBehaviour() != null).ToString();
            this.CloseButton.onClick.AddListener(() => this.Disable());
            this.Dungeon.RenderAll();

            base.OnEnable();
        }

        /// <summary>
        /// Called when this instance is disabled.
        /// </summary>
        public override void OnDisable()
        {
            this.CloseButton.onClick.RemoveAllListeners();
            this.Dungeon.OptimizeRendering();
            base.OnDisable();
        }

        /// <summary>
        /// Moves the camera.
        /// </summary>
        private void MoveCamera()
        {
            var movementVector = Vector2.zero;

            if (this.leftMouseButtonDown)
            {
                movementVector = new Vector2(
                    Input.GetAxis(InputAxes.MouseX) * this.mapScrollMultiplier,
                    Input.GetAxis(InputAxes.MouseY) * this.mapScrollMultiplier);
            }
            else if (Touch.activeFingers.Count > 0)
            { 
                if (Touch.activeFingers.Count == 1 && Touch.activeTouches.First().phase == UnityEngine.InputSystem.TouchPhase.Moved)
                {
                    movementVector = new Vector2(
                        -Touch.activeTouches.First().delta.normalized.x,
                        -Touch.activeTouches.First().delta.normalized.y);
                }
                else if (Touch.activeFingers.Count == 2)
                {
                    ZoomCamera(Touch.activeTouches[0], Touch.activeTouches[1]);
                }
            }
            else if (InputExtension.IsCenterMapPressed())
            {
                this.MinimapCamera.transform.position = new Vector3(
                    PlayerBehaviour.Instance.transform.position.x,
                    PlayerBehaviour.Instance.transform.position.y,
                    MinimapZPosition);
                return;
            }
            else
            {
                movementVector = new Vector2(
                    Input.GetAxisRaw(InputAxes.Horizontal) * this.CameraMovementSpeed,
                    Input.GetAxisRaw(InputAxes.Vertical) * this.CameraMovementSpeed);
            }

            movementVector.Normalize();
            this.MoveCamera(movementVector);
        }

        /// <summary>
        /// Moves the camera.
        /// </summary>
        /// <param name="movementVector">The movement vector.</param>
        private void MoveCamera(Vector2 movementVector)
        {
            if (movementVector == Vector2.zero)
            {
                return;
            }

            var cameraBoundary = this.Dungeon.Rectangle;

            var xPosition = MathfExtension.MaxOrMin(
                this.MinimapCamera.transform.position.x + movementVector.x, 
                cameraBoundary.RightBound, 
                cameraBoundary.LeftBound);

            var yPosition = MathfExtension.MaxOrMin(
                this.MinimapCamera.transform.position.y + movementVector.y,
                cameraBoundary.UpperBound,
                cameraBoundary.LowerBound);

            var newPosition = new Vector3(xPosition, yPosition, this.MinimapCamera.transform.position.z);

            this.MinimapCamera.transform.position = newPosition;
        }

        /// <summary>
        /// Zooms the camera.
        /// </summary>
        private void ZoomCamera()
        {
            var zoomDelta = InputExtension.GetZoomDelta() * this.zoomSpeed;
            this.ZoomCamera(zoomDelta);
        }

        private void ZoomCamera(Touch firstTouch, Touch secondTouch)
        {
            if (firstTouch.phase == UnityEngine.InputSystem.TouchPhase.Began ||
                secondTouch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                this.lastMultitouchDistance = Vector2.Distance(firstTouch.screenPosition,
                    secondTouch.screenPosition);
            }

            if (firstTouch.phase != UnityEngine.InputSystem.TouchPhase.Moved ||
                secondTouch.phase != UnityEngine.InputSystem.TouchPhase.Moved)
            {
                return;
            }

            var newMultiTouchDistance = Vector2.Distance(firstTouch.screenPosition,
                secondTouch.screenPosition);

            ZoomCamera((newMultiTouchDistance - this.lastMultitouchDistance) * this.zoomSpeed);

            this.lastMultitouchDistance = newMultiTouchDistance;
        }

        private void ZoomCamera(float zoomDelta)
        {
            this.MinimapCamera.fieldOfView = MathfExtension.MaxOrMin(this.MinimapCamera.fieldOfView - zoomDelta, FieldOfViewMax, FieldOfViewMin);
            this.mapScrollMultiplier = (this.MinimapCamera.fieldOfView / FieldOfViewMax) * -1.0f;
        }

        /// <summary>
        /// Enables this instance.
        /// </summary>
        public override void Enable()
        {
            if (PlayerBehaviour.Instance.IsWalking)
            {
                return;
            }

            base.Enable();
        }

        public void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(MinimapBehaviour));
        }
    }
}
