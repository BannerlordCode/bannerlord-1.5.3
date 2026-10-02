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
	// Token: 0x02000081 RID: 129
	public class DeclareWarDecisionItemVM : DecisionItemBaseVM
	{
		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000A57 RID: 2647 RVA: 0x0002D0D1 File Offset: 0x0002B2D1
		private Kingdom _sourceFaction
		{
			get
			{
				return Hero.MainHero.Clan.Kingdom;
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000A58 RID: 2648 RVA: 0x0002D0E2 File Offset: 0x0002B2E2
		public IFaction TargetFaction
		{
			get
			{
				return (this._decision as DeclareWarDecision).FactionToDeclareWarOn;
			}
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x0002D0F4 File Offset: 0x0002B2F4
		public DeclareWarDecisionItemVM(DeclareWarDecision decision, Action onDecisionOver)
			: base(decision, onDecisionOver)
		{
			this._declareWarDecision = decision;
			base.DecisionType = 4;
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x0002D10C File Offset: 0x0002B30C
		protected override void InitValues()
		{
			base.InitValues();
			TextObject textObject = GameTexts.FindText("str_kingdom_decision_declare_war", null);
			this.NameText = textObject.ToString();
			TextObject textObject2 = GameTexts.FindText("str_kingdom_decision_declare_war_desc", null);
			textObject2.SetTextVariable("FACTION", this.TargetFaction.Name);
			this.WarDescriptionText = textObject2.ToString();
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

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x0002D4F8 File Offset: 0x0002B6F8
		// (set) Token: 0x06000A5C RID: 2652 RVA: 0x0002D500 File Offset: 0x0002B700
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

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000A5D RID: 2653 RVA: 0x0002D523 File Offset: 0x0002B723
		// (set) Token: 0x06000A5E RID: 2654 RVA: 0x0002D52B File Offset: 0x0002B72B
		[DataSourceProperty]
		public string WarDescriptionText
		{
			get
			{
				return this._warDescriptionText;
			}
			set
			{
				if (value != this._warDescriptionText)
				{
					this._warDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "WarDescriptionText");
				}
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x0002D54E File Offset: 0x0002B74E
		// (set) Token: 0x06000A60 RID: 2656 RVA: 0x0002D556 File Offset: 0x0002B756
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

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x0002D574 File Offset: 0x0002B774
		// (set) Token: 0x06000A62 RID: 2658 RVA: 0x0002D57C File Offset: 0x0002B77C
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

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x0002D59A File Offset: 0x0002B79A
		// (set) Token: 0x06000A64 RID: 2660 RVA: 0x0002D5A2 File Offset: 0x0002B7A2
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

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x0002D5C0 File Offset: 0x0002B7C0
		// (set) Token: 0x06000A66 RID: 2662 RVA: 0x0002D5C8 File Offset: 0x0002B7C8
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

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x0002D5EB File Offset: 0x0002B7EB
		// (set) Token: 0x06000A68 RID: 2664 RVA: 0x0002D5F3 File Offset: 0x0002B7F3
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

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x0002D611 File Offset: 0x0002B811
		// (set) Token: 0x06000A6A RID: 2666 RVA: 0x0002D619 File Offset: 0x0002B819
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

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x0002D637 File Offset: 0x0002B837
		// (set) Token: 0x06000A6C RID: 2668 RVA: 0x0002D63F File Offset: 0x0002B83F
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

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x0002D65D File Offset: 0x0002B85D
		// (set) Token: 0x06000A6E RID: 2670 RVA: 0x0002D665 File Offset: 0x0002B865
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

		// Token: 0x0400048D RID: 1165
		private readonly DeclareWarDecision _declareWarDecision;

		// Token: 0x0400048E RID: 1166
		private string _nameText;

		// Token: 0x0400048F RID: 1167
		private string _warDescriptionText;

		// Token: 0x04000490 RID: 1168
		private BannerImageIdentifierVM _sourceFactionBanner;

		// Token: 0x04000491 RID: 1169
		private BannerImageIdentifierVM _targetFactionBanner;

		// Token: 0x04000492 RID: 1170
		private string _leaderText;

		// Token: 0x04000493 RID: 1171
		private HeroVM _sourceFactionLeader;

		// Token: 0x04000494 RID: 1172
		private HeroVM _targetFactionLeader;

		// Token: 0x04000495 RID: 1173
		private MBBindingList<KingdomWarComparableStatVM> _comparedStats;

		// Token: 0x04000496 RID: 1174
		private bool _isTargetFactionOtherWarsVisible;

		// Token: 0x04000497 RID: 1175
		private MBBindingList<KingdomDiplomacyFactionItemVM> _targetFactionOtherWars;
	}
}
