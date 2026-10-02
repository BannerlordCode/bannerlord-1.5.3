using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Authentication
{
	// Token: 0x02000076 RID: 118
	public class MPAuthenticationDebugVM : ViewModel
	{
		// Token: 0x06000BB7 RID: 2999 RVA: 0x0002339A File Offset: 0x0002159A
		public MPAuthenticationDebugVM()
		{
			this.RefreshValues();
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x000233A8 File Offset: 0x000215A8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=!}For Debug Purposes", null).ToString();
			this.UsernameText = new TextObject("{=!}Username:", null).ToString();
			this.PasswordText = new TextObject("{=!}Password:", null).ToString();
			this.LoginText = new TextObject("{=!}Login", null).ToString();
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00023414 File Offset: 0x00021614
		private async void ExecuteLogin()
		{
			LobbyState lobbyState = Game.Current.GameStateManager.ActiveState as LobbyState;
			this.IsLoginRequestActive = true;
			await lobbyState.TryLogin(this.Username, this.Password);
			this.IsLoginRequestActive = false;
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000BBA RID: 3002 RVA: 0x0002344D File Offset: 0x0002164D
		// (set) Token: 0x06000BBB RID: 3003 RVA: 0x00023455 File Offset: 0x00021655
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

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x00023473 File Offset: 0x00021673
		// (set) Token: 0x06000BBD RID: 3005 RVA: 0x0002347B File Offset: 0x0002167B
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
				}
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06000BBE RID: 3006 RVA: 0x00023499 File Offset: 0x00021699
		// (set) Token: 0x06000BBF RID: 3007 RVA: 0x000234A1 File Offset: 0x000216A1
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

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x000234C4 File Offset: 0x000216C4
		// (set) Token: 0x06000BC1 RID: 3009 RVA: 0x000234CC File Offset: 0x000216CC
		[DataSourceProperty]
		public string UsernameText
		{
			get
			{
				return this._usernameText;
			}
			set
			{
				if (value != this._usernameText)
				{
					this._usernameText = value;
					base.OnPropertyChangedWithValue<string>(value, "UsernameText");
				}
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x000234EF File Offset: 0x000216EF
		// (set) Token: 0x06000BC3 RID: 3011 RVA: 0x000234F7 File Offset: 0x000216F7
		[DataSourceProperty]
		public string PasswordText
		{
			get
			{
				return this._passwordText;
			}
			set
			{
				if (value != this._passwordText)
				{
					this._passwordText = value;
					base.OnPropertyChangedWithValue<string>(value, "PasswordText");
				}
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x0002351A File Offset: 0x0002171A
		// (set) Token: 0x06000BC5 RID: 3013 RVA: 0x00023522 File Offset: 0x00021722
		[DataSourceProperty]
		public string Username
		{
			get
			{
				return this._username;
			}
			set
			{
				if (value != this._username)
				{
					this._username = value;
					base.OnPropertyChangedWithValue<string>(value, "Username");
				}
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x00023545 File Offset: 0x00021745
		// (set) Token: 0x06000BC7 RID: 3015 RVA: 0x0002354D File Offset: 0x0002174D
		[DataSourceProperty]
		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				if (value != this._password)
				{
					this._password = value;
					base.OnPropertyChangedWithValue<string>(value, "Password");
				}
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x00023570 File Offset: 0x00021770
		// (set) Token: 0x06000BC9 RID: 3017 RVA: 0x00023578 File Offset: 0x00021778
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

		// Token: 0x0400054B RID: 1355
		private bool _isEnabled;

		// Token: 0x0400054C RID: 1356
		private bool _isLoginRequestActive;

		// Token: 0x0400054D RID: 1357
		private string _titleText;

		// Token: 0x0400054E RID: 1358
		private string _usernameText;

		// Token: 0x0400054F RID: 1359
		private string _passwordText;

		// Token: 0x04000550 RID: 1360
		private string _username;

		// Token: 0x04000551 RID: 1361
		private string _password;

		// Token: 0x04000552 RID: 1362
		private string _loginText;
	}
}
