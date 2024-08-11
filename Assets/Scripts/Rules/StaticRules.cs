/**************************************************
 *  StaticRule.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Rules
{
    using System;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;

    public static class StaticRules
    {
        public static MultiStatisticRule<int> FirstBlood = new MultiStatisticRule<int>("FirstBlood",
            new[]
            {
                Statistic.MuglumpsKilled,
                Statistic.BlackMuglumpsKilled,
                Statistic.BlueMuglumpsKilled,
                Statistic.GoldMuglumpsKilled,
                Statistic.SilverbackMuglumpsKilled
            },
            1);

        public static StatisticRule<int> BatRider = new StatisticRule<int>("BatRider", Statistic.BatsRidden, 25);

        public static StatisticRule<int> Shiny = new StatisticRule<int>("Shiny", Statistic.CrownsFound, 1);

        public static StatisticRule<int> IfItBleeds = new StatisticRule<int>("IfItBleeds", Statistic.BlackMuglumpsKilled, 1);

        public static StatisticRule<int> GoldenTicket = new StatisticRule<int>("GoldenTicket", Statistic.GoldMuglumpsKilled, 1);

        public static StatisticRule<int> Explorer = new StatisticRule<int>("Explorer", Statistic.RoomsExplored, 100);

        public static StatisticRule<int> DungeonCrawler = new StatisticRule<int>("DungeonCrawler", Statistic.RoomsExplored, 1000);

        public static StatisticRule<int> LightItUp = new StatisticRule<int>("LightItUp", Statistic.BatsFlashed, 5);

        public static StatisticRule<int> HuntTheHunter = new StatisticRule<int>("HuntTheHunter", Statistic.BlueMuglumpsKilled, 1);

        public static StatisticRule<int> ImSpecial = new StatisticRule<int>("ImSpecial", Statistic.SpecialKills, 1);

        public static MultiStatisticRule<int> MuglumpSlayer = new MultiStatisticRule<int>("MuglumpSlayer",
            new[]
            {
                Statistic.MuglumpsKilled,
                Statistic.BlackMuglumpsKilled,
                Statistic.BlueMuglumpsKilled,
                Statistic.GoldMuglumpsKilled,
                Statistic.SilverbackMuglumpsKilled
            }, 
            10);

        public static StatisticRule<int> ApexHunter = new StatisticRule<int>("ApexHunter", Statistic.SilverbackMuglumpsKilled, 1);

        public static StatisticRule<int> MuglumpMastery = new StatisticRule<int>("MuglumpMastery", Statistic.SilverbackMuglumpsKilled, 10);

        public static StatisticRule<int> Trapper = new StatisticRule<int>("Trapper", Statistic.BearTrapsSet, 5);

        public static StatisticRule<int> ExpertTrapper = new StatisticRule<int>("ExpertTrapper", Statistic.MuglumpsTrapped, 5);

        public static StatisticRule<int> MasterTrapper = new StatisticRule<int>("MasterTrapper", Statistic.MuglumpsTrapped, 25);

        public static StatisticRule<int> Overkill = new StatisticRule<int>("Overkill", Statistic.Overkills, 15);

        public static EndGameRule Survived = new EndGameRule("Survived", (settings) =>
        {
            return settings.Survived;
        });

        public static EndGameRule HitchHiker = new EndGameRule("HitchHiker", (settings) =>
        {
            return settings.Survived && settings.TimesCarriedCount >= 5;

        });

        public static EndGameRule NoMuglumpsKilled = new EndGameRule("NoMuglumpsKilled", (settings) =>
        {
            return settings.TotalMuglumpsSlainCount == 0;
        });

        public static EndGameRule NormalDifficulty = new EndGameRule("NormalDifficulty", (settings) =>
        {
            return Settings.Difficulty.Setting >= DifficultySetting.Normal;
        });

        public static EndGameRule AllMuglumpsKilled = new EndGameRule("AllMuglumpsKilled", (settings) =>
        {
            return settings.TotalMuglumpsSlainCount == settings.TotalMuglumpsCount;
        });

        public static EndGameRule Accurate = new EndGameRule("Accurate", (settings) =>
        {
            var allArrowsHit = settings.ArrowsFiredCount > 0 && settings.ArrowsFiredCount == settings.ArrowsHitCount;

            return allArrowsHit;
        });

        public static readonly EndGameRule[] EndGameRules = new EndGameRule[]
        {
            StaticRules.Survived,
            StaticRules.NoMuglumpsKilled,
            StaticRules.NormalDifficulty,
            StaticRules.AllMuglumpsKilled,
            StaticRules.Accurate
        };
    }
}
