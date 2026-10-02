using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x0200001D RID: 29
	public abstract class OrderItemBaseVM : ViewModel
	{
		// Token: 0x0600029D RID: 669 RVA: 0x0000AA3B File Offset: 0x00008C3B
		public OrderItemBaseVM(OrderController orderController)
		{
			this._orderController = orderController;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000AA4A File Offset: 0x00008C4A
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM shortcutKey = this.ShortcutKey;
			if (shortcutKey == null)
			{
				return;
			}
			shortcutKey.OnFinalize();
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000AA62 File Offset: 0x00008C62
		public void RefreshState()
		{
			this.OnRefreshState();
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000AA6A File Offset: 0x00008C6A
		public void ExecuteAction(VisualOrderExecutionParameters executionParameters)
		{
			this.OnExecuteAction(executionParameters);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000AA73 File Offset: 0x00008C73
		protected virtual void OnSelectedStateChanged(bool isSelected)
		{
		}

		// Token: 0x060002A2 RID: 674
		protected abstract void OnRefreshState();

		// Token: 0x060002A3 RID: 675
		protected abstract void OnExecuteAction(VisualOrderExecutionParameters executionParameters);

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000AA75 File Offset: 0x00008C75
		// (set) Token: 0x060002A5 RID: 677 RVA: 0x0000AA7D File Offset: 0x00008C7D
		[DataSourceProperty]
		public InputKeyItemVM ShortcutKey
		{
			get
			{
				return this._shortcutKey;
			}
			set
			{
				if (value != this._shortcutKey)
				{
					this._shortcutKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ShortcutKey");
				}
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000AA9B File Offset: 0x00008C9B
		public void SetShortcutKey(InputKeyItemVM inputKeyItem)
		{
			this.ShortcutKey = inputKeyItem;
			InputKeyItemVM shortcutKey = this.ShortcutKey;
			if (shortcutKey == null)
			{
				return;
			}
			shortcutKey.RefreshValues();
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x0000AAB4 File Offset: 0x00008CB4
		// (set) Token: 0x060002A8 RID: 680 RVA: 0x0000AABC File Offset: 0x00008CBC
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				this._isActive = value;
				base.OnPropertyChangedWithValue(value, "IsActive");
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000AAD1 File Offset: 0x00008CD1
		// (set) Token: 0x060002AA RID: 682 RVA: 0x0000AAD9 File Offset: 0x00008CD9
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				this._isSelected = value;
				base.OnPropertyChangedWithValue(value, "IsSelected");
				this.OnSelectedStateChanged(value);
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000AAF5 File Offset: 0x00008CF5
		// (set) Token: 0x060002AC RID: 684 RVA: 0x0000AAFD File Offset: 0x00008CFD
		[DataSourceProperty]
		public bool CanUseShortcuts
		{
			get
			{
				return this._canUseShortcuts;
			}
			set
			{
				if (value != this._canUseShortcuts)
				{
					this._canUseShortcuts = value;
					base.OnPropertyChangedWithValue(value, "CanUseShortcuts");
				}
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000AB1B File Offset: 0x00008D1B
		// (set) Token: 0x060002AE RID: 686 RVA: 0x0000AB23 File Offset: 0x00008D23
		[DataSourceProperty]
		public string OrderIconId
		{
			get
			{
				return this._orderIconId;
			}
			set
			{
				if (value != this._orderIconId)
				{
					this._orderIconId = value;
					base.OnPropertyChangedWithValue<string>(value, "OrderIconId");
				}
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000AB46 File Offset: 0x00008D46
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x0000AB4E File Offset: 0x00008D4E
		[DataSourceProperty]
		public string SelectionState
		{
			get
			{
				return this._selectionState;
			}
			set
			{
				if (value != this._selectionState)
				{
					this._selectionState = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectionState");
				}
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000AB71 File Offset: 0x00008D71
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x0000AB79 File Offset: 0x00008D79
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x0400012D RID: 301
		protected OrderController _orderController;

		// Token: 0x0400012E RID: 302
		private InputKeyItemVM _shortcutKey;

		// Token: 0x0400012F RID: 303
		private bool _isActive;

		// Token: 0x04000130 RID: 304
		private bool _isSelected;

		// Token: 0x04000131 RID: 305
		private bool _canUseShortcuts;

		// Token: 0x04000132 RID: 306
		private string _orderIconId;

		// Token: 0x04000133 RID: 307
		private string _selectionState;

		// Token: 0x04000134 RID: 308
		private string _name;
	}
}
