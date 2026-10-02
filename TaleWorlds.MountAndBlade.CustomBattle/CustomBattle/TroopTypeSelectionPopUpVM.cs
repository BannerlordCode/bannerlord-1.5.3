using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle
{
	// Token: 0x0200001D RID: 29
	public class TroopTypeSelectionPopUpVM : ViewModel
	{
		// Token: 0x0600018A RID: 394 RVA: 0x00009F4C File Offset: 0x0000814C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.DoneLbl = GameTexts.FindText("str_done", null).ToString();
			this.CancelLbl = GameTexts.FindText("str_cancel", null).ToString();
			this.SelectAllLbl = GameTexts.FindText("str_custom_battle_select_all", null).ToString();
			this.BackToDefaultLbl = GameTexts.FindText("str_custom_battle_back_to_default", null).ToString();
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00009FB7 File Offset: 0x000081B7
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.DoneInputKey.OnFinalize();
			this.CancelInputKey.OnFinalize();
			this.ResetInputKey.OnFinalize();
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00009FE0 File Offset: 0x000081E0
		public void OpenPopUp(string title, MBBindingList<CustomBattleTroopTypeVM> troops)
		{
			this._itemSelectionsBackUp = new List<bool>();
			foreach (CustomBattleTroopTypeVM customBattleTroopTypeVM in troops)
			{
				this._itemSelectionsBackUp.Add(customBattleTroopTypeVM.IsSelected);
			}
			this._selectedItemCount = troops.Count<CustomBattleTroopTypeVM>((CustomBattleTroopTypeVM x) => x.IsSelected);
			this.Title = title;
			this.Items = troops;
			this.IsOpen = true;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000A080 File Offset: 0x00008280
		public void OnItemSelectionToggled(CustomBattleTroopTypeVM item)
		{
			if (this._selectedItemCount > 1 || !item.IsSelected)
			{
				item.IsSelected = !item.IsSelected;
				this._selectedItemCount += (item.IsSelected ? 1 : (-1));
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000A0CC File Offset: 0x000082CC
		public void ExecuteSelectAll()
		{
			this.Items.ApplyActionOnAllItems(delegate(CustomBattleTroopTypeVM x)
			{
				x.IsSelected = true;
			});
			this._selectedItemCount = this.Items.Count;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000A10C File Offset: 0x0000830C
		public void ExecuteBackToDefault()
		{
			this.Items.ApplyActionOnAllItems(delegate(CustomBattleTroopTypeVM x)
			{
				x.IsSelected = x.IsDefault;
			});
			this._selectedItemCount = this.Items.Count<CustomBattleTroopTypeVM>((CustomBattleTroopTypeVM x) => x.IsSelected);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000A173 File Offset: 0x00008373
		public void ExecuteCancel()
		{
			this.ExecuteReset();
			Action onPopUpClosed = this.OnPopUpClosed;
			if (onPopUpClosed != null)
			{
				onPopUpClosed();
			}
			this.IsOpen = false;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000A193 File Offset: 0x00008393
		public void ExecuteDone()
		{
			this.IsOpen = false;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000A19C File Offset: 0x0000839C
		public void ExecuteReset()
		{
			int count = this._itemSelectionsBackUp.Count;
			if (count != this.Items.Count)
			{
				Debug.FailedAssert("Backup troop count does not match with the actual troop count.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.CustomBattle\\CustomBattle\\TroopTypeSelectionPopUpVM.cs", "ExecuteReset", 100);
				return;
			}
			for (int i = 0; i < count; i++)
			{
				this.Items[i].IsSelected = this._itemSelectionsBackUp[i];
			}
			this._selectedItemCount = this.Items.Count<CustomBattleTroopTypeVM>((CustomBattleTroopTypeVM x) => x.IsSelected);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000A233 File Offset: 0x00008433
		public void SetCancelInputKey(HotKey hotkey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000A242 File Offset: 0x00008442
		public void SetDoneInputKey(HotKey hotkey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000A251 File Offset: 0x00008451
		public void SetResetInputKey(HotKey hotkey)
		{
			this.ResetInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000196 RID: 406 RVA: 0x0000A260 File Offset: 0x00008460
		// (set) Token: 0x06000197 RID: 407 RVA: 0x0000A268 File Offset: 0x00008468
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000198 RID: 408 RVA: 0x0000A286 File Offset: 0x00008486
		// (set) Token: 0x06000199 RID: 409 RVA: 0x0000A28E File Offset: 0x0000848E
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600019A RID: 410 RVA: 0x0000A2AC File Offset: 0x000084AC
		// (set) Token: 0x0600019B RID: 411 RVA: 0x0000A2B4 File Offset: 0x000084B4
		[DataSourceProperty]
		public InputKeyItemVM ResetInputKey
		{
			get
			{
				return this._resetInputKey;
			}
			set
			{
				if (value != this._resetInputKey)
				{
					this._resetInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ResetInputKey");
				}
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600019C RID: 412 RVA: 0x0000A2D2 File Offset: 0x000084D2
		// (set) Token: 0x0600019D RID: 413 RVA: 0x0000A2DA File Offset: 0x000084DA
		[DataSourceProperty]
		public MBBindingList<CustomBattleTroopTypeVM> Items
		{
			get
			{
				return this._items;
			}
			set
			{
				if (value != this._items)
				{
					this._items = value;
					base.OnPropertyChangedWithValue<MBBindingList<CustomBattleTroopTypeVM>>(value, "Items");
				}
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600019E RID: 414 RVA: 0x0000A2F8 File Offset: 0x000084F8
		// (set) Token: 0x0600019F RID: 415 RVA: 0x0000A300 File Offset: 0x00008500
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000A323 File Offset: 0x00008523
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x0000A32B File Offset: 0x0000852B
		[DataSourceProperty]
		public string DoneLbl
		{
			get
			{
				return this._doneLbl;
			}
			set
			{
				if (value != this._doneLbl)
				{
					this._doneLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneLbl");
				}
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x0000A34E File Offset: 0x0000854E
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x0000A356 File Offset: 0x00008556
		[DataSourceProperty]
		public string CancelLbl
		{
			get
			{
				return this._cancelLbl;
			}
			set
			{
				if (value != this._cancelLbl)
				{
					this._cancelLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelLbl");
				}
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000A379 File Offset: 0x00008579
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x0000A381 File Offset: 0x00008581
		[DataSourceProperty]
		public string SelectAllLbl
		{
			get
			{
				return this._selectAllLbl;
			}
			set
			{
				if (value != this._selectAllLbl)
				{
					this._selectAllLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectAllLbl");
				}
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x0000A3A4 File Offset: 0x000085A4
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x0000A3AC File Offset: 0x000085AC
		[DataSourceProperty]
		public string BackToDefaultLbl
		{
			get
			{
				return this._backToDefaultLbl;
			}
			set
			{
				if (value != this._backToDefaultLbl)
				{
					this._backToDefaultLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "BackToDefaultLbl");
				}
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x0000A3CF File Offset: 0x000085CF
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x0000A3D7 File Offset: 0x000085D7
		[DataSourceProperty]
		public bool IsOpen
		{
			get
			{
				return this._isOpen;
			}
			set
			{
				if (value != this._isOpen)
				{
					this._isOpen = value;
					base.OnPropertyChangedWithValue(value, "IsOpen");
				}
			}
		}

		// Token: 0x040000F7 RID: 247
		public Action OnPopUpClosed;

		// Token: 0x040000F8 RID: 248
		private List<bool> _itemSelectionsBackUp;

		// Token: 0x040000F9 RID: 249
		private int _selectedItemCount;

		// Token: 0x040000FA RID: 250
		private InputKeyItemVM _doneInputKey;

		// Token: 0x040000FB RID: 251
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x040000FC RID: 252
		private InputKeyItemVM _resetInputKey;

		// Token: 0x040000FD RID: 253
		private MBBindingList<CustomBattleTroopTypeVM> _items;

		// Token: 0x040000FE RID: 254
		private string _title;

		// Token: 0x040000FF RID: 255
		private string _doneLbl;

		// Token: 0x04000100 RID: 256
		private string _cancelLbl;

		// Token: 0x04000101 RID: 257
		private string _selectAllLbl;

		// Token: 0x04000102 RID: 258
		private string _backToDefaultLbl;

		// Token: 0x04000103 RID: 259
		private bool _isOpen;
	}
}
