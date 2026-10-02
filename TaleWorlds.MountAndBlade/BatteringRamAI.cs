using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000161 RID: 353
	public sealed class BatteringRamAI : UsableMachineAIBase
	{
		// Token: 0x06001279 RID: 4729 RVA: 0x00039D13 File Offset: 0x00037F13
		public BatteringRamAI(BatteringRam batteringRam)
			: base(batteringRam)
		{
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x0600127A RID: 4730 RVA: 0x00039D1C File Offset: 0x00037F1C
		private BatteringRam BatteringRam
		{
			get
			{
				return this.UsableMachine as BatteringRam;
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x0600127B RID: 4731 RVA: 0x00039D29 File Offset: 0x00037F29
		public override bool HasActionCompleted
		{
			get
			{
				return this.BatteringRam.IsDeactivated;
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x0600127C RID: 4732 RVA: 0x00039D38 File Offset: 0x00037F38
		protected override MovementOrder NextOrder
		{
			get
			{
				TeamAISiegeComponent teamAISiegeComponent;
				if ((teamAISiegeComponent = Mission.Current.Teams[0].TeamAI as TeamAISiegeComponent) != null && teamAISiegeComponent.InnerGate != null && !teamAISiegeComponent.InnerGate.IsDestroyed)
				{
					return MovementOrder.MovementOrderAttackEntity(GameEntity.CreateFromWeakEntity(teamAISiegeComponent.InnerGate.GameEntity), false);
				}
				return MovementOrder.MovementOrderCharge;
			}
		}
	}
}
