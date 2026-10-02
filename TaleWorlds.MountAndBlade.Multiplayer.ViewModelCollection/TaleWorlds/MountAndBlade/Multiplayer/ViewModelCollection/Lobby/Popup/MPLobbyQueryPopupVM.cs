using System;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Popup
{
	// Token: 0x02000042 RID: 66
	public class MPLobbyQueryPopupVM : ViewModel
	{
		// Token: 0x0600063D RID: 1597 RVA: 0x00014700 File Offset: 0x00012900
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject titleObj = this._titleObj;
			this.Title = ((titleObj != null) ? titleObj.ToString() : null) ?? "";
			TextObject messageObj = this._messageObj;
			this.Message = ((messageObj != null) ? messageObj.ToString() : null) ?? "";
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00014755 File Offset: 0x00012955
		public void ShowMessage(TextObject title, TextObject message)
		{
			this.IsEnabled = true;
			this._titleObj = title;
			this._messageObj = message;
			this.RefreshValues();
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00014772 File Offset: 0x00012972
		public void ShowInquiry(TextObject title, TextObject message, Action onAccepted, Action onDeclined)
		{
			this.IsEnabled = true;
			this.IsInquiry = true;
			this._titleObj = title;
			this._messageObj = message;
			this._onAccepted = onAccepted;
			this._onDeclined = onDeclined;
			this.RefreshValues();
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x000147A5 File Offset: 0x000129A5
		public void ExecuteAccept()
		{
			this.IsEnabled = false;
			this.IsInquiry = false;
			Action onAccepted = this._onAccepted;
			if (onAccepted == null)
			{
				return;
			}
			onAccepted();
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x000147C5 File Offset: 0x000129C5
		public void ExecuteDecline()
		{
			this.IsEnabled = false;
			this.IsInquiry = false;
			Action onDeclined = this._onDeclined;
			if (onDeclined == null)
			{
				return;
			}
			onDeclined();
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x000147E5 File Offset: 0x000129E5
		// (set) Token: 0x06000643 RID: 1603 RVA: 0x000147ED File Offset: 0x000129ED
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

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x0001480B File Offset: 0x00012A0B
		// (set) Token: 0x06000645 RID: 1605 RVA: 0x00014813 File Offset: 0x00012A13
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

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x00014831 File Offset: 0x00012A31
		// (set) Token: 0x06000647 RID: 1607 RVA: 0x00014839 File Offset: 0x00012A39
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

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x00014857 File Offset: 0x00012A57
		// (set) Token: 0x06000649 RID: 1609 RVA: 0x0001485F File Offset: 0x00012A5F
		[DataSourceProperty]
		public bool IsInquiry
		{
			get
			{
				return this._isInquiry;
			}
			set
			{
				if (value != this._isInquiry)
				{
					this._isInquiry = value;
					base.OnPropertyChangedWithValue(value, "IsInquiry");
				}
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x0001487D File Offset: 0x00012A7D
		// (set) Token: 0x0600064B RID: 1611 RVA: 0x00014885 File Offset: 0x00012A85
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

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x000148A8 File Offset: 0x00012AA8
		// (set) Token: 0x0600064D RID: 1613 RVA: 0x000148B0 File Offset: 0x00012AB0
		[DataSourceProperty]
		public string Message
		{
			get
			{
				return this._message;
			}
			set
			{
				if (value != this._message)
				{
					this._message = value;
					base.OnPropertyChangedWithValue<string>(value, "Message");
				}
			}
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x000148D3 File Offset: 0x00012AD3
		public void SetDoneInputKey(HotKey hotkey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x000148E2 File Offset: 0x00012AE2
		public void SetCancelInputKey(HotKey hotkey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x040002F0 RID: 752
		private TextObject _titleObj;

		// Token: 0x040002F1 RID: 753
		private TextObject _messageObj;

		// Token: 0x040002F2 RID: 754
		private Action _onAccepted;

		// Token: 0x040002F3 RID: 755
		private Action _onDeclined;

		// Token: 0x040002F4 RID: 756
		private InputKeyItemVM _doneInputKey;

		// Token: 0x040002F5 RID: 757
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x040002F6 RID: 758
		private bool _isEnabled;

		// Token: 0x040002F7 RID: 759
		private bool _isInquiry;

		// Token: 0x040002F8 RID: 760
		private string _title;

		// Token: 0x040002F9 RID: 761
		private string _message;
	}
}
