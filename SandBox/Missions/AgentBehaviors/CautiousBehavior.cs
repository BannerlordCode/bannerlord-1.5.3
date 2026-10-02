using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000A7 RID: 167
	public class CautiousBehavior : AgentBehavior
	{
		// Token: 0x06000703 RID: 1795 RVA: 0x0002F04A File Offset: 0x0002D24A
		public CautiousBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
			this._waitTimer = new Timer(base.Mission.CurrentTime, 10f, true);
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0002F070 File Offset: 0x0002D270
		public override void Tick(float dt, bool isSimulation)
		{
			bool flag = true;
			if (base.OwnerAgent.IsCautious())
			{
				if (base.OwnerAgent.IsAIAtMoveDestination())
				{
					base.OwnerAgent.SetActionChannel(1, in ActionIndexCache.act_stealth_mission_guard_look_around_cautious_1, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
				}
				else
				{
					base.OwnerAgent.SetActionChannel(1, in ActionIndexCache.act_none, false, AnimFlags.amf_priority_jump, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
				}
				if (base.OwnerAgent.GetPrimaryWieldedItemIndex() != EquipmentIndex.None)
				{
					base.Mission.AddTickAction(Mission.MissionTickAction.TryToSheathWeaponInHand, base.OwnerAgent, 0, 1);
				}
				EquipmentIndex offhandWieldedItemIndex = base.OwnerAgent.GetOffhandWieldedItemIndex();
				if (offhandWieldedItemIndex != EquipmentIndex.None && offhandWieldedItemIndex != EquipmentIndex.ExtraWeaponSlot)
				{
					base.Mission.AddTickAction(Mission.MissionTickAction.TryToSheathWeaponInHand, base.OwnerAgent, 1, 1);
				}
			}
			else if (base.OwnerAgent.IsPatrollingCautious())
			{
				bool flag2 = base.OwnerAgent.IsAIAtMoveDestination();
				base.OwnerAgent.SetWeaponGuard(Agent.UsageDirection.AttackRight);
				if (flag2 && base.OwnerAgent.GetAIMoveDestination().AsVec2.DistanceSquared(base.OwnerAgent.GetAILastSuspiciousPosition().AsVec2) < 1f)
				{
					flag = false;
					if (this._waitTimer.Check(base.Mission.CurrentTime))
					{
						this._waitTimer.Reset(base.Mission.CurrentTime, MBRandom.RandomFloat * 4f + 8f);
						base.OwnerAgent.SetActionChannel(1, in ActionIndexCache.act_none, false, AnimFlags.amf_priority_jump, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
						WorldPosition worldPosition = base.OwnerAgent.GetWorldPosition();
						Vec2 asVec = worldPosition.AsVec2;
						Vec2 movementDirection = base.OwnerAgent.GetMovementDirection();
						movementDirection.RotateCCW(MBRandom.RandomFloat * 6.2831855f);
						worldPosition.SetVec2(worldPosition.AsVec2 + movementDirection * base.OwnerAgent.Monster.BodyCapsuleRadius * MBRandom.RandomFloatRanged(20f, 35f));
						bool flag3;
						worldPosition.SetVec2(base.OwnerAgent.FindLongestDirectMoveToPosition(worldPosition.AsVec2, true, false, out flag3));
						float num = worldPosition.AsVec2.DistanceSquared(asVec);
						if (num > base.OwnerAgent.Monster.BodyCapsuleRadius * base.OwnerAgent.Monster.BodyCapsuleRadius * 10f * 10f)
						{
							worldPosition.SetVec2(asVec + movementDirection * (MathF.Sqrt(num) - base.OwnerAgent.Monster.BodyCapsuleRadius * 10f));
							base.OwnerAgent.SetAILastSuspiciousPosition(worldPosition, false);
						}
					}
					else
					{
						base.OwnerAgent.SetActionChannel(1, in ActionIndexCache.act_stealth_mission_guard_look_around_cautious_1, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
					}
				}
			}
			if (flag)
			{
				this._waitTimer.Reset(base.Mission.CurrentTime);
			}
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0002F3A3 File Offset: 0x0002D5A3
		public override float GetAvailability(bool isSimulation)
		{
			if (!base.OwnerAgent.IsCautious() && !base.OwnerAgent.IsPatrollingCautious())
			{
				return 0f;
			}
			return 10f;
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0002F3CC File Offset: 0x0002D5CC
		protected override void OnDeactivate()
		{
			if (!base.OwnerAgent.IsAlarmed())
			{
				base.OwnerAgent.SetActionChannel(1, in ActionIndexCache.act_none, false, AnimFlags.amf_priority_jump, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
				if (base.OwnerAgent.GetPrimaryWieldedItemIndex() != EquipmentIndex.None)
				{
					base.Mission.AddTickActionMT(Mission.MissionTickAction.TryToSheathWeaponInHand, base.OwnerAgent, 0, 0);
				}
				EquipmentIndex offhandWieldedItemIndex = base.OwnerAgent.GetOffhandWieldedItemIndex();
				if (offhandWieldedItemIndex != EquipmentIndex.None && offhandWieldedItemIndex != EquipmentIndex.ExtraWeaponSlot)
				{
					base.Mission.AddTickActionMT(Mission.MissionTickAction.TryToSheathWeaponInHand, base.OwnerAgent, 1, 0);
				}
			}
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0002F469 File Offset: 0x0002D669
		protected override void OnActivate()
		{
			this._waitTimer.Reset(base.Mission.CurrentTime);
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0002F481 File Offset: 0x0002D681
		public override string GetDebugInfo()
		{
			return string.Empty;
		}

		// Token: 0x040003B1 RID: 945
		private readonly Timer _waitTimer;
	}
}
