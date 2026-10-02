using System;
using Helpers;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions.ItemTypes
{
	// Token: 0x0200007F RID: 127
	public class AcceptingCallToWarAgreementDecisionItemVM : DecisionItemBaseVM
	{
		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000A0A RID: 2570 RVA: 0x0002BDCF File Offset: 0x00029FCF
		private Kingdom _callingKingdom
		{
			get
			{
				return (this._decision as AcceptCallToWarAgreementDecision).CallingKingdom;
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000A0B RID: 2571 RVA: 0x0002BDE1 File Offset: 0x00029FE1
		public IFaction TargetFaction
		{
			get
			{
				return (this._decision as AcceptCallToWarAgreementDecision).KingdomToCallToWarAgainst;
			}
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x0002BDF3 File Offset: 0x00029FF3
		public AcceptingCallToWarAgreementDecisionItemVM(AcceptCallToWarAgreementDecision decision, Action onDecisionOver)
			: base(decision, onDecisionOver)
		{
			this._callToWarAgreementDecision = decision;
			base.DecisionType = 8;
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x0002BE0C File Offset: 0x0002A00C
		protected override void InitValues()
		{
			base.InitValues();
			TextObject textObject = GameTexts.FindText("str_kingdom_decision_accept_call_to_war_agreement", null);
			this.NameText = textObject.ToString();
			TextObject textObject2 = GameTexts.FindText("str_kingdom_decision_accept_call_to_war_agreement_desc", null);
			textObject2.SetTextVariable("CALLING_KINGDOM", this._callingKingdom.Name);
			textObject2.SetTextVariable("KINGDOM_TO_CALL_TO_WAR_AGAINST", this.TargetFaction.Name);
			this.AcceptCallToWarAgreementDescriptionText = textObject2.ToString();
			this.SourceFactionBanner = new BannerImageIdentifierVM(this._callingKingdom.Banner, true);
			this.TargetFactionBanner = new BannerImageIdentifierVM(this.TargetFaction.Banner, true);
			this.LeaderText = GameTexts.FindText("str_leader", null).ToString();
			this.SourceFactionLeader = new HeroVM(this._callingKingdom.Leader, false);
			this.TargetFactionLeader = new HeroVM(this.TargetFaction.Leader, false);
			this.ComparedStats = new MBBindingList<KingdomWarComparableStatVM>();
			Kingdom kingdom = this.TargetFaction as Kingdom;
			string text = Color.FromUint(this._callingKingdom.Color).ToString();
			string text2 = Color.FromUint(kingdom.Color).ToString();
			KingdomWarComparableStatVM kingdomWarComparableStatVM = new KingdomWarComparableStatVM((int)this._callingKingdom.CurrentTotalStrength, (int)kingdom.CurrentTotalStrength, GameTexts.FindText("str_strength", null), text, text2, 10000, null, null);
			this.ComparedStats.Add(kingdomWarComparableStatVM);
			this.TargetFactionOtherWars = new MBBindingList<KingdomDiplomacyFactionItemVM>();
			foreach (StanceLink stanceLink in FactionHelper.GetStances(this.TargetFaction))
			{
				if (stanceLink.IsAtWar && stanceLink.Faction1 != this._callingKingdom && stanceLink.Faction2 != this._callingKingdom && (stanceLink.Faction1.IsKingdomFaction || stanceLink.Faction1.Leader == Hero.MainHero) && (stanceLink.Faction2.IsKingdomFaction || stanceLink.Faction2.Leader == Hero.MainHero) && !stanceLink.Faction1.IsRebelClan && !stanceLink.Faction2.IsRebelClan && !stanceLink.Faction1.IsBanditFaction && !stanceLink.Faction2.IsBanditFaction)
				{
					this.TargetFactionOtherWars.Add(new KingdomDiplomacyFactionItemVM((stanceLink.Faction1 == this.TargetFaction) ? stanceLink.Faction2 : stanceLink.Faction1));
				}
			}
			this.IsTargetFactionOtherWarsVisible = this.TargetFactionOtherWars.Count > 0;
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000A0E RID: 2574 RVA: 0x0002C0C4 File Offset: 0x0002A2C4
		// (set) Token: 0x06000A0F RID: 2575 RVA: 0x0002C0CC File Offset: 0x0002A2CC
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

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000A10 RID: 2576 RVA: 0x0002C0EF File Offset: 0x0002A2EF
		// (set) Token: 0x06000A11 RID: 2577 RVA: 0x0002C0F7 File Offset: 0x0002A2F7
		[DataSourceProperty]
		public string AcceptCallToWarAgreementDescriptionText
		{
			get
			{
				return this._callToWarAgreementDescriptionText;
			}
			set
			{
				if (value != this._callToWarAgreementDescriptionText)
				{
					this._callToWarAgreementDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "AcceptCallToWarAgreementDescriptionText");
				}
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000A12 RID: 2578 RVA: 0x0002C11A File Offset: 0x0002A31A
		// (set) Token: 0x06000A13 RID: 2579 RVA: 0x0002C122 File Offset: 0x0002A322
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

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000A14 RID: 2580 RVA: 0x0002C140 File Offset: 0x0002A340
		// (set) Token: 0x06000A15 RID: 2581 RVA: 0x0002C148 File Offset: 0x0002A348
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

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000A16 RID: 2582 RVA: 0x0002C166 File Offset: 0x0002A366
		// (set) Token: 0x06000A17 RID: 2583 RVA: 0x0002C16E File Offset: 0x0002A36E
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

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000A18 RID: 2584 RVA: 0x0002C18C File Offset: 0x0002A38C
		// (set) Token: 0x06000A19 RID: 2585 RVA: 0x0002C194 File Offset: 0x0002A394
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

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000A1A RID: 2586 RVA: 0x0002C1B7 File Offset: 0x0002A3B7
		// (set) Token: 0x06000A1B RID: 2587 RVA: 0x0002C1BF File Offset: 0x0002A3BF
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

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000A1C RID: 2588 RVA: 0x0002C1DD File Offset: 0x0002A3DD
		// (set) Token: 0x06000A1D RID: 2589 RVA: 0x0002C1E5 File Offset: 0x0002A3E5
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

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000A1E RID: 2590 RVA: 0x0002C203 File Offset: 0x0002A403
		// (set) Token: 0x06000A1F RID: 2591 RVA: 0x0002C20B File Offset: 0x0002A40B
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

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000A20 RID: 2592 RVA: 0x0002C229 File Offset: 0x0002A429
		// (set) Token: 0x06000A21 RID: 2593 RVA: 0x0002C231 File Offset: 0x0002A431
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

		// Token: 0x0400046B RID: 1131
		private readonly AcceptCallToWarAgreementDecision _callToWarAgreementDecision;

		// Token: 0x0400046C RID: 1132
		private string _nameText;

		// Token: 0x0400046D RID: 1133
		private string _callToWarAgreementDescriptionText;

		// Token: 0x0400046E RID: 1134
		private BannerImageIdentifierVM _sourceFactionBanner;

		// Token: 0x0400046F RID: 1135
		private BannerImageIdentifierVM _targetFactionBanner;

		// Token: 0x04000470 RID: 1136
		private string _leaderText;

		// Token: 0x04000471 RID: 1137
		private HeroVM _sourceFactionLeader;

		// Token: 0x04000472 RID: 1138
		private HeroVM _targetFactionLeader;

		// Token: 0x04000473 RID: 1139
		private MBBindingList<KingdomWarComparableStatVM> _comparedStats;

		// Token: 0x04000474 RID: 1140
		private bool _isTargetFactionOtherWarsVisible;

		// Token: 0x04000475 RID: 1141
		private MBBindingList<KingdomDiplomacyFactionItemVM> _targetFactionOtherWars;
	}
}
