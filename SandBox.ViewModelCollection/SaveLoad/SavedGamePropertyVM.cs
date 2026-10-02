using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.SaveLoad
{
	// Token: 0x02000015 RID: 21
	public class SavedGamePropertyVM : ViewModel
	{
		// Token: 0x060001AD RID: 429 RVA: 0x00008324 File Offset: 0x00006524
		public SavedGamePropertyVM(SavedGamePropertyVM.SavedGameProperty type, TextObject value, TextObject hint)
		{
			this.PropertyType = type.ToString();
			this._valueText = value;
			this.Hint = new HintViewModel(hint, null);
			this.RefreshValues();
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00008359 File Offset: 0x00006559
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Value = this._valueText.ToString();
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00008372 File Offset: 0x00006572
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x0000837A File Offset: 0x0000657A
		[DataSourceProperty]
		public HintViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00008398 File Offset: 0x00006598
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x000083A0 File Offset: 0x000065A0
		[DataSourceProperty]
		public string PropertyType
		{
			get
			{
				return this._propertyType;
			}
			set
			{
				if (value != this._propertyType)
				{
					this._propertyType = value;
					base.OnPropertyChangedWithValue<string>(value, "PropertyType");
				}
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x000083C3 File Offset: 0x000065C3
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x000083CB File Offset: 0x000065CB
		[DataSourceProperty]
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (value != this._value)
				{
					this._value = value;
					base.OnPropertyChangedWithValue<string>(value, "Value");
				}
			}
		}

		// Token: 0x040000BA RID: 186
		private TextObject _valueText;

		// Token: 0x040000BB RID: 187
		private HintViewModel _hint;

		// Token: 0x040000BC RID: 188
		private string _propertyType;

		// Token: 0x040000BD RID: 189
		private string _value;

		// Token: 0x0200007E RID: 126
		public enum SavedGameProperty
		{
			// Token: 0x04000392 RID: 914
			None = -1,
			// Token: 0x04000393 RID: 915
			Health,
			// Token: 0x04000394 RID: 916
			Gold,
			// Token: 0x04000395 RID: 917
			Influence,
			// Token: 0x04000396 RID: 918
			PartySize,
			// Token: 0x04000397 RID: 919
			Food,
			// Token: 0x04000398 RID: 920
			Fiefs,
			// Token: 0x04000399 RID: 921
			Ships
		}
	}
}
