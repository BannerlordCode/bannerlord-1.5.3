using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000119 RID: 281
	public class BehaviorDefensiveRing : BehaviorComponent
	{
		// Token: 0x06000E3A RID: 3642 RVA: 0x0001EA5F File Offset: 0x0001CC5F
		public BehaviorDefensiveRing(Formation formation)
			: base(formation)
		{
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x0001EA70 File Offset: 0x0001CC70
		protected override void CalculateCurrentOrder()
		{
			Vec2 vec;
			if (this.TacticalDefendPosition != null)
			{
				vec = this.TacticalDefendPosition.Direction;
			}
			else if (base.Formation.CachedClosestEnemyFormation == null)
			{
				vec = base.Formation.Direction;
			}
			else
			{
				vec = ((base.Formation.Direction.DotProduct((base.Formation.CachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized()) < 0.5f) ? (base.Formation.CachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition) : base.Formation.Direction).Normalized();
			}
			if (this.TacticalDefendPosition != null)
			{
				base.CurrentOrder = MovementOrder.MovementOrderMove(this.TacticalDefendPosition.Position);
			}
			else
			{
				WorldPosition cachedMedianPosition = base.Formation.CachedMedianPosition;
				cachedMedianPosition.SetVec2(base.Formation.CachedAveragePosition);
				base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
			}
			this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x0001EBA0 File Offset: 0x0001CDA0
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (base.Formation.CachedAveragePosition.Distance(base.CurrentOrder.GetPosition(base.Formation)) - base.Formation.Arrangement.Depth * 0.5f < 10f)
			{
				base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderCircle);
				if (base.Formation.Team.FormationsIncludingEmpty.AnyQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.QuerySystem.IsRangedFormation))
				{
					Formation formation = base.Formation.Team.FormationsIncludingEmpty.WhereQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.QuerySystem.IsRangedFormation).MaxBy<Formation, int>((Formation f) => f.CountOfUnits);
					int num = (int)MathF.Sqrt((float)formation.CountOfUnits);
					float num2 = ((float)num * formation.UnitDiameter + (float)(num - 1) * formation.Interval) * 0.5f * 1.414213f;
					int i = base.Formation.Arrangement.UnitCount;
					int num3 = 0;
					while (i > 0)
					{
						double num4 = (double)(num2 + base.Formation.Distance * (float)num3 + base.Formation.UnitDiameter * (float)(num3 + 1)) * 3.141592653589793 * 2.0 / (double)(base.Formation.UnitDiameter + base.Formation.Interval);
						i -= (int)Math.Ceiling(num4);
						num3++;
					}
					float num5 = num2 + (float)num3 * base.Formation.UnitDiameter + (float)(num3 - 1) * base.Formation.Distance;
					base.Formation.SetFormOrder(FormOrder.FormOrderCustom(num5 * 2f), true);
					return;
				}
			}
			else
			{
				base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
			}
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x0001EDC4 File Offset: 0x0001CFC4
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderDeep, true);
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x0001EE2A File Offset: 0x0001D02A
		public override void ResetBehavior()
		{
			base.ResetBehavior();
			this.TacticalDefendPosition = null;
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x0001EE39 File Offset: 0x0001D039
		protected override float GetAiWeight()
		{
			if (this.TacticalDefendPosition == null)
			{
				return 0f;
			}
			return 1f;
		}

		// Token: 0x04000361 RID: 865
		public TacticalPosition TacticalDefendPosition;
	}
}
