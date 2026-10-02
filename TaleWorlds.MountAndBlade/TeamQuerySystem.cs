using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Handlers;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000182 RID: 386
	public class TeamQuerySystem
	{
		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06001485 RID: 5253 RVA: 0x0004B599 File Offset: 0x00049799
		public int MemberCount
		{
			get
			{
				return this._memberCount.Value;
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06001486 RID: 5254 RVA: 0x0004B5A6 File Offset: 0x000497A6
		public WorldPosition MedianPosition
		{
			get
			{
				return this._medianPosition.Value;
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06001487 RID: 5255 RVA: 0x0004B5B3 File Offset: 0x000497B3
		public Vec2 AveragePosition
		{
			get
			{
				return this._averagePosition.Value;
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06001488 RID: 5256 RVA: 0x0004B5C0 File Offset: 0x000497C0
		public Vec2 AverageEnemyPosition
		{
			get
			{
				return this._averageEnemyPosition.Value;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06001489 RID: 5257 RVA: 0x0004B5CD File Offset: 0x000497CD
		public FormationQuerySystem MedianTargetFormation
		{
			get
			{
				return this._medianTargetFormation.Value;
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x0600148A RID: 5258 RVA: 0x0004B5DA File Offset: 0x000497DA
		public WorldPosition MedianTargetFormationPosition
		{
			get
			{
				return this._medianTargetFormationPosition.Value;
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x0600148B RID: 5259 RVA: 0x0004B5E7 File Offset: 0x000497E7
		public WorldPosition LeftFlankEdgePosition
		{
			get
			{
				return this._leftFlankEdgePosition.Value;
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x0600148C RID: 5260 RVA: 0x0004B5F4 File Offset: 0x000497F4
		public WorldPosition RightFlankEdgePosition
		{
			get
			{
				return this._rightFlankEdgePosition.Value;
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x0600148D RID: 5261 RVA: 0x0004B601 File Offset: 0x00049801
		public float InfantryRatio
		{
			get
			{
				return this._infantryRatio.Value;
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x0600148E RID: 5262 RVA: 0x0004B60E File Offset: 0x0004980E
		public float RangedRatio
		{
			get
			{
				return this._rangedRatio.Value;
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x0600148F RID: 5263 RVA: 0x0004B61B File Offset: 0x0004981B
		public float CavalryRatio
		{
			get
			{
				return this._cavalryRatio.Value;
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06001490 RID: 5264 RVA: 0x0004B628 File Offset: 0x00049828
		public float RangedCavalryRatio
		{
			get
			{
				return this._rangedCavalryRatio.Value;
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06001491 RID: 5265 RVA: 0x0004B635 File Offset: 0x00049835
		public int AllyUnitCount
		{
			get
			{
				return this._allyMemberCount.Value;
			}
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06001492 RID: 5266 RVA: 0x0004B642 File Offset: 0x00049842
		public int EnemyUnitCount
		{
			get
			{
				return this._enemyMemberCount.Value;
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06001493 RID: 5267 RVA: 0x0004B64F File Offset: 0x0004984F
		public float AllyInfantryRatio
		{
			get
			{
				return this._allyInfantryRatio.Value;
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06001494 RID: 5268 RVA: 0x0004B65C File Offset: 0x0004985C
		public float AllyRangedRatio
		{
			get
			{
				return this._allyRangedRatio.Value;
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06001495 RID: 5269 RVA: 0x0004B669 File Offset: 0x00049869
		public float AllyCavalryRatio
		{
			get
			{
				return this._allyCavalryRatio.Value;
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06001496 RID: 5270 RVA: 0x0004B676 File Offset: 0x00049876
		public float AllyRangedCavalryRatio
		{
			get
			{
				return this._allyRangedCavalryRatio.Value;
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06001497 RID: 5271 RVA: 0x0004B683 File Offset: 0x00049883
		public float EnemyInfantryRatio
		{
			get
			{
				return this._enemyInfantryRatio.Value;
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06001498 RID: 5272 RVA: 0x0004B690 File Offset: 0x00049890
		public float EnemyRangedRatio
		{
			get
			{
				return this._enemyRangedRatio.Value;
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06001499 RID: 5273 RVA: 0x0004B69D File Offset: 0x0004989D
		public float EnemyCavalryRatio
		{
			get
			{
				return this._enemyCavalryRatio.Value;
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x0600149A RID: 5274 RVA: 0x0004B6AA File Offset: 0x000498AA
		public float EnemyRangedCavalryRatio
		{
			get
			{
				return this._enemyRangedCavalryRatio.Value;
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x0600149B RID: 5275 RVA: 0x0004B6B7 File Offset: 0x000498B7
		public float RemainingPowerRatio
		{
			get
			{
				return this._remainingPowerRatio.Value;
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x0600149C RID: 5276 RVA: 0x0004B6C4 File Offset: 0x000498C4
		public float TeamPower
		{
			get
			{
				return this._teamPower.Value;
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x0600149D RID: 5277 RVA: 0x0004B6D1 File Offset: 0x000498D1
		public float TotalPowerRatio
		{
			get
			{
				return this._totalPowerRatio.Value;
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x0600149E RID: 5278 RVA: 0x0004B6DE File Offset: 0x000498DE
		public float InsideWallsRatio
		{
			get
			{
				return this._insideWallsRatio.Value;
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x0600149F RID: 5279 RVA: 0x0004B6EB File Offset: 0x000498EB
		public IBattlePowerCalculationLogic BattlePowerLogic
		{
			get
			{
				if (this._battlePowerLogic == null)
				{
					this._battlePowerLogic = this._mission.GetMissionBehavior<IBattlePowerCalculationLogic>();
				}
				return this._battlePowerLogic;
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x060014A0 RID: 5280 RVA: 0x0004B70C File Offset: 0x0004990C
		public CasualtyHandler CasualtyHandler
		{
			get
			{
				if (this._casualtyHandler == null)
				{
					this._casualtyHandler = this._mission.GetMissionBehavior<CasualtyHandler>();
				}
				return this._casualtyHandler;
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060014A1 RID: 5281 RVA: 0x0004B72D File Offset: 0x0004992D
		public float MaxUnderRangedAttackRatio
		{
			get
			{
				return this._maxUnderRangedAttackRatio.Value;
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x060014A2 RID: 5282 RVA: 0x0004B73A File Offset: 0x0004993A
		// (set) Token: 0x060014A3 RID: 5283 RVA: 0x0004B742 File Offset: 0x00049942
		public int DeathCount { get; private set; }

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x060014A4 RID: 5284 RVA: 0x0004B74B File Offset: 0x0004994B
		// (set) Token: 0x060014A5 RID: 5285 RVA: 0x0004B753 File Offset: 0x00049953
		public int DeathByRangedCount { get; private set; }

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x060014A6 RID: 5286 RVA: 0x0004B75C File Offset: 0x0004995C
		public int AllyRangedUnitCount
		{
			get
			{
				return (int)(this.AllyRangedRatio * (float)this.AllyUnitCount);
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x060014A7 RID: 5287 RVA: 0x0004B76D File Offset: 0x0004996D
		public int AllCavalryUnitCount
		{
			get
			{
				return (int)(this.AllyCavalryRatio * (float)this.AllyUnitCount);
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x060014A8 RID: 5288 RVA: 0x0004B77E File Offset: 0x0004997E
		public int EnemyRangedUnitCount
		{
			get
			{
				return (int)(this.EnemyRangedRatio * (float)this.EnemyUnitCount);
			}
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x0004B790 File Offset: 0x00049990
		public void Expire()
		{
			this._memberCount.Expire();
			this._medianPosition.Expire();
			this._averagePosition.Expire();
			this._averageEnemyPosition.Expire();
			this._medianTargetFormationPosition.Expire();
			this._leftFlankEdgePosition.Expire();
			this._rightFlankEdgePosition.Expire();
			this._infantryRatio.Expire();
			this._rangedRatio.Expire();
			this._cavalryRatio.Expire();
			this._rangedCavalryRatio.Expire();
			this._allyMemberCount.Expire();
			this._enemyMemberCount.Expire();
			this._allyInfantryRatio.Expire();
			this._allyRangedRatio.Expire();
			this._allyCavalryRatio.Expire();
			this._allyRangedCavalryRatio.Expire();
			this._enemyInfantryRatio.Expire();
			this._enemyRangedRatio.Expire();
			this._enemyCavalryRatio.Expire();
			this._enemyRangedCavalryRatio.Expire();
			this._remainingPowerRatio.Expire();
			this._teamPower.Expire();
			this._totalPowerRatio.Expire();
			this._insideWallsRatio.Expire();
			this._maxUnderRangedAttackRatio.Expire();
			foreach (Formation formation in this.Team.FormationsIncludingSpecialAndEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					formation.QuerySystem.Expire();
				}
			}
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x0004B914 File Offset: 0x00049B14
		public void ExpireAfterUnitAddRemove()
		{
			this._memberCount.Expire();
			this._medianPosition.Expire();
			this._averagePosition.Expire();
			this._leftFlankEdgePosition.Expire();
			this._rightFlankEdgePosition.Expire();
			this._infantryRatio.Expire();
			this._rangedRatio.Expire();
			this._cavalryRatio.Expire();
			this._rangedCavalryRatio.Expire();
			this._allyMemberCount.Expire();
			this._allyInfantryRatio.Expire();
			this._allyRangedRatio.Expire();
			this._allyCavalryRatio.Expire();
			this._allyRangedCavalryRatio.Expire();
			this._remainingPowerRatio.Expire();
			this._teamPower.Expire();
			this._totalPowerRatio.Expire();
			this._insideWallsRatio.Expire();
			this._maxUnderRangedAttackRatio.Expire();
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x0004B9F2 File Offset: 0x00049BF2
		private void InitializeTelemetryScopeNames()
		{
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x0004B9F4 File Offset: 0x00049BF4
		public TeamQuerySystem(Team team)
		{
			TeamQuerySystem <>4__this = this;
			this.Team = team;
			this._mission = Mission.Current;
			this._memberCount = new QueryData<int>(delegate
			{
				int num = 0;
				foreach (Formation formation in <>4__this.Team.FormationsIncludingSpecialAndEmpty)
				{
					num += formation.CountOfUnits;
				}
				return num;
			}, 2f);
			this._allyMemberCount = new QueryData<int>(delegate
			{
				int num2 = 0;
				foreach (Team team2 in <>4__this._mission.Teams)
				{
					if (team2.IsFriendOf(<>4__this.Team))
					{
						num2 += team2.QuerySystem.MemberCount;
					}
				}
				return num2;
			}, 2f);
			this._enemyMemberCount = new QueryData<int>(delegate
			{
				int num3 = 0;
				foreach (Team team3 in <>4__this._mission.Teams)
				{
					if (team3.IsEnemyOf(<>4__this.Team))
					{
						num3 += team3.QuerySystem.MemberCount;
					}
				}
				return num3;
			}, 2f);
			this._averagePosition = new QueryData<Vec2>(new Func<Vec2>(team.GetAveragePosition), 5f);
			this._medianPosition = new QueryData<WorldPosition>(() => team.GetMedianPosition(<>4__this.AveragePosition), 5f);
			this._averageEnemyPosition = new QueryData<Vec2>(delegate
			{
				Vec2 averagePositionOfEnemies = team.GetAveragePositionOfEnemies();
				if (averagePositionOfEnemies.IsValid)
				{
					return averagePositionOfEnemies;
				}
				if (team.Side == BattleSideEnum.Attacker)
				{
					SiegeDeploymentHandler missionBehavior = <>4__this._mission.GetMissionBehavior<SiegeDeploymentHandler>();
					if (missionBehavior != null)
					{
						return missionBehavior.GetEstimatedAverageDefenderPosition();
					}
				}
				if (!<>4__this.AveragePosition.IsValid)
				{
					return team.GetAveragePosition();
				}
				return <>4__this.AveragePosition;
			}, 5f);
			this._medianTargetFormation = new QueryData<FormationQuerySystem>(delegate
			{
				float num4 = float.MaxValue;
				Formation formation2 = null;
				foreach (Team team4 in <>4__this._mission.Teams)
				{
					if (team4.IsEnemyOf(<>4__this.Team))
					{
						foreach (Formation formation3 in team4.FormationsIncludingSpecialAndEmpty)
						{
							if (formation3.CountOfUnits > 0)
							{
								float num5 = formation3.CachedMedianPosition.AsVec2.DistanceSquared(<>4__this.AverageEnemyPosition);
								if (num4 > num5)
								{
									num4 = num5;
									formation2 = formation3;
								}
							}
						}
					}
				}
				if (formation2 != null)
				{
					return formation2.QuerySystem;
				}
				return null;
			}, 1f);
			this._medianTargetFormationPosition = new QueryData<WorldPosition>(delegate
			{
				if (<>4__this.MedianTargetFormation != null)
				{
					return <>4__this.MedianTargetFormation.Formation.CachedMedianPosition;
				}
				return <>4__this.MedianPosition;
			}, 1f);
			QueryData<WorldPosition>.SetupSyncGroup(new IQueryData[] { this._averageEnemyPosition, this._medianTargetFormationPosition });
			this._leftFlankEdgePosition = new QueryData<WorldPosition>(delegate
			{
				Vec2 vec = (<>4__this.MedianTargetFormationPosition.AsVec2 - <>4__this.AveragePosition).RightVec();
				vec.Normalize();
				WorldPosition medianTargetFormationPosition = <>4__this.MedianTargetFormationPosition;
				medianTargetFormationPosition.SetVec2(medianTargetFormationPosition.AsVec2 - vec * 50f);
				return medianTargetFormationPosition;
			}, 5f);
			this._rightFlankEdgePosition = new QueryData<WorldPosition>(delegate
			{
				Vec2 vec2 = (<>4__this.MedianTargetFormationPosition.AsVec2 - <>4__this.AveragePosition).RightVec();
				vec2.Normalize();
				WorldPosition medianTargetFormationPosition2 = <>4__this.MedianTargetFormationPosition;
				medianTargetFormationPosition2.SetVec2(medianTargetFormationPosition2.AsVec2 + vec2 * 50f);
				return medianTargetFormationPosition2;
			}, 5f);
			this._infantryRatio = new QueryData<float>(delegate
			{
				if (<>4__this.MemberCount != 0)
				{
					return (<>4__this.Team.FormationsIncludingSpecialAndEmpty.Sum<Formation>(delegate(Formation f)
					{
						if (f.CountOfUnits <= 0)
						{
							return 0f;
						}
						return f.QuerySystem.InfantryUnitRatio * (float)f.CountOfUnits;
					}) + (float)team.Heroes.Count<Agent>((Agent h) => QueryLibrary.IsInfantry(h))) / (float)<>4__this.MemberCount;
				}
				return 0f;
			}, 15f);
			this._rangedRatio = new QueryData<float>(delegate
			{
				if (<>4__this.MemberCount != 0)
				{
					return (<>4__this.Team.FormationsIncludingSpecialAndEmpty.Sum<Formation>(delegate(Formation f)
					{
						if (f.CountOfUnits <= 0)
						{
							return 0f;
						}
						return f.QuerySystem.RangedUnitRatio * (float)f.CountOfUnits;
					}) + (float)team.Heroes.Count<Agent>((Agent h) => QueryLibrary.IsRanged(h))) / (float)<>4__this.MemberCount;
				}
				return 0f;
			}, 15f);
			this._cavalryRatio = new QueryData<float>(delegate
			{
				if (<>4__this.MemberCount != 0)
				{
					return (<>4__this.Team.FormationsIncludingSpecialAndEmpty.Sum<Formation>(delegate(Formation f)
					{
						if (f.CountOfUnits <= 0)
						{
							return 0f;
						}
						return f.QuerySystem.CavalryUnitRatio * (float)f.CountOfUnits;
					}) + (float)team.Heroes.Count<Agent>((Agent h) => QueryLibrary.IsCavalry(h))) / (float)<>4__this.MemberCount;
				}
				return 0f;
			}, 15f);
			this._rangedCavalryRatio = new QueryData<float>(delegate
			{
				if (<>4__this.MemberCount != 0)
				{
					return (<>4__this.Team.FormationsIncludingSpecialAndEmpty.Sum<Formation>(delegate(Formation f)
					{
						if (f.CountOfUnits <= 0)
						{
							return 0f;
						}
						return f.QuerySystem.RangedCavalryUnitRatio * (float)f.CountOfUnits;
					}) + (float)team.Heroes.Count<Agent>((Agent h) => QueryLibrary.IsRangedCavalry(h))) / (float)<>4__this.MemberCount;
				}
				return 0f;
			}, 15f);
			QueryData<float>.SetupSyncGroup(new IQueryData[] { this._infantryRatio, this._rangedRatio, this._cavalryRatio, this._rangedCavalryRatio });
			this._allyInfantryRatio = new QueryData<float>(delegate
			{
				float num6 = 0f;
				int num7 = 0;
				foreach (Team team5 in <>4__this._mission.Teams)
				{
					if (team5.IsFriendOf(<>4__this.Team))
					{
						int memberCount = team5.QuerySystem.MemberCount;
						num6 += team5.QuerySystem.InfantryRatio * (float)memberCount;
						num7 += memberCount;
					}
				}
				if (num7 != 0)
				{
					return num6 / (float)num7;
				}
				return 0f;
			}, 15f);
			this._allyRangedRatio = new QueryData<float>(delegate
			{
				float num8 = 0f;
				int num9 = 0;
				foreach (Team team6 in <>4__this._mission.Teams)
				{
					if (team6.IsFriendOf(<>4__this.Team))
					{
						int memberCount2 = team6.QuerySystem.MemberCount;
						num8 += team6.QuerySystem.RangedRatio * (float)memberCount2;
						num9 += memberCount2;
					}
				}
				if (num9 != 0)
				{
					return num8 / (float)num9;
				}
				return 0f;
			}, 15f);
			this._allyCavalryRatio = new QueryData<float>(delegate
			{
				float num10 = 0f;
				int num11 = 0;
				foreach (Team team7 in <>4__this._mission.Teams)
				{
					if (team7.IsFriendOf(<>4__this.Team))
					{
						int memberCount3 = team7.QuerySystem.MemberCount;
						num10 += team7.QuerySystem.CavalryRatio * (float)memberCount3;
						num11 += memberCount3;
					}
				}
				if (num11 != 0)
				{
					return num10 / (float)num11;
				}
				return 0f;
			}, 15f);
			this._allyRangedCavalryRatio = new QueryData<float>(delegate
			{
				float num12 = 0f;
				int num13 = 0;
				foreach (Team team8 in <>4__this._mission.Teams)
				{
					if (team8.IsFriendOf(<>4__this.Team))
					{
						int memberCount4 = team8.QuerySystem.MemberCount;
						num12 += team8.QuerySystem.RangedCavalryRatio * (float)memberCount4;
						num13 += memberCount4;
					}
				}
				if (num13 != 0)
				{
					return num12 / (float)num13;
				}
				return 0f;
			}, 15f);
			QueryData<float>.SetupSyncGroup(new IQueryData[] { this._allyInfantryRatio, this._allyRangedRatio, this._allyCavalryRatio, this._allyRangedCavalryRatio });
			this._enemyInfantryRatio = new QueryData<float>(delegate
			{
				float num14 = 0f;
				int num15 = 0;
				foreach (Team team9 in <>4__this._mission.Teams)
				{
					if (team9.IsEnemyOf(<>4__this.Team))
					{
						int memberCount5 = team9.QuerySystem.MemberCount;
						num14 += team9.QuerySystem.InfantryRatio * (float)memberCount5;
						num15 += memberCount5;
					}
				}
				if (num15 != 0)
				{
					return num14 / (float)num15;
				}
				return 0f;
			}, 15f);
			this._enemyRangedRatio = new QueryData<float>(delegate
			{
				float num16 = 0f;
				int num17 = 0;
				foreach (Team team10 in <>4__this._mission.Teams)
				{
					if (team10.IsEnemyOf(<>4__this.Team))
					{
						int memberCount6 = team10.QuerySystem.MemberCount;
						num16 += team10.QuerySystem.RangedRatio * (float)memberCount6;
						num17 += memberCount6;
					}
				}
				if (num17 != 0)
				{
					return num16 / (float)num17;
				}
				return 0f;
			}, 15f);
			this._enemyCavalryRatio = new QueryData<float>(delegate
			{
				float num18 = 0f;
				int num19 = 0;
				foreach (Team team11 in <>4__this._mission.Teams)
				{
					if (team11.IsEnemyOf(<>4__this.Team))
					{
						int memberCount7 = team11.QuerySystem.MemberCount;
						num18 += team11.QuerySystem.CavalryRatio * (float)memberCount7;
						num19 += memberCount7;
					}
				}
				if (num19 != 0)
				{
					return num18 / (float)num19;
				}
				return 0f;
			}, 15f);
			this._enemyRangedCavalryRatio = new QueryData<float>(delegate
			{
				float num20 = 0f;
				int num21 = 0;
				foreach (Team team12 in <>4__this._mission.Teams)
				{
					if (team12.IsEnemyOf(<>4__this.Team))
					{
						int memberCount8 = team12.QuerySystem.MemberCount;
						num20 += team12.QuerySystem.RangedCavalryRatio * (float)memberCount8;
						num21 += memberCount8;
					}
				}
				if (num21 != 0)
				{
					return num20 / (float)num21;
				}
				return 0f;
			}, 15f);
			this._teamPower = new QueryData<float>(() => team.FormationsIncludingSpecialAndEmpty.Sum<Formation>(delegate(Formation f)
			{
				if (f.CountOfUnits <= 0)
				{
					return 0f;
				}
				return f.GetFormationPower();
			}), 5f);
			this._remainingPowerRatio = new QueryData<float>(delegate
			{
				IBattlePowerCalculationLogic battlePowerLogic = <>4__this.BattlePowerLogic;
				CasualtyHandler casualtyHandler = <>4__this.CasualtyHandler;
				float num22 = 0f;
				float num23 = 0f;
				foreach (Team team13 in <>4__this.Team.Mission.Teams)
				{
					if (team13.IsEnemyOf(<>4__this.Team))
					{
						num23 += battlePowerLogic.GetTotalTeamPower(team13);
						using (List<Formation>.Enumerator enumerator15 = team13.FormationsIncludingSpecialAndEmpty.GetEnumerator())
						{
							while (enumerator15.MoveNext())
							{
								Formation formation4 = enumerator15.Current;
								num23 -= casualtyHandler.GetCasualtyPowerLossOfFormation(formation4);
							}
							continue;
						}
					}
					num22 += battlePowerLogic.GetTotalTeamPower(team13);
					foreach (Formation formation5 in team13.FormationsIncludingSpecialAndEmpty)
					{
						num22 -= casualtyHandler.GetCasualtyPowerLossOfFormation(formation5);
					}
				}
				num22 = MathF.Max(0f, num22);
				num23 = MathF.Max(0f, num23);
				return (num22 + 1f) / (num23 + 1f);
			}, 5f);
			this._totalPowerRatio = new QueryData<float>(delegate
			{
				IBattlePowerCalculationLogic battlePowerLogic2 = <>4__this.BattlePowerLogic;
				float num24 = 0f;
				float num25 = 0f;
				foreach (Team team14 in <>4__this.Team.Mission.Teams)
				{
					if (team14.IsEnemyOf(<>4__this.Team))
					{
						num25 += battlePowerLogic2.GetTotalTeamPower(team14);
					}
					else
					{
						num24 += battlePowerLogic2.GetTotalTeamPower(team14);
					}
				}
				return (num24 + 1f) / (num25 + 1f);
			}, 10f);
			this._insideWallsRatio = new QueryData<float>(delegate
			{
				if (!(team.TeamAI is TeamAISiegeComponent))
				{
					return 1f;
				}
				if (<>4__this.AllyUnitCount == 0)
				{
					return 0f;
				}
				int num26 = 0;
				foreach (Team team15 in Mission.Current.Teams)
				{
					if (team15.IsFriendOf(team))
					{
						foreach (Formation formation6 in team15.FormationsIncludingSpecialAndEmpty)
						{
							if (formation6.CountOfUnits > 0)
							{
								num26 += formation6.CountUnitsOnNavMeshIDMod10(1, false);
							}
						}
					}
				}
				return (float)num26 / (float)<>4__this.AllyUnitCount;
			}, 10f);
			this._maxUnderRangedAttackRatio = new QueryData<float>(delegate
			{
				float num27;
				if (<>4__this.AllyUnitCount == 0)
				{
					num27 = 0f;
				}
				else
				{
					float currentTime = Mission.Current.CurrentTime;
					int num28 = 0;
					Func<Agent, bool> <>9__35;
					foreach (Team team16 in <>4__this._mission.Teams)
					{
						if (team16.IsFriendOf(<>4__this.Team))
						{
							for (int i = 0; i < Math.Min(team16.FormationsIncludingSpecialAndEmpty.Count, 8); i++)
							{
								Formation formation7 = team16.FormationsIncludingSpecialAndEmpty[i];
								if (formation7.CountOfUnits > 0)
								{
									int num29 = num28;
									Formation formation8 = formation7;
									Func<Agent, bool> func;
									if ((func = <>9__35) == null)
									{
										func = (<>9__35 = (Agent agent) => currentTime - agent.LastRecievedRangedHitTime < 10f && !agent.Equipment.HasShield());
									}
									num28 = num29 + formation8.GetCountOfUnitsWithCondition(func);
								}
							}
						}
					}
					num27 = (float)num28 / (float)<>4__this.AllyUnitCount;
				}
				if (num27 <= <>4__this._maxUnderRangedAttackRatio.GetCachedValue())
				{
					return <>4__this._maxUnderRangedAttackRatio.GetCachedValue();
				}
				return num27;
			}, 3f);
			this.DeathCount = 0;
			this.DeathByRangedCount = 0;
			this.InitializeTelemetryScopeNames();
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x0004BDBC File Offset: 0x00049FBC
		public void RegisterDeath()
		{
			int deathCount = this.DeathCount;
			this.DeathCount = deathCount + 1;
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x0004BDDC File Offset: 0x00049FDC
		public void RegisterDeathByRanged()
		{
			int deathByRangedCount = this.DeathByRangedCount;
			this.DeathByRangedCount = deathByRangedCount + 1;
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x0004BDFC File Offset: 0x00049FFC
		public float GetLocalAllyPower(Vec2 target)
		{
			return this.Team.FormationsIncludingSpecialAndEmpty.Sum<Formation>(delegate(Formation f)
			{
				if (f.CountOfUnits <= 0)
				{
					return 0f;
				}
				return f.QuerySystem.FormationPower / f.CachedAveragePosition.Distance(target);
			});
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x0004BE34 File Offset: 0x0004A034
		public float GetLocalEnemyPower(Vec2 target)
		{
			float num = 0f;
			foreach (Team team in Mission.Current.Teams)
			{
				if (this.Team.IsEnemyOf(team))
				{
					foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
					{
						if (formation.CountOfUnits > 0)
						{
							num += formation.QuerySystem.FormationPower / formation.CachedAveragePosition.Distance(target);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x0400055D RID: 1373
		public readonly Team Team;

		// Token: 0x0400055E RID: 1374
		private readonly Mission _mission;

		// Token: 0x0400055F RID: 1375
		private readonly QueryData<int> _memberCount;

		// Token: 0x04000560 RID: 1376
		private readonly QueryData<WorldPosition> _medianPosition;

		// Token: 0x04000561 RID: 1377
		private readonly QueryData<Vec2> _averagePosition;

		// Token: 0x04000562 RID: 1378
		private readonly QueryData<Vec2> _averageEnemyPosition;

		// Token: 0x04000563 RID: 1379
		private readonly QueryData<FormationQuerySystem> _medianTargetFormation;

		// Token: 0x04000564 RID: 1380
		private readonly QueryData<WorldPosition> _medianTargetFormationPosition;

		// Token: 0x04000565 RID: 1381
		private readonly QueryData<WorldPosition> _leftFlankEdgePosition;

		// Token: 0x04000566 RID: 1382
		private readonly QueryData<WorldPosition> _rightFlankEdgePosition;

		// Token: 0x04000567 RID: 1383
		private readonly QueryData<float> _infantryRatio;

		// Token: 0x04000568 RID: 1384
		private readonly QueryData<float> _rangedRatio;

		// Token: 0x04000569 RID: 1385
		private readonly QueryData<float> _cavalryRatio;

		// Token: 0x0400056A RID: 1386
		private readonly QueryData<float> _rangedCavalryRatio;

		// Token: 0x0400056B RID: 1387
		private readonly QueryData<int> _allyMemberCount;

		// Token: 0x0400056C RID: 1388
		private readonly QueryData<int> _enemyMemberCount;

		// Token: 0x0400056D RID: 1389
		private readonly QueryData<float> _allyInfantryRatio;

		// Token: 0x0400056E RID: 1390
		private readonly QueryData<float> _allyRangedRatio;

		// Token: 0x0400056F RID: 1391
		private readonly QueryData<float> _allyCavalryRatio;

		// Token: 0x04000570 RID: 1392
		private readonly QueryData<float> _allyRangedCavalryRatio;

		// Token: 0x04000571 RID: 1393
		private readonly QueryData<float> _enemyInfantryRatio;

		// Token: 0x04000572 RID: 1394
		private readonly QueryData<float> _enemyRangedRatio;

		// Token: 0x04000573 RID: 1395
		private readonly QueryData<float> _enemyCavalryRatio;

		// Token: 0x04000574 RID: 1396
		private readonly QueryData<float> _enemyRangedCavalryRatio;

		// Token: 0x04000575 RID: 1397
		private readonly QueryData<float> _remainingPowerRatio;

		// Token: 0x04000576 RID: 1398
		private readonly QueryData<float> _teamPower;

		// Token: 0x04000577 RID: 1399
		private readonly QueryData<float> _totalPowerRatio;

		// Token: 0x04000578 RID: 1400
		private readonly QueryData<float> _insideWallsRatio;

		// Token: 0x04000579 RID: 1401
		private IBattlePowerCalculationLogic _battlePowerLogic;

		// Token: 0x0400057A RID: 1402
		private CasualtyHandler _casualtyHandler;

		// Token: 0x0400057B RID: 1403
		private readonly QueryData<float> _maxUnderRangedAttackRatio;
	}
}
