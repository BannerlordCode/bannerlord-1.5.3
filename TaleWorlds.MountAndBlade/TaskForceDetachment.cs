using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200017C RID: 380
	public class TaskForceDetachment : IDetachment
	{
		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x060013BC RID: 5052 RVA: 0x0004900C File Offset: 0x0004720C
		public MBReadOnlyList<Formation> UserFormations
		{
			get
			{
				return this._userFormations;
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x060013BD RID: 5053 RVA: 0x00049014 File Offset: 0x00047214
		public bool IsLoose
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x060013BE RID: 5054 RVA: 0x00049017 File Offset: 0x00047217
		// (set) Token: 0x060013BF RID: 5055 RVA: 0x0004901F File Offset: 0x0004721F
		public int CountOfAgents { get; private set; }

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x060013C0 RID: 5056 RVA: 0x00049028 File Offset: 0x00047228
		public Agent TargetAgent { get; }

		// Token: 0x060013C1 RID: 5057 RVA: 0x00049030 File Offset: 0x00047230
		public TaskForceDetachment(Agent attackedAgent, Agent targetAgent)
		{
			this._attackedAgent = attackedAgent;
			this.TargetAgent = targetAgent;
			this._userFormations = new MBList<Formation>();
			this._agents = new MBList<Agent>();
			this._tempAgentList = new MBList<Agent>();
			Mission.Current.GetNearbyAllyAgents(attackedAgent.Position.AsVec2, 5f, attackedAgent.Team, this._tempAgentList);
			int num = 0;
			foreach (Agent agent in this._tempAgentList)
			{
				if (agent.IsDetachableFromFormation && !agent.IsDetachedFromFormation && agent.Formation == attackedAgent.Formation)
				{
					if (num > 4)
					{
						break;
					}
					this.AddAgentAtSlotIndex(agent, num);
					num++;
				}
			}
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x00049110 File Offset: 0x00047310
		public void AddAgent(Agent agent, int slotIndex, Agent.AIScriptedFrameFlags customFlags = Agent.AIScriptedFrameFlags.None)
		{
			this._agents[slotIndex] = agent;
			int countOfAgents = this.CountOfAgents;
			this.CountOfAgents = countOfAgents + 1;
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x0004913C File Offset: 0x0004733C
		public void AddAgentAtSlotIndex(Agent agent, int slotIndex)
		{
			if (this._agents.Count <= slotIndex)
			{
				this._agents.Add(agent);
			}
			else
			{
				this._agents[slotIndex] = agent;
			}
			int countOfAgents = this.CountOfAgents;
			this.CountOfAgents = countOfAgents + 1;
			Formation formation = agent.Formation;
			if (formation != null)
			{
				formation.DetachUnit(agent, true);
			}
			agent.Detachment = this;
			agent.SetDetachmentWeight(1f);
			agent.SetDetachmentIndex(slotIndex);
			agent.SetFormationFrameDisabled();
			agent.SetAutomaticTargetSelection(false);
			agent.SetTargetAgent(this.TargetAgent);
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x000491C8 File Offset: 0x000473C8
		public void AddReinforcementAgent(Agent agent)
		{
			bool flag = false;
			int i;
			for (i = 0; i < this._agents.Count; i++)
			{
				if (this._agents[i] == null)
				{
					this.AddAgentAtSlotIndex(agent, i);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.AddAgentAtSlotIndex(agent, i);
			}
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x00049212 File Offset: 0x00047412
		void IDetachment.FormationStartUsing(Formation formation)
		{
			this._userFormations.Add(formation);
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x00049220 File Offset: 0x00047420
		void IDetachment.FormationStopUsing(Formation formation)
		{
			this._userFormations.Remove(formation);
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x0004922F File Offset: 0x0004742F
		public bool IsUsedByFormation(Formation formation)
		{
			return this._userFormations.Contains(formation);
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x0004923D File Offset: 0x0004743D
		Agent IDetachment.GetMovingAgentAtSlotIndex(int slotIndex)
		{
			if (slotIndex >= this._agents.Count)
			{
				return null;
			}
			return this._agents[slotIndex];
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x0004925B File Offset: 0x0004745B
		void IDetachment.GetSlotIndexWeightTuples(List<ValueTuple<int, float>> slotIndexWeightTuples)
		{
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x0004925D File Offset: 0x0004745D
		bool IDetachment.IsSlotAtIndexAvailableForAgent(int slotIndex, Agent agent)
		{
			return false;
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x00049260 File Offset: 0x00047460
		bool IDetachment.IsAgentEligible(Agent agent)
		{
			return agent.Detachment == this;
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x0004926B File Offset: 0x0004746B
		void IDetachment.UnmarkDetachment()
		{
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x0004926D File Offset: 0x0004746D
		bool IDetachment.IsDetachmentRecentlyEvaluated()
		{
			return true;
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x00049270 File Offset: 0x00047470
		void IDetachment.MarkSlotAtIndex(int slotIndex)
		{
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x00049272 File Offset: 0x00047472
		bool IDetachment.IsAgentUsingOrInterested(Agent agent)
		{
			return agent.DetachmentIndex >= 0 && agent.DetachmentIndex < this._agents.Count && this._agents[agent.DetachmentIndex] == agent;
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x000492A8 File Offset: 0x000474A8
		void IDetachment.OnFormationLeave(Formation formation)
		{
			for (int i = this._agents.Count - 1; i >= 0; i--)
			{
				Agent agent = this._agents[i];
				if (agent != null && agent.Formation == formation && !agent.IsPlayerControlled)
				{
					this._agents[i] = null;
					int countOfAgents = this.CountOfAgents;
					this.CountOfAgents = countOfAgents - 1;
					agent.DisableScriptedMovement();
					agent.DisableScriptedCombatMovement();
					formation.AttachUnit(agent);
				}
			}
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x0004931E File Offset: 0x0004751E
		public bool IsStandingPointAvailableForAgent(Agent agent)
		{
			return false;
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x00049321 File Offset: 0x00047521
		public List<float> GetTemplateCostsOfAgent(Agent candidate, List<float> oldValue)
		{
			return oldValue;
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x00049324 File Offset: 0x00047524
		float IDetachment.GetExactCostOfAgentAtSlot(Agent candidate, int slotIndex)
		{
			return float.MaxValue;
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x0004932B File Offset: 0x0004752B
		public float GetTemplateWeightOfAgent(Agent candidate)
		{
			return float.MaxValue;
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x00049334 File Offset: 0x00047534
		public float? GetWeightOfAgentAtNextSlot(List<Agent> newAgents, out Agent match)
		{
			match = null;
			return null;
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x00049350 File Offset: 0x00047550
		public float? GetWeightOfAgentAtNextSlot(List<ValueTuple<Agent, float>> agentTemplateScores, out Agent match)
		{
			match = null;
			return null;
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x00049369 File Offset: 0x00047569
		public float? GetWeightOfAgentAtOccupiedSlot(Agent detachedAgent, List<Agent> newAgents, out Agent match)
		{
			match = null;
			return new float?(float.MaxValue);
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x00049378 File Offset: 0x00047578
		public void RemoveAgent(Agent agent)
		{
			if (agent == this._attackedAgent)
			{
				foreach (Agent agent2 in this._agents)
				{
					if (agent2 != this._attackedAgent)
					{
						this._attackedAgent = agent2;
						break;
					}
				}
			}
			this._agents[agent.DetachmentIndex] = null;
			int countOfAgents = this.CountOfAgents;
			this.CountOfAgents = countOfAgents - 1;
			agent.DisableScriptedMovement();
			agent.DisableScriptedCombatMovement();
			agent.SetAutomaticTargetSelection(true);
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x00049414 File Offset: 0x00047614
		public int GetNumberOfUsableSlots()
		{
			return 0;
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x00049418 File Offset: 0x00047618
		public bool CalculateShouldBeDisbanded()
		{
			if (!this.TargetAgent.IsActive())
			{
				return true;
			}
			int num = 0;
			foreach (Agent agent in this._agents)
			{
				if (agent != null && agent.IsActive())
				{
					num++;
				}
			}
			if (num > 0)
			{
				Agent attackedAgent = this._attackedAgent;
				if (((attackedAgent != null) ? attackedAgent.Team : null) != null)
				{
					Agent targetAgent = this.TargetAgent;
					bool flag;
					if (targetAgent == null)
					{
						flag = null != null;
					}
					else
					{
						Formation formation = targetAgent.Formation;
						flag = ((formation != null) ? formation.Team : null) != null;
					}
					if (flag && this._tempAgentList != null)
					{
						float num2 = this.TargetAgent.Position.DistanceSquared(this._attackedAgent.Position);
						if (this.TargetAgent.Formation != null && (this.TargetAgent.Formation.CountOfUnits > this.CountOfAgents || (float)this.TargetAgent.Formation.CountOfUnits > (float)this._userFormations[0].CountOfUnits * 0.5f) && num2 > this.TargetAgent.Position.AsVec2.DistanceSquared(this.TargetAgent.Formation.CachedAveragePosition) * 0.36f)
						{
							return true;
						}
						if (num2 > this.TargetAgent.Position.AsVec2.DistanceSquared(this.TargetAgent.Team.QuerySystem.AveragePosition) * 0.36f)
						{
							return true;
						}
						Mission.Current.GetNearbyEnemyAgents(this.TargetAgent.Position.AsVec2, 20f, this._attackedAgent.Team, this._tempAgentList);
						return this._tempAgentList.Count > num;
					}
				}
			}
			return true;
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x000495F0 File Offset: 0x000477F0
		public WorldFrame? GetAgentFrame(Agent agent)
		{
			return null;
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x00049608 File Offset: 0x00047808
		public float? GetWeightOfNextSlot(BattleSideEnum side)
		{
			return null;
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x0004961E File Offset: 0x0004781E
		public float GetWeightOfOccupiedSlot(Agent agent)
		{
			return float.MinValue;
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x00049625 File Offset: 0x00047825
		float IDetachment.GetDetachmentWeight(BattleSideEnum side)
		{
			return float.MinValue;
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x0004962C File Offset: 0x0004782C
		void IDetachment.ResetEvaluation()
		{
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x0004962E File Offset: 0x0004782E
		bool IDetachment.IsEvaluated()
		{
			return true;
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x00049631 File Offset: 0x00047831
		void IDetachment.SetAsEvaluated()
		{
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x00049633 File Offset: 0x00047833
		float IDetachment.GetDetachmentWeightFromCache()
		{
			return float.MinValue;
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x0004963A File Offset: 0x0004783A
		float IDetachment.ComputeAndCacheDetachmentWeight(BattleSideEnum side)
		{
			return float.MinValue;
		}

		// Token: 0x0400050A RID: 1290
		private readonly MBList<Agent> _agents;

		// Token: 0x0400050B RID: 1291
		private readonly MBList<Agent> _tempAgentList;

		// Token: 0x0400050C RID: 1292
		private readonly MBList<Formation> _userFormations;

		// Token: 0x0400050D RID: 1293
		private Agent _attackedAgent;
	}
}
