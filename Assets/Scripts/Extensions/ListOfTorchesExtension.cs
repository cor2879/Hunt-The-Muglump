/**************************************************
 *  RoomBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public static class CollectionOfTorchesExtension
    {
        public static bool AreLit(this IEnumerable<TorchBehaviour> torches)
        {
            Validator.ArgumentIsNotNull(torches, nameof(torches));

            return torches.Any(torch => torch.IsLit);
        }
    }
}
