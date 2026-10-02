using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000132 RID: 306
	public class BehaviorShootFromCastleWalls : BehaviorComponent
	{
		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000EFB RID: 3835 RVA: 0x0002540E File Offset: 0x0002360E
		// (set) Token: 0x06000EFC RID: 3836 RVA: 0x00025416 File Offset: 0x00023616
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
					this.OnArcherPositionSet(value);
				}
			}
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x0002542D File Offset: 0x0002362D
		public BehaviorShootFromCastleWalls(Formation formation)
			: base(formation)
		{
			this.OnArcherPositionSet(this._archerPosition);
			base.BehaviorCoherence = 0f;
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x00025450 File Offset: 0x00023650
		private void OnArcherPositionSet(GameEntity value)
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
			this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(this._archerPosition.GetGlobalFrame().rotation.f.AsVec2);
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x0002553C File Offset: 0x0002373C
		public override void TickOccasionally()
		{
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (this._tacticalArcherPosition != null)
			{
				base.Formation.SetFormOrder(FormOrder.FormOrderCustom(this._tacticalArcherPosition.Width), true);
			}
			foreach (Team team in base.Formation.Team.Mission.Teams)
			{
				if (team.IsEnemyOf(base.Formation.Team))
				{
					if (!this._areStrategicArcherAreasAbandoned)
					{
						if (team.QuerySystem.InsideWallsRatio > 0.6f)
						{
							base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
							this._areStrategicArcherAreasAbandoned = true;
							break;
						}
						break;
					}
					else
					{
						if (team.QuerySystem.InsideWallsRatio <= 0.4f)
						{
							base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderScatter);
							this._areStrategicArcherAreasAbandoned = false;
							break;
						}
						break;
					}
				}
			}
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x00025654 File Offset: 0x00023854
		protected override void OnBehaviorActivatedAux()
		{
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderScatter);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000F01 RID: 3841 RVA: 0x000256B4 File Offset: 0x000238B4
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x000256BB File Offset: 0x000238BB
		protected override float GetAiWeight()
		{
			return 10f * (base.Formation.QuerySystem.RangedCavalryUnitRatio + base.Formation.QuerySystem.RangedUnitRatio);
		}

		// Token: 0x04000396 RID: 918
		private GameEntity _archerPosition;

		// Token: 0x04000397 RID: 919
		private TacticalPosition _tacticalArcherPosition;

		// Token: 0x04000398 RID: 920
		private bool _areStrategicArcherAreasAbandoned;
	}
}
