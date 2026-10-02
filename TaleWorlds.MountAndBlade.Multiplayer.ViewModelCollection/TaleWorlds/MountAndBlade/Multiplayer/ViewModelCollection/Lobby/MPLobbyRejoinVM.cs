using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x02000030 RID: 48
	public class MPLobbyRejoinVM : ViewModel
	{
		// Token: 0x06000393 RID: 915 RVA: 0x0000CEA3 File Offset: 0x0000B0A3
		public MPLobbyRejoinVM(Action<MPLobbyVM.LobbyPage> onChangePageRequest)
		{
			this._onChangePageRequest = onChangePageRequest;
			this.RefreshValues();
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000CEB8 File Offset: 0x0000B0B8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=6zYeU0VO}Disconnected from a match", null).ToString();
			this.DescriptionText = new TextObject("{=1A1t1naG}You have left a ranked game in progress. Please reconnect to the game.", null).ToString();
			this.RejoinText = new TextObject("{=5gGyaTPL}Reconnect", null).ToString();
			this.FleeText = new TextObject("{=3sRdGQou}Leave", null).ToString();
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000CF24 File Offset: 0x0000B124
		private void ExecuteRejoin()
		{
			NetworkMain.GameClient.RejoinBattle();
			this.TitleText = new TextObject("{=N0DXasar}Reconnecting", null).ToString();
			this.DescriptionText = new TextObject("{=BZcFB1My}Please wait while you are reconnecting to the game", null).ToString();
			this.IsRejoining = true;
			Action onRejoinRequested = this.OnRejoinRequested;
			if (onRejoinRequested == null)
			{
				return;
			}
			onRejoinRequested();
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000CF7E File Offset: 0x0000B17E
		private void ExecuteFlee()
		{
			NetworkMain.GameClient.FleeBattle();
			Action<MPLobbyVM.LobbyPage> onChangePageRequest = this._onChangePageRequest;
			if (onChangePageRequest == null)
			{
				return;
			}
			onChangePageRequest(MPLobbyVM.LobbyPage.Home);
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000397 RID: 919 RVA: 0x0000CF9B File Offset: 0x0000B19B
		// (set) Token: 0x06000398 RID: 920 RVA: 0x0000CFA3 File Offset: 0x0000B1A3
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

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000399 RID: 921 RVA: 0x0000CFC1 File Offset: 0x0000B1C1
		// (set) Token: 0x0600039A RID: 922 RVA: 0x0000CFC9 File Offset: 0x0000B1C9
		[DataSourceProperty]
		public bool IsRejoining
		{
			get
			{
				return this._isRejoining;
			}
			set
			{
				if (value != this._isRejoining)
				{
					this._isRejoining = value;
					base.OnPropertyChangedWithValue(value, "IsRejoining");
				}
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600039B RID: 923 RVA: 0x0000CFE7 File Offset: 0x0000B1E7
		// (set) Token: 0x0600039C RID: 924 RVA: 0x0000CFEF File Offset: 0x0000B1EF
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

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x0600039D RID: 925 RVA: 0x0000D012 File Offset: 0x0000B212
		// (set) Token: 0x0600039E RID: 926 RVA: 0x0000D01A File Offset: 0x0000B21A
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600039F RID: 927 RVA: 0x0000D03D File Offset: 0x0000B23D
		// (set) Token: 0x060003A0 RID: 928 RVA: 0x0000D045 File Offset: 0x0000B245
		[DataSourceProperty]
		public string RejoinText
		{
			get
			{
				return this._rejoinText;
			}
			set
			{
				if (value != this._rejoinText)
				{
					this._rejoinText = value;
					base.OnPropertyChangedWithValue<string>(value, "RejoinText");
				}
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x0000D068 File Offset: 0x0000B268
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x0000D070 File Offset: 0x0000B270
		[DataSourceProperty]
		public string FleeText
		{
			get
			{
				return this._fleeText;
			}
			set
			{
				if (value != this._fleeText)
				{
					this._fleeText = value;
					base.OnPropertyChangedWithValue<string>(value, "FleeText");
				}
			}
		}

		// Token: 0x040001CD RID: 461
		private readonly Action<MPLobbyVM.LobbyPage> _onChangePageRequest;

		// Token: 0x040001CE RID: 462
		public Action OnRejoinRequested;

		// Token: 0x040001CF RID: 463
		private bool _isEnabled;

		// Token: 0x040001D0 RID: 464
		private bool _isRejoining;

		// Token: 0x040001D1 RID: 465
		private string _titleText;

		// Token: 0x040001D2 RID: 466
		private string _descriptionText;

		// Token: 0x040001D3 RID: 467
		private string _rejoinText;

		// Token: 0x040001D4 RID: 468
		private string _fleeText;
	}
}
