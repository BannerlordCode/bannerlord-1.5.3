using System;
using System.Linq;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000173 RID: 371
	public class TacticFrontalCavalryCharge : TacticComponent
	{
		// Token: 0x0600136D RID: 4973 RVA: 0x00044379 File Offset: 0x00042579
		public TacticFrontalCavalryCharge(Team team)
			: base(team)
		{
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x00044384 File Offset: 0x00042584
		protected override void ManageFormationCounts()
		{
			base.ManageFormationCounts(1, 1, 1, 1);
			this._mainInfantry = TacticComponent.ChooseAndSortByPriority(base.FormationsIncludingEmpty, (Formation f) => f.CountOfUnits > 0 && f.QuerySystem.IsInfantryFormation, (Formation f) => f.IsAIControlled, (Formation f) => f.QuerySystem.FormationPower).FirstOrDefault<Formation>();
			if (this._mainInfantry != null)
			{
				this._mainInfantry.AI.IsMainFormation = true;
			}
			this._archers = TacticComponent.ChooseAndSortByPriority(base.FormationsIncludingEmpty, (Formation f) => f.CountOfUnits > 0 && f.QuerySystem.IsRangedFormation, (Formation f) => f.IsAIControlled, (Formation f) => f.QuerySystem.FormationPower).FirstOrDefault<Formation>();
			this._cavalry = TacticComponent.ChooseAndSortByPriority(base.FormationsIncludingEmpty, (Formation f) => f.CountOfUnits > 0 && f.QuerySystem.IsCavalryFormation, (Formation f) => f.IsAIControlled, (Formation f) => f.QuerySystem.FormationPower).FirstOrDefault<Formation>();
			this._rangedCavalry = TacticComponent.ChooseAndSortByPriority(base.FormationsIncludingEmpty, (Formation f) => f.CountOfUnits > 0 && f.QuerySystem.IsRangedCavalryFormation, (Formation f) => f.IsAIControlled, (Formation f) => f.QuerySystem.FormationPower).FirstOrDefault<Formation>();
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x00044580 File Offset: 0x00042780
		private void Advance()
		{
			if (base.Team.IsPlayerTeam && !base.Team.IsPlayerGeneral && base.Team.IsPlayerSergeant)
			{
				base.SoundTacticalHorn(TacticComponent.MoveHornSoundIndex);
			}
			if (this._mainInfantry != null)
			{
				this._mainInfantry.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._mainInfantry);
				this._mainInfantry.AI.SetBehaviorWeight<BehaviorAdvance>(1f);
			}
			if (this._archers != null)
			{
				this._archers.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._archers);
				this._archers.AI.SetBehaviorWeight<BehaviorSkirmishLine>(1f);
				this._archers.AI.SetBehaviorWeight<BehaviorScreenedSkirmish>(1f);
				this._archers.AI.SetBehaviorWeight<BehaviorSkirmish>(1f);
			}
			if (this._cavalry != null)
			{
				this._cavalry.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._cavalry);
				this._cavalry.AI.SetBehaviorWeight<BehaviorAdvance>(1f);
				this._cavalry.AI.SetBehaviorWeight<BehaviorVanguard>(1f);
			}
			if (this._rangedCavalry != null)
			{
				this._rangedCavalry.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._rangedCavalry);
				this._rangedCavalry.AI.SetBehaviorWeight<BehaviorMountedSkirmish>(1f);
				this._rangedCavalry.AI.SetBehaviorWeight<BehaviorHorseArcherSkirmish>(1f);
			}
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x000446FC File Offset: 0x000428FC
		private void Attack()
		{
			if (base.Team.IsPlayerTeam && !base.Team.IsPlayerGeneral && base.Team.IsPlayerSergeant)
			{
				base.SoundTacticalHorn(TacticComponent.AttackHornSoundIndex);
			}
			if (this._mainInfantry != null)
			{
				this._mainInfantry.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._mainInfantry);
				this._mainInfantry.AI.SetBehaviorWeight<BehaviorAdvance>(1f);
				this._mainInfantry.AI.SetBehaviorWeight<BehaviorTacticalCharge>(1f);
			}
			if (this._archers != null)
			{
				this._archers.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._archers);
				this._archers.AI.SetBehaviorWeight<BehaviorScreenedSkirmish>(1f);
				this._archers.AI.SetBehaviorWeight<BehaviorSkirmish>(1f);
			}
			if (this._cavalry != null)
			{
				this._cavalry.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._cavalry);
				this._cavalry.AI.SetBehaviorWeight<BehaviorFlank>(1f);
				this._cavalry.AI.SetBehaviorWeight<BehaviorTacticalCharge>(1f);
			}
			if (this._rangedCavalry != null)
			{
				this._rangedCavalry.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._rangedCavalry);
				this._rangedCavalry.AI.SetBehaviorWeight<BehaviorMountedSkirmish>(1f);
				this._rangedCavalry.AI.SetBehaviorWeight<BehaviorHorseArcherSkirmish>(1f);
			}
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x00044878 File Offset: 0x00042A78
		private bool HasBattleBeenJoined()
		{
			Formation cavalry = this._cavalry;
			return ((cavalry != null) ? cavalry.CachedClosestEnemyFormation : null) == null || this._cavalry.AI.ActiveBehavior is BehaviorCharge || this._cavalry.AI.ActiveBehavior is BehaviorTacticalCharge || this._cavalry.CachedMedianPosition.AsVec2.Distance(this._cavalry.CachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2) / this._cavalry.CachedClosestEnemyFormation.MovementSpeedMaximum <= 7f + (this._hasBattleBeenJoined ? 7f : 0f);
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x00044938 File Offset: 0x00042B38
		protected override bool CheckAndSetAvailableFormationsChanged()
		{
			int aicontrolledFormationCount = base.Team.GetAIControlledFormationCount();
			bool flag = aicontrolledFormationCount != this._AIControlledFormationCount;
			if (flag)
			{
				this._AIControlledFormationCount = aicontrolledFormationCount;
				this.IsTacticReapplyNeeded = true;
			}
			return flag || (this._mainInfantry != null && (this._mainInfantry.CountOfUnits == 0 || !this._mainInfantry.QuerySystem.IsInfantryFormation)) || (this._archers != null && (this._archers.CountOfUnits == 0 || !this._archers.QuerySystem.IsRangedFormation)) || (this._cavalry != null && (this._cavalry.CountOfUnits == 0 || !this._cavalry.QuerySystem.IsCavalryFormation)) || (this._rangedCavalry != null && (this._rangedCavalry.CountOfUnits == 0 || !this._rangedCavalry.QuerySystem.IsRangedCavalryFormation));
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x00044A1C File Offset: 0x00042C1C
		public override void TickOccasionally()
		{
			if (!base.AreFormationsCreated)
			{
				return;
			}
			if (this.CheckAndSetAvailableFormationsChanged())
			{
				this.ManageFormationCounts();
				if (this._hasBattleBeenJoined)
				{
					this.Attack();
				}
				else
				{
					this.Advance();
				}
				this.IsTacticReapplyNeeded = false;
			}
			bool flag = this.HasBattleBeenJoined();
			if (flag != this._hasBattleBeenJoined || this.IsTacticReapplyNeeded)
			{
				this._hasBattleBeenJoined = flag;
				if (this._hasBattleBeenJoined)
				{
					this.Attack();
				}
				else
				{
					this.Advance();
				}
				this.IsTacticReapplyNeeded = false;
			}
			base.TickOccasionally();
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x00044AA0 File Offset: 0x00042CA0
		protected internal override float GetTacticWeight()
		{
			float num = base.Team.QuerySystem.RangedCavalryRatio * (float)base.Team.QuerySystem.MemberCount;
			return base.Team.QuerySystem.CavalryRatio * (float)base.Team.QuerySystem.MemberCount / ((float)base.Team.QuerySystem.MemberCount - num) * MathF.Sqrt(base.Team.QuerySystem.RemainingPowerRatio);
		}

		// Token: 0x040004F0 RID: 1264
		private Formation _cavalry;

		// Token: 0x040004F1 RID: 1265
		private bool _hasBattleBeenJoined;
	}
}
