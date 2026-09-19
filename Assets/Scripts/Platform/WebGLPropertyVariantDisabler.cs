#if UNITY_WEBGL && !UNITY_EDITOR
using UnityEngine;
using UnityEngine.Localization.PropertyVariants;
using UnityEngine.SceneManagement;

namespace OldSchoolGames.HuntTheMuglump.Scripts.Platform
{
    /// <summary>
    /// Unity Localization property variants can release an Addressables group
    /// operation while WebGL is still completing it. Keep the serialized English
    /// UI values on WebGL instead of allowing GameObjectLocalizer.Start to run.
    /// </summary>
    public static class WebGLPropertyVariantDisabler
    {
        private static bool initialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (initialized)
            {
                return;
            }

            initialized = true;
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void DisableInitialSceneLocalizers()
        {
            DisableLocalizers();
        }

        private static void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            DisableLocalizers();
        }

        private static void DisableLocalizers()
        {
            GameObjectLocalizer[] localizers = Object.FindObjectsOfType<GameObjectLocalizer>(true);
            int disabledCount = 0;

            foreach (GameObjectLocalizer localizer in localizers)
            {
                if (!localizer.enabled)
                {
                    continue;
                }

                localizer.enabled = false;
                disabledCount++;
            }

            if (disabledCount > 0)
            {
                Debug.Log(
                    $"[WebGL Localization] Disabled {disabledCount} property variant localizer(s); " +
                    "using serialized English UI values.");
            }
        }
    }
}
#endif
