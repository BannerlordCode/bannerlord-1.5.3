using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x02000059 RID: 89
	public class MissionBasedMultiplayerGameMode : MultiplayerGameMode
	{
		// Token: 0x060002D0 RID: 720 RVA: 0x0000C136 File Offset: 0x0000A336
		public MissionBasedMultiplayerGameMode(string name)
			: base(name)
		{
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000C140 File Offset: 0x0000A340
		public override void JoinCustomGame(JoinGameData joinGameData)
		{
			LobbyGameStateCustomGameClient lobbyGameStateCustomGameClient = Game.Current.GameStateManager.CreateState<LobbyGameStateCustomGameClient>();
			lobbyGameStateCustomGameClient.SetStartingParameters(NetworkMain.GameClient, joinGameData.GameServerProperties.Address, joinGameData.GameServerProperties.Port, joinGameData.PeerIndex, joinGameData.SessionKey);
			Game.Current.GameStateManager.PushState(lobbyGameStateCustomGameClient, 0);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000C19C File Offset: 0x0000A39C
		public override void StartMultiplayerGame(string scene)
		{
			if (Mission.Current != null)
			{
				Debug.FailedAssert("Starting multiplayer game while a mission is ongoing", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\MissionBasedMultiplayerGameMode.cs", "StartMultiplayerGame", 31);
			}
			GameStateManager gameStateManager = GameStateManager.Current;
			if (((gameStateManager != null) ? gameStateManager.ActiveState : null) is MissionState)
			{
				Debug.FailedAssert("Starting multiplayer game while in mission state", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\MissionBasedMultiplayerGameMode.cs", "StartMultiplayerGame", 37);
			}
			if (base.Name == "TeamDeathmatch")
			{
				MultiplayerMissions.OpenTeamDeathmatchMission(scene);
				return;
			}
			if (base.Name == "Duel")
			{
				MultiplayerMissions.OpenDuelMission(scene);
				return;
			}
			if (base.Name == "Siege")
			{
				MultiplayerMissions.OpenSiegeMission(scene);
				return;
			}
			if (base.Name == "Battle")
			{
				MultiplayerMissions.OpenBattleMission(scene);
				return;
			}
			if (base.Name == "Captain")
			{
				MultiplayerMissions.OpenCaptainMission(scene);
				return;
			}
			if (base.Name == "Skirmish")
			{
				MultiplayerMissions.OpenSkirmishMission(scene);
			}
		}
	}
}
