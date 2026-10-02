using System;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions.ItemTypes
{
	// Token: 0x02000088 RID: 136
	public class SettlementDecisionItemVM : DecisionItemBaseVM
	{
		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x0002EE0C File Offset: 0x0002D00C
		public Settlement Settlement
		{
			get
			{
				if (this._settlementDecision == null && this._settlementPreliminaryDecision == null)
				{
					SettlementClaimantDecision settlementClaimantDecision;
					SettlementClaimantPreliminaryDecision settlementClaimantPreliminaryDecision;
					if ((settlementClaimantDecision = this._decision as SettlementClaimantDecision) != null)
					{
						this._settlementDecision = settlementClaimantDecision;
					}
					else if ((settlementClaimantPreliminaryDecision = this._decision as SettlementClaimantPreliminaryDecision) != null)
					{
						this._settlementPreliminaryDecision = settlementClaimantPreliminaryDecision;
					}
				}
				if (this._settlementDecision == null)
				{
					return this._settlementPreliminaryDecision.Settlement;
				}
				return this._settlementDecision.Settlement;
			}
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x0002EE76 File Offset: 0x0002D076
		public SettlementDecisionItemVM(Settlement settlement, KingdomDecision decision, Action onDecisionOver)
			: base(decision, onDecisionOver)
		{
			this._settlement = settlement;
			base.DecisionType = 1;
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0002EE90 File Offset: 0x0002D090
		protected override void InitValues()
		{
			base.InitValues();
			base.DecisionType = 1;
			this.SettlementImageID = ((this.Settlement.SettlementComponent != null) ? this.Settlement.SettlementComponent.WaitMeshName : "");
			this.BoundVillages = new MBBindingList<EncyclopediaSettlementVM>();
			this.NotableCharacters = new MBBindingList<HeroVM>();
			this.SettlementName = this.Settlement.Name.ToString();
			Town town = this.Settlement.Town;
			this.Governor = new HeroVM((town != null) ? town.Governor : null, false);
			foreach (Village village in this.Settlement.BoundVillages)
			{
				this.BoundVillages.Add(new EncyclopediaSettlementVM(village.Settlement));
			}
			Town town2 = this.Settlement.Town;
			this.WallsText = town2.GetWallLevel().ToString();
			this.WallsHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownWallsTooltip(this.Settlement.Town));
			this.HasNotables = this.Settlement.Notables.Count > 0;
			if (!this.Settlement.IsCastle)
			{
				Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Hero));
				foreach (Hero hero in this.Settlement.Notables)
				{
					this.NotableCharacters.Add(new HeroVM(hero, false));
				}
			}
			this.DescriptorText = this.Settlement.Culture.Name.ToString();
			this.DetailsText = GameTexts.FindText("str_people_encyclopedia_details", null).ToString();
			this.OwnerText = GameTexts.FindText("str_owner", null).ToString();
			this.Owner = new HeroVM(this.Settlement.OwnerClan.Leader, false);
			SettlementComponent settlementComponent = this.Settlement.SettlementComponent;
			this.SettlementPath = settlementComponent.BackgroundMeshName;
			this.SettlementCropPosition = (double)settlementComponent.BackgroundCropPosition;
			this.NotableCharactersText = GameTexts.FindText("str_notable_characters", null).ToString();
			this.BoundSettlementText = GameTexts.FindText("str_villages", null).ToString();
			if (this.HasBoundSettlement)
			{
				GameTexts.SetVariable("SETTLEMENT_LINK", this.Settlement.Village.Bound.EncyclopediaLinkWithName);
				this.BoundSettlementText = GameTexts.FindText("str_bound_settlement_encyclopedia", null).ToString();
			}
			this.MilitasText = ((int)this.Settlement.Militia).ToString();
			this.MilitasHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownMilitiaTooltip(this.Settlement.Town));
			this.ProsperityText = ((this.Settlement.Town != null) ? ((int)this.Settlement.Town.Prosperity).ToString() : (this.Settlement.IsVillage ? ((int)this.Settlement.Village.Hearth).ToString() : string.Empty));
			if (this.Settlement.IsTown)
			{
				this.ProsperityHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownProsperityTooltip(this.Settlement.Town));
			}
			else
			{
				this.ProsperityHint = new BasicTooltipViewModel(() => GameTexts.FindText("str_prosperity", null).ToString());
			}
			if (this.Settlement.Town != null)
			{
				this.LoyaltyHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownLoyaltyTooltip(this.Settlement.Town));
				this.LoyaltyText = string.Format("{0:0.#}", this.Settlement.Town.Loyalty);
				this.SecurityHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownSecurityTooltip(this.Settlement.Town));
				this.SecurityText = string.Format("{0:0.#}", this.Settlement.Town.Security);
			}
			else
			{
				this.LoyaltyText = "-";
				this.SecurityText = "-";
			}
			Town town3 = this.Settlement.Town;
			this.FoodText = ((town3 != null) ? town3.FoodStocks.ToString("0.0") : null);
			if (this.Settlement.IsFortification)
			{
				this.FoodHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownFoodTooltip(this.Settlement.Town));
				MobileParty garrisonParty = this.Settlement.Town.GarrisonParty;
				this.GarrisonText = ((garrisonParty != null) ? garrisonParty.Party.NumberOfAllMembers.ToString() : null) ?? "0";
				this.GarrisonHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownGarrisonTooltip(this.Settlement.Town));
				return;
			}
			this.FoodHint = new BasicTooltipViewModel();
			this.GarrisonText = "-";
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x0002F370 File Offset: 0x0002D570
		// (set) Token: 0x06000AE6 RID: 2790 RVA: 0x0002F378 File Offset: 0x0002D578
		[DataSourceProperty]
		public bool HasBoundSettlement
		{
			get
			{
				return this._hasBoundSettlement;
			}
			set
			{
				if (value != this._hasBoundSettlement)
				{
					this._hasBoundSettlement = value;
					base.OnPropertyChangedWithValue(value, "HasBoundSettlement");
				}
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x0002F396 File Offset: 0x0002D596
		// (set) Token: 0x06000AE8 RID: 2792 RVA: 0x0002F39E File Offset: 0x0002D59E
		[DataSourceProperty]
		public double SettlementCropPosition
		{
			get
			{
				return this._settlementCropPosition;
			}
			set
			{
				if (value != this._settlementCropPosition)
				{
					this._settlementCropPosition = value;
					base.OnPropertyChangedWithValue(value, "SettlementCropPosition");
				}
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x0002F3BC File Offset: 0x0002D5BC
		// (set) Token: 0x06000AEA RID: 2794 RVA: 0x0002F3C4 File Offset: 0x0002D5C4
		[DataSourceProperty]
		public string BoundSettlementText
		{
			get
			{
				return this._boundSettlementText;
			}
			set
			{
				if (value != this._boundSettlementText)
				{
					this._boundSettlementText = value;
					base.OnPropertyChangedWithValue<string>(value, "BoundSettlementText");
				}
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000AEB RID: 2795 RVA: 0x0002F3E7 File Offset: 0x0002D5E7
		// (set) Token: 0x06000AEC RID: 2796 RVA: 0x0002F3EF File Offset: 0x0002D5EF
		[DataSourceProperty]
		public string DetailsText
		{
			get
			{
				return this._detailsText;
			}
			set
			{
				if (value != this._detailsText)
				{
					this._detailsText = value;
					base.OnPropertyChangedWithValue<string>(value, "DetailsText");
				}
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x0002F412 File Offset: 0x0002D612
		// (set) Token: 0x06000AEE RID: 2798 RVA: 0x0002F41A File Offset: 0x0002D61A
		[DataSourceProperty]
		public string SettlementPath
		{
			get
			{
				return this._settlementPath;
			}
			set
			{
				if (value != this._settlementPath)
				{
					this._settlementPath = value;
					base.OnPropertyChangedWithValue<string>(value, "SettlementPath");
				}
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x0002F43D File Offset: 0x0002D63D
		// (set) Token: 0x06000AF0 RID: 2800 RVA: 0x0002F445 File Offset: 0x0002D645
		[DataSourceProperty]
		public string SettlementName
		{
			get
			{
				return this._settlementName;
			}
			set
			{
				if (value != this._settlementName)
				{
					this._settlementName = value;
					base.OnPropertyChangedWithValue<string>(value, "SettlementName");
				}
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x0002F468 File Offset: 0x0002D668
		// (set) Token: 0x06000AF2 RID: 2802 RVA: 0x0002F470 File Offset: 0x0002D670
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

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x0002F493 File Offset: 0x0002D693
		// (set) Token: 0x06000AF4 RID: 2804 RVA: 0x0002F49B File Offset: 0x0002D69B
		[DataSourceProperty]
		public HeroVM Owner
		{
			get
			{
				return this._owner;
			}
			set
			{
				if (value != this._owner)
				{
					this._owner = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "Owner");
				}
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x0002F4B9 File Offset: 0x0002D6B9
		// (set) Token: 0x06000AF6 RID: 2806 RVA: 0x0002F4C1 File Offset: 0x0002D6C1
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

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x0002F4E4 File Offset: 0x0002D6E4
		// (set) Token: 0x06000AF8 RID: 2808 RVA: 0x0002F4EC File Offset: 0x0002D6EC
		[DataSourceProperty]
		public string SettlementImageID
		{
			get
			{
				return this._settlementImageID;
			}
			set
			{
				if (value != this._settlementImageID)
				{
					this._settlementImageID = value;
					base.OnPropertyChangedWithValue<string>(value, "SettlementImageID");
				}
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000AF9 RID: 2809 RVA: 0x0002F50F File Offset: 0x0002D70F
		// (set) Token: 0x06000AFA RID: 2810 RVA: 0x0002F517 File Offset: 0x0002D717
		[DataSourceProperty]
		public string NotableCharactersText
		{
			get
			{
				return this._notableCharactersText;
			}
			set
			{
				if (value != this._notableCharactersText)
				{
					this._notableCharactersText = value;
					base.OnPropertyChangedWithValue<string>(value, "NotableCharactersText");
				}
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000AFB RID: 2811 RVA: 0x0002F53A File Offset: 0x0002D73A
		// (set) Token: 0x06000AFC RID: 2812 RVA: 0x0002F542 File Offset: 0x0002D742
		[DataSourceProperty]
		public MBBindingList<EncyclopediaSettlementVM> BoundVillages
		{
			get
			{
				return this._boundVillages;
			}
			set
			{
				if (value != this._boundVillages)
				{
					this._boundVillages = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaSettlementVM>>(value, "BoundVillages");
				}
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000AFD RID: 2813 RVA: 0x0002F560 File Offset: 0x0002D760
		// (set) Token: 0x06000AFE RID: 2814 RVA: 0x0002F568 File Offset: 0x0002D768
		[DataSourceProperty]
		public MBBindingList<HeroVM> NotableCharacters
		{
			get
			{
				return this._notableCharacters;
			}
			set
			{
				if (value != this._notableCharacters)
				{
					this._notableCharacters = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroVM>>(value, "NotableCharacters");
				}
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x0002F586 File Offset: 0x0002D786
		// (set) Token: 0x06000B00 RID: 2816 RVA: 0x0002F58E File Offset: 0x0002D78E
		[DataSourceProperty]
		public BasicTooltipViewModel MilitasHint
		{
			get
			{
				return this._militasHint;
			}
			set
			{
				if (value != this._militasHint)
				{
					this._militasHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "MilitasHint");
				}
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x0002F5AC File Offset: 0x0002D7AC
		// (set) Token: 0x06000B02 RID: 2818 RVA: 0x0002F5B4 File Offset: 0x0002D7B4
		[DataSourceProperty]
		public BasicTooltipViewModel FoodHint
		{
			get
			{
				return this._foodHint;
			}
			set
			{
				if (value != this._foodHint)
				{
					this._foodHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "FoodHint");
				}
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000B03 RID: 2819 RVA: 0x0002F5D2 File Offset: 0x0002D7D2
		// (set) Token: 0x06000B04 RID: 2820 RVA: 0x0002F5DA File Offset: 0x0002D7DA
		[DataSourceProperty]
		public BasicTooltipViewModel GarrisonHint
		{
			get
			{
				return this._garrisonHint;
			}
			set
			{
				if (value != this._garrisonHint)
				{
					this._garrisonHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "GarrisonHint");
				}
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x0002F5F8 File Offset: 0x0002D7F8
		// (set) Token: 0x06000B06 RID: 2822 RVA: 0x0002F600 File Offset: 0x0002D800
		[DataSourceProperty]
		public BasicTooltipViewModel ProsperityHint
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
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "ProsperityHint");
				}
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000B07 RID: 2823 RVA: 0x0002F61E File Offset: 0x0002D81E
		// (set) Token: 0x06000B08 RID: 2824 RVA: 0x0002F626 File Offset: 0x0002D826
		[DataSourceProperty]
		public BasicTooltipViewModel LoyaltyHint
		{
			get
			{
				return this._loyaltyHint;
			}
			set
			{
				if (value != this._loyaltyHint)
				{
					this._loyaltyHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "LoyaltyHint");
				}
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000B09 RID: 2825 RVA: 0x0002F644 File Offset: 0x0002D844
		// (set) Token: 0x06000B0A RID: 2826 RVA: 0x0002F64C File Offset: 0x0002D84C
		[DataSourceProperty]
		public BasicTooltipViewModel SecurityHint
		{
			get
			{
				return this._securityHint;
			}
			set
			{
				if (value != this._securityHint)
				{
					this._securityHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "SecurityHint");
				}
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000B0B RID: 2827 RVA: 0x0002F66A File Offset: 0x0002D86A
		// (set) Token: 0x06000B0C RID: 2828 RVA: 0x0002F672 File Offset: 0x0002D872
		[DataSourceProperty]
		public BasicTooltipViewModel WallsHint
		{
			get
			{
				return this._wallsHint;
			}
			set
			{
				if (value != this._wallsHint)
				{
					this._wallsHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "WallsHint");
				}
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000B0D RID: 2829 RVA: 0x0002F690 File Offset: 0x0002D890
		// (set) Token: 0x06000B0E RID: 2830 RVA: 0x0002F698 File Offset: 0x0002D898
		[DataSourceProperty]
		public string MilitasText
		{
			get
			{
				return this._militasText;
			}
			set
			{
				if (value != this._militasText)
				{
					this._militasText = value;
					base.OnPropertyChangedWithValue<string>(value, "MilitasText");
				}
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000B0F RID: 2831 RVA: 0x0002F6BB File Offset: 0x0002D8BB
		// (set) Token: 0x06000B10 RID: 2832 RVA: 0x0002F6C3 File Offset: 0x0002D8C3
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

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000B11 RID: 2833 RVA: 0x0002F6E6 File Offset: 0x0002D8E6
		// (set) Token: 0x06000B12 RID: 2834 RVA: 0x0002F6EE File Offset: 0x0002D8EE
		[DataSourceProperty]
		public string LoyaltyText
		{
			get
			{
				return this._loyaltyText;
			}
			set
			{
				if (value != this._loyaltyText)
				{
					this._loyaltyText = value;
					base.OnPropertyChangedWithValue<string>(value, "LoyaltyText");
				}
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x0002F711 File Offset: 0x0002D911
		// (set) Token: 0x06000B14 RID: 2836 RVA: 0x0002F719 File Offset: 0x0002D919
		[DataSourceProperty]
		public string SecurityText
		{
			get
			{
				return this._securityText;
			}
			set
			{
				if (value != this._securityText)
				{
					this._securityText = value;
					base.OnPropertyChangedWithValue<string>(value, "SecurityText");
				}
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x0002F73C File Offset: 0x0002D93C
		// (set) Token: 0x06000B16 RID: 2838 RVA: 0x0002F744 File Offset: 0x0002D944
		[DataSourceProperty]
		public string WallsText
		{
			get
			{
				return this._wallsText;
			}
			set
			{
				if (value != this._wallsText)
				{
					this._wallsText = value;
					base.OnPropertyChangedWithValue<string>(value, "WallsText");
				}
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x0002F767 File Offset: 0x0002D967
		// (set) Token: 0x06000B18 RID: 2840 RVA: 0x0002F76F File Offset: 0x0002D96F
		[DataSourceProperty]
		public string FoodText
		{
			get
			{
				return this._foodText;
			}
			set
			{
				if (value != this._foodText)
				{
					this._foodText = value;
					base.OnPropertyChangedWithValue<string>(value, "FoodText");
				}
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x0002F792 File Offset: 0x0002D992
		// (set) Token: 0x06000B1A RID: 2842 RVA: 0x0002F79A File Offset: 0x0002D99A
		[DataSourceProperty]
		public string GarrisonText
		{
			get
			{
				return this._garrisonText;
			}
			set
			{
				if (value != this._garrisonText)
				{
					this._garrisonText = value;
					base.OnPropertyChangedWithValue<string>(value, "GarrisonText");
				}
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000B1B RID: 2843 RVA: 0x0002F7BD File Offset: 0x0002D9BD
		// (set) Token: 0x06000B1C RID: 2844 RVA: 0x0002F7C5 File Offset: 0x0002D9C5
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

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000B1D RID: 2845 RVA: 0x0002F7E8 File Offset: 0x0002D9E8
		// (set) Token: 0x06000B1E RID: 2846 RVA: 0x0002F7F0 File Offset: 0x0002D9F0
		[DataSourceProperty]
		public string OwnerText
		{
			get
			{
				return this._ownerText;
			}
			set
			{
				if (value != this._ownerText)
				{
					this._ownerText = value;
					base.OnPropertyChangedWithValue<string>(value, "OwnerText");
				}
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000B1F RID: 2847 RVA: 0x0002F813 File Offset: 0x0002DA13
		// (set) Token: 0x06000B20 RID: 2848 RVA: 0x0002F81B File Offset: 0x0002DA1B
		[DataSourceProperty]
		public HeroVM Governor
		{
			get
			{
				return this._governor;
			}
			set
			{
				if (value != this._governor)
				{
					this._governor = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "Governor");
				}
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000B21 RID: 2849 RVA: 0x0002F839 File Offset: 0x0002DA39
		// (set) Token: 0x06000B22 RID: 2850 RVA: 0x0002F841 File Offset: 0x0002DA41
		[DataSourceProperty]
		public bool HasNotables
		{
			get
			{
				return this._hasNotables;
			}
			set
			{
				if (value != this._hasNotables)
				{
					this._hasNotables = value;
					base.OnPropertyChangedWithValue(value, "HasNotables");
				}
			}
		}

		// Token: 0x040004CB RID: 1227
		private SettlementClaimantDecision _settlementDecision;

		// Token: 0x040004CC RID: 1228
		private SettlementClaimantPreliminaryDecision _settlementPreliminaryDecision;

		// Token: 0x040004CD RID: 1229
		private Settlement _settlement;

		// Token: 0x040004CE RID: 1230
		private string _settlementName;

		// Token: 0x040004CF RID: 1231
		private HeroVM _governor;

		// Token: 0x040004D0 RID: 1232
		private MBBindingList<EncyclopediaSettlementVM> _boundVillages;

		// Token: 0x040004D1 RID: 1233
		private MBBindingList<HeroVM> _notableCharacters;

		// Token: 0x040004D2 RID: 1234
		private BasicTooltipViewModel _militasHint;

		// Token: 0x040004D3 RID: 1235
		private BasicTooltipViewModel _prosperityHint;

		// Token: 0x040004D4 RID: 1236
		private BasicTooltipViewModel _loyaltyHint;

		// Token: 0x040004D5 RID: 1237
		private BasicTooltipViewModel _securityHint;

		// Token: 0x040004D6 RID: 1238
		private BasicTooltipViewModel _wallsHint;

		// Token: 0x040004D7 RID: 1239
		private BasicTooltipViewModel _garrisonHint;

		// Token: 0x040004D8 RID: 1240
		private BasicTooltipViewModel _foodHint;

		// Token: 0x040004D9 RID: 1241
		private HeroVM _owner;

		// Token: 0x040004DA RID: 1242
		private string _ownerText;

		// Token: 0x040004DB RID: 1243
		private string _militasText;

		// Token: 0x040004DC RID: 1244
		private string _garrisonText;

		// Token: 0x040004DD RID: 1245
		private string _prosperityText;

		// Token: 0x040004DE RID: 1246
		private string _loyaltyText;

		// Token: 0x040004DF RID: 1247
		private string _securityText;

		// Token: 0x040004E0 RID: 1248
		private string _wallsText;

		// Token: 0x040004E1 RID: 1249
		private string _foodText;

		// Token: 0x040004E2 RID: 1250
		private string _descriptorText;

		// Token: 0x040004E3 RID: 1251
		private string _villagesText;

		// Token: 0x040004E4 RID: 1252
		private string _notableCharactersText;

		// Token: 0x040004E5 RID: 1253
		private string _settlementPath;

		// Token: 0x040004E6 RID: 1254
		private string _informationText;

		// Token: 0x040004E7 RID: 1255
		private string _settlementImageID;

		// Token: 0x040004E8 RID: 1256
		private string _boundSettlementText;

		// Token: 0x040004E9 RID: 1257
		private string _detailsText;

		// Token: 0x040004EA RID: 1258
		private double _settlementCropPosition;

		// Token: 0x040004EB RID: 1259
		private bool _hasBoundSettlement;

		// Token: 0x040004EC RID: 1260
		private bool _hasNotables;
	}
}
