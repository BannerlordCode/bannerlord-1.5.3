using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000276 RID: 630
	public class AgentVictoryLogic : MissionLogic
	{
		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06002371 RID: 9073 RVA: 0x0007D996 File Offset: 0x0007BB96
		public AgentVictoryLogic.CheerActionGroupEnum CheerActionGroup
		{
			get
			{
				return this._cheerActionGroup;
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06002372 RID: 9074 RVA: 0x0007D99E File Offset: 0x0007BB9E
		public AgentVictoryLogic.CheerReactionTimeSettings CheerReactionTimerData
		{
			get
			{
				return this._cheerReactionTimerData;
			}
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x0007D9A8 File Offset: 0x0007BBA8
		public override void AfterStart()
		{
			base.Mission.MissionCloseTimeAfterFinish = 60f;
			this._cheeringAgents = new List<AgentVictoryLogic.CheeringAgent>();
			this.SetCheerReactionTimerSettings(1f, 8f);
			if (base.Mission.PlayerTeam != null)
			{
				base.Mission.PlayerTeam.PlayerOrderController.OnOrderIssued += new OnOrderIssuedDelegate(this.MasterOrderControllerOnOrderIssued);
			}
			Mission.Current.IsBattleInRetreatEvent += this.CheckIfIsInRetreat;
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x0007DA24 File Offset: 0x0007BC24
		private void MasterOrderControllerOnOrderIssued(OrderType orderType, IEnumerable<Formation> appliedFormations, OrderController orderController, object[] delegateparams)
		{
			MBList<Formation> mblist = appliedFormations.ToMBList<Formation>();
			for (int i = this._cheeringAgents.Count - 1; i >= 0; i--)
			{
				Agent agent = this._cheeringAgents[i].Agent;
				if (mblist.Contains(agent.Formation))
				{
					this._cheeringAgents[i].OrderReceived();
				}
			}
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x0007DA84 File Offset: 0x0007BC84
		public void SetCheerActionGroup(AgentVictoryLogic.CheerActionGroupEnum cheerActionGroup = AgentVictoryLogic.CheerActionGroupEnum.None)
		{
			this._cheerActionGroup = cheerActionGroup;
			switch (this._cheerActionGroup)
			{
			case AgentVictoryLogic.CheerActionGroupEnum.LowCheerActions:
				this._selectedCheerActions = this._lowCheerActions;
				return;
			case AgentVictoryLogic.CheerActionGroupEnum.MidCheerActions:
				this._selectedCheerActions = this._midCheerActions;
				return;
			case AgentVictoryLogic.CheerActionGroupEnum.HighCheerActions:
				this._selectedCheerActions = this._highCheerActions;
				return;
			default:
				this._selectedCheerActions = null;
				return;
			}
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x0007DAE3 File Offset: 0x0007BCE3
		public void SetCheerReactionTimerSettings(float minDuration = 1f, float maxDuration = 8f)
		{
			this._cheerReactionTimerData = new AgentVictoryLogic.CheerReactionTimeSettings(minDuration, maxDuration);
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x0007DAF2 File Offset: 0x0007BCF2
		public override void OnClearScene()
		{
			this._cheeringAgents.Clear();
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x0007DB00 File Offset: 0x0007BD00
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			VictoryComponent component = affectedAgent.GetComponent<VictoryComponent>();
			if (component != null)
			{
				affectedAgent.RemoveComponent(component);
			}
			for (int i = 0; i < this._cheeringAgents.Count; i++)
			{
				if (this._cheeringAgents[i].Agent == affectedAgent)
				{
					this._cheeringAgents.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x0007DB56 File Offset: 0x0007BD56
		protected override void OnEndMission()
		{
			Mission.Current.IsBattleInRetreatEvent -= this.CheckIfIsInRetreat;
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x0007DB6E File Offset: 0x0007BD6E
		public override void OnMissionTick(float dt)
		{
			if (this._cheeringAgents.Count > 0)
			{
				this.CheckAnimationAndVoice();
			}
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x0007DB84 File Offset: 0x0007BD84
		private void CheckAnimationAndVoice()
		{
			for (int i = this._cheeringAgents.Count - 1; i >= 0; i--)
			{
				Agent agent = this._cheeringAgents[i].Agent;
				bool gotOrderRecently = this._cheeringAgents[i].GotOrderRecently;
				bool isCheeringOnRetreat = this._cheeringAgents[i].IsCheeringOnRetreat;
				bool flag = this._cheeringAgents[i].IsCheeringPaused;
				VictoryComponent component = agent.GetComponent<VictoryComponent>();
				if (component != null)
				{
					HumanAIComponent component2 = agent.GetComponent<HumanAIComponent>();
					bool flag2 = ((component2 != null) ? component2.GetCurrentlyMovingGameObject() : null) != null;
					bool flag3 = agent.GetCurrentAnimationFlag(0).HasAnyFlag(AnimFlags.anf_synch_with_ladder_movement) || agent.GetCurrentAnimationFlag(1).HasAnyFlag(AnimFlags.anf_synch_with_ladder_movement);
					bool flag4 = agent.IsInWater() || agent.IsUsingGameObject || flag2 || flag3;
					if (this.CheckIfIsInRetreat() && gotOrderRecently)
					{
						agent.RemoveComponent(component);
						agent.SetActionChannel(1, in ActionIndexCache.act_none, false, (AnimFlags)((long)Math.Min(agent.GetCurrentActionPriority(1), 73)), 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
						if (MBRandom.RandomFloat > 0.25f)
						{
							agent.MakeVoice(SkinVoiceManager.VoiceType.Yell, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
						}
						if (isCheeringOnRetreat)
						{
							agent.ClearTargetFrame();
						}
						this._cheeringAgents.RemoveAt(i);
					}
					else if (flag != flag4)
					{
						this._cheeringAgents[i].UpdatePauseState(flag4);
						flag = this._cheeringAgents[i].IsCheeringPaused;
					}
					if ((!this.CheckIfIsInRetreat() || !gotOrderRecently) && !flag && component.CheckTimer())
					{
						if (!agent.IsActive())
						{
							Debug.FailedAssert("Agent trying to cheer without being active", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\MissionLogics\\AgentVictoryLogic.cs", "CheckAnimationAndVoice", 250);
							Debug.Print("Agent trying to cheer without being active", 0, Debug.DebugColor.White, 17592186044416UL);
						}
						bool flag5;
						this.ChooseWeaponToCheerWithCheerAndUpdateTimer(agent, out flag5);
						if (flag5)
						{
							component.ChangeTimerDuration(6f, 12f);
						}
					}
				}
			}
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x0007DD7C File Offset: 0x0007BF7C
		private void SelectVictoryCondition(BattleSideEnum side)
		{
			if (this._cheerActionGroup == AgentVictoryLogic.CheerActionGroupEnum.None)
			{
				BattleObserverMissionLogic missionBehavior = Mission.Current.GetMissionBehavior<BattleObserverMissionLogic>();
				if (missionBehavior != null)
				{
					float deathToBuiltAgentRatioForSide = missionBehavior.GetDeathToBuiltAgentRatioForSide(side);
					if (deathToBuiltAgentRatioForSide < 0.25f)
					{
						this.SetCheerActionGroup(AgentVictoryLogic.CheerActionGroupEnum.HighCheerActions);
						return;
					}
					if (deathToBuiltAgentRatioForSide < 0.75f)
					{
						this.SetCheerActionGroup(AgentVictoryLogic.CheerActionGroupEnum.MidCheerActions);
						return;
					}
					this.SetCheerActionGroup(AgentVictoryLogic.CheerActionGroupEnum.LowCheerActions);
					return;
				}
				else
				{
					this.SetCheerActionGroup(AgentVictoryLogic.CheerActionGroupEnum.MidCheerActions);
				}
			}
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x0007DDD8 File Offset: 0x0007BFD8
		public void SetTimersOfVictoryReactionsOnBattleEnd(BattleSideEnum side)
		{
			this._isInRetreat = false;
			this.SelectVictoryCondition(side);
			foreach (Team team in base.Mission.Teams)
			{
				if (team.Side == side)
				{
					foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
					{
						if (formation.CountOfUnits > 0)
						{
							formation.SetMovementOrder(MovementOrder.MovementOrderStop);
						}
					}
				}
			}
			foreach (Agent agent in base.Mission.Agents)
			{
				if (agent.IsHuman && agent.IsAIControlled && agent.Team != null && side == agent.Team.Side && agent.CurrentWatchState == Agent.WatchState.Alarmed && agent.GetComponent<VictoryComponent>() == null)
				{
					this.RegisterAgentForCheerCheck(agent, false, this._cheerReactionTimerData.MinDuration, this._cheerReactionTimerData.MaxDuration);
				}
			}
		}

		// Token: 0x0600237E RID: 9086 RVA: 0x0007DF30 File Offset: 0x0007C130
		private void RegisterAgentForCheerCheck(Agent agent, bool isCheeringOnRetreat, float minReactionTime, float maxReactionTime)
		{
			agent.AddComponent(new VictoryComponent(agent, new RandomTimer(base.Mission.CurrentTime, minReactionTime, maxReactionTime)));
			this._cheeringAgents.Add(new AgentVictoryLogic.CheeringAgent(agent, isCheeringOnRetreat));
		}

		// Token: 0x0600237F RID: 9087 RVA: 0x0007DF64 File Offset: 0x0007C164
		public void SetTimersOfVictoryReactionsOnRetreat(BattleSideEnum side)
		{
			this._isInRetreat = true;
			this.SelectVictoryCondition(side);
			List<Agent> list = base.Mission.Agents.Where<Agent>((Agent agent) => agent.IsHuman && agent.IsAIControlled && agent.Team.Side == side).ToList<Agent>();
			int num = (int)((float)list.Count * 0.5f);
			List<Agent> list2 = new List<Agent>();
			int num2 = 0;
			while (num2 < list.Count && list2.Count != num)
			{
				Agent agent3 = list[num2];
				EquipmentIndex primaryWieldedItemIndex = agent3.GetPrimaryWieldedItemIndex();
				bool flag = primaryWieldedItemIndex != EquipmentIndex.None && agent3.Equipment[primaryWieldedItemIndex].Item.ItemFlags.HasAnyFlag(ItemFlags.DropOnAnyAction);
				EquipmentIndex offhandWieldedItemIndex = agent3.GetOffhandWieldedItemIndex();
				bool flag2 = offhandWieldedItemIndex != EquipmentIndex.None && agent3.Equipment[offhandWieldedItemIndex].Item.ItemFlags.HasAnyFlag(ItemFlags.DropOnAnyAction);
				HumanAIComponent component = agent3.GetComponent<HumanAIComponent>();
				bool flag3 = ((component != null) ? component.GetCurrentlyMovingGameObject() : null) != null;
				bool flag4 = agent3.GetCurrentAnimationFlag(0).HasAnyFlag(AnimFlags.anf_synch_with_ladder_movement) || agent3.GetCurrentAnimationFlag(1).HasAnyFlag(AnimFlags.anf_synch_with_ladder_movement);
				if (!agent3.IsInWater() && !flag && !flag2 && !agent3.IsUsingGameObject && !flag3 && !flag4)
				{
					int num3 = list.Count - num2;
					int num4 = num - list2.Count;
					int num5 = num3 - num4;
					float num6 = MBMath.ClampFloat((float)(num - num5) / (float)num, 0f, 1f);
					float num7;
					Vec3 vec;
					if (num6 < 1f && agent3.TryGetImmediateEnemyAgentMovementData(out num7, out vec))
					{
						float maximumForwardUnlimitedSpeed = agent3.GetMaximumForwardUnlimitedSpeed();
						float num8 = num7;
						if (maximumForwardUnlimitedSpeed > num8)
						{
							float num9 = (agent3.Position - vec).LengthSquared / (maximumForwardUnlimitedSpeed - num8);
							if (num9 < 900f)
							{
								float num10 = num6 - -1f;
								float num11 = num9 / 900f;
								num6 = -1f + num10 * num11;
							}
						}
					}
					if (MBRandom.RandomFloat <= 0.5f + 0.5f * num6)
					{
						list2.Add(agent3);
					}
				}
				num2++;
			}
			foreach (Agent agent2 in list2)
			{
				MatrixFrame frame = agent2.Frame;
				Vec2 asVec = frame.origin.AsVec2;
				Vec3 f = frame.rotation.f;
				agent2.SetTargetPositionAndDirectionSynched(ref asVec, ref f);
				this.SetTimersOfVictoryReactionsForSingleAgent(agent2, this._cheerReactionTimerData.MinDuration, this._cheerReactionTimerData.MaxDuration, true);
			}
		}

		// Token: 0x06002380 RID: 9088 RVA: 0x0007E230 File Offset: 0x0007C430
		public void SetTimersOfVictoryReactionsOnTournamentVictoryForAgent(Agent agent, float minStartTime, float maxStartTime)
		{
			this._selectedCheerActions = this._midCheerActions;
			this.SetTimersOfVictoryReactionsForSingleAgent(agent, minStartTime, maxStartTime, false);
		}

		// Token: 0x06002381 RID: 9089 RVA: 0x0007E248 File Offset: 0x0007C448
		private void SetTimersOfVictoryReactionsForSingleAgent(Agent agent, float minStartTime, float maxStartTime, bool isCheeringOnRetreat)
		{
			if (agent.IsActive() && agent.IsHuman && agent.IsAIControlled)
			{
				this.RegisterAgentForCheerCheck(agent, isCheeringOnRetreat, minStartTime, maxStartTime);
			}
		}

		// Token: 0x06002382 RID: 9090 RVA: 0x0007E270 File Offset: 0x0007C470
		private void ChooseWeaponToCheerWithCheerAndUpdateTimer(Agent cheerAgent, out bool resetTimer)
		{
			resetTimer = false;
			if (cheerAgent.GetCurrentActionType(1) != Agent.ActionCodeType.EquipUnequip)
			{
				EquipmentIndex primaryWieldedItemIndex = cheerAgent.GetPrimaryWieldedItemIndex();
				bool flag = primaryWieldedItemIndex != EquipmentIndex.None && !cheerAgent.Equipment[primaryWieldedItemIndex].Item.ItemFlags.HasAnyFlag(ItemFlags.DropOnAnyAction);
				if (!flag)
				{
					EquipmentIndex equipmentIndex = EquipmentIndex.None;
					for (EquipmentIndex equipmentIndex2 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex2 < EquipmentIndex.ExtraWeaponSlot; equipmentIndex2++)
					{
						if (!cheerAgent.Equipment[equipmentIndex2].IsEmpty && !cheerAgent.Equipment[equipmentIndex2].Item.ItemFlags.HasAnyFlag(ItemFlags.DropOnAnyAction))
						{
							equipmentIndex = equipmentIndex2;
							break;
						}
					}
					if (equipmentIndex == EquipmentIndex.None)
					{
						if (primaryWieldedItemIndex != EquipmentIndex.None)
						{
							cheerAgent.TryToSheathWeaponInHand(Agent.HandIndex.MainHand, Agent.WeaponWieldActionType.WithAnimation);
						}
						else
						{
							flag = true;
						}
					}
					else
					{
						cheerAgent.TryToWieldWeaponInSlot(equipmentIndex, Agent.WeaponWieldActionType.WithAnimation, false);
					}
				}
				if (flag)
				{
					ActionIndexCache[] array = this._selectedCheerActions;
					if (cheerAgent.HasMount)
					{
						array = this._midCheerActions;
					}
					cheerAgent.SetActionChannel(1, in array[MBRandom.RandomInt(array.Length)], false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
					cheerAgent.MakeVoice(SkinVoiceManager.VoiceType.Victory, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					resetTimer = true;
				}
			}
		}

		// Token: 0x06002383 RID: 9091 RVA: 0x0007E3A5 File Offset: 0x0007C5A5
		private bool CheckIfIsInRetreat()
		{
			return this._isInRetreat;
		}

		// Token: 0x04000D95 RID: 3477
		private const float HighCheerThreshold = 0.25f;

		// Token: 0x04000D96 RID: 3478
		private const float MidCheerThreshold = 0.75f;

		// Token: 0x04000D97 RID: 3479
		private const float YellIfOrderedInRetreatProbability = 0.25f;

		// Token: 0x04000D98 RID: 3480
		private AgentVictoryLogic.CheerActionGroupEnum _cheerActionGroup;

		// Token: 0x04000D99 RID: 3481
		private AgentVictoryLogic.CheerReactionTimeSettings _cheerReactionTimerData;

		// Token: 0x04000D9A RID: 3482
		private readonly ActionIndexCache[] _lowCheerActions = new ActionIndexCache[]
		{
			ActionIndexCache.act_cheering_low_01,
			ActionIndexCache.act_cheering_low_02,
			ActionIndexCache.act_cheering_low_03,
			ActionIndexCache.act_cheering_low_04,
			ActionIndexCache.act_cheering_low_05,
			ActionIndexCache.act_cheering_low_06,
			ActionIndexCache.act_cheering_low_07,
			ActionIndexCache.act_cheering_low_08,
			ActionIndexCache.act_cheering_low_09,
			ActionIndexCache.act_cheering_low_10
		};

		// Token: 0x04000D9B RID: 3483
		private readonly ActionIndexCache[] _midCheerActions = new ActionIndexCache[]
		{
			ActionIndexCache.act_cheer_1,
			ActionIndexCache.act_cheer_2,
			ActionIndexCache.act_cheer_3,
			ActionIndexCache.act_cheer_4
		};

		// Token: 0x04000D9C RID: 3484
		private readonly ActionIndexCache[] _highCheerActions = new ActionIndexCache[]
		{
			ActionIndexCache.act_cheering_high_01,
			ActionIndexCache.act_cheering_high_02,
			ActionIndexCache.act_cheering_high_03,
			ActionIndexCache.act_cheering_high_04,
			ActionIndexCache.act_cheering_high_05,
			ActionIndexCache.act_cheering_high_06,
			ActionIndexCache.act_cheering_high_07,
			ActionIndexCache.act_cheering_high_08
		};

		// Token: 0x04000D9D RID: 3485
		private ActionIndexCache[] _selectedCheerActions;

		// Token: 0x04000D9E RID: 3486
		private List<AgentVictoryLogic.CheeringAgent> _cheeringAgents;

		// Token: 0x04000D9F RID: 3487
		private bool _isInRetreat;

		// Token: 0x02000551 RID: 1361
		public enum CheerActionGroupEnum
		{
			// Token: 0x04001E3C RID: 7740
			None,
			// Token: 0x04001E3D RID: 7741
			LowCheerActions,
			// Token: 0x04001E3E RID: 7742
			MidCheerActions,
			// Token: 0x04001E3F RID: 7743
			HighCheerActions
		}

		// Token: 0x02000552 RID: 1362
		public struct CheerReactionTimeSettings
		{
			// Token: 0x06003D6B RID: 15723 RVA: 0x000F5B88 File Offset: 0x000F3D88
			public CheerReactionTimeSettings(float minDuration, float maxDuration)
			{
				this.MinDuration = minDuration;
				this.MaxDuration = maxDuration;
			}

			// Token: 0x04001E40 RID: 7744
			public readonly float MinDuration;

			// Token: 0x04001E41 RID: 7745
			public readonly float MaxDuration;
		}

		// Token: 0x02000553 RID: 1363
		private class CheeringAgent
		{
			// Token: 0x17000A7B RID: 2683
			// (get) Token: 0x06003D6C RID: 15724 RVA: 0x000F5B98 File Offset: 0x000F3D98
			// (set) Token: 0x06003D6D RID: 15725 RVA: 0x000F5BA0 File Offset: 0x000F3DA0
			public bool GotOrderRecently { get; private set; }

			// Token: 0x17000A7C RID: 2684
			// (get) Token: 0x06003D6E RID: 15726 RVA: 0x000F5BA9 File Offset: 0x000F3DA9
			// (set) Token: 0x06003D6F RID: 15727 RVA: 0x000F5BB1 File Offset: 0x000F3DB1
			public bool IsCheeringPaused { get; private set; }

			// Token: 0x06003D70 RID: 15728 RVA: 0x000F5BBA File Offset: 0x000F3DBA
			public CheeringAgent(Agent agent, bool isCheeringOnRetreat)
			{
				this.Agent = agent;
				this.IsCheeringOnRetreat = isCheeringOnRetreat;
			}

			// Token: 0x06003D71 RID: 15729 RVA: 0x000F5BD0 File Offset: 0x000F3DD0
			public void OrderReceived()
			{
				this.GotOrderRecently = true;
			}

			// Token: 0x06003D72 RID: 15730 RVA: 0x000F5BD9 File Offset: 0x000F3DD9
			internal void UpdatePauseState(bool shouldCheeringBePaused)
			{
				this.IsCheeringPaused = shouldCheeringBePaused;
			}

			// Token: 0x04001E42 RID: 7746
			public readonly Agent Agent;

			// Token: 0x04001E43 RID: 7747
			public readonly bool IsCheeringOnRetreat;
		}
	}
}
