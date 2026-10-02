using System;
using Helpers;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions.ItemTypes
{
	// Token: 0x02000085 RID: 133
	public class MakePeaceDecisionItemVM : DecisionItemBaseVM
	{
		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x0002E1E1 File Offset: 0x0002C3E1
		private Kingdom _sourceFaction
		{
			get
			{
				return Hero.MainHero.Clan.Kingdom;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000AA9 RID: 2729 RVA: 0x0002E1F2 File Offset: 0x0002C3F2
		public IFaction TargetFaction
		{
			get
			{
				return (this._decision as MakePeaceKingdomDecision).FactionToMakePeaceWith;
			}
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0002E204 File Offset: 0x0002C404
		public MakePeaceDecisionItemVM(MakePeaceKingdomDecision decision, Action onDecisionOver)
			: base(decision, onDecisionOver)
		{
			this._makePeaceDecision = decision;
			base.DecisionType = 5;
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0002E21C File Offset: 0x0002C41C
		protected override void InitValues()
		{
			base.InitValues();
			TextObject textObject = GameTexts.FindText("str_kingdom_decision_make_peace", null);
			this.NameText = textObject.ToString();
			TextObject textObject2 = GameTexts.FindText("str_kingdom_decision_make_peace_desc", null);
			textObject2.SetTextVariable("FACTION", this.TargetFaction.Name);
			this.PeaceDescriptionText = textObject2.ToString();
			this.SourceFactionBanner = new BannerImageIdentifierVM(this._sourceFaction.Banner, true);
			this.TargetFactionBanner = new BannerImageIdentifierVM(this.TargetFaction.Banner, true);
			this.LeaderText = GameTexts.FindText("str_leader", null).ToString();
			this.SourceFactionLeader = new HeroVM(this._sourceFaction.Leader, false);
			this.TargetFactionLeader = new HeroVM(this.TargetFaction.Leader, false);
			this.ComparedStats = new MBBindingList<KingdomWarComparableStatVM>();
			Kingdom targetFaction = this.TargetFaction as Kingdom;
			string text = Color.FromUint(this._sourceFaction.Color).ToString();
			string text2 = Color.FromUint(targetFaction.Color).ToString();
			StanceLink stanceWith = this._sourceFaction.GetStanceWith(this.TargetFaction);
			KingdomWarComparableStatVM kingdomWarComparableStatVM = new KingdomWarComparableStatVM((int)this._sourceFaction.CurrentTotalStrength, (int)targetFaction.CurrentTotalStrength, GameTexts.FindText("str_strength", null), text, text2, 10000, null, null);
			this.ComparedStats.Add(kingdomWarComparableStatVM);
			KingdomWarComparableStatVM kingdomWarComparableStatVM2 = new KingdomWarComparableStatVM(stanceWith.GetCasualties(targetFaction), stanceWith.GetCasualties(this._sourceFaction), GameTexts.FindText("str_war_casualties_inflicted", null), text, text2, 10000, null, null);
			this.ComparedStats.Add(kingdomWarComparableStatVM2);
			KingdomWarComparableStatVM kingdomWarComparableStatVM3 = new KingdomWarComparableStatVM(stanceWith.GetSuccessfulSieges(this._sourceFaction), stanceWith.GetSuccessfulSieges(targetFaction), GameTexts.FindText("str_war_successful_sieges", null), text, text2, 5, null, null);
			this.ComparedStats.Add(kingdomWarComparableStatVM3);
			KingdomWarComparableStatVM kingdomWarComparableStatVM4 = new KingdomWarComparableStatVM(stanceWith.GetSuccessfulRaids(this._sourceFaction), stanceWith.GetSuccessfulRaids(targetFaction), GameTexts.FindText("str_war_successful_raids", null), text, text2, 10, null, null);
			this.ComparedStats.Add(kingdomWarComparableStatVM4);
			ExplainedNumber warProgressOfFaction1 = Campaign.Current.Models.DiplomacyModel.GetWarProgressScore(this._sourceFaction, targetFaction, true);
			ExplainedNumber warProgressOfFaction2 = Campaign.Current.Models.DiplomacyModel.GetWarProgressScore(targetFaction, this._sourceFaction, true);
			int num = (int)(warProgressOfFaction1.ResultNumber * 100f / warProgressOfFaction1.LimitMaxValue);
			int num2 = (int)(warProgressOfFaction2.ResultNumber * 100f / warProgressOfFaction2.LimitMaxValue);
			int num3 = MathF.Max(0, num - num2);
			int num4 = MathF.Max(0, num2 - num);
			KingdomWarComparableStatVM kingdomWarComparableStatVM5 = new KingdomWarComparableStatVM(num3, num4, new TextObject("{=8qbkS5D2}War Progress", null), text, text2, 100, new BasicTooltipViewModel(() => CampaignUIHelper.GetNormalizedWarProgressTooltip(warProgressOfFaction1, warProgressOfFaction2, warProgressOfFaction1.LimitMaxValue, this._sourceFaction.Name, targetFaction.Name)), new BasicTooltipViewModel(() => CampaignUIHelper.GetNormalizedWarProgressTooltip(warProgressOfFaction2, warProgressOfFaction1, warProgressOfFaction2.LimitMaxValue, targetFaction.Name, this._sourceFaction.Name)));
			this.ComparedStats.Add(kingdomWarComparableStatVM5);
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

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000AAC RID: 2732 RVA: 0x0002E69C File Offset: 0x0002C89C
		// (set) Token: 0x06000AAD RID: 2733 RVA: 0x0002E6A4 File Offset: 0x0002C8A4
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

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x0002E6C7 File Offset: 0x0002C8C7
		// (set) Token: 0x06000AAF RID: 2735 RVA: 0x0002E6CF File Offset: 0x0002C8CF
		[DataSourceProperty]
		public string PeaceDescriptionText
		{
			get
			{
				return this._peaceDescriptionText;
			}
			set
			{
				if (value != this._peaceDescriptionText)
				{
					this._peaceDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "PeaceDescriptionText");
				}
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x0002E6F2 File Offset: 0x0002C8F2
		// (set) Token: 0x06000AB1 RID: 2737 RVA: 0x0002E6FA File Offset: 0x0002C8FA
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

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x0002E718 File Offset: 0x0002C918
		// (set) Token: 0x06000AB3 RID: 2739 RVA: 0x0002E720 File Offset: 0x0002C920
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

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x0002E73E File Offset: 0x0002C93E
		// (set) Token: 0x06000AB5 RID: 2741 RVA: 0x0002E746 File Offset: 0x0002C946
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

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x0002E764 File Offset: 0x0002C964
		// (set) Token: 0x06000AB7 RID: 2743 RVA: 0x0002E76C File Offset: 0x0002C96C
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

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x0002E78F File Offset: 0x0002C98F
		// (set) Token: 0x06000AB9 RID: 2745 RVA: 0x0002E797 File Offset: 0x0002C997
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

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000ABA RID: 2746 RVA: 0x0002E7B5 File Offset: 0x0002C9B5
		// (set) Token: 0x06000ABB RID: 2747 RVA: 0x0002E7BD File Offset: 0x0002C9BD
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

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000ABC RID: 2748 RVA: 0x0002E7DB File Offset: 0x0002C9DB
		// (set) Token: 0x06000ABD RID: 2749 RVA: 0x0002E7E3 File Offset: 0x0002C9E3
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

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000ABE RID: 2750 RVA: 0x0002E801 File Offset: 0x0002CA01
		// (set) Token: 0x06000ABF RID: 2751 RVA: 0x0002E809 File Offset: 0x0002CA09
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

		// Token: 0x040004B1 RID: 1201
		private readonly MakePeaceKingdomDecision _makePeaceDecision;

		// Token: 0x040004B2 RID: 1202
		private string _nameText;

		// Token: 0x040004B3 RID: 1203
		private string _peaceDescriptionText;

		// Token: 0x040004B4 RID: 1204
		private BannerImageIdentifierVM _sourceFactionBanner;

		// Token: 0x040004B5 RID: 1205
		private BannerImageIdentifierVM _targetFactionBanner;

		// Token: 0x040004B6 RID: 1206
		private string _leaderText;

		// Token: 0x040004B7 RID: 1207
		private HeroVM _sourceFactionLeader;

		// Token: 0x040004B8 RID: 1208
		private HeroVM _targetFactionLeader;

		// Token: 0x040004B9 RID: 1209
		private MBBindingList<KingdomWarComparableStatVM> _comparedStats;

		// Token: 0x040004BA RID: 1210
		private bool _isTargetFactionOtherWarsVisible;

		// Token: 0x040004BB RID: 1211
		private MBBindingList<KingdomDiplomacyFactionItemVM> _targetFactionOtherWars;
	}
}
