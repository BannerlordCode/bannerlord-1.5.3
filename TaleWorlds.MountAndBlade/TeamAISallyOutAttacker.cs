using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200018D RID: 397
	public class TeamAISallyOutAttacker : TeamAISiegeComponent
	{
		// Token: 0x06001545 RID: 5445 RVA: 0x0004E5F4 File Offset: 0x0004C7F4
		public TeamAISallyOutAttacker(Mission currentMission, Team currentTeam, float thinkTimerTime, float applyTimerTime)
			: base(currentMission, currentTeam, thinkTimerTime, applyTimerTime)
		{
			this.ArcherPositions = currentMission.Scene.FindEntitiesWithTag("archer_position").ToMBList<GameEntity>();
			this.BesiegerRangedSiegeWeapons = new List<UsableMachine>(from w in currentMission.ActiveMissionObjects.FindAllWithType<RangedSiegeWeapon>()
				where w.Side == BattleSideEnum.Attacker && !w.IsDisabled
				select w);
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x0004E664 File Offset: 0x0004C864
		public override void OnUnitAddedToFormationForTheFirstTime(Formation formation)
		{
			if (formation.AI.GetBehavior<BehaviorCharge>() == null)
			{
				formation.ForceCalculateCaches();
				if (formation.FormationIndex == FormationClass.NumberOfRegularFormations)
				{
					formation.AI.AddAiBehavior(new BehaviorGeneral(formation));
				}
				else if (formation.FormationIndex == FormationClass.Bodyguard)
				{
					formation.AI.AddAiBehavior(new BehaviorProtectGeneral(formation));
				}
				formation.AI.AddAiBehavior(new BehaviorCharge(formation));
				formation.AI.AddAiBehavior(new BehaviorPullBack(formation));
				formation.AI.AddAiBehavior(new BehaviorRegroup(formation));
				formation.AI.AddAiBehavior(new BehaviorReserve(formation));
				formation.AI.AddAiBehavior(new BehaviorRetreat(formation));
				formation.AI.AddAiBehavior(new BehaviorStop(formation));
				formation.AI.AddAiBehavior(new BehaviorTacticalCharge(formation));
				formation.AI.AddAiBehavior(new BehaviorShootFromCastleWalls(formation));
				formation.AI.AddAiBehavior(new BehaviorDestroySiegeWeapons(formation));
				formation.AI.AddAiBehavior(new BehaviorSparseSkirmish(formation));
				formation.AI.AddAiBehavior(new BehaviorDefend(formation));
				formation.AI.AddAiBehavior(new BehaviorRetreatToCastle(formation));
				formation.AI.AddAiBehavior(new BehaviorRetreatToKeep(formation));
				formation.AI.AddAiBehavior(new BehaviorDefendCastleKeyPosition(formation));
			}
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x0004E7AC File Offset: 0x0004C9AC
		public override void OnDeploymentFinished()
		{
			base.OnDeploymentFinished();
			if (base.CurrentTactic != null)
			{
				base.CurrentTactic.ResetTactic();
			}
		}

		// Token: 0x040005BB RID: 1467
		public MBList<GameEntity> ArcherPositions;

		// Token: 0x040005BC RID: 1468
		public readonly List<UsableMachine> BesiegerRangedSiegeWeapons;
	}
}
