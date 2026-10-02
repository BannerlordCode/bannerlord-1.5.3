using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000139 RID: 313
	public class BehaviorStop : BehaviorComponent
	{
		// Token: 0x06000F29 RID: 3881 RVA: 0x000273F2 File Offset: 0x000255F2
		public BehaviorStop(Formation formation)
			: base(formation)
		{
			base.CurrentOrder = MovementOrder.MovementOrderStop;
			base.BehaviorCoherence = 0f;
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x00027411 File Offset: 0x00025611
		public override void TickOccasionally()
		{
			base.Formation.SetMovementOrder(base.CurrentOrder);
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x00027424 File Offset: 0x00025624
		protected override void OnBehaviorActivatedAux()
		{
			base.Formation.SetArrangementOrder(base.Formation.QuerySystem.HasShield ? ArrangementOrder.ArrangementOrderShieldWall : ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			this._lastPlayerInformTime = Mission.Current.CurrentTime;
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000F2C RID: 3884 RVA: 0x0002747A File Offset: 0x0002567A
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x00027481 File Offset: 0x00025681
		protected override float GetAiWeight()
		{
			return 0.01f;
		}
	}
}
