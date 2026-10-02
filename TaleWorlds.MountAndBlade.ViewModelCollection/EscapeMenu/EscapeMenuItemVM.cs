using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu
{
	// Token: 0x0200007F RID: 127
	public class EscapeMenuItemVM : ViewModel
	{
		// Token: 0x06000AA1 RID: 2721 RVA: 0x00026167 File Offset: 0x00024367
		public EscapeMenuItemVM(TextObject item, Action<object> onExecute, object identifier, Func<Tuple<bool, TextObject>> getIsDisabledAndReason, bool isPositiveBehaviored = false)
		{
			this._onExecute = onExecute;
			this._identifier = identifier;
			this._itemObj = item;
			this.ActionText = this._itemObj.ToString();
			this.IsPositiveBehaviored = isPositiveBehaviored;
			this._getIsDisabledAndReason = getIsDisabledAndReason;
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x000261A8 File Offset: 0x000243A8
		public override void RefreshValues()
		{
			base.RefreshValues();
			Func<Tuple<bool, TextObject>> getIsDisabledAndReason = this._getIsDisabledAndReason;
			Tuple<bool, TextObject> tuple = ((getIsDisabledAndReason != null) ? getIsDisabledAndReason() : null);
			this.IsDisabled = tuple.Item1;
			this.DisabledHint = new HintViewModel(tuple.Item2, null);
			this.ActionText = this._itemObj.ToString();
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x000261FD File Offset: 0x000243FD
		public void ExecuteAction()
		{
			this._onExecute(this._identifier);
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x00026210 File Offset: 0x00024410
		// (set) Token: 0x06000AA5 RID: 2725 RVA: 0x00026218 File Offset: 0x00024418
		[DataSourceProperty]
		public HintViewModel DisabledHint
		{
			get
			{
				return this._disabledHint;
			}
			set
			{
				if (value != this._disabledHint)
				{
					this._disabledHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DisabledHint");
				}
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x00026236 File Offset: 0x00024436
		// (set) Token: 0x06000AA7 RID: 2727 RVA: 0x0002623E File Offset: 0x0002443E
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

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x00026261 File Offset: 0x00024461
		// (set) Token: 0x06000AA9 RID: 2729 RVA: 0x00026269 File Offset: 0x00024469
		[DataSourceProperty]
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
			set
			{
				if (value != this._isDisabled)
				{
					this._isDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsDisabled");
				}
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000AAA RID: 2730 RVA: 0x00026287 File Offset: 0x00024487
		// (set) Token: 0x06000AAB RID: 2731 RVA: 0x0002628F File Offset: 0x0002448F
		[DataSourceProperty]
		public bool IsPositiveBehaviored
		{
			get
			{
				return this._isPositiveBehaviored;
			}
			set
			{
				if (value != this._isPositiveBehaviored)
				{
					this._isPositiveBehaviored = value;
					base.OnPropertyChangedWithValue(value, "IsPositiveBehaviored");
				}
			}
		}

		// Token: 0x040004E4 RID: 1252
		private readonly object _identifier;

		// Token: 0x040004E5 RID: 1253
		private readonly Action<object> _onExecute;

		// Token: 0x040004E6 RID: 1254
		private readonly TextObject _itemObj;

		// Token: 0x040004E7 RID: 1255
		private readonly Func<Tuple<bool, TextObject>> _getIsDisabledAndReason;

		// Token: 0x040004E8 RID: 1256
		private HintViewModel _disabledHint;

		// Token: 0x040004E9 RID: 1257
		private string _actionText;

		// Token: 0x040004EA RID: 1258
		private bool _isDisabled;

		// Token: 0x040004EB RID: 1259
		private bool _isPositiveBehaviored;
	}
}
