using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200018B RID: 395
	public class TeamAiMultiplayerSiegeAttacker : TeamAISiegeComponent
	{
		// Token: 0x06001541 RID: 5441 RVA: 0x0004E5D6 File Offset: 0x0004C7D6
		public TeamAiMultiplayerSiegeAttacker(Mission currentMission, Team currentTeam, float thinkTimerTime, float applyTimerTime)
			: base(currentMission, currentTeam, thinkTimerTime, applyTimerTime)
		{
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x0004E5E3 File Offset: 0x0004C7E3
		public override void OnUnitAddedToFormationForTheFirstTime(Formation formation)
		{
		}
	}
}
