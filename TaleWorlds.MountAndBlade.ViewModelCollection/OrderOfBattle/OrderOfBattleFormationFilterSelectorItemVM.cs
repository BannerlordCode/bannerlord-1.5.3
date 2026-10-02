using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle
{
	// Token: 0x02000030 RID: 48
	public class OrderOfBattleFormationFilterSelectorItemVM : ViewModel
	{
		// Token: 0x06000373 RID: 883 RVA: 0x0000C6BA File Offset: 0x0000A8BA
		public OrderOfBattleFormationFilterSelectorItemVM(FormationFilterType filterType, Action<OrderOfBattleFormationFilterSelectorItemVM> onToggled)
		{
			this.FilterType = filterType;
			this.FilterTypeValue = (int)filterType;
			this._onToggled = onToggled;
			this.RefreshValues();
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000C6DD File Offset: 0x0000A8DD
		public override void RefreshValues()
		{
			this.Hint = new HintViewModel(this.FilterType.GetFilterDescription(), null);
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000375 RID: 885 RVA: 0x0000C6F6 File Offset: 0x0000A8F6
		// (set) Token: 0x06000376 RID: 886 RVA: 0x0000C6FE File Offset: 0x0000A8FE
		[DataSourceProperty]
		public int FilterTypeValue
		{
			get
			{
				return this._filterType;
			}
			set
			{
				if (value != this._filterType)
				{
					this._filterType = value;
					base.OnPropertyChangedWithValue(value, "FilterTypeValue");
				}
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000377 RID: 887 RVA: 0x0000C71C File Offset: 0x0000A91C
		// (set) Token: 0x06000378 RID: 888 RVA: 0x0000C724 File Offset: 0x0000A924
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
					Action<OrderOfBattleFormationFilterSelectorItemVM> onToggled = this._onToggled;
					if (onToggled == null)
					{
						return;
					}
					onToggled(this);
				}
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000379 RID: 889 RVA: 0x0000C753 File Offset: 0x0000A953
		// (set) Token: 0x0600037A RID: 890 RVA: 0x0000C75B File Offset: 0x0000A95B
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

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600037B RID: 891 RVA: 0x0000C779 File Offset: 0x0000A979
		// (set) Token: 0x0600037C RID: 892 RVA: 0x0000C781 File Offset: 0x0000A981
		[DataSourceProperty]
		public HintViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x0400018B RID: 395
		public readonly FormationFilterType FilterType;

		// Token: 0x0400018C RID: 396
		private Action<OrderOfBattleFormationFilterSelectorItemVM> _onToggled;

		// Token: 0x0400018D RID: 397
		private int _filterType;

		// Token: 0x0400018E RID: 398
		private bool _isActive;

		// Token: 0x0400018F RID: 399
		private bool _isEnabled;

		// Token: 0x04000190 RID: 400
		private HintViewModel _hint;
	}
}
