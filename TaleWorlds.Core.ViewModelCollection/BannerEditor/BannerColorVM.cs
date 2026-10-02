using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.BannerEditor
{
	// Token: 0x0200002C RID: 44
	public class BannerColorVM : ViewModel
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001DC RID: 476 RVA: 0x00005F51 File Offset: 0x00004151
		public int ColorID { get; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001DD RID: 477 RVA: 0x00005F59 File Offset: 0x00004159
		public uint Color { get; }

		// Token: 0x060001DE RID: 478 RVA: 0x00005F64 File Offset: 0x00004164
		public BannerColorVM(int colorID, uint color, Action<BannerColorVM> onSelection)
		{
			this.Color = color;
			this.ColorAsStr = TaleWorlds.Library.Color.FromUint(this.Color).ToString();
			this.ColorID = colorID;
			this._onSelection = onSelection;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00005FAB File Offset: 0x000041AB
		public void ExecuteSelectIcon()
		{
			this._onSelection(this);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00005FB9 File Offset: 0x000041B9
		public void SetOnSelectionAction(Action<BannerColorVM> onSelection)
		{
			this._onSelection = onSelection;
			this.IsSelected = false;
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x00005FC9 File Offset: 0x000041C9
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x00005FD1 File Offset: 0x000041D1
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

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x00005FF4 File Offset: 0x000041F4
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x00005FFC File Offset: 0x000041FC
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

		// Token: 0x040000C7 RID: 199
		private Action<BannerColorVM> _onSelection;

		// Token: 0x040000C8 RID: 200
		private string _colorAsStr;

		// Token: 0x040000C9 RID: 201
		private bool _isSelected;
	}
}
