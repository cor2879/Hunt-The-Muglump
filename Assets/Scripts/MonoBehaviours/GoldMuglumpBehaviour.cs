namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using UnityEngine;
    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class GoldMuglumpBehaviour : MuglumpBehaviour
    {
        public override Statistic<int> KillCountStatistic => Statistic.GoldMuglumpsKilled;

        public override MuglumpType MuglumpType { get => MuglumpType.GoldMuglump; }
    }
}
