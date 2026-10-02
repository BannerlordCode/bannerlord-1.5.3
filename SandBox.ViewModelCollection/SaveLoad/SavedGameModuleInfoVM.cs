using System;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.SaveLoad
{
	// Token: 0x02000014 RID: 20
	public class SavedGameModuleInfoVM : ViewModel
	{
		// Token: 0x060001A6 RID: 422 RVA: 0x00008286 File Offset: 0x00006486
		public SavedGameModuleInfoVM(string definition, string seperator, string value)
		{
			this.Definition = definition;
			this.Seperator = seperator;
			this.Value = value;
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x000082A3 File Offset: 0x000064A3
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x000082AB File Offset: 0x000064AB
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

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x000082CE File Offset: 0x000064CE
		// (set) Token: 0x060001AA RID: 426 RVA: 0x000082D6 File Offset: 0x000064D6
		[DataSourceProperty]
		public string Seperator
		{
			get
			{
				return this._seperator;
			}
			set
			{
				if (value != this._seperator)
				{
					this._seperator = value;
					base.OnPropertyChangedWithValue<string>(value, "Seperator");
				}
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001AB RID: 427 RVA: 0x000082F9 File Offset: 0x000064F9
		// (set) Token: 0x060001AC RID: 428 RVA: 0x00008301 File Offset: 0x00006501
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

		// Token: 0x040000B7 RID: 183
		private string _definition;

		// Token: 0x040000B8 RID: 184
		private string _seperator;

		// Token: 0x040000B9 RID: 185
		private string _value;
	}
}
