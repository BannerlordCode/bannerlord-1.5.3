using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.BannerBuilder
{
	// Token: 0x02000086 RID: 134
	public class BannerBuilderColorSelectionVM : ViewModel
	{
		// Token: 0x06000AF6 RID: 2806 RVA: 0x000270F8 File Offset: 0x000252F8
		public BannerBuilderColorSelectionVM()
		{
			this.Items = new MBBindingList<BannerBuilderColorItemVM>();
			this.PopulateItems();
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x00027114 File Offset: 0x00025314
		public void EnableWith(int selectedColorID, Action<BannerBuilderColorItemVM> onSelection)
		{
			this._onSelection = onSelection;
			this.Items.ApplyActionOnAllItems(delegate(BannerBuilderColorItemVM i)
			{
				i.IsSelected = i.ColorID == selectedColorID;
			});
			this.IsEnabled = true;
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x00027153 File Offset: 0x00025353
		private void OnItemSelection(BannerBuilderColorItemVM item)
		{
			Action<BannerBuilderColorItemVM> onSelection = this._onSelection;
			if (onSelection != null)
			{
				onSelection(item);
			}
			this._onSelection = null;
			this.IsEnabled = false;
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x00027178 File Offset: 0x00025378
		private void PopulateItems()
		{
			this.Items.Clear();
			MBReadOnlyDictionary<int, BannerColor> readOnlyColorPalette = BannerManager.Instance.ReadOnlyColorPalette;
			for (int i = 0; i < readOnlyColorPalette.Count; i++)
			{
				KeyValuePair<int, BannerColor> keyValuePair = readOnlyColorPalette.ElementAt<KeyValuePair<int, BannerColor>>(i);
				this.Items.Add(new BannerBuilderColorItemVM(new Action<BannerBuilderColorItemVM>(this.OnItemSelection), keyValuePair.Key, keyValuePair.Value));
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000AFA RID: 2810 RVA: 0x000271DE File Offset: 0x000253DE
		// (set) Token: 0x06000AFB RID: 2811 RVA: 0x000271E6 File Offset: 0x000253E6
		[DataSourceProperty]
		public MBBindingList<BannerBuilderColorItemVM> Items
		{
			get
			{
				return this._items;
			}
			set
			{
				if (value != this._items)
				{
					this._items = value;
					base.OnPropertyChangedWithValue<MBBindingList<BannerBuilderColorItemVM>>(value, "Items");
				}
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000AFC RID: 2812 RVA: 0x00027204 File Offset: 0x00025404
		// (set) Token: 0x06000AFD RID: 2813 RVA: 0x0002720C File Offset: 0x0002540C
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

		// Token: 0x04000511 RID: 1297
		private Action<BannerBuilderColorItemVM> _onSelection;

		// Token: 0x04000512 RID: 1298
		private MBBindingList<BannerBuilderColorItemVM> _items;

		// Token: 0x04000513 RID: 1299
		private bool _isEnabled;
	}
}
