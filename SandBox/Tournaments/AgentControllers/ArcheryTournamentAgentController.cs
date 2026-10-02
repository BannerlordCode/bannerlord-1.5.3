using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Tournaments.MissionLogics;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Tournaments.AgentControllers
{
	// Token: 0x02000031 RID: 49
	public class ArcheryTournamentAgentController : AgentController
	{
		// Token: 0x060001CD RID: 461 RVA: 0x0000BE96 File Offset: 0x0000A096
		public override void OnInitialize()
		{
			this._missionController = Mission.Current.GetMissionBehavior<TournamentArcheryMissionController>();
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000BEA8 File Offset: 0x0000A0A8
		public void OnTick()
		{
			if (!base.Owner.IsAIControlled)
			{
				return;
			}
			this.UpdateTarget();
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000BEBE File Offset: 0x0000A0BE
		public void SetTargets(List<DestructableComponent> targetList)
		{
			this._targetList = targetList;
			this._target = null;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000BECE File Offset: 0x0000A0CE
		private void UpdateTarget()
		{
			if (this._target == null || this._target.IsDestroyed)
			{
				this.SelectNewTarget();
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000BEEC File Offset: 0x0000A0EC
		private void SelectNewTarget()
		{
			List<KeyValuePair<float, DestructableComponent>> list = new List<KeyValuePair<float, DestructableComponent>>();
			foreach (DestructableComponent destructableComponent in this._targetList)
			{
				float score = this.GetScore(destructableComponent);
				if (score > 0f)
				{
					list.Add(new KeyValuePair<float, DestructableComponent>(score, destructableComponent));
				}
			}
			if (list.Count == 0)
			{
				this._target = null;
				base.Owner.DisableScriptedCombatMovement();
				WorldPosition worldPosition = base.Owner.GetWorldPosition();
				base.Owner.SetScriptedPosition(ref worldPosition, false, Agent.AIScriptedFrameFlags.None);
			}
			else
			{
				List<KeyValuePair<float, DestructableComponent>> list2 = list.OrderByDescending<KeyValuePair<float, DestructableComponent>, float>((KeyValuePair<float, DestructableComponent> x) => x.Key).ToList<KeyValuePair<float, DestructableComponent>>();
				int num = MathF.Min(list2.Count, 5);
				this._target = list2[MBRandom.RandomInt(num)].Value;
			}
			if (this._target != null)
			{
				base.Owner.SetScriptedTargetEntity(this._target.GameEntity, Agent.AISpecialCombatModeFlags.None, false);
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000C00C File Offset: 0x0000A20C
		private float GetScore(DestructableComponent target)
		{
			if (!target.IsDestroyed)
			{
				return 1f / base.Owner.Position.DistanceSquared(target.GameEntity.GlobalPosition);
			}
			return 0f;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000C04E File Offset: 0x0000A24E
		public void OnTargetHit(Agent agent, DestructableComponent target)
		{
			if (agent == base.Owner || target == this._target)
			{
				this.SelectNewTarget();
			}
		}

		// Token: 0x040000A5 RID: 165
		private List<DestructableComponent> _targetList;

		// Token: 0x040000A6 RID: 166
		private DestructableComponent _target;

		// Token: 0x040000A7 RID: 167
		private TournamentArcheryMissionController _missionController;
	}
}
