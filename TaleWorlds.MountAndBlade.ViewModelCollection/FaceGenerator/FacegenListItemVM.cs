using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.FaceGenerator
{
	// Token: 0x0200007C RID: 124
	public class FacegenListItemVM : ViewModel
	{
		// Token: 0x060009C2 RID: 2498 RVA: 0x00021BDA File Offset: 0x0001FDDA
		public void ExecuteAction()
		{
			this._setSelected(this, true);
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x00021BE9 File Offset: 0x0001FDE9
		public FacegenListItemVM(string imagePath, int index, Action<FacegenListItemVM, bool> setSelected)
		{
			this.ImagePath = imagePath;
			this.Index = index;
			this.IsSelected = false;
			this._setSelected = setSelected;
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x060009C4 RID: 2500 RVA: 0x00021C1B File Offset: 0x0001FE1B
		// (set) Token: 0x060009C5 RID: 2501 RVA: 0x00021C23 File Offset: 0x0001FE23
		[DataSourceProperty]
		public string ImagePath
		{
			get
			{
				return this._imagePath;
			}
			set
			{
				if (value != this._imagePath)
				{
					this._imagePath = value;
					base.OnPropertyChangedWithValue<string>(value, "ImagePath");
				}
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x060009C6 RID: 2502 RVA: 0x00021C46 File Offset: 0x0001FE46
		// (set) Token: 0x060009C7 RID: 2503 RVA: 0x00021C4E File Offset: 0x0001FE4E
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

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x00021C6C File Offset: 0x0001FE6C
		// (set) Token: 0x060009C9 RID: 2505 RVA: 0x00021C74 File Offset: 0x0001FE74
		[DataSourceProperty]
		public int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				if (value != this._index)
				{
					this._index = value;
					base.OnPropertyChangedWithValue(value, "Index");
				}
			}
		}

		// Token: 0x0400045F RID: 1119
		private readonly Action<FacegenListItemVM, bool> _setSelected;

		// Token: 0x04000460 RID: 1120
		private string _imagePath;

		// Token: 0x04000461 RID: 1121
		private bool _isSelected = true;

		// Token: 0x04000462 RID: 1122
		private int _index = -1;
	}
}
