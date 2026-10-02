using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x02000065 RID: 101
	public class MultiplayerSubModule : MBSubModuleBase
	{
		// Token: 0x060002FF RID: 767 RVA: 0x0000DB48 File Offset: 0x0000BD48
		protected internal override void OnSubModuleLoad()
		{
			base.OnSubModuleLoad();
			Module.CurrentModule.AddMultiplayerGameMode(new MissionBasedMultiplayerGameMode("TeamDeathmatch"));
			Module.CurrentModule.AddMultiplayerGameMode(new MissionBasedMultiplayerGameMode("Duel"));
			Module.CurrentModule.AddMultiplayerGameMode(new MissionBasedMultiplayerGameMode("Siege"));
			Module.CurrentModule.AddMultiplayerGameMode(new MissionBasedMultiplayerGameMode("Captain"));
			Module.CurrentModule.AddMultiplayerGameMode(new MissionBasedMultiplayerGameMode("Skirmish"));
			Module.CurrentModule.AddMultiplayerGameMode(new MissionBasedMultiplayerGameMode("Battle"));
			TextObject coreContentDisabledReason = new TextObject("{=V8BXjyYq}Disabled during installation.", null);
			if (Module.CurrentModule.StartupInfo.StartupType != GameStartupType.Singleplayer)
			{
				Module.CurrentModule.AddInitialStateOption(new InitialStateOption("Multiplayer", new TextObject("{=YDYnuBmC}Multiplayer", null), 9997, new Action(this.StartMultiplayer), () => new ValueTuple<bool, TextObject>(Module.CurrentModule.IsOnlyCoreContentEnabled, coreContentDisabledReason), null, null));
			}
			TauntUsageManager.Initialize();
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000DC40 File Offset: 0x0000BE40
		public override void OnGameLoaded(Game game, object initializerObject)
		{
			base.OnGameLoaded(game, initializerObject);
			MultiplayerMain.Initialize(new GameNetworkHandler());
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000DC54 File Offset: 0x0000BE54
		protected internal override void OnApplicationTick(float dt)
		{
			base.OnApplicationTick(dt);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000DC5D File Offset: 0x0000BE5D
		protected internal override void OnBeforeInitialModuleScreenSetAsRoot()
		{
			base.OnBeforeInitialModuleScreenSetAsRoot();
			if (GameNetwork.IsDedicatedServer)
			{
				MBGameManager.StartNewGame(new MultiplayerGameManager());
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000DC78 File Offset: 0x0000BE78
		public override void OnInitialState()
		{
			base.OnInitialState();
			if (Utilities.CommandLineArgumentExists("+connect_lobby"))
			{
				MBGameManager.StartNewGame(new MultiplayerGameManager());
				return;
			}
			if (!Module.CurrentModule.IsOnlyCoreContentEnabled && Module.CurrentModule.MultiplayerRequested)
			{
				MBGameManager.StartNewGame(new MultiplayerGameManager());
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000DCC4 File Offset: 0x0000BEC4
		private async void StartMultiplayer()
		{
			if (!this._isConnectingToMultiplayer)
			{
				this._isConnectingToMultiplayer = true;
				bool flag = NetworkMain.GameClient != null && await NetworkMain.GameClient.CheckConnection();
				bool isConnected = flag;
				PlatformServices.Instance.CheckPrivilege(Privilege.Multiplayer, true, delegate(bool result)
				{
					if (!isConnected || !result)
					{
						string text = new TextObject("{=ksq1IBh3}No connection", null).ToString();
						string text2 = new TextObject("{=5VIbo2Cb}No connection could be established to the lobby server. Check your internet connection and try again.", null).ToString();
						InformationManager.ShowInquiry(new InquiryData(text, text2, false, true, "", new TextObject("{=dismissnotification}Dismiss", null).ToString(), null, delegate
						{
							InformationManager.HideInquiry();
						}, "", 0f, null, null, null), false, false);
						return;
					}
					MBGameManager.StartNewGame(new MultiplayerGameManager());
				});
				this._isConnectingToMultiplayer = false;
			}
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000DCFD File Offset: 0x0000BEFD
		protected internal override void OnNetworkTick(float dt)
		{
			base.OnNetworkTick(dt);
			MultiplayerMain.Tick(dt);
			InternetAvailabilityChecker.Tick(dt);
		}

		// Token: 0x040000F2 RID: 242
		private bool _isConnectingToMultiplayer;
	}
}
