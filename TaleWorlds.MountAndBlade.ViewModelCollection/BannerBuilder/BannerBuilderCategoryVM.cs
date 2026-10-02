using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.BannerBuilder
{
	// Token: 0x02000084 RID: 132
	public class BannerBuilderCategoryVM : ViewModel
	{
		// Token: 0x06000AE1 RID: 2785 RVA: 0x00026E58 File Offset: 0x00025058
		public BannerBuilderCategoryVM(BannerIconGroup category, Action<BannerBuilderItemVM> onItemSelection)
		{
			this.ItemsList = new MBBindingList<BannerBuilderItemVM>();
			this._category = category;
			this._onItemSelection = onItemSelection;
			this.IsPattern = this._category.IsPattern;
			this.IsEnabled = true;
			this.PopulateItems();
			this.RefreshValues();
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x00026EA8 File Offset: 0x000250A8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Title = this._category.Name.ToString();
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00026EC8 File Offset: 0x000250C8
		private void PopulateItems()
		{
			this.ItemsList.Clear();
			if (this.IsPattern)
			{
				for (int i = 0; i < this._category.AllBackgrounds.Count; i++)
				{
					KeyValuePair<int, string> keyValuePair = this._category.AllBackgrounds.ElementAt<KeyValuePair<int, string>>(i);
					this.ItemsList.Add(new BannerBuilderItemVM(keyValuePair.Key, keyValuePair.Value, this._onItemSelection));
				}
				return;
			}
			for (int j = 0; j < this._category.AllIcons.Count; j++)
			{
				KeyValuePair<int, BannerIconData> keyValuePair2 = this._category.AllIcons.ElementAt<KeyValuePair<int, BannerIconData>>(j);
				this.ItemsList.Add(new BannerBuilderItemVM(keyValuePair2.Key, keyValuePair2.Value, this._onItemSelection));
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x00026F8B File Offset: 0x0002518B
		// (set) Token: 0x06000AE5 RID: 2789 RVA: 0x00026F93 File Offset: 0x00025193
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x00026FB6 File Offset: 0x000251B6
		// (set) Token: 0x06000AE7 RID: 2791 RVA: 0x00026FBE File Offset: 0x000251BE
		[DataSourceProperty]
		public bool IsPattern
		{
			get
			{
				return this._isPattern;
			}
			set
			{
				if (value != this._isPattern)
				{
					this._isPattern = value;
					base.OnPropertyChangedWithValue(value, "IsPattern");
				}
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x00026FDC File Offset: 0x000251DC
		// (set) Token: 0x06000AE9 RID: 2793 RVA: 0x00026FE4 File Offset: 0x000251E4
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

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000AEA RID: 2794 RVA: 0x00027002 File Offset: 0x00025202
		// (set) Token: 0x06000AEB RID: 2795 RVA: 0x0002700A File Offset: 0x0002520A
		[DataSourceProperty]
		public MBBindingList<BannerBuilderItemVM> ItemsList
		{
			get
			{
				return this._itemsList;
			}
			set
			{
				if (value != this._itemsList)
				{
					this._itemsList = value;
					base.OnPropertyChangedWithValue<MBBindingList<BannerBuilderItemVM>>(value, "ItemsList");
				}
			}
		}

		// Token: 0x04000506 RID: 1286
		private readonly BannerIconGroup _category;

		// Token: 0x04000507 RID: 1287
		private readonly Action<BannerBuilderItemVM> _onItemSelection;

		// Token: 0x04000508 RID: 1288
		private string _title;

		// Token: 0x04000509 RID: 1289
		private bool _isPattern;

		// Token: 0x0400050A RID: 1290
		private bool _isEnabled;

		// Token: 0x0400050B RID: 1291
		private MBBindingList<BannerBuilderItemVM> _itemsList;
	}
}
