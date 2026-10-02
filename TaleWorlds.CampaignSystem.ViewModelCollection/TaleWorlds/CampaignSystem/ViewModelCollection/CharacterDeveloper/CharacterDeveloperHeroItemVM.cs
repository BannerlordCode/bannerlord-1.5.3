using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper.PerkSelection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper
{
	// Token: 0x02000148 RID: 328
	public class CharacterDeveloperHeroItemVM : ViewModel
	{
		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x06001F80 RID: 8064 RVA: 0x0007200B File Offset: 0x0007020B
		public HeroDeveloper HeroDeveloper
		{
			get
			{
				return this.Hero.HeroDeveloper;
			}
		}

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x06001F81 RID: 8065 RVA: 0x00072018 File Offset: 0x00070218
		// (set) Token: 0x06001F82 RID: 8066 RVA: 0x00072020 File Offset: 0x00070220
		public Hero Hero { get; private set; }

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x06001F83 RID: 8067 RVA: 0x00072029 File Offset: 0x00070229
		// (set) Token: 0x06001F84 RID: 8068 RVA: 0x00072031 File Offset: 0x00070231
		public int OrgUnspentFocusPoints { get; private set; }

		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x06001F85 RID: 8069 RVA: 0x0007203A File Offset: 0x0007023A
		// (set) Token: 0x06001F86 RID: 8070 RVA: 0x00072042 File Offset: 0x00070242
		public int OrgUnspentAttributePoints { get; private set; }

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06001F87 RID: 8071 RVA: 0x0007204B File Offset: 0x0007024B
		public IReadOnlyPropertyOwner<CharacterAttribute> CharacterAttributes
		{
			get
			{
				return this._characterAttributes;
			}
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x00072054 File Offset: 0x00070254
		public CharacterDeveloperHeroItemVM(Hero hero, Action onPerkSelection)
		{
			this.LevelHint = new HintViewModel();
			this.Hero = hero;
			this.OrgUnspentFocusPoints = this.HeroDeveloper.UnspentFocusPoints;
			this.UnspentCharacterPoints = this.OrgUnspentFocusPoints;
			this.OrgUnspentAttributePoints = this.HeroDeveloper.UnspentAttributePoints;
			this.UnspentAttributePoints = this.OrgUnspentAttributePoints;
			this.Attributes = new MBBindingList<CharacterAttributeItemVM>();
			this._characterAttributes = new PropertyOwner<CharacterAttribute>();
			this.PerkSelection = new PerkSelectionVM(this.HeroDeveloper, new Action<SkillObject>(this.RefreshPerksOfSkill), onPerkSelection);
			this.InitializeCharacter();
			this.RefreshValues();
		}

		// Token: 0x06001F89 RID: 8073 RVA: 0x000720F4 File Offset: 0x000702F4
		public override void RefreshValues()
		{
			base.RefreshValues();
			StringHelpers.SetCharacterProperties("HERO", this.Hero.CharacterObject, null, false);
			this.HeroNameText = this.Hero.CharacterObject.Name.ToString();
			MBTextManager.SetTextVariable("LEVEL", this.Hero.CharacterObject.Level + 1);
			this.HeroNextLevelText = GameTexts.FindText("str_level_with_value", null).ToString();
			this.HeroInfoText = GameTexts.FindText("str_hero_name_level", null).ToString();
			this.FocusPointsText = GameTexts.FindText("str_focus_points", null).ToString();
			this.InitializeCharacter();
			this.Skills.ApplyActionOnAllItems(delegate(SkillVM x)
			{
				x.RefreshValues();
			});
			this.CurrentSkill.RefreshValues();
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x000721D4 File Offset: 0x000703D4
		private void InitializeCharacter()
		{
			this.HeroCharacter = new HeroViewModel(CharacterViewModel.StanceTypes.None);
			this.Skills = new MBBindingList<SkillVM>();
			this.Traits = new MBBindingList<EncyclopediaTraitItemVM>();
			this.Attributes.Clear();
			this.HeroCharacter.FillFrom(this.Hero, -1, false, false);
			this.HeroCharacter.SetEquipment(EquipmentIndex.ArmorItemEndSlot, default(EquipmentElement));
			this.HeroCharacter.SetEquipment(EquipmentIndex.HorseHarness, default(EquipmentElement));
			this.HeroCharacter.SetEquipment(EquipmentIndex.NumAllWeaponSlots, default(EquipmentElement));
			List<CharacterAttribute> list = TaleWorlds.CampaignSystem.Extensions.Attributes.All.ToList<CharacterAttribute>();
			list.Sort(CampaignUIHelper.CharacterAttributeComparerInstance);
			foreach (CharacterAttribute characterAttribute in list)
			{
				this._characterAttributes.SetPropertyValue(characterAttribute, this.Hero.GetAttributeValue(characterAttribute));
				this.Attributes.Add(new CharacterAttributeItemVM(this.Hero, characterAttribute, this, new Action<CharacterAttributeItemVM>(this.OnInspectAttribute), new Action<CharacterAttributeItemVM>(this.OnAddAttributePoint)));
			}
			List<SkillObject> list2 = TaleWorlds.CampaignSystem.Extensions.Skills.All.ToList<SkillObject>();
			list2.Sort(CampaignUIHelper.SkillObjectComparerInstance);
			foreach (SkillObject skillObject in list2)
			{
				this.Skills.Add(new SkillVM(skillObject, this, new Action<PerkVM>(this.OnStartPerkSelection)));
			}
			this.HasExtraSkills = this.Skills.Count > 18;
			foreach (SkillVM skillVM in this.Skills)
			{
				skillVM.RefreshWithCurrentValues();
			}
			foreach (CharacterAttributeItemVM characterAttributeItemVM in this.Attributes)
			{
				characterAttributeItemVM.RefreshWithCurrentValues();
			}
			this.SetCurrentSkill(this.Skills[0]);
			this.RefreshCharacterValues();
			this.CharacterStats = new MBBindingList<StringPairItemVM>();
			if (this.Hero.GovernorOf != null)
			{
				GameTexts.SetVariable("SETTLEMENT_NAME", this.Hero.GovernorOf.Name.ToString());
				this.CharacterStats.Add(new StringPairItemVM(GameTexts.FindText("str_governor_of_label", null).ToString(), "", null));
			}
			if (MobileParty.MainParty.GetHeroPartyRoles(this.Hero).Count > 0)
			{
				this.CharacterStats.Add(new StringPairItemVM(CampaignUIHelper.GetHeroClanRoleText(this.Hero, Clan.PlayerClan), "", null));
			}
			foreach (TraitObject traitObject in CampaignUIHelper.GetHeroTraits())
			{
				if (this.Hero.GetTraitLevel(traitObject) != 0)
				{
					this.Traits.Add(new EncyclopediaTraitItemVM(traitObject, this.Hero));
				}
			}
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x00072508 File Offset: 0x00070708
		private void OnInspectAttribute(CharacterAttributeItemVM att)
		{
			this.CurrentInspectedAttribute = att;
			this.IsInspectingAnAttribute = true;
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x00072518 File Offset: 0x00070718
		private void OnAddAttributePoint(CharacterAttributeItemVM att)
		{
			int unspentAttributePoints = this.UnspentAttributePoints;
			this.UnspentAttributePoints = unspentAttributePoints - 1;
			this._characterAttributes.SetPropertyValue(att.AttributeType, this._characterAttributes.GetPropertyValue(att.AttributeType) + 1);
			this.RefreshCharacterValues();
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x0007255F File Offset: 0x0007075F
		public void ExecuteStopInspectingCurrentAttribute()
		{
			this.IsInspectingAnAttribute = false;
			this.CurrentInspectedAttribute = null;
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x00072570 File Offset: 0x00070770
		public void RefreshCharacterValues()
		{
			this.CurrentCharacterLevelLbl = this.Hero.Level.ToString();
			this.CurrentTotalXp = this.HeroDeveloper.TotalXp;
			this.XpRequiredForNextLevel = Campaign.Current.Models.CharacterDevelopmentModel.SkillsRequiredForLevel(this.Hero.Level + 1);
			GameTexts.SetVariable("CURRENTAMOUNT", this.CurrentTotalXp);
			GameTexts.SetVariable("TARGETAMOUNT", this.XpRequiredForNextLevel);
			this.LevelProgressText = GameTexts.FindText("str_character_skillpoint_progress", null).ToString();
			GameTexts.SetVariable("newline", "\n");
			GameTexts.SetVariable("CURRENT_SKILL_POINTS", this.CurrentTotalXp);
			GameTexts.SetVariable("STR1", GameTexts.FindText("str_total_skill_points", null));
			GameTexts.SetVariable("NEXT_SKILL_POINTS", this.XpRequiredForNextLevel);
			GameTexts.SetVariable("STR2", GameTexts.FindText("str_next_level_at", null));
			string text = GameTexts.FindText("str_string_newline_string", null).ToString();
			GameTexts.SetVariable("SKILL_LEVEL_FOR_LEVEL_UP", this.XpRequiredForNextLevel - this.CurrentTotalXp);
			GameTexts.SetVariable("STR1", text);
			GameTexts.SetVariable("STR2", GameTexts.FindText("str_how_to_level_up_character", null));
			string text2 = GameTexts.FindText("str_string_newline_string", null).ToString();
			this.LevelHint.HintText = new TextObject("{=!}" + text2, null);
			foreach (SkillVM skillVM in this.Skills)
			{
				skillVM.RefreshWithCurrentValues();
			}
			foreach (CharacterAttributeItemVM characterAttributeItemVM in this.Attributes)
			{
				characterAttributeItemVM.RefreshWithCurrentValues();
			}
		}

		// Token: 0x06001F8F RID: 8079 RVA: 0x00072748 File Offset: 0x00070948
		public void RefreshPerksOfSkill(SkillObject skill)
		{
			SkillVM skillVM = this.Skills.SingleOrDefault<SkillVM>((SkillVM s) => s.Skill == skill);
			if (skillVM == null)
			{
				return;
			}
			skillVM.RefreshLists(null);
		}

		// Token: 0x06001F90 RID: 8080 RVA: 0x00072784 File Offset: 0x00070984
		public void ResetChanges(bool isCancel)
		{
			this.PerkSelection.ResetSelectedPerks();
			foreach (CharacterAttribute characterAttribute in TaleWorlds.CampaignSystem.Extensions.Attributes.All)
			{
				this._characterAttributes.SetPropertyValue(characterAttribute, this.Hero.GetAttributeValue(characterAttribute));
			}
			if (!isCancel)
			{
				this.UnspentCharacterPoints = this.OrgUnspentFocusPoints;
				this.UnspentAttributePoints = this.OrgUnspentAttributePoints;
			}
			foreach (CharacterAttributeItemVM characterAttributeItemVM in this.Attributes)
			{
				characterAttributeItemVM.Reset();
			}
			if (!isCancel)
			{
				foreach (CharacterAttributeItemVM characterAttributeItemVM2 in this.Attributes)
				{
					characterAttributeItemVM2.RefreshWithCurrentValues();
				}
			}
			foreach (SkillVM skillVM in this.Skills)
			{
				skillVM.ResetChanges();
			}
			if (!isCancel)
			{
				foreach (SkillVM skillVM2 in this.Skills)
				{
					skillVM2.RefreshWithCurrentValues();
				}
			}
		}

		// Token: 0x06001F91 RID: 8081 RVA: 0x000728F8 File Offset: 0x00070AF8
		public void ApplyChanges()
		{
			this.PerkSelection.ApplySelectedPerks();
			foreach (CharacterAttributeItemVM characterAttributeItemVM in this.Attributes)
			{
				characterAttributeItemVM.Commit();
			}
			foreach (SkillVM skillVM in this.Skills)
			{
				skillVM.ApplyChanges();
			}
		}

		// Token: 0x06001F92 RID: 8082 RVA: 0x00072988 File Offset: 0x00070B88
		public void SetCurrentSkill(SkillVM skill)
		{
			if (this.CurrentSkill != null)
			{
				this.CurrentSkill.IsInspected = false;
			}
			this.CurrentSkill = skill;
			this.CurrentSkill.IsInspected = true;
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x000729B4 File Offset: 0x00070BB4
		public bool IsThereAnyChanges()
		{
			bool flag = this.Skills.Any<SkillVM>((SkillVM s) => s.IsThereAnyChanges());
			return this.UnspentCharacterPoints != this.OrgUnspentFocusPoints || this.UnspentAttributePoints != this.OrgUnspentAttributePoints || this.PerkSelection.IsAnyPerkSelected() || flag;
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x00072A18 File Offset: 0x00070C18
		public int GetRequiredFocusPointsToAddFocusWithCurrentFocus(SkillObject skill)
		{
			return this.Hero.HeroDeveloper.GetRequiredFocusPointsToAddFocus(skill);
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x00072A2B File Offset: 0x00070C2B
		public bool CanAddFocusToSkillWithFocusAmount(int currentFocusAmount)
		{
			return currentFocusAmount < Campaign.Current.Models.CharacterDevelopmentModel.MaxFocusPerSkill && this.UnspentCharacterPoints > 0;
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x00072A50 File Offset: 0x00070C50
		public bool IsSkillMaxAmongOtherSkills(SkillVM skill)
		{
			if (this.Skills.Count > 0)
			{
				int currentFocusLevel = skill.CurrentFocusLevel;
				return this.Skills.Max<SkillVM>((SkillVM s) => s.CurrentFocusLevel) <= currentFocusLevel;
			}
			return false;
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x00072AA4 File Offset: 0x00070CA4
		public string GetNameWithNumOfUnopenedPerks()
		{
			if (this.Skills.Sum<SkillVM>((SkillVM s) => s.NumOfUnopenedPerks) == 0)
			{
				return this.HeroNameText;
			}
			GameTexts.SetVariable("STR1", "{=!}<img src=\"CharacterDeveloper\\UnselectedPerksIcon\" extend=\"2\">");
			GameTexts.SetVariable("STR2", this.HeroNameText);
			return GameTexts.FindText("str_STR1_space_STR2", null).ToString();
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x00072B13 File Offset: 0x00070D13
		private void OnStartPerkSelection(PerkVM perk)
		{
			this.PerkSelection.SetCurrentSelectionPerk(perk);
		}

		// Token: 0x06001F99 RID: 8089 RVA: 0x00072B21 File Offset: 0x00070D21
		public int GetNumberOfUnselectedPerks()
		{
			return this.Skills.Sum<SkillVM>((SkillVM s) => s.NumOfUnopenedPerks);
		}

		// Token: 0x06001F9A RID: 8090 RVA: 0x00072B4D File Offset: 0x00070D4D
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.HeroCharacter.OnFinalize();
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06001F9B RID: 8091 RVA: 0x00072B60 File Offset: 0x00070D60
		// (set) Token: 0x06001F9C RID: 8092 RVA: 0x00072B68 File Offset: 0x00070D68
		[DataSourceProperty]
		public MBBindingList<SkillVM> Skills
		{
			get
			{
				return this._skills;
			}
			set
			{
				if (value != this._skills)
				{
					this._skills = value;
					base.OnPropertyChangedWithValue<MBBindingList<SkillVM>>(value, "Skills");
				}
			}
		}

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x06001F9D RID: 8093 RVA: 0x00072B86 File Offset: 0x00070D86
		// (set) Token: 0x06001F9E RID: 8094 RVA: 0x00072B8E File Offset: 0x00070D8E
		[DataSourceProperty]
		public MBBindingList<StringPairItemVM> CharacterStats
		{
			get
			{
				return this._characterStats;
			}
			set
			{
				if (value != this._characterStats)
				{
					this._characterStats = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemVM>>(value, "CharacterStats");
				}
			}
		}

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x06001F9F RID: 8095 RVA: 0x00072BAC File Offset: 0x00070DAC
		// (set) Token: 0x06001FA0 RID: 8096 RVA: 0x00072BB4 File Offset: 0x00070DB4
		[DataSourceProperty]
		public MBBindingList<CharacterAttributeItemVM> Attributes
		{
			get
			{
				return this._attributes;
			}
			set
			{
				if (value != this._attributes)
				{
					this._attributes = value;
					base.OnPropertyChangedWithValue<MBBindingList<CharacterAttributeItemVM>>(value, "Attributes");
				}
			}
		}

		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x06001FA1 RID: 8097 RVA: 0x00072BD2 File Offset: 0x00070DD2
		// (set) Token: 0x06001FA2 RID: 8098 RVA: 0x00072BDA File Offset: 0x00070DDA
		[DataSourceProperty]
		public MBBindingList<EncyclopediaTraitItemVM> Traits
		{
			get
			{
				return this._traits;
			}
			set
			{
				if (value != this._traits)
				{
					this._traits = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaTraitItemVM>>(value, "Traits");
				}
			}
		}

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x06001FA3 RID: 8099 RVA: 0x00072BF8 File Offset: 0x00070DF8
		// (set) Token: 0x06001FA4 RID: 8100 RVA: 0x00072C00 File Offset: 0x00070E00
		[DataSourceProperty]
		public PerkSelectionVM PerkSelection
		{
			get
			{
				return this._perkSelection;
			}
			set
			{
				if (value != this._perkSelection)
				{
					this._perkSelection = value;
					base.OnPropertyChangedWithValue<PerkSelectionVM>(value, "PerkSelection");
				}
			}
		}

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06001FA5 RID: 8101 RVA: 0x00072C1E File Offset: 0x00070E1E
		// (set) Token: 0x06001FA6 RID: 8102 RVA: 0x00072C26 File Offset: 0x00070E26
		[DataSourceProperty]
		public SkillVM CurrentSkill
		{
			get
			{
				return this._currentSkill;
			}
			set
			{
				if (value != this._currentSkill)
				{
					this._currentSkill = value;
					base.OnPropertyChangedWithValue<SkillVM>(value, "CurrentSkill");
				}
			}
		}

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06001FA7 RID: 8103 RVA: 0x00072C44 File Offset: 0x00070E44
		// (set) Token: 0x06001FA8 RID: 8104 RVA: 0x00072C4C File Offset: 0x00070E4C
		[DataSourceProperty]
		public CharacterAttributeItemVM CurrentInspectedAttribute
		{
			get
			{
				return this._currentInspectedAttribute;
			}
			set
			{
				if (value != this._currentInspectedAttribute)
				{
					this._currentInspectedAttribute = value;
					base.OnPropertyChangedWithValue<CharacterAttributeItemVM>(value, "CurrentInspectedAttribute");
				}
			}
		}

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x06001FA9 RID: 8105 RVA: 0x00072C6A File Offset: 0x00070E6A
		// (set) Token: 0x06001FAA RID: 8106 RVA: 0x00072C72 File Offset: 0x00070E72
		[DataSourceProperty]
		public string FocusPointsText
		{
			get
			{
				return this._focusPointsText;
			}
			set
			{
				if (value != this._focusPointsText)
				{
					this._focusPointsText = value;
					base.OnPropertyChangedWithValue<string>(value, "FocusPointsText");
				}
			}
		}

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x06001FAB RID: 8107 RVA: 0x00072C95 File Offset: 0x00070E95
		// (set) Token: 0x06001FAC RID: 8108 RVA: 0x00072C9D File Offset: 0x00070E9D
		[DataSourceProperty]
		public string CurrentCharacterLevelLbl
		{
			get
			{
				return this._currentCharacterLevelLbl;
			}
			set
			{
				if (value != this._currentCharacterLevelLbl)
				{
					this._currentCharacterLevelLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentCharacterLevelLbl");
				}
			}
		}

		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x06001FAD RID: 8109 RVA: 0x00072CC0 File Offset: 0x00070EC0
		// (set) Token: 0x06001FAE RID: 8110 RVA: 0x00072CC8 File Offset: 0x00070EC8
		[DataSourceProperty]
		public string LevelProgressText
		{
			get
			{
				return this._levelProgressText;
			}
			set
			{
				if (value != this._levelProgressText)
				{
					this._levelProgressText = value;
					base.OnPropertyChangedWithValue<string>(value, "LevelProgressText");
				}
			}
		}

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x06001FAF RID: 8111 RVA: 0x00072CEB File Offset: 0x00070EEB
		// (set) Token: 0x06001FB0 RID: 8112 RVA: 0x00072CF3 File Offset: 0x00070EF3
		[DataSourceProperty]
		public HeroViewModel HeroCharacter
		{
			get
			{
				return this._heroCharacter;
			}
			set
			{
				if (value != this._heroCharacter)
				{
					this._heroCharacter = value;
					base.OnPropertyChangedWithValue<HeroViewModel>(value, "HeroCharacter");
				}
			}
		}

		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x06001FB1 RID: 8113 RVA: 0x00072D11 File Offset: 0x00070F11
		// (set) Token: 0x06001FB2 RID: 8114 RVA: 0x00072D19 File Offset: 0x00070F19
		[DataSourceProperty]
		public bool IsInspectingAnAttribute
		{
			get
			{
				return this._isInspectingAnAttribute;
			}
			set
			{
				if (value != this._isInspectingAnAttribute)
				{
					this._isInspectingAnAttribute = value;
					base.OnPropertyChangedWithValue(value, "IsInspectingAnAttribute");
				}
			}
		}

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x06001FB3 RID: 8115 RVA: 0x00072D37 File Offset: 0x00070F37
		// (set) Token: 0x06001FB4 RID: 8116 RVA: 0x00072D3F File Offset: 0x00070F3F
		[DataSourceProperty]
		public int LevelProgressPercentage
		{
			get
			{
				return this._levelProgressPercentage;
			}
			set
			{
				if (value != this._levelProgressPercentage)
				{
					this._levelProgressPercentage = value;
					base.OnPropertyChangedWithValue(value, "LevelProgressPercentage");
				}
			}
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x06001FB5 RID: 8117 RVA: 0x00072D5D File Offset: 0x00070F5D
		// (set) Token: 0x06001FB6 RID: 8118 RVA: 0x00072D65 File Offset: 0x00070F65
		[DataSourceProperty]
		public int CurrentTotalXp
		{
			get
			{
				return this._currentTotalXp;
			}
			set
			{
				if (value != this._currentTotalXp)
				{
					this._currentTotalXp = value;
					base.OnPropertyChangedWithValue(value, "CurrentTotalXp");
				}
			}
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06001FB7 RID: 8119 RVA: 0x00072D83 File Offset: 0x00070F83
		// (set) Token: 0x06001FB8 RID: 8120 RVA: 0x00072D8B File Offset: 0x00070F8B
		[DataSourceProperty]
		public int XpRequiredForNextLevel
		{
			get
			{
				return this._xpRequiredForNextLevel;
			}
			set
			{
				if (value != this._xpRequiredForNextLevel)
				{
					this._xpRequiredForNextLevel = value;
					base.OnPropertyChangedWithValue(value, "XpRequiredForNextLevel");
				}
			}
		}

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x06001FB9 RID: 8121 RVA: 0x00072DA9 File Offset: 0x00070FA9
		// (set) Token: 0x06001FBA RID: 8122 RVA: 0x00072DB1 File Offset: 0x00070FB1
		[DataSourceProperty]
		public int UnspentCharacterPoints
		{
			get
			{
				return this._unspentCharacterPoints;
			}
			set
			{
				if (value != this._unspentCharacterPoints)
				{
					this._unspentCharacterPoints = value;
					base.OnPropertyChangedWithValue(value, "UnspentCharacterPoints");
				}
			}
		}

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06001FBB RID: 8123 RVA: 0x00072DCF File Offset: 0x00070FCF
		// (set) Token: 0x06001FBC RID: 8124 RVA: 0x00072DD7 File Offset: 0x00070FD7
		[DataSourceProperty]
		public int UnspentAttributePoints
		{
			get
			{
				return this._unspentAttributePoints;
			}
			set
			{
				if (value != this._unspentAttributePoints)
				{
					this._unspentAttributePoints = value;
					base.OnPropertyChangedWithValue(value, "UnspentAttributePoints");
				}
			}
		}

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x06001FBD RID: 8125 RVA: 0x00072DF5 File Offset: 0x00070FF5
		// (set) Token: 0x06001FBE RID: 8126 RVA: 0x00072DFD File Offset: 0x00070FFD
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

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06001FBF RID: 8127 RVA: 0x00072E1B File Offset: 0x0007101B
		// (set) Token: 0x06001FC0 RID: 8128 RVA: 0x00072E23 File Offset: 0x00071023
		[DataSourceProperty]
		public string HeroNameText
		{
			get
			{
				return this._heroNameText;
			}
			set
			{
				if (value != this._heroNameText)
				{
					this._heroNameText = value;
					base.OnPropertyChangedWithValue<string>(value, "HeroNameText");
				}
			}
		}

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x06001FC1 RID: 8129 RVA: 0x00072E46 File Offset: 0x00071046
		// (set) Token: 0x06001FC2 RID: 8130 RVA: 0x00072E4E File Offset: 0x0007104E
		[DataSourceProperty]
		public string HeroInfoText
		{
			get
			{
				return this._heroInfoText;
			}
			set
			{
				if (value != this._heroInfoText)
				{
					this._heroInfoText = value;
					base.OnPropertyChangedWithValue<string>(value, "HeroInfoText");
				}
			}
		}

		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x06001FC3 RID: 8131 RVA: 0x00072E71 File Offset: 0x00071071
		// (set) Token: 0x06001FC4 RID: 8132 RVA: 0x00072E79 File Offset: 0x00071079
		[DataSourceProperty]
		public string HeroNextLevelText
		{
			get
			{
				return this._heroNextLevelText;
			}
			set
			{
				if (value != this._heroNextLevelText)
				{
					this._heroNextLevelText = value;
					base.OnPropertyChangedWithValue<string>(value, "HeroNextLevelText");
				}
			}
		}

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x06001FC5 RID: 8133 RVA: 0x00072E9C File Offset: 0x0007109C
		// (set) Token: 0x06001FC6 RID: 8134 RVA: 0x00072EA4 File Offset: 0x000710A4
		[DataSourceProperty]
		public bool HasExtraSkills
		{
			get
			{
				return this._hasExtraSkills;
			}
			set
			{
				if (value != this._hasExtraSkills)
				{
					this._hasExtraSkills = value;
					base.OnPropertyChangedWithValue(value, "HasExtraSkills");
				}
			}
		}

		// Token: 0x04000E76 RID: 3702
		private readonly PropertyOwner<CharacterAttribute> _characterAttributes;

		// Token: 0x04000E77 RID: 3703
		private MBBindingList<SkillVM> _skills;

		// Token: 0x04000E78 RID: 3704
		private PerkSelectionVM _perkSelection;

		// Token: 0x04000E79 RID: 3705
		private HeroViewModel _heroCharacter;

		// Token: 0x04000E7A RID: 3706
		private int _xpRequiredForNextLevel;

		// Token: 0x04000E7B RID: 3707
		private int _currentTotalXp;

		// Token: 0x04000E7C RID: 3708
		private int _levelProgressPercentage;

		// Token: 0x04000E7D RID: 3709
		private int _unspentCharacterPoints;

		// Token: 0x04000E7E RID: 3710
		private int _unspentAttributePoints;

		// Token: 0x04000E7F RID: 3711
		private string _currentCharacterLevelLbl;

		// Token: 0x04000E80 RID: 3712
		private string _levelProgressText;

		// Token: 0x04000E81 RID: 3713
		private string _heroNameText;

		// Token: 0x04000E82 RID: 3714
		private string _heroInfoText;

		// Token: 0x04000E83 RID: 3715
		private bool _isInspectingAnAttribute;

		// Token: 0x04000E84 RID: 3716
		private HintViewModel _levelHint;

		// Token: 0x04000E85 RID: 3717
		private SkillVM _currentSkill;

		// Token: 0x04000E86 RID: 3718
		private CharacterAttributeItemVM _currentInspectedAttribute;

		// Token: 0x04000E87 RID: 3719
		private string _heroNextLevelText;

		// Token: 0x04000E88 RID: 3720
		private string _focusPointsText;

		// Token: 0x04000E89 RID: 3721
		private MBBindingList<StringPairItemVM> _characterStats;

		// Token: 0x04000E8A RID: 3722
		private MBBindingList<CharacterAttributeItemVM> _attributes;

		// Token: 0x04000E8B RID: 3723
		private MBBindingList<EncyclopediaTraitItemVM> _traits;

		// Token: 0x04000E8C RID: 3724
		private bool _hasExtraSkills;
	}
}
