﻿#pragma warning disable CS0649
/**************************************************
 *  AmountSelectorBehaviour.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System.Linq;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.GameplayManagement;

    public class LanguageSelectorBehaviour : OptionSelectorBehaviour
    {
        public override void Initialize()
        {
            this.LocalizeText = false;

            if (this.Options == null || !this.Options.Any())
            {
                this.Options = SupportedLanguage.SupportedLanguages.Values.Select(v => v.Name).ToList();
            }

            this.SelectedIndex = this.Options.IndexOf(LocaleManager.Instance.SelectedLanguage);
        }

        public SupportedLanguage GetSelectedLanguage()
        {
            return SupportedLanguage.SupportedLanguages.ElementAt(this.SelectedIndex).Value;
        }
    }
}
