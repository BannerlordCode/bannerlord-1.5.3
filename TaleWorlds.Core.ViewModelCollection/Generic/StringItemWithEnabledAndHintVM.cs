using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.Core.ViewModelCollection.Generic
{
	// Token: 0x02000028 RID: 40
	public class StringItemWithEnabledAndHintVM : ViewModel
	{
		// Token: 0x060001BE RID: 446 RVA: 0x00005C91 File Offset: 0x00003E91
		public StringItemWithEnabledAndHintVM(Action<object> onExecute, string item, bool enabled, object identifier, TextObject hintText = null)
		{
			this._onExecute = onExecute;
			this.Identifier = identifier;
			this.ActionText = item;
			this.IsEnabled = enabled;
			this.Hint = new HintViewModel(hintText ?? TextObject.GetEmpty(), null);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00005CCD File Offset: 0x00003ECD
		public void ExecuteAction()
		{
			if (this.IsEnabled)
			{
				this._onExecute(this.Identifier);
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00005CE8 File Offset: 0x00003EE8
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x00005CF0 File Offset: 0x00003EF0
		[DataSourceProperty]
		public string ActionText
		{
			get
			{
				return this._actionText;
			}
			set
			{
				if (value != this._actionText)
				{
					this._actionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ActionText");
				}
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x00005D13 File Offset: 0x00003F13
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x00005D1B File Offset: 0x00003F1B
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

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x00005D39 File Offset: 0x00003F39
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x00005D41 File Offset: 0x00003F41
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

		// Token: 0x040000B5 RID: 181
		public object Identifier;

		// Token: 0x040000B6 RID: 182
		protected Action<object> _onExecute;

		// Token: 0x040000B7 RID: 183
		private HintViewModel _hint;

		// Token: 0x040000B8 RID: 184
		private string _actionText;

		// Token: 0x040000B9 RID: 185
		private bool _isEnabled;
	}
}
