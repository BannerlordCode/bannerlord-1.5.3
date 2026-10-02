using System;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Popup
{
	// Token: 0x02000041 RID: 65
	public class MPLobbyInformationPopup : ViewModel
	{
		// Token: 0x0600062D RID: 1581 RVA: 0x00014564 File Offset: 0x00012764
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this._titleTextObj != null)
			{
				this.Title = this._titleTextObj.ToString();
			}
			if (this._messageTextObj != null)
			{
				this.Message = this._messageTextObj.ToString();
			}
			this.CloseText = new TextObject("{=yQtzabbe}Close", null).ToString();
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x000145CB File Offset: 0x000127CB
		public void ShowInformation(TextObject title, TextObject message)
		{
			this.IsEnabled = true;
			this._titleTextObj = title;
			this._messageTextObj = message;
			this.RefreshValues();
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x000145E8 File Offset: 0x000127E8
		public void ShowInformation(string title, string message)
		{
			this.IsEnabled = true;
			this._titleTextObj = null;
			this._messageTextObj = null;
			this.Title = title;
			this.Message = message;
			this.RefreshValues();
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00014613 File Offset: 0x00012813
		public void ExecuteClose()
		{
			this.IsEnabled = false;
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x0001461C File Offset: 0x0001281C
		// (set) Token: 0x06000632 RID: 1586 RVA: 0x00014624 File Offset: 0x00012824
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

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x00014642 File Offset: 0x00012842
		// (set) Token: 0x06000634 RID: 1588 RVA: 0x0001464A File Offset: 0x0001284A
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

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x00014668 File Offset: 0x00012868
		// (set) Token: 0x06000636 RID: 1590 RVA: 0x00014670 File Offset: 0x00012870
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

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x00014693 File Offset: 0x00012893
		// (set) Token: 0x06000638 RID: 1592 RVA: 0x0001469B File Offset: 0x0001289B
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

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x000146BE File Offset: 0x000128BE
		// (set) Token: 0x0600063A RID: 1594 RVA: 0x000146C6 File Offset: 0x000128C6
		[DataSourceProperty]
		public string CloseText
		{
			get
			{
				return this._closeText;
			}
			set
			{
				if (value != this._closeText)
				{
					this._closeText = value;
					base.OnPropertyChangedWithValue<string>(value, "CloseText");
				}
			}
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x000146E9 File Offset: 0x000128E9
		public void SetDoneInputKey(HotKey hotkey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x040002E9 RID: 745
		private TextObject _titleTextObj;

		// Token: 0x040002EA RID: 746
		private TextObject _messageTextObj;

		// Token: 0x040002EB RID: 747
		private InputKeyItemVM _doneInputKey;

		// Token: 0x040002EC RID: 748
		private bool _isEnabled;

		// Token: 0x040002ED RID: 749
		private string _title;

		// Token: 0x040002EE RID: 750
		private string _message;

		// Token: 0x040002EF RID: 751
		private string _closeText;
	}
}
