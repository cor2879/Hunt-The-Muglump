#if UNITY_WEBGL && !UNITY_EDITOR
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OldSchoolGames.HuntTheMuglump.Scripts.Platform
{
    /// <summary>
    /// Captures WebGL-only failures before the first scene loads so exceptions
    /// that occur during a scene transition remain visible and actionable.
    /// </summary>
    public sealed class WebGLDiagnostics : MonoBehaviour
    {
        private const int MaximumDisplayedErrors = 12;
        private static WebGLDiagnostics instance;

        private readonly Queue<string> errors = new Queue<string>();
        private string displayedErrors = string.Empty;
        private bool hasError;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (instance != null)
            {
                return;
            }

            Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.Full);
            Application.SetStackTraceLogType(LogType.Assert, StackTraceLogType.Full);
            Application.SetStackTraceLogType(LogType.Exception, StackTraceLogType.Full);

            GameObject diagnosticsObject = new GameObject(nameof(WebGLDiagnostics));
            DontDestroyOnLoad(diagnosticsObject);
            instance = diagnosticsObject.AddComponent<WebGLDiagnostics>();
        }

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLogMessage;
            SceneManager.sceneLoaded += HandleSceneLoaded;
            SceneManager.activeSceneChanged += HandleActiveSceneChanged;

            Debug.Log("[WebGL Diagnostics] Initialized before scene load.");
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLogMessage;
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            SceneManager.activeSceneChanged -= HandleActiveSceneChanged;
        }

        private static void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"[WebGL Diagnostics] Scene loaded: {scene.name} ({mode}).");
        }

        private static void HandleActiveSceneChanged(Scene previousScene, Scene nextScene)
        {
            Debug.Log($"[WebGL Diagnostics] Active scene changed: {previousScene.name} -> {nextScene.name}.");
        }

        private void HandleLogMessage(string condition, string stackTrace, LogType type)
        {
            if (type != LogType.Error && type != LogType.Exception && type != LogType.Assert)
            {
                return;
            }

            StringBuilder entry = new StringBuilder();
            entry.Append('[').Append(type).Append("] ").AppendLine(condition);

            if (!string.IsNullOrWhiteSpace(stackTrace))
            {
                entry.AppendLine(stackTrace);
            }

            errors.Enqueue(entry.ToString());

            while (errors.Count > MaximumDisplayedErrors)
            {
                errors.Dequeue();
            }

            StringBuilder combinedErrors = new StringBuilder();
            combinedErrors.AppendLine("WEBGL ERROR — copy this text or capture a screenshot");
            combinedErrors.AppendLine();

            foreach (string error in errors)
            {
                combinedErrors.AppendLine(error);
            }

            displayedErrors = combinedErrors.ToString();
            hasError = true;
        }

        private void OnGUI()
        {
            if (!hasError)
            {
                return;
            }

            GUI.depth = -1000;

            float width = Mathf.Max(300f, Screen.width - 20f);
            float height = Mathf.Min(560f, Screen.height - 20f);
            Rect panel = new Rect(10f, 10f, width, height);

            GUI.Box(panel, GUIContent.none);
            GUI.TextArea(
                new Rect(panel.x + 10f, panel.y + 10f, panel.width - 20f, panel.height - 20f),
                displayedErrors);
        }
    }
}
#endif
