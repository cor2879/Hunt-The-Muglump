#pragma warning disable CS0649
/**************************************************
 *  MusicSelectorBehaviour.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class MusicSelectorBehaviour : OptionSelectorBehaviour
    {
        public override void Initialize()
        {
            this.LocalizeText = false;
            this.Options = SoundClips.PlaylistFriendlyNames;
        }
    }
}
