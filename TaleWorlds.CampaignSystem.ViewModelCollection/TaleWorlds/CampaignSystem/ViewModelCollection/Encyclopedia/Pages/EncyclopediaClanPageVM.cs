using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages
{
	// Token: 0x020000D4 RID: 212
	[EncyclopediaViewModel(typeof(Clan))]
	public class EncyclopediaClanPageVM : EncyclopediaContentPageVM
	{
		// Token: 0x060013B6 RID: 5046 RVA: 0x0004F7E4 File Offset: 0x0004D9E4
		public EncyclopediaClanPageVM(EncyclopediaPageArgs args)
			: base(args)
		{
			this._faction = base.Obj as IFaction;
			this._clan = this._faction as Clan;
			this.Members = new MBBindingList<HeroVM>();
			this.Enemies = new MBBindingList<EncyclopediaFactionVM>();
			this.BloodFeuds = new MBBindingList<EncyclopediaFactionVM>();
			this.Settlements = new MBBindingList<EncyclopediaSettlementVM>();
			this.History = new MBBindingList<EncyclopediaHistoryEventVM>();
			this.ClanInfo = new MBBindingList<StringPairItemVM>();
			base.IsBookmarked = Campaign.Current.EncyclopediaManager.ViewDataTracker.IsEncyclopediaBookmarked(this._clan);
			this.RefreshValues();
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x0004F884 File Offset: 0x0004DA84
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.StrengthHint = new HintViewModel(GameTexts.FindText("str_strength", null), null);
			this.ProsperityHint = new HintViewModel(GameTexts.FindText("str_prosperity", null), null);
			this.MembersText = GameTexts.FindText("str_members", null).ToString();
			this.AlliesText = new TextObject("{=bfQLwMUp}Clans", null).ToString();
			this.EnemiesText = new TextObject("{=zZlWRZjO}Wars", null).ToString();
			this.BloodFeudsText = new TextObject("{=kUxmw6U3}Blood Feuds", null).ToString();
			this.SettlementsText = GameTexts.FindText("str_settlements", null).ToString();
			this.VillagesText = GameTexts.FindText("str_villages", null).ToString();
			this.DestroyedText = new TextObject("{=w8Yzf0F0}Destroyed", null).ToString();
			this.PartOfText = GameTexts.FindText("str_encyclopedia_clan_part_of_kingdom", null).ToString();
			this.LeaderText = GameTexts.FindText("str_leader", null).ToString();
			this.InfoText = GameTexts.FindText("str_info", null).ToString();
			base.UpdateBookmarkHintText();
			this.Refresh();
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x0004F9B0 File Offset: 0x0004DBB0
		public override void Refresh()
		{
			this.Members.Clear();
			this.Enemies.Clear();
			this.BloodFeuds.Clear();
			this.Settlements.Clear();
			this.History.Clear();
			this.ClanInfo.Clear();
			TextObject encyclopediaText = this._faction.EncyclopediaText;
			this.InformationText = ((encyclopediaText != null) ? encyclopediaText.ToString() : null);
			this.Leader = new HeroVM(this._faction.Leader, true);
			this.NameText = this._clan.Name.ToString();
			this.HasParentKingdom = this._clan.Kingdom != null;
			this.ParentKingdom = (this.HasParentKingdom ? new EncyclopediaFactionVM(((Clan)this._faction).Kingdom) : null);
			if (this._faction.IsKingdomFaction)
			{
				this.DescriptorText = GameTexts.FindText("str_kingdom_faction", null).ToString();
			}
			else if (this._faction.IsBanditFaction)
			{
				this.DescriptorText = GameTexts.FindText("str_bandit_faction", null).ToString();
			}
			else if (this._faction.IsMinorFaction)
			{
				this.DescriptorText = GameTexts.FindText("str_minor_faction", null).ToString();
			}
			int num = 0;
			float num2 = 0f;
			EncyclopediaPage pageOf = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Hero));
			IEnumerable<Hero> heroes = this._faction.Heroes;
			Clan clan = this._clan;
			foreach (Hero hero in heroes.Union<Hero>((clan != null) ? clan.Companions : null))
			{
				if (pageOf.IsValidEncyclopediaItem(hero))
				{
					if (hero != this.Leader.Hero)
					{
						this.Members.Add(new HeroVM(hero, true));
					}
					num += hero.Gold;
				}
			}
			this.Members.Sort(new HeroAgeComparer(false));
			this.Banner = new BannerImageIdentifierVM(this._faction.Banner, true);
			foreach (MobileParty mobileParty in MobileParty.AllLordParties)
			{
				if (mobileParty.MapFaction == this._faction && !mobileParty.IsDisbanding)
				{
					num2 += mobileParty.Party.CalculateCurrentStrength();
				}
			}
			this.ProsperityText = num.ToString();
			this.StrengthText = num2.ToString();
			MBObjectBase mbobjectBase = this._faction as MBObjectBase;
			for (int i = Campaign.Current.LogEntryHistory.GameActionLogs.Count - 1; i >= 0; i--)
			{
				IEncyclopediaLog encyclopediaLog;
				if ((encyclopediaLog = Campaign.Current.LogEntryHistory.GameActionLogs[i] as IEncyclopediaLog) != null && encyclopediaLog.IsVisibleInEncyclopediaPageOf(mbobjectBase))
				{
					this.History.Add(new EncyclopediaHistoryEventVM(encyclopediaLog));
				}
			}
			EncyclopediaPage pageOf2 = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Clan));
			foreach (IFaction faction in Campaign.Current.Factions.OrderBy<IFaction, bool>((IFaction x) => !x.IsKingdomFaction).ThenBy<IFaction, string>((IFaction f) => f.Name.ToString()))
			{
				IFaction mapFaction = faction.MapFaction;
				if (pageOf2.IsValidEncyclopediaItem(mapFaction) && mapFaction != this._faction.MapFaction && mapFaction != this._faction && !mapFaction.IsBanditFaction && FactionManager.IsAtWarAgainstFaction(this._faction.MapFaction, mapFaction) && !this.Enemies.Any<EncyclopediaFactionVM>((EncyclopediaFactionVM x) => x.Faction == mapFaction))
				{
					this.Enemies.Add(new EncyclopediaFactionVM(mapFaction));
				}
			}
			if (this._clan != Clan.PlayerClan.MapFaction && this._clan != Clan.PlayerClan)
			{
				if (this._clan.HasBloodFeudWithPlayer)
				{
					this.BloodFeuds.Add(new EncyclopediaFactionVM(Clan.PlayerClan));
				}
			}
			else
			{
				foreach (Clan clan2 in Campaign.Current.Clans.OrderBy<Clan, string>((Clan x) => x.Name.ToString()))
				{
					if (pageOf2.IsValidEncyclopediaItem(clan2) && clan2 != this._faction.MapFaction && clan2 != this._faction && clan2.HasBloodFeudWithPlayer)
					{
						this.BloodFeuds.Add(new EncyclopediaFactionVM(clan2));
					}
				}
			}
			EncyclopediaPage pageOf3 = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Settlement));
			foreach (Settlement settlement in from s in Settlement.All
				orderby s.IsVillage, s.IsCastle, s.IsTown
				select s)
			{
				if ((settlement.MapFaction == this._faction || (settlement.OwnerClan == this._faction && settlement.OwnerClan.Leader != null)) && pageOf3.IsValidEncyclopediaItem(settlement) && (settlement.IsTown || settlement.IsCastle))
				{
					this.Settlements.Add(new EncyclopediaSettlementVM(settlement));
				}
			}
			GameTexts.SetVariable("LEFT", new TextObject("{=tTLvo8sM}Clan Tier", null).ToString());
			this.ClanInfo.Add(new StringPairItemVM(GameTexts.FindText("str_LEFT_colon", null).ToString(), this._clan.Tier.ToString(), null));
			GameTexts.SetVariable("LEFT", new TextObject("{=ODEnkg0o}Clan Strength", null).ToString());
			this.ClanInfo.Add(new StringPairItemVM(GameTexts.FindText("str_LEFT_colon", null).ToString(), this._clan.CurrentTotalStrength.ToString("F0"), null));
			GameTexts.SetVariable("LEFT", GameTexts.FindText("str_wealth", null).ToString());
			this.ClanInfo.Add(new StringPairItemVM(GameTexts.FindText("str_LEFT_colon", null).ToString(), CampaignUIHelper.GetClanWealthStatusText(this._clan), null));
			this.IsClanDestroyed = this._clan.IsEliminated;
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x00050100 File Offset: 0x0004E300
		public override string GetName()
		{
			return this._clan.Name.ToString();
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x00050114 File Offset: 0x0004E314
		public override string GetNavigationBarURL()
		{
			return HyperlinkTexts.GetGenericHyperlinkText("Home", GameTexts.FindText("str_encyclopedia_home", null).ToString()) + " \\ " + HyperlinkTexts.GetGenericHyperlinkText("ListPage-Clans", GameTexts.FindText("str_encyclopedia_clans", null).ToString()) + " \\ " + this.GetName();
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x0005017C File Offset: 0x0004E37C
		public override void ExecuteSwitchBookmarkedState()
		{
			base.ExecuteSwitchBookmarkedState();
			if (base.IsBookmarked)
			{
				Campaign.Current.EncyclopediaManager.ViewDataTracker.AddEncyclopediaBookmarkToItem(this._clan);
				return;
			}
			Campaign.Current.EncyclopediaManager.ViewDataTracker.RemoveEncyclopediaBookmarkFromItem(this._clan);
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x060013BC RID: 5052 RVA: 0x000501CC File Offset: 0x0004E3CC
		// (set) Token: 0x060013BD RID: 5053 RVA: 0x000501D4 File Offset: 0x0004E3D4
		[DataSourceProperty]
		public MBBindingList<StringPairItemVM> ClanInfo
		{
			get
			{
				return this._clanInfo;
			}
			set
			{
				if (value != this._clanInfo)
				{
					this._clanInfo = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemVM>>(value, "ClanInfo");
				}
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x060013BE RID: 5054 RVA: 0x000501F2 File Offset: 0x0004E3F2
		// (set) Token: 0x060013BF RID: 5055 RVA: 0x000501FA File Offset: 0x0004E3FA
		[DataSourceProperty]
		public MBBindingList<HeroVM> Members
		{
			get
			{
				return this._members;
			}
			set
			{
				if (value != this._members)
				{
					this._members = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroVM>>(value, "Members");
				}
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x060013C0 RID: 5056 RVA: 0x00050218 File Offset: 0x0004E418
		// (set) Token: 0x060013C1 RID: 5057 RVA: 0x00050220 File Offset: 0x0004E420
		[DataSourceProperty]
		public MBBindingList<EncyclopediaFactionVM> Enemies
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
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaFactionVM>>(value, "Enemies");
				}
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x060013C2 RID: 5058 RVA: 0x0005023E File Offset: 0x0004E43E
		// (set) Token: 0x060013C3 RID: 5059 RVA: 0x00050246 File Offset: 0x0004E446
		[DataSourceProperty]
		public MBBindingList<EncyclopediaFactionVM> BloodFeuds
		{
			get
			{
				return this._bloodFeuds;
			}
			set
			{
				if (value != this._bloodFeuds)
				{
					this._bloodFeuds = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaFactionVM>>(value, "BloodFeuds");
				}
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x060013C4 RID: 5060 RVA: 0x00050264 File Offset: 0x0004E464
		// (set) Token: 0x060013C5 RID: 5061 RVA: 0x0005026C File Offset: 0x0004E46C
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

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x060013C6 RID: 5062 RVA: 0x0005028A File Offset: 0x0004E48A
		// (set) Token: 0x060013C7 RID: 5063 RVA: 0x00050292 File Offset: 0x0004E492
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

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x060013C8 RID: 5064 RVA: 0x000502B0 File Offset: 0x0004E4B0
		// (set) Token: 0x060013C9 RID: 5065 RVA: 0x000502B8 File Offset: 0x0004E4B8
		[DataSourceProperty]
		public EncyclopediaFactionVM ParentKingdom
		{
			get
			{
				return this._parentKingdom;
			}
			set
			{
				if (value != this._parentKingdom)
				{
					this._parentKingdom = value;
					base.OnPropertyChangedWithValue<EncyclopediaFactionVM>(value, "ParentKingdom");
				}
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x060013CA RID: 5066 RVA: 0x000502D6 File Offset: 0x0004E4D6
		// (set) Token: 0x060013CB RID: 5067 RVA: 0x000502DE File Offset: 0x0004E4DE
		[DataSourceProperty]
		public bool HasParentKingdom
		{
			get
			{
				return this._hasParentKingdom;
			}
			set
			{
				if (value != this._hasParentKingdom)
				{
					this._hasParentKingdom = value;
					base.OnPropertyChangedWithValue(value, "HasParentKingdom");
				}
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x060013CC RID: 5068 RVA: 0x000502FC File Offset: 0x0004E4FC
		// (set) Token: 0x060013CD RID: 5069 RVA: 0x00050304 File Offset: 0x0004E504
		[DataSourceProperty]
		public bool IsClanDestroyed
		{
			get
			{
				return this._isClanDestroyed;
			}
			set
			{
				if (value != this._isClanDestroyed)
				{
					this._isClanDestroyed = value;
					base.OnPropertyChangedWithValue(value, "IsClanDestroyed");
				}
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x060013CE RID: 5070 RVA: 0x00050322 File Offset: 0x0004E522
		// (set) Token: 0x060013CF RID: 5071 RVA: 0x0005032A File Offset: 0x0004E52A
		[DataSourceProperty]
		public string DestroyedText
		{
			get
			{
				return this._destroyedText;
			}
			set
			{
				if (value != this._destroyedText)
				{
					this._destroyedText = value;
					base.OnPropertyChangedWithValue<string>(value, "DestroyedText");
				}
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x060013D0 RID: 5072 RVA: 0x0005034D File Offset: 0x0004E54D
		// (set) Token: 0x060013D1 RID: 5073 RVA: 0x00050355 File Offset: 0x0004E555
		[DataSourceProperty]
		public string PartOfText
		{
			get
			{
				return this._partOfText;
			}
			set
			{
				if (value != this._partOfText)
				{
					this._partOfText = value;
					base.OnPropertyChangedWithValue<string>(value, "PartOfText");
				}
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x060013D2 RID: 5074 RVA: 0x00050378 File Offset: 0x0004E578
		// (set) Token: 0x060013D3 RID: 5075 RVA: 0x00050380 File Offset: 0x0004E580
		[DataSourceProperty]
		public string TierText
		{
			get
			{
				return this._tierText;
			}
			set
			{
				if (value != this._tierText)
				{
					this._tierText = value;
					base.OnPropertyChangedWithValue<string>(value, "TierText");
				}
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x060013D4 RID: 5076 RVA: 0x000503A3 File Offset: 0x0004E5A3
		// (set) Token: 0x060013D5 RID: 5077 RVA: 0x000503AB File Offset: 0x0004E5AB
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

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x060013D6 RID: 5078 RVA: 0x000503CE File Offset: 0x0004E5CE
		// (set) Token: 0x060013D7 RID: 5079 RVA: 0x000503D6 File Offset: 0x0004E5D6
		[DataSourceProperty]
		public HeroVM Leader
		{
			get
			{
				return this._leader;
			}
			set
			{
				if (value != this._leader)
				{
					this._leader = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "Leader");
				}
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x060013D8 RID: 5080 RVA: 0x000503F4 File Offset: 0x0004E5F4
		// (set) Token: 0x060013D9 RID: 5081 RVA: 0x000503FC File Offset: 0x0004E5FC
		[DataSourceProperty]
		public BannerImageIdentifierVM Banner
		{
			get
			{
				return this._banner;
			}
			set
			{
				if (value != this._banner)
				{
					this._banner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Banner");
				}
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x060013DA RID: 5082 RVA: 0x0005041A File Offset: 0x0004E61A
		// (set) Token: 0x060013DB RID: 5083 RVA: 0x00050422 File Offset: 0x0004E622
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

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x060013DC RID: 5084 RVA: 0x00050445 File Offset: 0x0004E645
		// (set) Token: 0x060013DD RID: 5085 RVA: 0x0005044D File Offset: 0x0004E64D
		[DataSourceProperty]
		public string MembersText
		{
			get
			{
				return this._membersText;
			}
			set
			{
				if (value != this._membersText)
				{
					this._membersText = value;
					base.OnPropertyChangedWithValue<string>(value, "MembersText");
				}
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x060013DE RID: 5086 RVA: 0x00050470 File Offset: 0x0004E670
		// (set) Token: 0x060013DF RID: 5087 RVA: 0x00050478 File Offset: 0x0004E678
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

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x060013E0 RID: 5088 RVA: 0x0005049B File Offset: 0x0004E69B
		// (set) Token: 0x060013E1 RID: 5089 RVA: 0x000504A3 File Offset: 0x0004E6A3
		[DataSourceProperty]
		public string BloodFeudsText
		{
			get
			{
				return this._bloodFeudsText;
			}
			set
			{
				if (value != this._bloodFeudsText)
				{
					this._bloodFeudsText = value;
					base.OnPropertyChangedWithValue<string>(value, "BloodFeudsText");
				}
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x060013E2 RID: 5090 RVA: 0x000504C6 File Offset: 0x0004E6C6
		// (set) Token: 0x060013E3 RID: 5091 RVA: 0x000504CE File Offset: 0x0004E6CE
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

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x060013E4 RID: 5092 RVA: 0x000504F1 File Offset: 0x0004E6F1
		// (set) Token: 0x060013E5 RID: 5093 RVA: 0x000504F9 File Offset: 0x0004E6F9
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

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x060013E6 RID: 5094 RVA: 0x0005051C File Offset: 0x0004E71C
		// (set) Token: 0x060013E7 RID: 5095 RVA: 0x00050524 File Offset: 0x0004E724
		[DataSourceProperty]
		public string VillagesText
		{
			get
			{
				return this._villagesText;
			}
			set
			{
				if (value != this._villagesText)
				{
					this._villagesText = value;
					base.OnPropertyChangedWithValue<string>(value, "VillagesText");
				}
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x060013E8 RID: 5096 RVA: 0x00050547 File Offset: 0x0004E747
		// (set) Token: 0x060013E9 RID: 5097 RVA: 0x0005054F File Offset: 0x0004E74F
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

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x060013EA RID: 5098 RVA: 0x00050572 File Offset: 0x0004E772
		// (set) Token: 0x060013EB RID: 5099 RVA: 0x0005057A File Offset: 0x0004E77A
		[DataSourceProperty]
		public string LeaderText
		{
			get
			{
				return this._leaderText;
			}
			set
			{
				if (value != this._leaderText)
				{
					this._leaderText = value;
					base.OnPropertyChangedWithValue<string>(value, "LeaderText");
				}
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x060013EC RID: 5100 RVA: 0x0005059D File Offset: 0x0004E79D
		// (set) Token: 0x060013ED RID: 5101 RVA: 0x000505A5 File Offset: 0x0004E7A5
		[DataSourceProperty]
		public string DescriptorText
		{
			get
			{
				return this._descriptorText;
			}
			set
			{
				if (value != this._descriptorText)
				{
					this._descriptorText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptorText");
				}
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x060013EE RID: 5102 RVA: 0x000505C8 File Offset: 0x0004E7C8
		// (set) Token: 0x060013EF RID: 5103 RVA: 0x000505D0 File Offset: 0x0004E7D0
		[DataSourceProperty]
		public string ProsperityText
		{
			get
			{
				return this._prosperityText;
			}
			set
			{
				if (value != this._prosperityText)
				{
					this._prosperityText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProsperityText");
				}
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x060013F0 RID: 5104 RVA: 0x000505F3 File Offset: 0x0004E7F3
		// (set) Token: 0x060013F1 RID: 5105 RVA: 0x000505FB File Offset: 0x0004E7FB
		[DataSourceProperty]
		public string StrengthText
		{
			get
			{
				return this._strengthText;
			}
			set
			{
				if (value != this._strengthText)
				{
					this._strengthText = value;
					base.OnPropertyChangedWithValue<string>(value, "StrengthText");
				}
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x060013F2 RID: 5106 RVA: 0x0005061E File Offset: 0x0004E81E
		// (set) Token: 0x060013F3 RID: 5107 RVA: 0x00050626 File Offset: 0x0004E826
		[DataSourceProperty]
		public HintViewModel ProsperityHint
		{
			get
			{
				return this._prosperityHint;
			}
			set
			{
				if (value != this._prosperityHint)
				{
					this._prosperityHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ProsperityHint");
				}
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x060013F4 RID: 5108 RVA: 0x00050644 File Offset: 0x0004E844
		// (set) Token: 0x060013F5 RID: 5109 RVA: 0x0005064C File Offset: 0x0004E84C
		[DataSourceProperty]
		public HintViewModel StrengthHint
		{
			get
			{
				return this._strengthHint;
			}
			set
			{
				if (value != this._strengthHint)
				{
					this._strengthHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "StrengthHint");
				}
			}
		}

		// Token: 0x040008F8 RID: 2296
		private readonly IFaction _faction;

		// Token: 0x040008F9 RID: 2297
		private readonly Clan _clan;

		// Token: 0x040008FA RID: 2298
		private MBBindingList<StringPairItemVM> _clanInfo;

		// Token: 0x040008FB RID: 2299
		private MBBindingList<HeroVM> _members;

		// Token: 0x040008FC RID: 2300
		private MBBindingList<EncyclopediaFactionVM> _enemies;

		// Token: 0x040008FD RID: 2301
		private MBBindingList<EncyclopediaFactionVM> _bloodFeuds;

		// Token: 0x040008FE RID: 2302
		private MBBindingList<EncyclopediaSettlementVM> _settlements;

		// Token: 0x040008FF RID: 2303
		private MBBindingList<EncyclopediaHistoryEventVM> _history;

		// Token: 0x04000900 RID: 2304
		private HeroVM _leader;

		// Token: 0x04000901 RID: 2305
		private BannerImageIdentifierVM _banner;

		// Token: 0x04000902 RID: 2306
		private string _membersText;

		// Token: 0x04000903 RID: 2307
		private string _enemiesText;

		// Token: 0x04000904 RID: 2308
		private string _bloodFeudsText;

		// Token: 0x04000905 RID: 2309
		private string _alliesText;

		// Token: 0x04000906 RID: 2310
		private string _settlementsText;

		// Token: 0x04000907 RID: 2311
		private string _villagesText;

		// Token: 0x04000908 RID: 2312
		private string _leaderText;

		// Token: 0x04000909 RID: 2313
		private string _descriptorText;

		// Token: 0x0400090A RID: 2314
		private string _informationText;

		// Token: 0x0400090B RID: 2315
		private string _prosperityText;

		// Token: 0x0400090C RID: 2316
		private string _strengthText;

		// Token: 0x0400090D RID: 2317
		private string _destroyedText;

		// Token: 0x0400090E RID: 2318
		private string _partOfText;

		// Token: 0x0400090F RID: 2319
		private string _tierText;

		// Token: 0x04000910 RID: 2320
		private string _infoText;

		// Token: 0x04000911 RID: 2321
		private HintViewModel _prosperityHint;

		// Token: 0x04000912 RID: 2322
		private HintViewModel _strengthHint;

		// Token: 0x04000913 RID: 2323
		private EncyclopediaFactionVM _parentKingdom;

		// Token: 0x04000914 RID: 2324
		private string _nameText;

		// Token: 0x04000915 RID: 2325
		private bool _hasParentKingdom;

		// Token: 0x04000916 RID: 2326
		private bool _isClanDestroyed;
	}
}
