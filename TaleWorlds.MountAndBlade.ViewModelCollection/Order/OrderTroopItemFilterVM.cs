using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x02000021 RID: 33
	public class OrderTroopItemFilterVM : ViewModel
	{
		// Token: 0x060002E9 RID: 745 RVA: 0x0000B2B7 File Offset: 0x000094B7
		public OrderTroopItemFilterVM(int filterTypeValue)
		{
			this.FilterTypeValue = filterTypeValue;
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000B2C6 File Offset: 0x000094C6
		// (set) Token: 0x060002EB RID: 747 RVA: 0x0000B2CE File Offset: 0x000094CE
		[DataSourceProperty]
		public int FilterTypeValue
		{
			get
			{
				return this._filterTypeValue;
			}
			set
			{
				if (value != this._filterTypeValue)
				{
					this._filterTypeValue = value;
					base.OnPropertyChangedWithValue(value, "FilterTypeValue");
				}
			}
		}

		// Token: 0x04000146 RID: 326
		private int _filterTypeValue;
	}
}
