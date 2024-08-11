/**************************************************
 *  IFindable.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Interfaces
{
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;

    public interface IFindable
    {
        void HandleEncounter(PlayerBehaviour player);
    }
}
