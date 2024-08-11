namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    [RequireComponent(typeof(HunterBehaviour))]
    [RequireComponent(typeof(MovementBehaviour))]
    public class BlueMuglumpBehaviour : MuglumpBehaviour
    {
        private MovementBehaviour movementBehaviour;

        public MovementBehaviour MovementBehaviour
        {
            get
            {
                if (this.movementBehaviour == null)
                {
                    this.movementBehaviour = this.GetComponent<MovementBehaviour>();
                }

                return this.movementBehaviour;
            }
        }

        public override Statistic<int> KillCountStatistic => Statistic.BlueMuglumpsKilled;

        public override IList<string> IdleSounds { get => SoundClips.LargeBreathing; }

        public override MuglumpType MuglumpType { get => MuglumpType.BlueMuglump; }

        public override bool IsTrapped
        {
            get => this.MovementBehaviour.IsTrapped;
            set => this.MovementBehaviour.IsTrapped = value;
        }

        public override string GetMessage()
        {
            if (this.CurrentRoom.GetAdjacentRooms().Any(kvp => kvp.Value != null && kvp.Value == PlayerBehaviour.Instance.CurrentRoom))
            {
                return base.GetMessage();
            }

            return StringContent.BlueMuglumpWarning;
        }
    }
}
