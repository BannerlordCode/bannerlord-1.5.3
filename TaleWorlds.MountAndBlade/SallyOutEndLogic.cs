using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200029D RID: 669
	public class SallyOutEndLogic : MissionLogic
	{
		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06002535 RID: 9525 RVA: 0x00087365 File Offset: 0x00085565
		// (set) Token: 0x06002536 RID: 9526 RVA: 0x0008736D File Offset: 0x0008556D
		public bool IsSallyOutOver { get; private set; }

		// Token: 0x06002537 RID: 9527 RVA: 0x00087378 File Offset: 0x00085578
		public override void OnMissionTick(float dt)
		{
			if (this.CheckTimer(dt))
			{
				if (this._checkState == SallyOutEndLogic.EndConditionCheckState.Deactive)
				{
					using (IEnumerator<Team> enumerator = base.Mission.Teams.Where<Team>((Team t) => t.Side == BattleSideEnum.Defender).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Team team = enumerator.Current;
							foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
							{
								if (formation.CountOfUnits > 0 && formation.CountOfUnits > 0 && !TeamAISiegeComponent.IsFormationInsideCastle(formation, true, 0.1f))
								{
									this._checkState = SallyOutEndLogic.EndConditionCheckState.Active;
									return;
								}
							}
						}
						return;
					}
				}
				if (this._checkState == SallyOutEndLogic.EndConditionCheckState.Idle)
				{
					this._checkState = SallyOutEndLogic.EndConditionCheckState.Active;
				}
			}
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x00087470 File Offset: 0x00085670
		public override bool MissionEnded(ref MissionResult missionResult)
		{
			if (this.IsSallyOutOver)
			{
				missionResult = MissionResult.CreateSuccessful(base.Mission, false);
				return true;
			}
			if (this._checkState != SallyOutEndLogic.EndConditionCheckState.Active)
			{
				return false;
			}
			foreach (Team team in base.Mission.Teams)
			{
				BattleSideEnum side = team.Side;
				if (side != BattleSideEnum.Defender)
				{
					if (side == BattleSideEnum.Attacker && TeamAISiegeComponent.IsFormationGroupInsideCastle(team.FormationsIncludingSpecialAndEmpty, false, 0.1f))
					{
						this._checkState = SallyOutEndLogic.EndConditionCheckState.Idle;
						return false;
					}
				}
				else if (team.FormationsIncludingEmpty.Any<Formation>((Formation f) => f.CountOfUnits > 0 && !TeamAISiegeComponent.IsFormationInsideCastle(f, false, 0.9f)))
				{
					this._checkState = SallyOutEndLogic.EndConditionCheckState.Idle;
					return false;
				}
			}
			this.IsSallyOutOver = true;
			missionResult = MissionResult.CreateSuccessful(base.Mission, false);
			return true;
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x00087560 File Offset: 0x00085760
		private bool CheckTimer(float dt)
		{
			this._dtSum += dt;
			if (this._dtSum < this._nextCheckTime)
			{
				return false;
			}
			this._dtSum = 0f;
			this._nextCheckTime = 0.8f + MBRandom.RandomFloat * 0.4f;
			return true;
		}

		// Token: 0x04000E5B RID: 3675
		private SallyOutEndLogic.EndConditionCheckState _checkState;

		// Token: 0x04000E5D RID: 3677
		private float _nextCheckTime;

		// Token: 0x04000E5E RID: 3678
		private float _dtSum;

		// Token: 0x02000576 RID: 1398
		private enum EndConditionCheckState
		{
			// Token: 0x04001EA5 RID: 7845
			Deactive,
			// Token: 0x04001EA6 RID: 7846
			Active,
			// Token: 0x04001EA7 RID: 7847
			Idle
		}
	}
}
