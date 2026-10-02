using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000125 RID: 293
	public class BehaviorRegroup : BehaviorComponent
	{
		// Token: 0x06000E89 RID: 3721 RVA: 0x00021F6F File Offset: 0x0002016F
		public BehaviorRegroup(Formation formation)
			: base(formation)
		{
			base.BehaviorCoherence = 1f;
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00021F8C File Offset: 0x0002018C
		protected override void CalculateCurrentOrder()
		{
			Vec2 vec = ((base.Formation.CachedClosestEnemyFormation != null) ? (base.Formation.CachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized() : base.Formation.Direction);
			WorldPosition cachedMedianPosition = base.Formation.CachedMedianPosition;
			cachedMedianPosition.SetVec2(base.Formation.CachedAveragePosition);
			base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
			this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x0002201F File Offset: 0x0002021F
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x0002204C File Offset: 0x0002024C
		protected override float GetAiWeight()
		{
			FormationQuerySystem querySystem = base.Formation.QuerySystem;
			if (base.Formation.AI.ActiveBehavior == null)
			{
				return 0f;
			}
			float behaviorCoherence = base.Formation.AI.ActiveBehavior.BehaviorCoherence;
			return MBMath.Lerp(0.1f, 1.2f, MBMath.ClampFloat(behaviorCoherence * (base.Formation.CachedFormationIntegrityData.DeviationOfPositionsExcludeFarAgents + 1f) / (querySystem.IdealAverageDisplacement + 1f), 0f, 3f) / 3f, 1E-05f);
		}
	}
}
