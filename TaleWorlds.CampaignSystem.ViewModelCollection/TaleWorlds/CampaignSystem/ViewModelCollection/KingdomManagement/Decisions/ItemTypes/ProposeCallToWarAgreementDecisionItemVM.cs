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
	// Token: 0x02000087 RID: 135
	public class ProposeCallToWarAgreementDecisionItemVM : DecisionItemBaseVM
	{
		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000ACA RID: 2762 RVA: 0x0002E98A File Offset: 0x0002CB8A
		private Kingdom _calledKingdom
		{
			get
			{
				return (this._decision as ProposeCallToWarAgreementDecision).CalledKingdom;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000ACB RID: 2763 RVA: 0x0002E99C File Offset: 0x0002CB9C
		public IFaction TargetFaction
		{
			get
			{
				return (this._decision as ProposeCallToWarAgreementDecision).KingdomToCallToWarAgainst;
			}
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0002E9AE File Offset: 0x0002CBAE
		public ProposeCallToWarAgreementDecisionItemVM(ProposeCallToWarAgreementDecision decision, Action onDecisionOver)
			: base(decision, onDecisionOver)
		{
			this._proposeCallToWarAgreementDecision = decision;
			base.DecisionType = 9;
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x0002E9C8 File Offset: 0x0002CBC8
		protected override void InitValues()
		{
			base.InitValues();
			TextObject textObject = GameTexts.FindText("str_kingdom_decision_propose_call_to_war_agreement", null);
			this.NameText = textObject.ToString();
			TextObject textObject2 = GameTexts.FindText("str_kingdom_decision_propose_call_to_war_agreement_desc", null);
			textObject2.SetTextVariable("CALLED_KINGDOM", this._calledKingdom.Name);
			textObject2.SetTextVariable("KINGDOM_TO_CALL_TO_WAR_AGAINST", this.TargetFaction.Name);
			this.ProposeCallToWarAgreementDescriptionText = textObject2.ToString();
			this.SourceFactionBanner = new BannerImageIdentifierVM(this._calledKingdom.Banner, true);
			this.TargetFactionBanner = new BannerImageIdentifierVM(this.TargetFaction.Banner, true);
			this.LeaderText = GameTexts.FindText("str_leader", null).ToString();
			this.SourceFactionLeader = new HeroVM(this._calledKingdom.Leader, false);
			this.TargetFactionLeader = new HeroVM(this.TargetFaction.Leader, false);
			this.ComparedStats = new MBBindingList<KingdomWarComparableStatVM>();
			Kingdom kingdom = this.TargetFaction as Kingdom;
			string text = Color.FromUint(this._calledKingdom.Color).ToString();
			string text2 = Color.FromUint(kingdom.Color).ToString();
			KingdomWarComparableStatVM kingdomWarComparableStatVM = new KingdomWarComparableStatVM((int)this._calledKingdom.CurrentTotalStrength, (int)kingdom.CurrentTotalStrength, GameTexts.FindText("str_strength", null), text, text2, 10000, null, null);
			this.ComparedStats.Add(kingdomWarComparableStatVM);
			this.TargetFactionOtherWars = new MBBindingList<KingdomDiplomacyFactionItemVM>();
			foreach (StanceLink stanceLink in FactionHelper.GetStances(this.TargetFaction))
			{
				if (stanceLink.IsAtWar && stanceLink.Faction1 != this._calledKingdom && stanceLink.Faction2 != this._calledKingdom && (stanceLink.Faction1.IsKingdomFaction || stanceLink.Faction1.Leader == Hero.MainHero) && (stanceLink.Faction2.IsKingdomFaction || stanceLink.Faction2.Leader == Hero.MainHero) && !stanceLink.Faction1.IsRebelClan && !stanceLink.Faction2.IsRebelClan && !stanceLink.Faction1.IsBanditFaction && !stanceLink.Faction2.IsBanditFaction)
				{
					this.TargetFactionOtherWars.Add(new KingdomDiplomacyFactionItemVM((stanceLink.Faction1 == this.TargetFaction) ? stanceLink.Faction2 : stanceLink.Faction1));
				}
			}
			this.IsTargetFactionOtherWarsVisible = this.TargetFactionOtherWars.Count > 0;
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000ACE RID: 2766 RVA: 0x0002EC80 File Offset: 0x0002CE80
		// (set) Token: 0x06000ACF RID: 2767 RVA: 0x0002EC88 File Offset: 0x0002CE88
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

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x0002ECAB File Offset: 0x0002CEAB
		// (set) Token: 0x06000AD1 RID: 2769 RVA: 0x0002ECB3 File Offset: 0x0002CEB3
		[DataSourceProperty]
		public string ProposeCallToWarAgreementDescriptionText
		{
			get
			{
				return this._proposeCallToWarAgreementDescriptionText;
			}
			set
			{
				if (value != this._proposeCallToWarAgreementDescriptionText)
				{
					this._proposeCallToWarAgreementDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProposeCallToWarAgreementDescriptionText");
				}
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x0002ECD6 File Offset: 0x0002CED6
		// (set) Token: 0x06000AD3 RID: 2771 RVA: 0x0002ECDE File Offset: 0x0002CEDE
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

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x0002ECFC File Offset: 0x0002CEFC
		// (set) Token: 0x06000AD5 RID: 2773 RVA: 0x0002ED04 File Offset: 0x0002CF04
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

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x0002ED22 File Offset: 0x0002CF22
		// (set) Token: 0x06000AD7 RID: 2775 RVA: 0x0002ED2A File Offset: 0x0002CF2A
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

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x0002ED48 File Offset: 0x0002CF48
		// (set) Token: 0x06000AD9 RID: 2777 RVA: 0x0002ED50 File Offset: 0x0002CF50
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

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000ADA RID: 2778 RVA: 0x0002ED73 File Offset: 0x0002CF73
		// (set) Token: 0x06000ADB RID: 2779 RVA: 0x0002ED7B File Offset: 0x0002CF7B
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

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x0002ED99 File Offset: 0x0002CF99
		// (set) Token: 0x06000ADD RID: 2781 RVA: 0x0002EDA1 File Offset: 0x0002CFA1
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

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000ADE RID: 2782 RVA: 0x0002EDBF File Offset: 0x0002CFBF
		// (set) Token: 0x06000ADF RID: 2783 RVA: 0x0002EDC7 File Offset: 0x0002CFC7
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

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x0002EDE5 File Offset: 0x0002CFE5
		// (set) Token: 0x06000AE1 RID: 2785 RVA: 0x0002EDED File Offset: 0x0002CFED
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

		// Token: 0x040004C0 RID: 1216
		private readonly ProposeCallToWarAgreementDecision _proposeCallToWarAgreementDecision;

		// Token: 0x040004C1 RID: 1217
		private string _nameText;

		// Token: 0x040004C2 RID: 1218
		private string _proposeCallToWarAgreementDescriptionText;

		// Token: 0x040004C3 RID: 1219
		private BannerImageIdentifierVM _sourceFactionBanner;

		// Token: 0x040004C4 RID: 1220
		private BannerImageIdentifierVM _targetFactionBanner;

		// Token: 0x040004C5 RID: 1221
		private string _leaderText;

		// Token: 0x040004C6 RID: 1222
		private HeroVM _sourceFactionLeader;

		// Token: 0x040004C7 RID: 1223
		private HeroVM _targetFactionLeader;

		// Token: 0x040004C8 RID: 1224
		private MBBindingList<KingdomWarComparableStatVM> _comparedStats;

		// Token: 0x040004C9 RID: 1225
		private bool _isTargetFactionOtherWarsVisible;

		// Token: 0x040004CA RID: 1226
		private MBBindingList<KingdomDiplomacyFactionItemVM> _targetFactionOtherWars;
	}
}
