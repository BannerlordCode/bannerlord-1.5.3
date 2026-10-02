using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000170 RID: 368
	public class TacticDefensiveEngagement : TacticComponent
	{
		// Token: 0x06001340 RID: 4928 RVA: 0x000422AD File Offset: 0x000404AD
		public TacticDefensiveEngagement(Team team)
			: base(team)
		{
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x000422B6 File Offset: 0x000404B6
		protected override void ManageFormationCounts()
		{
			base.AssignTacticFormations1121();
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x000422C0 File Offset: 0x000404C0
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
				this._mainInfantry.AI.SetBehaviorWeight<BehaviorHoldHighGround>(1f).RangedAllyFormation = this._archers;
				this._mainInfantry.AI.SetBehaviorWeight<BehaviorTacticalCharge>(1f);
			}
			if (this._archers != null)
			{
				this._archers.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._archers);
				this._archers.AI.SetBehaviorWeight<BehaviorSkirmishLine>(1f);
				this._archers.AI.SetBehaviorWeight<BehaviorScreenedSkirmish>(1f);
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

		// Token: 0x06001343 RID: 4931 RVA: 0x000424A0 File Offset: 0x000406A0
		private void Engage()
		{
			if (base.Team.IsPlayerTeam && !base.Team.IsPlayerGeneral && base.Team.IsPlayerSergeant)
			{
				base.SoundTacticalHorn(TacticComponent.AttackHornSoundIndex);
			}
			if (this._mainInfantry != null)
			{
				this._mainInfantry.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._mainInfantry);
				this._mainInfantry.AI.SetBehaviorWeight<BehaviorHoldHighGround>(1f).RangedAllyFormation = this._archers;
				this._mainInfantry.AI.SetBehaviorWeight<BehaviorTacticalCharge>(1f);
			}
			if (this._archers != null)
			{
				this._archers.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._archers);
				this._archers.AI.SetBehaviorWeight<BehaviorSkirmish>(1f);
				this._archers.AI.SetBehaviorWeight<BehaviorScreenedSkirmish>(1f);
			}
			if (this._leftCavalry != null)
			{
				this._leftCavalry.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._leftCavalry);
				this._leftCavalry.AI.SetBehaviorWeight<BehaviorFlank>(1f);
				this._leftCavalry.AI.SetBehaviorWeight<BehaviorTacticalCharge>(1f);
			}
			if (this._rightCavalry != null)
			{
				this._rightCavalry.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._rightCavalry);
				this._rightCavalry.AI.SetBehaviorWeight<BehaviorFlank>(1f);
				this._rightCavalry.AI.SetBehaviorWeight<BehaviorTacticalCharge>(1f);
			}
			if (this._rangedCavalry != null)
			{
				this._rangedCavalry.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._rangedCavalry);
				this._rangedCavalry.AI.SetBehaviorWeight<BehaviorMountedSkirmish>(1f);
				this._rangedCavalry.AI.SetBehaviorWeight<BehaviorHorseArcherSkirmish>(1f);
			}
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x00042674 File Offset: 0x00040874
		private bool HasBattleBeenJoined()
		{
			Formation mainInfantry = this._mainInfantry;
			return ((mainInfantry != null) ? mainInfantry.CachedClosestEnemyFormation : null) == null || this._mainInfantry.AI.ActiveBehavior is BehaviorCharge || this._mainInfantry.AI.ActiveBehavior is BehaviorTacticalCharge || this._mainInfantry.CachedMedianPosition.AsVec2.Distance(this._mainInfantry.CachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2) / this._mainInfantry.CachedClosestEnemyFormation.MovementSpeedMaximum <= 5f + (this._hasBattleBeenJoined ? 5f : 0f);
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x00042734 File Offset: 0x00040934
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

		// Token: 0x06001346 RID: 4934 RVA: 0x00042844 File Offset: 0x00040A44
		public override void TickOccasionally()
		{
			if (base.AreFormationsCreated)
			{
				bool flag = this.HasBattleBeenJoined();
				bool flag2 = this.CheckAndSetAvailableFormationsChanged();
				if (flag2 || flag != this._hasBattleBeenJoined || this.IsTacticReapplyNeeded)
				{
					this._hasBattleBeenJoined = flag;
					if (flag2)
					{
						this.ManageFormationCounts();
					}
					if (this._hasBattleBeenJoined)
					{
						this.Engage();
					}
					else
					{
						this.Defend();
					}
					this.IsTacticReapplyNeeded = false;
				}
			}
			base.TickOccasionally();
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x000428B0 File Offset: 0x00040AB0
		protected internal override float GetTacticWeight()
		{
			if (base.Team.TeamAI.IsDefenseApplicable)
			{
				if (!base.FormationsIncludingEmpty.All<Formation>((Formation f) => f.CountOfUnits == 0 || !f.QuerySystem.IsInfantryFormation))
				{
					Formation formation;
					if ((formation = this._mainInfantry) == null)
					{
						formation = base.FormationsIncludingEmpty.Where<Formation>((Formation f) => f.CountOfUnits > 0 && f.QuerySystem.IsInfantryFormation).MaxBy<Formation, int>((Formation f) => f.CountOfUnits);
					}
					Formation formation2 = formation;
					if (formation2 == null)
					{
						return 0f;
					}
					if (this._mainInfantry == null)
					{
						this._mainInfantry = formation2;
					}
					float num = base.Team.QuerySystem.InfantryRatio + base.Team.QuerySystem.RangedRatio;
					float num2 = this._mainInfantry.CachedAveragePosition.Distance(this._mainInfantry.QuerySystem.HighGroundCloseToForeseenBattleGround);
					float num3 = MBMath.Lerp(0.7f, 1f, (150f - MBMath.ClampFloat(num2, 50f, 150f)) / 100f, 1E-05f);
					return num * 1.1f * TacticComponent.CalculateNotEngagingTacticalAdvantage(base.Team.QuerySystem) * num3 / MathF.Sqrt(base.Team.QuerySystem.RemainingPowerRatio);
				}
			}
			return 0f;
		}

		// Token: 0x040004E8 RID: 1256
		private const float DefendersAdvantage = 1.1f;

		// Token: 0x040004E9 RID: 1257
		private bool _hasBattleBeenJoined;
	}
}
