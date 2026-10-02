using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions.ItemTypes
{
	// Token: 0x0200008A RID: 138
	public class TradeAgreementDecisionItemVM : DecisionItemBaseVM
	{
		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000B42 RID: 2882 RVA: 0x0002FE8F File Offset: 0x0002E08F
		private Kingdom _sourceFaction
		{
			get
			{
				return Hero.MainHero.Clan.Kingdom;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000B43 RID: 2883 RVA: 0x0002FEA0 File Offset: 0x0002E0A0
		public IFaction TargetFaction
		{
			get
			{
				return (this._decision as TradeAgreementDecision).TargetKingdom;
			}
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x0002FEB2 File Offset: 0x0002E0B2
		public TradeAgreementDecisionItemVM(TradeAgreementDecision decision, Action onDecisionOver)
			: base(decision, onDecisionOver)
		{
			this._tradeAgreementDecision = decision;
			base.DecisionType = 10;
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x0002FECC File Offset: 0x0002E0CC
		protected override void InitValues()
		{
			base.InitValues();
			TextObject textObject = GameTexts.FindText("str_kingdom_decision_trade_agreement", null);
			this.NameText = textObject.ToString();
			TextObject textObject2 = GameTexts.FindText("str_kingdom_decision_trade_agreement_desc", null);
			textObject2.SetTextVariable("FACTION", this.TargetFaction.Name);
			this.TradeAgreementDescriptionText = textObject2.ToString();
			this.SourceFactionBanner = new BannerImageIdentifierVM(this._sourceFaction.Banner, true);
			this.TargetFactionBanner = new BannerImageIdentifierVM(this.TargetFaction.Banner, true);
			this.LeaderText = GameTexts.FindText("str_leader", null).ToString();
			this.SourceFactionLeader = new HeroVM(this._sourceFaction.Leader, false);
			this.TargetFactionLeader = new HeroVM(this.TargetFaction.Leader, false);
			this.ComparedStats = new MBBindingList<KingdomWarComparableStatVM>();
			Kingdom kingdom = this.TargetFaction as Kingdom;
			string text = Color.FromUint(this._sourceFaction.Color).ToString();
			string text2 = Color.FromUint(kingdom.Color).ToString();
			KingdomWarComparableStatVM kingdomWarComparableStatVM = new KingdomWarComparableStatVM((int)this._sourceFaction.CurrentTotalStrength, (int)kingdom.CurrentTotalStrength, GameTexts.FindText("str_strength", null), text, text2, 10000, null, null);
			this.ComparedStats.Add(kingdomWarComparableStatVM);
			KingdomWarComparableStatVM kingdomWarComparableStatVM2 = new KingdomWarComparableStatVM(this._sourceFaction.Armies.Count, kingdom.Armies.Count, GameTexts.FindText("str_armies", null), text, text2, 5, null, null);
			this.ComparedStats.Add(kingdomWarComparableStatVM2);
			int num = this._sourceFaction.Settlements.Count<Settlement>((Settlement settlement) => settlement.IsTown);
			int num2 = kingdom.Settlements.Count<Settlement>((Settlement settlement) => settlement.IsTown);
			KingdomWarComparableStatVM kingdomWarComparableStatVM3 = new KingdomWarComparableStatVM(num, num2, GameTexts.FindText("str_towns", null), text, text2, 50, null, null);
			this.ComparedStats.Add(kingdomWarComparableStatVM3);
			int num3 = this._sourceFaction.Settlements.Count<Settlement>((Settlement settlement) => settlement.IsCastle);
			int num4 = this.TargetFaction.Settlements.Count<Settlement>((Settlement settlement) => settlement.IsCastle);
			KingdomWarComparableStatVM kingdomWarComparableStatVM4 = new KingdomWarComparableStatVM(num3, num4, GameTexts.FindText("str_castles", null), text, text2, 50, null, null);
			this.ComparedStats.Add(kingdomWarComparableStatVM4);
			this.TargetFactionOtherWars = new MBBindingList<KingdomDiplomacyFactionItemVM>();
			foreach (StanceLink stanceLink in FactionHelper.GetStances(this.TargetFaction))
			{
				if (stanceLink.IsAtWar && stanceLink.Faction1 != this._sourceFaction && stanceLink.Faction2 != this._sourceFaction && (stanceLink.Faction1.IsKingdomFaction || stanceLink.Faction1.Leader == Hero.MainHero) && (stanceLink.Faction2.IsKingdomFaction || stanceLink.Faction2.Leader == Hero.MainHero) && !stanceLink.Faction1.IsRebelClan && !stanceLink.Faction2.IsRebelClan && !stanceLink.Faction1.IsBanditFaction && !stanceLink.Faction2.IsBanditFaction)
				{
					this.TargetFactionOtherWars.Add(new KingdomDiplomacyFactionItemVM((stanceLink.Faction1 == this.TargetFaction) ? stanceLink.Faction2 : stanceLink.Faction1));
				}
			}
			this.IsTargetFactionOtherWarsVisible = this.TargetFactionOtherWars.Count > 0;
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000B46 RID: 2886 RVA: 0x000302B8 File Offset: 0x0002E4B8
		// (set) Token: 0x06000B47 RID: 2887 RVA: 0x000302C0 File Offset: 0x0002E4C0
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

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000B48 RID: 2888 RVA: 0x000302E3 File Offset: 0x0002E4E3
		// (set) Token: 0x06000B49 RID: 2889 RVA: 0x000302EB File Offset: 0x0002E4EB
		[DataSourceProperty]
		public string TradeAgreementDescriptionText
		{
			get
			{
				return this._tradeAgreementDescriptionText;
			}
			set
			{
				if (value != this._tradeAgreementDescriptionText)
				{
					this._tradeAgreementDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "TradeAgreementDescriptionText");
				}
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000B4A RID: 2890 RVA: 0x0003030E File Offset: 0x0002E50E
		// (set) Token: 0x06000B4B RID: 2891 RVA: 0x00030316 File Offset: 0x0002E516
		[DataSourceProperty]
		public BannerImageIdentifierVM SourceFactionBanner
		{
			get
			{
				return this._sourceFactionBanner;
			}
			set
			{
				if (value != this._sourceFactionBanner)
				{
					this._sourceFactionBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "SourceFactionBanner");
				}
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000B4C RID: 2892 RVA: 0x00030334 File Offset: 0x0002E534
		// (set) Token: 0x06000B4D RID: 2893 RVA: 0x0003033C File Offset: 0x0002E53C
		[DataSourceProperty]
		public BannerImageIdentifierVM TargetFactionBanner
		{
			get
			{
				return this._targetFactionBanner;
			}
			set
			{
				if (value != this._targetFactionBanner)
				{
					this._targetFactionBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "TargetFactionBanner");
				}
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000B4E RID: 2894 RVA: 0x0003035A File Offset: 0x0002E55A
		// (set) Token: 0x06000B4F RID: 2895 RVA: 0x00030362 File Offset: 0x0002E562
		[DataSourceProperty]
		public MBBindingList<KingdomWarComparableStatVM> ComparedStats
		{
			get
			{
				return this._comparedStats;
			}
			set
			{
				if (value != this._comparedStats)
				{
					this._comparedStats = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomWarComparableStatVM>>(value, "ComparedStats");
				}
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000B50 RID: 2896 RVA: 0x00030380 File Offset: 0x0002E580
		// (set) Token: 0x06000B51 RID: 2897 RVA: 0x00030388 File Offset: 0x0002E588
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

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000B52 RID: 2898 RVA: 0x000303AB File Offset: 0x0002E5AB
		// (set) Token: 0x06000B53 RID: 2899 RVA: 0x000303B3 File Offset: 0x0002E5B3
		[DataSourceProperty]
		public HeroVM SourceFactionLeader
		{
			get
			{
				return this._sourceFactionLeader;
			}
			set
			{
				if (value != this._sourceFactionLeader)
				{
					this._sourceFactionLeader = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "SourceFactionLeader");
				}
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000B54 RID: 2900 RVA: 0x000303D1 File Offset: 0x0002E5D1
		// (set) Token: 0x06000B55 RID: 2901 RVA: 0x000303D9 File Offset: 0x0002E5D9
		[DataSourceProperty]
		public HeroVM TargetFactionLeader
		{
			get
			{
				return this._targetFactionLeader;
			}
			set
			{
				if (value != this._targetFactionLeader)
				{
					this._targetFactionLeader = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "TargetFactionLeader");
				}
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000B56 RID: 2902 RVA: 0x000303F7 File Offset: 0x0002E5F7
		// (set) Token: 0x06000B57 RID: 2903 RVA: 0x000303FF File Offset: 0x0002E5FF
		[DataSourceProperty]
		public bool IsTargetFactionOtherWarsVisible
		{
			get
			{
				return this._isTargetFactionOtherWarsVisible;
			}
			set
			{
				if (value != this._isTargetFactionOtherWarsVisible)
				{
					this._isTargetFactionOtherWarsVisible = value;
					base.OnPropertyChangedWithValue(value, "IsTargetFactionOtherWarsVisible");
				}
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000B58 RID: 2904 RVA: 0x0003041D File Offset: 0x0002E61D
		// (set) Token: 0x06000B59 RID: 2905 RVA: 0x00030425 File Offset: 0x0002E625
		[DataSourceProperty]
		public MBBindingList<KingdomDiplomacyFactionItemVM> TargetFactionOtherWars
		{
			get
			{
				return this._targetFactionOtherWars;
			}
			set
			{
				if (value != this._targetFactionOtherWars)
				{
					this._targetFactionOtherWars = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomDiplomacyFactionItemVM>>(value, "TargetFactionOtherWars");
				}
			}
		}

		// Token: 0x040004F8 RID: 1272
		private readonly TradeAgreementDecision _tradeAgreementDecision;

		// Token: 0x040004F9 RID: 1273
		private string _nameText;

		// Token: 0x040004FA RID: 1274
		private string _tradeAgreementDescriptionText;

		// Token: 0x040004FB RID: 1275
		private BannerImageIdentifierVM _sourceFactionBanner;

		// Token: 0x040004FC RID: 1276
		private BannerImageIdentifierVM _targetFactionBanner;

		// Token: 0x040004FD RID: 1277
		private string _leaderText;

		// Token: 0x040004FE RID: 1278
		private HeroVM _sourceFactionLeader;

		// Token: 0x040004FF RID: 1279
		private HeroVM _targetFactionLeader;

		// Token: 0x04000500 RID: 1280
		private MBBindingList<KingdomWarComparableStatVM> _comparedStats;

		// Token: 0x04000501 RID: 1281
		private bool _isTargetFactionOtherWarsVisible;

		// Token: 0x04000502 RID: 1282
		private MBBindingList<KingdomDiplomacyFactionItemVM> _targetFactionOtherWars;
	}
}
