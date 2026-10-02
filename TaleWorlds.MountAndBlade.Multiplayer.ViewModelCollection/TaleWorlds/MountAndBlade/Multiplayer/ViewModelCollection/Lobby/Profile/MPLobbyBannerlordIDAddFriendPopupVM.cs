using System;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x02000037 RID: 55
	public class MPLobbyBannerlordIDAddFriendPopupVM : ViewModel
	{
		// Token: 0x06000516 RID: 1302 RVA: 0x00011DCA File Offset: 0x0000FFCA
		public MPLobbyBannerlordIDAddFriendPopupVM()
		{
			this.BannerlordIDInputText = "";
			this.ErrorText = "";
			this.RefreshValues();
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00011DEE File Offset: 0x0000FFEE
		public void ExecuteOpenPopup()
		{
			this.IsSelected = true;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00011DF7 File Offset: 0x0000FFF7
		public void ExecuteClosePopup()
		{
			this.IsSelected = false;
			this.BannerlordIDInputText = "";
			this.ErrorText = "";
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00011E16 File Offset: 0x00010016
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=L3DJHTdY}Enter Bannerlord ID", null).ToString();
			this.AddText = new TextObject("{=tC9C8TLi}Add Friend", null).ToString();
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00011E4C File Offset: 0x0001004C
		public async void ExecuteTryAddFriend()
		{
			string[] array = this.BannerlordIDInputText.Split(new char[] { '#' });
			if (array.Length == 2 && !array[1].IsEmpty<char>())
			{
				string username = array[0];
				int id = 0;
				bool flag = Common.IsAllLetters(array[0]) && array[0].Length >= Parameters.UsernameMinLength;
				if (int.TryParse(array[1], out id) && flag)
				{
					TaskAwaiter<bool> taskAwaiter = NetworkMain.GameClient.DoesPlayerWithUsernameAndIdExist(username, id).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						NetworkMain.GameClient.AddFriendByUsernameAndId(username, id, BannerlordConfig.EnableGenericNames);
						this.ExecuteClosePopup();
					}
					else
					{
						this.ErrorText = new TextObject("{=tTwQsP6j}Player does not exist", null).ToString();
					}
				}
				else
				{
					this.ErrorText = new TextObject("{=rWm5udCd}You must enter a valid Bannerlord ID", null).ToString();
				}
				username = null;
			}
			else
			{
				this.ErrorText = new TextObject("{=rWm5udCd}You must enter a valid Bannerlord ID", null).ToString();
			}
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00011E85 File Offset: 0x00010085
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey == null)
			{
				return;
			}
			doneInputKey.OnFinalize();
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00011EAE File Offset: 0x000100AE
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00011EBD File Offset: 0x000100BD
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x00011ECC File Offset: 0x000100CC
		// (set) Token: 0x0600051F RID: 1311 RVA: 0x00011ED4 File Offset: 0x000100D4
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
					base.OnPropertyChanged("CancelInputKey");
				}
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x00011EF1 File Offset: 0x000100F1
		// (set) Token: 0x06000521 RID: 1313 RVA: 0x00011EF9 File Offset: 0x000100F9
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
					base.OnPropertyChanged("DoneInputKey");
				}
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x00011F16 File Offset: 0x00010116
		// (set) Token: 0x06000523 RID: 1315 RVA: 0x00011F1E File Offset: 0x0001011E
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
					base.OnPropertyChanged("IsSelected");
				}
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x00011F3B File Offset: 0x0001013B
		// (set) Token: 0x06000525 RID: 1317 RVA: 0x00011F43 File Offset: 0x00010143
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
					base.OnPropertyChanged("TitleText");
				}
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x00011F65 File Offset: 0x00010165
		// (set) Token: 0x06000527 RID: 1319 RVA: 0x00011F6D File Offset: 0x0001016D
		[DataSourceProperty]
		public string AddText
		{
			get
			{
				return this._addText;
			}
			set
			{
				if (value != this._addText)
				{
					this._addText = value;
					base.OnPropertyChanged("AddText");
				}
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x00011F8F File Offset: 0x0001018F
		// (set) Token: 0x06000529 RID: 1321 RVA: 0x00011F97 File Offset: 0x00010197
		[DataSourceProperty]
		public string ErrorText
		{
			get
			{
				return this._errorText;
			}
			set
			{
				if (value != this._errorText)
				{
					this._errorText = value;
					base.OnPropertyChanged("ErrorText");
				}
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00011FB9 File Offset: 0x000101B9
		// (set) Token: 0x0600052B RID: 1323 RVA: 0x00011FC1 File Offset: 0x000101C1
		[DataSourceProperty]
		public string BannerlordIDInputText
		{
			get
			{
				return this._bannerlordIDInputText;
			}
			set
			{
				if (value != this._bannerlordIDInputText)
				{
					this._bannerlordIDInputText = value;
					base.OnPropertyChanged("BannerlordIDInputText");
					this.ErrorText = "";
				}
			}
		}

		// Token: 0x04000275 RID: 629
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000276 RID: 630
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000277 RID: 631
		private bool _isSelected;

		// Token: 0x04000278 RID: 632
		private string _titleText;

		// Token: 0x04000279 RID: 633
		private string _addText;

		// Token: 0x0400027A RID: 634
		private string _errorText;

		// Token: 0x0400027B RID: 635
		private string _bannerlordIDInputText;
	}
}
