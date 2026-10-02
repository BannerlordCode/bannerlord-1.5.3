using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000135 RID: 309
	public class BehaviorSkirmish : BehaviorComponent
	{
		// Token: 0x06000F0F RID: 3855 RVA: 0x00025C64 File Offset: 0x00023E64
		public BehaviorSkirmish(Formation formation)
			: base(formation)
		{
			base.BehaviorCoherence = 0.5f;
			this._cantShootTimer = new Timer(0f, 0f, true);
			this._pullBackTimer = new Timer(0f, 0f, true);
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x00025CD4 File Offset: 0x00023ED4
		protected override void CalculateCurrentOrder()
		{
			WorldPosition worldPosition = base.Formation.CachedMedianPosition;
			bool flag = false;
			Vec2 vec;
			if (base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation == null)
			{
				vec = base.Formation.Direction;
				worldPosition.SetVec2(base.Formation.CachedAveragePosition);
			}
			else
			{
				vec = base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition;
				float num = vec.Normalize();
				float num2 = base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedCurrentVelocity.DotProduct(vec);
				float num3 = MBMath.Lerp(5f, 10f, (MBMath.ClampFloat((float)base.Formation.CountOfUnits, 10f, 60f) - 10f) * 0.02f, 1E-05f) * num2;
				num += num3;
				float num4 = MBMath.Lerp(0.1f, 0.33f, 1f - MBMath.ClampFloat((float)base.Formation.CountOfUnits, 1f, 50f) * 0.02f, 1E-05f) * base.Formation.QuerySystem.RangedUnitRatio;
				switch (this._behaviorState)
				{
				case BehaviorSkirmish.BehaviorState.Approaching:
					if (num < this._cantShootDistance * 0.8f)
					{
						this._behaviorState = BehaviorSkirmish.BehaviorState.Shooting;
						this._cantShoot = false;
						flag = true;
					}
					else if (base.Formation.QuerySystem.MakingRangedAttackRatio >= num4 * 1.2f)
					{
						this._behaviorState = BehaviorSkirmish.BehaviorState.Shooting;
						this._cantShoot = false;
						flag = true;
					}
					break;
				case BehaviorSkirmish.BehaviorState.Shooting:
					if (base.Formation.QuerySystem.MakingRangedAttackRatio <= num4)
					{
						if (num > base.Formation.QuerySystem.MaximumMissileRange)
						{
							this._behaviorState = BehaviorSkirmish.BehaviorState.Approaching;
							this._cantShootDistance = MathF.Min(this._cantShootDistance, base.Formation.QuerySystem.MaximumMissileRange * 0.9f);
						}
						else if (!this._cantShoot)
						{
							this._cantShoot = true;
							this._cantShootTimer.Reset(Mission.Current.CurrentTime, MBMath.Lerp(5f, 10f, (MBMath.ClampFloat((float)base.Formation.CountOfUnits, 10f, 60f) - 10f) * 0.02f, 1E-05f));
						}
						else if (this._cantShootTimer.Check(Mission.Current.CurrentTime))
						{
							this._behaviorState = BehaviorSkirmish.BehaviorState.Approaching;
							this._cantShootDistance = MathF.Min(this._cantShootDistance, num);
						}
					}
					else
					{
						this._cantShootDistance = MathF.Max(this._cantShootDistance, num);
						this._cantShoot = false;
						if (this._pullBackTimer.Check(Mission.Current.CurrentTime) && base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.IsInfantryFormation && num < MathF.Min(base.Formation.QuerySystem.MissileRangeAdjusted * 0.4f, this._cantShootDistance * 0.666f))
						{
							this._behaviorState = BehaviorSkirmish.BehaviorState.PullingBack;
							this._pullBackTimer.Reset(Mission.Current.CurrentTime, 10f);
						}
					}
					break;
				case BehaviorSkirmish.BehaviorState.PullingBack:
					if (num > MathF.Min(this._cantShootDistance, base.Formation.QuerySystem.MissileRangeAdjusted) * 0.8f)
					{
						this._behaviorState = BehaviorSkirmish.BehaviorState.Shooting;
						this._cantShoot = false;
						flag = true;
					}
					else if (this._pullBackTimer.Check(Mission.Current.CurrentTime) && base.Formation.QuerySystem.MakingRangedAttackRatio <= num4 * 0.5f)
					{
						this._behaviorState = BehaviorSkirmish.BehaviorState.Shooting;
						this._cantShoot = false;
						flag = true;
						this._pullBackTimer.Reset(Mission.Current.CurrentTime, 5f);
					}
					break;
				}
				switch (this._behaviorState)
				{
				case BehaviorSkirmish.BehaviorState.Approaching:
				{
					bool flag2 = false;
					if (this._alternatePositionUsed)
					{
						float num5 = base.Formation.CachedAveragePosition.DistanceSquared(base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedAveragePosition);
						Vec2 vec2 = (base.Formation.CachedAveragePosition + base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedAveragePosition) * 0.5f;
						bool flag3 = (double)this._alternatePosition.AsVec2.DistanceSquared(vec2) > (double)num5 * 0.0625;
						if (!flag3)
						{
							Scene scene = Mission.Current.Scene;
							Vec3 navMeshVec = this._alternatePosition.GetNavMeshVec3();
							int num6;
							scene.GetNavigationMeshForPosition(in navMeshVec, out num6, 1.5f, false);
							Agent medianAgent = base.Formation.GetMedianAgent(true, true, base.Formation.CachedAveragePosition);
							flag3 = (medianAgent != null && medianAgent.GetCurrentNavigationFaceId() % 10 == 1) != (num6 % 10 == 1);
						}
						if (flag3)
						{
							Agent medianAgent2 = base.Formation.GetMedianAgent(true, true, base.Formation.CachedAveragePosition);
							bool flag4 = medianAgent2 != null && medianAgent2.GetCurrentNavigationFaceId() % 10 == 1;
							Agent medianAgent3 = base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.GetMedianAgent(true, true, base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedAveragePosition);
							if (flag4 == (medianAgent3 != null && medianAgent3.GetCurrentNavigationFaceId() % 10 == 1))
							{
								this._alternatePositionUsed = false;
								this._alternatePosition = WorldPosition.Invalid;
							}
							else
							{
								flag2 = true;
							}
						}
					}
					else if (Mission.Current.MissionTeamAIType == Mission.MissionTeamAITypeEnum.Siege || Mission.Current.MissionTeamAIType == Mission.MissionTeamAITypeEnum.SallyOut)
					{
						Agent medianAgent4 = base.Formation.GetMedianAgent(true, true, base.Formation.CachedAveragePosition);
						bool flag5 = medianAgent4 != null && medianAgent4.GetCurrentNavigationFaceId() % 10 == 1;
						Agent medianAgent5 = base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.GetMedianAgent(true, true, base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedAveragePosition);
						if (flag5 != (medianAgent5 != null && medianAgent5.GetCurrentNavigationFaceId() % 10 == 1))
						{
							this._alternatePositionUsed = true;
							flag2 = true;
						}
					}
					if (this._alternatePositionUsed)
					{
						if (flag2)
						{
							this._alternatePosition = new WorldPosition(Mission.Current.Scene, new Vec3((base.Formation.CachedAveragePosition + base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedAveragePosition) * 0.5f, base.Formation.CachedMedianPosition.GetNavMeshZ(), -1f));
						}
						worldPosition = this._alternatePosition;
					}
					else
					{
						worldPosition = base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition;
						worldPosition.SetVec2(base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedAveragePosition);
					}
					break;
				}
				case BehaviorSkirmish.BehaviorState.Shooting:
					worldPosition.SetVec2(base.Formation.CachedAveragePosition + base.Formation.CachedCurrentVelocity.Normalized() * (base.Formation.Depth * 0.5f));
					break;
				case BehaviorSkirmish.BehaviorState.PullingBack:
					worldPosition = base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition;
					worldPosition.SetVec2(worldPosition.AsVec2 - vec * (base.Formation.QuerySystem.MissileRangeAdjusted - base.Formation.Depth * 0.5f - 10f));
					break;
				}
			}
			if (!base.CurrentOrder.GetPosition(base.Formation).IsValid || this._behaviorState != BehaviorSkirmish.BehaviorState.Shooting || flag)
			{
				base.CurrentOrder = MovementOrder.MovementOrderMove(worldPosition);
			}
			if (!this.CurrentFacingOrder.GetDirection(base.Formation, null).IsValid || this._behaviorState != BehaviorSkirmish.BehaviorState.Shooting || flag)
			{
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
			}
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x000264E5 File Offset: 0x000246E5
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x00026510 File Offset: 0x00024710
		protected override void OnBehaviorActivatedAux()
		{
			this._cantShoot = false;
			this._cantShootDistance = float.MaxValue;
			this._behaviorState = BehaviorSkirmish.BehaviorState.Shooting;
			this._cantShootTimer.Reset(Mission.Current.CurrentTime, MBMath.Lerp(5f, 10f, (MBMath.ClampFloat((float)base.Formation.CountOfUnits, 10f, 60f) - 10f) * 0.02f, 1E-05f));
			this._pullBackTimer.Reset(0f, 0f);
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderScatter);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x000265F4 File Offset: 0x000247F4
		protected override float GetAiWeight()
		{
			FormationQuerySystem querySystem = base.Formation.QuerySystem;
			return MBMath.Lerp(0.1f, 1f, MBMath.ClampFloat(querySystem.RangedUnitRatio + querySystem.RangedCavalryUnitRatio, 0f, 0.5f) * 2f, 1E-05f);
		}

		// Token: 0x0400039C RID: 924
		private bool _cantShoot;

		// Token: 0x0400039D RID: 925
		private float _cantShootDistance = float.MaxValue;

		// Token: 0x0400039E RID: 926
		private bool _alternatePositionUsed;

		// Token: 0x0400039F RID: 927
		private WorldPosition _alternatePosition = WorldPosition.Invalid;

		// Token: 0x040003A0 RID: 928
		private BehaviorSkirmish.BehaviorState _behaviorState = BehaviorSkirmish.BehaviorState.Shooting;

		// Token: 0x040003A1 RID: 929
		private Timer _cantShootTimer;

		// Token: 0x040003A2 RID: 930
		private Timer _pullBackTimer;

		// Token: 0x02000455 RID: 1109
		private enum BehaviorState
		{
			// Token: 0x04001A11 RID: 6673
			Approaching,
			// Token: 0x04001A12 RID: 6674
			Shooting,
			// Token: 0x04001A13 RID: 6675
			PullingBack
		}
	}
}
