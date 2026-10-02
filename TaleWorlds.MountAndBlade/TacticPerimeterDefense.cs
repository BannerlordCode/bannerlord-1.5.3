using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000176 RID: 374
	public class TacticPerimeterDefense : TacticComponent
	{
		// Token: 0x0600138C RID: 5004 RVA: 0x00045A80 File Offset: 0x00043C80
		public TacticPerimeterDefense(Team team)
			: base(team)
		{
			Scene scene = Mission.Current.Scene;
			FleePosition fleePosition = Mission.Current.GetFleePositionsForSide(BattleSideEnum.Defender).FirstOrDefault<FleePosition>((FleePosition fp) => fp.GetSide() == BattleSideEnum.Defender);
			if (fleePosition != null)
			{
				this._defendPosition = fleePosition.GameEntity.GlobalPosition.ToWorldPosition();
			}
			else
			{
				this._defendPosition = WorldPosition.Invalid;
			}
			this._enemyClusters = new List<TacticPerimeterDefense.EnemyCluster>();
			this._defenseFronts = new List<TacticPerimeterDefense.DefenseFront>();
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x00045B10 File Offset: 0x00043D10
		private void DetermineEnemyClusters()
		{
			List<Formation> list = new List<Formation>();
			float num = 0f;
			foreach (Team team in base.Team.Mission.Teams)
			{
				if (team.IsEnemyOf(base.Team))
				{
					num += team.QuerySystem.TeamPower;
				}
			}
			foreach (Team team2 in base.Team.Mission.Teams)
			{
				if (team2.IsEnemyOf(base.Team))
				{
					for (int i = 0; i < Math.Min(team2.FormationsIncludingSpecialAndEmpty.Count, 8); i++)
					{
						Formation enemyFormation = team2.FormationsIncludingSpecialAndEmpty[i];
						if (enemyFormation.CountOfUnits > 0 && enemyFormation.QuerySystem.FormationPower < MathF.Min(base.Team.QuerySystem.TeamPower, num) / 4f)
						{
							if (!this._enemyClusters.Any<TacticPerimeterDefense.EnemyCluster>((TacticPerimeterDefense.EnemyCluster ec) => ec.EnemyFormations.IndexOf(enemyFormation) >= 0))
							{
								list.Add(enemyFormation);
							}
						}
						else
						{
							TacticPerimeterDefense.EnemyCluster enemyCluster = this._enemyClusters.FirstOrDefault<TacticPerimeterDefense.EnemyCluster>((TacticPerimeterDefense.EnemyCluster ec) => ec.EnemyFormations.IndexOf(enemyFormation) >= 0);
							if (enemyCluster != null)
							{
								if ((double)(this._defendPosition.AsVec2 - enemyCluster.AggregatePosition).DotProduct(this._defendPosition.AsVec2 - enemyFormation.CachedAveragePosition) >= 0.70710678118)
								{
									goto IL_0211;
								}
								enemyCluster.RemoveFromCluster(enemyFormation);
							}
							List<TacticPerimeterDefense.EnemyCluster> list2 = this._enemyClusters.Where<TacticPerimeterDefense.EnemyCluster>((TacticPerimeterDefense.EnemyCluster c) => (double)(this._defendPosition.AsVec2 - c.AggregatePosition).DotProduct(this._defendPosition.AsVec2 - enemyFormation.CachedMedianPosition.AsVec2) >= 0.70710678118).ToList<TacticPerimeterDefense.EnemyCluster>();
							if (list2.Count > 0)
							{
								list2.MaxBy<TacticPerimeterDefense.EnemyCluster, float>((TacticPerimeterDefense.EnemyCluster ec) => (this._defendPosition.AsVec2 - ec.AggregatePosition).DotProduct(this._defendPosition.AsVec2 - enemyFormation.CachedMedianPosition.AsVec2)).AddToCluster(enemyFormation);
							}
							else
							{
								TacticPerimeterDefense.EnemyCluster enemyCluster2 = new TacticPerimeterDefense.EnemyCluster();
								enemyCluster2.AddToCluster(enemyFormation);
								this._enemyClusters.Add(enemyCluster2);
							}
						}
						IL_0211:;
					}
				}
			}
			if (this._enemyClusters.Count > 0)
			{
				using (List<Formation>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Formation skippedFormation = enumerator2.Current;
						this._enemyClusters.MaxBy<TacticPerimeterDefense.EnemyCluster, float>((TacticPerimeterDefense.EnemyCluster ec) => (this._defendPosition.AsVec2 - ec.AggregatePosition).DotProduct(this._defendPosition.AsVec2 - skippedFormation.CachedMedianPosition.AsVec2)).AddToCluster(skippedFormation);
					}
				}
			}
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x00045E28 File Offset: 0x00044028
		private bool MustRetreatToCastle()
		{
			return base.Team.QuerySystem.TotalPowerRatio / base.Team.QuerySystem.RemainingPowerRatio > 2f;
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x00045E54 File Offset: 0x00044054
		private void StartRetreatToKeep()
		{
			foreach (Formation formation in base.FormationsIncludingEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					formation.AI.ResetBehaviorWeights();
					TacticComponent.SetDefaultBehaviorWeights(formation);
					formation.AI.SetBehaviorWeight<BehaviorRetreatToKeep>(1f);
				}
			}
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x00045ECC File Offset: 0x000440CC
		private void CheckAndChangeState()
		{
			if (this.MustRetreatToCastle())
			{
				if (this._isRetreatingToKeep)
				{
					return;
				}
				this._isRetreatingToKeep = true;
				this.StartRetreatToKeep();
			}
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x00045EEC File Offset: 0x000440EC
		private void ArrangeDefenseFronts()
		{
			this._meleeFormations = base.FormationsIncludingEmpty.Where<Formation>((Formation f) => f.CountOfUnits > 0 && (f.QuerySystem.IsInfantryFormation || f.QuerySystem.IsCavalryFormation)).ToList<Formation>();
			this._rangedFormations = base.FormationsIncludingEmpty.Where<Formation>((Formation f) => f.CountOfUnits > 0 && (f.QuerySystem.IsRangedFormation || f.QuerySystem.IsRangedCavalryFormation)).ToList<Formation>();
			int num = MathF.Min(8 - this._rangedFormations.Count, this._enemyClusters.Count);
			if (this._meleeFormations.Count != num)
			{
				base.SplitFormationClassIntoGivenNumber((Formation f) => f.QuerySystem.IsInfantryFormation || f.QuerySystem.IsCavalryFormation, num);
				this._meleeFormations = base.FormationsIncludingEmpty.Where<Formation>((Formation f) => f.CountOfUnits > 0 && (f.QuerySystem.IsInfantryFormation || f.QuerySystem.IsCavalryFormation)).ToList<Formation>();
			}
			int num2 = MathF.Min(8 - num, this._enemyClusters.Count);
			if (this._rangedFormations.Count != num2)
			{
				base.SplitFormationClassIntoGivenNumber((Formation f) => f.QuerySystem.IsRangedFormation || f.QuerySystem.IsRangedCavalryFormation, num2);
				this._rangedFormations = base.FormationsIncludingEmpty.Where<Formation>((Formation f) => f.CountOfUnits > 0 && (f.QuerySystem.IsRangedFormation || f.QuerySystem.IsRangedCavalryFormation)).ToList<Formation>();
			}
			foreach (TacticPerimeterDefense.DefenseFront defenseFront in this._defenseFronts)
			{
				defenseFront.MatchedEnemyCluster.UpdateClusterData();
				BehaviorDefendKeyPosition behaviorDefendKeyPosition = defenseFront.MeleeFormation.AI.SetBehaviorWeight<BehaviorDefendKeyPosition>(1f);
				behaviorDefendKeyPosition.EnemyClusterPosition = defenseFront.MatchedEnemyCluster.MedianAggregatePosition;
				behaviorDefendKeyPosition.EnemyClusterPosition.SetVec2(defenseFront.MatchedEnemyCluster.AggregatePosition);
			}
			IEnumerable<TacticPerimeterDefense.EnemyCluster> enumerable = this._enemyClusters.Where<TacticPerimeterDefense.EnemyCluster>((TacticPerimeterDefense.EnemyCluster ec) => this._defenseFronts.All<TacticPerimeterDefense.DefenseFront>((TacticPerimeterDefense.DefenseFront df) => df.MatchedEnemyCluster != ec));
			List<Formation> list = this._meleeFormations.Where<Formation>((Formation mf) => this._defenseFronts.All<TacticPerimeterDefense.DefenseFront>((TacticPerimeterDefense.DefenseFront df) => df.MeleeFormation != mf)).ToList<Formation>();
			List<Formation> list2 = this._rangedFormations.Where<Formation>((Formation rf) => this._defenseFronts.All<TacticPerimeterDefense.DefenseFront>((TacticPerimeterDefense.DefenseFront df) => df.RangedFormation != rf)).ToList<Formation>();
			foreach (TacticPerimeterDefense.EnemyCluster enemyCluster in enumerable)
			{
				if (list.IsEmpty<Formation>())
				{
					break;
				}
				Formation formation = list[list.Count - 1];
				TacticPerimeterDefense.DefenseFront defenseFront2 = new TacticPerimeterDefense.DefenseFront(enemyCluster, formation);
				formation.AI.ResetBehaviorWeights();
				TacticComponent.SetDefaultBehaviorWeights(formation);
				BehaviorDefendKeyPosition behaviorDefendKeyPosition2 = formation.AI.SetBehaviorWeight<BehaviorDefendKeyPosition>(1f);
				behaviorDefendKeyPosition2.DefensePosition = this._defendPosition;
				behaviorDefendKeyPosition2.EnemyClusterPosition = enemyCluster.MedianAggregatePosition;
				behaviorDefendKeyPosition2.EnemyClusterPosition.SetVec2(enemyCluster.AggregatePosition);
				list.Remove(formation);
				if (!list2.IsEmpty<Formation>())
				{
					Formation formation2 = list2[list2.Count - 1];
					formation2.AI.ResetBehaviorWeights();
					TacticComponent.SetDefaultBehaviorWeights(formation2);
					formation2.AI.SetBehaviorWeight<BehaviorSkirmishBehindFormation>(1f).ReferenceFormation = formation;
					defenseFront2.RangedFormation = formation2;
					list2.Remove(formation2);
					this._defenseFronts.Add(defenseFront2);
				}
			}
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x00046260 File Offset: 0x00044460
		public override void TickOccasionally()
		{
			if (!base.AreFormationsCreated)
			{
				return;
			}
			this.CheckAndChangeState();
			if (!this._isRetreatingToKeep)
			{
				this.DetermineEnemyClusters();
				this.ArrangeDefenseFronts();
			}
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x00046285 File Offset: 0x00044485
		protected internal override float GetTacticWeight()
		{
			if (this._defendPosition.IsValid)
			{
				return 10f;
			}
			return 0f;
		}

		// Token: 0x040004F6 RID: 1270
		private WorldPosition _defendPosition;

		// Token: 0x040004F7 RID: 1271
		private readonly List<TacticPerimeterDefense.EnemyCluster> _enemyClusters;

		// Token: 0x040004F8 RID: 1272
		private readonly List<TacticPerimeterDefense.DefenseFront> _defenseFronts;

		// Token: 0x040004F9 RID: 1273
		private const float RetreatThresholdValue = 2f;

		// Token: 0x040004FA RID: 1274
		private List<Formation> _meleeFormations;

		// Token: 0x040004FB RID: 1275
		private List<Formation> _rangedFormations;

		// Token: 0x040004FC RID: 1276
		private bool _isRetreatingToKeep;

		// Token: 0x020004BF RID: 1215
		private class DefenseFront
		{
			// Token: 0x06003AF5 RID: 15093 RVA: 0x000EE4FF File Offset: 0x000EC6FF
			public DefenseFront(TacticPerimeterDefense.EnemyCluster matchedEnemyCluster, Formation meleeFormation)
			{
				this.MatchedEnemyCluster = matchedEnemyCluster;
				this.MeleeFormation = meleeFormation;
				this.RangedFormation = null;
			}

			// Token: 0x04001C04 RID: 7172
			public Formation MeleeFormation;

			// Token: 0x04001C05 RID: 7173
			public Formation RangedFormation;

			// Token: 0x04001C06 RID: 7174
			public TacticPerimeterDefense.EnemyCluster MatchedEnemyCluster;
		}

		// Token: 0x020004C0 RID: 1216
		private class EnemyCluster
		{
			// Token: 0x17000A4F RID: 2639
			// (get) Token: 0x06003AF6 RID: 15094 RVA: 0x000EE51C File Offset: 0x000EC71C
			// (set) Token: 0x06003AF7 RID: 15095 RVA: 0x000EE524 File Offset: 0x000EC724
			public Vec2 AggregatePosition { get; private set; }

			// Token: 0x17000A50 RID: 2640
			// (get) Token: 0x06003AF8 RID: 15096 RVA: 0x000EE52D File Offset: 0x000EC72D
			// (set) Token: 0x06003AF9 RID: 15097 RVA: 0x000EE535 File Offset: 0x000EC735
			public WorldPosition MedianAggregatePosition { get; private set; }

			// Token: 0x17000A51 RID: 2641
			// (get) Token: 0x06003AFA RID: 15098 RVA: 0x000EE53E File Offset: 0x000EC73E
			public MBReadOnlyList<Formation> EnemyFormations
			{
				get
				{
					return this._enemyFormations;
				}
			}

			// Token: 0x06003AFB RID: 15099 RVA: 0x000EE548 File Offset: 0x000EC748
			public void UpdateClusterData()
			{
				this._totalPower = this._enemyFormations.Sum<Formation>((Formation ef) => ef.QuerySystem.FormationPower);
				this.AggregatePosition = Vec2.Zero;
				foreach (Formation formation in this._enemyFormations)
				{
					this.AggregatePosition += formation.CachedAveragePosition * (formation.QuerySystem.FormationPower / this._totalPower);
				}
				this.UpdateMedianPosition();
			}

			// Token: 0x06003AFC RID: 15100 RVA: 0x000EE604 File Offset: 0x000EC804
			public void AddToCluster(Formation formation)
			{
				this._enemyFormations.Add(formation);
				float totalPower = this._totalPower;
				this._totalPower += formation.QuerySystem.FormationPower;
				this.AggregatePosition = this.AggregatePosition * (totalPower / this._totalPower) + formation.CachedAveragePosition * (formation.QuerySystem.FormationPower / this._totalPower);
				this.UpdateMedianPosition();
			}

			// Token: 0x06003AFD RID: 15101 RVA: 0x000EE680 File Offset: 0x000EC880
			public void RemoveFromCluster(Formation formation)
			{
				this._enemyFormations.Remove(formation);
				float totalPower = this._totalPower;
				this._totalPower -= formation.QuerySystem.FormationPower;
				this.AggregatePosition -= formation.CachedAveragePosition * (formation.QuerySystem.FormationPower / totalPower);
				this.AggregatePosition *= totalPower / this._totalPower;
				this.UpdateMedianPosition();
			}

			// Token: 0x06003AFE RID: 15102 RVA: 0x000EE704 File Offset: 0x000EC904
			private void UpdateMedianPosition()
			{
				float num = float.MaxValue;
				foreach (Formation formation in this._enemyFormations)
				{
					float num2 = formation.CachedMedianPosition.AsVec2.DistanceSquared(this.AggregatePosition);
					if (num2 < num)
					{
						num = num2;
						this.MedianAggregatePosition = formation.CachedMedianPosition;
					}
				}
			}

			// Token: 0x04001C07 RID: 7175
			private readonly MBList<Formation> _enemyFormations = new MBList<Formation>();

			// Token: 0x04001C08 RID: 7176
			private float _totalPower;
		}
	}
}
