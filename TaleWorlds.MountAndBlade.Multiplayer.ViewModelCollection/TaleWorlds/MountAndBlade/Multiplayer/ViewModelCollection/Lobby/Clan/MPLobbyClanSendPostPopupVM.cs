using System;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x02000074 RID: 116
	public class MPLobbyClanSendPostPopupVM : ViewModel
	{
		// Token: 0x06000B8F RID: 2959 RVA: 0x00022F18 File Offset: 0x00021118
		public MPLobbyClanSendPostPopupVM(MPLobbyClanSendPostPopupVM.PostPopupMode popupMode)
		{
			this._popupMode = popupMode;
			this.RefreshValues();
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x00022F30 File Offset: 0x00021130
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SendText = new TextObject("{=qTYsYJ9V}Send", null).ToString();
			if (this._popupMode == MPLobbyClanSendPostPopupVM.PostPopupMode.Information)
			{
				this.TitleText = new TextObject("{=zravuI1b}Type Clan Information", null).ToString();
				return;
			}
			if (this._popupMode == MPLobbyClanSendPostPopupVM.PostPopupMode.Announcement)
			{
				this.TitleText = new TextObject("{=g5W32uf4}Type Your Announcement", null).ToString();
			}
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x00022F97 File Offset: 0x00021197
		public void ExecuteOpenPopup()
		{
			this.IsSelected = true;
			this.PostData = "";
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x00022FAB File Offset: 0x000211AB
		public void ExecuteClosePopup()
		{
			this.IsSelected = false;
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x00022FB4 File Offset: 0x000211B4
		public async void ExecuteSend()
		{
			bool flag;
			if (this._popupMode == MPLobbyClanSendPostPopupVM.PostPopupMode.Information)
			{
				flag = await NetworkMain.GameClient.SetClanInformationText(this.PostData);
			}
			else
			{
				flag = this._popupMode != MPLobbyClanSendPostPopupVM.PostPopupMode.Announcement || await NetworkMain.GameClient.AddClanAnnouncement(this.PostData);
			}
			if (flag)
			{
				this.ExecuteClosePopup();
			}
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x00022FED File Offset: 0x000211ED
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

		// Token: 0x06000B95 RID: 2965 RVA: 0x00023016 File Offset: 0x00021216
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x00023025 File Offset: 0x00021225
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000B97 RID: 2967 RVA: 0x00023034 File Offset: 0x00021234
		// (set) Token: 0x06000B98 RID: 2968 RVA: 0x0002303C File Offset: 0x0002123C
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

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000B99 RID: 2969 RVA: 0x00023059 File Offset: 0x00021259
		// (set) Token: 0x06000B9A RID: 2970 RVA: 0x00023061 File Offset: 0x00021261
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

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000B9B RID: 2971 RVA: 0x0002307E File Offset: 0x0002127E
		// (set) Token: 0x06000B9C RID: 2972 RVA: 0x00023086 File Offset: 0x00021286
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

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000B9D RID: 2973 RVA: 0x000230A3 File Offset: 0x000212A3
		// (set) Token: 0x06000B9E RID: 2974 RVA: 0x000230AB File Offset: 0x000212AB
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

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000B9F RID: 2975 RVA: 0x000230CD File Offset: 0x000212CD
		// (set) Token: 0x06000BA0 RID: 2976 RVA: 0x000230D5 File Offset: 0x000212D5
		[DataSourceProperty]
		public string PostData
		{
			get
			{
				return this._postData;
			}
			set
			{
				if (value != this._postData)
				{
					this._postData = value;
					base.OnPropertyChanged("PostData");
				}
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000BA1 RID: 2977 RVA: 0x000230F7 File Offset: 0x000212F7
		// (set) Token: 0x06000BA2 RID: 2978 RVA: 0x000230FF File Offset: 0x000212FF
		[DataSourceProperty]
		public string SendText
		{
			get
			{
				return this._sendText;
			}
			set
			{
				if (value != this._sendText)
				{
					this._sendText = value;
					base.OnPropertyChanged("SendText");
				}
			}
		}

		// Token: 0x0400053D RID: 1341
		private MPLobbyClanSendPostPopupVM.PostPopupMode _popupMode;

		// Token: 0x0400053E RID: 1342
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x0400053F RID: 1343
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000540 RID: 1344
		private bool _isSelected;

		// Token: 0x04000541 RID: 1345
		private string _titleText;

		// Token: 0x04000542 RID: 1346
		private string _postData;

		// Token: 0x04000543 RID: 1347
		private string _sendText;

		// Token: 0x02000165 RID: 357
		public enum PostPopupMode
		{
			// Token: 0x04000A3C RID: 2620
			Information,
			// Token: 0x04000A3D RID: 2621
			Announcement
		}
	}
}
