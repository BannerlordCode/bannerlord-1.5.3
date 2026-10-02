using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000128 RID: 296
	public class BehaviorRetreat : BehaviorComponent
	{
		// Token: 0x06000EA3 RID: 3747 RVA: 0x00022AD6 File Offset: 0x00020CD6
		public BehaviorRetreat(Formation formation)
			: base(formation)
		{
			base.CurrentOrder = MovementOrder.MovementOrderRetreat;
			base.BehaviorCoherence = 0f;
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x00022AF5 File Offset: 0x00020CF5
		public override void TickOccasionally()
		{
			base.Formation.SetMovementOrder(base.CurrentOrder);
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x00022B08 File Offset: 0x00020D08
		protected override void OnBehaviorActivatedAux()
		{
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000EA6 RID: 3750 RVA: 0x00022B0A File Offset: 0x00020D0A
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x00022B14 File Offset: 0x00020D14
		protected override float GetAiWeight()
		{
			float casualtyPowerLossOfFormation = Mission.Current.GetMissionBehavior<CasualtyHandler>().GetCasualtyPowerLossOfFormation(base.Formation);
			float num = MathF.Sqrt(casualtyPowerLossOfFormation / (base.Formation.QuerySystem.FormationPower + casualtyPowerLossOfFormation));
			return MBMath.ClampFloat(base.Formation.Team.QuerySystem.TotalPowerRatio, 0.1f, 3f) / MBMath.ClampFloat(base.Formation.Team.QuerySystem.RemainingPowerRatio, 0.1f, 3f) * (0.05f + num);
		}
	}
}
