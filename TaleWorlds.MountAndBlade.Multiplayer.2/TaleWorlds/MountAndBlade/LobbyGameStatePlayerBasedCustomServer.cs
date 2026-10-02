using System;
using System.Threading.Tasks;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200000E RID: 14
	public sealed class LobbyGameStatePlayerBasedCustomServer : LobbyGameState
	{
		// Token: 0x06000090 RID: 144 RVA: 0x00003DF1 File Offset: 0x00001FF1
		public void SetStartingParameters(LobbyGameClientHandler lobbyGameClientHandler)
		{
			this._gameClient = lobbyGameClientHandler.GameClient;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003DFF File Offset: 0x00001FFF
		protected override void OnActivate()
		{
			base.OnActivate();
			if (this._gameClient != null && (this._gameClient.AtLobby || !this._gameClient.Connected))
			{
				base.GameStateManager.PopState(0);
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003E35 File Offset: 0x00002035
		protected override void StartMultiplayer()
		{
			this.HandleServerStartMultiplayer();
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003E40 File Offset: 0x00002040
		private async void HandleServerStartMultiplayer()
		{
			GameNetwork.PreStartMultiplayerOnServer();
			BannerlordNetwork.StartMultiplayerLobbyMission(LobbyMissionType.Custom);
			if (!Module.CurrentModule.StartMultiplayerGame(this._gameClient.CustomGameType, this._gameClient.CustomGameScene))
			{
				Debug.FailedAssert("[DEBUG]Invalid multiplayer game type.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\LobbyGameState.cs", "HandleServerStartMultiplayer", 346);
			}
			while (Mission.Current == null || Mission.Current.CurrentState != Mission.State.Continuing)
			{
				await Task.Delay(1);
			}
			GameNetwork.StartMultiplayerOnServer(9999);
			if (this._gameClient.IsInGame)
			{
				BannerlordNetwork.CreateServerPeer();
				MBDebug.Print("Server: I finished loading and I am now visible to clients in the server list.", 0, Debug.DebugColor.White, 17179869184UL);
				if (!GameNetwork.IsDedicatedServer)
				{
					GameNetwork.ClientFinishedLoading(GameNetwork.MyPeer);
				}
			}
			IPlatformServices instance = PlatformServices.Instance;
			if (instance != null)
			{
				instance.CheckPrivilege(Privilege.Chat, false, delegate(bool result)
				{
					if (!result)
					{
						PlatformServices.Instance.ShowRestrictedInformation();
					}
				});
			}
		}

		// Token: 0x0400001C RID: 28
		private LobbyClient _gameClient;
	}
}
