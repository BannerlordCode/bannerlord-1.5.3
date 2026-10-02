using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200013A RID: 314
	public class BehaviorTacticalCharge : BehaviorComponent
	{
		// Token: 0x06000F2E RID: 3886 RVA: 0x00027488 File Offset: 0x00025688
		public BehaviorTacticalCharge(Formation formation)
			: base(formation)
		{
			this._lastTarget = null;
			base.CurrentOrder = MovementOrder.MovementOrderCharge;
			this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			this._chargeState = BehaviorTacticalCharge.ChargeState.Undetermined;
			base.BehaviorCoherence = 0.5f;
			this._desiredChargeStopDistance = 20f;
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x000274E4 File Offset: 0x000256E4
		public override void TickOccasionally()
		{
			base.TickOccasionally();
			if (base.Formation.AI.ActiveBehavior != this)
			{
				return;
			}
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00027534 File Offset: 0x00025734
		private BehaviorTacticalCharge.ChargeState CheckAndChangeState()
		{
			BehaviorTacticalCharge.ChargeState chargeState = this._chargeState;
			FormationQuerySystem cachedClosestEnemyFormation = base.Formation.CachedClosestEnemyFormation;
			if (cachedClosestEnemyFormation == null)
			{
				chargeState = BehaviorTacticalCharge.ChargeState.Undetermined;
			}
			else
			{
				switch (this._chargeState)
				{
				case BehaviorTacticalCharge.ChargeState.Undetermined:
					if ((!base.Formation.QuerySystem.IsCavalryFormation && !base.Formation.QuerySystem.IsRangedCavalryFormation) || base.Formation.CachedAveragePosition.Distance(cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2) / base.Formation.QuerySystem.MovementSpeedMaximum <= 5f)
					{
						chargeState = BehaviorTacticalCharge.ChargeState.Charging;
					}
					break;
				case BehaviorTacticalCharge.ChargeState.Charging:
					if (this._lastTarget == null || this._lastTarget.Formation.CountOfUnits == 0)
					{
						chargeState = BehaviorTacticalCharge.ChargeState.Undetermined;
					}
					else if (!base.Formation.QuerySystem.IsCavalryFormation && !base.Formation.QuerySystem.IsRangedCavalryFormation)
					{
						if (!base.Formation.QuerySystem.IsInfantryFormation || !cachedClosestEnemyFormation.IsCavalryFormation)
						{
							chargeState = BehaviorTacticalCharge.ChargeState.Charging;
						}
						else
						{
							Vec2 vec = base.Formation.CachedAveragePosition - cachedClosestEnemyFormation.Formation.CachedAveragePosition;
							float num = vec.Normalize();
							Vec2 cachedCurrentVelocity = cachedClosestEnemyFormation.Formation.CachedCurrentVelocity;
							float num2 = cachedCurrentVelocity.Normalize();
							if (num / num2 <= 6f && vec.DotProduct(cachedCurrentVelocity) > 0.5f)
							{
								this._chargeState = BehaviorTacticalCharge.ChargeState.Bracing;
							}
						}
					}
					else if (this._initialChargeDirection.DotProduct(cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition) <= 0f)
					{
						chargeState = BehaviorTacticalCharge.ChargeState.ChargingPast;
					}
					break;
				case BehaviorTacticalCharge.ChargeState.ChargingPast:
					if (this._chargingPastTimer.Check(Mission.Current.CurrentTime) || base.Formation.CachedAveragePosition.Distance(cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2) >= this._desiredChargeStopDistance)
					{
						chargeState = BehaviorTacticalCharge.ChargeState.Reforming;
					}
					break;
				case BehaviorTacticalCharge.ChargeState.Reforming:
					if (this._reformTimer.Check(Mission.Current.CurrentTime) || base.Formation.CachedAveragePosition.Distance(cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2) <= 30f)
					{
						chargeState = BehaviorTacticalCharge.ChargeState.Charging;
					}
					break;
				case BehaviorTacticalCharge.ChargeState.Bracing:
				{
					bool flag = false;
					if (base.Formation.QuerySystem.IsInfantryFormation && cachedClosestEnemyFormation.IsCavalryFormation)
					{
						Vec2 vec2 = base.Formation.CachedAveragePosition - cachedClosestEnemyFormation.Formation.CachedAveragePosition;
						float num3 = vec2.Normalize();
						Vec2 cachedCurrentVelocity2 = cachedClosestEnemyFormation.Formation.CachedCurrentVelocity;
						float num4 = cachedCurrentVelocity2.Normalize();
						if (num3 / num4 <= 8f && vec2.DotProduct(cachedCurrentVelocity2) > 0.33f)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						this._bracePosition = Vec2.Invalid;
						this._chargeState = BehaviorTacticalCharge.ChargeState.Charging;
					}
					break;
				}
				}
			}
			return chargeState;
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x00027830 File Offset: 0x00025A30
		protected override void CalculateCurrentOrder()
		{
			FormationQuerySystem cachedClosestEnemyFormation = base.Formation.CachedClosestEnemyFormation;
			if (cachedClosestEnemyFormation == null)
			{
				base.CurrentOrder = MovementOrder.MovementOrderCharge;
				return;
			}
			if (base.Formation.QuerySystem.IsCavalryFormation || base.Formation.QuerySystem.IsRangedCavalryFormation)
			{
				base.CurrentOrder = MovementOrder.MovementOrderChargeToTarget(cachedClosestEnemyFormation.Formation);
				return;
			}
			BehaviorTacticalCharge.ChargeState chargeState = this.CheckAndChangeState();
			if (chargeState != this._chargeState)
			{
				this._chargeState = chargeState;
				switch (this._chargeState)
				{
				case BehaviorTacticalCharge.ChargeState.Undetermined:
					base.CurrentOrder = MovementOrder.MovementOrderCharge;
					break;
				case BehaviorTacticalCharge.ChargeState.Charging:
					this._lastTarget = cachedClosestEnemyFormation;
					if (base.Formation.QuerySystem.IsCavalryFormation || base.Formation.QuerySystem.IsRangedCavalryFormation)
					{
						this._initialChargeDirection = this._lastTarget.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition;
						float num = this._initialChargeDirection.Normalize();
						this._desiredChargeStopDistance = MBMath.ClampFloat(num, 20f, 50f);
					}
					break;
				case BehaviorTacticalCharge.ChargeState.ChargingPast:
					this._chargingPastTimer = new Timer(Mission.Current.CurrentTime, 5f, true);
					break;
				case BehaviorTacticalCharge.ChargeState.Reforming:
					this._reformTimer = new Timer(Mission.Current.CurrentTime, 2f, true);
					break;
				case BehaviorTacticalCharge.ChargeState.Bracing:
				{
					Vec2 vec = (base.Formation.QuerySystem.Team.MedianTargetFormationPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized();
					this._bracePosition = base.Formation.CachedAveragePosition + vec * 5f;
					break;
				}
				}
			}
			switch (this._chargeState)
			{
			case BehaviorTacticalCharge.ChargeState.Undetermined:
				if (cachedClosestEnemyFormation != null && (base.Formation.QuerySystem.IsCavalryFormation || base.Formation.QuerySystem.IsRangedCavalryFormation))
				{
					base.CurrentOrder = MovementOrder.MovementOrderMove(cachedClosestEnemyFormation.Formation.CachedMedianPosition);
				}
				else
				{
					base.CurrentOrder = MovementOrder.MovementOrderCharge;
				}
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
				return;
			case BehaviorTacticalCharge.ChargeState.Charging:
			{
				if (base.Formation.QuerySystem.IsCavalryFormation || base.Formation.QuerySystem.IsRangedCavalryFormation)
				{
					Vec2 vec2 = (this._lastTarget.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized();
					WorldPosition cachedMedianPosition = this._lastTarget.Formation.CachedMedianPosition;
					Vec2 vec3 = cachedMedianPosition.AsVec2 + vec2 * this._desiredChargeStopDistance;
					cachedMedianPosition.SetVec2(vec3);
					base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
					this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec2);
					return;
				}
				if (base.Formation.Width >= cachedClosestEnemyFormation.Formation.Width * (1f + ((base.Formation.GetReadonlyMovementOrderReference().OrderEnum != MovementOrder.MovementOrderEnum.Charge) ? 0.1f : 0f)))
				{
					base.CurrentOrder = MovementOrder.MovementOrderCharge;
					this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
					return;
				}
				WorldPosition cachedMedianPosition2 = cachedClosestEnemyFormation.Formation.CachedMedianPosition;
				base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition2);
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
				return;
			}
			case BehaviorTacticalCharge.ChargeState.ChargingPast:
			{
				Vec2 vec4 = (base.Formation.CachedAveragePosition - this._lastTarget.Formation.CachedMedianPosition.AsVec2).Normalized();
				this._lastReformDestination = this._lastTarget.Formation.CachedMedianPosition;
				Vec2 vec5 = this._lastTarget.Formation.CachedMedianPosition.AsVec2 + vec4 * this._desiredChargeStopDistance;
				this._lastReformDestination.SetVec2(vec5);
				base.CurrentOrder = MovementOrder.MovementOrderMove(this._lastReformDestination);
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec4);
				return;
			}
			case BehaviorTacticalCharge.ChargeState.Reforming:
				base.CurrentOrder = MovementOrder.MovementOrderMove(this._lastReformDestination);
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
				return;
			case BehaviorTacticalCharge.ChargeState.Bracing:
			{
				WorldPosition cachedMedianPosition3 = base.Formation.CachedMedianPosition;
				cachedMedianPosition3.SetVec2(this._bracePosition);
				base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition3);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x00027C80 File Offset: 0x00025E80
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (base.Formation.QuerySystem.IsCavalryFormation || base.Formation.QuerySystem.IsRangedCavalryFormation)
			{
				base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderSkein);
			}
			else
			{
				base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			}
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x00027D1C File Offset: 0x00025F1C
		public override TextObject GetBehaviorString()
		{
			TextObject behaviorString = base.GetBehaviorString();
			if (base.Formation.CachedClosestEnemyFormation != null)
			{
				behaviorString.SetTextVariable("AI_SIDE", GameTexts.FindText("str_formation_ai_side_strings", base.Formation.CachedClosestEnemyFormation.Formation.AI.Side.ToString()));
				behaviorString.SetTextVariable("CLASS", GameTexts.FindText("str_formation_class_string", base.Formation.CachedClosestEnemyFormation.Formation.PhysicalClass.GetName()));
			}
			return behaviorString;
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000F34 RID: 3892 RVA: 0x00027DAC File Offset: 0x00025FAC
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x00027DB4 File Offset: 0x00025FB4
		private float CalculateAIWeight()
		{
			FormationQuerySystem querySystem = base.Formation.QuerySystem;
			FormationQuerySystem cachedClosestEnemyFormation = base.Formation.CachedClosestEnemyFormation;
			if (cachedClosestEnemyFormation == null)
			{
				return 0f;
			}
			float num = base.Formation.CachedAveragePosition.Distance(cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2) / querySystem.MovementSpeedMaximum;
			float num3;
			if (!querySystem.IsCavalryFormation && !querySystem.IsRangedCavalryFormation)
			{
				float num2 = MBMath.ClampFloat(num, 4f, 10f);
				num3 = MBMath.Lerp(0.8f, 1f, 1f - (num2 - 4f) / 6f, 1E-05f);
			}
			else if (num <= 4f)
			{
				float num4 = MBMath.ClampFloat(num, 0f, 4f);
				num3 = MBMath.Lerp(0.8f, 1.2f, num4 / 4f, 1E-05f);
			}
			else
			{
				float num5 = MBMath.ClampFloat(num, 4f, 10f);
				num3 = MBMath.Lerp(0.8f, 1.2f, 1f - (num5 - 4f) / 6f, 1E-05f);
			}
			float num6 = 1f;
			if (num <= 4f)
			{
				float length = (base.Formation.CachedAveragePosition - cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2).Length;
				if (length > 1E-45f)
				{
					WorldPosition cachedMedianPosition = base.Formation.CachedMedianPosition;
					cachedMedianPosition.SetVec2(base.Formation.CachedAveragePosition);
					float navMeshZ = cachedMedianPosition.GetNavMeshZ();
					if (!float.IsNaN(navMeshZ))
					{
						float num7 = (navMeshZ - cachedClosestEnemyFormation.Formation.CachedMedianPosition.GetNavMeshZ()) / length;
						num6 = MBMath.Lerp(0.9f, 1.1f, (MBMath.ClampFloat(num7, -0.58f, 0.58f) + 0.58f) / 1.16f, 1E-05f);
					}
				}
			}
			float num8 = 1f;
			if (num <= 4f && num >= 1.5f)
			{
				num8 = 1.2f;
			}
			float num9 = 1f;
			if (num <= 4f && cachedClosestEnemyFormation.Formation.CachedClosestEnemyFormation != querySystem)
			{
				num9 = 1.2f;
			}
			float num10 = querySystem.GetClassWeightedFactor(1f, 1f, 1.5f, 1.5f) * cachedClosestEnemyFormation.GetClassWeightedFactor(1f, 1f, 0.5f, 0.5f);
			return num3 * num6 * num8 * num9 * num10;
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x00028028 File Offset: 0x00026228
		protected override float GetAiWeight()
		{
			float num = 0f;
			FormationQuerySystem cachedClosestEnemyFormation = base.Formation.CachedClosestEnemyFormation;
			if (cachedClosestEnemyFormation != null)
			{
				bool flag;
				if (!(base.Formation.Team.TeamAI is TeamAISiegeComponent))
				{
					flag = true;
				}
				else if ((base.Formation.Team.TeamAI as TeamAISiegeComponent).CalculateIsChargePastWallsApplicable(base.Formation.AI.Side))
				{
					flag = true;
				}
				else
				{
					bool flag2 = TeamAISiegeComponent.IsFormationInsideCastle(cachedClosestEnemyFormation.Formation, true, 0.51f);
					flag = flag2 == TeamAISiegeComponent.IsFormationInsideCastle(base.Formation, true, flag2 ? 0.9f : 0.1f);
				}
				if (flag)
				{
					num = this.CalculateAIWeight();
				}
			}
			return num;
		}

		// Token: 0x040003A9 RID: 937
		private BehaviorTacticalCharge.ChargeState _chargeState;

		// Token: 0x040003AA RID: 938
		private FormationQuerySystem _lastTarget;

		// Token: 0x040003AB RID: 939
		private Vec2 _initialChargeDirection;

		// Token: 0x040003AC RID: 940
		private float _desiredChargeStopDistance;

		// Token: 0x040003AD RID: 941
		private WorldPosition _lastReformDestination;

		// Token: 0x040003AE RID: 942
		private Timer _chargingPastTimer;

		// Token: 0x040003AF RID: 943
		private Timer _reformTimer;

		// Token: 0x040003B0 RID: 944
		private Vec2 _bracePosition = Vec2.Invalid;

		// Token: 0x02000457 RID: 1111
		private enum ChargeState
		{
			// Token: 0x04001A19 RID: 6681
			Undetermined,
			// Token: 0x04001A1A RID: 6682
			Charging,
			// Token: 0x04001A1B RID: 6683
			ChargingPast,
			// Token: 0x04001A1C RID: 6684
			Reforming,
			// Token: 0x04001A1D RID: 6685
			Bracing
		}
	}
}
