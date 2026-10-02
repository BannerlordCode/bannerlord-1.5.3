using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000111 RID: 273
	public sealed class BehaviorCautiousAdvance : BehaviorComponent
	{
		// Token: 0x06000DE0 RID: 3552 RVA: 0x0001B744 File Offset: 0x00019944
		public BehaviorCautiousAdvance()
		{
			base.BehaviorCoherence = 1f;
			this._cantShootTimer = new Timer(0f, 0f, true);
			this._switchedToShieldWallTimer = new Timer(0f, 0f, true);
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0001B7B8 File Offset: 0x000199B8
		public BehaviorCautiousAdvance(Formation formation)
			: base(formation)
		{
			base.BehaviorCoherence = 1f;
			this._cantShootTimer = new Timer(0f, 0f, true);
			this._switchedToShieldWallTimer = new Timer(0f, 0f, true);
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x0001B834 File Offset: 0x00019A34
		protected override void CalculateCurrentOrder()
		{
			WorldPosition worldPosition = base.Formation.CachedMedianPosition;
			bool flag = false;
			Vec2 vec;
			if (this._targetFormation == null || this._archerFormation == null)
			{
				vec = base.Formation.Direction;
				worldPosition.SetVec2(base.Formation.CachedAveragePosition);
			}
			else
			{
				vec = this._targetFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition;
				float num = vec.Normalize();
				float num2 = this._archerFormation.QuerySystem.RangedUnitRatio * 0.5f / (float)this._archerFormation.Arrangement.RankCount;
				switch (this._behaviorState)
				{
				case BehaviorCautiousAdvance.BehaviorState.Approaching:
					if (num < this._cantShootDistance * 0.8f)
					{
						this._behaviorState = BehaviorCautiousAdvance.BehaviorState.Shooting;
						this._cantShoot = false;
						flag = true;
					}
					else if (this._archerFormation.QuerySystem.MakingRangedAttackRatio >= num2 * 1.2f)
					{
						this._behaviorState = BehaviorCautiousAdvance.BehaviorState.Shooting;
						this._cantShoot = false;
						flag = true;
					}
					if (this._behaviorState == BehaviorCautiousAdvance.BehaviorState.Shooting)
					{
						this._shootPosition = base.Formation.CachedAveragePosition + vec * 5f;
					}
					break;
				case BehaviorCautiousAdvance.BehaviorState.Shooting:
					if (this._archerFormation.QuerySystem.MakingRangedAttackRatio <= num2)
					{
						if (num > this._archerFormation.QuerySystem.MaximumMissileRange)
						{
							this._behaviorState = BehaviorCautiousAdvance.BehaviorState.Approaching;
							this._cantShootDistance = MathF.Min(this._cantShootDistance, this._archerFormation.QuerySystem.MaximumMissileRange * 0.9f);
							this._shootPosition = Vec2.Invalid;
						}
						else if (!this._cantShoot)
						{
							this._cantShoot = true;
							this._cantShootTimer.Reset(Mission.Current.CurrentTime, (this._archerFormation == null) ? 10f : MBMath.Lerp(10f, 15f, (MBMath.ClampFloat((float)this._archerFormation.CountOfUnits, 10f, 60f) - 10f) * 0.02f, 1E-05f));
						}
						else if (this._cantShootTimer.Check(Mission.Current.CurrentTime))
						{
							this._behaviorState = BehaviorCautiousAdvance.BehaviorState.Approaching;
							this._cantShootDistance = MathF.Min(this._cantShootDistance, num);
							this._shootPosition = Vec2.Invalid;
						}
					}
					else
					{
						this._cantShootDistance = MathF.Max(this._cantShootDistance, num);
						this._cantShoot = false;
						if (((!this._targetFormation.IsRangedFormation && !this._targetFormation.IsRangedCavalryFormation) || (num < this._targetFormation.MissileRangeAdjusted && this._targetFormation.MakingRangedAttackRatio < 0.1f)) && num < MathF.Min(this._archerFormation.QuerySystem.MissileRangeAdjusted * 0.4f, this._cantShootDistance * 0.667f))
						{
							this._behaviorState = BehaviorCautiousAdvance.BehaviorState.PullingBack;
							this._shootPosition = Vec2.Invalid;
						}
					}
					break;
				case BehaviorCautiousAdvance.BehaviorState.PullingBack:
					if (num > MathF.Min(this._cantShootDistance, this._archerFormation.QuerySystem.MissileRangeAdjusted) * 0.8f)
					{
						this._behaviorState = BehaviorCautiousAdvance.BehaviorState.Shooting;
						this._cantShoot = false;
						this._shootPosition = base.Formation.CachedAveragePosition + vec * 5f;
						flag = true;
					}
					break;
				}
				switch (this._behaviorState)
				{
				case BehaviorCautiousAdvance.BehaviorState.Approaching:
				{
					worldPosition = this._targetFormation.Formation.CachedMedianPosition;
					Formation.FormationIntegrityDataGroup cachedFormationIntegrityData = base.Formation.CachedFormationIntegrityData;
					if (this._switchedToShieldWallRecently && !this._switchedToShieldWallTimer.Check(Mission.Current.CurrentTime) && cachedFormationIntegrityData.DeviationOfPositionsExcludeFarAgents > cachedFormationIntegrityData.AverageMaxUnlimitedSpeedExcludeFarAgents * 0.5f)
					{
						if (this._reformPosition.IsValid)
						{
							worldPosition.SetVec2(this._reformPosition);
						}
						else
						{
							vec = (base.Formation.QuerySystem.Team.MedianTargetFormationPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized();
							this._reformPosition = base.Formation.CachedAveragePosition + vec * 5f;
							worldPosition.SetVec2(this._reformPosition);
						}
					}
					else
					{
						this._switchedToShieldWallRecently = false;
						this._reformPosition = Vec2.Invalid;
						worldPosition.SetVec2(this._targetFormation.Formation.CachedAveragePosition);
					}
					break;
				}
				case BehaviorCautiousAdvance.BehaviorState.Shooting:
					if (this._shootPosition.IsValid)
					{
						worldPosition.SetVec2(this._shootPosition);
					}
					else
					{
						worldPosition.SetVec2(base.Formation.CachedAveragePosition);
					}
					break;
				case BehaviorCautiousAdvance.BehaviorState.PullingBack:
					worldPosition = base.Formation.CachedMedianPosition;
					worldPosition.SetVec2(base.Formation.CachedAveragePosition);
					break;
				}
			}
			if (!base.CurrentOrder.CreateNewOrderWorldPositionMT(base.Formation, WorldPosition.WorldPositionEnforcedCache.None).IsValid || this._behaviorState != BehaviorCautiousAdvance.BehaviorState.Shooting || flag || base.CurrentOrder.CreateNewOrderWorldPositionMT(base.Formation, WorldPosition.WorldPositionEnforcedCache.NavMeshVec3).GetNavMeshVec3().DistanceSquared(worldPosition.GetNavMeshVec3()) >= base.Formation.Depth * base.Formation.Depth)
			{
				base.CurrentOrder = MovementOrder.MovementOrderMove(worldPosition);
			}
			if (!this.CurrentFacingOrder.GetDirection(base.Formation, null).IsValid || this._behaviorState != BehaviorCautiousAdvance.BehaviorState.Shooting || flag || this.CurrentFacingOrder.GetDirection(base.Formation, null).DotProduct(vec) <= MBMath.Lerp(0.5f, 1f, 1f - MBMath.ClampFloat(base.Formation.Width, 1f, 20f) * 0.05f, 1E-05f))
			{
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
			}
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0001BE24 File Offset: 0x0001A024
		protected override void OnBehaviorActivatedAux()
		{
			IEnumerable<Formation> enumerable = base.Formation.Team.FormationsIncludingEmpty.WhereQ<Formation>((Formation f) => f.CountOfUnits > 0 && f != base.Formation && f.QuerySystem.IsRangedFormation);
			if (enumerable.AnyQ<Formation>())
			{
				this._archerFormation = enumerable.MaxBy<Formation, float>((Formation f) => f.QuerySystem.FormationPower);
			}
			this._cantShoot = false;
			this._cantShootDistance = float.MaxValue;
			this._behaviorState = BehaviorCautiousAdvance.BehaviorState.Shooting;
			this._cantShootTimer.Reset(Mission.Current.CurrentTime, (this._archerFormation == null) ? 10f : MBMath.Lerp(10f, 15f, (MBMath.ClampFloat((float)this._archerFormation.CountOfUnits, 10f, 60f) - 10f) * 0.02f, 1E-05f));
			this._targetFormation = base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation ?? base.Formation.CachedClosestEnemyFormation;
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			this._isInShieldWallDistance = true;
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0001BF83 File Offset: 0x0001A183
		public override void OnBehaviorCanceled()
		{
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0001BF88 File Offset: 0x0001A188
		public override void TickOccasionally()
		{
			this._targetFormation = base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation ?? base.Formation.CachedClosestEnemyFormation;
			if (base.Formation.PhysicalClass.IsMeleeInfantry())
			{
				bool flag = this._targetFormation != null && (base.Formation.QuerySystem.IsUnderRangedAttack || base.Formation.CachedAveragePosition.DistanceSquared(base.CurrentOrder.GetPosition(base.Formation)) < 25f + (this._isInShieldWallDistance ? 75f : 0f)) && base.Formation.CachedAveragePosition.DistanceSquared(this._targetFormation.Formation.CachedMedianPosition.AsVec2) > 100f - (this._isInShieldWallDistance ? 75f : 0f);
				if (flag != this._isInShieldWallDistance)
				{
					this._isInShieldWallDistance = flag;
					if (this._isInShieldWallDistance)
					{
						ArrangementOrder arrangementOrder = (base.Formation.QuerySystem.HasShield ? ArrangementOrder.ArrangementOrderShieldWall : ArrangementOrder.ArrangementOrderLoose);
						if (base.Formation.ArrangementOrder != arrangementOrder)
						{
							base.Formation.SetArrangementOrder(arrangementOrder);
							this._switchedToShieldWallRecently = true;
							this._switchedToShieldWallTimer.Reset(Mission.Current.CurrentTime, 5f);
						}
					}
					else if (!(base.Formation.ArrangementOrder == ArrangementOrder.ArrangementOrderLine))
					{
						base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
					}
				}
			}
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0001C148 File Offset: 0x0001A348
		protected override float GetAiWeight()
		{
			return 1f;
		}

		// Token: 0x04000330 RID: 816
		private bool _isInShieldWallDistance;

		// Token: 0x04000331 RID: 817
		private bool _switchedToShieldWallRecently;

		// Token: 0x04000332 RID: 818
		private Timer _switchedToShieldWallTimer;

		// Token: 0x04000333 RID: 819
		private Vec2 _reformPosition = Vec2.Invalid;

		// Token: 0x04000334 RID: 820
		private Formation _archerFormation;

		// Token: 0x04000335 RID: 821
		private bool _cantShoot;

		// Token: 0x04000336 RID: 822
		private float _cantShootDistance = float.MaxValue;

		// Token: 0x04000337 RID: 823
		private BehaviorCautiousAdvance.BehaviorState _behaviorState = BehaviorCautiousAdvance.BehaviorState.Shooting;

		// Token: 0x04000338 RID: 824
		private Timer _cantShootTimer;

		// Token: 0x04000339 RID: 825
		private Vec2 _shootPosition = Vec2.Invalid;

		// Token: 0x0400033A RID: 826
		private FormationQuerySystem _targetFormation;

		// Token: 0x0200043A RID: 1082
		private enum BehaviorState
		{
			// Token: 0x040019BE RID: 6590
			Approaching,
			// Token: 0x040019BF RID: 6591
			Shooting,
			// Token: 0x040019C0 RID: 6592
			PullingBack
		}
	}
}
