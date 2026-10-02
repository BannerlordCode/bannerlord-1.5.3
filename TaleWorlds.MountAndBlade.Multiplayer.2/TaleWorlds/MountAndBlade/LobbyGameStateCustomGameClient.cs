using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200000B RID: 11
	public sealed class LobbyGameStateCustomGameClient : LobbyGameState
	{
		// Token: 0x06000081 RID: 129 RVA: 0x00003A4D File Offset: 0x00001C4D
		public void SetStartingParameters(LobbyClient gameClient, string address, int port, int peerIndex, int sessionKey)
		{
			this._gameClient = gameClient;
			this._address = address;
			this._port = port;
			this._peerIndex = peerIndex;
			this._sessionKey = sessionKey;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003A74 File Offset: 0x00001C74
		protected override void OnActivate()
		{
			base.OnActivate();
			this._inactivityTimer = new Timer(MBCommon.GetApplicationTime(), LobbyGameStateCustomGameClient.InactivityThreshold, true);
			if (this._gameClient != null && (this._gameClient.AtLobby || !this._gameClient.Connected))
			{
				base.GameStateManager.PopState(0);
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003ACC File Offset: 0x00001CCC
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (GameNetwork.IsClient && this._inactivityTimer.Check(MBCommon.GetApplicationTime()) && this._gameClient != null)
			{
				this._gameClient.IsInCriticalState = GameNetwork.ElapsedTimeSinceLastUdpPacketArrived() > (double)LobbyGameStateCustomGameClient.InactivityThreshold;
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003B1C File Offset: 0x00001D1C
		protected override void StartMultiplayer()
		{
			MBDebug.Print("CUSTOM GAME SERVER ADDRESS: " + this._address, 0, Debug.DebugColor.White, 17592186044416UL);
			GameNetwork.StartMultiplayerOnClient(this._address, this._port, this._sessionKey, this._peerIndex);
			BannerlordNetwork.StartMultiplayerLobbyMission(LobbyMissionType.Custom);
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

		// Token: 0x04000008 RID: 8
		private LobbyClient _gameClient;

		// Token: 0x04000009 RID: 9
		private string _address;

		// Token: 0x0400000A RID: 10
		private int _port;

		// Token: 0x0400000B RID: 11
		private int _peerIndex;

		// Token: 0x0400000C RID: 12
		private int _sessionKey;

		// Token: 0x0400000D RID: 13
		private Timer _inactivityTimer;

		// Token: 0x0400000E RID: 14
		private static readonly float InactivityThreshold = 2f;
	}
}
