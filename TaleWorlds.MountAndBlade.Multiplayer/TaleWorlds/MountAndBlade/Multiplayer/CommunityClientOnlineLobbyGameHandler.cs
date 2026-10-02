using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x02000053 RID: 83
	public class CommunityClientOnlineLobbyGameHandler : ICommunityClientHandler
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000BC38 File Offset: 0x00009E38
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x0000BC40 File Offset: 0x00009E40
		public LobbyState LobbyState { get; private set; }

		// Token: 0x060002B1 RID: 689 RVA: 0x0000BC49 File Offset: 0x00009E49
		public CommunityClientOnlineLobbyGameHandler(LobbyState lobbyState)
		{
			this.LobbyState = lobbyState;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000BC58 File Offset: 0x00009E58
		void ICommunityClientHandler.OnQuitFromGame()
		{
			if (Game.Current != null)
			{
				GameStateManager gameStateManager = Game.Current.GameStateManager;
				if (!(gameStateManager.ActiveState is LobbyState))
				{
					if (Game.Current.GameStateManager.ActiveState is MissionState)
					{
						BannerlordNetwork.EndMultiplayerLobbyMission();
						return;
					}
					gameStateManager.PopState(0);
				}
			}
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000BCA8 File Offset: 0x00009EA8
		void ICommunityClientHandler.OnJoinCustomGameResponse(string address, int port, PlayerJoinGameResponseDataFromHost response)
		{
			if (Game.Current != null)
			{
				GameStateManager gameStateManager = Game.Current.GameStateManager;
				if (response != null)
				{
					LobbyGameStateCommunityClient lobbyGameStateCommunityClient = Game.Current.GameStateManager.CreateState<LobbyGameStateCommunityClient>();
					lobbyGameStateCommunityClient.SetStartingParameters(NetworkMain.CommunityClient, address, port, response.PeerIndex, response.SessionKey);
					Game.Current.GameStateManager.PushState(lobbyGameStateCommunityClient, 0);
					Debug.Print("Join game successful", 0, Debug.DebugColor.Green, 17592186044416UL);
				}
			}
		}
	}
}
