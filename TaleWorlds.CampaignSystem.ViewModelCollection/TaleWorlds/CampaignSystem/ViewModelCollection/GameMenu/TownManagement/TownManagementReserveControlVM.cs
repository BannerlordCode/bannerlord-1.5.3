using System;
using Helpers;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement
{
	// Token: 0x020000AF RID: 175
	public class TownManagementReserveControlVM : ViewModel
	{
		// Token: 0x06001074 RID: 4212 RVA: 0x00043514 File Offset: 0x00041714
		public TownManagementReserveControlVM(Settlement settlement, Action onReserveUpdated)
		{
			this._settlement = settlement;
			this._onReserveUpdated = onReserveUpdated;
			if (((settlement != null) ? settlement.Town : null) != null)
			{
				this.CurrentReserveAmount = Settlement.CurrentSettlement.Town.BoostBuildingProcess;
				this.CurrentGivenAmount = 0;
				this.MaxReserveAmount = MathF.Min(Hero.MainHero.Gold, 10000);
				this.AddGoldToReserveHint = new HintViewModel(new TextObject("{=TeZ3QzN6}Add gold to the reserve", null), null);
			}
			this.RefreshValues();
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x00043598 File Offset: 0x00041798
		public override void RefreshValues()
		{
			base.RefreshValues();
			Settlement settlement = this._settlement;
			if (((settlement != null) ? settlement.Town : null) != null)
			{
				this.ReserveText = new TextObject("{=2ckyCKR7}Reserve", null).ToString();
				GameTexts.SetVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
				this.UpdateReserveText();
			}
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x000435EC File Offset: 0x000417EC
		private void UpdateReserveText()
		{
			TextObject textObject = GameTexts.FindText("str_town_management_reserve_explanation", null);
			textObject.SetTextVariable("BOOST", Campaign.Current.Models.BuildingConstructionModel.GetBoostAmount(this._settlement.Town));
			textObject.SetTextVariable("COST", Campaign.Current.Models.BuildingConstructionModel.GetBoostCost(this._settlement.Town));
			this.ReserveBonusText = textObject.ToString();
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x00043668 File Offset: 0x00041868
		public void ExecuteConfirm()
		{
			this.IsEnabled = false;
			BuildingHelper.BoostBuildingProcessWithGold(this.CurrentReserveAmount + this.CurrentGivenAmount, Settlement.CurrentSettlement.Town);
			this.CurrentGivenAmount = 0;
			GameTexts.SetVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
			this.UpdateReserveText();
			this.MaxReserveAmount = MathF.Min(Hero.MainHero.Gold, 10000);
			this.CurrentReserveAmount = Settlement.CurrentSettlement.Town.BoostBuildingProcess;
			Action onReserveUpdated = this._onReserveUpdated;
			if (onReserveUpdated == null)
			{
				return;
			}
			onReserveUpdated();
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x000436F3 File Offset: 0x000418F3
		public void ExecuteCancel()
		{
			this.IsEnabled = false;
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001079 RID: 4217 RVA: 0x000436FC File Offset: 0x000418FC
		// (set) Token: 0x0600107A RID: 4218 RVA: 0x00043704 File Offset: 0x00041904
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x0600107B RID: 4219 RVA: 0x00043722 File Offset: 0x00041922
		// (set) Token: 0x0600107C RID: 4220 RVA: 0x0004372C File Offset: 0x0004192C
		[DataSourceProperty]
		public int CurrentReserveAmount
		{
			get
			{
				return this._currentReserveAmount;
			}
			set
			{
				if (value != this._currentReserveAmount)
				{
					this._currentReserveAmount = value;
					base.OnPropertyChangedWithValue(value, "CurrentReserveAmount");
					this.CurrentReserveText = (this.CurrentGivenAmount + value).ToString();
				}
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x0600107D RID: 4221 RVA: 0x0004376B File Offset: 0x0004196B
		// (set) Token: 0x0600107E RID: 4222 RVA: 0x00043773 File Offset: 0x00041973
		[DataSourceProperty]
		public int CurrentGivenAmount
		{
			get
			{
				return this._currentGivenAmount;
			}
			set
			{
				if (value != this._currentGivenAmount)
				{
					this._currentGivenAmount = value;
					base.OnPropertyChangedWithValue(value, "CurrentGivenAmount");
				}
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x0600107F RID: 4223 RVA: 0x00043791 File Offset: 0x00041991
		// (set) Token: 0x06001080 RID: 4224 RVA: 0x00043799 File Offset: 0x00041999
		[DataSourceProperty]
		public int MaxReserveAmount
		{
			get
			{
				return this._maxReserveAmount;
			}
			set
			{
				if (value != this._maxReserveAmount)
				{
					this._maxReserveAmount = value;
					base.OnPropertyChangedWithValue(value, "MaxReserveAmount");
				}
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001081 RID: 4225 RVA: 0x000437B7 File Offset: 0x000419B7
		// (set) Token: 0x06001082 RID: 4226 RVA: 0x000437BF File Offset: 0x000419BF
		[DataSourceProperty]
		public string ReserveBonusText
		{
			get
			{
				return this._reserveBonusText;
			}
			set
			{
				if (value != this._reserveBonusText)
				{
					this._reserveBonusText = value;
					base.OnPropertyChangedWithValue<string>(value, "ReserveBonusText");
				}
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001083 RID: 4227 RVA: 0x000437E2 File Offset: 0x000419E2
		// (set) Token: 0x06001084 RID: 4228 RVA: 0x000437EA File Offset: 0x000419EA
		[DataSourceProperty]
		public string ReserveText
		{
			get
			{
				return this._reserveText;
			}
			set
			{
				if (value != this._reserveText)
				{
					this._reserveText = value;
					base.OnPropertyChangedWithValue<string>(value, "ReserveText");
				}
			}
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001085 RID: 4229 RVA: 0x0004380D File Offset: 0x00041A0D
		// (set) Token: 0x06001086 RID: 4230 RVA: 0x00043815 File Offset: 0x00041A15
		[DataSourceProperty]
		public string CurrentReserveText
		{
			get
			{
				return this._currentReserveText;
			}
			set
			{
				if (value != this._currentReserveText)
				{
					this._currentReserveText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentReserveText");
				}
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001087 RID: 4231 RVA: 0x00043838 File Offset: 0x00041A38
		// (set) Token: 0x06001088 RID: 4232 RVA: 0x00043840 File Offset: 0x00041A40
		[DataSourceProperty]
		public HintViewModel AddGoldToReserveHint
		{
			get
			{
				return this._addGoldToReserveHint;
			}
			set
			{
				if (value != this._addGoldToReserveHint)
				{
					this._addGoldToReserveHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AddGoldToReserveHint");
				}
			}
		}

		// Token: 0x0400077A RID: 1914
		private readonly Action _onReserveUpdated;

		// Token: 0x0400077B RID: 1915
		private readonly Settlement _settlement;

		// Token: 0x0400077C RID: 1916
		private const int MaxOneTimeAmount = 10000;

		// Token: 0x0400077D RID: 1917
		private bool _isEnabled;

		// Token: 0x0400077E RID: 1918
		private string _reserveText;

		// Token: 0x0400077F RID: 1919
		private int _currentReserveAmount;

		// Token: 0x04000780 RID: 1920
		private int _currentGivenAmount;

		// Token: 0x04000781 RID: 1921
		private int _maxReserveAmount;

		// Token: 0x04000782 RID: 1922
		private string _reserveBonusText;

		// Token: 0x04000783 RID: 1923
		private string _currentReserveText;

		// Token: 0x04000784 RID: 1924
		private HintViewModel _addGoldToReserveHint;
	}
}
