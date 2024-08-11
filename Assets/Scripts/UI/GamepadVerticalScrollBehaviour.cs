#pragma warning disable CS0649
/**************************************************
 *  GamepadVerticalScrollBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    [RequireComponent(typeof(Scrollbar))]
    public class GamepadVerticalScrollBehaviour : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private Scrollbar scrollbar;

        public Scrollbar Scrollbar
        {
            get
            {
                if (this.scrollbar == null)
                {
                    this.scrollbar = this.GetComponent<Scrollbar>();
                }

                return this.scrollbar;
            }
        }

        public void FixedUpdate()
        {
            var scrollDelta = InputExtension.GetVerticalScrollDelta();

            if (scrollDelta > float.Epsilon && this.Scrollbar.value < 1.0f)
            {
                this.Scrollbar.value = Mathf.Min(this.Scrollbar.value + scrollDelta, 1.0f);
            }
            else if (scrollDelta < -float.Epsilon && this.Scrollbar.value > 0.0f)
            {
                this.Scrollbar.value = Mathf.Max(this.Scrollbar.value + scrollDelta, 0.0f);
            }
        }
    }
}
