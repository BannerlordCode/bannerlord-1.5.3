using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000362 RID: 866
	public class StandingPointForRangedArea : StandingPoint
	{
		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x060031F9 RID: 12793 RVA: 0x000CBEA2 File Offset: 0x000CA0A2
		public override Agent.AIScriptedFrameFlags DisableScriptedFrameFlags
		{
			get
			{
				return Agent.AIScriptedFrameFlags.NoAttack | Agent.AIScriptedFrameFlags.ConsiderRotation;
			}
		}

		// Token: 0x060031FA RID: 12794 RVA: 0x000CBEA5 File Offset: 0x000CA0A5
		protected internal override void OnInit()
		{
			base.OnInit();
			this.AutoSheathWeapons = false;
			this.LockUserFrames = false;
			this.LockUserPositions = true;
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x060031FB RID: 12795 RVA: 0x000CBED0 File Offset: 0x000CA0D0
		public override bool IsDisabledForAgent(Agent agent)
		{
			EquipmentIndex primaryWieldedItemIndex = agent.GetPrimaryWieldedItemIndex();
			if (primaryWieldedItemIndex == EquipmentIndex.None)
			{
				return true;
			}
			WeaponComponentData currentUsageItem = agent.Equipment[primaryWieldedItemIndex].CurrentUsageItem;
			if (currentUsageItem == null || !currentUsageItem.IsRangedWeapon)
			{
				return true;
			}
			if (primaryWieldedItemIndex == EquipmentIndex.ExtraWeaponSlot)
			{
				return this.ThrowingValueMultiplier <= 0f || base.IsDisabledForAgent(agent);
			}
			return this.RangedWeaponValueMultiplier <= 0f || base.IsDisabledForAgent(agent);
		}

		// Token: 0x060031FC RID: 12796 RVA: 0x000CBF40 File Offset: 0x000CA140
		public override float GetUsageScoreForAgent(Agent agent)
		{
			EquipmentIndex primaryWieldedItemIndex = agent.GetPrimaryWieldedItemIndex();
			float num = 0f;
			if (primaryWieldedItemIndex != EquipmentIndex.None && agent.Equipment[primaryWieldedItemIndex].CurrentUsageItem.IsRangedWeapon)
			{
				num = ((primaryWieldedItemIndex == EquipmentIndex.ExtraWeaponSlot) ? this.ThrowingValueMultiplier : this.RangedWeaponValueMultiplier);
			}
			return base.GetUsageScoreForAgent(agent) + num;
		}

		// Token: 0x060031FD RID: 12797 RVA: 0x000CBF95 File Offset: 0x000CA195
		public override bool HasAlternative()
		{
			return true;
		}

		// Token: 0x060031FE RID: 12798 RVA: 0x000CBF98 File Offset: 0x000CA198
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.HasUser)
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.TickParallel2;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x060031FF RID: 12799 RVA: 0x000CBFB1 File Offset: 0x000CA1B1
		protected internal override void OnTickParallel2(float dt)
		{
			base.OnTickParallel2(dt);
			if (base.HasUser && this.IsDisabledForAgent(base.UserAgent))
			{
				base.UserAgent.StopUsingGameObjectMT(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
			}
		}

		// Token: 0x04001514 RID: 5396
		public float ThrowingValueMultiplier = 5f;

		// Token: 0x04001515 RID: 5397
		public float RangedWeaponValueMultiplier = 2f;
	}
}
