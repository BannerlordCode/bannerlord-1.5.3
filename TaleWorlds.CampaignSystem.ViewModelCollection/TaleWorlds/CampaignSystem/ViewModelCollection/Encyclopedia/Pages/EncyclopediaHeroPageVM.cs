using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages
{
	// Token: 0x020000D9 RID: 217
	[EncyclopediaViewModel(typeof(Hero))]
	public class EncyclopediaHeroPageVM : EncyclopediaContentPageVM
	{
		// Token: 0x0600144B RID: 5195 RVA: 0x00051764 File Offset: 0x0004F964
		public EncyclopediaHeroPageVM(EncyclopediaPageArgs args)
			: base(args)
		{
			this._hero = base.Obj as Hero;
			this._relationAscendingComparer = new HeroRelationComparer(this._hero, true, true);
			this._relationDescendingComparer = new HeroRelationComparer(this._hero, false, true);
			TextObject textObject;
			this.IsInformationHidden = CampaignUIHelper.IsHeroInformationHidden(this._hero, out textObject);
			this._infoHiddenReasonText = textObject;
			this._allRelatedHeroes = new List<Hero>
			{
				this._hero.Father,
				this._hero.Mother,
				this._hero.Spouse
			};
			this._allRelatedHeroes.AddRange(this._hero.Siblings);
			this._allRelatedHeroes.AddRange(this._hero.ExSpouses);
			this._allRelatedHeroes.AddRange(CampaignUIHelper.GetChildrenAndGrandchildrenOfHero(this._hero));
			StringHelpers.SetCharacterProperties("NPC", this._hero.CharacterObject, null, false);
			this.Settlements = new MBBindingList<EncyclopediaSettlementVM>();
			this.Dwellings = new MBBindingList<EncyclopediaDwellingVM>();
			this.Allies = new MBBindingList<HeroVM>();
			this.AdditionalAllies = new MBBindingList<HeroVM>();
			this.Enemies = new MBBindingList<HeroVM>();
			this.AdditionalEnemies = new MBBindingList<HeroVM>();
			this.Family = new MBBindingList<EncyclopediaFamilyMemberVM>();
			this.Companions = new MBBindingList<HeroVM>();
			this.History = new MBBindingList<EncyclopediaHistoryEventVM>();
			this.Skills = new MBBindingList<EncyclopediaSkillVM>();
			this.Stats = new MBBindingList<StringPairItemVM>();
			this.Traits = new MBBindingList<EncyclopediaTraitItemVM>();
			this.HeroCharacter = new HeroViewModel(CharacterViewModel.StanceTypes.EmphasizeFace);
			base.IsBookmarked = Campaign.Current.EncyclopediaManager.ViewDataTracker.IsEncyclopediaBookmarked(this._hero);
			this.Faction = new EncyclopediaFactionVM(this._hero.Clan);
			this.RefreshValues();
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x00051930 File Offset: 0x0004FB30
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ClanText = GameTexts.FindText("str_clan", null).ToString();
			this.AlliesText = GameTexts.FindText("str_friends", null).ToString();
			this.EnemiesText = GameTexts.FindText("str_enemies", null).ToString();
			this.FamilyText = GameTexts.FindText("str_family_group", null).ToString();
			this.CompanionsText = GameTexts.FindText("str_companions", null).ToString();
			this.DwellingsText = GameTexts.FindText("str_dwellings", null).ToString();
			this.SettlementsText = GameTexts.FindText("str_settlements", null).ToString();
			this.DeceasedText = GameTexts.FindText("str_encyclopedia_deceased", null).ToString();
			this.TraitsText = GameTexts.FindText("str_traits_group", null).ToString();
			this.SkillsText = GameTexts.FindText("str_skills", null).ToString();
			this.InfoText = GameTexts.FindText("str_info", null).ToString();
			this.PregnantHint = new HintViewModel(GameTexts.FindText("str_pregnant", null), null);
			base.UpdateBookmarkHintText();
			this.UpdateInformationText();
			this.Refresh();
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x00051A60 File Offset: 0x0004FC60
		public override void Refresh()
		{
			base.IsLoadingOver = false;
			this.Settlements.Clear();
			this.Dwellings.Clear();
			this.Allies.Clear();
			this.Enemies.Clear();
			this.AdditionalAllies.Clear();
			this.AdditionalEnemies.Clear();
			this.Companions.Clear();
			this.Family.Clear();
			this.History.Clear();
			this.Skills.Clear();
			this.Stats.Clear();
			this.Traits.Clear();
			this.NameText = this._hero.Name.ToString();
			string text = GameTexts.FindText("str_missing_info_indicator", null).ToString();
			EncyclopediaPage pageOf = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Hero));
			this.HasNeutralClan = this._hero.Clan == null;
			if (!this.IsInformationHidden)
			{
				List<SkillObject> list = TaleWorlds.CampaignSystem.Extensions.Skills.All.ToList<SkillObject>();
				list.Sort(CampaignUIHelper.SkillObjectComparerInstance);
				foreach (SkillObject skillObject in list)
				{
					if (this._hero.GetSkillValue(skillObject) >= 50)
					{
						this.Skills.Add(new EncyclopediaSkillVM(skillObject, this._hero.GetSkillValue(skillObject)));
					}
				}
				foreach (TraitObject traitObject in CampaignUIHelper.GetHeroTraits())
				{
					if (this._hero.GetTraitLevel(traitObject) != 0)
					{
						this.Traits.Add(new EncyclopediaTraitItemVM(traitObject, this._hero));
					}
				}
				if (this._hero.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge)
				{
					for (int i = 0; i < Hero.AllAliveHeroes.Count; i++)
					{
						this.AddHeroToRelatedVMList(Hero.AllAliveHeroes[i]);
					}
					for (int j = 0; j < Hero.DeadOrDisabledHeroes.Count; j++)
					{
						this.AddHeroToRelatedVMList(Hero.DeadOrDisabledHeroes[j]);
					}
					this.Allies.Sort(this._relationDescendingComparer);
					this.Enemies.Sort(this._relationAscendingComparer);
					while (this.Allies.Count > 13)
					{
						HeroVM heroVM = this.Allies[13];
						this.Allies.Remove(heroVM);
						this.AdditionalAllies.Add(heroVM);
					}
					while (this.Enemies.Count > 13)
					{
						HeroVM heroVM2 = this.Enemies[13];
						this.Enemies.Remove(heroVM2);
						this.AdditionalEnemies.Add(heroVM2);
					}
					this.OnAdditionalListsUpdated();
				}
				if (this._hero.Clan != null && this._hero == this._hero.Clan.Leader)
				{
					for (int k = 0; k < this._hero.Clan.Companions.Count; k++)
					{
						Hero hero = this._hero.Clan.Companions[k];
						this.Companions.Add(new HeroVM(hero, false));
					}
				}
				for (int l = 0; l < this._allRelatedHeroes.Count; l++)
				{
					Hero hero2 = this._allRelatedHeroes[l];
					if (hero2 != null && pageOf.IsValidEncyclopediaItem(hero2))
					{
						this.Family.Add(new EncyclopediaFamilyMemberVM(hero2, this._hero));
					}
				}
				for (int m = 0; m < this._hero.OwnedWorkshops.Count; m++)
				{
					this.Dwellings.Add(new EncyclopediaDwellingVM(this._hero.OwnedWorkshops[m].WorkshopType));
				}
				EncyclopediaPage pageOf2 = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Settlement));
				for (int n = 0; n < Settlement.All.Count; n++)
				{
					Settlement settlement = Settlement.All[n];
					if (settlement.OwnerClan != null && settlement.OwnerClan.Leader == this._hero && pageOf2.IsValidEncyclopediaItem(settlement))
					{
						this.Settlements.Add(new EncyclopediaSettlementVM(settlement));
					}
				}
			}
			this.HasAnySkills = this.Skills.Count > 0;
			if (this._hero.Culture != null)
			{
				string text2 = GameTexts.FindText("str_enc_sf_culture", null).ToString();
				this.Stats.Add(new StringPairItemVM(text2, this._hero.Culture.Name.ToString(), null));
			}
			string text3 = GameTexts.FindText("str_enc_sf_age", null).ToString();
			this.Stats.Add(new StringPairItemVM(text3, this.IsInformationHidden ? text : ((int)this._hero.Age).ToString(), null));
			MBObjectBase hero3 = this._hero;
			for (int num = Campaign.Current.LogEntryHistory.GameActionLogs.Count - 1; num >= 0; num--)
			{
				IEncyclopediaLog encyclopediaLog;
				if ((encyclopediaLog = Campaign.Current.LogEntryHistory.GameActionLogs[num] as IEncyclopediaLog) != null && encyclopediaLog.IsVisibleInEncyclopediaPageOf(hero3))
				{
					this.History.Add(new EncyclopediaHistoryEventVM(encyclopediaLog));
				}
			}
			if (!this._hero.IsNotable && !this._hero.IsWanderer)
			{
				Clan clan = this._hero.Clan;
				if (((clan != null) ? clan.Kingdom : null) != null)
				{
					this.KingdomRankText = CampaignUIHelper.GetHeroKingdomRank(this._hero);
				}
			}
			string heroOccupationName = CampaignUIHelper.GetHeroOccupationName(this._hero);
			if (!string.IsNullOrEmpty(heroOccupationName))
			{
				string text4 = GameTexts.FindText("str_enc_sf_occupation", null).ToString();
				this.Stats.Add(new StringPairItemVM(text4, this.IsInformationHidden ? text : heroOccupationName, null));
			}
			if (this._hero != Hero.MainHero)
			{
				string text5 = GameTexts.FindText("str_enc_sf_relation", null).ToString();
				this.Stats.Add(new StringPairItemVM(text5, this.IsInformationHidden ? text : this._hero.GetRelationWithPlayer().ToString(), null));
			}
			this.LastSeenText = ((this._hero == Hero.MainHero) ? "" : HeroHelper.GetLastSeenText(this._hero).ToString());
			this.HeroCharacter.FillFrom(this._hero, -1, this._hero.IsNotable, true);
			this.HeroCharacter.SetEquipment(EquipmentIndex.ArmorItemEndSlot, default(EquipmentElement));
			this.HeroCharacter.SetEquipment(EquipmentIndex.HorseHarness, default(EquipmentElement));
			this.HeroCharacter.SetEquipment(EquipmentIndex.NumAllWeaponSlots, default(EquipmentElement));
			this.IsCompanion = this._hero.CompanionOf != null;
			if (this.IsCompanion)
			{
				this.MasterText = GameTexts.FindText("str_companion_of", null).ToString();
				Clan companionOf = this._hero.CompanionOf;
				this.Master = new HeroVM((companionOf != null) ? companionOf.Leader : null, false);
			}
			this.IsPregnant = this._hero.IsPregnant;
			this.IsDead = !this._hero.IsAlive;
			base.IsLoadingOver = true;
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x000521C8 File Offset: 0x000503C8
		private void AddHeroToRelatedVMList(Hero hero)
		{
			if (!Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Hero)).IsValidEncyclopediaItem(hero) || hero.IsNotable)
			{
				return;
			}
			if (hero != this._hero && hero.IsAlive && hero.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && !this._allRelatedHeroes.Contains(hero))
			{
				if (this._hero.IsFriend(hero))
				{
					this.Allies.Add(new HeroVM(hero, false));
					return;
				}
				if (this._hero.IsEnemy(hero))
				{
					this.Enemies.Add(new HeroVM(hero, false));
				}
			}
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0005227C File Offset: 0x0005047C
		public override string GetName()
		{
			return this._hero.Name.ToString();
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x00052290 File Offset: 0x00050490
		public override string GetNavigationBarURL()
		{
			return HyperlinkTexts.GetGenericHyperlinkText("Home", GameTexts.FindText("str_encyclopedia_home", null).ToString()) + " \\ " + HyperlinkTexts.GetGenericHyperlinkText("ListPage-Heroes", GameTexts.FindText("str_encyclopedia_heroes", null).ToString()) + " \\ " + this.GetName();
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x000522F5 File Offset: 0x000504F5
		public void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x00052308 File Offset: 0x00050508
		public override void ExecuteSwitchBookmarkedState()
		{
			base.ExecuteSwitchBookmarkedState();
			if (base.IsBookmarked)
			{
				Campaign.Current.EncyclopediaManager.ViewDataTracker.AddEncyclopediaBookmarkToItem(this._hero);
				return;
			}
			Campaign.Current.EncyclopediaManager.ViewDataTracker.RemoveEncyclopediaBookmarkFromItem(this._hero);
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x00052358 File Offset: 0x00050558
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.HeroCharacter.OnFinalize();
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x0005236C File Offset: 0x0005056C
		private void UpdateInformationText()
		{
			this.InformationText = "";
			if (!TextObject.IsNullOrEmpty(this._hero.EncyclopediaText))
			{
				this.InformationText = this._hero.EncyclopediaText.ToString();
				return;
			}
			if (this._hero.CharacterObject.Occupation == Occupation.Lord)
			{
				this.InformationText = Hero.SetHeroEncyclopediaTextAndLinks(this._hero).ToString();
			}
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x000523D8 File Offset: 0x000505D8
		private void OnAdditionalListsUpdated()
		{
			this.AnyAdditionalAllies = this.AdditionalAllies.Count > 0;
			this.AnyAdditionalEnemies = this.AdditionalEnemies.Count > 0;
			this.AdditionalAlliesString = (this.AnyAdditionalAllies ? new TextObject("{=!}+{REMAINING}", null).SetTextVariable("REMAINING", this.AdditionalAllies.Count).ToString() : string.Empty);
			this.AdditionalEnemiesString = (this.AnyAdditionalEnemies ? new TextObject("{=!}+{REMAINING}", null).SetTextVariable("REMAINING", this.AdditionalEnemies.Count).ToString() : string.Empty);
			this.AdditionalAlliesHint = new BasicTooltipViewModel(() => this.GetOverflowTooltip(this.AdditionalAllies));
			this.AdditionalEnemiesHint = new BasicTooltipViewModel(() => this.GetOverflowTooltip(this.AdditionalEnemies));
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x000524B0 File Offset: 0x000506B0
		private List<TooltipProperty> GetOverflowTooltip(MBBindingList<HeroVM> overflowList)
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			foreach (HeroVM heroVM in overflowList)
			{
				list.Add(new TooltipProperty(string.Empty, heroVM.NameText, 0, false, TooltipProperty.TooltipPropertyFlags.None));
			}
			return list;
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06001457 RID: 5207 RVA: 0x00052514 File Offset: 0x00050714
		// (set) Token: 0x06001458 RID: 5208 RVA: 0x0005251C File Offset: 0x0005071C
		[DataSourceProperty]
		public EncyclopediaFactionVM Faction
		{
			get
			{
				return this._faction;
			}
			set
			{
				if (value != this._faction)
				{
					this._faction = value;
					base.OnPropertyChangedWithValue<EncyclopediaFactionVM>(value, "Faction");
				}
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06001459 RID: 5209 RVA: 0x0005253A File Offset: 0x0005073A
		// (set) Token: 0x0600145A RID: 5210 RVA: 0x00052542 File Offset: 0x00050742
		[DataSourceProperty]
		public bool IsCompanion
		{
			get
			{
				return this._isCompanion;
			}
			set
			{
				if (value != this._isCompanion)
				{
					this._isCompanion = value;
					base.OnPropertyChangedWithValue(value, "IsCompanion");
				}
			}
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x0600145B RID: 5211 RVA: 0x00052560 File Offset: 0x00050760
		// (set) Token: 0x0600145C RID: 5212 RVA: 0x00052568 File Offset: 0x00050768
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

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x0600145D RID: 5213 RVA: 0x00052586 File Offset: 0x00050786
		// (set) Token: 0x0600145E RID: 5214 RVA: 0x0005258E File Offset: 0x0005078E
		[DataSourceProperty]
		public HeroVM Master
		{
			get
			{
				return this._master;
			}
			set
			{
				if (value != this._master)
				{
					this._master = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "Master");
				}
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x0600145F RID: 5215 RVA: 0x000525AC File Offset: 0x000507AC
		// (set) Token: 0x06001460 RID: 5216 RVA: 0x000525B4 File Offset: 0x000507B4
		[DataSourceProperty]
		public string ClanText
		{
			get
			{
				return this._clanText;
			}
			set
			{
				if (value != this._clanText)
				{
					this._clanText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClanText");
				}
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06001461 RID: 5217 RVA: 0x000525D7 File Offset: 0x000507D7
		// (set) Token: 0x06001462 RID: 5218 RVA: 0x000525DF File Offset: 0x000507DF
		[DataSourceProperty]
		public string InfoText
		{
			get
			{
				return this._infoText;
			}
			set
			{
				if (value != this._infoText)
				{
					this._infoText = value;
					base.OnPropertyChangedWithValue<string>(value, "InfoText");
				}
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06001463 RID: 5219 RVA: 0x00052602 File Offset: 0x00050802
		// (set) Token: 0x06001464 RID: 5220 RVA: 0x0005260A File Offset: 0x0005080A
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

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06001465 RID: 5221 RVA: 0x0005262D File Offset: 0x0005082D
		// (set) Token: 0x06001466 RID: 5222 RVA: 0x00052635 File Offset: 0x00050835
		[DataSourceProperty]
		public string MasterText
		{
			get
			{
				return this._masterText;
			}
			set
			{
				if (value != this._masterText)
				{
					this._masterText = value;
					base.OnPropertyChangedWithValue<string>(value, "MasterText");
				}
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06001467 RID: 5223 RVA: 0x00052658 File Offset: 0x00050858
		// (set) Token: 0x06001468 RID: 5224 RVA: 0x00052660 File Offset: 0x00050860
		[DataSourceProperty]
		public string KingdomRankText
		{
			get
			{
				return this._kingdomRankText;
			}
			set
			{
				if (value != this._kingdomRankText)
				{
					this._kingdomRankText = value;
					base.OnPropertyChangedWithValue<string>(value, "KingdomRankText");
				}
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06001469 RID: 5225 RVA: 0x00052683 File Offset: 0x00050883
		[DataSourceProperty]
		public string InfoHiddenReasonText
		{
			get
			{
				return this._infoHiddenReasonText.ToString();
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x0600146A RID: 5226 RVA: 0x00052690 File Offset: 0x00050890
		// (set) Token: 0x0600146B RID: 5227 RVA: 0x00052698 File Offset: 0x00050898
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

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x0600146C RID: 5228 RVA: 0x000526BB File Offset: 0x000508BB
		// (set) Token: 0x0600146D RID: 5229 RVA: 0x000526C3 File Offset: 0x000508C3
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

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x0600146E RID: 5230 RVA: 0x000526E1 File Offset: 0x000508E1
		// (set) Token: 0x0600146F RID: 5231 RVA: 0x000526E9 File Offset: 0x000508E9
		[DataSourceProperty]
		public string LastSeenText
		{
			get
			{
				return this._lastSeenText;
			}
			set
			{
				if (value != this._lastSeenText)
				{
					this._lastSeenText = value;
					base.OnPropertyChangedWithValue<string>(value, "LastSeenText");
				}
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06001470 RID: 5232 RVA: 0x0005270C File Offset: 0x0005090C
		// (set) Token: 0x06001471 RID: 5233 RVA: 0x00052714 File Offset: 0x00050914
		[DataSourceProperty]
		public string DeceasedText
		{
			get
			{
				return this._deceasedText;
			}
			set
			{
				if (value != this._deceasedText)
				{
					this._deceasedText = value;
					base.OnPropertyChangedWithValue<string>(value, "DeceasedText");
				}
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06001472 RID: 5234 RVA: 0x00052737 File Offset: 0x00050937
		// (set) Token: 0x06001473 RID: 5235 RVA: 0x0005273F File Offset: 0x0005093F
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06001474 RID: 5236 RVA: 0x00052762 File Offset: 0x00050962
		// (set) Token: 0x06001475 RID: 5237 RVA: 0x0005276A File Offset: 0x0005096A
		[DataSourceProperty]
		public string SettlementsText
		{
			get
			{
				return this._settlementsText;
			}
			set
			{
				if (value != this._settlementsText)
				{
					this._settlementsText = value;
					base.OnPropertyChangedWithValue<string>(value, "SettlementsText");
				}
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06001476 RID: 5238 RVA: 0x0005278D File Offset: 0x0005098D
		// (set) Token: 0x06001477 RID: 5239 RVA: 0x00052795 File Offset: 0x00050995
		[DataSourceProperty]
		public string DwellingsText
		{
			get
			{
				return this._dwellingsText;
			}
			set
			{
				if (value != this._dwellingsText)
				{
					this._dwellingsText = value;
					base.OnPropertyChangedWithValue<string>(value, "DwellingsText");
				}
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06001478 RID: 5240 RVA: 0x000527B8 File Offset: 0x000509B8
		// (set) Token: 0x06001479 RID: 5241 RVA: 0x000527C0 File Offset: 0x000509C0
		[DataSourceProperty]
		public string CompanionsText
		{
			get
			{
				return this._companionsText;
			}
			set
			{
				if (value != this._companionsText)
				{
					this._companionsText = value;
					base.OnPropertyChangedWithValue<string>(value, "CompanionsText");
				}
			}
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x0600147A RID: 5242 RVA: 0x000527E3 File Offset: 0x000509E3
		// (set) Token: 0x0600147B RID: 5243 RVA: 0x000527EB File Offset: 0x000509EB
		[DataSourceProperty]
		public string AlliesText
		{
			get
			{
				return this._alliesText;
			}
			set
			{
				if (value != this._alliesText)
				{
					this._alliesText = value;
					base.OnPropertyChangedWithValue<string>(value, "AlliesText");
				}
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x0600147C RID: 5244 RVA: 0x0005280E File Offset: 0x00050A0E
		// (set) Token: 0x0600147D RID: 5245 RVA: 0x00052816 File Offset: 0x00050A16
		[DataSourceProperty]
		public string EnemiesText
		{
			get
			{
				return this._enemiesText;
			}
			set
			{
				if (value != this._enemiesText)
				{
					this._enemiesText = value;
					base.OnPropertyChangedWithValue<string>(value, "EnemiesText");
				}
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x0600147E RID: 5246 RVA: 0x00052839 File Offset: 0x00050A39
		// (set) Token: 0x0600147F RID: 5247 RVA: 0x00052841 File Offset: 0x00050A41
		[DataSourceProperty]
		public string FamilyText
		{
			get
			{
				return this._familyText;
			}
			set
			{
				if (value != this._familyText)
				{
					this._familyText = value;
					base.OnPropertyChangedWithValue<string>(value, "FamilyText");
				}
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06001480 RID: 5248 RVA: 0x00052864 File Offset: 0x00050A64
		// (set) Token: 0x06001481 RID: 5249 RVA: 0x0005286C File Offset: 0x00050A6C
		[DataSourceProperty]
		public MBBindingList<StringPairItemVM> Stats
		{
			get
			{
				return this._stats;
			}
			set
			{
				if (value != this._stats)
				{
					this._stats = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemVM>>(value, "Stats");
				}
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06001482 RID: 5250 RVA: 0x0005288A File Offset: 0x00050A8A
		// (set) Token: 0x06001483 RID: 5251 RVA: 0x00052892 File Offset: 0x00050A92
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

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06001484 RID: 5252 RVA: 0x000528B0 File Offset: 0x00050AB0
		// (set) Token: 0x06001485 RID: 5253 RVA: 0x000528B8 File Offset: 0x00050AB8
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

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06001486 RID: 5254 RVA: 0x000528D6 File Offset: 0x00050AD6
		// (set) Token: 0x06001487 RID: 5255 RVA: 0x000528DE File Offset: 0x00050ADE
		[DataSourceProperty]
		public MBBindingList<EncyclopediaDwellingVM> Dwellings
		{
			get
			{
				return this._dwellings;
			}
			set
			{
				if (value != this._dwellings)
				{
					this._dwellings = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaDwellingVM>>(value, "Dwellings");
				}
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06001488 RID: 5256 RVA: 0x000528FC File Offset: 0x00050AFC
		// (set) Token: 0x06001489 RID: 5257 RVA: 0x00052904 File Offset: 0x00050B04
		[DataSourceProperty]
		public MBBindingList<EncyclopediaSettlementVM> Settlements
		{
			get
			{
				return this._settlements;
			}
			set
			{
				if (value != this._settlements)
				{
					this._settlements = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaSettlementVM>>(value, "Settlements");
				}
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x0600148A RID: 5258 RVA: 0x00052922 File Offset: 0x00050B22
		// (set) Token: 0x0600148B RID: 5259 RVA: 0x0005292A File Offset: 0x00050B2A
		[DataSourceProperty]
		public MBBindingList<EncyclopediaFamilyMemberVM> Family
		{
			get
			{
				return this._family;
			}
			set
			{
				if (value != this._family)
				{
					this._family = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaFamilyMemberVM>>(value, "Family");
				}
			}
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x0600148C RID: 5260 RVA: 0x00052948 File Offset: 0x00050B48
		// (set) Token: 0x0600148D RID: 5261 RVA: 0x00052950 File Offset: 0x00050B50
		[DataSourceProperty]
		public MBBindingList<HeroVM> Companions
		{
			get
			{
				return this._companions;
			}
			set
			{
				if (value != this._companions)
				{
					this._companions = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroVM>>(value, "Companions");
				}
			}
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x0600148E RID: 5262 RVA: 0x0005296E File Offset: 0x00050B6E
		// (set) Token: 0x0600148F RID: 5263 RVA: 0x00052976 File Offset: 0x00050B76
		[DataSourceProperty]
		public MBBindingList<HeroVM> Enemies
		{
			get
			{
				return this._enemies;
			}
			set
			{
				if (value != this._enemies)
				{
					this._enemies = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroVM>>(value, "Enemies");
				}
			}
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06001490 RID: 5264 RVA: 0x00052994 File Offset: 0x00050B94
		// (set) Token: 0x06001491 RID: 5265 RVA: 0x0005299C File Offset: 0x00050B9C
		[DataSourceProperty]
		public MBBindingList<HeroVM> Allies
		{
			get
			{
				return this._allies;
			}
			set
			{
				if (value != this._allies)
				{
					this._allies = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroVM>>(value, "Allies");
				}
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06001492 RID: 5266 RVA: 0x000529BA File Offset: 0x00050BBA
		// (set) Token: 0x06001493 RID: 5267 RVA: 0x000529C2 File Offset: 0x00050BC2
		[DataSourceProperty]
		public MBBindingList<EncyclopediaHistoryEventVM> History
		{
			get
			{
				return this._history;
			}
			set
			{
				if (value != this._history)
				{
					this._history = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaHistoryEventVM>>(value, "History");
				}
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06001494 RID: 5268 RVA: 0x000529E0 File Offset: 0x00050BE0
		// (set) Token: 0x06001495 RID: 5269 RVA: 0x000529E8 File Offset: 0x00050BE8
		[DataSourceProperty]
		public bool HasNeutralClan
		{
			get
			{
				return this._hasNeutralClan;
			}
			set
			{
				if (value != this._hasNeutralClan)
				{
					this._hasNeutralClan = value;
					base.OnPropertyChangedWithValue(value, "HasNeutralClan");
				}
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06001496 RID: 5270 RVA: 0x00052A06 File Offset: 0x00050C06
		// (set) Token: 0x06001497 RID: 5271 RVA: 0x00052A0E File Offset: 0x00050C0E
		[DataSourceProperty]
		public bool IsDead
		{
			get
			{
				return this._isDead;
			}
			set
			{
				if (value != this._isDead)
				{
					this._isDead = value;
					base.OnPropertyChanged("IsAlive");
					base.OnPropertyChangedWithValue(value, "IsDead");
				}
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06001498 RID: 5272 RVA: 0x00052A37 File Offset: 0x00050C37
		// (set) Token: 0x06001499 RID: 5273 RVA: 0x00052A3F File Offset: 0x00050C3F
		[DataSourceProperty]
		public bool IsInformationHidden
		{
			get
			{
				return this._isInformationHidden;
			}
			set
			{
				if (value != this._isInformationHidden)
				{
					this._isInformationHidden = value;
					base.OnPropertyChangedWithValue(value, "IsInformationHidden");
				}
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x0600149A RID: 5274 RVA: 0x00052A5D File Offset: 0x00050C5D
		// (set) Token: 0x0600149B RID: 5275 RVA: 0x00052A65 File Offset: 0x00050C65
		[DataSourceProperty]
		public string InformationText
		{
			get
			{
				return this._informationText;
			}
			set
			{
				if (value != this._informationText)
				{
					this._informationText = value;
					base.OnPropertyChangedWithValue<string>(value, "InformationText");
				}
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x0600149C RID: 5276 RVA: 0x00052A88 File Offset: 0x00050C88
		// (set) Token: 0x0600149D RID: 5277 RVA: 0x00052A90 File Offset: 0x00050C90
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

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x0600149E RID: 5278 RVA: 0x00052AAE File Offset: 0x00050CAE
		// (set) Token: 0x0600149F RID: 5279 RVA: 0x00052AB6 File Offset: 0x00050CB6
		[DataSourceProperty]
		public bool HasAnySkills
		{
			get
			{
				return this._hasAnySkills;
			}
			set
			{
				if (value != this._hasAnySkills)
				{
					this._hasAnySkills = value;
					base.OnPropertyChangedWithValue(value, "HasAnySkills");
				}
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x060014A0 RID: 5280 RVA: 0x00052AD4 File Offset: 0x00050CD4
		// (set) Token: 0x060014A1 RID: 5281 RVA: 0x00052ADC File Offset: 0x00050CDC
		[DataSourceProperty]
		public MBBindingList<HeroVM> AdditionalEnemies
		{
			get
			{
				return this._additionalEnemies;
			}
			set
			{
				if (value != this._additionalEnemies)
				{
					this._additionalEnemies = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroVM>>(value, "AdditionalEnemies");
				}
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x060014A2 RID: 5282 RVA: 0x00052AFA File Offset: 0x00050CFA
		// (set) Token: 0x060014A3 RID: 5283 RVA: 0x00052B02 File Offset: 0x00050D02
		[DataSourceProperty]
		public MBBindingList<HeroVM> AdditionalAllies
		{
			get
			{
				return this._additionalAllies;
			}
			set
			{
				if (value != this._additionalAllies)
				{
					this._additionalAllies = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroVM>>(value, "AdditionalAllies");
				}
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x060014A4 RID: 5284 RVA: 0x00052B20 File Offset: 0x00050D20
		// (set) Token: 0x060014A5 RID: 5285 RVA: 0x00052B28 File Offset: 0x00050D28
		[DataSourceProperty]
		public bool AnyAdditionalAllies
		{
			get
			{
				return this._anyAdditionalAllies;
			}
			set
			{
				if (value != this._anyAdditionalAllies)
				{
					this._anyAdditionalAllies = value;
					base.OnPropertyChangedWithValue(value, "AnyAdditionalAllies");
				}
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x060014A6 RID: 5286 RVA: 0x00052B46 File Offset: 0x00050D46
		// (set) Token: 0x060014A7 RID: 5287 RVA: 0x00052B4E File Offset: 0x00050D4E
		[DataSourceProperty]
		public bool AnyAdditionalEnemies
		{
			get
			{
				return this._anyAdditionalEnemies;
			}
			set
			{
				if (value != this._anyAdditionalEnemies)
				{
					this._anyAdditionalEnemies = value;
					base.OnPropertyChangedWithValue(value, "AnyAdditionalEnemies");
				}
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x060014A8 RID: 5288 RVA: 0x00052B6C File Offset: 0x00050D6C
		// (set) Token: 0x060014A9 RID: 5289 RVA: 0x00052B74 File Offset: 0x00050D74
		[DataSourceProperty]
		public string AdditionalAlliesString
		{
			get
			{
				return this._additionalAlliesString;
			}
			set
			{
				if (value != this._additionalAlliesString)
				{
					this._additionalAlliesString = value;
					base.OnPropertyChangedWithValue<string>(value, "AdditionalAlliesString");
				}
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x060014AA RID: 5290 RVA: 0x00052B97 File Offset: 0x00050D97
		// (set) Token: 0x060014AB RID: 5291 RVA: 0x00052B9F File Offset: 0x00050D9F
		[DataSourceProperty]
		public string AdditionalEnemiesString
		{
			get
			{
				return this._additionalEnemiesString;
			}
			set
			{
				if (value != this._additionalEnemiesString)
				{
					this._additionalEnemiesString = value;
					base.OnPropertyChangedWithValue<string>(value, "AdditionalEnemiesString");
				}
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x060014AC RID: 5292 RVA: 0x00052BC2 File Offset: 0x00050DC2
		// (set) Token: 0x060014AD RID: 5293 RVA: 0x00052BCA File Offset: 0x00050DCA
		[DataSourceProperty]
		public BasicTooltipViewModel AdditionalAlliesHint
		{
			get
			{
				return this._additionalAlliesHint;
			}
			set
			{
				if (value != this._additionalAlliesHint)
				{
					this._additionalAlliesHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "AdditionalAlliesHint");
				}
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x060014AE RID: 5294 RVA: 0x00052BE8 File Offset: 0x00050DE8
		// (set) Token: 0x060014AF RID: 5295 RVA: 0x00052BF0 File Offset: 0x00050DF0
		[DataSourceProperty]
		public BasicTooltipViewModel AdditionalEnemiesHint
		{
			get
			{
				return this._additionalEnemiesHint;
			}
			set
			{
				if (value != this._additionalEnemiesHint)
				{
					this._additionalEnemiesHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "AdditionalEnemiesHint");
				}
			}
		}

		// Token: 0x0400093D RID: 2365
		private readonly Hero _hero;

		// Token: 0x0400093E RID: 2366
		private readonly TextObject _infoHiddenReasonText;

		// Token: 0x0400093F RID: 2367
		private List<Hero> _allRelatedHeroes;

		// Token: 0x04000940 RID: 2368
		private readonly HeroRelationComparer _relationAscendingComparer;

		// Token: 0x04000941 RID: 2369
		private readonly HeroRelationComparer _relationDescendingComparer;

		// Token: 0x04000942 RID: 2370
		private const int _alliesEnemiesCapacity = 13;

		// Token: 0x04000943 RID: 2371
		private MBBindingList<HeroVM> _enemies;

		// Token: 0x04000944 RID: 2372
		private MBBindingList<HeroVM> _allies;

		// Token: 0x04000945 RID: 2373
		private MBBindingList<EncyclopediaFamilyMemberVM> _family;

		// Token: 0x04000946 RID: 2374
		private MBBindingList<HeroVM> _companions;

		// Token: 0x04000947 RID: 2375
		private MBBindingList<EncyclopediaSettlementVM> _settlements;

		// Token: 0x04000948 RID: 2376
		private MBBindingList<EncyclopediaDwellingVM> _dwellings;

		// Token: 0x04000949 RID: 2377
		private MBBindingList<EncyclopediaHistoryEventVM> _history;

		// Token: 0x0400094A RID: 2378
		private MBBindingList<EncyclopediaSkillVM> _skills;

		// Token: 0x0400094B RID: 2379
		private MBBindingList<StringPairItemVM> _stats;

		// Token: 0x0400094C RID: 2380
		private MBBindingList<EncyclopediaTraitItemVM> _traits;

		// Token: 0x0400094D RID: 2381
		private string _clanText;

		// Token: 0x0400094E RID: 2382
		private string _settlementsText;

		// Token: 0x0400094F RID: 2383
		private string _dwellingsText;

		// Token: 0x04000950 RID: 2384
		private string _alliesText;

		// Token: 0x04000951 RID: 2385
		private string _enemiesText;

		// Token: 0x04000952 RID: 2386
		private string _companionsText;

		// Token: 0x04000953 RID: 2387
		private string _lastSeenText;

		// Token: 0x04000954 RID: 2388
		private string _nameText;

		// Token: 0x04000955 RID: 2389
		private string _informationText;

		// Token: 0x04000956 RID: 2390
		private string _deceasedText;

		// Token: 0x04000957 RID: 2391
		private string _traitsText;

		// Token: 0x04000958 RID: 2392
		private string _skillsText;

		// Token: 0x04000959 RID: 2393
		private string _infoText;

		// Token: 0x0400095A RID: 2394
		private string _kingdomRankText;

		// Token: 0x0400095B RID: 2395
		private string _familyText;

		// Token: 0x0400095C RID: 2396
		private HeroViewModel _heroCharacter;

		// Token: 0x0400095D RID: 2397
		private bool _isCompanion;

		// Token: 0x0400095E RID: 2398
		private bool _isPregnant;

		// Token: 0x0400095F RID: 2399
		private bool _hasNeutralClan;

		// Token: 0x04000960 RID: 2400
		private bool _isDead;

		// Token: 0x04000961 RID: 2401
		private bool _isInformationHidden;

		// Token: 0x04000962 RID: 2402
		private HeroVM _master;

		// Token: 0x04000963 RID: 2403
		private EncyclopediaFactionVM _faction;

		// Token: 0x04000964 RID: 2404
		private string _masterText;

		// Token: 0x04000965 RID: 2405
		private HintViewModel _pregnantHint;

		// Token: 0x04000966 RID: 2406
		private bool _hasAnySkills;

		// Token: 0x04000967 RID: 2407
		private MBBindingList<HeroVM> _additionalAllies;

		// Token: 0x04000968 RID: 2408
		private MBBindingList<HeroVM> _additionalEnemies;

		// Token: 0x04000969 RID: 2409
		private bool _anyAdditionalAllies;

		// Token: 0x0400096A RID: 2410
		private bool _anyAdditionalEnemies;

		// Token: 0x0400096B RID: 2411
		private string _additionalAlliesString;

		// Token: 0x0400096C RID: 2412
		private string _additionalEnemiesString;

		// Token: 0x0400096D RID: 2413
		private BasicTooltipViewModel _additionalAlliesHint;

		// Token: 0x0400096E RID: 2414
		private BasicTooltipViewModel _additionalEnemiesHint;
	}
}
