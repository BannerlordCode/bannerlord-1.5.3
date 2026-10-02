using System;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200016E RID: 366
	public class TacticCoordinatedRetreat : TacticComponent
	{
		// Token: 0x06001321 RID: 4897 RVA: 0x0003EC8F File Offset: 0x0003CE8F
		public TacticCoordinatedRetreat(Team team)
			: base(team)
		{
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x0003ECA3 File Offset: 0x0003CEA3
		protected override void ManageFormationCounts()
		{
			if (!this._canWeSafelyRunAway)
			{
				base.AssignTacticFormations1121();
			}
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x0003ECB4 File Offset: 0x0003CEB4
		private void OrganizedRetreat()
		{
			if (base.Team.IsPlayerTeam && !base.Team.IsPlayerGeneral && base.Team.IsPlayerSergeant)
			{
				base.SoundTacticalHorn(TacticComponent.RetreatHornSoundIndex);
			}
			if (this._mainInfantry != null)
			{
				this._mainInfantry.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._mainInfantry);
				BehaviorDefend behaviorDefend = this._mainInfantry.AI.SetBehaviorWeight<BehaviorDefend>(1f);
				WorldPosition closestFleePositionForFormation = Mission.Current.GetClosestFleePositionForFormation(this._mainInfantry);
				closestFleePositionForFormation.SetVec2(Mission.Current.GetClosestBoundaryPosition(closestFleePositionForFormation.AsVec2));
				this._retreatPosition = closestFleePositionForFormation.AsVec2;
				behaviorDefend.DefensePosition = closestFleePositionForFormation;
				this._mainInfantry.AI.SetBehaviorWeight<BehaviorPullBack>(1f);
			}
			if (this._archers != null)
			{
				this._archers.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._archers);
				this._archers.AI.SetBehaviorWeight<BehaviorScreenedSkirmish>(1f);
				this._archers.AI.SetBehaviorWeight<BehaviorPullBack>(1.5f);
			}
			if (this._leftCavalry != null)
			{
				this._leftCavalry.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._leftCavalry);
				this._leftCavalry.AI.SetBehaviorWeight<BehaviorProtectFlank>(1f).FlankSide = FormationAI.BehaviorSide.Left;
				this._leftCavalry.AI.SetBehaviorWeight<BehaviorCavalryScreen>(1f);
				this._leftCavalry.AI.SetBehaviorWeight<BehaviorPullBack>(1.5f);
			}
			if (this._rightCavalry != null)
			{
				this._rightCavalry.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._rightCavalry);
				this._rightCavalry.AI.SetBehaviorWeight<BehaviorProtectFlank>(1f).FlankSide = FormationAI.BehaviorSide.Right;
				this._rightCavalry.AI.SetBehaviorWeight<BehaviorCavalryScreen>(1f);
				this._rightCavalry.AI.SetBehaviorWeight<BehaviorPullBack>(1.5f);
			}
			if (this._rangedCavalry != null)
			{
				this._rangedCavalry.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(this._rangedCavalry);
				this._rangedCavalry.AI.SetBehaviorWeight<BehaviorMountedSkirmish>(1f);
				this._rangedCavalry.AI.SetBehaviorWeight<BehaviorPullBack>(1.5f);
				this._rangedCavalry.AI.SetBehaviorWeight<BehaviorHorseArcherSkirmish>(1f);
			}
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x0003EF08 File Offset: 0x0003D108
		private void RunForTheBorder()
		{
			if (base.Team.IsPlayerTeam && !base.Team.IsPlayerGeneral && base.Team.IsPlayerSergeant)
			{
				base.SoundTacticalHorn(TacticComponent.RetreatHornSoundIndex);
			}
			foreach (Formation formation in base.FormationsIncludingSpecialAndEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					formation.AI.ResetBehaviorWeights();
					formation.AI.SetBehaviorWeight<BehaviorRetreat>(1f);
				}
			}
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x0003EFAC File Offset: 0x0003D1AC
		private bool HasRetreatDestinationBeenReached()
		{
			return base.FormationsIncludingEmpty.All<Formation>((Formation f) => f.CountOfUnits == 0 || !f.QuerySystem.IsInfantryFormation || f.CachedAveragePosition.DistanceSquared(this._retreatPosition) < 5625f);
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x0003EFC8 File Offset: 0x0003D1C8
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

		// Token: 0x06001327 RID: 4903 RVA: 0x0003F0D8 File Offset: 0x0003D2D8
		public override void TickOccasionally()
		{
			if (!base.AreFormationsCreated)
			{
				return;
			}
			bool flag = this.HasRetreatDestinationBeenReached();
			if (this.CheckAndSetAvailableFormationsChanged())
			{
				this._canWeSafelyRunAway = flag;
				this.ManageFormationCounts();
				if (this._canWeSafelyRunAway)
				{
					this.RunForTheBorder();
				}
				else
				{
					this.OrganizedRetreat();
				}
				this.IsTacticReapplyNeeded = false;
			}
			if (flag != this._canWeSafelyRunAway || this.IsTacticReapplyNeeded)
			{
				this._canWeSafelyRunAway = flag;
				if (this._canWeSafelyRunAway)
				{
					this.RunForTheBorder();
				}
				else
				{
					this.OrganizedRetreat();
				}
				this.IsTacticReapplyNeeded = false;
			}
			base.TickOccasionally();
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x0003F164 File Offset: 0x0003D364
		protected internal override float GetTacticWeight()
		{
			float num = base.Team.QuerySystem.TotalPowerRatio / base.Team.QuerySystem.RemainingPowerRatio;
			float num2 = MathF.Max(base.Team.QuerySystem.InfantryRatio, MathF.Max(base.Team.QuerySystem.RangedRatio, base.Team.QuerySystem.CavalryRatio));
			float num3 = MBMath.LinearExtrapolation(0f, num2, MBMath.ClampFloat(num, 0f, 4f) / 2f);
			float num4 = 0f;
			int num5 = 0;
			foreach (Team team in base.Team.Mission.Teams)
			{
				if (team.IsEnemyOf(base.Team))
				{
					num5++;
					num4 += team.QuerySystem.CavalryRatio + team.QuerySystem.RangedCavalryRatio;
				}
			}
			if (num5 > 0)
			{
				num4 /= (float)num5;
			}
			float num6 = ((num4 == 0f) ? 1.2f : MBMath.Lerp(0.8f, 1.2f, MBMath.ClampFloat((base.Team.QuerySystem.CavalryRatio + base.Team.QuerySystem.RangedCavalryRatio) / num4, 0f, 2f) / 2f, 1E-05f));
			return num3 * num6 * MathF.Min(1f, MathF.Sqrt(base.Team.QuerySystem.RemainingPowerRatio));
		}

		// Token: 0x040004CF RID: 1231
		private bool _canWeSafelyRunAway;

		// Token: 0x040004D0 RID: 1232
		private Vec2 _retreatPosition = Vec2.Invalid;

		// Token: 0x040004D1 RID: 1233
		private const float RetreatThresholdValue = 2f;
	}
}
