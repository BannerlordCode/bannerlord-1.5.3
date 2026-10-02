using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement
{
	// Token: 0x020000A9 RID: 169
	public class SettlementDailyProjectVM : SettlementProjectVM
	{
		// Token: 0x06001007 RID: 4103 RVA: 0x00042323 File Offset: 0x00040523
		public SettlementDailyProjectVM(Action<SettlementProjectVM, bool> onSelection, Action<SettlementProjectVM> onSetAsCurrent, Action onResetCurrent, Building building, Settlement settlement)
			: base(onSelection, onSetAsCurrent, onResetCurrent, building, settlement)
		{
			base.IsDaily = true;
			this.RefreshValues();
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0004233F File Offset: 0x0004053F
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.DefaultText = GameTexts.FindText("str_default", null).ToString();
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0004235D File Offset: 0x0004055D
		public override void RefreshProductionText()
		{
			base.RefreshProductionText();
			base.ProductionText = new TextObject("{=bd7oAQq6}Daily", null).ToString();
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0004237B File Offset: 0x0004057B
		public override void ExecuteAddRemoveToQueue()
		{
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0004237D File Offset: 0x0004057D
		public override void ExecuteSetAsActiveDevelopment()
		{
			this._onSelection(this, false);
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0004238C File Offset: 0x0004058C
		public override void ExecuteSetAsCurrent()
		{
			Action<SettlementProjectVM> onSetAsCurrent = this._onSetAsCurrent;
			if (onSetAsCurrent == null)
			{
				return;
			}
			onSetAsCurrent(this);
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0004239F File Offset: 0x0004059F
		public override void ExecuteResetCurrent()
		{
			Action onResetCurrent = this._onResetCurrent;
			if (onResetCurrent == null)
			{
				return;
			}
			onResetCurrent();
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x000423B1 File Offset: 0x000405B1
		public override void ExecuteToggleSelected()
		{
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x0600100F RID: 4111 RVA: 0x000423B3 File Offset: 0x000405B3
		// (set) Token: 0x06001010 RID: 4112 RVA: 0x000423BB File Offset: 0x000405BB
		[DataSourceProperty]
		public bool IsDefault
		{
			get
			{
				return this._isDefault;
			}
			set
			{
				if (value != this._isDefault)
				{
					this._isDefault = value;
					base.OnPropertyChangedWithValue(value, "IsDefault");
				}
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06001011 RID: 4113 RVA: 0x000423D9 File Offset: 0x000405D9
		// (set) Token: 0x06001012 RID: 4114 RVA: 0x000423E1 File Offset: 0x000405E1
		[DataSourceProperty]
		public string DefaultText
		{
			get
			{
				return this._defaultText;
			}
			set
			{
				if (value != this._defaultText)
				{
					this._defaultText = value;
					base.OnPropertyChangedWithValue<string>(value, "DefaultText");
				}
			}
		}

		// Token: 0x04000748 RID: 1864
		private bool _isDefault;

		// Token: 0x04000749 RID: 1865
		private string _defaultText;
	}
}
