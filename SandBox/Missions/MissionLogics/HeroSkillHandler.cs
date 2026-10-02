using System;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x0200006B RID: 107
	public class HeroSkillHandler : MissionLogic
	{
		// Token: 0x06000467 RID: 1127 RVA: 0x0001AA34 File Offset: 0x00018C34
		public override void AfterStart()
		{
			this._nextCaptainSkillMoraleBoostTime = MissionTime.SecondsFromNow(10f);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0001AA48 File Offset: 0x00018C48
		public override void OnMissionTick(float dt)
		{
			if (this._nextCaptainSkillMoraleBoostTime.IsPast)
			{
				this._boostMorale = true;
				this._nextMoraleTeam = 0;
				this._nextCaptainSkillMoraleBoostTime = MissionTime.SecondsFromNow(10f);
			}
			if (this._boostMorale)
			{
				if (this._nextMoraleTeam >= base.Mission.Teams.Count)
				{
					this._boostMorale = false;
					return;
				}
				Team team = base.Mission.Teams[this._nextMoraleTeam];
				this.BoostMoraleForTeam(team);
				this._nextMoraleTeam++;
			}
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0001AAD4 File Offset: 0x00018CD4
		private void BoostMoraleForTeam(Team team)
		{
		}

		// Token: 0x0400025B RID: 603
		private MissionTime _nextCaptainSkillMoraleBoostTime;

		// Token: 0x0400025C RID: 604
		private bool _boostMorale;

		// Token: 0x0400025D RID: 605
		private int _nextMoraleTeam;
	}
}
