using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List
{
	// Token: 0x020000E2 RID: 226
	public class EncyclopediaListFilterVM : ViewModel
	{
		// Token: 0x06001557 RID: 5463 RVA: 0x00054E0A File Offset: 0x0005300A
		public EncyclopediaListFilterVM(EncyclopediaFilterItem filter, Action<EncyclopediaListFilterVM> UpdateFilters)
		{
			this.Filter = filter;
			this._isSelected = this.Filter.IsActive;
			this._updateFilters = UpdateFilters;
			this.RefreshValues();
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x00054E37 File Offset: 0x00053037
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.Filter.Name.ToString();
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x00054E55 File Offset: 0x00053055
		public void CopyFilterFrom(Dictionary<EncyclopediaFilterItem, bool> filters)
		{
			if (filters.ContainsKey(this.Filter))
			{
				this.IsSelected = filters[this.Filter];
			}
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x00054E77 File Offset: 0x00053077
		public void ExecuteOnFilterActivated()
		{
			Game.Current.EventManager.TriggerEvent<OnEncyclopediaFilterActivatedEvent>(new OnEncyclopediaFilterActivatedEvent());
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x0600155B RID: 5467 RVA: 0x00054E8D File Offset: 0x0005308D
		// (set) Token: 0x0600155C RID: 5468 RVA: 0x00054E95 File Offset: 0x00053095
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
					this.Filter.IsActive = value;
					this._updateFilters(this);
				}
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x0600155D RID: 5469 RVA: 0x00054ECB File Offset: 0x000530CB
		// (set) Token: 0x0600155E RID: 5470 RVA: 0x00054ED3 File Offset: 0x000530D3
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

		// Token: 0x040009AE RID: 2478
		public readonly EncyclopediaFilterItem Filter;

		// Token: 0x040009AF RID: 2479
		private readonly Action<EncyclopediaListFilterVM> _updateFilters;

		// Token: 0x040009B0 RID: 2480
		private string _name;

		// Token: 0x040009B1 RID: 2481
		private bool _isSelected;
	}
}
