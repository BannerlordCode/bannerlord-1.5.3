using System;
using Helpers;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement
{
	// Token: 0x020000A8 RID: 168
	public class SettlementBuildingProjectVM : SettlementProjectVM
	{
		// Token: 0x06000FEA RID: 4074 RVA: 0x00041EF0 File Offset: 0x000400F0
		public SettlementBuildingProjectVM(Action<SettlementProjectVM, bool> onSelection, Action<SettlementProjectVM> onSetAsCurrent, Action onResetCurrent, Building building, Settlement settlement)
			: base(onSelection, onSetAsCurrent, onResetCurrent, building, settlement)
		{
			this.Level = building.CurrentLevel;
			this.MaxLevel = 3;
			this.DevelopmentLevelText = building.CurrentLevel.ToString();
			this.CanBuild = this.Level < 3;
			base.IsDaily = false;
			this.RefreshValues();
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x00041F56 File Offset: 0x00040156
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.AlreadyAtMaxText = new TextObject("{=ybLA7ZXp}Already at Max", null).ToString();
			this.UpdateProjectHints();
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x00041F7C File Offset: 0x0004017C
		private void UpdateProjectHints()
		{
			if (this.AddRemoveHint == null)
			{
				this.AddRemoveHint = new HintViewModel();
			}
			if (this.SetAsActiveHint == null)
			{
				this.SetAsActiveHint = new HintViewModel();
			}
			this.AddRemoveHint.HintText = (this.IsInQueue ? new TextObject("{=faDegful}Remove from queue", null) : new TextObject("{=SFebv4hH}Add to queue", null));
			this.SetAsActiveHint.HintText = ((this.DevelopmentQueueIndex == 0) ? new TextObject("{=cD1HTdYJ}Already active development", null) : new TextObject("{=PcLGc2bM}Set as active development", null));
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x00042008 File Offset: 0x00040208
		public override void RefreshProductionText()
		{
			base.RefreshProductionText();
			if (this.DevelopmentQueueIndex == 0)
			{
				GameTexts.SetVariable("LEFT", GameTexts.FindText("str_completion", null));
				int daysToComplete = BuildingHelper.GetDaysToComplete(base.Building, this._settlement.Town);
				TextObject textObject;
				if (daysToComplete != -1)
				{
					textObject = new TextObject("{=c5eYzHaM}{DAYS} {?DAY_IS_PLURAL}Days{?}Day{\\?} ({PERCENTAGE}%)", null);
					textObject.SetTextVariable("DAYS", daysToComplete);
					GameTexts.SetVariable("DAY_IS_PLURAL", (daysToComplete > 1) ? 1 : 0);
				}
				else
				{
					textObject = new TextObject("{=0TauthlH}Never ({PERCENTAGE}%)", null);
				}
				textObject.SetTextVariable("PERCENTAGE", (int)(BuildingHelper.GetProgressOfBuilding(base.Building, this._settlement.Town) * 100f));
				GameTexts.SetVariable("RIGHT", textObject);
				base.ProductionText = GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).ToString();
				return;
			}
			if (this.DevelopmentQueueIndex > 0)
			{
				GameTexts.SetVariable("NUMBER", this.DevelopmentQueueIndex);
				base.ProductionText = GameTexts.FindText("str_in_queue_with_number", null).ToString();
				return;
			}
			base.ProductionText = " ";
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x00042115 File Offset: 0x00040315
		public override void ExecuteAddRemoveToQueue()
		{
			if (this._onSelection != null && this.CanBuild)
			{
				this._onSelection(this, false);
			}
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x00042134 File Offset: 0x00040334
		public override void ExecuteSetAsActiveDevelopment()
		{
			if (this._onSelection != null && this.CanBuild)
			{
				this._onSelection(this, true);
			}
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x00042153 File Offset: 0x00040353
		public override void ExecuteSetAsCurrent()
		{
			Action<SettlementProjectVM> onSetAsCurrent = this._onSetAsCurrent;
			if (onSetAsCurrent == null)
			{
				return;
			}
			onSetAsCurrent(this);
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x00042166 File Offset: 0x00040366
		public override void ExecuteResetCurrent()
		{
			Action onResetCurrent = this._onResetCurrent;
			if (onResetCurrent == null)
			{
				return;
			}
			onResetCurrent();
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x00042178 File Offset: 0x00040378
		public override void ExecuteToggleSelected()
		{
			if (this.CanBuild)
			{
				this.IsSelected = !this.IsSelected;
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06000FF3 RID: 4083 RVA: 0x00042191 File Offset: 0x00040391
		// (set) Token: 0x06000FF4 RID: 4084 RVA: 0x00042199 File Offset: 0x00040399
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

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06000FF5 RID: 4085 RVA: 0x000421B7 File Offset: 0x000403B7
		// (set) Token: 0x06000FF6 RID: 4086 RVA: 0x000421BF File Offset: 0x000403BF
		[DataSourceProperty]
		public string DevelopmentLevelText
		{
			get
			{
				return this._developmentLevelText;
			}
			set
			{
				if (value != this._developmentLevelText)
				{
					this._developmentLevelText = value;
					base.OnPropertyChangedWithValue<string>(value, "DevelopmentLevelText");
				}
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06000FF7 RID: 4087 RVA: 0x000421E2 File Offset: 0x000403E2
		// (set) Token: 0x06000FF8 RID: 4088 RVA: 0x000421EA File Offset: 0x000403EA
		[DataSourceProperty]
		public int Level
		{
			get
			{
				return this._level;
			}
			set
			{
				if (value != this._level)
				{
					this._level = value;
					base.OnPropertyChangedWithValue(value, "Level");
				}
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06000FF9 RID: 4089 RVA: 0x00042208 File Offset: 0x00040408
		// (set) Token: 0x06000FFA RID: 4090 RVA: 0x00042210 File Offset: 0x00040410
		[DataSourceProperty]
		public int MaxLevel
		{
			get
			{
				return this._maxLevel;
			}
			set
			{
				if (value != this._maxLevel)
				{
					this._maxLevel = value;
					base.OnPropertyChangedWithValue(value, "MaxLevel");
				}
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06000FFB RID: 4091 RVA: 0x0004222E File Offset: 0x0004042E
		// (set) Token: 0x06000FFC RID: 4092 RVA: 0x00042236 File Offset: 0x00040436
		[DataSourceProperty]
		public int DevelopmentQueueIndex
		{
			get
			{
				return this._developmentQueueIndex;
			}
			set
			{
				if (value != this._developmentQueueIndex)
				{
					this._developmentQueueIndex = value;
					base.OnPropertyChangedWithValue(value, "DevelopmentQueueIndex");
					this.UpdateProjectHints();
				}
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06000FFD RID: 4093 RVA: 0x0004225A File Offset: 0x0004045A
		// (set) Token: 0x06000FFE RID: 4094 RVA: 0x00042262 File Offset: 0x00040462
		[DataSourceProperty]
		public bool IsInQueue
		{
			get
			{
				return this._isInQueue;
			}
			set
			{
				if (value != this._isInQueue)
				{
					this._isInQueue = value;
					base.OnPropertyChangedWithValue(value, "IsInQueue");
					this.UpdateProjectHints();
				}
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06000FFF RID: 4095 RVA: 0x00042286 File Offset: 0x00040486
		// (set) Token: 0x06001000 RID: 4096 RVA: 0x0004228E File Offset: 0x0004048E
		[DataSourceProperty]
		public string AlreadyAtMaxText
		{
			get
			{
				return this._alreadyAtMaxText;
			}
			set
			{
				if (value != this._alreadyAtMaxText)
				{
					this._alreadyAtMaxText = value;
					base.OnPropertyChangedWithValue<string>(value, "AlreadyAtMaxText");
				}
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001001 RID: 4097 RVA: 0x000422B1 File Offset: 0x000404B1
		// (set) Token: 0x06001002 RID: 4098 RVA: 0x000422B9 File Offset: 0x000404B9
		[DataSourceProperty]
		public bool CanBuild
		{
			get
			{
				return this._canBuild;
			}
			set
			{
				if (value != this._canBuild)
				{
					this._canBuild = value;
					base.OnPropertyChangedWithValue(value, "CanBuild");
				}
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001003 RID: 4099 RVA: 0x000422D7 File Offset: 0x000404D7
		// (set) Token: 0x06001004 RID: 4100 RVA: 0x000422DF File Offset: 0x000404DF
		[DataSourceProperty]
		public HintViewModel AddRemoveHint
		{
			get
			{
				return this._addRemoveHint;
			}
			set
			{
				if (value != this._addRemoveHint)
				{
					this._addRemoveHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AddRemoveHint");
				}
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06001005 RID: 4101 RVA: 0x000422FD File Offset: 0x000404FD
		// (set) Token: 0x06001006 RID: 4102 RVA: 0x00042305 File Offset: 0x00040505
		[DataSourceProperty]
		public HintViewModel SetAsActiveHint
		{
			get
			{
				return this._setAsActiveHint;
			}
			set
			{
				if (value != this._setAsActiveHint)
				{
					this._setAsActiveHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "SetAsActiveHint");
				}
			}
		}

		// Token: 0x0400073E RID: 1854
		private bool _isSelected;

		// Token: 0x0400073F RID: 1855
		private string _alreadyAtMaxText;

		// Token: 0x04000740 RID: 1856
		private string _developmentLevelText;

		// Token: 0x04000741 RID: 1857
		private int _level;

		// Token: 0x04000742 RID: 1858
		private int _maxLevel;

		// Token: 0x04000743 RID: 1859
		private int _developmentQueueIndex = -1;

		// Token: 0x04000744 RID: 1860
		private bool _canBuild;

		// Token: 0x04000745 RID: 1861
		private bool _isInQueue;

		// Token: 0x04000746 RID: 1862
		private HintViewModel _addRemoveHint;

		// Token: 0x04000747 RID: 1863
		private HintViewModel _setAsActiveHint;
	}
}
