/**************************************************
 *  RectTransformExtension.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Extensions
{
    using UnityEngine;

    using System.Collections.Generic;
    using System.Linq;

    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public static class RectTransformExtension
    {
        public static bool IntersectsWith(this RectTransform transform, Vector3 position)
        {
            if (transform == null)
            {
                return false;
            }

            var origin = new Vector2(transform.position.x - transform.sizeDelta.x, transform.position.y - transform.sizeDelta.y);
            var range = new Vector2(transform.position.x + transform.sizeDelta.x, transform.position.y + transform.sizeDelta.y);

            var xValueIntersects = Utility.Between(position.x, origin.x, range.x);
            var yValueIntersects = Utility.Between(position.y, origin.y, range.y);

            return xValueIntersects && yValueIntersects;
        }
    }
}
