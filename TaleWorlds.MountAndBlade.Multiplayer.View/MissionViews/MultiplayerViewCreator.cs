using System;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews
{
	// Token: 0x0200001D RID: 29
	public static class MultiplayerViewCreator
	{
		// Token: 0x06000035 RID: 53 RVA: 0x00002F07 File Offset: 0x00001107
		public static MissionView CreateMissionMultiplayerPreloadView(Mission mission = null)
		{
			return ViewCreatorManager.CreateMissionView<MissionMultiplayerPreloadView>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002F18 File Offset: 0x00001118
		public static MissionView CreateMissionScoreBoardUIHandler(Mission mission, bool isSingleTeam)
		{
			return ViewCreatorManager.CreateMissionView<MissionScoreboardUIHandler>(mission != null, mission, new object[] { isSingleTeam });
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002F33 File Offset: 0x00001133
		public static MissionView CreateMultiplayerEndOfRoundUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MultiplayerEndOfRoundUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002F41 File Offset: 0x00001141
		public static MissionView CreateMultiplayerTeamSelectUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MultiplayerTeamSelectUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002F4F File Offset: 0x0000114F
		public static MissionView CreateMultiplayerCultureSelectUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MultiplayerCultureSelectUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002F5D File Offset: 0x0000115D
		public static MissionView CreateLobbyEquipmentUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MissionLobbyEquipmentUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002F6B File Offset: 0x0000116B
		public static MissionView CreatePollProgressUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MultiplayerPollProgressUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002F79 File Offset: 0x00001179
		public static MissionView CreateMissionMultiplayerEscapeMenu(string gameType)
		{
			return ViewCreatorManager.CreateMissionView<MissionMultiplayerEscapeMenu>(false, null, new object[] { gameType });
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002F8C File Offset: 0x0000118C
		public static MissionView CreateMissionMultiplayerPracticeEscapeMenu()
		{
			return ViewCreatorManager.CreateMissionView<MissionMultiplayerPracticeEscapeMenu>(false, null, Array.Empty<object>());
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002F9A File Offset: 0x0000119A
		public static MissionView CreateMissionKillNotificationUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MissionMultiplayerKillNotificationUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002FA8 File Offset: 0x000011A8
		public static MissionView CreateMissionServerStatusUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MissionMultiplayerServerStatusUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002FB6 File Offset: 0x000011B6
		public static MissionView CreateMultiplayerAdminPanelUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MultiplayerAdminPanelUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002FC4 File Offset: 0x000011C4
		public static MissionView CreateMultiplayerFactionBanVoteUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MultiplayerFactionBanVoteUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002FD2 File Offset: 0x000011D2
		public static MissionView CreateMultiplayerMissionHUDExtensionUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MissionMultiplayerHUDExtensionUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002FE0 File Offset: 0x000011E0
		public static MissionView CreateMultiplayerMissionVoiceChatUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MissionMultiplayerVoiceChatUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002FEE File Offset: 0x000011EE
		public static MissionView CreateMultiplayerMissionOrderUIHandler(Mission mission = null)
		{
			return ViewCreatorManager.CreateMissionView<MultiplayerMissionOrderUIHandler>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002FFF File Offset: 0x000011FF
		public static MissionView CreateMultiplayerMissionDeathCardUIHandler(Mission mission = null)
		{
			return ViewCreatorManager.CreateMissionView<MissionMultiplayerDeathCardUIHandler>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003010 File Offset: 0x00001210
		public static MissionView CreateMissionMultiplayerDuelUI()
		{
			return ViewCreatorManager.CreateMissionView<MissionMultiplayerDuelUI>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000301E File Offset: 0x0000121E
		public static MissionView CreateMultiplayerEndOfBattleUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MultiplayerEndOfBattleUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000302C File Offset: 0x0000122C
		public static MissionView CreateMissionFlagMarkerUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MissionMultiplayerMarkerUIHandler>(false, null, Array.Empty<object>());
		}
	}
}
