using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200013E RID: 318
	public class BehaviorWaitForLadders : BehaviorComponent
	{
		// Token: 0x06000F4A RID: 3914 RVA: 0x00028D9C File Offset: 0x00026F9C
		public BehaviorWaitForLadders(Formation formation)
			: base(formation)
		{
			this._behaviorSide = formation.AI.Side;
			this._ladders = Mission.Current.ActiveMissionObjects.OfType<SiegeLadder>().ToList<SiegeLadder>();
			this._ladders.RemoveAll((SiegeLadder l) => l.IsDeactivated || l.WeaponSide != this._behaviorSide);
			this._teamAISiegeComponent = (TeamAISiegeComponent)formation.Team.TeamAI;
			SiegeLane siegeLane = TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == this._behaviorSide);
			object obj;
			if (siegeLane == null)
			{
				obj = null;
			}
			else
			{
				obj = siegeLane.DefensePoints.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp is WallSegment && (dp as WallSegment).IsBreachedWall);
			}
			this._breachedWallSegment = obj as WallSegment;
			this.ResetFollowOrder();
			this._stopOrder = MovementOrder.MovementOrderStop;
			if (this._followOrder.OrderEnum != MovementOrder.MovementOrderEnum.Invalid)
			{
				base.CurrentOrder = this._followOrder;
				this._behaviorState = BehaviorWaitForLadders.BehaviorState.Follow;
				return;
			}
			base.CurrentOrder = this._stopOrder;
			this._behaviorState = BehaviorWaitForLadders.BehaviorState.Stop;
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x00028EA0 File Offset: 0x000270A0
		private void ResetFollowOrder()
		{
			this._followedEntity = null;
			this._followTacticalPosition = null;
			if (this._ladders.Count > 0)
			{
				SiegeLadder siegeLadder;
				if ((siegeLadder = this._ladders.FirstOrDefault<SiegeLadder>((SiegeLadder l) => !l.IsDeactivated && l.InitialWaitPosition.HasScriptOfType<TacticalPosition>())) == null)
				{
					siegeLadder = this._ladders.FirstOrDefault<SiegeLadder>((SiegeLadder l) => !l.IsDeactivated);
				}
				this._followedEntity = siegeLadder.InitialWaitPosition;
				if (this._followedEntity == null)
				{
					this._followedEntity = this._ladders.FirstOrDefault<SiegeLadder>((SiegeLadder l) => !l.IsDeactivated).InitialWaitPosition;
				}
				this._followOrder = MovementOrder.MovementOrderFollowEntity(this._followedEntity);
			}
			else if (this._breachedWallSegment != null)
			{
				WeakGameEntity firstChildEntityWithTagRecursive = this._breachedWallSegment.GameEntity.GetFirstChildEntityWithTagRecursive("attacker_wait_pos");
				this._followedEntity = GameEntity.CreateFromWeakEntity(firstChildEntityWithTagRecursive);
				this._followOrder = MovementOrder.MovementOrderFollowEntity(this._followedEntity);
			}
			else
			{
				this._followOrder = MovementOrder.MovementOrderNull;
			}
			if (this._followedEntity != null)
			{
				this._followTacticalPosition = this._followedEntity.GetFirstScriptOfType<TacticalPosition>();
			}
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x00028FF0 File Offset: 0x000271F0
		public override void OnValidBehaviorSideChanged()
		{
			base.OnValidBehaviorSideChanged();
			this._ladders = Mission.Current.ActiveMissionObjects.OfType<SiegeLadder>().ToList<SiegeLadder>();
			this._ladders.RemoveAll((SiegeLadder l) => l.IsDeactivated || l.WeaponSide != this._behaviorSide);
			SiegeLane siegeLane = TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == this._behaviorSide);
			object obj;
			if (siegeLane == null)
			{
				obj = null;
			}
			else
			{
				obj = siegeLane.DefensePoints.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp is WallSegment && (dp as WallSegment).IsBreachedWall);
			}
			this._breachedWallSegment = obj as WallSegment;
			this.ResetFollowOrder();
			this._behaviorState = BehaviorWaitForLadders.BehaviorState.Unset;
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x00029094 File Offset: 0x00027294
		protected override void CalculateCurrentOrder()
		{
			BehaviorWaitForLadders.BehaviorState behaviorState = ((this._followOrder.OrderEnum != MovementOrder.MovementOrderEnum.Invalid) ? BehaviorWaitForLadders.BehaviorState.Follow : BehaviorWaitForLadders.BehaviorState.Stop);
			if (behaviorState != this._behaviorState)
			{
				if (behaviorState == BehaviorWaitForLadders.BehaviorState.Follow)
				{
					base.CurrentOrder = this._followOrder;
					if (this._followTacticalPosition != null)
					{
						this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(this._followTacticalPosition.Direction);
					}
					else
					{
						this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
					}
				}
				else
				{
					base.CurrentOrder = this._stopOrder;
					this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
				}
				this._behaviorState = behaviorState;
			}
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x00029118 File Offset: 0x00027318
		public override void TickOccasionally()
		{
			base.TickOccasionally();
			if (this._ladders.RemoveAll((SiegeLadder l) => l.IsDeactivated) > 0)
			{
				this.ResetFollowOrder();
				this.CalculateCurrentOrder();
			}
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (this._behaviorState == BehaviorWaitForLadders.BehaviorState.Follow && this._followTacticalPosition != null)
			{
				base.Formation.SetFormOrder(FormOrder.FormOrderCustom(this._followTacticalPosition.Width), true);
			}
			foreach (SiegeLadder siegeLadder in this._ladders)
			{
				if (siegeLadder.IsUsedByFormation(base.Formation))
				{
					base.Formation.StopUsingMachine(siegeLadder, false);
				}
			}
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x00029210 File Offset: 0x00027410
		protected override void OnBehaviorActivatedAux()
		{
			base.Formation.SetArrangementOrder(base.Formation.QuerySystem.HasShield ? ArrangementOrder.ArrangementOrderShieldWall : ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFacingOrder(FacingOrder.FacingOrderLookAtEnemy);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000F50 RID: 3920 RVA: 0x00029277 File Offset: 0x00027477
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x00029280 File Offset: 0x00027480
		protected override float GetAiWeight()
		{
			float num = 0f;
			if (this._followOrder.OrderEnum != MovementOrder.MovementOrderEnum.Invalid && !this._teamAISiegeComponent.AreLaddersReady)
			{
				num = ((!this._teamAISiegeComponent.IsCastleBreached()) ? 1f : 0.5f);
			}
			return num;
		}

		// Token: 0x040003BB RID: 955
		private const string WallWaitPositionTag = "attacker_wait_pos";

		// Token: 0x040003BC RID: 956
		private List<SiegeLadder> _ladders;

		// Token: 0x040003BD RID: 957
		private WallSegment _breachedWallSegment;

		// Token: 0x040003BE RID: 958
		private TeamAISiegeComponent _teamAISiegeComponent;

		// Token: 0x040003BF RID: 959
		private MovementOrder _stopOrder;

		// Token: 0x040003C0 RID: 960
		private MovementOrder _followOrder;

		// Token: 0x040003C1 RID: 961
		private BehaviorWaitForLadders.BehaviorState _behaviorState;

		// Token: 0x040003C2 RID: 962
		private GameEntity _followedEntity;

		// Token: 0x040003C3 RID: 963
		private TacticalPosition _followTacticalPosition;

		// Token: 0x0200045B RID: 1115
		private enum BehaviorState
		{
			// Token: 0x04001A2C RID: 6700
			Unset,
			// Token: 0x04001A2D RID: 6701
			Stop,
			// Token: 0x04001A2E RID: 6702
			Follow
		}
	}
}
