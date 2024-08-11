/**************************************************
 *  GameObjectExtension.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Extensions
{
    using UnityEngine;

    using System.Collections.Generic;
    using System.Linq;

    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public static class GameObjectExtension
    {
        public static bool HasComponent<TComponent>(this GameObject gameObject)
            where TComponent : MonoBehaviour
        {
            Validator.ArgumentIsNotNull(gameObject, nameof(gameObject));

            return gameObject.GetComponent<TComponent>() != null;
        }
    }
}
