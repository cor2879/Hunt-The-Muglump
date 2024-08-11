#pragma warning disable CS0649
/**************************************************
 *  FlashArrowBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    [RequireComponent(typeof(SpriteRenderer))]
    public class FlashArrowBehaviour : ArrowBehaviour
    {
        private Dictionary<Vector2, Vector3> flamePositionPresets = new Dictionary<Vector2, Vector3>()
        {
            { Vector2.zero, Vector3.zero },
            { new Vector2(-1.0f, 0f), new Vector3(-0.311f, 0.07f, 0) },
            { new Vector2(1.0f, 0f), new Vector3(0.311f, 0.07f, 0) },
            { new Vector2(0, -1.0f), new Vector3(0f, -0.311f, 0) },
            { new Vector2(0, 1.0f), new Vector3(0f, 0.311f, 0) }
        };

        [SerializeField]
        private FlameBehaviour flameBehaviour;

        public override ArrowType ArrowType { get => ArrowType.FlashArrow; }

        public FlameBehaviour FlameBehaviour
        {
            get => this.flameBehaviour;
        }

        public void Update()
        {
            this.FlameBehaviour.gameObject.transform.localPosition = flamePositionPresets[new Vector2(
                    this.Animator.GetFloat(Constants.XFiringDirection),
                    this.Animator.GetFloat(Constants.YFiringDirection))];
        }

        public override void OnDestinationReached()
        {
            this.Destination.HideDarkness();
            this.FlameBehaviour.Light.intensity *= 15;
            this.FlameBehaviour.Light.range *= 10;
            GameManager.Instance.SoundEffectManager.PlayAudioOnce(SoundClips.SmallExplosion);

            StartCoroutine(nameof(DelayThenFinish), 0.1f);
        }

        public override void Fire(RoomBehaviour startingRoom, Direction direction, float velocity, Action onDestinationReached)
        {
            this.FlameBehaviour.Ignite();
            base.Fire(startingRoom, direction, velocity, onDestinationReached);
        }

        public IEnumerator DelayThenFinish(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            this.Destination.TurnOnLights();

            var flock = this.Destination.GetBats();

            if (flock != null)
            {
                flock.Flee();
                PlayerBehaviour.Instance.FlashArrowsHitCount++;
                Statistic.BatsFlashed.Value++;
            }

            this.FlameBehaviour.Extinguish();
            base.OnDestinationReached();
        }

        protected virtual void IncrementArrowsHitCount()
        {
            PlayerBehaviour.Instance.FlashArrowsHitCount++;
        }
    }
}
