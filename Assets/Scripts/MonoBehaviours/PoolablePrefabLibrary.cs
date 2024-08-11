#pragma warning disable CS0649

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Interfaces;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class PoolablePrefabLibrary : MonoBehaviour
    {
        [SerializeField]
        private BearTrapBehaviour bearTrapPrefab;

        [SerializeField]
        private ArrowBehaviour arrowPrefab;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static PoolablePrefabLibrary Instance { get; private set; }

        /// <summary>
        /// Executes when this instance is awakened.
        /// </summary>
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public ArrowBehaviour ArrowPrefab
        {
            get
            {
                ValidateUnityEditorParameter(this.arrowPrefab, nameof(this.arrowPrefab), nameof(PoolablePrefabLibrary));

                return this.arrowPrefab;
            }
        }

        public BearTrapBehaviour BearTrapPrefab
        {
            get
            {
                ValidateUnityEditorParameter(this.bearTrapPrefab, nameof(this.bearTrapPrefab), nameof(PoolablePrefabLibrary));

                return this.bearTrapPrefab;
            }
        }

        public static void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName, string typeName)
        {
            if (parameter == null)
            {
                throw new UIException($"The parameter {parameterName} needs to be set in the Unity Edtior for the {typeName}.");
            }
        }
    }
}
