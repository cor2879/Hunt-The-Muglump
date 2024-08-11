// The SteamManager is designed to work with Steamworks.NET
// This file is released into the public domain.
// Where that dedication is not recognized you are granted a perpetual,
// irrevocable license to copy and modify this file as you see fit.
//
// Version: 1.0.8


#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH || XBOX || UNITY_STANDALONE_OSX
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS && !UNITY_STANDALONE_OSX
#define UNITY_STEAMWORKS
#endif

#if !DISABLESTEAMWORKS || UNITY_EDITOR || UNIT_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_XBOXONE || UNITY_WSA
    #define XINPUT
#endif

#if !DISABLESTEAMWORKS
namespace OldSchoolGames.HuntTheMuglump.Scripts.Platform
{

    using UnityEngine;
    using System.Collections;
    using System.IO;
    using Steamworks;

    //
    // The SteamManager provides a base implementation of Steamworks.NET on which you can build upon.
    // It handles the basics of starting up and shutting down the SteamAPI for use.
    //
    [DisallowMultipleComponent]
    public class SteamManager : MonoBehaviour
    {
        protected static SteamManager s_instance;
        public const string SaveDirectoryName = "SavesDir";

        private CallResult<LeaderboardScoreUploaded_t> leaderboardScoreUploadedCallResult;
        private CallResult<LeaderboardFindResult_t> leaderBoardFindCallResult;

        private static long appId = -1;
        private static ulong? steamUserId = null;

        public static SteamManager Instance
        {
            get
            {
                if (s_instance == null)
                {
                    return new GameObject("SteamManager").AddComponent<SteamManager>();
                }
                else
                {
                    return s_instance;
                }
            }
        }

        protected static bool s_EverInitialized;

        protected bool m_bInitialized;

        public static bool Initialized
        {
            get
            {
                return Instance.m_bInitialized;
            }
        }

        public static long AppId
        {
            get
            {
                if (appId < 0 && SteamManager.Initialized)
                {
                    appId = SteamUtils.GetAppID().m_AppId;
                }

                return appId;
            }
        }

        public static ulong SteamUserId
        {
            get
            {
                if (steamUserId == null && SteamManager.Initialized)
                {
                    steamUserId = SteamUser.GetSteamID().m_SteamID;
                }

                return steamUserId.HasValue ? steamUserId.Value : 0;
            }
        }

        protected SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
        protected static void SteamAPIDebugTextHook(int nSeverity, System.Text.StringBuilder pchDebugText)
        {
            Debug.LogWarning(pchDebugText);
        }

        protected virtual void Awake()
        {
            // Only one instance of SteamManager at a time!
            if (s_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            s_instance = this;

            if (s_EverInitialized)
            {
                // This is almost always an error.
                // The most common case where this happens is when SteamManager gets destroyed because of Application.Quit(),
                // and then some Steamworks code in some other OnDestroy gets called afterwards, creating a new SteamManager.
                // You should never call Steamworks functions in OnDestroy, always prefer OnDisable if possible.
                throw new System.Exception("Tried to Initialize the SteamAPI twice in one session!");
            }

            // We want our SteamManager Instance to persist across scenes.
            DontDestroyOnLoad(gameObject);

            if (!Packsize.Test())
            {
                Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
            }

            if (!DllCheck.Test())
            {
                Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
            }

            try
            {
                // If Steam is not running or the game wasn't started through Steam, SteamAPI_RestartAppIfNecessary starts the
                // Steam client and also launches this game again if the User owns it. This can act as a rudimentary form of DRM.

                // Once you get a Steam AppID assigned by Valve, you need to replace AppId_t.Invalid with it and
                // remove steam_appid.txt from the game depot. eg: "(AppId_t)480" or "new AppId_t(480)".
                // See the Valve documentation for more information: https://partner.steamgames.com/doc/sdk/api#initialization_and_shutdown
                if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
                {
                    Application.Quit();
                    return;
                }
            }
            catch (System.DllNotFoundException e)
            { // We catch this exception here, as it will be the first occurrence of it.
                Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + e, this);

                Application.Quit();
                return;
            }

            // Initializes the Steamworks API.
            // If this returns false then this indicates one of the following conditions:
            // [*] The Steam client isn't running. A running Steam client is required to provide implementations of the various Steamworks interfaces.
            // [*] The Steam client couldn't determine the App ID of game. If you're running your application from the executable or debugger directly then you must have a [code-inline]steam_appid.txt[/code-inline] in your game directory next to the executable, with your app ID in it and nothing else. Steam will look for this file in the current working directory. If you are running your executable from a different directory you may need to relocate the [code-inline]steam_appid.txt[/code-inline] file.
            // [*] Your application is not running under the same OS user context as the Steam client, such as a different user or administration access level.
            // [*] Ensure that you own a license for the App ID on the currently active Steam account. Your game must show up in your Steam library.
            // [*] Your App ID is not completely set up, i.e. in Release State: Unavailable, or it's missing default packages.
            // Valve's documentation for this is located here:
            // https://partner.steamgames.com/doc/sdk/api#initialization_and_shutdown
            m_bInitialized = SteamAPI.Init();
            if (!m_bInitialized)
            {
                Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);

                return;
            }

