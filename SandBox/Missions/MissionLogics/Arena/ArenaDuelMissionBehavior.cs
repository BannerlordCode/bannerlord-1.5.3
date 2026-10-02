using System;
using SandBox.Tournaments.MissionLogics;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics.Arena
{
	// Token: 0x0200009B RID: 155
	public class ArenaDuelMissionBehavior : MissionLogic
	{
		// Token: 0x06000667 RID: 1639 RVA: 0x0002B282 File Offset: 0x00029482
		public override void AfterStart()
		{
			TournamentBehavior.DeleteTournamentSetsExcept(base.Mission.Scene.FindEntityWithTag("tournament_fight"));
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0002B29E File Offset: 0x0002949E
		public override void OnAfterMissionLoadingFinished()
		{
			base.Mission.OnInitialSpawnCompleted(BattleSideEnum.None);
		}
	}
}
