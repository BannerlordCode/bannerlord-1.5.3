using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.Generic
{
	// Token: 0x02000024 RID: 36
	public class BindingListFloatItem : ViewModel
	{
		// Token: 0x060001B0 RID: 432 RVA: 0x00005B71 File Offset: 0x00003D71
		public BindingListFloatItem(float value)
		{
			this.Item = value;
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00005B80 File Offset: 0x00003D80
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x00005B88 File Offset: 0x00003D88
		[DataSourceProperty]
		public float Item
		{
			get
			{
				return this._item;
			}
			set
			{
				if (value != this._item)
				{
					this._item = value;
					base.OnPropertyChangedWithValue(value, "Item");
				}
			}
		}

		// Token: 0x040000AD RID: 173
		private float _item;
	}
}
