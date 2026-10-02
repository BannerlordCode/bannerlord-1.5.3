using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.Generic
{
	// Token: 0x0200002A RID: 42
	public class StringPairItemVM : ViewModel
	{
		// Token: 0x060001CB RID: 459 RVA: 0x00005DCC File Offset: 0x00003FCC
		public StringPairItemVM(string definition, string value, BasicTooltipViewModel hint = null)
		{
			this.Definition = definition;
			this.Value = value;
			this.Hint = hint;
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001CC RID: 460 RVA: 0x00005DE9 File Offset: 0x00003FE9
		// (set) Token: 0x060001CD RID: 461 RVA: 0x00005DF1 File Offset: 0x00003FF1
		[DataSourceProperty]
		public string Definition
		{
			get
			{
				return this._definition;
			}
			set
			{
				if (value != this._definition)
				{
					this._definition = value;
					base.OnPropertyChangedWithValue<string>(value, "Definition");
				}
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00005E14 File Offset: 0x00004014
		// (set) Token: 0x060001CF RID: 463 RVA: 0x00005E1C File Offset: 0x0000401C
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

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x00005E3F File Offset: 0x0000403F
		// (set) Token: 0x060001D1 RID: 465 RVA: 0x00005E47 File Offset: 0x00004047
		[DataSourceProperty]
		public BasicTooltipViewModel Hint
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
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x040000BC RID: 188
		private string _definition;

		// Token: 0x040000BD RID: 189
		private string _value;

		// Token: 0x040000BE RID: 190
		private BasicTooltipViewModel _hint;
	}
}
