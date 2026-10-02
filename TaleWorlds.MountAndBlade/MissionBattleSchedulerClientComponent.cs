using System;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002AC RID: 684
	public class MissionBattleSchedulerClientComponent : MissionLobbyComponent
	{
		// Token: 0x060025F0 RID: 9712 RVA: 0x0008993B File Offset: 0x00087B3B
		public override void QuitMission()
		{
			base.QuitMission();
			if (base.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending && NetworkMain.GameClient.LoggedIn && NetworkMain.GameClient.CurrentState == LobbyClient.State.AtBattle)
			{
				NetworkMain.GameClient.QuitFromMatchmakerGame();
			}
		}
	}
}
