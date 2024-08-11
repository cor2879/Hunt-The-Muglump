#pragma warning disable CS0649, CS0109
/**************************************************
 *  FlameBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    [RequireComponent(typeof(SpriteRenderer))]
    public class FlameBehaviour : MonoBehaviour
    {
        private const float defaultRange = 10.0f;

        private const float defaultIntensity = 1.0f;

        [SerializeField, ReadOnly]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private new Light light;

        [SerializeField, ReadOnly]
        private bool isLit;

        private bool playSound;

        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if (this.spriteRenderer == null)
                {
                    this.spriteRenderer = this.GetComponent<SpriteRenderer>();
                }

                return this.spriteRenderer;
            }
        }

        public Light Light
        {
            get => this.light;
        }

        public bool IsLit
        {
            get => this.isLit;
            private set => this.isLit = value;
        }

        public void Ignite()
        {
            this.Ignite(true);
        }

        public void Ignite(bool playSound)
        {
            this.playSound = playSound;
            this.gameObject.SetActive(true);
        }

        public void Extinguish()
        {
            this.Reset();

            if (this.IsLit)
            {
                this.gameObject.SetActive(false);
            }
        }

        public void SetImportant()
        {
            this.light.renderMode = LightRenderMode.ForcePixel;
        }

        public void SetUnimportant()
        {
            this.light.renderMode = LightRenderMode.ForceVertex;
        }

        public void OnEnable()
        {
            if (this.playSound)
            {
                GameManager.Instance.SoundEffectManager?.PlayAudioOnce(SoundClips.Ignite2);
            }

            this.IsLit = true;
        }

        public void Reset()
        {
            this.Light.range = defaultRange;
            this.Light.intensity = defaultIntensity;
        }
    }
}
