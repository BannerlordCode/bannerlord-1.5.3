using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x02000020 RID: 32
	public abstract class OrderSubjectVM : ViewModel
	{
		// Token: 0x060002CF RID: 719 RVA: 0x0000B0C3 File Offset: 0x000092C3
		public OrderSubjectVM()
		{
			this.ActiveOrders = new MBBindingList<OrderItemVM>();
			this.RefreshValues();
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000B0DC File Offset: 0x000092DC
		public void AddActiveOrder(OrderItemVM order)
		{
			this.ActiveOrders.Add(order);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000B0EA File Offset: 0x000092EA
		public void RemoveActiveOrder(OrderItemVM order)
		{
			this.ActiveOrders.Remove(order);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000B0F9 File Offset: 0x000092F9
		public void ClearActiveOrders()
		{
			this.ActiveOrders.Clear();
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000B106 File Offset: 0x00009306
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SelectionText = new TextObject("{=xbk1WAt6}Select", null).ToString();
		}

		// Token: 0x060002D4 RID: 724
		protected abstract void OnSelectionStateChanged(bool isSelected);

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x0000B124 File Offset: 0x00009324
		// (set) Token: 0x060002D6 RID: 726 RVA: 0x0000B12C File Offset: 0x0000932C
		[DataSourceProperty]
		public bool IsSelectable
		{
			get
			{
				return this._isSelectable;
			}
			set
			{
				if (value != this._isSelectable)
				{
					this._isSelectable = value;
					base.OnPropertyChangedWithValue(value, "IsSelectable");
				}
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000B14A File Offset: 0x0000934A
		// (set) Token: 0x060002D8 RID: 728 RVA: 0x0000B152 File Offset: 0x00009352
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if ((!value || this.IsSelectable) && value != this._isSelected)
				{
					this._isSelected = value;
					this.OnSelectionStateChanged(value);
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000B182 File Offset: 0x00009382
		// (set) Token: 0x060002DA RID: 730 RVA: 0x0000B18A File Offset: 0x0000938A
		[DataSourceProperty]
		public bool IsSelectionHighlightActive
		{
			get
			{
				return this._isSelectionHighlightActive;
			}
			set
			{
				if (value != this._isSelectionHighlightActive)
				{
					this._isSelectionHighlightActive = value;
					base.OnPropertyChangedWithValue(value, "IsSelectionHighlightActive");
				}
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000B1A8 File Offset: 0x000093A8
		// (set) Token: 0x060002DC RID: 732 RVA: 0x0000B1B0 File Offset: 0x000093B0
		[DataSourceProperty]
		public bool ShowSelectionInputs
		{
			get
			{
				return this._showSelectionInputs;
			}
			set
			{
				if (value != this._showSelectionInputs)
				{
					this._showSelectionInputs = value;
					base.OnPropertyChangedWithValue(value, "ShowSelectionInputs");
				}
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060002DD RID: 733 RVA: 0x0000B1CE File Offset: 0x000093CE
		// (set) Token: 0x060002DE RID: 734 RVA: 0x0000B1D6 File Offset: 0x000093D6
		[DataSourceProperty]
		public int BehaviorType
		{
			get
			{
				return this._behaviorType;
			}
			set
			{
				if (value != this._behaviorType)
				{
					this._behaviorType = value;
					base.OnPropertyChangedWithValue(value, "BehaviorType");
				}
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000B1F4 File Offset: 0x000093F4
		// (set) Token: 0x060002E0 RID: 736 RVA: 0x0000B1FC File Offset: 0x000093FC
		[DataSourceProperty]
		public int UnderAttackOfType
		{
			get
			{
				return this._underAttackOfType;
			}
			set
			{
				if (value != this._underAttackOfType)
				{
					this._underAttackOfType = value;
					base.OnPropertyChangedWithValue(value, "UnderAttackOfType");
				}
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000B21A File Offset: 0x0000941A
		// (set) Token: 0x060002E2 RID: 738 RVA: 0x0000B222 File Offset: 0x00009422
		[DataSourceProperty]
		public string SelectionText
		{
			get
			{
				return this._selectionText;
			}
			set
			{
				if (value != this._selectionText)
				{
					this._selectionText = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectionText");
				}
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x0000B245 File Offset: 0x00009445
		// (set) Token: 0x060002E4 RID: 740 RVA: 0x0000B24D File Offset: 0x0000944D
		[DataSourceProperty]
		public InputKeyItemVM ApplySelectionKey
		{
			get
			{
				return this._applySelectionKey;
			}
			set
			{
				if (value != this._applySelectionKey)
				{
					this._applySelectionKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ApplySelectionKey");
				}
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x0000B26B File Offset: 0x0000946B
		// (set) Token: 0x060002E6 RID: 742 RVA: 0x0000B273 File Offset: 0x00009473
		[DataSourceProperty]
		public InputKeyItemVM ToggleSelectionKey
		{
			get
			{
				return this._toggleSelectionKey;
			}
			set
			{
				if (value != this._toggleSelectionKey)
				{
					this._toggleSelectionKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ToggleSelectionKey");
				}
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x0000B291 File Offset: 0x00009491
		// (set) Token: 0x060002E8 RID: 744 RVA: 0x0000B299 File Offset: 0x00009499
		[DataSourceProperty]
		public MBBindingList<OrderItemVM> ActiveOrders
		{
			get
			{
				return this._activeOrders;
			}
			set
			{
				if (value != this._activeOrders)
				{
					this._activeOrders = value;
					base.OnPropertyChangedWithValue<MBBindingList<OrderItemVM>>(value, "ActiveOrders");
				}
			}
		}

		// Token: 0x0400013C RID: 316
		private int _behaviorType;

		// Token: 0x0400013D RID: 317
		private int _underAttackOfType;

		// Token: 0x0400013E RID: 318
		private bool _isSelectable;

		// Token: 0x0400013F RID: 319
		private bool _isSelected;

		// Token: 0x04000140 RID: 320
		private bool _isSelectionHighlightActive;

		// Token: 0x04000141 RID: 321
		private bool _showSelectionInputs;

		// Token: 0x04000142 RID: 322
		private string _selectionText;

		// Token: 0x04000143 RID: 323
		private InputKeyItemVM _applySelectionKey;

		// Token: 0x04000144 RID: 324
		private InputKeyItemVM _toggleSelectionKey;

		// Token: 0x04000145 RID: 325
		private MBBindingList<OrderItemVM> _activeOrders;
	}
}
