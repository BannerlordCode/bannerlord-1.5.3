using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000165 RID: 357
	public sealed class SiegeTowerAI : UsableMachineAIBase
	{
		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x0600128A RID: 4746 RVA: 0x0003A14D File Offset: 0x0003834D
		private SiegeTower SiegeTower
		{
			get
			{
				return this.UsableMachine as SiegeTower;
			}
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x0003A15A File Offset: 0x0003835A
		public SiegeTowerAI(SiegeTower siegeTower)
			: base(siegeTower)
		{
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x0600128C RID: 4748 RVA: 0x0003A163 File Offset: 0x00038363
		public override bool HasActionCompleted
		{
			get
			{
				return this.SiegeTower.MovementComponent.HasArrivedAtTarget && this.SiegeTower.State == SiegeTower.GateState.Open;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x0600128D RID: 4749 RVA: 0x0003A187 File Offset: 0x00038387
		protected override MovementOrder NextOrder
		{
			get
			{
				return MovementOrder.MovementOrderCharge;
			}
		}
	}
}
