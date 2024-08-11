#pragma warning disable CS0649
/**************************************************
 *  AmountSelectorBehaviour.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    using Interface.Elements.Scripts;

    public class AmountSelectorBehaviour : OptionSelectorBehaviour
    {
        public override void Initialize() 
        {
            this.Options = Constants.AmountOptions;
        }
    }
}
