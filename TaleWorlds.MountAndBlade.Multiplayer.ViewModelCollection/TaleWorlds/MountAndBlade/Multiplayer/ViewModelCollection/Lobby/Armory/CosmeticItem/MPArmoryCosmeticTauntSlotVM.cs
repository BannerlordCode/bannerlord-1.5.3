using System;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticItem
{
	// Token: 0x02000081 RID: 129
	public class MPArmoryCosmeticTauntSlotVM : ViewModel
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000CF7 RID: 3319 RVA: 0x000282CC File Offset: 0x000264CC
		// (remove) Token: 0x06000CF8 RID: 3320 RVA: 0x00028300 File Offset: 0x00026500
		public static event Action<MPArmoryCosmeticTauntSlotVM, bool> OnFocusChanged;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000CF9 RID: 3321 RVA: 0x00028334 File Offset: 0x00026534
		// (remove) Token: 0x06000CFA RID: 3322 RVA: 0x00028368 File Offset: 0x00026568
		public static event Action<MPArmoryCosmeticTauntSlotVM> OnSelected;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000CFB RID: 3323 RVA: 0x0002839C File Offset: 0x0002659C
		// (remove) Token: 0x06000CFC RID: 3324 RVA: 0x000283D0 File Offset: 0x000265D0
		public static event Action<MPArmoryCosmeticTauntSlotVM> OnPreview;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000CFD RID: 3325 RVA: 0x00028404 File Offset: 0x00026604
		// (remove) Token: 0x06000CFE RID: 3326 RVA: 0x00028438 File Offset: 0x00026638
		public static event Action<MPArmoryCosmeticTauntSlotVM, MPArmoryCosmeticTauntItemVM, bool> OnTauntEquipped;

		// Token: 0x06000CFF RID: 3327 RVA: 0x0002846B File Offset: 0x0002666B
		public MPArmoryCosmeticTauntSlotVM(int slotIndex)
		{
			this.IsEmpty = true;
			this.SlotIndex = slotIndex;
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x00028481 File Offset: 0x00026681
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM selectKeyVisual = this.SelectKeyVisual;
			if (selectKeyVisual == null)
			{
				return;
			}
			selectKeyVisual.OnFinalize();
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x0002849C File Offset: 0x0002669C
		public void AssignTauntItem(MPArmoryCosmeticTauntItemVM tauntItem, bool isSwapping = false)
		{
			MPArmoryCosmeticTauntItemVM assignedTauntItem = this.AssignedTauntItem;
			this.AssignedTauntItem = tauntItem;
			this.IsEmpty = tauntItem == null;
			if (!isSwapping && assignedTauntItem != null)
			{
				assignedTauntItem.IsUsed = false;
			}
			if (this.AssignedTauntItem != null)
			{
				this.AssignedTauntItem.TauntCosmeticElement.UsageIndex = this.SlotIndex;
				this.AssignedTauntItem.IsUsed = true;
			}
			Action<MPArmoryCosmeticTauntSlotVM, MPArmoryCosmeticTauntItemVM, bool> onTauntEquipped = MPArmoryCosmeticTauntSlotVM.OnTauntEquipped;
			if (onTauntEquipped == null)
			{
				return;
			}
			onTauntEquipped(this, assignedTauntItem, isSwapping);
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x0002850A File Offset: 0x0002670A
		public void ExecuteSelect()
		{
			Action<MPArmoryCosmeticTauntSlotVM> onSelected = MPArmoryCosmeticTauntSlotVM.OnSelected;
			if (onSelected == null)
			{
				return;
			}
			onSelected(this);
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x0002851C File Offset: 0x0002671C
		public void ExecutePreview()
		{
			Action<MPArmoryCosmeticTauntSlotVM> onPreview = MPArmoryCosmeticTauntSlotVM.OnPreview;
			if (onPreview == null)
			{
				return;
			}
			onPreview(this);
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x0002852E File Offset: 0x0002672E
		public void ExecuteFocus()
		{
			Action<MPArmoryCosmeticTauntSlotVM, bool> onFocusChanged = MPArmoryCosmeticTauntSlotVM.OnFocusChanged;
			if (onFocusChanged == null)
			{
				return;
			}
			onFocusChanged(this, true);
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x00028541 File Offset: 0x00026741
		public void ExecuteUnfocus()
		{
			Action<MPArmoryCosmeticTauntSlotVM, bool> onFocusChanged = MPArmoryCosmeticTauntSlotVM.OnFocusChanged;
			if (onFocusChanged == null)
			{
				return;
			}
			onFocusChanged(this, false);
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00028554 File Offset: 0x00026754
		public void SetSelectKeyVisual(HotKey hotKey)
		{
			this.SelectKeyVisual = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x00028563 File Offset: 0x00026763
		public void SetEmptySlotKeyVisual(HotKey hotKey)
		{
			this.EmptySlotKeyVisual = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06000D08 RID: 3336 RVA: 0x00028572 File Offset: 0x00026772
		// (set) Token: 0x06000D09 RID: 3337 RVA: 0x0002857A File Offset: 0x0002677A
		[DataSourceProperty]
		public InputKeyItemVM SelectKeyVisual
		{
			get
			{
				return this._selectKeyVisual;
			}
			set
			{
				if (value != this._selectKeyVisual)
				{
					this._selectKeyVisual = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "SelectKeyVisual");
				}
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06000D0A RID: 3338 RVA: 0x00028598 File Offset: 0x00026798
		// (set) Token: 0x06000D0B RID: 3339 RVA: 0x000285A0 File Offset: 0x000267A0
		[DataSourceProperty]
		public InputKeyItemVM EmptySlotKeyVisual
		{
			get
			{
				return this._emptySlotKeyVisual;
			}
			set
			{
				if (value != this._emptySlotKeyVisual)
				{
					this._emptySlotKeyVisual = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "EmptySlotKeyVisual");
				}
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06000D0C RID: 3340 RVA: 0x000285BE File Offset: 0x000267BE
		// (set) Token: 0x06000D0D RID: 3341 RVA: 0x000285C6 File Offset: 0x000267C6
		[DataSourceProperty]
		public bool IsAcceptingTaunts
		{
			get
			{
				return this._isAcceptingTaunts;
			}
			set
			{
				if (value != this._isAcceptingTaunts)
				{
					this._isAcceptingTaunts = value;
					base.OnPropertyChangedWithValue(value, "IsAcceptingTaunts");
				}
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x000285E4 File Offset: 0x000267E4
		// (set) Token: 0x06000D0F RID: 3343 RVA: 0x000285EC File Offset: 0x000267EC
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06000D10 RID: 3344 RVA: 0x0002860A File Offset: 0x0002680A
		// (set) Token: 0x06000D11 RID: 3345 RVA: 0x00028612 File Offset: 0x00026812
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

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06000D12 RID: 3346 RVA: 0x00028630 File Offset: 0x00026830
		// (set) Token: 0x06000D13 RID: 3347 RVA: 0x00028638 File Offset: 0x00026838
		[DataSourceProperty]
		public bool IsEmpty
		{
			get
			{
				return this._isEmpty;
			}
			set
			{
				if (value != this._isEmpty)
				{
					this._isEmpty = value;
					base.OnPropertyChangedWithValue(value, "IsEmpty");
				}
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06000D14 RID: 3348 RVA: 0x00028656 File Offset: 0x00026856
		// (set) Token: 0x06000D15 RID: 3349 RVA: 0x0002865E File Offset: 0x0002685E
		[DataSourceProperty]
		public bool IsFocused
		{
			get
			{
				return this._isFocused;
			}
			set
			{
				if (value != this._isFocused)
				{
					this._isFocused = value;
					base.OnPropertyChangedWithValue(value, "IsFocused");
				}
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x0002867C File Offset: 0x0002687C
		// (set) Token: 0x06000D17 RID: 3351 RVA: 0x00028684 File Offset: 0x00026884
		[DataSourceProperty]
		public MPArmoryCosmeticTauntItemVM AssignedTauntItem
		{
			get
			{
				return this._assignedTauntItem;
			}
			set
			{
				if (value != this._assignedTauntItem)
				{
					this._assignedTauntItem = value;
					base.OnPropertyChangedWithValue<MPArmoryCosmeticTauntItemVM>(value, "AssignedTauntItem");
				}
			}
		}

		// Token: 0x040005E3 RID: 1507
		public readonly int SlotIndex;

		// Token: 0x040005E4 RID: 1508
		private InputKeyItemVM _selectKeyVisual;

		// Token: 0x040005E5 RID: 1509
		private InputKeyItemVM _emptySlotKeyVisual;

		// Token: 0x040005E6 RID: 1510
		private bool _isAcceptingTaunts;

		// Token: 0x040005E7 RID: 1511
		private bool _isSelected;

		// Token: 0x040005E8 RID: 1512
		private bool _isEnabled;

		// Token: 0x040005E9 RID: 1513
		private bool _isEmpty;

		// Token: 0x040005EA RID: 1514
		private bool _isFocused;

		// Token: 0x040005EB RID: 1515
		private MPArmoryCosmeticTauntItemVM _assignedTauntItem;
	}
}
