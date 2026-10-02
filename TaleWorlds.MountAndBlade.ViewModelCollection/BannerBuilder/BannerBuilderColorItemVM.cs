using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.BannerBuilder
{
	// Token: 0x02000085 RID: 133
	public class BannerBuilderColorItemVM : ViewModel
	{
		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x00027028 File Offset: 0x00025228
		// (set) Token: 0x06000AED RID: 2797 RVA: 0x00027030 File Offset: 0x00025230
		public int ColorID { get; private set; }

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x00027039 File Offset: 0x00025239
		// (set) Token: 0x06000AEF RID: 2799 RVA: 0x00027041 File Offset: 0x00025241
		public BannerColor BannerColor { get; private set; }

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0002704C File Offset: 0x0002524C
		public BannerBuilderColorItemVM(Action<BannerBuilderColorItemVM> onItemSelection, int key, BannerColor value)
		{
			this._onItemSelection = onItemSelection;
			this.ColorID = key;
			this.BannerColor = value;
			this.ColorAsStr = Color.FromUint(value.Color).ToString();
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x00027094 File Offset: 0x00025294
		public void ExecuteSelection()
		{
			Action<BannerBuilderColorItemVM> onItemSelection = this._onItemSelection;
			if (onItemSelection == null)
			{
				return;
			}
			onItemSelection(this);
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x000270A7 File Offset: 0x000252A7
		// (set) Token: 0x06000AF3 RID: 2803 RVA: 0x000270AF File Offset: 0x000252AF
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

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x000270CD File Offset: 0x000252CD
		// (set) Token: 0x06000AF5 RID: 2805 RVA: 0x000270D5 File Offset: 0x000252D5
		[DataSourceProperty]
		public string ColorAsStr
		{
			get
			{
				return this._colorAsStr;
			}
			set
			{
				if (value != this._colorAsStr)
				{
					this._colorAsStr = value;
					base.OnPropertyChangedWithValue<string>(value, "ColorAsStr");
				}
			}
		}

		// Token: 0x0400050C RID: 1292
		private readonly Action<BannerBuilderColorItemVM> _onItemSelection;

		// Token: 0x0400050F RID: 1295
		private bool _isSelected;

		// Token: 0x04000510 RID: 1296
		private string _colorAsStr;
	}
}
