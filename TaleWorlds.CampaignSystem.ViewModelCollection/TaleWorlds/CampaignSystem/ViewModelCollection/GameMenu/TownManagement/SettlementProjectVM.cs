using System;
using Helpers;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement
{
	// Token: 0x020000AD RID: 173
	public abstract class SettlementProjectVM : ViewModel
	{
		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001043 RID: 4163 RVA: 0x00042F2B File Offset: 0x0004112B
		// (set) Token: 0x06001044 RID: 4164 RVA: 0x00042F33 File Offset: 0x00041133
		public bool IsDaily { get; protected set; }

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001045 RID: 4165 RVA: 0x00042F3C File Offset: 0x0004113C
		// (set) Token: 0x06001046 RID: 4166 RVA: 0x00042F44 File Offset: 0x00041144
		public Building Building
		{
			get
			{
				return this._building;
			}
			set
			{
				this._building = value;
				this.Name = ((value != null) ? value.Name.ToString() : "");
				this.Explanation = ((value != null) ? value.Explanation.ToString() : "");
				this.VisualCode = ((value != null) ? value.BuildingType.StringId.ToLower() : "");
				int constructionCost = this.Building.GetConstructionCost();
				TextObject textObject;
				if (constructionCost > 0)
				{
					textObject = new TextObject("{=tAwRIPiy}Construction Cost: {COST}", null);
					textObject.SetTextVariable("COST", constructionCost);
				}
				else
				{
					textObject = TextObject.GetEmpty();
				}
				this.ProductionCostText = ((value != null) ? textObject.ToString() : "");
				this.CurrentPositiveEffectText = ((value != null) ? value.GetBonusExplanation().ToString() : "");
			}
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x00043010 File Offset: 0x00041210
		protected SettlementProjectVM(Action<SettlementProjectVM, bool> onSelection, Action<SettlementProjectVM> onSetAsCurrent, Action onResetCurrent, Building building, Settlement settlement)
		{
			this._onSelection = onSelection;
			this._onSetAsCurrent = onSetAsCurrent;
			this._onResetCurrent = onResetCurrent;
			this.Building = building;
			this._settlement = settlement;
			this.Progress = (int)(BuildingHelper.GetProgressOfBuilding(building, this._settlement.Town) * 100f);
			this.RefreshValues();
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x000430A0 File Offset: 0x000412A0
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this.Building.BuildingType.IsDailyProject)
			{
				this.CurrentPositiveEffectText = this.Building.BuildingType.GetExplanationAtLevel(this.Building.CurrentLevel).ToString();
				this.NextPositiveEffectText = "";
				return;
			}
			this.CurrentPositiveEffectText = this.GetBonusText(this.Building, this.Building.CurrentLevel);
			this.NextPositiveEffectText = this.GetBonusText(this.Building, this.Building.CurrentLevel + 1);
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x00043134 File Offset: 0x00041334
		private string GetBonusText(Building building, int level)
		{
			if (level == 0 || level == 4)
			{
				return "";
			}
			object obj = ((level == 1) ? this.L1BonusText : ((level == 2) ? this.L2BonusText : this.L3BonusText));
			TextObject bonusExplanationOfLevel = this.GetBonusExplanationOfLevel(level);
			object obj2 = obj;
			obj2.SetTextVariable("BONUS", bonusExplanationOfLevel);
			return obj2.ToString();
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x00043186 File Offset: 0x00041386
		private void ExecuteShowTooltip()
		{
			InformationManager.ShowTooltip(typeof(Building), new object[] { this._building });
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x000431A6 File Offset: 0x000413A6
		private void ExecuteHideTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x000431AD File Offset: 0x000413AD
		private TextObject GetBonusExplanationOfLevel(int level)
		{
			if (level >= 0 && level <= 3)
			{
				return this.Building.BuildingType.GetExplanationAtLevel(level);
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x000431CE File Offset: 0x000413CE
		public virtual void RefreshProductionText()
		{
		}

		// Token: 0x0600104E RID: 4174
		public abstract void ExecuteAddRemoveToQueue();

		// Token: 0x0600104F RID: 4175
		public abstract void ExecuteSetAsActiveDevelopment();

		// Token: 0x06001050 RID: 4176
		public abstract void ExecuteSetAsCurrent();

		// Token: 0x06001051 RID: 4177
		public abstract void ExecuteResetCurrent();

		// Token: 0x06001052 RID: 4178
		public abstract void ExecuteToggleSelected();

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001053 RID: 4179 RVA: 0x000431D0 File Offset: 0x000413D0
		// (set) Token: 0x06001054 RID: 4180 RVA: 0x000431D8 File Offset: 0x000413D8
		[DataSourceProperty]
		public string VisualCode
		{
			get
			{
				return this._visualCode;
			}
			set
			{
				if (value != this._visualCode)
				{
					this._visualCode = value;
					base.OnPropertyChangedWithValue<string>(value, "VisualCode");
				}
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001055 RID: 4181 RVA: 0x000431FB File Offset: 0x000413FB
		// (set) Token: 0x06001056 RID: 4182 RVA: 0x00043203 File Offset: 0x00041403
		[DataSourceProperty]
		public string ProductionText
		{
			get
			{
				return this._productionText;
			}
			set
			{
				if (value != this._productionText)
				{
					this._productionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProductionText");
				}
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001057 RID: 4183 RVA: 0x00043226 File Offset: 0x00041426
		// (set) Token: 0x06001058 RID: 4184 RVA: 0x0004322E File Offset: 0x0004142E
		[DataSourceProperty]
		public string CurrentPositiveEffectText
		{
			get
			{
				return this._currentPositiveEffectText;
			}
			set
			{
				if (value != this._currentPositiveEffectText)
				{
					this._currentPositiveEffectText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentPositiveEffectText");
				}
			}
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001059 RID: 4185 RVA: 0x00043251 File Offset: 0x00041451
		// (set) Token: 0x0600105A RID: 4186 RVA: 0x00043259 File Offset: 0x00041459
		[DataSourceProperty]
		public string NextPositiveEffectText
		{
			get
			{
				return this._nextPositiveEffectText;
			}
			set
			{
				if (value != this._nextPositiveEffectText)
				{
					this._nextPositiveEffectText = value;
					base.OnPropertyChangedWithValue<string>(value, "NextPositiveEffectText");
				}
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x0600105B RID: 4187 RVA: 0x0004327C File Offset: 0x0004147C
		// (set) Token: 0x0600105C RID: 4188 RVA: 0x00043284 File Offset: 0x00041484
		[DataSourceProperty]
		public string ProductionCostText
		{
			get
			{
				return this._productionCostText;
			}
			set
			{
				if (value != this._productionCostText)
				{
					this._productionCostText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProductionCostText");
				}
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x0600105D RID: 4189 RVA: 0x000432A7 File Offset: 0x000414A7
		// (set) Token: 0x0600105E RID: 4190 RVA: 0x000432AF File Offset: 0x000414AF
		[DataSourceProperty]
		public bool IsCurrentActiveProject
		{
			get
			{
				return this._isCurrentActiveProject;
			}
			set
			{
				if (value != this._isCurrentActiveProject)
				{
					this._isCurrentActiveProject = value;
					base.OnPropertyChangedWithValue(value, "IsCurrentActiveProject");
				}
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x0600105F RID: 4191 RVA: 0x000432CD File Offset: 0x000414CD
		// (set) Token: 0x06001060 RID: 4192 RVA: 0x000432D5 File Offset: 0x000414D5
		[DataSourceProperty]
		public int Progress
		{
			get
			{
				return this._progress;
			}
			set
			{
				if (value != this._progress)
				{
					this._progress = value;
					base.OnPropertyChangedWithValue(value, "Progress");
				}
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06001061 RID: 4193 RVA: 0x000432F3 File Offset: 0x000414F3
		// (set) Token: 0x06001062 RID: 4194 RVA: 0x000432FB File Offset: 0x000414FB
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

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06001063 RID: 4195 RVA: 0x0004331E File Offset: 0x0004151E
		// (set) Token: 0x06001064 RID: 4196 RVA: 0x00043326 File Offset: 0x00041526
		[DataSourceProperty]
		public string Explanation
		{
			get
			{
				return this._explanation;
			}
			set
			{
				if (value != this._explanation)
				{
					this._explanation = value;
					base.OnPropertyChangedWithValue<string>(value, "Explanation");
				}
			}
		}

		// Token: 0x04000760 RID: 1888
		public int Index;

		// Token: 0x04000762 RID: 1890
		private Building _building;

		// Token: 0x04000763 RID: 1891
		protected Action<SettlementProjectVM, bool> _onSelection;

		// Token: 0x04000764 RID: 1892
		protected Action<SettlementProjectVM> _onSetAsCurrent;

		// Token: 0x04000765 RID: 1893
		protected Action _onResetCurrent;

		// Token: 0x04000766 RID: 1894
		protected Settlement _settlement;

		// Token: 0x04000767 RID: 1895
		private readonly TextObject L1BonusText = new TextObject("{=PJZ8QYgA}L-I : {BONUS}", null);

		// Token: 0x04000768 RID: 1896
		private readonly TextObject L2BonusText = new TextObject("{=9i0wnjJK}L-II : {BONUS}", null);

		// Token: 0x04000769 RID: 1897
		private readonly TextObject L3BonusText = new TextObject("{=pRP2sOWP}L-III : {BONUS}", null);

		// Token: 0x0400076A RID: 1898
		private string _name;

		// Token: 0x0400076B RID: 1899
		private string _visualCode;

		// Token: 0x0400076C RID: 1900
		private string _explanation;

		// Token: 0x0400076D RID: 1901
		private string _currentPositiveEffectText;

		// Token: 0x0400076E RID: 1902
		private string _nextPositiveEffectText;

		// Token: 0x0400076F RID: 1903
		private string _productionCostText;

		// Token: 0x04000770 RID: 1904
		private int _progress;

		// Token: 0x04000771 RID: 1905
		private bool _isCurrentActiveProject;

		// Token: 0x04000772 RID: 1906
		private string _productionText;
	}
}
