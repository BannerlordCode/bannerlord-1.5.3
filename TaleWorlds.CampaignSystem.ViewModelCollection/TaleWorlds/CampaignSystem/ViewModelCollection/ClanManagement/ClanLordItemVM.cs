using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x0200012C RID: 300
	public class ClanLordItemVM : ViewModel
	{
		// Token: 0x06001AE8 RID: 6888 RVA: 0x00065208 File Offset: 0x00063408
		public ClanLordItemVM(Hero hero, ITeleportationCampaignBehavior teleportationBehavior, Action<Hero> showHeroOnMap, Action<ClanLordItemVM> onCharacterSelect, Action onRecall, Action onTalk)
		{
			this._hero = hero;
			this._onCharacterSelect = onCharacterSelect;
			this._onRecall = onRecall;
			this._onTalk = onTalk;
			this._showHeroOnMap = showHeroOnMap;
			this._teleportationBehavior = teleportationBehavior;
			CharacterCode characterCode = CampaignUIHelper.GetCharacterCode(hero.CharacterObject, false);
			this.Visual = new CharacterImageIdentifierVM(characterCode);
			this.Skills = new MBBindingList<EncyclopediaSkillVM>();
			this.Traits = new MBBindingList<EncyclopediaTraitItemVM>();
			this.IsFamilyMember = Hero.MainHero.Clan.AliveLords.Contains(this._hero);
			this.Banner_9 = new BannerImageIdentifierVM(hero.ClanBanner, true);
			this.RefreshValues();
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x000652F4 File Offset: 0x000634F4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._hero.Name.ToString();
			StringHelpers.SetCharacterProperties("NPC", this._hero.CharacterObject, null, false);
			this.CurrentActionText = ((this._hero != Hero.MainHero) ? CampaignUIHelper.GetHeroBehaviorText(this._hero, this._teleportationBehavior) : "");
			this.LocationText = this.CurrentActionText;
			this.PregnantHint = new HintViewModel(GameTexts.FindText("str_pregnant", null), null);
			this.UpdateProperties();
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x00065389 File Offset: 0x00063589
		public void ExecuteLocationLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x0006539C File Offset: 0x0006359C
		public void UpdateProperties()
		{
			this.RelationToMainHeroText = "";
			this.GovernorOfText = "";
			this.Skills.Clear();
			this.Traits.Clear();
			this.IsMainHero = this._hero == Hero.MainHero;
			this.IsPregnant = this._hero.IsPregnant;
			List<SkillObject> list = TaleWorlds.CampaignSystem.Extensions.Skills.All.ToList<SkillObject>();
			list.Sort(CampaignUIHelper.SkillObjectComparerInstance);
			foreach (SkillObject skillObject in list)
			{
				this.Skills.Add(new EncyclopediaSkillVM(skillObject, this._hero.GetSkillValue(skillObject)));
			}
			foreach (TraitObject traitObject in CampaignUIHelper.GetHeroTraits())
			{
				if (this._hero.GetTraitLevel(traitObject) != 0)
				{
					this.Traits.Add(new EncyclopediaTraitItemVM(traitObject, this._hero));
				}
			}
			this.IsChild = FaceGen.GetMaturityTypeWithAge(this._hero.Age) <= BodyMeshMaturityType.Child;
			if (this._hero != Hero.MainHero)
			{
				this.RelationToMainHeroText = CampaignUIHelper.GetHeroRelationToHeroText(this._hero, Hero.MainHero, true).ToString();
			}
			if (this._hero.GovernorOf != null)
			{
				GameTexts.SetVariable("SETTLEMENT_NAME", this._hero.GovernorOf.Owner.Settlement.EncyclopediaLinkWithName);
				this.GovernorOfText = GameTexts.FindText("str_governor_of_label", null).ToString();
			}
			this.HeroModel = new HeroViewModel(CharacterViewModel.StanceTypes.None);
			this.HeroModel.FillFrom(this._hero, -1, false, false);
			this.Banner_9 = new BannerImageIdentifierVM(this._hero.ClanBanner, true);
			bool flag = MobileParty.MainParty.CurrentSettlement == null || MobileParty.MainParty.CurrentSettlement == this._hero.CurrentSettlement;
			this.CanShowLocationOfHero = this._hero.GetCampaignPosition().IsValid() && this._hero.PartyBelongedTo != MobileParty.MainParty && flag;
			this.ShowOnMapHint = new HintViewModel(this.CanShowLocationOfHero ? this._showLocationOfHeroOnMap : TextObject.GetEmpty(), null);
			TextObject empty = TextObject.GetEmpty();
			bool flag2 = this._hero.PartyBelongedTo == MobileParty.MainParty;
			this.IsTalkVisible = flag2 && !this.IsMainHero;
			this.IsTalkEnabled = this.IsTalkVisible && CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out empty);
			bool flag3;
			bool flag4;
			IMapPoint mapPoint;
			this.IsTeleporting = this._teleportationBehavior.GetTargetOfTeleportingHero(this._hero, out flag3, out flag4, out mapPoint);
			TextObject empty2 = TextObject.GetEmpty();
			this.IsRecallVisible = !this.IsMainHero && !flag2 && !this.IsTeleporting;
			this.IsRecallEnabled = this.IsRecallVisible && CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out empty2) && FactionHelper.IsMainClanMemberAvailableForRecall(this._hero, MobileParty.MainParty, out empty2);
			this.RecallHint = new HintViewModel(this.IsRecallEnabled ? this._recallHeroToMainPartyHintText : empty2, null);
			this.TalkHint = new HintViewModel(this.IsTalkEnabled ? this._talkToHeroHintText : empty, null);
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x000656F8 File Offset: 0x000638F8
		public void ExecuteLink()
		{
			Campaign.Current.EncyclopediaManager.GoToLink(this._hero.EncyclopediaLink);
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x00065714 File Offset: 0x00063914
		public void OnCharacterSelect()
		{
			this._onCharacterSelect(this);
		}

		// Token: 0x06001AEE RID: 6894 RVA: 0x00065722 File Offset: 0x00063922
		public virtual void ExecuteBeginHint()
		{
			InformationManager.ShowTooltip(typeof(Hero), new object[] { this._hero, true });
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x0006574B File Offset: 0x0006394B
		public virtual void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x00065752 File Offset: 0x00063952
		public Hero GetHero()
		{
			return this._hero;
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x0006575C File Offset: 0x0006395C
		public void ExecuteRename()
		{
			InformationManager.ShowTextInquiry(new TextInquiryData(new TextObject("{=2lFwF07j}Change Name", null).ToString(), string.Empty, true, true, GameTexts.FindText("str_done", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), new Action<string>(this.OnNamingHeroOver), null, false, new Func<string, Tuple<bool, string>>(CampaignUIHelper.IsStringApplicableForHeroName), "", ""), false, false);
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x000657D0 File Offset: 0x000639D0
		private void OnNamingHeroOver(string suggestedName)
		{
			if (CampaignUIHelper.IsStringApplicableForHeroName(suggestedName).Item1)
			{
				TextObject textObject = GameTexts.FindText("str_generic_character_firstname", null);
				textObject.SetTextVariable("CHARACTER_FIRSTNAME", new TextObject(suggestedName, null));
				TextObject textObject2 = GameTexts.FindText("str_generic_character_name", null);
				textObject2.SetTextVariable("CHARACTER_NAME", new TextObject(suggestedName, null));
				textObject2.SetTextVariable("CHARACTER_GENDER", this._hero.IsFemale ? 1 : 0);
				textObject.SetTextVariable("CHARACTER_GENDER", this._hero.IsFemale ? 1 : 0);
				this._hero.SetName(textObject2, textObject);
				this.Name = suggestedName;
				MobileParty partyBelongedTo = this._hero.PartyBelongedTo;
				if (((partyBelongedTo != null) ? partyBelongedTo.Army : null) != null && this._hero.PartyBelongedTo.Army.LeaderParty.Owner == this._hero)
				{
					this._hero.PartyBelongedTo.Army.UpdateName();
					return;
				}
			}
			else
			{
				Debug.FailedAssert("Suggested name is not acceptable. This shouldn't happen", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\ClanManagement\\ClanLordItemVM.cs", "OnNamingHeroOver", 203);
			}
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x000658E3 File Offset: 0x00063AE3
		public void ExecuteShowOnMap()
		{
			if (this._hero != null && this.CanShowLocationOfHero)
			{
				this._showHeroOnMap(this._hero);
			}
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x00065906 File Offset: 0x00063B06
		public void ExecuteRecall()
		{
			Action onRecall = this._onRecall;
			if (onRecall == null)
			{
				return;
			}
			onRecall();
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x00065918 File Offset: 0x00063B18
		public void ExecuteTalk()
		{
			Action onTalk = this._onTalk;
			if (onTalk == null)
			{
				return;
			}
			onTalk();
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x0006592A File Offset: 0x00063B2A
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.HeroModel.OnFinalize();
		}

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06001AF7 RID: 6903 RVA: 0x0006593D File Offset: 0x00063B3D
		// (set) Token: 0x06001AF8 RID: 6904 RVA: 0x00065945 File Offset: 0x00063B45
		[DataSourceProperty]
		public MBBindingList<EncyclopediaSkillVM> Skills
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
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaSkillVM>>(value, "Skills");
				}
			}
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06001AF9 RID: 6905 RVA: 0x00065963 File Offset: 0x00063B63
		// (set) Token: 0x06001AFA RID: 6906 RVA: 0x0006596B File Offset: 0x00063B6B
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

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06001AFB RID: 6907 RVA: 0x00065989 File Offset: 0x00063B89
		// (set) Token: 0x06001AFC RID: 6908 RVA: 0x00065991 File Offset: 0x00063B91
		[DataSourceProperty]
		public HeroViewModel HeroModel
		{
			get
			{
				return this._heroModel;
			}
			set
			{
				if (value != this._heroModel)
				{
					this._heroModel = value;
					base.OnPropertyChangedWithValue<HeroViewModel>(value, "HeroModel");
				}
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06001AFD RID: 6909 RVA: 0x000659AF File Offset: 0x00063BAF
		// (set) Token: 0x06001AFE RID: 6910 RVA: 0x000659B7 File Offset: 0x00063BB7
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06001AFF RID: 6911 RVA: 0x000659D5 File Offset: 0x00063BD5
		// (set) Token: 0x06001B00 RID: 6912 RVA: 0x000659DD File Offset: 0x00063BDD
		[DataSourceProperty]
		public bool IsChild
		{
			get
			{
				return this._isChild;
			}
			set
			{
				if (value != this._isChild)
				{
					this._isChild = value;
					base.OnPropertyChangedWithValue(value, "IsChild");
				}
			}
		}

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x06001B01 RID: 6913 RVA: 0x000659FB File Offset: 0x00063BFB
		// (set) Token: 0x06001B02 RID: 6914 RVA: 0x00065A03 File Offset: 0x00063C03
		[DataSourceProperty]
		public bool IsTeleporting
		{
			get
			{
				return this._isTeleporting;
			}
			set
			{
				if (value != this._isTeleporting)
				{
					this._isTeleporting = value;
					base.OnPropertyChangedWithValue(value, "IsTeleporting");
				}
			}
		}

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06001B03 RID: 6915 RVA: 0x00065A21 File Offset: 0x00063C21
		// (set) Token: 0x06001B04 RID: 6916 RVA: 0x00065A29 File Offset: 0x00063C29
		[DataSourceProperty]
		public bool IsRecallVisible
		{
			get
			{
				return this._isRecallVisible;
			}
			set
			{
				if (value != this._isRecallVisible)
				{
					this._isRecallVisible = value;
					base.OnPropertyChangedWithValue(value, "IsRecallVisible");
				}
			}
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06001B05 RID: 6917 RVA: 0x00065A47 File Offset: 0x00063C47
		// (set) Token: 0x06001B06 RID: 6918 RVA: 0x00065A4F File Offset: 0x00063C4F
		[DataSourceProperty]
		public bool IsRecallEnabled
		{
			get
			{
				return this._isRecallEnabled;
			}
			set
			{
				if (value != this._isRecallEnabled)
				{
					this._isRecallEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsRecallEnabled");
				}
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06001B07 RID: 6919 RVA: 0x00065A6D File Offset: 0x00063C6D
		// (set) Token: 0x06001B08 RID: 6920 RVA: 0x00065A75 File Offset: 0x00063C75
		[DataSourceProperty]
		public bool IsTalkVisible
		{
			get
			{
				return this._isTalkVisible;
			}
			set
			{
				if (value != this._isTalkVisible)
				{
					this._isTalkVisible = value;
					base.OnPropertyChangedWithValue(value, "IsTalkVisible");
				}
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06001B09 RID: 6921 RVA: 0x00065A93 File Offset: 0x00063C93
		// (set) Token: 0x06001B0A RID: 6922 RVA: 0x00065A9B File Offset: 0x00063C9B
		[DataSourceProperty]
		public bool IsTalkEnabled
		{
			get
			{
				return this._isTalkEnabled;
			}
			set
			{
				if (value != this._isTalkEnabled)
				{
					this._isTalkEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsTalkEnabled");
				}
			}
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06001B0B RID: 6923 RVA: 0x00065AB9 File Offset: 0x00063CB9
		// (set) Token: 0x06001B0C RID: 6924 RVA: 0x00065AC1 File Offset: 0x00063CC1
		[DataSourceProperty]
		public bool CanShowLocationOfHero
		{
			get
			{
				return this._canShowLocationOfHero;
			}
			set
			{
				if (value != this._canShowLocationOfHero)
				{
					this._canShowLocationOfHero = value;
					base.OnPropertyChangedWithValue(value, "CanShowLocationOfHero");
				}
			}
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x06001B0D RID: 6925 RVA: 0x00065ADF File Offset: 0x00063CDF
		// (set) Token: 0x06001B0E RID: 6926 RVA: 0x00065AE7 File Offset: 0x00063CE7
		[DataSourceProperty]
		public bool IsMainHero
		{
			get
			{
				return this._isMainHero;
			}
			set
			{
				if (value != this._isMainHero)
				{
					this._isMainHero = value;
					base.OnPropertyChangedWithValue(value, "IsMainHero");
				}
			}
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06001B0F RID: 6927 RVA: 0x00065B05 File Offset: 0x00063D05
		// (set) Token: 0x06001B10 RID: 6928 RVA: 0x00065B0D File Offset: 0x00063D0D
		[DataSourceProperty]
		public bool IsFamilyMember
		{
			get
			{
				return this._isFamilyMember;
			}
			set
			{
				if (value != this._isFamilyMember)
				{
					this._isFamilyMember = value;
					base.OnPropertyChangedWithValue(value, "IsFamilyMember");
				}
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06001B11 RID: 6929 RVA: 0x00065B2B File Offset: 0x00063D2B
		// (set) Token: 0x06001B12 RID: 6930 RVA: 0x00065B33 File Offset: 0x00063D33
		[DataSourceProperty]
		public bool IsPregnant
		{
			get
			{
				return this._isPregnant;
			}
			set
			{
				if (value != this._isPregnant)
				{
					this._isPregnant = value;
					base.OnPropertyChangedWithValue(value, "IsPregnant");
				}
			}
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06001B13 RID: 6931 RVA: 0x00065B51 File Offset: 0x00063D51
		// (set) Token: 0x06001B14 RID: 6932 RVA: 0x00065B59 File Offset: 0x00063D59
		[DataSourceProperty]
		public CharacterImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				if (value != this._visual)
				{
					this._visual = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06001B15 RID: 6933 RVA: 0x00065B77 File Offset: 0x00063D77
		// (set) Token: 0x06001B16 RID: 6934 RVA: 0x00065B7F File Offset: 0x00063D7F
		[DataSourceProperty]
		public BannerImageIdentifierVM Banner_9
		{
			get
			{
				return this._banner_9;
			}
			set
			{
				if (value != this._banner_9)
				{
					this._banner_9 = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Banner_9");
				}
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06001B17 RID: 6935 RVA: 0x00065B9D File Offset: 0x00063D9D
		// (set) Token: 0x06001B18 RID: 6936 RVA: 0x00065BA5 File Offset: 0x00063DA5
		[DataSourceProperty]
		public string LocationText
		{
			get
			{
				return this._locationText;
			}
			set
			{
				if (value != this._locationText)
				{
					this._locationText = value;
					base.OnPropertyChangedWithValue<string>(value, "LocationText");
				}
			}
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06001B19 RID: 6937 RVA: 0x00065BC8 File Offset: 0x00063DC8
		// (set) Token: 0x06001B1A RID: 6938 RVA: 0x00065BD0 File Offset: 0x00063DD0
		[DataSourceProperty]
		public string CurrentActionText
		{
			get
			{
				return this._currentActionText;
			}
			set
			{
				if (value != this._currentActionText)
				{
					this._currentActionText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentActionText");
				}
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06001B1B RID: 6939 RVA: 0x00065BF3 File Offset: 0x00063DF3
		// (set) Token: 0x06001B1C RID: 6940 RVA: 0x00065BFB File Offset: 0x00063DFB
		[DataSourceProperty]
		public string RelationToMainHeroText
		{
			get
			{
				return this._relationToMainHeroText;
			}
			set
			{
				if (value != this._relationToMainHeroText)
				{
					this._relationToMainHeroText = value;
					base.OnPropertyChangedWithValue<string>(value, "RelationToMainHeroText");
				}
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06001B1D RID: 6941 RVA: 0x00065C1E File Offset: 0x00063E1E
		// (set) Token: 0x06001B1E RID: 6942 RVA: 0x00065C26 File Offset: 0x00063E26
		[DataSourceProperty]
		public string GovernorOfText
		{
			get
			{
				return this._governorOfText;
			}
			set
			{
				if (value != this._governorOfText)
				{
					this._governorOfText = value;
					base.OnPropertyChangedWithValue<string>(value, "GovernorOfText");
				}
			}
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06001B1F RID: 6943 RVA: 0x00065C49 File Offset: 0x00063E49
		// (set) Token: 0x06001B20 RID: 6944 RVA: 0x00065C51 File Offset: 0x00063E51
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06001B21 RID: 6945 RVA: 0x00065C74 File Offset: 0x00063E74
		// (set) Token: 0x06001B22 RID: 6946 RVA: 0x00065C7C File Offset: 0x00063E7C
		[DataSourceProperty]
		public HintViewModel PregnantHint
		{
			get
			{
				return this._pregnantHint;
			}
			set
			{
				if (value != this._pregnantHint)
				{
					this._pregnantHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "PregnantHint");
				}
			}
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06001B23 RID: 6947 RVA: 0x00065C9A File Offset: 0x00063E9A
		// (set) Token: 0x06001B24 RID: 6948 RVA: 0x00065CA2 File Offset: 0x00063EA2
		[DataSourceProperty]
		public HintViewModel ShowOnMapHint
		{
			get
			{
				return this._showOnMapHint;
			}
			set
			{
				if (value != this._showOnMapHint)
				{
					this._showOnMapHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ShowOnMapHint");
				}
			}
		}

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06001B25 RID: 6949 RVA: 0x00065CC0 File Offset: 0x00063EC0
		// (set) Token: 0x06001B26 RID: 6950 RVA: 0x00065CC8 File Offset: 0x00063EC8
		[DataSourceProperty]
		public HintViewModel RecallHint
		{
			get
			{
				return this._recallHint;
			}
			set
			{
				if (value != this._recallHint)
				{
					this._recallHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RecallHint");
				}
			}
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06001B27 RID: 6951 RVA: 0x00065CE6 File Offset: 0x00063EE6
		// (set) Token: 0x06001B28 RID: 6952 RVA: 0x00065CEE File Offset: 0x00063EEE
		[DataSourceProperty]
		public HintViewModel TalkHint
		{
			get
			{
				return this._talkHint;
			}
			set
			{
				if (value != this._talkHint)
				{
					this._talkHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "TalkHint");
				}
			}
		}

		// Token: 0x04000C7C RID: 3196
		private readonly Action<ClanLordItemVM> _onCharacterSelect;

		// Token: 0x04000C7D RID: 3197
		private readonly Action _onRecall;

		// Token: 0x04000C7E RID: 3198
		private readonly Action _onTalk;

		// Token: 0x04000C7F RID: 3199
		private readonly Hero _hero;

		// Token: 0x04000C80 RID: 3200
		private readonly Action<Hero> _showHeroOnMap;

		// Token: 0x04000C81 RID: 3201
		private readonly ITeleportationCampaignBehavior _teleportationBehavior;

		// Token: 0x04000C82 RID: 3202
		private readonly TextObject _prisonerOfText = new TextObject("{=a8nRxITn}Prisoner of {PARTY_NAME}", null);

		// Token: 0x04000C83 RID: 3203
		private readonly TextObject _showLocationOfHeroOnMap = new TextObject("{=aGJYQOef}Show hero's location on map.", null);

		// Token: 0x04000C84 RID: 3204
		private readonly TextObject _recallHeroToMainPartyHintText = new TextObject("{=ANV8UV5f}Recall this member to your party.", null);

		// Token: 0x04000C85 RID: 3205
		private readonly TextObject _talkToHeroHintText = new TextObject("{=j4BdjLYp}Start a conversation with this clan member.", null);

		// Token: 0x04000C86 RID: 3206
		private CharacterImageIdentifierVM _visual;

		// Token: 0x04000C87 RID: 3207
		private BannerImageIdentifierVM _banner_9;

		// Token: 0x04000C88 RID: 3208
		private bool _isSelected;

		// Token: 0x04000C89 RID: 3209
		private bool _isChild;

		// Token: 0x04000C8A RID: 3210
		private bool _isMainHero;

		// Token: 0x04000C8B RID: 3211
		private bool _isFamilyMember;

		// Token: 0x04000C8C RID: 3212
		private bool _isPregnant;

		// Token: 0x04000C8D RID: 3213
		private bool _isTeleporting;

		// Token: 0x04000C8E RID: 3214
		private bool _isRecallVisible;

		// Token: 0x04000C8F RID: 3215
		private bool _isRecallEnabled;

		// Token: 0x04000C90 RID: 3216
		private bool _isTalkVisible;

		// Token: 0x04000C91 RID: 3217
		private bool _isTalkEnabled;

		// Token: 0x04000C92 RID: 3218
		private bool _canShowLocationOfHero;

		// Token: 0x04000C93 RID: 3219
		private string _name;

		// Token: 0x04000C94 RID: 3220
		private string _locationText;

		// Token: 0x04000C95 RID: 3221
		private string _relationToMainHeroText;

		// Token: 0x04000C96 RID: 3222
		private string _governorOfText;

		// Token: 0x04000C97 RID: 3223
		private string _currentActionText;

		// Token: 0x04000C98 RID: 3224
		private HeroViewModel _heroModel;

		// Token: 0x04000C99 RID: 3225
		private MBBindingList<EncyclopediaSkillVM> _skills;

		// Token: 0x04000C9A RID: 3226
		private MBBindingList<EncyclopediaTraitItemVM> _traits;

		// Token: 0x04000C9B RID: 3227
		private HintViewModel _pregnantHint;

		// Token: 0x04000C9C RID: 3228
		private HintViewModel _showOnMapHint;

		// Token: 0x04000C9D RID: 3229
		private HintViewModel _recallHint;

		// Token: 0x04000C9E RID: 3230
		private HintViewModel _talkHint;
	}
}
