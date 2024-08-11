/**************************************************
 *  RoomPrefabs.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Utilities
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Static class that is used for storing and retrieving all of the available room prefabs at runtime.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class RoomPrefabs : MonoBehaviour
    {
        /// <summary>
        /// The inner room prefabs
        /// </summary>
        [SerializeField]
        private List<GameObject> innerRoomPrefabs = new List<GameObject>();

        /// <summary>
        /// The edge north prefabs
        /// </summary>
        [SerializeField]
        private List<GameObject> edgeNorthPrefabs = new List<GameObject>();

        /// <summary>
        /// The edge east prefabs
        /// </summary>
        [SerializeField]
        private List<GameObject> edgeEastPrefabs = new List<GameObject>();

        /// <summary>
        /// The edge south prefabs
        /// </summary>
        [SerializeField]
        private List<GameObject> edgeSouthPrefabs = new List<GameObject>();

        /// <summary>
        /// The edge west prefabs
        /// </summary>
        [SerializeField]
        private List<GameObject> edgeWestPrefabs = new List<GameObject>();

        /// <summary>
        /// The corner north east prefabs
        /// </summary>
        [SerializeField]
        private List<GameObject> cornerNorthEastPrefabs = new List<GameObject>();

        /// <summary>
        /// The corner south east prefabs
        /// </summary>
        [SerializeField]
        private List<GameObject> cornerSouthEastPrefabs = new List<GameObject>();

        /// <summary>
        /// The corner south west prefabs
        /// </summary>
        [SerializeField]
        private List<GameObject> cornerSouthWestPrefabs = new List<GameObject>();

        /// <summary>
        /// The corner north west prefabs
        /// </summary>
        [SerializeField]
        private List<GameObject> cornerNorthWestPrefabs = new List<GameObject>();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static RoomPrefabs Instance { get; private set; }

        /// <summary>
        /// Gets the inner room prefabs.
        /// </summary>
        /// <value>
        /// The inner room prefabs.
        /// </value>
        public List<GameObject> InnerRoomPrefabs { get => this.innerRoomPrefabs; }

        /// <summary>
        /// Gets the edge north prefabs.
        /// </summary>
        /// <value>
        /// The edge north prefabs.
        /// </value>
        public List<GameObject> EdgeNorthPrefabs { get => this.edgeNorthPrefabs; }

        /// <summary>
        /// Gets the edge east prefabs.
        /// </summary>
        /// <value>
        /// The edge east prefabs.
        /// </value>
        public List<GameObject> EdgeEastPrefabs { get => this.edgeEastPrefabs; }

        /// <summary>
        /// Gets the edge south prefabs.
        /// </summary>
        /// <value>
        /// The edge south prefabs.
        /// </value>
        public List<GameObject> EdgeSouthPrefabs { get => this.edgeSouthPrefabs; }

        /// <summary>
        /// Gets the edge west prefabs.
        /// </summary>
        /// <value>
        /// The edge west prefabs.
        /// </value>
        public List<GameObject> EdgeWestPrefabs { get => this.edgeWestPrefabs; }

        /// <summary>
        /// Gets the corner north east prefabs.
        /// </summary>
        /// <value>
        /// The corner north east prefabs.
        /// </value>
        public List<GameObject> CornerNorthEastPrefabs { get => this.cornerNorthEastPrefabs; }

        /// <summary>
        /// Gets the corner south east prefabs.
        /// </summary>
        /// <value>
        /// The corner south east prefabs.
        /// </value>
        public List<GameObject> CornerSouthEastPrefabs { get => this.cornerSouthEastPrefabs; }

        /// <summary>
        /// Gets the corner south west prefabs.
        /// </summary>
        /// <value>
        /// The corner south west prefabs.
        /// </value>
        public List<GameObject> CornerSouthWestPrefabs { get => this.cornerSouthWestPrefabs; }

        /// <summary>
        /// Gets the corner north west prefabs.
        /// </summary>
        /// <value>
        /// The corner north west prefabs.
        /// </value>
        public List<GameObject> CornerNorthWestPrefabs { get => this.cornerNorthWestPrefabs; }

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
    }
}
