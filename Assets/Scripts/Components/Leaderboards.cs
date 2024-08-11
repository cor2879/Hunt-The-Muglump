/**************************************************
 *  Leaderboards.cs
 *  
 *  copyright (c) 2020 Old School Games
 ***************************************************/
 // #define DISABLESTEAMWORKS

namespace OldSchoolGames.HuntTheMuglump.Scripts.Components
{
    using UnityEngine;

#if UNITY_STEAM
    using Steamworks;

    using OldSchoolGames.HuntTheMuglump.Scripts.Platform;
#endif

    public static class LeaderboardNames
    {
        public const string TopScores = "TopScores";
        public const string RoomsExplored = "RoomsExplored";
    }

    public class LeaderBoardData
    {
        public string LeaderboardName { get; set; }

        public int Score { get; set; }
    }

#if UNITY_STEAM
    public class Leaderboard
    {
        private const ELeaderboardUploadScoreMethod UploadMethod = ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate;
        private SteamLeaderboard_t steamLeaderboard;
        private CallResult<LeaderboardFindResult_t> leaderboardFindCallResult;
        private CallResult<LeaderboardScoreUploaded_t> leaderboardScoreUploadedCallResult;
        private CallResult<LeaderboardScoresDownloaded_t> leaderboardScoresDownloadedCallResult;
        private LeaderboardEntry_t leaderboardEntry;

        public string Name { get; set; }

        public void Init()
        {
            this.leaderboardFindCallResult = CallResult<LeaderboardFindResult_t>.Create(this.OnLeaderboardFound);
            this.leaderboardScoreUploadedCallResult = CallResult<LeaderboardScoreUploaded_t>.Create(this.OnLeaderboardScoreUploaded);
            this.leaderboardScoresDownloadedCallResult = CallResult<LeaderboardScoresDownloaded_t>.Create(this.OnLeaderboardScoresDownloaded);

            FindLeaderboard(this.Name);
        }

        private void FindLeaderboard(string name)
        {
            if (SteamManager.Instance.IsSteamClientInitialized())
            {
                var handle = SteamUserStats.FindLeaderboard(name);
                this.leaderboardFindCallResult.Set(handle);
            }
        }

        public string GetLeaderboardName(SteamLeaderboard_t handle)
        {
            var name = SteamUserStats.GetLeaderboardName(handle);

            return name;
        }

        public void UploadLeaderboardScore(int score)
        {
            if (!SteamManager.Instance.IsSteamClientInitialized())
            {
                return;
            }

            if (this.steamLeaderboard.m_SteamLeaderboard != 0 && (this.leaderboardEntry.m_nScore < score))
            {
                var handle = SteamUserStats.UploadLeaderboardScore(this.steamLeaderboard, Leaderboard.UploadMethod, score, null, 0);
                this.leaderboardScoreUploadedCallResult.Set(handle);
            }
        }

        private void DownloadScore()
        {
            if (this.steamLeaderboard.m_SteamLeaderboard != 0)
            {
                var handle = SteamUserStats.DownloadLeaderboardEntries(this.steamLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser, 0, 0);
                this.leaderboardScoresDownloadedCallResult.Set(handle);
            }
        }

        private void OnLeaderboardScoresDownloaded(LeaderboardScoresDownloaded_t result, bool ioFailure)
        {
            if (!ioFailure)
            {
                var leaderboardEntry = new LeaderboardEntry_t()
                {
                    m_steamIDUser = CSteamID.Nil,
                    m_nGlobalRank = 0,
                    m_nScore = 0,
                    m_cDetails = 0,
                    m_hUGC = UGCHandle_t.Invalid
                };

                SteamUserStats.GetDownloadedLeaderboardEntry(result.m_hSteamLeaderboardEntries, 0, out leaderboardEntry, null, 0);

                Debug.Log(leaderboardEntry.m_nScore);

                if (leaderboardEntry.m_steamIDUser != CSteamID.Nil)
                {
                    this.leaderboardEntry = leaderboardEntry;
                }
            }
        }

        private void OnLeaderboardFound(LeaderboardFindResult_t result, bool ioFailure)
        {
            if (result.m_bLeaderboardFound != 0)
            {
                this.steamLeaderboard = result.m_hSteamLeaderboard;
            }

            this.GetLeaderboardName(this.steamLeaderboard);
            this.DownloadScore();
        }

        private void OnLeaderboardScoreUploaded(LeaderboardScoreUploaded_t result, bool ioFailure)
        {
            Debug.Log(result);
        }
    }
#endif
}
