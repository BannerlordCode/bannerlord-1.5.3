using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000284 RID: 644
	public class BattleReinforcementsSpawnController : MissionLogic
	{
		// Token: 0x060023F4 RID: 9204 RVA: 0x00080670 File Offset: 0x0007E870
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._missionAgentSpawnLogic = base.Mission.GetMissionBehavior<IMissionAgentSpawnLogic>();
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x0008068C File Offset: 0x0007E88C
		public override void AfterStart()
		{
			foreach (Team team in base.Mission.Teams)
			{
				foreach (Formation formation in team.FormationsIncludingEmpty)
				{
					formation.OnBeforeMovementOrderApplied += this.OnBeforeFormationMovementOrderApplied;
				}
			}
		}

		// Token: 0x060023F6 RID: 9206 RVA: 0x00080728 File Offset: 0x0007E928
		public override void OnMissionTick(float dt)
		{
			for (int i = 0; i < 2; i++)
			{
				if (this._sideRequiresUpdate[i])
				{
					this.UpdateSide((BattleSideEnum)i);
					this._sideRequiresUpdate[i] = false;
				}
			}
		}

		// Token: 0x060023F7 RID: 9207 RVA: 0x0008075C File Offset: 0x0007E95C
		protected override void OnEndMission()
		{
			foreach (Team team in base.Mission.Teams)
			{
				foreach (Formation formation in team.FormationsIncludingEmpty)
				{
					formation.OnBeforeMovementOrderApplied -= this.OnBeforeFormationMovementOrderApplied;
				}
			}
		}

		// Token: 0x060023F8 RID: 9208 RVA: 0x000807F8 File Offset: 0x0007E9F8
		private void UpdateSide(BattleSideEnum side)
		{
			if (this.IsBattleSideRetreating(side))
			{
				if (!this._sideReinforcementSuspended[(int)side] && this._missionAgentSpawnLogic.IsSideSpawnEnabled(side))
				{
					this._missionAgentSpawnLogic.StopSpawner(side);
					this._sideReinforcementSuspended[(int)side] = true;
					return;
				}
			}
			else if (this._sideReinforcementSuspended[(int)side])
			{
				this._missionAgentSpawnLogic.StartSpawner(side);
				this._sideReinforcementSuspended[(int)side] = false;
			}
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x00080860 File Offset: 0x0007EA60
		private bool IsBattleSideRetreating(BattleSideEnum side)
		{
			bool flag = true;
			foreach (Team team in base.Mission.Teams)
			{
				if (team.Side == side)
				{
					foreach (Formation formation in team.FormationsIncludingEmpty)
					{
						if (formation.CountOfUnits > 0 && formation.GetReadonlyMovementOrderReference().OrderEnum != MovementOrder.MovementOrderEnum.Retreat)
						{
							flag = false;
							break;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x00080918 File Offset: 0x0007EB18
		private unsafe void OnBeforeFormationMovementOrderApplied(Formation formation, MovementOrder.MovementOrderEnum orderEnum)
		{
			if (formation.GetReadonlyMovementOrderReference()->OrderEnum == MovementOrder.MovementOrderEnum.Retreat || orderEnum == MovementOrder.MovementOrderEnum.Retreat)
			{
				int side = (int)formation.Team.Side;
				this._sideRequiresUpdate[side] = true;
			}
		}

		// Token: 0x04000DD5 RID: 3541
		private IMissionAgentSpawnLogic _missionAgentSpawnLogic;

		// Token: 0x04000DD6 RID: 3542
		private bool[] _sideReinforcementSuspended = new bool[2];

		// Token: 0x04000DD7 RID: 3543
		private bool[] _sideRequiresUpdate = new bool[2];
	}
}
