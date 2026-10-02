using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Source.Missions.Handlers.Logic
{
	// Token: 0x020003E6 RID: 998
	public class AgentMoraleInteractionLogic : MissionLogic
	{
		// Token: 0x06003783 RID: 14211 RVA: 0x000E69C8 File Offset: 0x000E4BC8
		public AgentMoraleInteractionLogic()
		{
			this._nearbyAgentsCache = new MBList<Agent>();
			this._nearbyAllyAgentsCache = new MBList<Agent>();
		}

		// Token: 0x06003784 RID: 14212 RVA: 0x000E6A28 File Offset: 0x000E4C28
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			if (affectedAgent == null || !affectedAgent.IsHuman || (agentState != AgentState.Killed && agentState != AgentState.Unconscious))
			{
				return;
			}
			ValueTuple<float, float> valueTuple = MissionGameModels.Current.BattleMoraleModel.CalculateMaxMoraleChangeDueToAgentIncapacitated(affectedAgent, agentState, affectorAgent, in killingBlow);
			float item = valueTuple.Item1;
			float item2 = valueTuple.Item2;
			if (item > 0f || item2 > 0f)
			{
				if (affectorAgent != null)
				{
					affectorAgent = (affectorAgent.IsHuman ? affectorAgent : (affectorAgent.IsMount ? affectorAgent.RiderAgent : null));
				}
				this.ApplyMoraleEffectOnAgentIncapacitated(affectedAgent, affectorAgent, item, item2, 4f);
			}
		}

		// Token: 0x06003785 RID: 14213 RVA: 0x000E6AAC File Offset: 0x000E4CAC
		public override void OnAgentFleeing(Agent affectedAgent)
		{
			if (affectedAgent == null || !affectedAgent.IsHuman)
			{
				return;
			}
			ValueTuple<float, float> valueTuple = MissionGameModels.Current.BattleMoraleModel.CalculateMaxMoraleChangeDueToAgentPanicked(affectedAgent);
			float item = valueTuple.Item1;
			float item2 = valueTuple.Item2;
			if (item > 0f || item2 > 0f)
			{
				this.ApplyMoraleEffectOnAgentIncapacitated(affectedAgent, null, item, item2, 4f);
			}
			if (MBRandom.RandomFloat < 0.7f)
			{
				affectedAgent.MakeVoice(SkinVoiceManager.VoiceType.Debacle, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
			}
		}

		// Token: 0x06003786 RID: 14214 RVA: 0x000E6B1C File Offset: 0x000E4D1C
		private void ApplyMoraleEffectOnAgentIncapacitated(Agent affectedAgent, Agent affectorAgent, float affectedSideMaxMoraleLoss, float affectorSideMoraleMaxGain, float effectRadius)
		{
			AgentMoraleInteractionLogic.<>c__DisplayClass15_0 CS$<>8__locals1 = new AgentMoraleInteractionLogic.<>c__DisplayClass15_0();
			CS$<>8__locals1.affectedAgent = affectedAgent;
			CS$<>8__locals1.affectorAgent = affectorAgent;
			this._agentsToReceiveMoraleLoss.Clear();
			this._agentsToReceiveMoraleGain.Clear();
			if (CS$<>8__locals1.affectedAgent != null && CS$<>8__locals1.affectedAgent.IsHuman)
			{
				Vec2 asVec = CS$<>8__locals1.affectedAgent.GetWorldPosition().AsVec2;
				base.Mission.GetNearbyAgents(asVec, effectRadius, this._nearbyAgentsCache);
				this.SelectRandomAgentsFromListToAgentSet(this._nearbyAgentsCache, this._agentsToReceiveMoraleLoss, 10, new Predicate<Agent>(CS$<>8__locals1.<ApplyMoraleEffectOnAgentIncapacitated>g__AffectedsAllyCondition|0));
				if (this._agentsToReceiveMoraleLoss.Count < 10 && CS$<>8__locals1.affectedAgent.Formation != null)
				{
					this.SelectRandomAgentsFromFormationToAgentSet(CS$<>8__locals1.affectedAgent.Formation, this._agentsToReceiveMoraleLoss, 10, new Predicate<IFormationUnit>(AgentMoraleInteractionLogic.<>c.<>9.<ApplyMoraleEffectOnAgentIncapacitated>g__FormationCondition|15_1));
				}
				if (CS$<>8__locals1.affectorAgent != null && CS$<>8__locals1.affectorAgent.IsActive() && CS$<>8__locals1.affectorAgent.IsHuman && CS$<>8__locals1.affectorAgent.IsAIControlled && CS$<>8__locals1.affectorAgent.IsEnemyOf(CS$<>8__locals1.affectedAgent))
				{
					this._agentsToReceiveMoraleGain.Add(CS$<>8__locals1.affectorAgent);
				}
				if (this._agentsToReceiveMoraleGain.Count < 10)
				{
					this.SelectRandomAgentsFromListToAgentSet(this._nearbyAgentsCache, this._agentsToReceiveMoraleGain, 10, new Predicate<Agent>(CS$<>8__locals1.<ApplyMoraleEffectOnAgentIncapacitated>g__AffectedsEnemyCondition|2));
				}
				if (this._agentsToReceiveMoraleGain.Count < 10)
				{
					Agent affectorAgent2 = CS$<>8__locals1.affectorAgent;
					if (((affectorAgent2 != null) ? affectorAgent2.Team : null) != null && CS$<>8__locals1.affectorAgent.IsEnemyOf(CS$<>8__locals1.affectedAgent))
					{
						Vec2 asVec2 = CS$<>8__locals1.affectorAgent.GetWorldPosition().AsVec2;
						if (asVec2.DistanceSquared(asVec) > 2.25f)
						{
							base.Mission.GetNearbyAllyAgents(asVec2, effectRadius, CS$<>8__locals1.affectedAgent.Team, this._nearbyAllyAgentsCache);
							this.SelectRandomAgentsFromListToAgentSet(this._nearbyAllyAgentsCache, this._agentsToReceiveMoraleGain, 10, new Predicate<Agent>(CS$<>8__locals1.<ApplyMoraleEffectOnAgentIncapacitated>g__AffectorsAllyCondition|4));
						}
					}
				}
				if (this._agentsToReceiveMoraleGain.Count < 10)
				{
					Agent affectorAgent3 = CS$<>8__locals1.affectorAgent;
					if (((affectorAgent3 != null) ? affectorAgent3.Formation : null) != null)
					{
						this.SelectRandomAgentsFromFormationToAgentSet(CS$<>8__locals1.affectorAgent.Formation, this._agentsToReceiveMoraleGain, 10, new Predicate<IFormationUnit>(AgentMoraleInteractionLogic.<>c.<>9.<ApplyMoraleEffectOnAgentIncapacitated>g__FormationCondition|15_3));
					}
				}
			}
			foreach (Agent agent in this._agentsToReceiveMoraleLoss)
			{
				float num = -MissionGameModels.Current.BattleMoraleModel.CalculateMoraleChangeToCharacter(agent, affectedSideMaxMoraleLoss);
				agent.ChangeMorale(num);
			}
			foreach (Agent agent2 in this._agentsToReceiveMoraleGain)
			{
				float num2 = MissionGameModels.Current.BattleMoraleModel.CalculateMoraleChangeToCharacter(agent2, affectorSideMoraleMaxGain);
				agent2.ChangeMorale(num2);
			}
		}

		// Token: 0x06003787 RID: 14215 RVA: 0x000E6E24 File Offset: 0x000E5024
		private void SelectRandomAgentsFromListToAgentSet(MBReadOnlyList<Agent> agentsList, HashSet<Agent> outputAgentsSet, int maxCountInSet, Predicate<Agent> conditions = null)
		{
			if (outputAgentsSet != null && agentsList != null)
			{
				this._randomAgentSelector.Initialize(agentsList);
				Agent agent;
				while (outputAgentsSet.Count < maxCountInSet && this._randomAgentSelector.SelectRandom(out agent, conditions))
				{
					outputAgentsSet.Add(agent);
				}
			}
		}

		// Token: 0x06003788 RID: 14216 RVA: 0x000E6E68 File Offset: 0x000E5068
		private void SelectRandomAgentsFromFormationToAgentSet(Formation formation, HashSet<Agent> outputAgentsSet, int maxCountInSet, Predicate<IFormationUnit> conditions = null)
		{
			if (outputAgentsSet != null && formation != null && formation.CountOfUnits > 0)
			{
				int num = Math.Max(0, maxCountInSet - outputAgentsSet.Count);
				if (num > 0)
				{
					int num2 = (int)((float)formation.CountOfDetachedUnits / (float)formation.CountOfUnits * (float)num);
					if (num2 > 0)
					{
						this._randomAgentSelector.Initialize(formation.DetachedUnits);
						int num3 = 0;
						Agent agent;
						while (num3 < num2 && outputAgentsSet.Count < maxCountInSet && this._randomAgentSelector.SelectRandom(out agent, conditions))
						{
							if (outputAgentsSet.Add(agent))
							{
								num3++;
							}
						}
					}
					if (outputAgentsSet.Count < maxCountInSet)
					{
						IFormationArrangement arrangement = formation.Arrangement;
						MBList<IFormationUnit> mblist;
						if ((mblist = ((arrangement != null) ? arrangement.GetAllUnits() : null) as MBList<IFormationUnit>) != null && mblist.Count > 0)
						{
							this._randomFormationUnitSelector.Initialize(mblist);
							IFormationUnit formationUnit;
							while (outputAgentsSet.Count < maxCountInSet && this._randomFormationUnitSelector.SelectRandom(out formationUnit, conditions))
							{
								Agent agent2;
								if ((agent2 = formationUnit as Agent) != null)
								{
									outputAgentsSet.Add(agent2);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x040017FA RID: 6138
		private const float DebacleVoiceChance = 0.7f;

		// Token: 0x040017FB RID: 6139
		private const float MoraleEffectRadius = 4f;

		// Token: 0x040017FC RID: 6140
		private const int MaxNumAgentsToGainMorale = 10;

		// Token: 0x040017FD RID: 6141
		private const int MaxNumAgentsToLoseMorale = 10;

		// Token: 0x040017FE RID: 6142
		private const float SquaredDistanceForSeparateAffectorQuery = 2.25f;

		// Token: 0x040017FF RID: 6143
		private const ushort RandomSelectorCapacity = 1024;

		// Token: 0x04001800 RID: 6144
		private readonly HashSet<Agent> _agentsToReceiveMoraleGain = new HashSet<Agent>();

		// Token: 0x04001801 RID: 6145
		private readonly HashSet<Agent> _agentsToReceiveMoraleLoss = new HashSet<Agent>();

		// Token: 0x04001802 RID: 6146
		private readonly MBFastRandomSelector<Agent> _randomAgentSelector = new MBFastRandomSelector<Agent>(1024);

		// Token: 0x04001803 RID: 6147
		private readonly MBFastRandomSelector<IFormationUnit> _randomFormationUnitSelector = new MBFastRandomSelector<IFormationUnit>(1024);

		// Token: 0x04001804 RID: 6148
		private readonly MBList<Agent> _nearbyAgentsCache;

		// Token: 0x04001805 RID: 6149
		private readonly MBList<Agent> _nearbyAllyAgentsCache;
	}
}
