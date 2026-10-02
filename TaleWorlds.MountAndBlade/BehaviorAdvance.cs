using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200010F RID: 271
	public sealed class BehaviorAdvance : BehaviorComponent
	{
		// Token: 0x06000DCE RID: 3534 RVA: 0x0001A642 File Offset: 0x00018842
		public BehaviorAdvance(Formation formation)
			: base(formation)
		{
			base.BehaviorCoherence = 0.8f;
			this._switchedToShieldWallTimer = new Timer(0f, 0f, true);
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x0001A680 File Offset: 0x00018880
		protected override void CalculateCurrentOrder()
		{
			Formation.FormationIntegrityDataGroup cachedFormationIntegrityData = base.Formation.CachedFormationIntegrityData;
			if (this._switchedToShieldWallRecently && !this._switchedToShieldWallTimer.Check(Mission.Current.CurrentTime) && cachedFormationIntegrityData.DeviationOfPositionsExcludeFarAgents > cachedFormationIntegrityData.AverageMaxUnlimitedSpeedExcludeFarAgents * 0.5f)
			{
				WorldPosition cachedMedianPosition = base.Formation.CachedMedianPosition;
				if (this._reformPosition.IsValid)
				{
					cachedMedianPosition.SetVec2(this._reformPosition);
				}
				else
				{
					Vec2 vec = (base.Formation.QuerySystem.Team.MedianTargetFormationPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized();
					this._reformPosition = base.Formation.CachedAveragePosition + vec * 5f;
					cachedMedianPosition.SetVec2(this._reformPosition);
				}
				base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
				return;
			}
			this._switchedToShieldWallRecently = false;
			bool flag = false;
			FormationQuerySystem cachedClosestEnemyFormation = base.Formation.CachedClosestEnemyFormation;
			if (cachedClosestEnemyFormation != null && cachedClosestEnemyFormation.IsCavalryFormation)
			{
				Vec2 vec2 = base.Formation.CachedAveragePosition - cachedClosestEnemyFormation.Formation.CachedAveragePosition;
				float num = vec2.Normalize();
				Vec2 cachedCurrentVelocity = cachedClosestEnemyFormation.Formation.CachedCurrentVelocity;
				float num2 = cachedCurrentVelocity.Normalize();
				if (num < 30f && num2 > 2f && vec2.DotProduct(cachedCurrentVelocity) > 0.5f)
				{
					flag = true;
					WorldPosition cachedMedianPosition2 = base.Formation.CachedMedianPosition;
					if (this._reformPosition.IsValid)
					{
						cachedMedianPosition2.SetVec2(this._reformPosition);
					}
					else
					{
						Vec2 vec3 = (base.Formation.QuerySystem.Team.MedianTargetFormationPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized();
						this._reformPosition = base.Formation.CachedAveragePosition + vec3 * 5f;
						cachedMedianPosition2.SetVec2(this._reformPosition);
					}
					base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition2);
				}
			}
			if (!flag)
			{
				this._reformPosition = Vec2.Invalid;
				int num3 = 0;
				bool flag2 = false;
				foreach (Team team in Mission.Current.Teams)
				{
					if (team.IsEnemyOf(base.Formation.Team))
					{
						using (List<Formation>.Enumerator enumerator2 = team.FormationsIncludingSpecialAndEmpty.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								if (enumerator2.Current.CountOfUnits > 0)
								{
									num3++;
									flag2 = num3 == 1;
									if (num3 > 1)
									{
										break;
									}
								}
							}
						}
					}
				}
				FormationQuerySystem formationQuerySystem = (flag2 ? base.Formation.CachedClosestEnemyFormation : base.Formation.QuerySystem.Team.MedianTargetFormation);
				if (formationQuerySystem != null)
				{
					WorldPosition cachedMedianPosition3 = formationQuerySystem.Formation.CachedMedianPosition;
					cachedMedianPosition3.SetVec2(cachedMedianPosition3.AsVec2 + formationQuerySystem.Formation.Direction * formationQuerySystem.Formation.Depth * 0.5f);
					Vec2 vec4 = -formationQuerySystem.Formation.Direction;
					base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition3);
					this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec4);
					return;
				}
				FormationQuerySystem medianTargetFormation = base.Formation.QuerySystem.Team.MedianTargetFormation;
				WorldPosition worldPosition = (flag2 ? base.Formation.CachedClosestEnemyFormation.Formation.CachedMedianPosition : ((medianTargetFormation != null) ? base.Formation.QuerySystem.Team.MedianTargetFormationPosition : WorldPosition.Invalid));
				Vec2 vec5 = ((medianTargetFormation != null) ? (base.Formation.QuerySystem.Team.MedianTargetFormationPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized() : Vec2.Invalid);
				if (worldPosition.IsValid)
				{
					base.CurrentOrder = MovementOrder.MovementOrderMove(worldPosition);
				}
				if (vec5.IsValid)
				{
					this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec5);
				}
			}
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x0001AACC File Offset: 0x00018CCC
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			this._isInShieldWallDistance = false;
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x0001AB3C File Offset: 0x00018D3C
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (base.Formation.PhysicalClass.IsMeleeInfantry())
			{
				bool flag = false;
				if (base.Formation.CachedClosestEnemyFormation != null && base.Formation.QuerySystem.IsUnderRangedAttack)
				{
					float num = base.Formation.CachedAveragePosition.DistanceSquared(base.Formation.CachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2);
					if (num < 6400f + (this._isInShieldWallDistance ? 3600f : 0f) && num > 100f - (this._isInShieldWallDistance ? 75f : 0f))
					{
						flag = true;
					}
				}
				if (flag != this._isInShieldWallDistance)
				{
					this._isInShieldWallDistance = flag;
					if (this._isInShieldWallDistance)
					{
						if (base.Formation.QuerySystem.HasShield)
						{
							base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderShieldWall);
						}
						else
						{
							base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
						}
						this._switchedToShieldWallRecently = true;
						this._switchedToShieldWallTimer.Reset(Mission.Current.CurrentTime, 5f);
					}
					else
					{
						base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
					}
				}
			}
			base.Formation.SetMovementOrder(base.CurrentOrder);
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0001ACA4 File Offset: 0x00018EA4
		protected override float GetAiWeight()
		{
			return 1f;
		}

		// Token: 0x0400031E RID: 798
		private bool _isInShieldWallDistance;

		// Token: 0x0400031F RID: 799
		private bool _switchedToShieldWallRecently;

		// Token: 0x04000320 RID: 800
		private Timer _switchedToShieldWallTimer;

		// Token: 0x04000321 RID: 801
		private Vec2 _reformPosition = Vec2.Invalid;
	}
}
