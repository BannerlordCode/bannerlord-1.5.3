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
	// Token: 0x02000089 RID: 137
	public class StartAllianceDecisionItemVM : DecisionItemBaseVM
	{
		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000B2A RID: 2858 RVA: 0x0002F8DD File Offset: 0x0002DADD
		private Kingdom _sourceFaction
		{
			get
			{
				return Hero.MainHero.Clan.Kingdom;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000B2B RID: 2859 RVA: 0x0002F8EE File Offset: 0x0002DAEE
		public IFaction TargetFaction
		{
			get
			{
				return (this._decision as StartAllianceDecision).KingdomToStartAllianceWith;
			}
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0002F900 File Offset: 0x0002DB00
		public StartAllianceDecisionItemVM(StartAllianceDecision decision, Action onDecisionOver)
			: base(decision, onDecisionOver)
		{
			this._startAllianceDecision = decision;
			base.DecisionType = 7;
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0002F918 File Offset: 0x0002DB18
		protected override void InitValues()
		{
			base.InitValues();
			TextObject textObject = GameTexts.FindText("str_kingdom_decision_start_alliance", null);
			this.NameText = textObject.ToString();
			TextObject textObject2 = GameTexts.FindText("str_kingdom_decision_start_alliance_desc", null);
			textObject2.SetTextVariable("FACTION", this.TargetFaction.Name);
			this.StartAllianceDescriptionText = textObject2.ToString();
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

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000B2E RID: 2862 RVA: 0x0002FD04 File Offset: 0x0002DF04
		// (set) Token: 0x06000B2F RID: 2863 RVA: 0x0002FD0C File Offset: 0x0002DF0C
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

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x0002FD2F File Offset: 0x0002DF2F
		// (set) Token: 0x06000B31 RID: 2865 RVA: 0x0002FD37 File Offset: 0x0002DF37
		[DataSourceProperty]
		public string StartAllianceDescriptionText
		{
			get
			{
				return this._startAllianceDescriptionText;
			}
			set
			{
				if (value != this._startAllianceDescriptionText)
				{
					this._startAllianceDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "StartAllianceDescriptionText");
				}
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000B32 RID: 2866 RVA: 0x0002FD5A File Offset: 0x0002DF5A
		// (set) Token: 0x06000B33 RID: 2867 RVA: 0x0002FD62 File Offset: 0x0002DF62
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

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000B34 RID: 2868 RVA: 0x0002FD80 File Offset: 0x0002DF80
		// (set) Token: 0x06000B35 RID: 2869 RVA: 0x0002FD88 File Offset: 0x0002DF88
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

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000B36 RID: 2870 RVA: 0x0002FDA6 File Offset: 0x0002DFA6
		// (set) Token: 0x06000B37 RID: 2871 RVA: 0x0002FDAE File Offset: 0x0002DFAE
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

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000B38 RID: 2872 RVA: 0x0002FDCC File Offset: 0x0002DFCC
		// (set) Token: 0x06000B39 RID: 2873 RVA: 0x0002FDD4 File Offset: 0x0002DFD4
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

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000B3A RID: 2874 RVA: 0x0002FDF7 File Offset: 0x0002DFF7
		// (set) Token: 0x06000B3B RID: 2875 RVA: 0x0002FDFF File Offset: 0x0002DFFF
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

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000B3C RID: 2876 RVA: 0x0002FE1D File Offset: 0x0002E01D
		// (set) Token: 0x06000B3D RID: 2877 RVA: 0x0002FE25 File Offset: 0x0002E025
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

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000B3E RID: 2878 RVA: 0x0002FE43 File Offset: 0x0002E043
		// (set) Token: 0x06000B3F RID: 2879 RVA: 0x0002FE4B File Offset: 0x0002E04B
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

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000B40 RID: 2880 RVA: 0x0002FE69 File Offset: 0x0002E069
		// (set) Token: 0x06000B41 RID: 2881 RVA: 0x0002FE71 File Offset: 0x0002E071
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

		// Token: 0x040004ED RID: 1261
		private readonly StartAllianceDecision _startAllianceDecision;

		// Token: 0x040004EE RID: 1262
		private string _nameText;

		// Token: 0x040004EF RID: 1263
		private string _startAllianceDescriptionText;

		// Token: 0x040004F0 RID: 1264
		private BannerImageIdentifierVM _sourceFactionBanner;

		// Token: 0x040004F1 RID: 1265
		private BannerImageIdentifierVM _targetFactionBanner;

		// Token: 0x040004F2 RID: 1266
		private string _leaderText;

		// Token: 0x040004F3 RID: 1267
		private HeroVM _sourceFactionLeader;

		// Token: 0x040004F4 RID: 1268
		private HeroVM _targetFactionLeader;

		// Token: 0x040004F5 RID: 1269
		private MBBindingList<KingdomWarComparableStatVM> _comparedStats;

		// Token: 0x040004F6 RID: 1270
		private bool _isTargetFactionOtherWarsVisible;

		// Token: 0x040004F7 RID: 1271
		private MBBindingList<KingdomDiplomacyFactionItemVM> _targetFactionOtherWars;
	}
}
