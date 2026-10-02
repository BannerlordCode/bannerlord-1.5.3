using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.Generic
{
	// Token: 0x0200002B RID: 43
	public class StringPairItemWithActionVM : ViewModel
	{
		// Token: 0x060001D2 RID: 466 RVA: 0x00005E65 File Offset: 0x00004065
		public StringPairItemWithActionVM(Action<object> onExecute, string definition, string value, object identifier)
		{
			this._onExecute = onExecute;
			this.Identifier = identifier;
			this.Definition = definition;
			this.Value = value;
			this.Hint = new HintViewModel();
			this.IsEnabled = true;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00005E9C File Offset: 0x0000409C
		public void ExecuteAction()
		{
			this._onExecute(this.Identifier);
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x00005EAF File Offset: 0x000040AF
		// (set) Token: 0x060001D5 RID: 469 RVA: 0x00005EB7 File Offset: 0x000040B7
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

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x00005EDA File Offset: 0x000040DA
		// (set) Token: 0x060001D7 RID: 471 RVA: 0x00005EE2 File Offset: 0x000040E2
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

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x00005F05 File Offset: 0x00004105
		// (set) Token: 0x060001D9 RID: 473 RVA: 0x00005F0D File Offset: 0x0000410D
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

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001DA RID: 474 RVA: 0x00005F2B File Offset: 0x0000412B
		// (set) Token: 0x060001DB RID: 475 RVA: 0x00005F33 File Offset: 0x00004133
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x040000BF RID: 191
		public object Identifier;

		// Token: 0x040000C0 RID: 192
		protected Action<object> _onExecute;

		// Token: 0x040000C1 RID: 193
		private string _definition;

		// Token: 0x040000C2 RID: 194
		private string _value;

		// Token: 0x040000C3 RID: 195
		private HintViewModel _hint;

		// Token: 0x040000C4 RID: 196
		private bool _isEnabled;
	}
}
