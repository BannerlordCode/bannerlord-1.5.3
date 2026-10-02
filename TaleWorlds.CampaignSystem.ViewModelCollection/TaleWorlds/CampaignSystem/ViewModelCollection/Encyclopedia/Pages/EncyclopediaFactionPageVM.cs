using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages
{
	// Token: 0x020000D8 RID: 216
	[EncyclopediaViewModel(typeof(Kingdom))]
	public class EncyclopediaFactionPageVM : EncyclopediaContentPageVM
	{
		// Token: 0x06001415 RID: 5141 RVA: 0x00050B2C File Offset: 0x0004ED2C
		public EncyclopediaFactionPageVM(EncyclopediaPageArgs args)
			: base(args)
		{
			this._faction = base.Obj as Kingdom;
			this.Clans = new MBBindingList<EncyclopediaFactionVM>();
			this.Enemies = new MBBindingList<EncyclopediaFactionVM>();
			this.TradeAgreements = new MBBindingList<EncyclopediaFactionVM>();
			this.Alliances = new MBBindingList<EncyclopediaFactionVM>();
			this.Settlements = new MBBindingList<EncyclopediaSettlementVM>();
			this.History = new MBBindingList<EncyclopediaHistoryEventVM>();
			base.IsBookmarked = Campaign.Current.EncyclopediaManager.ViewDataTracker.IsEncyclopediaBookmarked(this._faction);
			this.RefreshValues();
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x00050BBC File Offset: 0x0004EDBC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.StrengthHint = new HintViewModel(GameTexts.FindText("str_strength", null), null);
			this.ProsperityHint = new HintViewModel(GameTexts.FindText("str_prosperity", null), null);
			this.MembersText = GameTexts.FindText("str_members", null).ToString();
			this.ClansText = new TextObject("{=bfQLwMUp}Clans", null).ToString();
			this.EnemiesText = new TextObject("{=zZlWRZjO}Wars", null).ToString();
			this.TradeAgreementsText = new TextObject("{=pWyRg13f}Trade Agreements", null).ToString();
			this.AlliancesText = new TextObject("{=f90A6PGd}Alliances", null).ToString();
			this.SettlementsText = new TextObject("{=LBNzsqyb}Fiefs", null).ToString();
			this.VillagesText = GameTexts.FindText("str_villages", null).ToString();
			TextObject encyclopediaText = this._faction.EncyclopediaText;
			this.InformationText = ((encyclopediaText != null) ? encyclopediaText.ToString() : null) ?? string.Empty;
			base.UpdateBookmarkHintText();
			this.Refresh();
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x00050CCC File Offset: 0x0004EECC
		public override void Refresh()
		{
			base.IsLoadingOver = false;
			this.Clans.Clear();
			this.Enemies.Clear();
			this.TradeAgreements.Clear();
			this.Alliances.Clear();
			this.Settlements.Clear();
			this.History.Clear();
			this.Leader = new HeroVM(this._faction.Leader, false);
			this.LeaderText = GameTexts.FindText("str_leader", null).ToString();
			this.NameText = this._faction.Name.ToString();
			this.DescriptorText = GameTexts.FindText("str_kingdom_faction", null).ToString();
			int num = 0;
			float num2 = 0f;
			EncyclopediaPage pageOf = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Hero));
			foreach (Hero hero in this._faction.AliveLords)
			{
				if (pageOf.IsValidEncyclopediaItem(hero))
				{
					num += hero.Gold;
				}
			}
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
			MBObjectBase faction = this._faction;
			for (int i = Campaign.Current.LogEntryHistory.GameActionLogs.Count - 1; i >= 0; i--)
			{
				IEncyclopediaLog encyclopediaLog;
				if ((encyclopediaLog = Campaign.Current.LogEntryHistory.GameActionLogs[i] as IEncyclopediaLog) != null && encyclopediaLog.IsVisibleInEncyclopediaPageOf(faction))
				{
					this.History.Add(new EncyclopediaHistoryEventVM(encyclopediaLog));
				}
			}
			EncyclopediaPage pageOf2 = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Clan));
			List<IFaction> list = Campaign.Current.Factions.OrderBy<IFaction, bool>((IFaction x) => !x.IsKingdomFaction).ThenBy<IFaction, string>((IFaction f) => f.Name.ToString()).ToList<IFaction>();
			HashSet<IFaction> hashSet = new HashSet<IFaction>();
			HashSet<IFaction> hashSet2 = new HashSet<IFaction>();
			HashSet<IFaction> hashSet3 = new HashSet<IFaction>();
			foreach (IFaction faction2 in list)
			{
				if (pageOf2.IsValidEncyclopediaItem(faction2) && faction2 != this._faction)
				{
					if (!faction2.IsBanditFaction && FactionManager.IsAtWarAgainstFaction(this._faction, faction2.MapFaction) && !hashSet.Contains(faction2.MapFaction))
					{
						hashSet.Add(faction2.MapFaction);
						this.Enemies.Add(new EncyclopediaFactionVM(faction2.MapFaction));
					}
					Kingdom kingdom;
					if ((kingdom = faction2 as Kingdom) != null)
					{
						if (this.HasTradeAgreementWithFaction(this._faction, kingdom.MapFaction) && !hashSet2.Contains(kingdom.MapFaction))
						{
							hashSet2.Add(kingdom.MapFaction);
							this.TradeAgreements.Add(new EncyclopediaFactionVM(kingdom.MapFaction));
						}
						if (DiplomacyHelper.HasAllianceWithFaction(this._faction, kingdom.MapFaction) && !hashSet3.Contains(kingdom.MapFaction))
						{
							hashSet3.Add(kingdom.MapFaction);
							this.Alliances.Add(new EncyclopediaFactionVM(kingdom.MapFaction));
						}
					}
				}
			}
			foreach (Clan clan in Campaign.Current.Clans.Where<Clan>((Clan c) => c.Kingdom == this._faction))
			{
				this.Clans.Add(new EncyclopediaFactionVM(clan));
			}
			EncyclopediaPage pageOf3 = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Settlement));
			foreach (Settlement settlement in from s in Settlement.All
				where s.IsTown || s.IsCastle
				orderby s.IsCastle, s.IsTown
				select s)
			{
				if ((settlement.MapFaction == this._faction || (settlement.OwnerClan == this._faction.RulingClan && settlement.OwnerClan.Leader != null)) && pageOf3.IsValidEncyclopediaItem(settlement))
				{
					this.Settlements.Add(new EncyclopediaSettlementVM(settlement));
				}
			}
			base.IsLoadingOver = true;
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x00051280 File Offset: 0x0004F480
		private bool HasTradeAgreementWithFaction(IFaction faction1, IFaction faction2)
		{
			if (faction1 == null || faction2 == null || faction1 == faction2 || faction1.IsEliminated || faction2.IsEliminated || !faction1.IsKingdomFaction || !faction2.IsKingdomFaction)
			{
				return false;
			}
			ITradeAgreementsCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<ITradeAgreementsCampaignBehavior>();
			TradeAgreementsCampaignBehavior.TradeAgreement tradeAgreement;
			return campaignBehavior != null && campaignBehavior.HasTradeAgreement(faction1 as Kingdom, faction2 as Kingdom, out tradeAgreement);
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x000512DC File Offset: 0x0004F4DC
		public override string GetName()
		{
			return this._faction.Name.ToString();
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x000512F0 File Offset: 0x0004F4F0
		public override string GetNavigationBarURL()
		{
			return HyperlinkTexts.GetGenericHyperlinkText("Home", GameTexts.FindText("str_encyclopedia_home", null).ToString()) + " \\ " + HyperlinkTexts.GetGenericHyperlinkText("ListPage-Kingdoms", GameTexts.FindText("str_encyclopedia_kingdoms", null).ToString()) + " \\ " + this.GetName();
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x00051358 File Offset: 0x0004F558
		public override void ExecuteSwitchBookmarkedState()
		{
			base.ExecuteSwitchBookmarkedState();
			if (base.IsBookmarked)
			{
				Campaign.Current.EncyclopediaManager.ViewDataTracker.AddEncyclopediaBookmarkToItem(this._faction);
				return;
			}
			Campaign.Current.EncyclopediaManager.ViewDataTracker.RemoveEncyclopediaBookmarkFromItem(this._faction);
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x0600141C RID: 5148 RVA: 0x000513A8 File Offset: 0x0004F5A8
		// (set) Token: 0x0600141D RID: 5149 RVA: 0x000513B0 File Offset: 0x0004F5B0
		[DataSourceProperty]
		public MBBindingList<EncyclopediaFactionVM> Clans
		{
			get
			{
				return this._clans;
			}
			set
			{
				if (value != this._clans)
				{
					this._clans = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaFactionVM>>(value, "Clans");
				}
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x0600141E RID: 5150 RVA: 0x000513CE File Offset: 0x0004F5CE
		// (set) Token: 0x0600141F RID: 5151 RVA: 0x000513D6 File Offset: 0x0004F5D6
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

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06001420 RID: 5152 RVA: 0x000513F4 File Offset: 0x0004F5F4
		// (set) Token: 0x06001421 RID: 5153 RVA: 0x000513FC File Offset: 0x0004F5FC
		[DataSourceProperty]
		public MBBindingList<EncyclopediaFactionVM> TradeAgreements
		{
			get
			{
				return this._tradeAgreements;
			}
			set
			{
				if (value != this._tradeAgreements)
				{
					this._tradeAgreements = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaFactionVM>>(value, "TradeAgreements");
				}
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06001422 RID: 5154 RVA: 0x0005141A File Offset: 0x0004F61A
		// (set) Token: 0x06001423 RID: 5155 RVA: 0x00051422 File Offset: 0x0004F622
		[DataSourceProperty]
		public MBBindingList<EncyclopediaFactionVM> Alliances
		{
			get
			{
				return this._alliances;
			}
			set
			{
				if (value != this._alliances)
				{
					this._alliances = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaFactionVM>>(value, "Alliances");
				}
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06001424 RID: 5156 RVA: 0x00051440 File Offset: 0x0004F640
		// (set) Token: 0x06001425 RID: 5157 RVA: 0x00051448 File Offset: 0x0004F648
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

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06001426 RID: 5158 RVA: 0x00051466 File Offset: 0x0004F666
		// (set) Token: 0x06001427 RID: 5159 RVA: 0x0005146E File Offset: 0x0004F66E
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

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06001428 RID: 5160 RVA: 0x0005148C File Offset: 0x0004F68C
		// (set) Token: 0x06001429 RID: 5161 RVA: 0x00051494 File Offset: 0x0004F694
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

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x0600142A RID: 5162 RVA: 0x000514B2 File Offset: 0x0004F6B2
		// (set) Token: 0x0600142B RID: 5163 RVA: 0x000514BA File Offset: 0x0004F6BA
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

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x0600142C RID: 5164 RVA: 0x000514D8 File Offset: 0x0004F6D8
		// (set) Token: 0x0600142D RID: 5165 RVA: 0x000514E0 File Offset: 0x0004F6E0
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

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x0600142E RID: 5166 RVA: 0x00051503 File Offset: 0x0004F703
		// (set) Token: 0x0600142F RID: 5167 RVA: 0x0005150B File Offset: 0x0004F70B
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

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06001430 RID: 5168 RVA: 0x0005152E File Offset: 0x0004F72E
		// (set) Token: 0x06001431 RID: 5169 RVA: 0x00051536 File Offset: 0x0004F736
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

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06001432 RID: 5170 RVA: 0x00051559 File Offset: 0x0004F759
		// (set) Token: 0x06001433 RID: 5171 RVA: 0x00051561 File Offset: 0x0004F761
		[DataSourceProperty]
		public string TradeAgreementsText
		{
			get
			{
				return this._tradeAgreementsText;
			}
			set
			{
				if (value != this._tradeAgreementsText)
				{
					this._tradeAgreementsText = value;
					base.OnPropertyChangedWithValue<string>(value, "TradeAgreementsText");
				}
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06001434 RID: 5172 RVA: 0x00051584 File Offset: 0x0004F784
		// (set) Token: 0x06001435 RID: 5173 RVA: 0x0005158C File Offset: 0x0004F78C
		[DataSourceProperty]
		public string AlliancesText
		{
			get
			{
				return this._alliancesText;
			}
			set
			{
				if (value != this._alliancesText)
				{
					this._alliancesText = value;
					base.OnPropertyChangedWithValue<string>(value, "AlliancesText");
				}
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06001436 RID: 5174 RVA: 0x000515AF File Offset: 0x0004F7AF
		// (set) Token: 0x06001437 RID: 5175 RVA: 0x000515B7 File Offset: 0x0004F7B7
		[DataSourceProperty]
		public string ClansText
		{
			get
			{
				return this._clansText;
			}
			set
			{
				if (value != this._clansText)
				{
					this._clansText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClansText");
				}
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06001438 RID: 5176 RVA: 0x000515DA File Offset: 0x0004F7DA
		// (set) Token: 0x06001439 RID: 5177 RVA: 0x000515E2 File Offset: 0x0004F7E2
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

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x0600143A RID: 5178 RVA: 0x00051605 File Offset: 0x0004F805
		// (set) Token: 0x0600143B RID: 5179 RVA: 0x0005160D File Offset: 0x0004F80D
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

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x0600143C RID: 5180 RVA: 0x00051630 File Offset: 0x0004F830
		// (set) Token: 0x0600143D RID: 5181 RVA: 0x00051638 File Offset: 0x0004F838
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

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x0600143E RID: 5182 RVA: 0x0005165B File Offset: 0x0004F85B
		// (set) Token: 0x0600143F RID: 5183 RVA: 0x00051663 File Offset: 0x0004F863
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

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x06001440 RID: 5184 RVA: 0x00051686 File Offset: 0x0004F886
		// (set) Token: 0x06001441 RID: 5185 RVA: 0x0005168E File Offset: 0x0004F88E
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

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06001442 RID: 5186 RVA: 0x000516B1 File Offset: 0x0004F8B1
		// (set) Token: 0x06001443 RID: 5187 RVA: 0x000516B9 File Offset: 0x0004F8B9
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

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06001444 RID: 5188 RVA: 0x000516DC File Offset: 0x0004F8DC
		// (set) Token: 0x06001445 RID: 5189 RVA: 0x000516E4 File Offset: 0x0004F8E4
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

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06001446 RID: 5190 RVA: 0x00051707 File Offset: 0x0004F907
		// (set) Token: 0x06001447 RID: 5191 RVA: 0x0005170F File Offset: 0x0004F90F
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

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06001448 RID: 5192 RVA: 0x0005172D File Offset: 0x0004F92D
		// (set) Token: 0x06001449 RID: 5193 RVA: 0x00051735 File Offset: 0x0004F935
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

		// Token: 0x04000925 RID: 2341
		private Kingdom _faction;

		// Token: 0x04000926 RID: 2342
		private MBBindingList<EncyclopediaFactionVM> _clans;

		// Token: 0x04000927 RID: 2343
		private MBBindingList<EncyclopediaFactionVM> _enemies;

		// Token: 0x04000928 RID: 2344
		private MBBindingList<EncyclopediaFactionVM> _tradeAgreements;

		// Token: 0x04000929 RID: 2345
		private MBBindingList<EncyclopediaFactionVM> _alliances;

		// Token: 0x0400092A RID: 2346
		private MBBindingList<EncyclopediaSettlementVM> _settlements;

		// Token: 0x0400092B RID: 2347
		private MBBindingList<EncyclopediaHistoryEventVM> _history;

		// Token: 0x0400092C RID: 2348
		private HeroVM _leader;

		// Token: 0x0400092D RID: 2349
		private BannerImageIdentifierVM _banner;

		// Token: 0x0400092E RID: 2350
		private string _membersText;

		// Token: 0x0400092F RID: 2351
		private string _enemiesText;

		// Token: 0x04000930 RID: 2352
		private string _tradeAgreementsText;

		// Token: 0x04000931 RID: 2353
		private string _alliancesText;

		// Token: 0x04000932 RID: 2354
		private string _clansText;

		// Token: 0x04000933 RID: 2355
		private string _settlementsText;

		// Token: 0x04000934 RID: 2356
		private string _villagesText;

		// Token: 0x04000935 RID: 2357
		private string _leaderText;

		// Token: 0x04000936 RID: 2358
		private string _descriptorText;

		// Token: 0x04000937 RID: 2359
		private string _prosperityText;

		// Token: 0x04000938 RID: 2360
		private string _strengthText;

		// Token: 0x04000939 RID: 2361
		private string _informationText;

		// Token: 0x0400093A RID: 2362
		private HintViewModel _prosperityHint;

		// Token: 0x0400093B RID: 2363
		private HintViewModel _strengthHint;

		// Token: 0x0400093C RID: 2364
		private string _nameText;
	}
}
