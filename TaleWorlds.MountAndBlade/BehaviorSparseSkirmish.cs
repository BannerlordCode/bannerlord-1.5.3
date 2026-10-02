using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000138 RID: 312
	public class BehaviorSparseSkirmish : BehaviorComponent
	{
		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000F21 RID: 3873 RVA: 0x00027222 File Offset: 0x00025422
		// (set) Token: 0x06000F22 RID: 3874 RVA: 0x0002722A File Offset: 0x0002542A
		public GameEntity ArcherPosition
		{
			get
			{
				return this._archerPosition;
			}
			set
			{
				if (this._archerPosition != value)
				{
					this.SetArcherPosition(value);
				}
			}
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x00027244 File Offset: 0x00025444
		private void SetArcherPosition(GameEntity value)
		{
			this._archerPosition = value;
			if (!(this._archerPosition != null))
			{
				this._tacticalArcherPosition = null;
				WorldPosition cachedMedianPosition = base.Formation.CachedMedianPosition;
				cachedMedianPosition.SetVec2(base.Formation.CurrentPosition);
				base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
				return;
			}
			this._tacticalArcherPosition = this._archerPosition.GetFirstScriptOfType<TacticalPosition>();
			if (this._tacticalArcherPosition != null)
			{
				base.CurrentOrder = MovementOrder.MovementOrderMove(this._tacticalArcherPosition.Position);
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(this._tacticalArcherPosition.Direction);
				return;
			}
			base.CurrentOrder = MovementOrder.MovementOrderMove(this._archerPosition.GlobalPosition.ToWorldPosition());
			this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x0002730F File Offset: 0x0002550F
		public BehaviorSparseSkirmish(Formation formation)
			: base(formation)
		{
			this.SetArcherPosition(this._archerPosition);
			base.BehaviorCoherence = 0f;
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x00027330 File Offset: 0x00025530
		public override void TickOccasionally()
		{
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (this._tacticalArcherPosition != null)
			{
				base.Formation.SetFormOrder(FormOrder.FormOrderCustom(this._tacticalArcherPosition.Width), true);
			}
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x00027384 File Offset: 0x00025584
		protected override void OnBehaviorActivatedAux()
		{
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderScatter);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWider, true);
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000F27 RID: 3879 RVA: 0x000273E4 File Offset: 0x000255E4
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x000273EB File Offset: 0x000255EB
		protected override float GetAiWeight()
		{
			return 2f;
		}

		// Token: 0x040003A7 RID: 935
		private GameEntity _archerPosition;

		// Token: 0x040003A8 RID: 936
		private TacticalPosition _tacticalArcherPosition;
	}
}
