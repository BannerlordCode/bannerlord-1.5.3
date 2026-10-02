using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.Generic
{
	// Token: 0x02000025 RID: 37
	public class BindingListStringItem : ViewModel
	{
		// Token: 0x060001B3 RID: 435 RVA: 0x00005BA6 File Offset: 0x00003DA6
		public BindingListStringItem(string value)
		{
			this.Item = value;
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00005BB5 File Offset: 0x00003DB5
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x00005BBD File Offset: 0x00003DBD
		[DataSourceProperty]
		public string Item
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
					base.OnPropertyChangedWithValue<string>(value, "Item");
				}
			}
		}

		// Token: 0x040000AE RID: 174
		private string _item;
	}
}
