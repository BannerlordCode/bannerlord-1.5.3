using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List
{
	// Token: 0x020000E1 RID: 225
	public class EncyclopediaFilterGroupVM : ViewModel
	{
		// Token: 0x06001550 RID: 5456 RVA: 0x00054CB8 File Offset: 0x00052EB8
		public EncyclopediaFilterGroupVM(EncyclopediaFilterGroup filterGroup, Action<EncyclopediaListFilterVM> UpdateFilters)
		{
			this.FilterGroup = filterGroup;
			this.Filters = new MBBindingList<EncyclopediaListFilterVM>();
			foreach (EncyclopediaFilterItem encyclopediaFilterItem in filterGroup.Filters)
			{
				this.Filters.Add(new EncyclopediaListFilterVM(encyclopediaFilterItem, UpdateFilters));
			}
			this.RefreshValues();
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x00054D34 File Offset: 0x00052F34
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Filters.ApplyActionOnAllItems(delegate(EncyclopediaListFilterVM x)
			{
				x.RefreshValues();
			});
			this.FilterName = this.FilterGroup.Name.ToString();
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x00054D88 File Offset: 0x00052F88
		public void CopyFiltersFrom(Dictionary<EncyclopediaFilterItem, bool> filters)
		{
			this.Filters.ApplyActionOnAllItems(delegate(EncyclopediaListFilterVM x)
			{
				x.CopyFilterFrom(filters);
			});
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06001553 RID: 5459 RVA: 0x00054DB9 File Offset: 0x00052FB9
		// (set) Token: 0x06001554 RID: 5460 RVA: 0x00054DC1 File Offset: 0x00052FC1
		[DataSourceProperty]
		public string FilterName
		{
			get
			{
				return this._filterName;
			}
			set
			{
				if (value != this._filterName)
				{
					this._filterName = value;
					base.OnPropertyChangedWithValue<string>(value, "FilterName");
				}
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06001555 RID: 5461 RVA: 0x00054DE4 File Offset: 0x00052FE4
		// (set) Token: 0x06001556 RID: 5462 RVA: 0x00054DEC File Offset: 0x00052FEC
		[DataSourceProperty]
		public MBBindingList<EncyclopediaListFilterVM> Filters
		{
			get
			{
				return this._filters;
			}
			set
			{
				if (value != this._filters)
				{
					this._filters = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaListFilterVM>>(value, "Filters");
				}
			}
		}

		// Token: 0x040009AB RID: 2475
		public readonly EncyclopediaFilterGroup FilterGroup;

		// Token: 0x040009AC RID: 2476
		private MBBindingList<EncyclopediaListFilterVM> _filters;

		// Token: 0x040009AD RID: 2477
		private string _filterName;
	}
}
