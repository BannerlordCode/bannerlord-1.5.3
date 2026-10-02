using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000129 RID: 297
	public class BehaviorRetreatToCastle : BehaviorComponent
	{
		// Token: 0x06000EA8 RID: 3752 RVA: 0x00022BA4 File Offset: 0x00020DA4
		public BehaviorRetreatToCastle(Formation formation)
			: base(formation)
		{
			WorldPosition worldPosition = Mission.Current.DeploymentPlan.GetFormationPlan(formation.Team, FormationClass.Cavalry, false).CreateNewDeploymentWorldPosition(WorldPosition.WorldPositionEnforcedCache.GroundVec3);
			base.CurrentOrder = MovementOrder.MovementOrderMove(worldPosition);
			base.BehaviorCoherence = 0f;
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x00022BED File Offset: 0x00020DED
		public override void TickOccasionally()
		{
			base.TickOccasionally();
			if (base.Formation.AI.ActiveBehavior == this)
			{
				base.Formation.SetMovementOrder(base.CurrentOrder);
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000EAA RID: 3754 RVA: 0x00022C19 File Offset: 0x00020E19
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x00022C20 File Offset: 0x00020E20
		protected override float GetAiWeight()
		{
			return 1f;
		}
	}
}
