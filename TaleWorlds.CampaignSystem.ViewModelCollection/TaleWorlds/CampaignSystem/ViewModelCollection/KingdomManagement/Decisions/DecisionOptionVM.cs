using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions
{
	// Token: 0x0200007B RID: 123
	public class DecisionOptionVM : ViewModel
	{
		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x0002AB22 File Offset: 0x00028D22
		// (set) Token: 0x060009A2 RID: 2466 RVA: 0x0002AB2A File Offset: 0x00028D2A
		public DecisionOutcome Option { get; private set; }

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x060009A3 RID: 2467 RVA: 0x0002AB33 File Offset: 0x00028D33
		// (set) Token: 0x060009A4 RID: 2468 RVA: 0x0002AB3B File Offset: 0x00028D3B
		public KingdomDecision Decision { get; private set; }

		// Token: 0x060009A5 RID: 2469 RVA: 0x0002AB44 File Offset: 0x00028D44
		public DecisionOptionVM(DecisionOutcome option, KingdomDecision decision, KingdomElection kingdomDecisionMaker, Action<DecisionOptionVM> onSelect, Action<DecisionOptionVM> onSupportStrengthChange)
		{
			this._onSelect = onSelect;
			this._onSupportStrengthChange = onSupportStrengthChange;
			this._kingdomDecisionMaker = kingdomDecisionMaker;
			this.Decision = decision;
			this.CurrentSupportWeight = Supporter.SupportWeights.Choose;
			this.OptionHint = new HintViewModel();
			this.IsPlayerSupporter = !this._kingdomDecisionMaker.IsPlayerChooser;
			this.SupportersOfThisOption = new MBBindingList<DecisionSupporterVM>();
			this.Option = option;
			if (option != null)
			{
				Clan sponsorClan = option.SponsorClan;
				if (((sponsorClan != null) ? sponsorClan.Leader : null) != null)
				{
					this.Sponsor = new HeroVM(option.SponsorClan.Leader, false);
				}
				List<Supporter> supporterList = option.SupporterList;
				if (supporterList != null && supporterList.Count > 0)
				{
					foreach (Supporter supporter in option.SupporterList)
					{
						if (supporter.SupportWeight > Supporter.SupportWeights.StayNeutral)
						{
							if (supporter.Clan != option.SponsorClan)
							{
								this.SupportersOfThisOption.Add(new DecisionSupporterVM(supporter.Name, supporter.ImagePath, supporter.Clan, supporter.SupportWeight));
							}
							else
							{
								this.SponsorWeightImagePath = DecisionSupporterVM.GetSupporterWeightImagePath(supporter.SupportWeight);
							}
						}
					}
				}
				this.IsOptionForAbstain = false;
			}
			else
			{
				this.IsOptionForAbstain = true;
			}
			this.RefreshValues();
			this.RefreshSupportOptionEnabled();
			this.RefreshCanChooseOption();
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x0002ACBC File Offset: 0x00028EBC
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this.Option != null)
			{
				this.Name = this.Option.GetDecisionTitle().ToString();
				this.Description = this.Option.GetDecisionDescription().ToString();
			}
			else
			{
				this.Name = GameTexts.FindText("str_abstain", null).ToString();
				this.Description = GameTexts.FindText(this.IsPlayerSupporter ? "str_kingdom_decision_abstain_desc" : "str_kingdom_decision_ruler_abstain_desc", null).ToString();
			}
			MBBindingList<DecisionSupporterVM> supportersOfThisOption = this.SupportersOfThisOption;
			if (supportersOfThisOption == null)
			{
				return;
			}
			supportersOfThisOption.ApplyActionOnAllItems(delegate(DecisionSupporterVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0002AD70 File Offset: 0x00028F70
		private void ExecuteShowSupporterTooltip()
		{
			DecisionOutcome option = this.Option;
			if (option != null && option.SupporterList.Count > 0)
			{
				List<TooltipProperty> list = new List<TooltipProperty>();
				this._kingdomDecisionMaker.DetermineOfficialSupport();
				foreach (Supporter supporter in this.Option.SupporterList)
				{
					if (supporter.SupportWeight > Supporter.SupportWeights.StayNeutral && !supporter.IsPlayer)
					{
						int influenceCost = this.Decision.GetInfluenceCost(this.Option, supporter.Clan, supporter.SupportWeight);
						GameTexts.SetVariable("AMOUNT", influenceCost);
						GameTexts.SetVariable("INFLUENCE_ICON", "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"5\">");
						list.Add(new TooltipProperty(supporter.Name.ToString(), GameTexts.FindText("str_amount_with_influence_icon", null).ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
					}
				}
				InformationManager.ShowTooltip(typeof(List<TooltipProperty>), new object[] { list });
			}
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0002AE80 File Offset: 0x00029080
		private void ExecuteHideSupporterTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0002AE88 File Offset: 0x00029088
		private void RefreshSupportOptionEnabled()
		{
			int influenceCostOfOutcome = this._kingdomDecisionMaker.GetInfluenceCostOfOutcome(this.Option, Clan.PlayerClan, Supporter.SupportWeights.SlightlyFavor);
			int influenceCostOfOutcome2 = this._kingdomDecisionMaker.GetInfluenceCostOfOutcome(this.Option, Clan.PlayerClan, Supporter.SupportWeights.StronglyFavor);
			int influenceCostOfOutcome3 = this._kingdomDecisionMaker.GetInfluenceCostOfOutcome(this.Option, Clan.PlayerClan, Supporter.SupportWeights.FullyPush);
			this.SupportOption1Text = influenceCostOfOutcome.ToString();
			this.SupportOption2Text = influenceCostOfOutcome2.ToString();
			this.SupportOption3Text = influenceCostOfOutcome3.ToString();
			this.IsSupportOption1Enabled = (float)influenceCostOfOutcome <= Clan.PlayerClan.Influence && !this.IsOptionForAbstain;
			this.IsSupportOption2Enabled = (float)influenceCostOfOutcome2 <= Clan.PlayerClan.Influence && !this.IsOptionForAbstain;
			this.IsSupportOption3Enabled = (float)influenceCostOfOutcome3 <= Clan.PlayerClan.Influence && !this.IsOptionForAbstain;
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0002AF64 File Offset: 0x00029164
		private void OnSupportStrengthChange(int index)
		{
			if (!this.IsOptionForAbstain)
			{
				switch (index)
				{
				case 0:
					this.CurrentSupportWeight = Supporter.SupportWeights.SlightlyFavor;
					break;
				case 1:
					this.CurrentSupportWeight = Supporter.SupportWeights.StronglyFavor;
					break;
				case 2:
					this.CurrentSupportWeight = Supporter.SupportWeights.FullyPush;
					break;
				}
				this._kingdomDecisionMaker.OnPlayerSupport((!this.IsOptionForAbstain) ? this.Option : null, this.CurrentSupportWeight);
				this._onSupportStrengthChange(this);
			}
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0002AFD4 File Offset: 0x000291D4
		public void AfterKingChooseOutcome()
		{
			this._hasKingChoosen = true;
			this.RefreshCanChooseOption();
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x0002AFE4 File Offset: 0x000291E4
		private void RefreshCanChooseOption()
		{
			if (this._hasKingChoosen)
			{
				this.CanBeChosen = false;
				return;
			}
			if (this.IsOptionForAbstain)
			{
				this.CanBeChosen = true;
				return;
			}
			if (this.IsPlayerSupporter)
			{
				this.CanBeChosen = (float)this._kingdomDecisionMaker.GetInfluenceCostOfOutcome(this.Option, Clan.PlayerClan, Supporter.SupportWeights.SlightlyFavor) <= Clan.PlayerClan.Influence;
			}
			else
			{
				int influenceCostOfOutcome = this._kingdomDecisionMaker.GetInfluenceCostOfOutcome(this.Option, Clan.PlayerClan, Supporter.SupportWeights.Choose);
				this.CanBeChosen = (float)influenceCostOfOutcome <= Clan.PlayerClan.Influence || influenceCostOfOutcome == 0;
			}
			this.OptionHint.HintText = (this.CanBeChosen ? TextObject.GetEmpty() : new TextObject("{=Xmw93W6a}Not Enough Influence", null));
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0002B0A0 File Offset: 0x000292A0
		private void ExecuteSelection()
		{
			this._onSelect(this);
			Game game = Game.Current;
			if (game == null)
			{
				return;
			}
			EventManager eventManager = game.EventManager;
			if (eventManager == null)
			{
				return;
			}
			eventManager.TriggerEvent<PlayerSelectedAKingdomDecisionOptionEvent>(new PlayerSelectedAKingdomDecisionOptionEvent(this.Option));
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x060009AE RID: 2478 RVA: 0x0002B0D2 File Offset: 0x000292D2
		// (set) Token: 0x060009AF RID: 2479 RVA: 0x0002B0DA File Offset: 0x000292DA
		[DataSourceProperty]
		public HintViewModel OptionHint
		{
			get
			{
				return this._optionHint;
			}
			set
			{
				if (value != this._optionHint)
				{
					this._optionHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "OptionHint");
				}
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x060009B0 RID: 2480 RVA: 0x0002B0F8 File Offset: 0x000292F8
		// (set) Token: 0x060009B1 RID: 2481 RVA: 0x0002B100 File Offset: 0x00029300
		[DataSourceProperty]
		public MBBindingList<DecisionSupporterVM> SupportersOfThisOption
		{
			get
			{
				return this._supportersOfThisOption;
			}
			set
			{
				if (value != this._supportersOfThisOption)
				{
					this._supportersOfThisOption = value;
					base.OnPropertyChangedWithValue<MBBindingList<DecisionSupporterVM>>(value, "SupportersOfThisOption");
				}
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x060009B2 RID: 2482 RVA: 0x0002B11E File Offset: 0x0002931E
		// (set) Token: 0x060009B3 RID: 2483 RVA: 0x0002B126 File Offset: 0x00029326
		[DataSourceProperty]
		public HeroVM Sponsor
		{
			get
			{
				return this._sponsor;
			}
			set
			{
				if (value != this._sponsor)
				{
					this._sponsor = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "Sponsor");
				}
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x060009B4 RID: 2484 RVA: 0x0002B144 File Offset: 0x00029344
		// (set) Token: 0x060009B5 RID: 2485 RVA: 0x0002B14C File Offset: 0x0002934C
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x060009B6 RID: 2486 RVA: 0x0002B16F File Offset: 0x0002936F
		// (set) Token: 0x060009B7 RID: 2487 RVA: 0x0002B177 File Offset: 0x00029377
		[DataSourceProperty]
		public string SponsorWeightImagePath
		{
			get
			{
				return this._sponsorWeightImagePath;
			}
			set
			{
				if (value != this._sponsorWeightImagePath)
				{
					this._sponsorWeightImagePath = value;
					base.OnPropertyChangedWithValue<string>(value, "SponsorWeightImagePath");
				}
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x060009B8 RID: 2488 RVA: 0x0002B19A File Offset: 0x0002939A
		// (set) Token: 0x060009B9 RID: 2489 RVA: 0x0002B1A2 File Offset: 0x000293A2
		[DataSourceProperty]
		public bool CanBeChosen
		{
			get
			{
				return this._canBeChosen;
			}
			set
			{
				if (value != this._canBeChosen)
				{
					this._canBeChosen = value;
					base.OnPropertyChangedWithValue(value, "CanBeChosen");
				}
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x060009BA RID: 2490 RVA: 0x0002B1C0 File Offset: 0x000293C0
		// (set) Token: 0x060009BB RID: 2491 RVA: 0x0002B1C8 File Offset: 0x000293C8
		[DataSourceProperty]
		public bool IsKingsOutcome
		{
			get
			{
				return this._isKingsOutcome;
			}
			set
			{
				if (value != this._isKingsOutcome)
				{
					this._isKingsOutcome = value;
					base.OnPropertyChangedWithValue(value, "IsKingsOutcome");
				}
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x0002B1E6 File Offset: 0x000293E6
		// (set) Token: 0x060009BD RID: 2493 RVA: 0x0002B1EE File Offset: 0x000293EE
		[DataSourceProperty]
		public bool IsPlayerSupporter
		{
			get
			{
				return this._isPlayerSupporter;
			}
			set
			{
				if (value != this._isPlayerSupporter)
				{
					this._isPlayerSupporter = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerSupporter");
				}
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x0002B20C File Offset: 0x0002940C
		// (set) Token: 0x060009BF RID: 2495 RVA: 0x0002B214 File Offset: 0x00029414
		[DataSourceProperty]
		public bool IsHighlightEnabled
		{
			get
			{
				return this._isHighlightEnabled;
			}
			set
			{
				if (value != this._isHighlightEnabled)
				{
					this._isHighlightEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsHighlightEnabled");
				}
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x060009C0 RID: 2496 RVA: 0x0002B232 File Offset: 0x00029432
		// (set) Token: 0x060009C1 RID: 2497 RVA: 0x0002B23A File Offset: 0x0002943A
		[DataSourceProperty]
		public int WinPercentage
		{
			get
			{
				return this._winPercentage;
			}
			set
			{
				if (value != this._winPercentage)
				{
					this._winPercentage = value;
					base.OnPropertyChangedWithValue(value, "WinPercentage");
					GameTexts.SetVariable("NUMBER", value);
					this.WinPercentageStr = GameTexts.FindText("str_NUMBER_percent", null).ToString();
				}
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x060009C2 RID: 2498 RVA: 0x0002B279 File Offset: 0x00029479
		// (set) Token: 0x060009C3 RID: 2499 RVA: 0x0002B281 File Offset: 0x00029481
		[DataSourceProperty]
		public string WinPercentageStr
		{
			get
			{
				return this._winPercentageStr;
			}
			set
			{
				if (value != this._winPercentageStr)
				{
					this._winPercentageStr = value;
					base.OnPropertyChangedWithValue<string>(value, "WinPercentageStr");
				}
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x060009C4 RID: 2500 RVA: 0x0002B2A4 File Offset: 0x000294A4
		// (set) Token: 0x060009C5 RID: 2501 RVA: 0x0002B2AC File Offset: 0x000294AC
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x060009C6 RID: 2502 RVA: 0x0002B2CF File Offset: 0x000294CF
		// (set) Token: 0x060009C7 RID: 2503 RVA: 0x0002B2D7 File Offset: 0x000294D7
		[DataSourceProperty]
		public int InitialPercentage
		{
			get
			{
				return this._initialPercentage;
			}
			set
			{
				if (value != this._initialPercentage)
				{
					this._initialPercentage = value;
					base.OnPropertyChangedWithValue(value, "InitialPercentage");
				}
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x0002B2F5 File Offset: 0x000294F5
		// (set) Token: 0x060009C9 RID: 2505 RVA: 0x0002B2FD File Offset: 0x000294FD
		[DataSourceProperty]
		public int InfluenceCost
		{
			get
			{
				return this._influenceCost;
			}
			set
			{
				if (value != this._influenceCost)
				{
					this._influenceCost = value;
					base.OnPropertyChangedWithValue(value, "InfluenceCost");
				}
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x0002B31B File Offset: 0x0002951B
		// (set) Token: 0x060009CB RID: 2507 RVA: 0x0002B323 File Offset: 0x00029523
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x0002B341 File Offset: 0x00029541
		// (set) Token: 0x060009CD RID: 2509 RVA: 0x0002B349 File Offset: 0x00029549
		[DataSourceProperty]
		public bool IsOptionForAbstain
		{
			get
			{
				return this._isOptionForAbstain;
			}
			set
			{
				if (value != this._isOptionForAbstain)
				{
					this._isOptionForAbstain = value;
					base.OnPropertyChangedWithValue(value, "IsOptionForAbstain");
				}
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x060009CE RID: 2510 RVA: 0x0002B367 File Offset: 0x00029567
		// (set) Token: 0x060009CF RID: 2511 RVA: 0x0002B36F File Offset: 0x0002956F
		[DataSourceProperty]
		public Supporter.SupportWeights CurrentSupportWeight
		{
			get
			{
				return this._currentSupportWeight;
			}
			set
			{
				if (value != this._currentSupportWeight)
				{
					this._currentSupportWeight = value;
					base.OnPropertyChanged("CurrentSupportWeight");
					this.CurrentSupportWeightIndex = (int)value;
				}
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x060009D0 RID: 2512 RVA: 0x0002B393 File Offset: 0x00029593
		// (set) Token: 0x060009D1 RID: 2513 RVA: 0x0002B39B File Offset: 0x0002959B
		[DataSourceProperty]
		public int CurrentSupportWeightIndex
		{
			get
			{
				return this._currentSupportWeightIndex;
			}
			set
			{
				if (value != this._currentSupportWeightIndex)
				{
					this._currentSupportWeightIndex = value;
					base.OnPropertyChangedWithValue(value, "CurrentSupportWeightIndex");
				}
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x060009D2 RID: 2514 RVA: 0x0002B3B9 File Offset: 0x000295B9
		// (set) Token: 0x060009D3 RID: 2515 RVA: 0x0002B3C1 File Offset: 0x000295C1
		[DataSourceProperty]
		public string SupportOption1Text
		{
			get
			{
				return this._supportOption1Text;
			}
			set
			{
				if (value != this._supportOption1Text)
				{
					this._supportOption1Text = value;
					base.OnPropertyChangedWithValue<string>(value, "SupportOption1Text");
				}
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x060009D4 RID: 2516 RVA: 0x0002B3E4 File Offset: 0x000295E4
		// (set) Token: 0x060009D5 RID: 2517 RVA: 0x0002B3EC File Offset: 0x000295EC
		[DataSourceProperty]
		public string SupportOption2Text
		{
			get
			{
				return this._supportOption2Text;
			}
			set
			{
				if (value != this._supportOption2Text)
				{
					this._supportOption2Text = value;
					base.OnPropertyChangedWithValue<string>(value, "SupportOption2Text");
				}
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x060009D6 RID: 2518 RVA: 0x0002B40F File Offset: 0x0002960F
		// (set) Token: 0x060009D7 RID: 2519 RVA: 0x0002B417 File Offset: 0x00029617
		[DataSourceProperty]
		public string SupportOption3Text
		{
			get
			{
				return this._supportOption3Text;
			}
			set
			{
				if (value != this._supportOption3Text)
				{
					this._supportOption3Text = value;
					base.OnPropertyChangedWithValue<string>(value, "SupportOption3Text");
				}
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x060009D8 RID: 2520 RVA: 0x0002B43A File Offset: 0x0002963A
		// (set) Token: 0x060009D9 RID: 2521 RVA: 0x0002B442 File Offset: 0x00029642
		[DataSourceProperty]
		public bool IsSupportOption1Enabled
		{
			get
			{
				return this._isSupportOption1Enabled;
			}
			set
			{
				if (value != this._isSupportOption1Enabled)
				{
					this._isSupportOption1Enabled = value;
					base.OnPropertyChangedWithValue(value, "IsSupportOption1Enabled");
				}
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x060009DA RID: 2522 RVA: 0x0002B460 File Offset: 0x00029660
		// (set) Token: 0x060009DB RID: 2523 RVA: 0x0002B468 File Offset: 0x00029668
		[DataSourceProperty]
		public bool IsSupportOption2Enabled
		{
			get
			{
				return this._isSupportOption2Enabled;
			}
			set
			{
				if (value != this._isSupportOption2Enabled)
				{
					this._isSupportOption2Enabled = value;
					base.OnPropertyChangedWithValue(value, "IsSupportOption2Enabled");
				}
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x060009DC RID: 2524 RVA: 0x0002B486 File Offset: 0x00029686
		// (set) Token: 0x060009DD RID: 2525 RVA: 0x0002B48E File Offset: 0x0002968E
		[DataSourceProperty]
		public bool IsSupportOption3Enabled
		{
			get
			{
				return this._isSupportOption3Enabled;
			}
			set
			{
				if (value != this._isSupportOption3Enabled)
				{
					this._isSupportOption3Enabled = value;
					base.OnPropertyChangedWithValue(value, "IsSupportOption3Enabled");
				}
			}
		}

		// Token: 0x0400043B RID: 1083
		private readonly Action<DecisionOptionVM> _onSelect;

		// Token: 0x0400043C RID: 1084
		private readonly Action<DecisionOptionVM> _onSupportStrengthChange;

		// Token: 0x0400043D RID: 1085
		private readonly KingdomElection _kingdomDecisionMaker;

		// Token: 0x0400043E RID: 1086
		private bool _hasKingChoosen;

		// Token: 0x0400043F RID: 1087
		private MBBindingList<DecisionSupporterVM> _supportersOfThisOption;

		// Token: 0x04000440 RID: 1088
		private HeroVM _sponsor;

		// Token: 0x04000441 RID: 1089
		private bool _isOptionForAbstain;

		// Token: 0x04000442 RID: 1090
		private bool _isPlayerSupporter;

		// Token: 0x04000443 RID: 1091
		private bool _isSelected;

		// Token: 0x04000444 RID: 1092
		private bool _canBeChosen;

		// Token: 0x04000445 RID: 1093
		private bool _isKingsOutcome;

		// Token: 0x04000446 RID: 1094
		private bool _isHighlightEnabled;

		// Token: 0x04000447 RID: 1095
		private int _winPercentage = -1;

		// Token: 0x04000448 RID: 1096
		private int _influenceCost;

		// Token: 0x04000449 RID: 1097
		private int _initialPercentage = -99;

		// Token: 0x0400044A RID: 1098
		private int _currentSupportWeightIndex;

		// Token: 0x0400044B RID: 1099
		private string _name;

		// Token: 0x0400044C RID: 1100
		private string _description;

		// Token: 0x0400044D RID: 1101
		private string _winPercentageStr;

		// Token: 0x0400044E RID: 1102
		private string _sponsorWeightImagePath;

		// Token: 0x0400044F RID: 1103
		private Supporter.SupportWeights _currentSupportWeight;

		// Token: 0x04000450 RID: 1104
		private string _supportOption1Text;

		// Token: 0x04000451 RID: 1105
		private bool _isSupportOption1Enabled;

		// Token: 0x04000452 RID: 1106
		private string _supportOption2Text;

		// Token: 0x04000453 RID: 1107
		private bool _isSupportOption2Enabled;

		// Token: 0x04000454 RID: 1108
		private string _supportOption3Text;

		// Token: 0x04000455 RID: 1109
		private bool _isSupportOption3Enabled;

		// Token: 0x04000456 RID: 1110
		private HintViewModel _optionHint;
	}
}
