using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200000D RID: 13
	public sealed class LobbyGameStateMatchmakerClient : LobbyGameState
	{
		// Token: 0x0600008C RID: 140 RVA: 0x00003CB4 File Offset: 0x00001EB4
		public void SetStartingParameters(LobbyGameClientHandler lobbyGameClientHandler, int playerIndex, int sessionKey, string address, int assignedPort, string multiplayerGameType, string scene)
		{
			this._lobbyGameClientHandler = lobbyGameClientHandler;
			this._gameClient = lobbyGameClientHandler.GameClient;
			this._playerIndex = playerIndex;
			this._sessionKey = sessionKey;
			this._address = address;
			this._assignedPort = assignedPort;
			this._multiplayerGameType = multiplayerGameType;
			this._scene = scene;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003D04 File Offset: 0x00001F04
		protected override void OnActivate()
		{
			base.OnActivate();
			if (this._gameClient != null && (this._gameClient.CurrentState == LobbyClient.State.AtLobby || this._gameClient.CurrentState == LobbyClient.State.QuittingFromBattle || !this._gameClient.Connected))
			{
				base.GameStateManager.PopState(0);
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003D58 File Offset: 0x00001F58
		protected override void StartMultiplayer()
		{
			GameNetwork.StartMultiplayerOnClient(this._address, this._assignedPort, this._sessionKey, this._playerIndex);
			BannerlordNetwork.StartMultiplayerLobbyMission(LobbyMissionType.Matchmaker);
			if (!Module.CurrentModule.StartMultiplayerGame(this._multiplayerGameType, this._scene))
			{
				Debug.FailedAssert("[DEBUG]Invalid multiplayer game type.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\LobbyGameState.cs", "StartMultiplayer", 301);
			}
			IPlatformServices instance = PlatformServices.Instance;
			if (instance == null)
			{
				return;
			}
			instance.CheckPrivilege(Privilege.Chat, true, delegate(bool result)
			{
				if (!result)
				{
					PlatformServices.Instance.ShowRestrictedInformation();
				}
			});
		}

		// Token: 0x04000014 RID: 20
		private LobbyClient _gameClient;

		// Token: 0x04000015 RID: 21
		private int _playerIndex;

		// Token: 0x04000016 RID: 22
		private int _sessionKey;

		// Token: 0x04000017 RID: 23
		private string _address;

		// Token: 0x04000018 RID: 24
		private int _assignedPort;

		// Token: 0x04000019 RID: 25
		private string _multiplayerGameType;

		// Token: 0x0400001A RID: 26
		private string _scene;

		// Token: 0x0400001B RID: 27
		private LobbyGameClientHandler _lobbyGameClientHandler;
	}
}
