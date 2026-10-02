using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.BannerBuilder
{
	// Token: 0x02000087 RID: 135
	public class BannerBuilderItemVM : ViewModel
	{
		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000AFE RID: 2814 RVA: 0x0002722A File Offset: 0x0002542A
		// (set) Token: 0x06000AFF RID: 2815 RVA: 0x00027232 File Offset: 0x00025432
		public BannerIconData IconData { get; private set; }

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x0002723B File Offset: 0x0002543B
		// (set) Token: 0x06000B01 RID: 2817 RVA: 0x00027243 File Offset: 0x00025443
		public string BackgroundTextureID { get; private set; }

		// Token: 0x06000B02 RID: 2818 RVA: 0x0002724C File Offset: 0x0002544C
		public BannerBuilderItemVM(int key, BannerIconData iconData, Action<BannerBuilderItemVM> onItemSelection)
		{
			this.MeshID = key;
			this.IconData = iconData;
			this._onItemSelection = onItemSelection;
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x00027269 File Offset: 0x00025469
		public BannerBuilderItemVM(int key, string backgroundTextureID, Action<BannerBuilderItemVM> onItemSelection)
		{
			this.MeshID = key;
			this.BackgroundTextureID = backgroundTextureID;
			this._onItemSelection = onItemSelection;
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x00027286 File Offset: 0x00025486
		public void ExecuteSelection()
		{
			Action<BannerBuilderItemVM> onItemSelection = this._onItemSelection;
			if (onItemSelection == null)
			{
				return;
			}
			onItemSelection(this);
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x00027299 File Offset: 0x00025499
		// (set) Token: 0x06000B06 RID: 2822 RVA: 0x000272A1 File Offset: 0x000254A1
		[DataSourceProperty]
		public int MeshID
		{
			get
			{
				return this._meshID;
			}
			set
			{
				if (value != this._meshID)
				{
					this._meshID = value;
					base.OnPropertyChangedWithValue(value, "MeshID");
					this.MeshIDAsString = this._meshID.ToString();
				}
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000B07 RID: 2823 RVA: 0x000272D0 File Offset: 0x000254D0
		// (set) Token: 0x06000B08 RID: 2824 RVA: 0x000272D8 File Offset: 0x000254D8
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

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000B09 RID: 2825 RVA: 0x000272F6 File Offset: 0x000254F6
		// (set) Token: 0x06000B0A RID: 2826 RVA: 0x000272FE File Offset: 0x000254FE
		[DataSourceProperty]
		public string MeshIDAsString
		{
			get
			{
				return this._meshIDAsString;
			}
			set
			{
				if (value != this._meshIDAsString)
				{
					this._meshIDAsString = value;
					base.OnPropertyChangedWithValue<string>(value, "MeshIDAsString");
				}
			}
		}

		// Token: 0x04000516 RID: 1302
		private readonly Action<BannerBuilderItemVM> _onItemSelection;

		// Token: 0x04000517 RID: 1303
		public int _meshID;

		// Token: 0x04000518 RID: 1304
		public string _meshIDAsString;

		// Token: 0x04000519 RID: 1305
		public bool _isSelected;
	}
}
