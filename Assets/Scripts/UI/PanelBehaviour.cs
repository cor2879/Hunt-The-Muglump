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

            canvasGroup = GetComponent<CanvasGroup>();

            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        public virtual void Show(bool fadeIn = true)
        {
            Debug.Log($"SHOW CALLED on {gameObject.name}, active={gameObject.activeInHierarchy}, alpha={canvasGroup.alpha}");
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            if (fadeIn)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;

                if (fadeRoutine != null)
                {
                    StopCoroutine(fadeRoutine);
                }

                fadeRoutine = StartCoroutine(Fade(1f));
            }

            if (ButtonsPanel != null)
            {
                ButtonsPanel.Activate();
                ButtonsPanel.DefaultButton.Select();
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

                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;

                fadeRoutine = StartCoroutine(FadeOutAndHide());
            }
            else
            {
                gameObject.SetActive(false);
            }

            if (this.ButtonsPanel != null)
            {
                this.ButtonsPanel.Deactivate();
            }
        }

        #region Coroutines

        private IEnumerator Fade(float target)
        {
            float start = canvasGroup.alpha;
            float time = 0f;

            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                float t = time / fadeDuration;

                canvasGroup.alpha = Mathf.Lerp(start, target, t);
                yield return null;
            }

            canvasGroup.alpha = target;
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

        #endregion
    }
}
