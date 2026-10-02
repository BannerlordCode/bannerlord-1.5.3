using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper
{
	// Token: 0x02000149 RID: 329
	public class CharacterDeveloperVM : ViewModel
	{
		// Token: 0x06001FC7 RID: 8135 RVA: 0x00072EC4 File Offset: 0x000710C4
		public CharacterDeveloperVM(Action closeCharacterDeveloper)
		{
			this._closeCharacterDeveloper = closeCharacterDeveloper;
			this.TutorialNotification = new ElementNotificationVM();
			this._viewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
			this._heroList = new List<CharacterDeveloperHeroItemVM>();
			this.HeroList = new ReadOnlyCollection<CharacterDeveloperHeroItemVM>(this._heroList);
			foreach (Hero hero in this.GetApplicableHeroes())
			{
				if (hero == null)
				{
					Debug.FailedAssert("Trying to use null hero for character developer", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\CharacterDeveloper\\CharacterDeveloperVM.cs", ".ctor", 39);
				}
				else if (hero.HeroDeveloper == null)
				{
					Debug.FailedAssert("Hero does not have hero developer", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\CharacterDeveloper\\CharacterDeveloperVM.cs", ".ctor", 45);
				}
				else if (hero == Hero.MainHero)
				{
					this._heroList.Insert(0, new CharacterDeveloperHeroItemVM(hero, new Action(this.OnPerkSelection)));
				}
				else
				{
					this._heroList.Add(new CharacterDeveloperHeroItemVM(hero, new Action(this.OnPerkSelection)));
				}
			}
			this._heroIndex = 0;
			this.CharacterList = new SelectorVM<SelectorItemVM>(new List<string>(), this._heroIndex, new Action<SelectorVM<SelectorItemVM>>(this.OnCharacterSelection));
			this.RefreshCharacterSelector();
			this.IsPlayerAccompanied = this._heroList.Count > 1;
			this.SetCurrentHero(this._heroList[this._heroIndex]);
			this._viewDataTracker.ClearCharacterNotification();
			this.UnopenedPerksNumForCurrentCharacter = this.CurrentCharacter.GetNumberOfUnselectedPerks();
			Game.Current.EventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
			this.RefreshValues();
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x0007307C File Offset: 0x0007127C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.DoneLbl = GameTexts.FindText("str_done", null).ToString();
			this.ResetLbl = GameTexts.FindText("str_reset", null).ToString();
			this.CancelLbl = GameTexts.FindText("str_cancel", null).ToString();
			this.SkillsText = GameTexts.FindText("str_skills", null).ToString();
			this.AddFocusText = GameTexts.FindText("str_add_focus", null).ToString();
			this.UnspentCharacterPointsText = GameTexts.FindText("str_character_unspent_character_points", null).ToString();
			this.TraitsText = new TextObject("{=FYJC7cDD}Trait(s)", null).ToString();
			this.PartyRoleText = new TextObject("{=9FJi2SaE}Party Role", null).ToString();
			this.ResetHint = new HintViewModel(GameTexts.FindText("str_reset", null), null);
			this.SkillFocusText = GameTexts.FindText("str_character_skill_focus", null).ToString();
			this.FocusVisualHint = new HintViewModel(new TextObject("{=GwA9oUBC}Your skill focus determines the rate your skill increases with practice", null), null);
			GameTexts.SetVariable("FOCUS_PER_LEVEL", Campaign.Current.Models.CharacterDevelopmentModel.FocusPointsPerLevel);
			GameTexts.SetVariable("ATTRIBUTE_EVERY_LEVEL", Campaign.Current.Models.CharacterDevelopmentModel.LevelsPerAttributePoint);
			this.UnspentCharacterPointsHint = new HintViewModel(GameTexts.FindText("str_character_points_how_to_get", null), null);
			this.UnspentAttributePointsHint = new HintViewModel(GameTexts.FindText("str_attribute_points_how_to_get", null), null);
			this.LevelHint = new HintViewModel(GameTexts.FindText("str_level_tag", null), null);
			this.UnopenedPerksHint = new HintViewModel(new TextObject("{=jmLg6HQh}Number of available perk unlocks.", null), null);
			this.SetPreviousCharacterHint();
			this.SetNextCharacterHint();
			this.CharacterList.RefreshValues();
			this.CurrentCharacter.RefreshValues();
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x0007323D File Offset: 0x0007143D
		private void SetPreviousCharacterHint()
		{
			this.PreviousCharacterHint = new BasicTooltipViewModel(delegate
			{
				GameTexts.SetVariable("HOTKEY", this.GetPreviousCharacterKeyText());
				GameTexts.SetVariable("TEXT", GameTexts.FindText("str_inventory_prev_char", null));
				return GameTexts.FindText("str_hotkey_with_hint", null).ToString();
			});
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x00073256 File Offset: 0x00071456
		private void SetNextCharacterHint()
		{
			this.NextCharacterHint = new BasicTooltipViewModel(delegate
			{
				GameTexts.SetVariable("HOTKEY", this.GetNextCharacterKeyText());
				GameTexts.SetVariable("TEXT", GameTexts.FindText("str_inventory_next_char", null));
				return GameTexts.FindText("str_hotkey_with_hint", null).ToString();
			});
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x00073270 File Offset: 0x00071470
		public void SelectHero(Hero hero)
		{
			for (int i = 0; i < this._heroList.Count; i++)
			{
				if (this._heroList[i].Hero == hero)
				{
					this._heroIndex = i;
					this.RefreshCharacterSelector();
					return;
				}
			}
		}

		// Token: 0x06001FCC RID: 8140 RVA: 0x000732B8 File Offset: 0x000714B8
		private void OnCharacterSelection(SelectorVM<SelectorItemVM> newIndex)
		{
			if (newIndex.SelectedIndex >= 0 && newIndex.SelectedIndex < this._heroList.Count)
			{
				this._heroIndex = newIndex.SelectedIndex;
				this.SetCurrentHero(this._heroList[this._heroIndex]);
				this.UnopenedPerksNumForCurrentCharacter = this.CurrentCharacter.GetNumberOfUnselectedPerks();
				this.HasUnopenedPerksForCurrentCharacter = this._heroList[this._heroIndex].GetNumberOfUnselectedPerks() > 0;
			}
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x00073334 File Offset: 0x00071534
		private void OnPerkSelection()
		{
			this.RefreshCharacterSelector();
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x0007333C File Offset: 0x0007153C
		private void RefreshCharacterSelector()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < this._heroList.Count; i++)
			{
				list.Add(this._heroList[i].GetNameWithNumOfUnopenedPerks());
			}
			this.CharacterList.Refresh(list, this._heroIndex, new Action<SelectorVM<SelectorItemVM>>(this.OnCharacterSelection));
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x0007339C File Offset: 0x0007159C
		public void ExecuteReset()
		{
			foreach (CharacterDeveloperHeroItemVM characterDeveloperHeroItemVM in this._heroList)
			{
				characterDeveloperHeroItemVM.ResetChanges(false);
			}
			this.RefreshCharacterSelector();
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x000733F4 File Offset: 0x000715F4
		public void ExecuteDone()
		{
			this.ApplyAllChanges();
			this._closeCharacterDeveloper();
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x00073408 File Offset: 0x00071608
		public void ExecuteCancel()
		{
			foreach (CharacterDeveloperHeroItemVM characterDeveloperHeroItemVM in this._heroList)
			{
				characterDeveloperHeroItemVM.ResetChanges(true);
			}
			this._closeCharacterDeveloper();
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x00073464 File Offset: 0x00071664
		private void SetCurrentHero(CharacterDeveloperHeroItemVM currentHero)
		{
			CharacterDeveloperVM.<>c__DisplayClass18_0 CS$<>8__locals1 = new CharacterDeveloperVM.<>c__DisplayClass18_0();
			CharacterDeveloperVM.<>c__DisplayClass18_0 CS$<>8__locals2 = CS$<>8__locals1;
			CharacterDeveloperHeroItemVM currentCharacter = this.CurrentCharacter;
			SkillObject skillObject;
			if (currentCharacter == null)
			{
				skillObject = null;
			}
			else
			{
				SkillVM skillVM = currentCharacter.Skills.FirstOrDefault<SkillVM>((SkillVM s) => s.IsInspected);
				skillObject = ((skillVM != null) ? skillVM.Skill : null);
			}
			CS$<>8__locals2.prevSkill = skillObject;
			this.CurrentCharacter = currentHero;
			if (CS$<>8__locals1.prevSkill != null)
			{
				CharacterDeveloperHeroItemVM currentCharacter2 = this.CurrentCharacter;
				if (currentCharacter2 == null)
				{
					return;
				}
				currentCharacter2.SetCurrentSkill(this.CurrentCharacter.Skills.FirstOrDefault<SkillVM>((SkillVM s) => s.Skill == CS$<>8__locals1.prevSkill));
			}
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x000734FC File Offset: 0x000716FC
		public void ApplyAllChanges()
		{
			foreach (CharacterDeveloperHeroItemVM characterDeveloperHeroItemVM in this._heroList)
			{
				characterDeveloperHeroItemVM.ApplyChanges();
			}
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x0007354C File Offset: 0x0007174C
		public bool IsThereAnyChanges()
		{
			return this._heroList.Any<CharacterDeveloperHeroItemVM>((CharacterDeveloperHeroItemVM c) => c.IsThereAnyChanges());
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x00073578 File Offset: 0x00071778
		private List<Hero> GetApplicableHeroes()
		{
			List<Hero> list = new List<Hero>();
			Func<Hero, bool> func = (Hero x) => x != null && x.HeroState != Hero.CharacterStates.Disabled && x.IsAlive && !x.IsChild;
			Clan playerClan = Clan.PlayerClan;
			IEnumerable<Hero> enumerable = ((playerClan != null) ? playerClan.Heroes : null);
			foreach (Hero hero in (enumerable ?? Enumerable.Empty<Hero>()))
			{
				if (func(hero))
				{
					list.Add(hero);
				}
			}
			Clan playerClan2 = Clan.PlayerClan;
			enumerable = ((playerClan2 != null) ? playerClan2.Companions : null);
			foreach (Hero hero2 in (enumerable ?? Enumerable.Empty<Hero>()))
			{
				if (func(hero2) && !list.Contains(hero2))
				{
					list.Add(hero2);
				}
			}
			return list;
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x00073678 File Offset: 0x00071878
		private void OnTutorialNotificationElementIDChange(TutorialNotificationElementChangeEvent obj)
		{
			if (obj.NewNotificationElementID != this._latestTutorialElementID)
			{
				if (this._latestTutorialElementID != null)
				{
					this.TutorialNotification.ElementID = string.Empty;
					if (this._isActivePerkHighlightsApplied)
					{
						this.SetAvailablePerksHighlightState(false);
						this._isActivePerkHighlightsApplied = false;
					}
				}
				this._latestTutorialElementID = obj.NewNotificationElementID;
				if (this._latestTutorialElementID != null)
				{
					this.TutorialNotification.ElementID = this._latestTutorialElementID;
					if (!this._isActivePerkHighlightsApplied && this._latestTutorialElementID == this._availablePerksHighlighId)
					{
						this.SetAvailablePerksHighlightState(true);
						this._isActivePerkHighlightsApplied = true;
						SkillVM skillVM = this.CurrentCharacter.Skills.FirstOrDefault<SkillVM>((SkillVM s) => s.NumOfUnopenedPerks > 0);
						if (skillVM == null)
						{
							return;
						}
						skillVM.ExecuteInspect();
					}
				}
			}
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x00073750 File Offset: 0x00071950
		private void SetAvailablePerksHighlightState(bool state)
		{
			foreach (SkillVM skillVM in this.CurrentCharacter.Skills)
			{
				foreach (PerkVM perkVM in skillVM.Perks)
				{
					if (state && perkVM.CurrentState == PerkVM.PerkStates.EarnedButNotSelected)
					{
						perkVM.IsTutorialHighlightEnabled = true;
					}
					else if (!state)
					{
						perkVM.IsTutorialHighlightEnabled = false;
					}
				}
			}
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x000737F0 File Offset: 0x000719F0
		public override void OnFinalize()
		{
			base.OnFinalize();
			Game.Current.EventManager.UnregisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
			this.CancelInputKey.OnFinalize();
			this.DoneInputKey.OnFinalize();
			this.PreviousCharacterInputKey.OnFinalize();
			this.NextCharacterInputKey.OnFinalize();
			this._heroList.ForEach(delegate(CharacterDeveloperHeroItemVM h)
			{
				h.OnFinalize();
			});
		}

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06001FD9 RID: 8153 RVA: 0x00073874 File Offset: 0x00071A74
		// (set) Token: 0x06001FDA RID: 8154 RVA: 0x0007387C File Offset: 0x00071A7C
		[DataSourceProperty]
		public string CurrentCharacterNameText
		{
			get
			{
				return this._currentCharacterNameText;
			}
			set
			{
				if (value != this._currentCharacterNameText)
				{
					this._currentCharacterNameText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentCharacterNameText");
				}
			}
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06001FDB RID: 8155 RVA: 0x0007389F File Offset: 0x00071A9F
		// (set) Token: 0x06001FDC RID: 8156 RVA: 0x000738A8 File Offset: 0x00071AA8
		[DataSourceProperty]
		public CharacterDeveloperHeroItemVM CurrentCharacter
		{
			get
			{
				return this._currentCharacter;
			}
			set
			{
				if (value != this._currentCharacter)
				{
					if (this._currentCharacter != null)
					{
						if (this._currentCharacter.IsInspectingAnAttribute)
						{
							this._currentCharacter.ExecuteStopInspectingCurrentAttribute();
						}
						if (this._currentCharacter.PerkSelection.IsActive)
						{
							this._currentCharacter.PerkSelection.ExecuteDeactivate();
						}
					}
					this._currentCharacter = value;
					CharacterDeveloperHeroItemVM currentCharacter = this._currentCharacter;
					this.CurrentCharacterNameText = ((currentCharacter != null) ? currentCharacter.HeroNameText : null) ?? string.Empty;
					base.OnPropertyChangedWithValue<CharacterDeveloperHeroItemVM>(value, "CurrentCharacter");
				}
			}
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06001FDD RID: 8157 RVA: 0x00073934 File Offset: 0x00071B34
		// (set) Token: 0x06001FDE RID: 8158 RVA: 0x0007393C File Offset: 0x00071B3C
		[DataSourceProperty]
		public SelectorVM<SelectorItemVM> CharacterList
		{
			get
			{
				return this._characterList;
			}
			set
			{
				if (value != this._characterList)
				{
					this._characterList = value;
					base.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(value, "CharacterList");
				}
			}
		}

		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06001FDF RID: 8159 RVA: 0x0007395A File Offset: 0x00071B5A
		// (set) Token: 0x06001FE0 RID: 8160 RVA: 0x00073962 File Offset: 0x00071B62
		[DataSourceProperty]
		public HintViewModel FocusVisualHint
		{
			get
			{
				return this._focusVisualHint;
			}
			set
			{
				if (value != this._focusVisualHint)
				{
					this._focusVisualHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "FocusVisualHint");
				}
			}
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06001FE1 RID: 8161 RVA: 0x00073980 File Offset: 0x00071B80
		// (set) Token: 0x06001FE2 RID: 8162 RVA: 0x00073988 File Offset: 0x00071B88
		[DataSourceProperty]
		public HintViewModel ResetHint
		{
			get
			{
				return this._resetHint;
			}
			set
			{
				if (value != this._resetHint)
				{
					this._resetHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ResetHint");
				}
			}
		}

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06001FE3 RID: 8163 RVA: 0x000739A6 File Offset: 0x00071BA6
		// (set) Token: 0x06001FE4 RID: 8164 RVA: 0x000739AE File Offset: 0x00071BAE
		[DataSourceProperty]
		public ElementNotificationVM TutorialNotification
		{
			get
			{
				return this._tutorialNotification;
			}
			set
			{
				if (value != this._tutorialNotification)
				{
					this._tutorialNotification = value;
					base.OnPropertyChangedWithValue<ElementNotificationVM>(value, "TutorialNotification");
				}
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06001FE5 RID: 8165 RVA: 0x000739CC File Offset: 0x00071BCC
		// (set) Token: 0x06001FE6 RID: 8166 RVA: 0x000739D4 File Offset: 0x00071BD4
		[DataSourceProperty]
		public bool IsPlayerAccompanied
		{
			get
			{
				return this._isPlayerAccompanied;
			}
			set
			{
				if (value != this._isPlayerAccompanied)
				{
					this._isPlayerAccompanied = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerAccompanied");
				}
			}
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06001FE7 RID: 8167 RVA: 0x000739F2 File Offset: 0x00071BF2
		// (set) Token: 0x06001FE8 RID: 8168 RVA: 0x000739FA File Offset: 0x00071BFA
		[DataSourceProperty]
		public string UnspentCharacterPointsText
		{
			get
			{
				return this._unspentFocusPointsText;
			}
			set
			{
				if (value != this._unspentFocusPointsText)
				{
					this._unspentFocusPointsText = value;
					base.OnPropertyChangedWithValue<string>(value, "UnspentCharacterPointsText");
				}
			}
		}

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06001FE9 RID: 8169 RVA: 0x00073A1D File Offset: 0x00071C1D
		// (set) Token: 0x06001FEA RID: 8170 RVA: 0x00073A25 File Offset: 0x00071C25
		[DataSourceProperty]
		public string TraitsText
		{
			get
			{
				return this._traitsText;
			}
			set
			{
				if (value != this._traitsText)
				{
					this._traitsText = value;
					base.OnPropertyChangedWithValue<string>(value, "TraitsText");
				}
			}
		}

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06001FEB RID: 8171 RVA: 0x00073A48 File Offset: 0x00071C48
		// (set) Token: 0x06001FEC RID: 8172 RVA: 0x00073A50 File Offset: 0x00071C50
		[DataSourceProperty]
		public string PartyRoleText
		{
			get
			{
				return this._partyRoleText;
			}
			set
			{
				if (value != this._partyRoleText)
				{
					this._partyRoleText = value;
					base.OnPropertyChangedWithValue<string>(value, "PartyRoleText");
				}
			}
		}

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06001FED RID: 8173 RVA: 0x00073A73 File Offset: 0x00071C73
		// (set) Token: 0x06001FEE RID: 8174 RVA: 0x00073A7B File Offset: 0x00071C7B
		[DataSourceProperty]
		public HintViewModel UnspentCharacterPointsHint
		{
			get
			{
				return this._unspentCharacterPointsHint;
			}
			set
			{
				if (value != this._unspentCharacterPointsHint)
				{
					this._unspentCharacterPointsHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "UnspentCharacterPointsHint");
				}
			}
		}

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06001FEF RID: 8175 RVA: 0x00073A99 File Offset: 0x00071C99
		// (set) Token: 0x06001FF0 RID: 8176 RVA: 0x00073AA1 File Offset: 0x00071CA1
		[DataSourceProperty]
		public HintViewModel UnspentAttributePointsHint
		{
			get
			{
				return this._unspentAttributePointsHint;
			}
			set
			{
				if (value != this._unspentAttributePointsHint)
				{
					this._unspentAttributePointsHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "UnspentAttributePointsHint");
				}
			}
		}

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06001FF1 RID: 8177 RVA: 0x00073ABF File Offset: 0x00071CBF
		// (set) Token: 0x06001FF2 RID: 8178 RVA: 0x00073AC7 File Offset: 0x00071CC7
		[DataSourceProperty]
		public HintViewModel LevelHint
		{
			get
			{
				return this._levelHint;
			}
			set
			{
				if (value != this._levelHint)
				{
					this._levelHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "LevelHint");
				}
			}
		}

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x06001FF3 RID: 8179 RVA: 0x00073AE5 File Offset: 0x00071CE5
		// (set) Token: 0x06001FF4 RID: 8180 RVA: 0x00073AED File Offset: 0x00071CED
		[DataSourceProperty]
		public HintViewModel UnopenedPerksHint
		{
			get
			{
				return this._unopenedPerksHint;
			}
			set
			{
				if (value != this._unopenedPerksHint)
				{
					this._unopenedPerksHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "UnopenedPerksHint");
				}
			}
		}

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06001FF5 RID: 8181 RVA: 0x00073B0B File Offset: 0x00071D0B
		// (set) Token: 0x06001FF6 RID: 8182 RVA: 0x00073B13 File Offset: 0x00071D13
		[DataSourceProperty]
		public BasicTooltipViewModel PreviousCharacterHint
		{
			get
			{
				return this._previousCharacterHint;
			}
			set
			{
				if (value != this._previousCharacterHint)
				{
					this._previousCharacterHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "PreviousCharacterHint");
				}
			}
		}

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06001FF7 RID: 8183 RVA: 0x00073B31 File Offset: 0x00071D31
		// (set) Token: 0x06001FF8 RID: 8184 RVA: 0x00073B39 File Offset: 0x00071D39
		[DataSourceProperty]
		public BasicTooltipViewModel NextCharacterHint
		{
			get
			{
				return this._nextCharacterHint;
			}
			set
			{
				if (value != this._nextCharacterHint)
				{
					this._nextCharacterHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "NextCharacterHint");
				}
			}
		}

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x06001FF9 RID: 8185 RVA: 0x00073B57 File Offset: 0x00071D57
		// (set) Token: 0x06001FFA RID: 8186 RVA: 0x00073B5F File Offset: 0x00071D5F
		[DataSourceProperty]
		public string DoneLbl
		{
			get
			{
				return this._doneLbl;
			}
			set
			{
				if (value != this._doneLbl)
				{
					this._doneLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneLbl");
				}
			}
		}

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x06001FFB RID: 8187 RVA: 0x00073B82 File Offset: 0x00071D82
		// (set) Token: 0x06001FFC RID: 8188 RVA: 0x00073B8A File Offset: 0x00071D8A
		[DataSourceProperty]
		public string ResetLbl
		{
			get
			{
				return this._resetLbl;
			}
			set
			{
				if (value != this._resetLbl)
				{
					this._resetLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "ResetLbl");
				}
			}
		}

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x06001FFD RID: 8189 RVA: 0x00073BAD File Offset: 0x00071DAD
		// (set) Token: 0x06001FFE RID: 8190 RVA: 0x00073BB5 File Offset: 0x00071DB5
		[DataSourceProperty]
		public string CancelLbl
		{
			get
			{
				return this._cancelLbl;
			}
			set
			{
				if (value != this._cancelLbl)
				{
					this._cancelLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelLbl");
				}
			}
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06001FFF RID: 8191 RVA: 0x00073BD8 File Offset: 0x00071DD8
		// (set) Token: 0x06002000 RID: 8192 RVA: 0x00073BE0 File Offset: 0x00071DE0
		[DataSourceProperty]
		public string SkillFocusText
		{
			get
			{
				return this._skillFocusText;
			}
			set
			{
				if (value != this._skillFocusText)
				{
					this._skillFocusText = value;
					base.OnPropertyChangedWithValue<string>(value, "SkillFocusText");
				}
			}
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x06002001 RID: 8193 RVA: 0x00073C03 File Offset: 0x00071E03
		// (set) Token: 0x06002002 RID: 8194 RVA: 0x00073C0B File Offset: 0x00071E0B
		[DataSourceProperty]
		public string AddFocusText
		{
			get
			{
				return this._addFocusText;
			}
			set
			{
				if (value != this._addFocusText)
				{
					this._addFocusText = value;
					base.OnPropertyChangedWithValue<string>(value, "AddFocusText");
				}
			}
		}

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06002003 RID: 8195 RVA: 0x00073C2E File Offset: 0x00071E2E
		// (set) Token: 0x06002004 RID: 8196 RVA: 0x00073C36 File Offset: 0x00071E36
		[DataSourceProperty]
		public string SkillsText
		{
			get
			{
				return this._skillsText;
			}
			set
			{
				if (value != this._skillsText)
				{
					this._skillsText = value;
					base.OnPropertyChangedWithValue<string>(value, "SkillsText");
				}
			}
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06002005 RID: 8197 RVA: 0x00073C59 File Offset: 0x00071E59
		// (set) Token: 0x06002006 RID: 8198 RVA: 0x00073C61 File Offset: 0x00071E61
		[DataSourceProperty]
		public int UnopenedPerksNumForCurrentCharacter
		{
			get
			{
				return this._unopenedPerksNumForCurrentCharacter;
			}
			set
			{
				if (value != this._unopenedPerksNumForCurrentCharacter)
				{
					this._unopenedPerksNumForCurrentCharacter = value;
					base.OnPropertyChangedWithValue(value, "UnopenedPerksNumForCurrentCharacter");
				}
			}
		}

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06002007 RID: 8199 RVA: 0x00073C7F File Offset: 0x00071E7F
		// (set) Token: 0x06002008 RID: 8200 RVA: 0x00073C87 File Offset: 0x00071E87
		[DataSourceProperty]
		public bool HasUnopenedPerksForCurrentCharacter
		{
			get
			{
				return this._hasUnopenedPerksForCurrentCharacter;
			}
			set
			{
				if (value != this._hasUnopenedPerksForCurrentCharacter)
				{
					this._hasUnopenedPerksForCurrentCharacter = value;
					base.OnPropertyChangedWithValue(value, "HasUnopenedPerksForCurrentCharacter");
				}
			}
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x00073CA5 File Offset: 0x00071EA5
		private TextObject GetPreviousCharacterKeyText()
		{
			if (this.PreviousCharacterInputKey == null || this._getKeyTextFromKeyId == null)
			{
				return TextObject.GetEmpty();
			}
			return this._getKeyTextFromKeyId(this.PreviousCharacterInputKey.KeyID);
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x00073CD3 File Offset: 0x00071ED3
		private TextObject GetNextCharacterKeyText()
		{
			if (this.NextCharacterInputKey == null || this._getKeyTextFromKeyId == null)
			{
				return TextObject.GetEmpty();
			}
			return this._getKeyTextFromKeyId(this.NextCharacterInputKey.KeyID);
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x00073D01 File Offset: 0x00071F01
		public void SetCancelInputKey(HotKey gameKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(gameKey, true);
		}

		// Token: 0x0600200C RID: 8204 RVA: 0x00073D10 File Offset: 0x00071F10
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x00073D1F File Offset: 0x00071F1F
		public void SetResetInputKey(HotKey hotKey)
		{
			this.ResetInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x0600200E RID: 8206 RVA: 0x00073D2E File Offset: 0x00071F2E
		public void SetPreviousCharacterInputKey(HotKey hotKey)
		{
			this.PreviousCharacterInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
			this.SetPreviousCharacterHint();
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x00073D43 File Offset: 0x00071F43
		public void SetNextCharacterInputKey(HotKey hotKey)
		{
			this.NextCharacterInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
			this.SetNextCharacterHint();
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x00073D58 File Offset: 0x00071F58
		public void SetGetKeyTextFromKeyIDFunc(Func<string, TextObject> getKeyTextFromKeyId)
		{
			this._getKeyTextFromKeyId = getKeyTextFromKeyId;
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x06002011 RID: 8209 RVA: 0x00073D61 File Offset: 0x00071F61
		// (set) Token: 0x06002012 RID: 8210 RVA: 0x00073D69 File Offset: 0x00071F69
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x06002013 RID: 8211 RVA: 0x00073D87 File Offset: 0x00071F87
		// (set) Token: 0x06002014 RID: 8212 RVA: 0x00073D8F File Offset: 0x00071F8F
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06002015 RID: 8213 RVA: 0x00073DAD File Offset: 0x00071FAD
		// (set) Token: 0x06002016 RID: 8214 RVA: 0x00073DB5 File Offset: 0x00071FB5
		[DataSourceProperty]
		public InputKeyItemVM ResetInputKey
		{
			get
			{
				return this._resetInputKey;
			}
			set
			{
				if (value != this._resetInputKey)
				{
					this._resetInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ResetInputKey");
				}
			}
		}

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06002017 RID: 8215 RVA: 0x00073DD3 File Offset: 0x00071FD3
		// (set) Token: 0x06002018 RID: 8216 RVA: 0x00073DDB File Offset: 0x00071FDB
		[DataSourceProperty]
		public InputKeyItemVM PreviousCharacterInputKey
		{
			get
			{
				return this._previousCharacterInputKey;
			}
			set
			{
				if (value != this._previousCharacterInputKey)
				{
					this._previousCharacterInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "PreviousCharacterInputKey");
				}
			}
		}

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06002019 RID: 8217 RVA: 0x00073DF9 File Offset: 0x00071FF9
		// (set) Token: 0x0600201A RID: 8218 RVA: 0x00073E01 File Offset: 0x00072001
		[DataSourceProperty]
		public InputKeyItemVM NextCharacterInputKey
		{
			get
			{
				return this._nextCharacterInputKey;
			}
			set
			{
				if (value != this._nextCharacterInputKey)
				{
					this._nextCharacterInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "NextCharacterInputKey");
				}
			}
		}

		// Token: 0x04000E8D RID: 3725
		private readonly Action _closeCharacterDeveloper;

		// Token: 0x04000E8E RID: 3726
		private readonly List<CharacterDeveloperHeroItemVM> _heroList;

		// Token: 0x04000E8F RID: 3727
		private readonly IViewDataTracker _viewDataTracker;

		// Token: 0x04000E90 RID: 3728
		public readonly ReadOnlyCollection<CharacterDeveloperHeroItemVM> HeroList;

		// Token: 0x04000E91 RID: 3729
		private int _heroIndex;

		// Token: 0x04000E92 RID: 3730
		private string _latestTutorialElementID;

		// Token: 0x04000E93 RID: 3731
		private Func<string, TextObject> _getKeyTextFromKeyId;

		// Token: 0x04000E94 RID: 3732
		private bool _isActivePerkHighlightsApplied;

		// Token: 0x04000E95 RID: 3733
		private readonly string _availablePerksHighlighId = "AvailablePerks";

		// Token: 0x04000E96 RID: 3734
		private string _skillsText;

		// Token: 0x04000E97 RID: 3735
		private string _doneLbl;

		// Token: 0x04000E98 RID: 3736
		private string _resetLbl;

		// Token: 0x04000E99 RID: 3737
		private string _cancelLbl;

		// Token: 0x04000E9A RID: 3738
		private string _unspentFocusPointsText;

		// Token: 0x04000E9B RID: 3739
		private string _traitsText;

		// Token: 0x04000E9C RID: 3740
		private string _partyRoleText;

		// Token: 0x04000E9D RID: 3741
		private HintViewModel _unspentCharacterPointsHint;

		// Token: 0x04000E9E RID: 3742
		private HintViewModel _unspentAttributePointsHint;

		// Token: 0x04000E9F RID: 3743
		private HintViewModel _levelHint;

		// Token: 0x04000EA0 RID: 3744
		private HintViewModel _unopenedPerksHint;

		// Token: 0x04000EA1 RID: 3745
		private BasicTooltipViewModel _previousCharacterHint;

		// Token: 0x04000EA2 RID: 3746
		private BasicTooltipViewModel _nextCharacterHint;

		// Token: 0x04000EA3 RID: 3747
		private string _addFocusText;

		// Token: 0x04000EA4 RID: 3748
		private bool _isPlayerAccompanied;

		// Token: 0x04000EA5 RID: 3749
		private string _skillFocusText;

		// Token: 0x04000EA6 RID: 3750
		private ElementNotificationVM _tutorialNotification;

		// Token: 0x04000EA7 RID: 3751
		private HintViewModel _resetHint;

		// Token: 0x04000EA8 RID: 3752
		private HintViewModel _focusVisualHint;

		// Token: 0x04000EA9 RID: 3753
		private CharacterDeveloperHeroItemVM _currentCharacter;

		// Token: 0x04000EAA RID: 3754
		private string _currentCharacterNameText;

		// Token: 0x04000EAB RID: 3755
		private SelectorVM<SelectorItemVM> _characterList;

		// Token: 0x04000EAC RID: 3756
		private int _unopenedPerksNumForCurrentCharacter;

		// Token: 0x04000EAD RID: 3757
		private bool _hasUnopenedPerksForCurrentCharacter;

		// Token: 0x04000EAE RID: 3758
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000EAF RID: 3759
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000EB0 RID: 3760
		private InputKeyItemVM _resetInputKey;

		// Token: 0x04000EB1 RID: 3761
		private InputKeyItemVM _previousCharacterInputKey;

		// Token: 0x04000EB2 RID: 3762
		private InputKeyItemVM _nextCharacterInputKey;
	}
}
