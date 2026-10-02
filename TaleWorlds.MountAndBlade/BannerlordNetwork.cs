using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002EA RID: 746
	public static class BannerlordNetwork
	{
		// Token: 0x06002B5F RID: 11103 RVA: 0x000A76BC File Offset: 0x000A58BC
		private static PlayerConnectionInfo CreateServerPeerConnectionInfo()
		{
			LobbyClient gameClient = NetworkMain.GameClient;
			PlayerConnectionInfo playerConnectionInfo = new PlayerConnectionInfo(gameClient.PlayerID);
			PlayerData playerData = gameClient.PlayerData;
			playerConnectionInfo.AddParameter("PlayerData", playerData);
			playerConnectionInfo.AddParameter("UsedCosmetics", gameClient.UsedCosmetics);
			playerConnectionInfo.Name = gameClient.Name;
			return playerConnectionInfo;
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x000A770A File Offset: 0x000A590A
		public static void CreateServerPeer()
		{
			if (MBCommon.CurrentGameType == MBCommon.GameType.MultiClientServer)
			{
				GameNetwork.AddNewPlayerOnServer(BannerlordNetwork.CreateServerPeerConnectionInfo(), true, true, false);
			}
		}

		// Token: 0x06002B61 RID: 11105 RVA: 0x000A7722 File Offset: 0x000A5922
		public static void StartMultiplayerLobbyMission(LobbyMissionType lobbyMissionType)
		{
			BannerlordNetwork.LobbyMissionType = lobbyMissionType;
		}

		// Token: 0x06002B62 RID: 11106 RVA: 0x000A772C File Offset: 0x000A592C
		public static void EndMultiplayerLobbyMission()
		{
			MissionState missionState = Game.Current.GameStateManager.ActiveState as MissionState;
			if (missionState != null && missionState.CurrentMission != null && !missionState.CurrentMission.MissionEnded)
			{
				if (missionState.CurrentMission.CurrentState != Mission.State.Continuing)
				{
					Debug.Print("Remove From Game: Begin delayed disconnect from server.".ToUpper(), 0, Debug.DebugColor.White, 17179869184UL);
					missionState.BeginDelayedDisconnectFromMission();
				}
				else
				{
					Debug.Print("Remove From Game: Begin instant disconnect from server.".ToUpper(), 0, Debug.DebugColor.White, 17179869184UL);
					missionState.CurrentMission.EndMission();
				}
				MBDebug.Print("Starting to clean up the current mission now.", 0, Debug.DebugColor.White, 17179869184UL);
			}
			ChatBox gameHandler = Game.Current.GetGameHandler<ChatBox>();
			if (gameHandler != null)
			{
				gameHandler.ResetMuteList();
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06002B63 RID: 11107 RVA: 0x000A77EA File Offset: 0x000A59EA
		// (set) Token: 0x06002B64 RID: 11108 RVA: 0x000A77F1 File Offset: 0x000A59F1
		public static LobbyMissionType LobbyMissionType { get; private set; }

		// Token: 0x0400107B RID: 4219
		public const int DefaultPort = 9999;
	}
}
