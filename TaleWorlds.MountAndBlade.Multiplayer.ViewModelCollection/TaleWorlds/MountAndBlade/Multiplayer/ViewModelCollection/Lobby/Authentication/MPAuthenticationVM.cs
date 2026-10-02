using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Authentication
{
	// Token: 0x02000077 RID: 119
	public class MPAuthenticationVM : ViewModel
	{
		// Token: 0x06000BCA RID: 3018 RVA: 0x0002359C File Offset: 0x0002179C
		public MPAuthenticationVM(LobbyState lobbyState)
		{
			this._lobbyState = lobbyState;
			this._hasPrivilege = this._lobbyState.HasMultiplayerPrivilege;
			LobbyState lobbyState2 = this._lobbyState;
			lobbyState2.OnMultiplayerPrivilegeUpdated = (Action<bool>)Delegate.Combine(lobbyState2.OnMultiplayerPrivilegeUpdated, new Action<bool>(this.OnMultiplayerPrivilegeUpdated));
			InternetAvailabilityChecker.OnInternetConnectionAvailabilityChanged = (Action<bool>)Delegate.Combine(InternetAvailabilityChecker.OnInternetConnectionAvailabilityChanged, new Action<bool>(this.OnInternetConnectionAvailabilityChanged));
			this.AuthenticationDebug = new MPAuthenticationDebugVM();
			this.AuthenticationDebug.IsEnabled = this.IsDebugAuthenticationEnabled();
			this.RefreshValues();
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x00023688 File Offset: 0x00021888
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ExitText = new TextObject("{=exitMenuOption}Exit", null).ToString();
			this.LoginText = new TextObject("{=lugGPVOb}Login", null).ToString();
			this.TitleText = this._idleTitle.ToString();
			this.MessageText = this._idleMessage.ToString();
			this.CommunityGamesText = new TextObject("{=SIIgjILk}Community Games", null).ToString();
			this.AuthenticationDebug.RefreshValues();
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x0002370C File Offset: 0x0002190C
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			LobbyState lobbyState = this._lobbyState;
			lobbyState.OnMultiplayerPrivilegeUpdated = (Action<bool>)Delegate.Remove(lobbyState.OnMultiplayerPrivilegeUpdated, new Action<bool>(this.OnMultiplayerPrivilegeUpdated));
			InternetAvailabilityChecker.OnInternetConnectionAvailabilityChanged = (Action<bool>)Delegate.Remove(InternetAvailabilityChecker.OnInternetConnectionAvailabilityChanged, new Action<bool>(this.OnInternetConnectionAvailabilityChanged));
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x00023788 File Offset: 0x00021988
		private bool IsDebugAuthenticationEnabled()
		{
			return Utilities.CommandLineArgumentExists("/forceMultiplayerDebugLogin") || (MBCommon.IsDebugMode && Module.CurrentModule.StartupInfo.StartupType != GameStartupType.Multiplayer);
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x000237B8 File Offset: 0x000219B8
		public void OnTick(float dt)
		{
			if (!this.IsEnabled || this._lobbyState == null)
			{
				return;
			}
			LobbyClient.State currentState = NetworkMain.GameClient.CurrentState;
			if (currentState != LobbyClient.State.Working && currentState != LobbyClient.State.Connected)
			{
				bool flag = currentState == LobbyClient.State.SessionRequested;
			}
			if (this._hasPrivilege != null && !this._hasPrivilege.Value)
			{
				this.TitleText = this._idleTitle.ToString();
				this.MessageText = this._noAccessMessage.ToString();
				return;
			}
			if (this._lobbyState.IsLoggingIn)
			{
				this.IsLoginRequestActive = true;
				this.TitleText = this._loggingInTitle.ToString();
				this.MessageText = this._loggingInMessage.ToString();
				return;
			}
			this.IsLoginRequestActive = false;
			this.TitleText = this._idleTitle.ToString();
			this.MessageText = this._idleMessage.ToString();
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00023890 File Offset: 0x00021A90
		public void ExecuteExit()
		{
			LobbyClient.State currentState = NetworkMain.GameClient.CurrentState;
			if (currentState == LobbyClient.State.Idle)
			{
				InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_exit", null).ToString(), GameTexts.FindText("str_mp_exit_query", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
				{
					this.OnExit();
				}, null, "", 0f, null, null, null), false, false);
				return;
			}
			TextObject textObject = MPAuthenticationVM.CantLogoutSearchingForMatchTextObject;
			if (currentState == LobbyClient.State.Working || currentState == LobbyClient.State.Connected || currentState == LobbyClient.State.SessionRequested)
			{
				textObject = MPAuthenticationVM.CantLogoutLoggingInTextObject;
			}
			InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_exit", null).ToString(), textObject.ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), null, null, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00023970 File Offset: 0x00021B70
		private void OnExit()
		{
			LobbyState lobbyState = this._lobbyState;
			if (lobbyState == null || !lobbyState.IsLoggingIn)
			{
				if (Module.CurrentModule.StartupInfo.StartupType == GameStartupType.Multiplayer)
				{
					MBInitialScreenBase.DoExitButtonAction();
					return;
				}
				Game.Current.GameStateManager.PopState(0);
			}
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x000239BC File Offset: 0x00021BBC
		public async void ExecuteLogin()
		{
			LobbyState lobbyState = this._lobbyState;
			if (lobbyState == null || !lobbyState.IsLoggingIn)
			{
				try
				{
					await this._lobbyState.TryLogin();
				}
				catch (Exception ex)
				{
					Debug.Print(ex.StackTrace ?? "", 0, Debug.DebugColor.White, 17592186044416UL);
				}
			}
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x000239F5 File Offset: 0x00021BF5
		private void OnMultiplayerPrivilegeUpdated(bool hasPrivilege)
		{
			this._hasPrivilege = new bool?(hasPrivilege);
			this.UpdateCanTryLogin();
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00023A09 File Offset: 0x00021C09
		private void OnInternetConnectionAvailabilityChanged(bool isInternetAvailable)
		{
			this.UpdatePrivilegeInformation();
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x00023A11 File Offset: 0x00021C11
		private void UpdateCanTryLogin()
		{
			this.CanTryLogin = this._hasPrivilege.GetValueOrDefault() && !this._lobbyState.IsLoggingIn;
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x00023A37 File Offset: 0x00021C37
		private void UpdatePrivilegeInformation()
		{
			LobbyState lobbyState = this._lobbyState;
			if (lobbyState == null)
			{
				return;
			}
			lobbyState.UpdateHasMultiplayerPrivilege();
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000BD6 RID: 3030 RVA: 0x00023A4A File Offset: 0x00021C4A
		// (set) Token: 0x06000BD7 RID: 3031 RVA: 0x00023A52 File Offset: 0x00021C52
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

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x00023A70 File Offset: 0x00021C70
		// (set) Token: 0x06000BD9 RID: 3033 RVA: 0x00023A78 File Offset: 0x00021C78
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

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000BDA RID: 3034 RVA: 0x00023A96 File Offset: 0x00021C96
		// (set) Token: 0x06000BDB RID: 3035 RVA: 0x00023A9E File Offset: 0x00021C9E
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
					if (this.IsEnabled)
					{
						this.UpdatePrivilegeInformation();
					}
				}
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06000BDC RID: 3036 RVA: 0x00023ACA File Offset: 0x00021CCA
		// (set) Token: 0x06000BDD RID: 3037 RVA: 0x00023AD2 File Offset: 0x00021CD2
		[DataSourceProperty]
		public bool IsLoginRequestActive
		{
			get
			{
				return this._isLoginRequestActive;
			}
			set
			{
				if (value != this._isLoginRequestActive)
				{
					this._isLoginRequestActive = value;
					base.OnPropertyChangedWithValue(value, "IsLoginRequestActive");
					this.UpdateCanTryLogin();
				}
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x00023AF6 File Offset: 0x00021CF6
		// (set) Token: 0x06000BDF RID: 3039 RVA: 0x00023AFE File Offset: 0x00021CFE
		[DataSourceProperty]
		public bool CanTryLogin
		{
			get
			{
				return this._canTryLogin;
			}
			set
			{
				if (value != this._canTryLogin)
				{
					this._canTryLogin = value;
					base.OnPropertyChangedWithValue(value, "CanTryLogin");
				}
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000BE0 RID: 3040 RVA: 0x00023B1C File Offset: 0x00021D1C
		// (set) Token: 0x06000BE1 RID: 3041 RVA: 0x00023B24 File Offset: 0x00021D24
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

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x00023B47 File Offset: 0x00021D47
		// (set) Token: 0x06000BE3 RID: 3043 RVA: 0x00023B4F File Offset: 0x00021D4F
		[DataSourceProperty]
		public string MessageText
		{
			get
			{
				return this._messageText;
			}
			set
			{
				if (value != this._messageText)
				{
					this._messageText = value;
					base.OnPropertyChangedWithValue<string>(value, "MessageText");
				}
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x00023B72 File Offset: 0x00021D72
		// (set) Token: 0x06000BE5 RID: 3045 RVA: 0x00023B7A File Offset: 0x00021D7A
		[DataSourceProperty]
		public string ExitText
		{
			get
			{
				return this._exitText;
			}
			set
			{
				if (value != this._exitText)
				{
					this._exitText = value;
					base.OnPropertyChangedWithValue<string>(value, "ExitText");
				}
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x00023B9D File Offset: 0x00021D9D
		// (set) Token: 0x06000BE7 RID: 3047 RVA: 0x00023BA5 File Offset: 0x00021DA5
		[DataSourceProperty]
		public string LoginText
		{
			get
			{
				return this._loginText;
			}
			set
			{
				if (value != this._loginText)
				{
					this._loginText = value;
					base.OnPropertyChangedWithValue<string>(value, "LoginText");
				}
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x00023BC8 File Offset: 0x00021DC8
		// (set) Token: 0x06000BE9 RID: 3049 RVA: 0x00023BD0 File Offset: 0x00021DD0
		[DataSourceProperty]
		public string CommunityGamesText
		{
			get
			{
				return this._communityGamesText;
			}
			set
			{
				if (value != this._communityGamesText)
				{
					this._communityGamesText = value;
					base.OnPropertyChangedWithValue<string>(value, "CommunityGamesText");
				}
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x00023BF3 File Offset: 0x00021DF3
		// (set) Token: 0x06000BEB RID: 3051 RVA: 0x00023BFB File Offset: 0x00021DFB
		[DataSourceProperty]
		public MPAuthenticationDebugVM AuthenticationDebug
		{
			get
			{
				return this._authenticationDebug;
			}
			set
			{
				if (value != this._authenticationDebug)
				{
					this._authenticationDebug = value;
					base.OnPropertyChangedWithValue<MPAuthenticationDebugVM>(value, "AuthenticationDebug");
				}
			}
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00023C19 File Offset: 0x00021E19
		public void SetDoneInputKey(HotKey hotkey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00023C28 File Offset: 0x00021E28
		public void SetCancelInputKey(HotKey hotkey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x04000553 RID: 1363
		private readonly LobbyState _lobbyState;

		// Token: 0x04000554 RID: 1364
		private bool? _hasPrivilege;

		// Token: 0x04000555 RID: 1365
		private readonly TextObject _idleTitle = new TextObject("{=g1lgiwn1}Not Logged In", null);

		// Token: 0x04000556 RID: 1366
		private readonly TextObject _idleMessage = new TextObject("{=saZ1OvPt}You can press the login button to establish connection", null);

		// Token: 0x04000557 RID: 1367
		private readonly TextObject _noAccessMessage = new TextObject("{=9P0VL49j}You don't have access to multiplayer.", null);

		// Token: 0x04000558 RID: 1368
		private readonly TextObject _loggingInTitle = new TextObject("{=iNqucBor}Logging In", null);

		// Token: 0x04000559 RID: 1369
		private readonly TextObject _loggingInMessage = new TextObject("{=U4dzbzNb}Please wait while you are being connected to the server", null);

		// Token: 0x0400055A RID: 1370
		private static readonly TextObject CantLogoutLoggingInTextObject = new TextObject("{=E0q43haK}Please wait until you are logged in.", null);

		// Token: 0x0400055B RID: 1371
		private static readonly TextObject CantLogoutSearchingForMatchTextObject = new TextObject("{=DyeaObj5}Please cancel game search request before logging out.", null);

		// Token: 0x0400055C RID: 1372
		private bool _isEnabled;

		// Token: 0x0400055D RID: 1373
		private InputKeyItemVM _doneInputKey;

		// Token: 0x0400055E RID: 1374
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x0400055F RID: 1375
		private bool _isLoginRequestActive;

		// Token: 0x04000560 RID: 1376
		private bool _canTryLogin;

		// Token: 0x04000561 RID: 1377
		private string _titleText;

		// Token: 0x04000562 RID: 1378
		private string _messageText;

		// Token: 0x04000563 RID: 1379
		private string _exitText;

		// Token: 0x04000564 RID: 1380
		private string _loginText;

		// Token: 0x04000565 RID: 1381
		private string _communityGamesText;

		// Token: 0x04000566 RID: 1382
		private MPAuthenticationDebugVM _authenticationDebug;
	}
}
