#pragma warning disable CS0649
/**************************************************
 *  ActionStateLabelController.cs
 *  
 *  Controls which action state label is visible
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI.GameplayPrompts
{
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.GameplayManagement;

    public class ActionStateLabelController : MonoBehaviour
    {
        [SerializeField]
        private GameObject walkModeLabel;

        [SerializeField]
        private GameObject shootModeLabel;

        [SerializeField]
        private GameObject lookModeLabel;

        private ActionStateBase lastState;

        private GameplayMenuManagerBehaviour GameplayMenuManager 
        { get => GameplayMenuManagerBehaviour.Instance; }

        private void OnEnable()
        {
            GameplayMenuStateBase.OnMenuStateChanged += OnMenuStateChanged;
        }

        private void OnDisable()
        {
            GameplayMenuStateBase.OnMenuStateChanged -= OnMenuStateChanged;
        }

        private void Update()
        {
            var currentState = GameplayMenuManager.MenuState.ActionState;

           /* // Only update when state changes (avoids unnecessary SetActive spam)
            if (currentState == lastState)
            {
                return;
            }

            lastState = currentState; */

            this.UpdateLabels(currentState);
        }

        private void OnMenuStateChanged()
        {
            walkModeLabel.SetActive(false);
            shootModeLabel.SetActive(false);
            lookModeLabel.SetActive(false);            
        }

        private void UpdateLabels(ActionStateBase state)
        {
            // Enable correct one
            if (state is WalkingState)
            {
                walkModeLabel.SetActive(true);
                shootModeLabel.SetActive(false);
                lookModeLabel.SetActive(false);
            }
            else if (state is ShootingState)
            {
                shootModeLabel.SetActive(true);
                walkModeLabel.SetActive(false);
                lookModeLabel.SetActive(false);
            }
            else if (state is LookingState)
            {
                lookModeLabel.SetActive(true);
                shootModeLabel.SetActive(false);
                walkModeLabel.SetActive(false);
            }
            else if (state is EverythingState)
            {
                // Mobile fallback — you can decide behavior here
                // For now, just show walk (or none if you prefer)
                walkModeLabel.SetActive(true);
                shootModeLabel.SetActive(false);
                lookModeLabel.SetActive(true);
            }
        }
    }
}