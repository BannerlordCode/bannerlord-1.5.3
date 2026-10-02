using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000175 RID: 373
	public class TacticHoldChokePoint : TacticComponent
	{
		// Token: 0x0600137D RID: 4989 RVA: 0x000451A4 File Offset: 0x000433A4
		public TacticHoldChokePoint(Team team)
			: base(team)
		{
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x000451AD File Offset: 0x000433AD
		protected override void ManageFormationCounts()
		{
			base.AssignTacticFormations1121();
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x000451B8 File Offset: 0x000433B8
		private void Defend()
		{
			if (base.Team.IsPlayerTeam && !base.Team.IsPlayerGeneral && base.Team.IsPlayerSergeant)
			{
				base.SoundTacticalHorn(TacticComponent.MoveHornSoundIndex);
			}
			if (this._mainInfantry != null)
			{
				this._mainInfantry.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._mainInfantry);
				this._mainInfantry.AI.SetBehaviorWeight<BehaviorDefend>(1f).TacticalDefendPosition = this._chokePointTacticalPosition;
			}
			if (this._archers != null)
			{
				this._archers.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._archers);
				this._archers.AI.SetBehaviorWeight<BehaviorSkirmishLine>(1f);
				this._archers.AI.SetBehaviorWeight<BehaviorScreenedSkirmish>(1f);
				if (this._linkedRangedDefensivePosition != null)
				{
					this._archers.AI.SetBehaviorWeight<BehaviorDefend>(10f).TacticalDefendPosition = this._linkedRangedDefensivePosition;
				}
			}
			if (this._leftCavalry != null)
			{
				this._leftCavalry.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._leftCavalry);
				this._leftCavalry.AI.SetBehaviorWeight<BehaviorProtectFlank>(1f).FlankSide = FormationAI.BehaviorSide.Left;
				this._leftCavalry.AI.SetBehaviorWeight<BehaviorCavalryScreen>(1f);
			}
			if (this._rightCavalry != null)
			{
				this._rightCavalry.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._rightCavalry);
				this._rightCavalry.AI.SetBehaviorWeight<BehaviorProtectFlank>(1f).FlankSide = FormationAI.BehaviorSide.Right;
				this._rightCavalry.AI.SetBehaviorWeight<BehaviorCavalryScreen>(1f);
			}
			if (this._rangedCavalry != null)
			{
				this._rangedCavalry.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._rangedCavalry);
				this._rangedCavalry.AI.SetBehaviorWeight<BehaviorMountedSkirmish>(1f);
				this._rangedCavalry.AI.SetBehaviorWeight<BehaviorHorseArcherSkirmish>(1f);
			}
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x000453A8 File Offset: 0x000435A8
		protected override bool CheckAndSetAvailableFormationsChanged()
		{
			int aicontrolledFormationCount = base.Team.GetAIControlledFormationCount();
			bool flag = aicontrolledFormationCount != this._AIControlledFormationCount;
			if (flag)
			{
				this._AIControlledFormationCount = aicontrolledFormationCount;
				this.IsTacticReapplyNeeded = true;
			}
			return flag || (this._mainInfantry != null && (this._mainInfantry.CountOfUnits == 0 || !this._mainInfantry.QuerySystem.IsInfantryFormation)) || (this._archers != null && (this._archers.CountOfUnits == 0 || !this._archers.QuerySystem.IsRangedFormation)) || (this._leftCavalry != null && (this._leftCavalry.CountOfUnits == 0 || !this._leftCavalry.QuerySystem.IsCavalryFormation)) || (this._rightCavalry != null && (this._rightCavalry.CountOfUnits == 0 || !this._rightCavalry.QuerySystem.IsCavalryFormation)) || (this._rangedCavalry != null && (this._rangedCavalry.CountOfUnits == 0 || !this._rangedCavalry.QuerySystem.IsRangedCavalryFormation));
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x000454B8 File Offset: 0x000436B8
		public override void TickOccasionally()
		{
			if (!base.AreFormationsCreated)
			{
				return;
			}
			if (this.CheckAndSetAvailableFormationsChanged() || this.IsTacticReapplyNeeded)
			{
				this.ManageFormationCounts();
				this.Defend();
				this.IsTacticReapplyNeeded = false;
			}
			base.TickOccasionally();
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x000454EC File Offset: 0x000436EC
		protected internal override float GetTacticWeight()
		{
			if (base.Team.TeamAI.IsDefenseApplicable)
			{
				if (base.CheckAndDetermineFormation(ref this._mainInfantry, (Formation f) => f.CountOfUnits > 0 && f.QuerySystem.IsInfantryFormation))
				{
					if (!base.Team.TeamAI.IsCurrentTactic(this) || this._chokePointTacticalPosition == null || !this.IsTacticalPositionEligible(this._chokePointTacticalPosition))
					{
						this.DetermineChokePoints();
					}
					if (this._chokePointTacticalPosition == null)
					{
						return 0f;
					}
					float infantryRatio = base.Team.QuerySystem.InfantryRatio;
					float num = MathF.Min(infantryRatio, base.Team.QuerySystem.RangedRatio);
					float num2 = infantryRatio + num;
					float num3 = MBMath.ClampFloat((float)base.Team.QuerySystem.EnemyUnitCount / (float)base.Team.QuerySystem.MemberCount, 0.33f, 3f);
					return num2 * num3 * this.GetTacticalPositionScore(this._chokePointTacticalPosition) * TacticComponent.CalculateNotEngagingTacticalAdvantage(base.Team.QuerySystem) * 1.3f / MathF.Sqrt(base.Team.QuerySystem.RemainingPowerRatio);
				}
			}
			return 0f;
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x00045614 File Offset: 0x00043814
		private bool IsTacticalPositionEligible(TacticalPosition tacticalPosition)
		{
			if (!base.CheckAndDetermineFormation(ref this._mainInfantry, (Formation f) => f.CountOfUnits > 0 && f.QuerySystem.IsInfantryFormation))
			{
				return false;
			}
			float num = base.Team.QuerySystem.AveragePosition.Distance(tacticalPosition.Position.AsVec2);
			float num2 = base.Team.QuerySystem.AverageEnemyPosition.Distance(this._mainInfantry.CachedAveragePosition);
			if (num > 20f && num > num2 * 0.5f)
			{
				return false;
			}
			if (this._mainInfantry.MaximumWidth < tacticalPosition.Width)
			{
				return false;
			}
			float num3 = (base.Team.QuerySystem.AverageEnemyPosition - tacticalPosition.Position.AsVec2).Normalized().DotProduct(tacticalPosition.Direction);
			if (tacticalPosition.IsInsurmountable)
			{
				return MathF.Abs(num3) >= 0.5f;
			}
			return num3 >= 0.5f;
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x00045728 File Offset: 0x00043928
		private float GetTacticalPositionScore(TacticalPosition tacticalPosition)
		{
			if (base.CheckAndDetermineFormation(ref this._mainInfantry, (Formation f) => f.CountOfUnits > 0 && f.QuerySystem.IsInfantryFormation))
			{
				float num = MBMath.Lerp(1f, 1.5f, MBMath.ClampFloat(tacticalPosition.Slope, 0f, 60f) / 60f, 1E-05f);
				int countOfUnits = this._mainInfantry.CountOfUnits;
				float num2 = this._mainInfantry.Interval * (float)(countOfUnits - 1) + this._mainInfantry.UnitDiameter * (float)countOfUnits;
				float num3 = MBMath.Lerp(0.67f, 1.5f, (MBMath.ClampFloat(num2 / tacticalPosition.Width, 0.5f, 3f) - 0.5f) / 2.5f, 1E-05f);
				float num4 = 1f;
				if (base.CheckAndDetermineFormation(ref this._archers, (Formation f) => f.CountOfUnits > 0 && f.QuerySystem.IsRangedFormation))
				{
					if (tacticalPosition.LinkedTacticalPositions.Where<TacticalPosition>((TacticalPosition lcp) => lcp.TacticalPositionType == TacticalPosition.TacticalPositionTypeEnum.Cliff).ToList<TacticalPosition>().Count > 0)
					{
						num4 = MBMath.Lerp(1f, 1.5f, (MBMath.ClampFloat(base.Team.QuerySystem.RangedRatio, 0.05f, 0.25f) - 0.05f) * 5f, 1E-05f);
					}
				}
				float num5 = this._mainInfantry.CachedAveragePosition.Distance(tacticalPosition.Position.AsVec2);
				float num6 = MBMath.Lerp(0.7f, 1f, (150f - MBMath.ClampFloat(num5, 50f, 150f)) / 100f, 1E-05f);
				return num * num3 * num4 * num6;
			}
			return 0f;
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x00045907 File Offset: 0x00043B07
		protected internal override bool ResetTacticalPositions()
		{
			this.DetermineChokePoints();
			return true;
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x00045910 File Offset: 0x00043B10
		private void DetermineChokePoints()
		{
			IEnumerable<ValueTuple<TacticalPosition, float>> enumerable = from tp in base.Team.TeamAI.TacticalPositions
				where tp.TacticalPositionType == TacticalPosition.TacticalPositionTypeEnum.ChokePoint && this.IsTacticalPositionEligible(tp)
				select new ValueTuple<TacticalPosition, float>(tp, this.GetTacticalPositionScore(tp));
			IEnumerable<ValueTuple<TacticalPosition, float>> enumerable2 = from tp in base.Team.TeamAI.TacticalRegions.SelectMany<TacticalRegion, TacticalPosition>((TacticalRegion r) => r.LinkedTacticalPositions.Where<TacticalPosition>((TacticalPosition tpftr) => tpftr.TacticalPositionType == TacticalPosition.TacticalPositionTypeEnum.ChokePoint && this.IsTacticalPositionEligible(tpftr)))
				select new ValueTuple<TacticalPosition, float>(tp, this.GetTacticalPositionScore(tp));
			IEnumerable<ValueTuple<TacticalPosition, float>> enumerable3 = enumerable.Concat<ValueTuple<TacticalPosition, float>>(enumerable2);
			if (enumerable3.Any<ValueTuple<TacticalPosition, float>>())
			{
				TacticalPosition item = enumerable3.MaxBy<ValueTuple<TacticalPosition, float>, float>(([TupleElementNames(new string[] { "tp", null })] ValueTuple<TacticalPosition, float> pst) => pst.Item2).Item1;
				if (item != this._chokePointTacticalPosition)
				{
					this._chokePointTacticalPosition = item;
					this.IsTacticReapplyNeeded = true;
				}
				if (this._chokePointTacticalPosition.LinkedTacticalPositions.Count <= 0)
				{
					this._linkedRangedDefensivePosition = null;
					return;
				}
				TacticalPosition tacticalPosition = this._chokePointTacticalPosition.LinkedTacticalPositions.FirstOrDefault<TacticalPosition>();
				if (tacticalPosition != this._linkedRangedDefensivePosition)
				{
					this._linkedRangedDefensivePosition = tacticalPosition;
					this.IsTacticReapplyNeeded = true;
					return;
				}
			}
			else
			{
				this._chokePointTacticalPosition = null;
			}
		}

		// Token: 0x040004F3 RID: 1267
		private const float DefendersAdvantage = 1.3f;

		// Token: 0x040004F4 RID: 1268
		private TacticalPosition _chokePointTacticalPosition;

		// Token: 0x040004F5 RID: 1269
		private TacticalPosition _linkedRangedDefensivePosition;
	}
}