            s_EverInitialized = true;
        }

        // This should only ever get called on first load and after an Assembly reload, You should never Disable the Steamworks Manager yourself.
        protected virtual void OnEnable()
        {
            if (s_instance == null)
            {
                s_instance = this;
            }

            if (!m_bInitialized)
            {
                return;
            }

            if (m_SteamAPIWarningMessageHook == null)
            {
                // Set up our callback to receive warning messages from Steam.
                // You must launch with "-debug_steamapi" in the launch args to receive warnings.
                m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);
                SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
            }

            this.leaderboardScoreUploadedCallResult = CallResult<LeaderboardScoreUploaded_t>.Create(OnLeaderboardScoreUploaded);
            this.leaderBoardFindCallResult = CallResult<LeaderboardFindResult_t>.Create(OnLeaderboardFound);
        }

        // OnApplicationQuit gets called too early to shutdown the SteamAPI.
        // Because the SteamManager should be persistent and never disabled or destroyed we can shutdown the SteamAPI here.
        // Thus it is not recommended to perform any Steamworks work in other OnDestroy functions as the order of execution can not be garenteed upon Shutdown. Prefer OnDisable().
        protected virtual void OnDestroy()
        {
            if (s_instance != this)
            {
                return;
            }

            s_instance = null;

            if (!m_bInitialized)
            {
                return;
            }

            SteamAPI.Shutdown();
        }

        protected virtual void Update()
        {
            if (!m_bInitialized)
            {
                return;
            }

            // Run Steam client callbacks
            SteamAPI.RunCallbacks();
        }

        private void OnLeaderboardScoreUploaded(LeaderboardScoreUploaded_t callback, bool ioFailure)
        {
            Debug.Log(callback);
        }

        private void OnLeaderboardFound(LeaderboardFindResult_t callback, bool ioFailure)
        {
            Debug.Log(callback);
        }

        public void FindLeaderboard(string leaderboardName)
        {
            if (!m_bInitialized)
            {
                return;
            }

            var handle = SteamUserStats.FindLeaderboard(leaderboardName);
            this.leaderBoardFindCallResult.Set(handle);
        }

        public void UploadLeaderboardScore(SteamLeaderboard_t leaderboard, ELeaderboardUploadScoreMethod uploadMethod, int score)
        {
            if (!m_bInitialized)
            {
                return;
            }

            var handle = SteamUserStats.UploadLeaderboardScore(leaderboard, uploadMethod, score, new int[0], 0);
            this.leaderboardScoreUploadedCallResult.Set(handle);
        }

        public string GetUsername()
        {
            if (!m_bInitialized)
            {
                return "OfflineUser";
            }

            return SteamFriends.GetPersonaName();
        }

        public bool IsSteamClientInitialized()
        {
            return m_bInitialized;
        }

        private string GetSaveFileName() => $"{this.GetUsername()}.sav";

        public void SaveFileOffline(string text)
        {
            if (!Directory.Exists(SaveDirectoryName))
            {
                Directory.CreateDirectory(SaveDirectoryName);
            }

            var saveFileName = Path.Combine(SaveDirectoryName, this.GetSaveFileName());
            var doNotAppend = false;

            using (var streamWriter = new StreamWriter(saveFileName, doNotAppend))
            {
                streamWriter.Write(text);
            }
        }

        public string GetSaveFileContent()
        {
            if (!Directory.Exists(SaveDirectoryName))
            {
                Directory.CreateDirectory(SaveDirectoryName);
            }

            var saveFileName = Path.Combine(SaveDirectoryName, this.GetSaveFileName());

            using (var streamReader = new StreamReader(saveFileName))
            {
                if (streamReader != null)
                {
                    return streamReader.ReadToEnd();
                }
            }

            return null;
        }
    }
}
#endif // !DISABLESTEAMWORKS
