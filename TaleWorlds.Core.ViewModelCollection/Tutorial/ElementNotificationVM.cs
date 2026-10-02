using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.Tutorial
{
	// Token: 0x0200000E RID: 14
	public class ElementNotificationVM : ViewModel
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00003458 File Offset: 0x00001658
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00003460 File Offset: 0x00001660
		[DataSourceProperty]
		public string ElementID
		{
			get
			{
				return this._elementID;
			}
			set
			{
				if (value != this._elementID)
				{
					this._elementID = value;
					base.OnPropertyChangedWithValue<string>(value, "ElementID");
				}
			}
		}

		// Token: 0x04000053 RID: 83
		private string _elementID = string.Empty;
	}
}
