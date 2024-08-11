#pragma warning disable CS0649
/**************************************************
 *  ArrowImageStateManager.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System;
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;

    [RequireComponent(typeof(ImageStateManager))]
    public class ArrowImageStateManager : UIHelperBehaviour
    {
        private static ArrowType[] arrowTypes = Enum.GetValues(typeof(ArrowType)) as ArrowType[];
        private static bool[] stateMap = new bool[arrowTypes.Length];

        private ImageStateManager imageStateManager;

        public static ArrowType[] ArrowTypes { get { return arrowTypes; } }

        public ImageStateManager ImageStateManager 
        { 
            get 
            { 
                if (this.imageStateManager == null)
                {
                    this.imageStateManager = this.GetComponent<ImageStateManager>();
                }

                return this.imageStateManager;
            } 
        }

        private void Update()
        {
            for (var i = 0; i < arrowTypes.Length; i++)
            {
                stateMap[i] = PlayerBehaviour.Instance.SelectedArrowType == arrowTypes[i];
            }

            ImageStateManager.SetImageStates(stateMap);
        }
    }
}
