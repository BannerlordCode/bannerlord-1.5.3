using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Inquiries
{
	// Token: 0x02000045 RID: 69
	public abstract class PopUpBaseVM : ViewModel
	{
		// Token: 0x060005C5 RID: 1477 RVA: 0x00015A7C File Offset: 0x00013C7C
		public PopUpBaseVM(Action closeQuery)
		{
			this._closeQuery = closeQuery;
		}

		// Token: 0x060005C6 RID: 1478
		public abstract void ExecuteAffirmativeAction();

		// Token: 0x060005C7 RID: 1479
		public abstract void ExecuteNegativeAction();

		// Token: 0x060005C8 RID: 1480 RVA: 0x00015A8B File Offset: 0x00013C8B
		public virtual void OnTick(float dt)
		{
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00015A8D File Offset: 0x00013C8D
		public virtual void OnClearData()
		{
			this.TitleText = null;
			this.PopUpLabel = null;
			this.ButtonOkLabel = null;
			this.ButtonCancelLabel = null;
			this.IsButtonOkShown = false;
			this.IsButtonCancelShown = false;
			this.IsButtonOkEnabled = false;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00015AC0 File Offset: 0x00013CC0
		public void ForceRefreshKeyVisuals()
		{
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.RefreshValues();
			}
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey == null)
			{
				return;
			}
			doneInputKey.RefreshValues();
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00015AE3 File Offset: 0x00013CE3
		public void CloseQuery()
		{
			Action closeQuery = this._closeQuery;
			if (closeQuery == null)
			{
				return;
			}
			closeQuery();
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00015AF5 File Offset: 0x00013CF5
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey == null)
			{
				return;
			}
			cancelInputKey.OnFinalize();
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x00015B1E File Offset: 0x00013D1E
		// (set) Token: 0x060005CE RID: 1486 RVA: 0x00015B26 File Offset: 0x00013D26
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x00015B49 File Offset: 0x00013D49
		// (set) Token: 0x060005D0 RID: 1488 RVA: 0x00015B51 File Offset: 0x00013D51
		[DataSourceProperty]
		public string PopUpLabel
		{
			get
			{
				return this._popUpLabel;
			}
			set
			{
				if (value != this._popUpLabel)
				{
					this._popUpLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "PopUpLabel");
				}
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x00015B74 File Offset: 0x00013D74
		// (set) Token: 0x060005D2 RID: 1490 RVA: 0x00015B7C File Offset: 0x00013D7C
		[DataSourceProperty]
		public string ButtonOkLabel
		{
			get
			{
				return this._buttonOkLabel;
			}
			set
			{
				if (value != this._buttonOkLabel)
				{
					this._buttonOkLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "ButtonOkLabel");
				}
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x00015B9F File Offset: 0x00013D9F
		// (set) Token: 0x060005D4 RID: 1492 RVA: 0x00015BA7 File Offset: 0x00013DA7
		[DataSourceProperty]
		public string ButtonCancelLabel
		{
			get
			{
				return this._buttonCancelLabel;
			}
			set
			{
				if (value != this._buttonCancelLabel)
				{
					this._buttonCancelLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "ButtonCancelLabel");
				}
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x00015BCA File Offset: 0x00013DCA
		// (set) Token: 0x060005D6 RID: 1494 RVA: 0x00015BD2 File Offset: 0x00013DD2
		[DataSourceProperty]
		public bool IsButtonOkShown
		{
			get
			{
				return this._isButtonOkShown;
			}
			set
			{
				if (value != this._isButtonOkShown)
				{
					this._isButtonOkShown = value;
					base.OnPropertyChangedWithValue(value, "IsButtonOkShown");
				}
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x00015BF0 File Offset: 0x00013DF0
		// (set) Token: 0x060005D8 RID: 1496 RVA: 0x00015BF8 File Offset: 0x00013DF8
		[DataSourceProperty]
		public bool IsButtonCancelShown
		{
			get
			{
				return this._isButtonCancelShown;
			}
			set
			{
				if (value != this._isButtonCancelShown)
				{
					this._isButtonCancelShown = value;
					base.OnPropertyChangedWithValue(value, "IsButtonCancelShown");
				}
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x00015C16 File Offset: 0x00013E16
		// (set) Token: 0x060005DA RID: 1498 RVA: 0x00015C1E File Offset: 0x00013E1E
		[DataSourceProperty]
		public bool IsButtonOkEnabled
		{
			get
			{
				return this._isButtonOkEnabled;
			}
			set
			{
				if (value != this._isButtonOkEnabled)
				{
					this._isButtonOkEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsButtonOkEnabled");
				}
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x00015C3C File Offset: 0x00013E3C
		// (set) Token: 0x060005DC RID: 1500 RVA: 0x00015C44 File Offset: 0x00013E44
		[DataSourceProperty]
		public bool IsButtonCancelEnabled
		{
			get
			{
				return this._isButtonCancelEnabled;
			}
			set
			{
				if (value != this._isButtonCancelEnabled)
				{
					this._isButtonCancelEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsButtonCancelEnabled");
				}
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060005DD RID: 1501 RVA: 0x00015C62 File Offset: 0x00013E62
		// (set) Token: 0x060005DE RID: 1502 RVA: 0x00015C6A File Offset: 0x00013E6A
		[DataSourceProperty]
		public HintViewModel ButtonOkHint
		{
			get
			{
				return this._buttonOkHint;
			}
			set
			{
				if (value != this._buttonOkHint)
				{
					this._buttonOkHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ButtonOkHint");
				}
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x00015C88 File Offset: 0x00013E88
		// (set) Token: 0x060005E0 RID: 1504 RVA: 0x00015C90 File Offset: 0x00013E90
		[DataSourceProperty]
		public HintViewModel ButtonCancelHint
		{
			get
			{
				return this._buttonCancelHint;
			}
			set
			{
				if (value != this._buttonCancelHint)
				{
					this._buttonCancelHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ButtonCancelHint");
				}
			}
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00015CAE File Offset: 0x00013EAE
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00015CBD File Offset: 0x00013EBD
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x00015CCC File Offset: 0x00013ECC
		// (set) Token: 0x060005E4 RID: 1508 RVA: 0x00015CD4 File Offset: 0x00013ED4
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

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060005E5 RID: 1509 RVA: 0x00015CF2 File Offset: 0x00013EF2
		// (set) Token: 0x060005E6 RID: 1510 RVA: 0x00015CFA File Offset: 0x00013EFA
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

		// Token: 0x04000298 RID: 664
		protected Action _affirmativeAction;

		// Token: 0x04000299 RID: 665
		protected Action _negativeAction;

		// Token: 0x0400029A RID: 666
		private Action _closeQuery;

		// Token: 0x0400029B RID: 667
		private string _titleText;

		// Token: 0x0400029C RID: 668
		private string _popUpLabel;

		// Token: 0x0400029D RID: 669
		private string _buttonOkLabel;

		// Token: 0x0400029E RID: 670
		private string _buttonCancelLabel;

		// Token: 0x0400029F RID: 671
		private bool _isButtonOkShown;

		// Token: 0x040002A0 RID: 672
		private bool _isButtonCancelShown;

		// Token: 0x040002A1 RID: 673
		private bool _isButtonOkEnabled;

		// Token: 0x040002A2 RID: 674
		private bool _isButtonCancelEnabled;

		// Token: 0x040002A3 RID: 675
		private HintViewModel _buttonOkHint;

		// Token: 0x040002A4 RID: 676
		private HintViewModel _buttonCancelHint;

		// Token: 0x040002A5 RID: 677
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x040002A6 RID: 678
		private InputKeyItemVM _doneInputKey;
	}
}
