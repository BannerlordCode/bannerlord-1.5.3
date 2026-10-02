using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002A6 RID: 678
	public static class MissionReinforcementsHelper
	{
		// Token: 0x060025A1 RID: 9633 RVA: 0x00089120 File Offset: 0x00087320
		public static void OnMissionStart()
		{
			Mission mission = Mission.Current;
			MissionReinforcementsHelper._reinforcementFormationsData = new MissionReinforcementsHelper.ReinforcementFormationData[mission.Teams.Count, 8];
			foreach (Team team in mission.Teams)
			{
				for (int i = 0; i < 8; i++)
				{
					MissionReinforcementsHelper._reinforcementFormationsData[team.TeamIndex, i] = new MissionReinforcementsHelper.ReinforcementFormationData();
				}
			}
			MissionReinforcementsHelper._localInitTime = 0U;
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x000891B0 File Offset: 0x000873B0
		[return: TupleElementNames(new string[] { "origin", "formationIndex" })]
		public unsafe static List<ValueTuple<IAgentOriginBase, int>> GetReinforcementAssignments(BattleSideEnum battleSide, List<IAgentOriginBase> troopOrigins)
		{
			Mission mission = Mission.Current;
			MissionReinforcementsHelper._localInitTime += 1U;
			List<ValueTuple<IAgentOriginBase, int>> list = new List<ValueTuple<IAgentOriginBase, int>>();
			PriorityQueue<MissionReinforcementsHelper.ReinforcementFormationPriority, Formation> priorityQueue = new PriorityQueue<MissionReinforcementsHelper.ReinforcementFormationPriority, Formation>(new MissionReinforcementsHelper.ReinforcementFormationPreferenceComparer());
			foreach (IAgentOriginBase agentOriginBase in troopOrigins)
			{
				priorityQueue.Clear();
				FormationClass agentTroopClass = Mission.Current.GetAgentTroopClass(battleSide, agentOriginBase.Troop);
				bool flag = Mission.Current.PlayerTeam.Side == battleSide;
				Team agentTeam = Mission.GetAgentTeam(agentOriginBase, flag);
				foreach (Formation formation in agentTeam.FormationsIncludingEmpty)
				{
					int formationIndex = (int)formation.FormationIndex;
					if (formation.GetReadonlyMovementOrderReference()->OrderEnum != MovementOrder.MovementOrderEnum.Retreat)
					{
						MissionReinforcementsHelper.ReinforcementFormationData reinforcementFormationData = MissionReinforcementsHelper._reinforcementFormationsData[agentTeam.TeamIndex, formationIndex];
						if (!reinforcementFormationData.IsInitialized(MissionReinforcementsHelper._localInitTime))
						{
							reinforcementFormationData.Initialize(formation, MissionReinforcementsHelper._localInitTime);
						}
						MissionReinforcementsHelper.ReinforcementFormationPriority priority = reinforcementFormationData.GetPriority(agentTroopClass);
						if (priorityQueue.IsEmpty<KeyValuePair<MissionReinforcementsHelper.ReinforcementFormationPriority, Formation>>() || priority >= priorityQueue.Peek().Key)
						{
							priorityQueue.Enqueue(priority, formation);
						}
					}
				}
				Formation formation2 = MissionReinforcementsHelper.FindBestFormationAmong(priorityQueue);
				if (formation2 == null)
				{
					formation2 = agentTeam.GetFormation(agentTroopClass);
				}
				int formationIndex2 = (int)formation2.FormationIndex;
				MissionReinforcementsHelper._reinforcementFormationsData[formation2.Team.TeamIndex, formationIndex2].AddProspectiveTroop(agentTroopClass);
				ValueTuple<IAgentOriginBase, int> valueTuple = new ValueTuple<IAgentOriginBase, int>(agentOriginBase, formationIndex2);
				list.Add(valueTuple);
			}
			return list;
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x00089384 File Offset: 0x00087584
		public static void OnMissionEnd()
		{
			MissionReinforcementsHelper._reinforcementFormationsData = null;
		}

		// Token: 0x060025A4 RID: 9636 RVA: 0x0008938C File Offset: 0x0008758C
		private static Formation FindBestFormationAmong(PriorityQueue<MissionReinforcementsHelper.ReinforcementFormationPriority, Formation> matchingFormations)
		{
			Formation formation = null;
			float num = float.MinValue;
			if (!matchingFormations.IsEmpty<KeyValuePair<MissionReinforcementsHelper.ReinforcementFormationPriority, Formation>>())
			{
				int key = (int)matchingFormations.Peek().Key;
				foreach (KeyValuePair<MissionReinforcementsHelper.ReinforcementFormationPriority, Formation> keyValuePair in matchingFormations)
				{
					int key2 = (int)keyValuePair.Key;
					if (key2 < key)
					{
						break;
					}
					Formation value = keyValuePair.Value;
					if (key2 == 3 || key2 == 4)
					{
						if (formation == null || value.FormationIndex < formation.FormationIndex)
						{
							formation = value;
						}
					}
					else
					{
						float formationReinforcementScore = MissionReinforcementsHelper.GetFormationReinforcementScore(value);
						if (formationReinforcementScore > num)
						{
							num = formationReinforcementScore;
							formation = value;
						}
					}
				}
			}
			return formation;
		}

		// Token: 0x060025A5 RID: 9637 RVA: 0x00089444 File Offset: 0x00087644
		private static float GetFormationReinforcementScore(Formation formation)
		{
			Mission mission = Mission.Current;
			float num = (float)formation.CountOfUnits / (float)Math.Max(1, formation.Team.ActiveAgents.Count);
			float num2 = MathF.Max(0f, 1f - num);
			float num3 = 0f;
			Team team = formation.Team;
			DefaultMissionDeploymentPlan defaultMissionDeploymentPlan;
			if (mission.GetDeploymentPlan<DefaultMissionDeploymentPlan>(out defaultMissionDeploymentPlan) && formation.HasBeenPositioned && defaultMissionDeploymentPlan.IsReinforcementPlanMade(team))
			{
				Vec2 asVec = defaultMissionDeploymentPlan.GetMeanPosition(team, false).AsVec2;
				float num4 = formation.CurrentPosition.DistanceSquared(asVec);
				float num5 = MathF.Min(1f, num4 / 62500f);
				num3 = MathF.Max(0f, 1f - num5);
			}
			return 0.6f * num2 + 0.4f * num3;
		}

		// Token: 0x04000E8C RID: 3724
		private const float DominantClassThreshold = 0.5f;

		// Token: 0x04000E8D RID: 3725
		private const float CommonClassThreshold = 0.25f;

		// Token: 0x04000E8E RID: 3726
		private static uint _localInitTime;

		// Token: 0x04000E8F RID: 3727
		private static MissionReinforcementsHelper.ReinforcementFormationData[,] _reinforcementFormationsData;

		// Token: 0x0200057E RID: 1406
		public enum ReinforcementFormationPriority
		{
			// Token: 0x04001EC2 RID: 7874
			Dominant = 6,
			// Token: 0x04001EC3 RID: 7875
			Common = 5,
			// Token: 0x04001EC4 RID: 7876
			EmptyRepresentativeMatch = 4,
			// Token: 0x04001EC5 RID: 7877
			EmptyNoMatch = 3,
			// Token: 0x04001EC6 RID: 7878
			AlternativeDominant = 2,
			// Token: 0x04001EC7 RID: 7879
			AlternativeCommon = 1,
			// Token: 0x04001EC8 RID: 7880
			Default = 0
		}

		// Token: 0x0200057F RID: 1407
		public class ReinforcementFormationPreferenceComparer : IComparer<MissionReinforcementsHelper.ReinforcementFormationPriority>
		{
			// Token: 0x06003E15 RID: 15893 RVA: 0x000F7248 File Offset: 0x000F5448
			public int Compare(MissionReinforcementsHelper.ReinforcementFormationPriority left, MissionReinforcementsHelper.ReinforcementFormationPriority right)
			{
				if (right < left)
				{
					return 1;
				}
				if (right > left)
				{
					return -1;
				}
				return 0;
			}
		}

		// Token: 0x02000580 RID: 1408
		public class ReinforcementFormationData
		{
			// Token: 0x06003E17 RID: 15895 RVA: 0x000F726E File Offset: 0x000F546E
			public ReinforcementFormationData()
			{
				this._initTime = 0U;
				this._expectedTroopCountPerClass = new int[4];
				this._expectedTotalTroopCount = 0;
				this._isClassified = false;
				this._representativeClass = FormationClass.NumberOfAllFormations;
				this._troopClasses = new bool[4];
			}

			// Token: 0x06003E18 RID: 15896 RVA: 0x000F72AC File Offset: 0x000F54AC
			public void Initialize(Formation formation, uint initTime)
			{
				int countOfUnits = formation.CountOfUnits;
				this._expectedTroopCountPerClass[0] = (int)(formation.QuerySystem.InfantryUnitRatio * (float)countOfUnits);
				this._expectedTroopCountPerClass[1] = (int)(formation.QuerySystem.RangedUnitRatio * (float)countOfUnits);
				this._expectedTroopCountPerClass[2] = (int)(formation.QuerySystem.CavalryUnitRatio * (float)countOfUnits);
				this._expectedTroopCountPerClass[3] = (int)(formation.QuerySystem.RangedCavalryUnitRatio * (float)countOfUnits);
				this._expectedTotalTroopCount = countOfUnits;
				this._isClassified = false;
				this._representativeClass = formation.RepresentativeClass;
				this._initTime = initTime;
			}

			// Token: 0x06003E19 RID: 15897 RVA: 0x000F7340 File Offset: 0x000F5540
			public void AddProspectiveTroop(FormationClass troopClass)
			{
				this._expectedTroopCountPerClass[(int)troopClass]++;
				this._expectedTotalTroopCount++;
				this._isClassified = false;
			}

			// Token: 0x06003E1A RID: 15898 RVA: 0x000F7375 File Offset: 0x000F5575
			public bool IsInitialized(uint initTime)
			{
				return initTime == this._initTime;
			}

			// Token: 0x06003E1B RID: 15899 RVA: 0x000F7380 File Offset: 0x000F5580
			public MissionReinforcementsHelper.ReinforcementFormationPriority GetPriority(FormationClass troopClass)
			{
				if (this._expectedTotalTroopCount == 0)
				{
					if (this._representativeClass == troopClass)
					{
						return MissionReinforcementsHelper.ReinforcementFormationPriority.EmptyRepresentativeMatch;
					}
					return MissionReinforcementsHelper.ReinforcementFormationPriority.EmptyNoMatch;
				}
				else
				{
					if (!this._isClassified)
					{
						this.Classify();
					}
					bool flag;
					if (this.HasTroopClass(troopClass, out flag))
					{
						if (!flag)
						{
							return MissionReinforcementsHelper.ReinforcementFormationPriority.Common;
						}
						return MissionReinforcementsHelper.ReinforcementFormationPriority.Dominant;
					}
					else
					{
						FormationClass formationClass = troopClass.AlternativeClass();
						if (!this.HasTroopClass(formationClass, out flag))
						{
							return MissionReinforcementsHelper.ReinforcementFormationPriority.Default;
						}
						if (!flag)
						{
							return MissionReinforcementsHelper.ReinforcementFormationPriority.AlternativeCommon;
						}
						return MissionReinforcementsHelper.ReinforcementFormationPriority.AlternativeDominant;
					}
				}
			}

			// Token: 0x06003E1C RID: 15900 RVA: 0x000F73E0 File Offset: 0x000F55E0
			private void Classify()
			{
				if (this._expectedTotalTroopCount > 0)
				{
					int num = -1;
					int num2 = 4;
					for (int i = 0; i < num2; i++)
					{
						float num3 = (float)this._expectedTroopCountPerClass[i] / (float)this._expectedTotalTroopCount;
						this._troopClasses[i] = num3 >= 0.25f;
						if (num3 > 0.5f)
						{
							num = i;
							break;
						}
					}
					if (num >= 0)
					{
						this.ResetClassAssignments();
						this._troopClasses[num] = true;
					}
				}
				else
				{
					this.ResetClassAssignments();
				}
				this._isClassified = true;
			}

			// Token: 0x06003E1D RID: 15901 RVA: 0x000F745C File Offset: 0x000F565C
			private bool HasTroopClass(FormationClass troopClass, out bool isDominant)
			{
				int num = 0;
				for (int i = 0; i < 4; i++)
				{
					if (i == (int)troopClass && this._troopClasses[i])
					{
						num++;
					}
				}
				isDominant = num == 1;
				return num >= 1;
			}

			// Token: 0x06003E1E RID: 15902 RVA: 0x000F7498 File Offset: 0x000F5698
			private void ResetClassAssignments()
			{
				int num = 4;
				for (int i = 0; i < num; i++)
				{
					this._troopClasses[i] = false;
				}
			}

			// Token: 0x04001EC9 RID: 7881
			private uint _initTime;

			// Token: 0x04001ECA RID: 7882
			private bool _isClassified;

			// Token: 0x04001ECB RID: 7883
			private int[] _expectedTroopCountPerClass;

			// Token: 0x04001ECC RID: 7884
			private int _expectedTotalTroopCount;

			// Token: 0x04001ECD RID: 7885
			private bool[] _troopClasses;

			// Token: 0x04001ECE RID: 7886
			private FormationClass _representativeClass;
		}
	}
}
