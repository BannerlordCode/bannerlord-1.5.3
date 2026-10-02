using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000184 RID: 388
	public class AttackEntityOrderSecondaryDetachment
	{
		// Token: 0x060014BF RID: 5311 RVA: 0x0004C42A File Offset: 0x0004A62A
		public AttackEntityOrderSecondaryDetachment(GameEntity targetEntity)
		{
			this._targetEntity = targetEntity;
			this._surroundEntity = this._targetEntity.GetFirstScriptOfType<CastleGate>() == null;
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x0004C450 File Offset: 0x0004A650
		public void TickOccasionally(Formation formation)
		{
			foreach (IFormationUnit formationUnit in formation.Arrangement.GetAllUnits())
			{
				((Agent)formationUnit).SetScriptedTargetEntity(this._targetEntity.WeakEntity, this._surroundEntity ? Agent.AISpecialCombatModeFlags.SurroundAttackEntity : Agent.AISpecialCombatModeFlags.None, true);
			}
			foreach (Agent agent in formation.DetachedUnits)
			{
				if (agent.GetScriptedCombatFlags().HasAnyFlag(Agent.AISpecialCombatModeFlags.AttackEntity))
				{
					agent.DisableScriptedCombatMovement();
				}
			}
			foreach (Agent agent2 in formation.LooseDetachedUnits)
			{
				if (agent2.GetScriptedCombatFlags().HasAnyFlag(Agent.AISpecialCombatModeFlags.AttackEntity))
				{
					agent2.DisableScriptedCombatMovement();
				}
			}
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x0004C560 File Offset: 0x0004A760
		public void Disband(Formation formation)
		{
			foreach (IFormationUnit formationUnit in formation.Arrangement.GetAllUnits())
			{
				Agent agent = (Agent)formationUnit;
				if (agent.GetScriptedCombatFlags().HasAnyFlag(Agent.AISpecialCombatModeFlags.AttackEntity))
				{
					agent.DisableScriptedCombatMovement();
				}
			}
			foreach (Agent agent2 in formation.DetachedUnits)
			{
				if (agent2.GetScriptedCombatFlags().HasAnyFlag(Agent.AISpecialCombatModeFlags.AttackEntity))
				{
					agent2.DisableScriptedCombatMovement();
				}
			}
			foreach (Agent agent3 in formation.LooseDetachedUnits)
			{
				if (agent3.GetScriptedCombatFlags().HasAnyFlag(Agent.AISpecialCombatModeFlags.AttackEntity))
				{
					agent3.DisableScriptedCombatMovement();
				}
			}
		}

		// Token: 0x04000584 RID: 1412
		private readonly GameEntity _targetEntity;

		// Token: 0x04000585 RID: 1413
		private readonly bool _surroundEntity;
	}
}
