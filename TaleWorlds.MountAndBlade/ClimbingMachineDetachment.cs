using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects.Usables;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000185 RID: 389
	public class ClimbingMachineDetachment : IDetachment
	{
		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x060014C2 RID: 5314 RVA: 0x0004C66C File Offset: 0x0004A86C
		public MBReadOnlyList<Formation> UserFormations
		{
			get
			{
				return this._userFormations;
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x060014C3 RID: 5315 RVA: 0x0004C674 File Offset: 0x0004A874
		public bool IsLoose
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x060014C4 RID: 5316 RVA: 0x0004C677 File Offset: 0x0004A877
		// (set) Token: 0x060014C5 RID: 5317 RVA: 0x0004C67F File Offset: 0x0004A87F
		public bool IsActive { get; private set; }

		// Token: 0x060014C6 RID: 5318 RVA: 0x0004C688 File Offset: 0x0004A888
		public ClimbingMachineDetachment(in MBList<ClimbingMachine> climbingMachines)
		{
			this._agents = new List<Agent>();
			this._userFormations = new MBList<Formation>();
			this._climbingMachines = climbingMachines;
			this._climberAgents = new MBList<Agent>();
			this.IsActive = true;
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x0004C6C0 File Offset: 0x0004A8C0
		public void Deactivate()
		{
			this.IsActive = false;
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x0004C6C9 File Offset: 0x0004A8C9
		public void AddAgent(Agent agent, int slotIndex, Agent.AIScriptedFrameFlags customFlags = Agent.AIScriptedFrameFlags.None)
		{
			this._agents.Add(agent);
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x0004C6D7 File Offset: 0x0004A8D7
		public void AddAgentAtSlotIndex(Agent agent, int slotIndex)
		{
			this.AddAgent(agent, slotIndex, Agent.AIScriptedFrameFlags.None);
			Formation formation = agent.Formation;
			if (formation != null)
			{
				formation.DetachUnit(agent, true);
			}
			agent.Detachment = this;
			agent.SetDetachmentWeight(1f);
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x0004C707 File Offset: 0x0004A907
		void IDetachment.FormationStartUsing(Formation formation)
		{
			this._userFormations.Add(formation);
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x0004C715 File Offset: 0x0004A915
		void IDetachment.FormationStopUsing(Formation formation)
		{
			this._userFormations.Remove(formation);
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x0004C724 File Offset: 0x0004A924
		public bool IsUsedByFormation(Formation formation)
		{
			return this._userFormations.Contains(formation);
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x0004C732 File Offset: 0x0004A932
		Agent IDetachment.GetMovingAgentAtSlotIndex(int slotIndex)
		{
			if (slotIndex >= this._agents.Count)
			{
				return null;
			}
			return this._agents[slotIndex];
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0004C750 File Offset: 0x0004A950
		void IDetachment.GetSlotIndexWeightTuples(List<ValueTuple<int, float>> slotIndexWeightTuples)
		{
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0004C752 File Offset: 0x0004A952
		bool IDetachment.IsSlotAtIndexAvailableForAgent(int slotIndex, Agent agent)
		{
			return false;
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0004C755 File Offset: 0x0004A955
		bool IDetachment.IsAgentEligible(Agent agent)
		{
			return agent.Detachment == this;
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0004C760 File Offset: 0x0004A960
		void IDetachment.UnmarkDetachment()
		{
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0004C762 File Offset: 0x0004A962
		bool IDetachment.IsDetachmentRecentlyEvaluated()
		{
			return true;
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0004C765 File Offset: 0x0004A965
		void IDetachment.MarkSlotAtIndex(int slotIndex)
		{
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x0004C767 File Offset: 0x0004A967
		bool IDetachment.IsAgentUsingOrInterested(Agent agent)
		{
			return this._agents.Contains(agent);
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x0004C778 File Offset: 0x0004A978
		void IDetachment.OnFormationLeave(Formation formation)
		{
			for (int i = this._agents.Count - 1; i >= 0; i--)
			{
				Agent agent = this._agents[i];
				if (agent.Formation == formation && !agent.IsPlayerControlled)
				{
					((IDetachment)this).RemoveAgent(agent);
					formation.AttachUnit(agent);
				}
			}
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x0004C7C9 File Offset: 0x0004A9C9
		public bool IsStandingPointAvailableForAgent(Agent agent)
		{
			return false;
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x0004C7CC File Offset: 0x0004A9CC
		public List<float> GetTemplateCostsOfAgent(Agent candidate, List<float> oldValue)
		{
			return oldValue;
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x0004C7CF File Offset: 0x0004A9CF
		float IDetachment.GetExactCostOfAgentAtSlot(Agent candidate, int slotIndex)
		{
			return float.MaxValue;
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x0004C7D6 File Offset: 0x0004A9D6
		public float GetTemplateWeightOfAgent(Agent candidate)
		{
			return float.MaxValue;
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x0004C7E0 File Offset: 0x0004A9E0
		public float? GetWeightOfAgentAtNextSlot(List<Agent> newAgents, out Agent match)
		{
			match = null;
			return null;
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x0004C7FC File Offset: 0x0004A9FC
		public float? GetWeightOfAgentAtNextSlot(List<ValueTuple<Agent, float>> agentTemplateScores, out Agent match)
		{
			match = null;
			return null;
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x0004C815 File Offset: 0x0004AA15
		public float? GetWeightOfAgentAtOccupiedSlot(Agent detachedAgent, List<Agent> newAgents, out Agent match)
		{
			match = null;
			return new float?(float.MaxValue);
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x0004C824 File Offset: 0x0004AA24
		public void RemoveAgent(Agent agent)
		{
			this._agents.Remove(agent);
			agent.DisableScriptedMovement();
			agent.DisableScriptedCombatMovement();
			this._climberAgents.Remove(agent);
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x0004C84C File Offset: 0x0004AA4C
		public int GetNumberOfUsableSlots()
		{
			return int.MaxValue;
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x0004C854 File Offset: 0x0004AA54
		public WorldFrame? GetAgentFrame(Agent agent)
		{
			return null;
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x0004C86C File Offset: 0x0004AA6C
		public float? GetWeightOfNextSlot(BattleSideEnum side)
		{
			return null;
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x0004C882 File Offset: 0x0004AA82
		public float GetWeightOfOccupiedSlot(Agent agent)
		{
			return float.MinValue;
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x0004C889 File Offset: 0x0004AA89
		float IDetachment.GetDetachmentWeight(BattleSideEnum side)
		{
			return float.MinValue;
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x0004C890 File Offset: 0x0004AA90
		void IDetachment.ResetEvaluation()
		{
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x0004C892 File Offset: 0x0004AA92
		bool IDetachment.IsEvaluated()
		{
			return true;
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x0004C895 File Offset: 0x0004AA95
		void IDetachment.SetAsEvaluated()
		{
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x0004C897 File Offset: 0x0004AA97
		float IDetachment.GetDetachmentWeightFromCache()
		{
			return float.MinValue;
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x0004C89E File Offset: 0x0004AA9E
		float IDetachment.ComputeAndCacheDetachmentWeight(BattleSideEnum side)
		{
			return float.MinValue;
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x0004C8A8 File Offset: 0x0004AAA8
		public void TickClimbingMachines()
		{
			if (!this.IsActive)
			{
				return;
			}
			if (this._userFormations.Count > 0)
			{
				for (int i = this._userFormations[0].UnitsWithoutLooseDetachedOnes.Count - 1; i >= 0; i--)
				{
					Agent agent;
					if ((agent = this._userFormations[0].UnitsWithoutLooseDetachedOnes[i] as Agent) != null && !agent.IsPlayerControlled && agent.IsDetachableFromFormation && agent.IsInWater() && agent.Formation != null && !this._climberAgents.Contains(agent))
					{
						if (agent.AIMoveToGameObjectIsEnabled() || agent.CurrentlyUsedGameObject != null)
						{
							agent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.None);
						}
						this._climberAgents.Add(agent);
						this.AddAgentAtSlotIndex(agent, 0);
						if (agent.GetPrimaryWieldedItemIndex() != EquipmentIndex.None)
						{
							agent.TryToSheathWeaponInHand(Agent.HandIndex.MainHand, Agent.WeaponWieldActionType.Instant);
						}
						if (agent.GetOffhandWieldedItemIndex() != EquipmentIndex.None)
						{
							agent.TryToSheathWeaponInHand(Agent.HandIndex.OffHand, Agent.WeaponWieldActionType.Instant);
						}
						if (agent.GetAgentFlags().HasAnyFlag(AgentFlag.CanAttack | AgentFlag.CanDefend))
						{
							agent.SetAgentFlags(agent.GetAgentFlags() & ~(AgentFlag.CanAttack | AgentFlag.CanDefend));
						}
					}
				}
				for (int j = this._userFormations[0].DetachedUnits.Count - 1; j >= 0; j--)
				{
					Agent agent2 = this._userFormations[0].DetachedUnits[j];
					if (!agent2.IsPlayerControlled && agent2.IsDetachableFromFormation && agent2.Detachment != this && agent2.IsInWater())
					{
						agent2.Detachment.RemoveAgent(agent2);
						if (agent2.Formation != null && !this._climberAgents.Contains(agent2))
						{
							agent2.Formation.AttachUnit(agent2);
							this._climberAgents.Add(agent2);
							this.AddAgentAtSlotIndex(agent2, 0);
							if (agent2.GetPrimaryWieldedItemIndex() != EquipmentIndex.None)
							{
								agent2.TryToSheathWeaponInHand(Agent.HandIndex.MainHand, Agent.WeaponWieldActionType.Instant);
							}
							if (agent2.GetOffhandWieldedItemIndex() != EquipmentIndex.None)
							{
								agent2.TryToSheathWeaponInHand(Agent.HandIndex.OffHand, Agent.WeaponWieldActionType.Instant);
							}
							if (agent2.GetAgentFlags().HasAnyFlag(AgentFlag.CanAttack | AgentFlag.CanDefend))
							{
								agent2.SetAgentFlags(agent2.GetAgentFlags() & ~(AgentFlag.CanAttack | AgentFlag.CanDefend));
							}
						}
					}
				}
				for (int k = this._climberAgents.Count - 1; k >= 0; k--)
				{
					Agent agent3 = this._climberAgents[k];
					if (agent3.IsActive())
					{
						if (agent3.IsInWater())
						{
							float num = float.MaxValue;
							ClimbingMachine climbingMachine = null;
							foreach (ClimbingMachine climbingMachine2 in this._climbingMachines)
							{
								float num2 = climbingMachine2.GameEntity.GlobalPosition.DistanceSquared(agent3.Position) * (Vec2.DotProduct((agent3.Position.AsVec2 - climbingMachine2.GameEntity.GlobalPosition.AsVec2).Normalized(), climbingMachine2.PilotStandingPoint.GameEntity.GetGlobalFrame().rotation.f.AsVec2.Normalized()) + 2f);
								if (num2 < num)
								{
									num = num2;
									climbingMachine = climbingMachine2;
								}
							}
							if (climbingMachine != null)
							{
								StandingPoint standingPoint = null;
								foreach (StandingPoint standingPoint2 in climbingMachine.StandingPoints)
								{
									if (!standingPoint2.HasUser && !standingPoint2.IsDeactivated)
									{
										standingPoint = standingPoint2;
										break;
									}
								}
								if (standingPoint != null && agent3.CanReachAndUseObject(standingPoint, standingPoint.GetUserFrameForAgent(agent3).Origin.AsVec2.DistanceSquared(agent3.Position.AsVec2)) && MathF.Abs(standingPoint.GameEntity.GlobalPosition.z - agent3.Position.z) < 1.5f)
								{
									agent3.DisableScriptedMovement();
									agent3.UseGameObject(standingPoint, -1);
								}
								else
								{
									WorldPosition worldPosition = new WorldPosition(climbingMachine.GameEntity.Scene, climbingMachine.GameEntity.GlobalPosition);
									agent3.SetScriptedPosition(ref worldPosition, false, Agent.AIScriptedFrameFlags.None);
								}
							}
						}
						else if (agent3.CurrentlyUsedGameObject == null)
						{
							this._climberAgents.RemoveAt(k);
							this.RemoveAgent(agent3);
							Formation formation = agent3.Formation;
							if (formation != null)
							{
								formation.AttachUnit(agent3);
							}
						}
					}
					else
					{
						this._climberAgents.RemoveAt(k);
						this.RemoveAgent(agent3);
						Formation formation2 = agent3.Formation;
						if (formation2 != null)
						{
							formation2.AttachUnit(agent3);
						}
					}
				}
			}
		}

		// Token: 0x04000586 RID: 1414
		private const int Capacity = 2147483647;

		// Token: 0x04000587 RID: 1415
		private readonly List<Agent> _agents;

		// Token: 0x04000588 RID: 1416
		private readonly MBList<Formation> _userFormations;

		// Token: 0x04000589 RID: 1417
		private readonly MBReadOnlyList<ClimbingMachine> _climbingMachines;

		// Token: 0x0400058A RID: 1418
		private readonly MBList<Agent> _climberAgents;
	}
}
