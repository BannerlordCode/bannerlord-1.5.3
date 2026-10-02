using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200018C RID: 396
	public class TeamAiMultiplayerSiegeDefender : TeamAISiegeComponent
	{
		// Token: 0x06001543 RID: 5443 RVA: 0x0004E5E5 File Offset: 0x0004C7E5
		public TeamAiMultiplayerSiegeDefender(Mission currentMission, Team currentTeam, float thinkTimerTime, float applyTimerTime)
			: base(currentMission, currentTeam, thinkTimerTime, applyTimerTime)
		{
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x0004E5F2 File Offset: 0x0004C7F2
		public override void OnUnitAddedToFormationForTheFirstTime(Formation formation)
		{
		}
	}
}
