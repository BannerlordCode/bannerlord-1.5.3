using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x02000038 RID: 56
	public class MPLobbyBannerlordIDChangePopup : ViewModel
	{
		// Token: 0x0600052C RID: 1324 RVA: 0x00011FEE File Offset: 0x000101EE
		public MPLobbyBannerlordIDChangePopup()
		{
			this.BannerlordIDInputText = "";
			this.RefreshValues();
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00012008 File Offset: 0x00010208
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ChangeBannerlordIDText = new TextObject("{=ozREO8ev}Change Bannerlord ID", null).ToString();
			this.TypeYourNameText = new TextObject("{=clxT9H4T}Type Your Name", null).ToString();
			this.RequestSentText = new TextObject("{=V2lpn6dc}Your Bannerlord ID changing request has been successfully sent.", null).ToString();
			this.DoneText = GameTexts.FindText("str_done", null).ToString();
			this.CancelText = GameTexts.FindText("str_cancel", null).ToString();
			this.ErrorText = "";
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00012094 File Offset: 0x00010294
		public void ExecuteOpenPopup()
		{
			this.IsSelected = true;
			this.HasRequestSent = false;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x000120A4 File Offset: 0x000102A4
		public void ExecuteClosePopup()
		{
			this.IsSelected = false;
			this.BannerlordIDInputText = "";
			this.ErrorText = "";
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x000120C4 File Offset: 0x000102C4
		private async Task<bool> IsInputValid()
		{
			bool flag;
			if (this.BannerlordIDInputText.Length < Parameters.UsernameMinLength)
			{
				GameTexts.SetVariable("STR1", new TextObject("{=k7fJ7TF0}Has to be at least", null));
				GameTexts.SetVariable("STR2", Parameters.UsernameMinLength);
				string text = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
				GameTexts.SetVariable("STR1", text);
				GameTexts.SetVariable("STR2", new TextObject("{=nWJGjCgy}characters", null));
				this.ErrorText = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
				flag = false;
			}
			else if (!Common.IsAllLetters(this.BannerlordIDInputText))
			{
				this.ErrorText = new TextObject("{=Po8jNaXb}Can only contain letters", null).ToString();
				flag = false;
			}
			else
			{
				TaskAwaiter<bool> taskAwaiter = PlatformServices.Instance.VerifyString(this.BannerlordIDInputText).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (!taskAwaiter.GetResult())
				{
					this.ErrorText = new TextObject("{=bXAIlBHv}Can not contain offensive language", null).ToString();
					flag = false;
				}
				else
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0001210C File Offset: 0x0001030C
		public async void ExecuteApply()
		{
			if (!this.HasRequestSent)
			{
				TaskAwaiter<bool> taskAwaiter = this.IsInputValid().GetAwaiter();
				TaskAwaiter<bool> taskAwaiter2;
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					taskAwaiter = NetworkMain.GameClient.ChangeUsername(this.BannerlordIDInputText).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						this.HasRequestSent = true;
						this.ErrorText = "";
					}
				}
			}
			else
			{
				this.ExecuteClosePopup();
			}
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x00012145 File Offset: 0x00010345
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

		// Token: 0x06000533 RID: 1331 RVA: 0x0001216E File Offset: 0x0001036E
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001217D File Offset: 0x0001037D
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x0001218C File Offset: 0x0001038C
		// (set) Token: 0x06000536 RID: 1334 RVA: 0x00012194 File Offset: 0x00010394
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

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x000121B1 File Offset: 0x000103B1
		// (set) Token: 0x06000538 RID: 1336 RVA: 0x000121B9 File Offset: 0x000103B9
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

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x000121D6 File Offset: 0x000103D6
		// (set) Token: 0x0600053A RID: 1338 RVA: 0x000121DE File Offset: 0x000103DE
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

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x000121FB File Offset: 0x000103FB
		// (set) Token: 0x0600053C RID: 1340 RVA: 0x00012203 File Offset: 0x00010403
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
					this.ErrorText = "";
					base.OnPropertyChanged("BannerlordIDInputText");
				}
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x00012230 File Offset: 0x00010430
		// (set) Token: 0x0600053E RID: 1342 RVA: 0x00012238 File Offset: 0x00010438
		[DataSourceProperty]
		public string ChangeBannerlordIDText
		{
			get
			{
				return this._changeBannerlordIDText;
			}
			set
			{
				if (value != this._changeBannerlordIDText)
				{
					this._changeBannerlordIDText = value;
					base.OnPropertyChanged("ChangeBannerlordIDText");
				}
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x0001225A File Offset: 0x0001045A
		// (set) Token: 0x06000540 RID: 1344 RVA: 0x00012262 File Offset: 0x00010462
		[DataSourceProperty]
		public string TypeYourNameText
		{
			get
			{
				return this._typeYourNameText;
			}
			set
			{
				if (value != this._typeYourNameText)
				{
					this._typeYourNameText = value;
					base.OnPropertyChanged("TypeYourNameText");
				}
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000541 RID: 1345 RVA: 0x00012284 File Offset: 0x00010484
		// (set) Token: 0x06000542 RID: 1346 RVA: 0x0001228C File Offset: 0x0001048C
		[DataSourceProperty]
		public string RequestSentText
		{
			get
			{
				return this._requestSentText;
			}
			set
			{
				if (value != this._requestSentText)
				{
					this._requestSentText = value;
					base.OnPropertyChanged("RequestSentText");
				}
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x000122AE File Offset: 0x000104AE
		// (set) Token: 0x06000544 RID: 1348 RVA: 0x000122B6 File Offset: 0x000104B6
		[DataSourceProperty]
		public bool HasRequestSent
		{
			get
			{
				return this._hasRequestSent;
			}
			set
			{
				if (value != this._hasRequestSent)
				{
					this._hasRequestSent = value;
					base.OnPropertyChanged("HasRequestSent");
				}
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x000122D3 File Offset: 0x000104D3
		// (set) Token: 0x06000546 RID: 1350 RVA: 0x000122DB File Offset: 0x000104DB
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

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x000122FD File Offset: 0x000104FD
		// (set) Token: 0x06000548 RID: 1352 RVA: 0x00012305 File Offset: 0x00010505
		[DataSourceProperty]
		public string CancelText
		{
			get
			{
				return this._cancelText;
			}
			set
			{
				if (value != this._cancelText)
				{
					this._cancelText = value;
					base.OnPropertyChanged("CancelText");
				}
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x00012327 File Offset: 0x00010527
		// (set) Token: 0x0600054A RID: 1354 RVA: 0x0001232F File Offset: 0x0001052F
		[DataSourceProperty]
		public string DoneText
		{
			get
			{
				return this._doneText;
			}
			set
			{
				if (value != this._doneText)
				{
					this._doneText = value;
					base.OnPropertyChanged("DoneText");
				}
			}
		}

		// Token: 0x0400027C RID: 636
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x0400027D RID: 637
		private InputKeyItemVM _doneInputKey;

		// Token: 0x0400027E RID: 638
		private bool _isSelected;

		// Token: 0x0400027F RID: 639
		private bool _hasRequestSent;

		// Token: 0x04000280 RID: 640
		private string _bannerlordIDInputText;

		// Token: 0x04000281 RID: 641
		private string _changeBannerlordIDText;

		// Token: 0x04000282 RID: 642
		private string _typeYourNameText;

		// Token: 0x04000283 RID: 643
		private string _requestSentText;

		// Token: 0x04000284 RID: 644
		private string _errorText;

		// Token: 0x04000285 RID: 645
		private string _cancelText;

		// Token: 0x04000286 RID: 646
		private string _doneText;
	}
}
