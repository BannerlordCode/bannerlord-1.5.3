using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200000C RID: 12
	public sealed class LobbyGameStateCommunityClient : LobbyGameState
	{
		// Token: 0x06000087 RID: 135 RVA: 0x00003BB1 File Offset: 0x00001DB1
		public void SetStartingParameters(CommunityClient communityClient, string address, int port, int peerIndex, int sessionKey)
		{
			this._communityClient = communityClient;
			this._address = address;
			this._port = port;
			this._peerIndex = peerIndex;
			this._sessionKey = sessionKey;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003BD8 File Offset: 0x00001DD8
		protected override void OnActivate()
		{
			base.OnActivate();
			if (this._communityClient != null && !this._communityClient.IsInGame)
			{
				base.GameStateManager.PopState(0);
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003C04 File Offset: 0x00001E04
		protected override void StartMultiplayer()
		{
			MBDebug.Print("COMMUNITY GAME SERVER ADDRESS: " + this._address, 0, Debug.DebugColor.White, 17592186044416UL);
			GameNetwork.StartMultiplayerOnClient(this._address, this._port, this._sessionKey, this._peerIndex);
			BannerlordNetwork.StartMultiplayerLobbyMission(LobbyMissionType.Community);
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

		// Token: 0x0600008A RID: 138 RVA: 0x00003C85 File Offset: 0x00001E85
		protected override void OnDisconnectedFromServer()
		{
			base.OnDisconnectedFromServer();
			if (Game.Current.GameStateManager.ActiveState == this)
			{
				base.GameStateManager.PopState(0);
			}
		}

		// Token: 0x0400000F RID: 15
		private CommunityClient _communityClient;

		// Token: 0x04000010 RID: 16
		private string _address;

		// Token: 0x04000011 RID: 17
		private int _port;

		// Token: 0x04000012 RID: 18
		private int _peerIndex;

		// Token: 0x04000013 RID: 19
		private int _sessionKey;
	}
}
