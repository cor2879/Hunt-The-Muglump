using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Localization.PropertyVariants;
using UnityEngine.SceneManagement;

namespace OldSchoolGames.HuntTheMuglump.Editor
{
    /// <summary>
    /// Removes Localization property-variant components from the temporary scene
    /// copies used for WebGL builds. The source scenes and prefabs are unchanged.
    /// </summary>
    public sealed class WebGLPropertyVariantBuildProcessor : IProcessSceneWithReport
    {
        public int callbackOrder => -10000;

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            if (report == null || report.summary.platform != BuildTarget.WebGL)
            {
                return;
            }

            int removedCount = 0;

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                GameObjectLocalizer[] localizers =
                    root.GetComponentsInChildren<GameObjectLocalizer>(true);

                foreach (GameObjectLocalizer localizer in localizers)
                {
                    Object.DestroyImmediate(localizer);
                    removedCount++;
                }
            }

            if (removedCount > 0)
            {
                Debug.Log(
                    $"[WebGL Localization] Removed {removedCount} property variant localizer(s) " +
                    $"from build scene '{scene.name}'. Serialized English UI values will be used.");
            }
        }
    }
}
