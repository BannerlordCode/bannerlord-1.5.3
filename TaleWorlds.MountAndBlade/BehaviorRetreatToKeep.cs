using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200012A RID: 298
	public class BehaviorRetreatToKeep : BehaviorComponent
	{
		// Token: 0x06000EAC RID: 3756 RVA: 0x00022C27 File Offset: 0x00020E27
		public BehaviorRetreatToKeep(Formation formation)
			: base(formation)
		{
			base.CurrentOrder = MovementOrder.MovementOrderRetreat;
			base.BehaviorCoherence = 0f;
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x00022C46 File Offset: 0x00020E46
		public override void TickOccasionally()
		{
			base.TickOccasionally();
			if (base.Formation.AI.ActiveBehavior == this)
			{
				base.Formation.SetMovementOrder(base.CurrentOrder);
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000EAE RID: 3758 RVA: 0x00022C72 File Offset: 0x00020E72
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x00022C79 File Offset: 0x00020E79
		protected override float GetAiWeight()
		{
			return 1f;
		}
	}
}
