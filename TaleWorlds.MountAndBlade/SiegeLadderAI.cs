using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000164 RID: 356
	public sealed class SiegeLadderAI : UsableMachineAIBase
	{
		// Token: 0x06001286 RID: 4742 RVA: 0x0003A12D File Offset: 0x0003832D
		public SiegeLadderAI(SiegeLadder ladder)
			: base(ladder)
		{
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06001287 RID: 4743 RVA: 0x0003A136 File Offset: 0x00038336
		public SiegeLadder Ladder
		{
			get
			{
				return this.UsableMachine as SiegeLadder;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06001288 RID: 4744 RVA: 0x0003A143 File Offset: 0x00038343
		public override bool HasActionCompleted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06001289 RID: 4745 RVA: 0x0003A146 File Offset: 0x00038346
		protected override MovementOrder NextOrder
		{
			get
			{
				return MovementOrder.MovementOrderCharge;
			}
		}
	}
}
