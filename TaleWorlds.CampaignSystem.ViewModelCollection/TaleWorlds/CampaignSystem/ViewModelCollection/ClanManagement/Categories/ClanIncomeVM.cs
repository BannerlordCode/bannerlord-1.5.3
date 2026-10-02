using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.ClanFinance;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Supporters;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories
{
	// Token: 0x02000140 RID: 320
	public class ClanIncomeVM : ViewModel
	{
		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06001E8C RID: 7820 RVA: 0x0006E3E2 File Offset: 0x0006C5E2
		// (set) Token: 0x06001E8D RID: 7821 RVA: 0x0006E3EA File Offset: 0x0006C5EA
		public int TotalIncome { get; private set; }

		// Token: 0x06001E8E RID: 7822 RVA: 0x0006E3F4 File Offset: 0x0006C5F4
		public ClanIncomeVM(Action onRefresh, Action<ClanCardSelectionInfo> openCardSelectionPopup)
		{
			this._onRefresh = onRefresh;
			this._openCardSelectionPopup = openCardSelectionPopup;
			this.Incomes = new MBBindingList<ClanFinanceWorkshopItemVM>();
			this.SupporterGroups = new MBBindingList<ClanSupporterGroupVM>();
			this.Alleys = new MBBindingList<ClanFinanceAlleyItemVM>();
			this.SortController = new ClanIncomeSortControllerVM(this._incomes, this._supporterGroups, this._alleys);
			this.RefreshList();
		}

		// Token: 0x06001E8F RID: 7823 RVA: 0x0006E45C File Offset: 0x0006C65C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.NameText = GameTexts.FindText("str_sort_by_name_label", null).ToString();
			this.IncomeText = GameTexts.FindText("str_income", null).ToString();
			this.LocationText = GameTexts.FindText("str_tooltip_label_location", null).ToString();
			this.NoAdditionalIncomesText = GameTexts.FindText("str_clan_no_additional_incomes", null).ToString();
			this.Incomes.ApplyActionOnAllItems(delegate(ClanFinanceWorkshopItemVM x)
			{
				x.RefreshValues();
			});
			ClanFinanceWorkshopItemVM currentSelectedIncome = this.CurrentSelectedIncome;
			if (currentSelectedIncome != null)
			{
				currentSelectedIncome.RefreshValues();
			}
			this.SortController.RefreshValues();
		}

		// Token: 0x06001E90 RID: 7824 RVA: 0x0006E510 File Offset: 0x0006C710
		public void RefreshList()
		{
			this.Incomes.Clear();
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsTown)
				{
					foreach (Workshop workshop in settlement.Town.Workshops)
					{
						if (workshop.Owner == Hero.MainHero)
						{
							this.Incomes.Add(new ClanFinanceWorkshopItemVM(workshop, new Action<ClanFinanceWorkshopItemVM>(this.OnIncomeSelection), new Action(this.OnRefresh), this._openCardSelectionPopup));
						}
					}
				}
			}
			this.RefreshSupporters();
			this.RefreshAlleys();
			this.SortController.ResetAllStates();
			GameTexts.SetVariable("STR1", GameTexts.FindText("str_clan_workshops", null));
			GameTexts.SetVariable("LEFT", Hero.MainHero.OwnedWorkshops.Count);
			GameTexts.SetVariable("RIGHT", Campaign.Current.Models.WorkshopModel.GetMaxWorkshopCountForClanTier(Clan.PlayerClan.Tier));
			GameTexts.SetVariable("STR2", GameTexts.FindText("str_LEFT_over_RIGHT_in_paranthesis", null));
			this.WorkshopText = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			int num = 0;
			foreach (ClanSupporterGroupVM clanSupporterGroupVM in this.SupporterGroups)
			{
				num += clanSupporterGroupVM.Supporters.Count;
			}
			GameTexts.SetVariable("RANK", new TextObject("{=RzFyGnWJ}Supporters", null).ToString());
			GameTexts.SetVariable("NUMBER", num);
			this.SupportersText = GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null).ToString();
			GameTexts.SetVariable("RANK", new TextObject("{=7tKjfMSb}Alleys", null).ToString());
			GameTexts.SetVariable("NUMBER", this.Alleys.Count);
			this.AlleysText = GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null).ToString();
			this.RefreshTotalIncome();
			this.OnIncomeSelection(this.GetDefaultIncome());
			this.RefreshValues();
		}

		// Token: 0x06001E91 RID: 7825 RVA: 0x0006E74C File Offset: 0x0006C94C
		private void RefreshSupporters()
		{
			foreach (ClanSupporterGroupVM clanSupporterGroupVM in this.SupporterGroups)
			{
				clanSupporterGroupVM.Supporters.Clear();
			}
			this.SupporterGroups.Clear();
			Dictionary<float, List<Hero>> dictionary = new Dictionary<float, List<Hero>>();
			NotablePowerModel notablePowerModel = Campaign.Current.Models.NotablePowerModel;
			foreach (Hero hero in Clan.PlayerClan.SupporterNotables.OrderBy<Hero, float>((Hero x) => x.Power))
			{
				if (hero.CurrentSettlement != null)
				{
					float influenceBonusToClan = notablePowerModel.GetInfluenceBonusToClan(hero);
					List<Hero> list;
					if (dictionary.TryGetValue(influenceBonusToClan, out list))
					{
						list.Add(hero);
					}
					else
					{
						dictionary.Add(influenceBonusToClan, new List<Hero> { hero });
					}
				}
			}
			foreach (KeyValuePair<float, List<Hero>> keyValuePair in dictionary)
			{
				if (keyValuePair.Value.Count > 0)
				{
					ClanSupporterGroupVM clanSupporterGroupVM2 = new ClanSupporterGroupVM(notablePowerModel.GetPowerRankName(keyValuePair.Value.FirstOrDefault<Hero>()), keyValuePair.Key, new Action<ClanSupporterGroupVM>(this.OnSupporterSelection));
					foreach (Hero hero2 in keyValuePair.Value)
					{
						clanSupporterGroupVM2.AddSupporter(hero2);
					}
					this.SupporterGroups.Add(clanSupporterGroupVM2);
				}
			}
			foreach (ClanSupporterGroupVM clanSupporterGroupVM3 in this.SupporterGroups)
			{
				clanSupporterGroupVM3.Refresh();
			}
		}

		// Token: 0x06001E92 RID: 7826 RVA: 0x0006E960 File Offset: 0x0006CB60
		private void RefreshAlleys()
		{
			this.Alleys.Clear();
			foreach (Alley alley in Hero.MainHero.OwnedAlleys)
			{
				this.Alleys.Add(new ClanFinanceAlleyItemVM(alley, this._openCardSelectionPopup, new Action<ClanFinanceAlleyItemVM>(this.OnAlleySelection), new Action(this.OnRefresh)));
			}
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x0006E9EC File Offset: 0x0006CBEC
		private ClanFinanceWorkshopItemVM GetDefaultIncome()
		{
			return this.Incomes.FirstOrDefault<ClanFinanceWorkshopItemVM>();
		}

		// Token: 0x06001E94 RID: 7828 RVA: 0x0006E9FC File Offset: 0x0006CBFC
		public void SelectWorkshop(Workshop workshop)
		{
			foreach (ClanFinanceWorkshopItemVM clanFinanceWorkshopItemVM in this.Incomes)
			{
				if (clanFinanceWorkshopItemVM != null)
				{
					ClanFinanceWorkshopItemVM clanFinanceWorkshopItemVM2 = clanFinanceWorkshopItemVM;
					if (clanFinanceWorkshopItemVM2.Workshop == workshop)
					{
						this.OnIncomeSelection(clanFinanceWorkshopItemVM2);
						break;
					}
				}
			}
		}

		// Token: 0x06001E95 RID: 7829 RVA: 0x0006EA5C File Offset: 0x0006CC5C
		public void SelectAlley(Alley alley)
		{
			for (int i = 0; i < this.Alleys.Count; i++)
			{
				if (this.Alleys[i].Alley == alley)
				{
					this.OnAlleySelection(this.Alleys[i]);
					return;
				}
			}
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x0006EAA8 File Offset: 0x0006CCA8
		private void OnAlleySelection(ClanFinanceAlleyItemVM alley)
		{
			if (alley == null)
			{
				if (this.CurrentSelectedAlley != null)
				{
					this.CurrentSelectedAlley.IsSelected = false;
				}
				this.CurrentSelectedAlley = null;
				return;
			}
			this.OnIncomeSelection(null);
			this.OnSupporterSelection(null);
			if (this.CurrentSelectedAlley != null)
			{
				this.CurrentSelectedAlley.IsSelected = false;
			}
			this.CurrentSelectedAlley = alley;
			if (alley != null)
			{
				alley.IsSelected = true;
			}
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x0006EB08 File Offset: 0x0006CD08
		private void OnIncomeSelection(ClanFinanceWorkshopItemVM income)
		{
			if (income == null)
			{
				if (this.CurrentSelectedIncome != null)
				{
					this.CurrentSelectedIncome.IsSelected = false;
				}
				this.CurrentSelectedIncome = null;
				return;
			}
			this.OnSupporterSelection(null);
			this.OnAlleySelection(null);
			if (this.CurrentSelectedIncome != null)
			{
				this.CurrentSelectedIncome.IsSelected = false;
			}
			this.CurrentSelectedIncome = income;
			if (income != null)
			{
				income.IsSelected = true;
			}
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x0006EB68 File Offset: 0x0006CD68
		private void OnSupporterSelection(ClanSupporterGroupVM supporter)
		{
			if (supporter == null)
			{
				if (this.CurrentSelectedSupporterGroup != null)
				{
					this.CurrentSelectedSupporterGroup.IsSelected = false;
				}
				this.CurrentSelectedSupporterGroup = null;
				return;
			}
			this.OnIncomeSelection(null);
			this.OnAlleySelection(null);
			if (this.CurrentSelectedSupporterGroup != null)
			{
				this.CurrentSelectedSupporterGroup.IsSelected = false;
			}
			this.CurrentSelectedSupporterGroup = supporter;
			if (this.CurrentSelectedSupporterGroup != null)
			{
				this.CurrentSelectedSupporterGroup.IsSelected = true;
			}
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x0006EBD1 File Offset: 0x0006CDD1
		public void RefreshTotalIncome()
		{
			this.TotalIncome = this.Incomes.Sum<ClanFinanceWorkshopItemVM>((ClanFinanceWorkshopItemVM i) => i.Income);
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x0006EC03 File Offset: 0x0006CE03
		public void OnRefresh()
		{
			Action onRefresh = this._onRefresh;
			if (onRefresh == null)
			{
				return;
			}
			onRefresh();
		}

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x06001E9B RID: 7835 RVA: 0x0006EC15 File Offset: 0x0006CE15
		// (set) Token: 0x06001E9C RID: 7836 RVA: 0x0006EC1D File Offset: 0x0006CE1D
		[DataSourceProperty]
		public ClanFinanceAlleyItemVM CurrentSelectedAlley
		{
			get
			{
				return this._currentSelectedAlley;
			}
			set
			{
				if (value != this._currentSelectedAlley)
				{
					this._currentSelectedAlley = value;
					base.OnPropertyChangedWithValue<ClanFinanceAlleyItemVM>(value, "CurrentSelectedAlley");
					this.IsAnyValidAlleySelected = value != null;
					this.IsAnyValidIncomeSelected = false;
					this.IsAnyValidSupporterSelected = false;
				}
			}
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06001E9D RID: 7837 RVA: 0x0006EC53 File Offset: 0x0006CE53
		// (set) Token: 0x06001E9E RID: 7838 RVA: 0x0006EC5B File Offset: 0x0006CE5B
		[DataSourceProperty]
		public ClanFinanceWorkshopItemVM CurrentSelectedIncome
		{
			get
			{
				return this._currentSelectedIncome;
			}
			set
			{
				if (value != this._currentSelectedIncome)
				{
					this._currentSelectedIncome = value;
					base.OnPropertyChangedWithValue<ClanFinanceWorkshopItemVM>(value, "CurrentSelectedIncome");
					this.IsAnyValidIncomeSelected = value != null;
					this.IsAnyValidSupporterSelected = false;
					this.IsAnyValidAlleySelected = false;
				}
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06001E9F RID: 7839 RVA: 0x0006EC91 File Offset: 0x0006CE91
		// (set) Token: 0x06001EA0 RID: 7840 RVA: 0x0006EC99 File Offset: 0x0006CE99
		[DataSourceProperty]
		public ClanSupporterGroupVM CurrentSelectedSupporterGroup
		{
			get
			{
				return this._currentSelectedSupporterGroup;
			}
			set
			{
				if (value != this._currentSelectedSupporterGroup)
				{
					this._currentSelectedSupporterGroup = value;
					base.OnPropertyChangedWithValue<ClanSupporterGroupVM>(value, "CurrentSelectedSupporterGroup");
					this.IsAnyValidSupporterSelected = value != null;
					this.IsAnyValidIncomeSelected = false;
					this.IsAnyValidAlleySelected = false;
				}
			}
		}

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06001EA1 RID: 7841 RVA: 0x0006ECCF File Offset: 0x0006CECF
		// (set) Token: 0x06001EA2 RID: 7842 RVA: 0x0006ECD7 File Offset: 0x0006CED7
		[DataSourceProperty]
		public bool IsAnyValidAlleySelected
		{
			get
			{
				return this._isAnyValidAlleySelected;
			}
			set
			{
				if (value != this._isAnyValidAlleySelected)
				{
					this._isAnyValidAlleySelected = value;
					base.OnPropertyChangedWithValue(value, "IsAnyValidAlleySelected");
				}
			}
		}

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06001EA3 RID: 7843 RVA: 0x0006ECF5 File Offset: 0x0006CEF5
		// (set) Token: 0x06001EA4 RID: 7844 RVA: 0x0006ECFD File Offset: 0x0006CEFD
		[DataSourceProperty]
		public bool IsAnyValidIncomeSelected
		{
			get
			{
				return this._isAnyValidIncomeSelected;
			}
			set
			{
				if (value != this._isAnyValidIncomeSelected)
				{
					this._isAnyValidIncomeSelected = value;
					base.OnPropertyChangedWithValue(value, "IsAnyValidIncomeSelected");
				}
			}
		}

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06001EA5 RID: 7845 RVA: 0x0006ED1B File Offset: 0x0006CF1B
		// (set) Token: 0x06001EA6 RID: 7846 RVA: 0x0006ED23 File Offset: 0x0006CF23
		[DataSourceProperty]
		public bool IsAnyValidSupporterSelected
		{
			get
			{
				return this._isAnyValidSupporterSelected;
			}
			set
			{
				if (value != this._isAnyValidSupporterSelected)
				{
					this._isAnyValidSupporterSelected = value;
					base.OnPropertyChangedWithValue(value, "IsAnyValidSupporterSelected");
				}
			}
		}

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06001EA7 RID: 7847 RVA: 0x0006ED41 File Offset: 0x0006CF41
		// (set) Token: 0x06001EA8 RID: 7848 RVA: 0x0006ED49 File Offset: 0x0006CF49
		[DataSourceProperty]
		public string IncomeText
		{
			get
			{
				return this._incomeText;
			}
			set
			{
				if (value != this._incomeText)
				{
					this._incomeText = value;
					base.OnPropertyChangedWithValue<string>(value, "IncomeText");
				}
			}
		}

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x06001EA9 RID: 7849 RVA: 0x0006ED6C File Offset: 0x0006CF6C
		// (set) Token: 0x06001EAA RID: 7850 RVA: 0x0006ED74 File Offset: 0x0006CF74
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

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06001EAB RID: 7851 RVA: 0x0006ED92 File Offset: 0x0006CF92
		// (set) Token: 0x06001EAC RID: 7852 RVA: 0x0006ED9A File Offset: 0x0006CF9A
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

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06001EAD RID: 7853 RVA: 0x0006EDBD File Offset: 0x0006CFBD
		// (set) Token: 0x06001EAE RID: 7854 RVA: 0x0006EDC5 File Offset: 0x0006CFC5
		[DataSourceProperty]
		public string LocationText
		{
			get
			{
				return this._locationText;
			}
			set
			{
				if (value != this._locationText)
				{
					this._locationText = value;
					base.OnPropertyChangedWithValue<string>(value, "LocationText");
				}
			}
		}

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06001EAF RID: 7855 RVA: 0x0006EDE8 File Offset: 0x0006CFE8
		// (set) Token: 0x06001EB0 RID: 7856 RVA: 0x0006EDF0 File Offset: 0x0006CFF0
		[DataSourceProperty]
		public string WorkshopText
		{
			get
			{
				return this._workshopsText;
			}
			set
			{
				if (value != this._workshopsText)
				{
					this._workshopsText = value;
					base.OnPropertyChangedWithValue<string>(value, "WorkshopText");
				}
			}
		}

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06001EB1 RID: 7857 RVA: 0x0006EE13 File Offset: 0x0006D013
		// (set) Token: 0x06001EB2 RID: 7858 RVA: 0x0006EE1B File Offset: 0x0006D01B
		[DataSourceProperty]
		public string SupportersText
		{
			get
			{
				return this._supportersText;
			}
			set
			{
				if (value != this._supportersText)
				{
					this._supportersText = value;
					base.OnPropertyChangedWithValue<string>(value, "SupportersText");
				}
			}
		}

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06001EB3 RID: 7859 RVA: 0x0006EE3E File Offset: 0x0006D03E
		// (set) Token: 0x06001EB4 RID: 7860 RVA: 0x0006EE46 File Offset: 0x0006D046
		[DataSourceProperty]
		public string AlleysText
		{
			get
			{
				return this._alleysText;
			}
			set
			{
				if (value != this._alleysText)
				{
					this._alleysText = value;
					base.OnPropertyChangedWithValue<string>(value, "AlleysText");
				}
			}
		}

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06001EB5 RID: 7861 RVA: 0x0006EE69 File Offset: 0x0006D069
		// (set) Token: 0x06001EB6 RID: 7862 RVA: 0x0006EE71 File Offset: 0x0006D071
		[DataSourceProperty]
		public string NoAdditionalIncomesText
		{
			get
			{
				return this._noAdditionalIncomesText;
			}
			set
			{
				if (this._noAdditionalIncomesText != value)
				{
					this._noAdditionalIncomesText = value;
					base.OnPropertyChangedWithValue<string>(value, "NoAdditionalIncomesText");
				}
			}
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06001EB7 RID: 7863 RVA: 0x0006EE94 File Offset: 0x0006D094
		// (set) Token: 0x06001EB8 RID: 7864 RVA: 0x0006EE9C File Offset: 0x0006D09C
		[DataSourceProperty]
		public MBBindingList<ClanFinanceWorkshopItemVM> Incomes
		{
			get
			{
				return this._incomes;
			}
			set
			{
				if (value != this._incomes)
				{
					this._incomes = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanFinanceWorkshopItemVM>>(value, "Incomes");
				}
			}
		}

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06001EB9 RID: 7865 RVA: 0x0006EEBA File Offset: 0x0006D0BA
		// (set) Token: 0x06001EBA RID: 7866 RVA: 0x0006EEC2 File Offset: 0x0006D0C2
		[DataSourceProperty]
		public MBBindingList<ClanSupporterGroupVM> SupporterGroups
		{
			get
			{
				return this._supporterGroups;
			}
			set
			{
				if (value != this._supporterGroups)
				{
					this._supporterGroups = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanSupporterGroupVM>>(value, "SupporterGroups");
				}
			}
		}

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06001EBB RID: 7867 RVA: 0x0006EEE0 File Offset: 0x0006D0E0
		// (set) Token: 0x06001EBC RID: 7868 RVA: 0x0006EEE8 File Offset: 0x0006D0E8
		[DataSourceProperty]
		public MBBindingList<ClanFinanceAlleyItemVM> Alleys
		{
			get
			{
				return this._alleys;
			}
			set
			{
				if (value != this._alleys)
				{
					this._alleys = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanFinanceAlleyItemVM>>(value, "Alleys");
				}
			}
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06001EBD RID: 7869 RVA: 0x0006EF06 File Offset: 0x0006D106
		// (set) Token: 0x06001EBE RID: 7870 RVA: 0x0006EF0E File Offset: 0x0006D10E
		[DataSourceProperty]
		public ClanIncomeSortControllerVM SortController
		{
			get
			{
				return this._sortController;
			}
			set
			{
				if (value != this._sortController)
				{
					this._sortController = value;
					base.OnPropertyChangedWithValue<ClanIncomeSortControllerVM>(value, "SortController");
				}
			}
		}

		// Token: 0x04000E01 RID: 3585
		private readonly Action _onRefresh;

		// Token: 0x04000E02 RID: 3586
		private readonly Action<ClanCardSelectionInfo> _openCardSelectionPopup;

		// Token: 0x04000E04 RID: 3588
		private MBBindingList<ClanFinanceWorkshopItemVM> _incomes;

		// Token: 0x04000E05 RID: 3589
		private MBBindingList<ClanSupporterGroupVM> _supporterGroups;

		// Token: 0x04000E06 RID: 3590
		private MBBindingList<ClanFinanceAlleyItemVM> _alleys;

		// Token: 0x04000E07 RID: 3591
		private ClanFinanceAlleyItemVM _currentSelectedAlley;

		// Token: 0x04000E08 RID: 3592
		private ClanFinanceWorkshopItemVM _currentSelectedIncome;

		// Token: 0x04000E09 RID: 3593
		private ClanSupporterGroupVM _currentSelectedSupporterGroup;

		// Token: 0x04000E0A RID: 3594
		private bool _isSelected;

		// Token: 0x04000E0B RID: 3595
		private string _nameText;

		// Token: 0x04000E0C RID: 3596
		private string _incomeText;

		// Token: 0x04000E0D RID: 3597
		private string _locationText;

		// Token: 0x04000E0E RID: 3598
		private string _workshopsText;

		// Token: 0x04000E0F RID: 3599
		private string _supportersText;

		// Token: 0x04000E10 RID: 3600
		private string _alleysText;

		// Token: 0x04000E11 RID: 3601
		private string _noAdditionalIncomesText;

		// Token: 0x04000E12 RID: 3602
		private bool _isAnyValidAlleySelected;

		// Token: 0x04000E13 RID: 3603
		private bool _isAnyValidIncomeSelected;

		// Token: 0x04000E14 RID: 3604
		private bool _isAnyValidSupporterSelected;

		// Token: 0x04000E15 RID: 3605
		private ClanIncomeSortControllerVM _sortController;
	}
}
