/**************************************************
 *  NewGameMenuBehaviour.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.UI;

    using BeautifulInterface = Interface.Elements.Scripts;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class PanelBehaviour : BeautifulInterface.BasePanel
    {
        private ButtonsPanelBehaviour buttonsPanel;
        private CanvasGroup canvasGroup;
        private Coroutine fadeRoutine;

        private CanvasGroup CanvasGroup
        {
            get
            {
                if (this.canvasGroup == null)
                {
                    this.canvasGroup = this.GetComponent<CanvasGroup>();

                    if (this.canvasGroup == null)
                    {
                        this.canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
                    }
                }

                return this.canvasGroup;
            }
        }

        [SerializeField]
        private float fadeDuration = 0.15f;

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

        protected override void Awake()
        {
            base.Awake();

            _ = this.CanvasGroup;
        }

        public virtual void Show(bool fadeIn = true)
        {
            Debug.Log($"SHOW CALLED on {gameObject.name}, active={gameObject.activeInHierarchy}, alpha={this.CanvasGroup.alpha}");
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            if (fadeIn)
            {
                this.CanvasGroup.alpha = 0f;
                this.CanvasGroup.interactable = true;
                this.CanvasGroup.blocksRaycasts = true;

                if (fadeRoutine != null)
                {
                    StopCoroutine(fadeRoutine);
                }

                if (this.isActiveAndEnabled)
                {
                    fadeRoutine = StartCoroutine(Fade(1f));
                }
                else
                {
                    // A panel can be requested while a containing UI group is still
                    // inactive. Coroutines cannot run in that state, so complete the
                    // visual transition immediately; the panel will be ready when its
                    // hierarchy becomes active.
                    this.CanvasGroup.alpha = 1f;
                }
            }

            if (this.ButtonsPanel != null)
            {
                this.ButtonsPanel.Activate();

                // Touch-only panels do not require a selected navigation button.
                if (this.ButtonsPanel.DefaultButton != null)
                {
                    this.ButtonsPanel.DefaultButton.Select();
                }
            }
        }

        public virtual void Hide(bool fadeOut = false)
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            if (fadeOut)
            {
                if (fadeRoutine != null)
                {
                    StopCoroutine(fadeRoutine);
                }

                this.CanvasGroup.interactable = false;
                this.CanvasGroup.blocksRaycasts = false;

                fadeRoutine = StartCoroutine(FadeOutAndHide());
            }
            else
            {
                if (gameObject != null)
                {
                    gameObject.SetActive(false);
                }
            }

            if (this.ButtonsPanel != null)
            {
                this.ButtonsPanel.Deactivate();
            }
        }

        #region Coroutines

        private IEnumerator Fade(float target)
        {
            float start = this.CanvasGroup.alpha;
            float time = 0f;

            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                float t = time / fadeDuration;

                this.CanvasGroup.alpha = Mathf.Lerp(start, target, t);
                yield return null;
            }

            this.CanvasGroup.alpha = target;
        }

        private IEnumerator FadeOutAndHide()
        {
            yield return Fade(0f);

            gameObject.SetActive(false);

            if (this.ButtonsPanel != null)
            {
                this.ButtonsPanel.Deactivate();
            }
        }

        private IEnumerator SelectDefaultButtonNextFrame()
        {
            yield return new WaitForSeconds(1f); // wait 1 frame

            if (ButtonsPanel != null && ButtonsPanel.DefaultButton != null)
            {
                ButtonsPanel.DefaultButton.Select();
            }
        }

        private IEnumerator DisableNextFrame()
        {
            yield return null;
            gameObject.SetActive(false);
        }

        #endregion
    }
}
