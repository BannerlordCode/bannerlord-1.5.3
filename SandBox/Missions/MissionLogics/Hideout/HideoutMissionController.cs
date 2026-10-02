using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Conversation.MissionLogics;
using SandBox.Missions.AgentBehaviors;
using SandBox.Missions.MissionLogics.Hideout.Objectives;
using SandBox.Objects.AnimationPoints;
using SandBox.Objects.AreaMarkers;
using SandBox.Objects.Usables;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Missions.MissionLogics;

namespace SandBox.Missions.MissionLogics.Hideout
{
	// Token: 0x02000096 RID: 150
	public class HideoutMissionController : MissionLogic, IMissionAgentSpawnLogic, IMissionBehavior
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x00029F7F File Offset: 0x0002817F
		// (set) Token: 0x06000632 RID: 1586 RVA: 0x00029F87 File Offset: 0x00028187
		public BattleSideEnum PlayerSide { get; private set; }

		// Token: 0x06000633 RID: 1587 RVA: 0x00029F90 File Offset: 0x00028190
		public HideoutMissionController(IMissionTroopSupplier[] suppliers, BattleSideEnum playerSide, int firstPhaseEnemyTroopCount, int firstPhasePlayerSideTroopCount)
		{
			this.PlayerSide = playerSide;
			this._areaMarkers = new List<CommonAreaMarker>();
			this._patrolAreas = new List<PatrolArea>();
			this._defenderAgentObjects = new Dictionary<Agent, HideoutMissionController.UsedObject>();
			this._firstPhaseEnemyTroopCount = firstPhaseEnemyTroopCount;
			this._firstPhasePlayerSideTroopCount = firstPhasePlayerSideTroopCount;
			this._overriddenHideoutBossCharacterObject = null;
			this._missionSides = new HideoutMissionController.MissionSide[2];
			for (int i = 0; i < 2; i++)
			{
				IMissionTroopSupplier missionTroopSupplier = suppliers[i];
				bool flag = i == (int)playerSide;
				this._missionSides[i] = new HideoutMissionController.MissionSide((BattleSideEnum)i, missionTroopSupplier, flag);
			}
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0002A01D File Offset: 0x0002821D
		public override void OnCreated()
		{
			base.OnCreated();
			base.Mission.DoesMissionRequireCivilianEquipment = false;
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0002A034 File Offset: 0x00028234
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._battleAgentLogic = base.Mission.GetMissionBehavior<BattleAgentLogic>();
			this._battleEndLogic = base.Mission.GetMissionBehavior<BattleEndLogic>();
			this._battleEndLogic.ChangeCanCheckForEndCondition(false);
			this._agentVictoryLogic = base.Mission.GetMissionBehavior<AgentVictoryLogic>();
			this._cinematicController = base.Mission.GetMissionBehavior<HideoutCinematicController>();
			this._missionObjectiveLogic = base.Mission.GetMissionBehavior<MissionObjectiveLogic>();
			base.Mission.IsMainAgentObjectInteractionEnabled = false;
			this._cinematicController = base.Mission.GetMissionBehavior<HideoutCinematicController>();
			foreach (StealthAreaUsePoint stealthAreaUsePoint in base.Mission.MissionObjects.FindAllWithType<StealthAreaUsePoint>())
			{
				stealthAreaUsePoint.DisableStealthAreaUsePoint();
			}
			base.Mission.GetAgentTroopClass_Override += this.GetHideoutMissionTroopClass;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0002A124 File Offset: 0x00028324
		public override void OnObjectStoppedBeingUsed(Agent userAgent, UsableMissionObject usedObject)
		{
			if (usedObject != null && usedObject is AnimationPoint && userAgent.IsActive() && userAgent.IsAIControlled && userAgent.CurrentWatchState == Agent.WatchState.Patrolling)
			{
				PatrolArea firstScriptOfType = usedObject.GameEntity.Parent.GetFirstScriptOfType<PatrolArea>();
				if (firstScriptOfType == null)
				{
					return;
				}
				((IDetachment)firstScriptOfType).AddAgent(userAgent, -1, Agent.AIScriptedFrameFlags.None);
			}
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0002A178 File Offset: 0x00028378
		public override void OnAgentAlarmedStateChanged(Agent agent, Agent.AIStateFlag flag)
		{
			if (this._hideoutMissionState < HideoutMissionController.HideoutMissionState.ConversationBetweenLeaders && agent.Team == base.Mission.DefenderTeam)
			{
				bool flag2 = (flag & Agent.AIStateFlag.Alarmed) == Agent.AIStateFlag.Alarmed;
				if (flag2 || (flag & Agent.AIStateFlag.Alarmed) == Agent.AIStateFlag.Cautious)
				{
					if (agent.IsUsingGameObject)
					{
						agent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
					else
					{
						agent.DisableScriptedMovement();
						if (agent.IsAIControlled && agent.AIMoveToGameObjectIsEnabled())
						{
							agent.AIMoveToGameObjectDisable();
							Formation formation = agent.Formation;
							if (formation != null)
							{
								formation.Team.DetachmentManager.RemoveScoresOfAgentFromDetachments(agent);
							}
						}
					}
					this._defenderAgentObjects[agent].IsMachineAITicked = false;
				}
				else if ((flag & Agent.AIStateFlag.Alarmed) == Agent.AIStateFlag.None)
				{
					this._defenderAgentObjects[agent].IsMachineAITicked = true;
					agent.TryToSheathWeaponInHand(Agent.HandIndex.MainHand, Agent.WeaponWieldActionType.WithAnimation);
					((IDetachment)this._defenderAgentObjects[agent].Machine).AddAgent(agent, -1, Agent.AIScriptedFrameFlags.None);
				}
				if (flag2)
				{
					agent.SetWantsToYell();
				}
			}
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0002A254 File Offset: 0x00028454
		public override void OnMissionTick(float dt)
		{
			if (!this._isMissionInitialized)
			{
				this.InitializeMission();
				this._isMissionInitialized = true;
				return;
			}
			if (!this._troopsInitialized)
			{
				this._troopsInitialized = true;
				foreach (Agent agent in base.Mission.Agents)
				{
					this._battleAgentLogic.OnAgentBuild(agent, null);
				}
			}
			this.UsedObjectTick(dt);
			if (!this._battleResolved)
			{
				this.CheckBattleResolved();
			}
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0002A2EC File Offset: 0x000284EC
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (this._clearObjectiveTargetAgents.Contains(affectedAgent))
			{
				this._clearObjectiveTargetAgents.Remove(affectedAgent);
			}
			if (this._hideoutMissionState == HideoutMissionController.HideoutMissionState.BossFightWithDuel)
			{
				using (List<Agent>.Enumerator enumerator = base.Mission.Agents.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Agent agent = enumerator.Current;
						if (agent != affectedAgent && agent != affectorAgent && agent.IsActive() && agent.GetLookAgent() == affectedAgent)
						{
							agent.SetLookAgent(null);
						}
					}
					return;
				}
			}
			if (this._hideoutMissionState == HideoutMissionController.HideoutMissionState.InitialFightBeforeBossFight && affectedAgent.IsMainAgent)
			{
				base.Mission.PlayerTeam.PlayerOrderController.SelectAllFormations(false);
				affectedAgent.Formation = null;
				base.Mission.PlayerTeam.PlayerOrderController.SetOrder(OrderType.Retreat);
			}
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0002A3C8 File Offset: 0x000285C8
		public override void OnMissionStateFinalized()
		{
			base.Mission.GetAgentTroopClass_Override -= this.GetHideoutMissionTroopClass;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0002A3E1 File Offset: 0x000285E1
		public void SetOverriddenHideoutBossCharacterObject(CharacterObject characterObject)
		{
			this._overriddenHideoutBossCharacterObject = characterObject;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0002A3EC File Offset: 0x000285EC
		private void InitializeMission()
		{
			base.Mission.GetMissionBehavior<MissionConversationLogic>().DisableStartConversation(true);
			base.Mission.SetMissionMode(MissionMode.Stealth, true);
			this._areaMarkers.AddRange(from area in base.Mission.ActiveMissionObjects.FindAllWithType<CommonAreaMarker>()
				orderby area.AreaIndex
				select area);
			this._patrolAreas.AddRange(from area in base.Mission.ActiveMissionObjects.FindAllWithType<PatrolArea>()
				orderby area.AreaIndex
				select area);
			this.DecideMissionState();
			base.Mission.DeploymentPlan.MakeDefaultDeploymentPlans();
			for (int i = 0; i < 2; i++)
			{
				int num;
				if (this._missionSides[i].IsPlayerSide)
				{
					num = this._firstPhasePlayerSideTroopCount;
				}
				else
				{
					if (this._missionSides[i].NumberOfTroopsNotSupplied <= this._firstPhaseEnemyTroopCount)
					{
						Debug.FailedAssert("_missionSides[i].NumberOfTroopsNotSupplied <= _firstPhaseEnemyTroopCount", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Missions\\MissionLogics\\Hideout\\HideoutMissionController.cs", "InitializeMission", 569);
						this._firstPhaseEnemyTroopCount = (int)((float)this._missionSides[i].NumberOfTroopsNotSupplied * 0.7f);
					}
					num = ((this._hideoutMissionState == HideoutMissionController.HideoutMissionState.InitialFightBeforeBossFight) ? this._firstPhaseEnemyTroopCount : this._missionSides[i].NumberOfTroopsNotSupplied);
				}
				this._missionSides[i].SpawnTroops(this._areaMarkers, this._patrolAreas, this._defenderAgentObjects, num);
			}
			base.Mission.OnInitialSpawnCompleted(BattleSideEnum.None);
			foreach (Agent agent in base.Mission.PlayerEnemyTeam.ActiveAgents)
			{
				this._clearObjectiveTargetAgents.Add(agent);
			}
			this._clearTheMainCampObjective = new ClearTheMainCampObjective(base.Mission, this._clearObjectiveTargetAgents);
			this._missionObjectiveLogic.StartObjective(this._clearTheMainCampObjective);
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0002A5E8 File Offset: 0x000287E8
		private void UsedObjectTick(float dt)
		{
			foreach (KeyValuePair<Agent, HideoutMissionController.UsedObject> keyValuePair in this._defenderAgentObjects)
			{
				if (keyValuePair.Value.IsMachineAITicked)
				{
					keyValuePair.Value.MachineAI.Tick(keyValuePair.Key, null, null, dt);
				}
			}
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0002A660 File Offset: 0x00028860
		protected override void OnEndMission()
		{
			int num = 0;
			if (this._hideoutMissionState == HideoutMissionController.HideoutMissionState.BossFightWithDuel)
			{
				if (Agent.Main == null || !Agent.Main.IsActive())
				{
					List<Agent> duelPhaseAllyAgents = this._duelPhaseAllyAgents;
					num = ((duelPhaseAllyAgents != null) ? duelPhaseAllyAgents.Count : 0);
				}
				else if (this._bossAgent == null || !this._bossAgent.IsActive())
				{
					PlayerEncounter.EnemySurrender = true;
				}
			}
			if (MobileParty.MainParty.MemberRoster.TotalHealthyCount <= num && MapEvent.PlayerMapEvent.BattleState == BattleState.None)
			{
				MapEvent.PlayerMapEvent.SetOverrideWinner(BattleSideEnum.Defender);
			}
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x0002A6E4 File Offset: 0x000288E4
		private void CheckBattleResolved()
		{
			if (this._hideoutMissionState != HideoutMissionController.HideoutMissionState.CutSceneBeforeBossFight && this._hideoutMissionState != HideoutMissionController.HideoutMissionState.ConversationBetweenLeaders)
			{
				if (this.IsSideDepleted(BattleSideEnum.Attacker))
				{
					if (this._hideoutMissionState == HideoutMissionController.HideoutMissionState.BossFightWithDuel)
					{
						this.OnDuelOver(BattleSideEnum.Defender);
					}
					this._battleEndLogic.ChangeCanCheckForEndCondition(true);
					this._battleResolved = true;
					this._missionObjectiveLogic.CompleteCurrentObjective();
					return;
				}
				if (this.IsSideDepleted(BattleSideEnum.Defender))
				{
					if (this._hideoutMissionState == HideoutMissionController.HideoutMissionState.InitialFightBeforeBossFight)
					{
						if (this._firstPhaseEndTimer == null)
						{
							this._firstPhaseEndTimer = new Timer(base.Mission.CurrentTime, 4f, true);
							this._oldMissionMode = Mission.Current.Mode;
							Mission.Current.SetMissionMode(MissionMode.CutScene, false);
							return;
						}
						if (this._firstPhaseEndTimer.Check(base.Mission.CurrentTime))
						{
							this._cinematicController.StartCinematic(new HideoutCinematicController.OnInitialFadeOutFinished(this.OnInitialFadeOutOver), new Action(this.OnCutSceneOver), 0.4f, 0.2f, 8f, false);
							this._missionObjectiveLogic.CompleteCurrentObjective();
							return;
						}
					}
					else
					{
						if (this._hideoutMissionState == HideoutMissionController.HideoutMissionState.BossFightWithDuel)
						{
							this.OnDuelOver(BattleSideEnum.Attacker);
						}
						this._battleEndLogic.ChangeCanCheckForEndCondition(true);
						MapEvent.PlayerMapEvent.SetOverrideWinner(BattleSideEnum.Attacker);
						this._battleResolved = true;
						this._missionObjectiveLogic.CompleteCurrentObjective();
					}
				}
			}
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0002A82B File Offset: 0x00028A2B
		public void StartSpawner(BattleSideEnum side)
		{
			this._missionSides[(int)side].SetSpawnTroops(true);
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0002A83B File Offset: 0x00028A3B
		public void StopSpawner(BattleSideEnum side)
		{
			this._missionSides[(int)side].SetSpawnTroops(false);
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0002A84B File Offset: 0x00028A4B
		public bool IsSideSpawnEnabled(BattleSideEnum side)
		{
			return this._missionSides[(int)side].TroopSpawningActive;
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0002A85A File Offset: 0x00028A5A
		public float GetReinforcementInterval(BattleSideEnum battleSide = BattleSideEnum.None)
		{
			return 0f;
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0002A864 File Offset: 0x00028A64
		public unsafe bool IsSideDepleted(BattleSideEnum side)
		{
			bool flag = this._missionSides[(int)side].NumberOfActiveTroops == 0;
			if (!flag)
			{
				if ((Agent.Main == null || !Agent.Main.IsActive()) && side == BattleSideEnum.Attacker)
				{
					if (this._hideoutMissionState == HideoutMissionController.HideoutMissionState.BossFightWithDuel || this._hideoutMissionState == HideoutMissionController.HideoutMissionState.InitialFightBeforeBossFight)
					{
						flag = true;
					}
					else if (this._hideoutMissionState == HideoutMissionController.HideoutMissionState.WithoutBossFight || this._hideoutMissionState == HideoutMissionController.HideoutMissionState.BossFightWithAll)
					{
						bool flag2 = base.Mission.Teams.Attacker.FormationsIncludingEmpty.Any<Formation>(delegate(Formation f)
						{
							if (f.CountOfUnits > 0)
							{
								MovementOrder movementOrder = *f.GetReadonlyMovementOrderReference();
								return movementOrder.OrderType == OrderType.Charge;
							}
							return false;
						});
						bool flag3 = base.Mission.Teams.Defender.ActiveAgents.Any<Agent>((Agent t) => t.CurrentWatchState == Agent.WatchState.Alarmed);
						flag = !flag2 && !flag3;
					}
				}
				else if (side == BattleSideEnum.Defender && this._hideoutMissionState == HideoutMissionController.HideoutMissionState.BossFightWithDuel && (this._bossAgent == null || !this._bossAgent.IsActive()))
				{
					flag = true;
				}
			}
			else if (side == BattleSideEnum.Defender && this._hideoutMissionState == HideoutMissionController.HideoutMissionState.InitialFightBeforeBossFight && (Agent.Main == null || !Agent.Main.IsActive()))
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0002A99C File Offset: 0x00028B9C
		private void DecideMissionState()
		{
			HideoutMissionController.MissionSide missionSide = this._missionSides[0];
			this._hideoutMissionState = ((!missionSide.IsPlayerSide) ? HideoutMissionController.HideoutMissionState.InitialFightBeforeBossFight : HideoutMissionController.HideoutMissionState.WithoutBossFight);
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0002A9C4 File Offset: 0x00028BC4
		private void SetWatchStateOfAIAgents(Agent.WatchState state)
		{
			foreach (Agent agent in base.Mission.Agents)
			{
				if (agent.IsAIControlled)
				{
					agent.SetWatchState(state);
				}
			}
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0002AA24 File Offset: 0x00028C24
		private void SpawnBossAndBodyguards()
		{
			HideoutMissionController.MissionSide missionSide = this._missionSides[0];
			MatrixFrame banditsInitialFrame = this._cinematicController.GetBanditsInitialFrame();
			missionSide.SpawnRemainingTroopsForBossFight(new List<MatrixFrame> { banditsInitialFrame }, missionSide.NumberOfTroopsNotSupplied, this._overriddenHideoutBossCharacterObject);
			this._bossAgent = this.SelectBossAgent();
			this._bossAgent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.MeleeForMainHand);
			foreach (Agent agent in this._enemyTeam.ActiveAgents)
			{
				if (agent != this._bossAgent)
				{
					agent.WieldInitialWeapons(Agent.WeaponWieldActionType.WithAnimationUninterruptible, Equipment.InitialWeaponEquipPreference.Any);
				}
			}
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0002AAD4 File Offset: 0x00028CD4
		private Agent SelectBossAgent()
		{
			Agent agent = null;
			Agent agent2 = null;
			foreach (Agent agent3 in base.Mission.Agents)
			{
				if (agent3.Team == this._enemyTeam && agent3.IsHuman)
				{
					if (agent3.IsHero)
					{
						agent = agent3;
						agent2 = agent3;
						break;
					}
					if (agent3.Character.Culture.IsBandit)
					{
						CultureObject cultureObject = agent3.Character.Culture as CultureObject;
						if (((cultureObject != null) ? cultureObject.BanditBoss : null) != null && ((CultureObject)agent3.Character.Culture).BanditBoss == agent3.Character)
						{
							agent = agent3;
						}
					}
					if (agent2 == null || agent3.Character.Level > agent2.Character.Level)
					{
						agent2 = agent3;
					}
				}
			}
			agent = agent ?? agent2;
			return agent;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0002ABD0 File Offset: 0x00028DD0
		private void OnInitialFadeOutOver(ref Agent playerAgent, ref List<Agent> playerCompanions, ref Agent bossAgent, ref List<Agent> bossCompanions, ref float placementPerturbation, ref float placementAngle)
		{
			this._hideoutMissionState = HideoutMissionController.HideoutMissionState.CutSceneBeforeBossFight;
			this._enemyTeam = base.Mission.PlayerEnemyTeam;
			this.SpawnBossAndBodyguards();
			base.Mission.PlayerTeam.SetIsEnemyOf(this._enemyTeam, false);
			this.SetWatchStateOfAIAgents(Agent.WatchState.Patrolling);
			if (Agent.Main.IsUsingGameObject)
			{
				Agent.Main.StopUsingGameObject(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
			}
			playerAgent = Agent.Main;
			playerCompanions = base.Mission.Agents.Where<Agent>((Agent x) => x.IsActive() && x.Team == base.Mission.PlayerTeam && x.IsHuman && x.IsAIControlled).ToList<Agent>();
			bossAgent = this._bossAgent;
			bossCompanions = base.Mission.Agents.Where<Agent>((Agent x) => x.IsActive() && x.Team == this._enemyTeam && x.IsHuman && x.IsAIControlled && x != this._bossAgent).ToList<Agent>();
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0002AC87 File Offset: 0x00028E87
		private void OnCutSceneOver()
		{
			Mission.Current.SetMissionMode(this._oldMissionMode, false);
			this._hideoutMissionState = HideoutMissionController.HideoutMissionState.ConversationBetweenLeaders;
			MissionConversationLogic missionBehavior = base.Mission.GetMissionBehavior<MissionConversationLogic>();
			missionBehavior.DisableStartConversation(false);
			missionBehavior.StartConversation(this._bossAgent, false, false);
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0002ACC0 File Offset: 0x00028EC0
		private void OnDuelOver(BattleSideEnum winnerSide)
		{
			AgentVictoryLogic missionBehavior = base.Mission.GetMissionBehavior<AgentVictoryLogic>();
			if (missionBehavior != null)
			{
				missionBehavior.SetCheerActionGroup(AgentVictoryLogic.CheerActionGroupEnum.HighCheerActions);
			}
			if (missionBehavior != null)
			{
				missionBehavior.SetCheerReactionTimerSettings(0.25f, 3f);
			}
			if (winnerSide == BattleSideEnum.Attacker && this._duelPhaseAllyAgents != null)
			{
				using (List<Agent>.Enumerator enumerator = this._duelPhaseAllyAgents.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Agent agent = enumerator.Current;
						if (agent.State == AgentState.Active)
						{
							agent.SetTeam(base.Mission.PlayerTeam, true);
							agent.SetWatchState(Agent.WatchState.Alarmed);
						}
					}
					return;
				}
			}
			if (winnerSide == BattleSideEnum.Defender && this._duelPhaseBanditAgents != null)
			{
				foreach (Agent agent2 in this._duelPhaseBanditAgents)
				{
					if (agent2.State == AgentState.Active)
					{
						agent2.SetTeam(this._enemyTeam, true);
						agent2.SetWatchState(Agent.WatchState.Alarmed);
					}
				}
			}
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0002ADCC File Offset: 0x00028FCC
		private FormationClass GetHideoutMissionTroopClass(BattleSideEnum battleSide, BasicCharacterObject agentCharacter)
		{
			return agentCharacter.GetFormationClass().DismountedClass();
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0002ADD9 File Offset: 0x00028FD9
		public static void StartBossFightDuelMode()
		{
			Mission mission = Mission.Current;
			HideoutMissionController hideoutMissionController = ((mission != null) ? mission.GetMissionBehavior<HideoutMissionController>() : null);
			if (hideoutMissionController == null)
			{
				return;
			}
			hideoutMissionController.StartBossFightDuelModeInternal();
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0002ADF8 File Offset: 0x00028FF8
		private void StartBossFightDuelModeInternal()
		{
			base.Mission.GetMissionBehavior<MissionConversationLogic>().DisableStartConversation(true);
			base.Mission.PlayerTeam.SetIsEnemyOf(this._enemyTeam, true);
			this._duelPhaseAllyAgents = base.Mission.Agents.Where<Agent>((Agent x) => x.IsActive() && x.Team == base.Mission.PlayerTeam && x.IsHuman && x.IsAIControlled && x != Agent.Main).ToList<Agent>();
			this._duelPhaseBanditAgents = base.Mission.Agents.Where<Agent>((Agent x) => x.IsActive() && x.Team == this._enemyTeam && x.IsHuman && x.IsAIControlled && x != this._bossAgent).ToList<Agent>();
			foreach (Agent agent in this._duelPhaseAllyAgents)
			{
				agent.SetTeam(Team.Invalid, true);
				WorldPosition worldPosition = agent.GetWorldPosition();
				agent.SetScriptedPosition(ref worldPosition, false, Agent.AIScriptedFrameFlags.None);
				agent.SetLookAgent(Agent.Main);
			}
			foreach (Agent agent2 in this._duelPhaseBanditAgents)
			{
				agent2.SetTeam(Team.Invalid, true);
				WorldPosition worldPosition2 = agent2.GetWorldPosition();
				agent2.SetScriptedPosition(ref worldPosition2, false, Agent.AIScriptedFrameFlags.None);
				agent2.SetLookAgent(this._bossAgent);
			}
			this._bossAgent.SetWatchState(Agent.WatchState.Alarmed);
			this._hideoutMissionState = HideoutMissionController.HideoutMissionState.BossFightWithDuel;
			this._defeatHideoutBossObjective = new DefeatHideoutBossObjective(base.Mission, true);
			this._missionObjectiveLogic.StartObjective(this._defeatHideoutBossObjective);
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0002AF78 File Offset: 0x00029178
		public static void StartBossFightBattleMode()
		{
			Mission mission = Mission.Current;
			HideoutMissionController hideoutMissionController = ((mission != null) ? mission.GetMissionBehavior<HideoutMissionController>() : null);
			if (hideoutMissionController == null)
			{
				return;
			}
			hideoutMissionController.StartBossFightBattleModeInternal();
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0002AF98 File Offset: 0x00029198
		private void StartBossFightBattleModeInternal()
		{
			base.Mission.GetMissionBehavior<MissionConversationLogic>().DisableStartConversation(true);
			base.Mission.PlayerTeam.SetIsEnemyOf(this._enemyTeam, true);
			this.SetWatchStateOfAIAgents(Agent.WatchState.Alarmed);
			this._hideoutMissionState = HideoutMissionController.HideoutMissionState.BossFightWithAll;
			foreach (Formation formation in base.Mission.PlayerTeam.FormationsIncludingEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					formation.SetMovementOrder(MovementOrder.MovementOrderCharge);
					formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
				}
			}
			this._defeatHideoutBossObjective = new DefeatHideoutBossObjective(base.Mission, false);
			this._missionObjectiveLogic.StartObjective(this._defeatHideoutBossObjective);
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0002B068 File Offset: 0x00029268
		public IEnumerable<IAgentOriginBase> GetAllTroopsForSide(BattleSideEnum side)
		{
			return this._missionSides[(int)side].GetAllTroops();
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0002B084 File Offset: 0x00029284
		public int GetNumberOfPlayerControllableTroops()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0002B08B File Offset: 0x0002928B
		public bool GetSpawnHorses(BattleSideEnum side)
		{
			return false;
		}

		// Token: 0x0400034C RID: 844
		private const int FirstPhaseEndInSeconds = 4;

		// Token: 0x0400034E RID: 846
		private readonly List<CommonAreaMarker> _areaMarkers;

		// Token: 0x0400034F RID: 847
		private readonly List<PatrolArea> _patrolAreas;

		// Token: 0x04000350 RID: 848
		private readonly Dictionary<Agent, HideoutMissionController.UsedObject> _defenderAgentObjects;

		// Token: 0x04000351 RID: 849
		private readonly HideoutMissionController.MissionSide[] _missionSides;

		// Token: 0x04000352 RID: 850
		private List<Agent> _duelPhaseAllyAgents;

		// Token: 0x04000353 RID: 851
		private List<Agent> _duelPhaseBanditAgents;

		// Token: 0x04000354 RID: 852
		private BattleAgentLogic _battleAgentLogic;

		// Token: 0x04000355 RID: 853
		private BattleEndLogic _battleEndLogic;

		// Token: 0x04000356 RID: 854
		private AgentVictoryLogic _agentVictoryLogic;

		// Token: 0x04000357 RID: 855
		private HideoutMissionController.HideoutMissionState _hideoutMissionState;

		// Token: 0x04000358 RID: 856
		private Agent _bossAgent;

		// Token: 0x04000359 RID: 857
		private Team _enemyTeam;

		// Token: 0x0400035A RID: 858
		private Timer _firstPhaseEndTimer;

		// Token: 0x0400035B RID: 859
		private CharacterObject _overriddenHideoutBossCharacterObject;

		// Token: 0x0400035C RID: 860
		private bool _troopsInitialized;

		// Token: 0x0400035D RID: 861
		private bool _isMissionInitialized;

		// Token: 0x0400035E RID: 862
		private bool _battleResolved;

		// Token: 0x0400035F RID: 863
		private int _firstPhaseEnemyTroopCount;

		// Token: 0x04000360 RID: 864
		private int _firstPhasePlayerSideTroopCount;

		// Token: 0x04000361 RID: 865
		private MissionMode _oldMissionMode;

		// Token: 0x04000362 RID: 866
		private HideoutCinematicController _cinematicController;

		// Token: 0x04000363 RID: 867
		private MissionObjectiveLogic _missionObjectiveLogic;

		// Token: 0x04000364 RID: 868
		private ClearTheMainCampObjective _clearTheMainCampObjective;

		// Token: 0x04000365 RID: 869
		private DefeatHideoutBossObjective _defeatHideoutBossObjective;

		// Token: 0x04000366 RID: 870
		private readonly List<Agent> _clearObjectiveTargetAgents = new List<Agent>();

		// Token: 0x020001AC RID: 428
		private class MissionSide
		{
			// Token: 0x17000143 RID: 323
			// (get) Token: 0x06000F4B RID: 3915 RVA: 0x00068081 File Offset: 0x00066281
			// (set) Token: 0x06000F4C RID: 3916 RVA: 0x00068089 File Offset: 0x00066289
			public bool TroopSpawningActive { get; private set; }

			// Token: 0x17000144 RID: 324
			// (get) Token: 0x06000F4D RID: 3917 RVA: 0x00068092 File Offset: 0x00066292
			public int NumberOfActiveTroops
			{
				get
				{
					return this._numberOfSpawnedTroops - this._troopSupplier.NumRemovedTroops;
				}
			}

			// Token: 0x17000145 RID: 325
			// (get) Token: 0x06000F4E RID: 3918 RVA: 0x000680A6 File Offset: 0x000662A6
			public int NumberOfTroopsNotSupplied
			{
				get
				{
					return this._troopSupplier.NumTroopsNotSupplied;
				}
			}

			// Token: 0x06000F4F RID: 3919 RVA: 0x000680B3 File Offset: 0x000662B3
			public MissionSide(BattleSideEnum side, IMissionTroopSupplier troopSupplier, bool isPlayerSide)
			{
				this._side = side;
				this.IsPlayerSide = isPlayerSide;
				this._troopSupplier = troopSupplier;
			}

			// Token: 0x06000F50 RID: 3920 RVA: 0x000680D0 File Offset: 0x000662D0
			public void SpawnTroops(List<CommonAreaMarker> areaMarkers, List<PatrolArea> patrolAreas, Dictionary<Agent, HideoutMissionController.UsedObject> defenderAgentObjects, int spawnCount)
			{
				int num = 0;
				bool flag = false;
				List<StandingPoint> list = new List<StandingPoint>();
				foreach (CommonAreaMarker commonAreaMarker in areaMarkers)
				{
					foreach (UsableMachine usableMachine in commonAreaMarker.GetUsableMachinesInRange(null))
					{
						list.AddRange(usableMachine.StandingPoints);
					}
				}
				List<IAgentOriginBase> list2 = this._troopSupplier.SupplyTroops(spawnCount).ToList<IAgentOriginBase>();
				for (int i = 0; i < list2.Count; i++)
				{
					if (BattleSideEnum.Attacker == this._side)
					{
						Mission.Current.SpawnTroop(list2[i], true, true, false, false, 0, 0, true, true, null, null, null, null, FormationClass.NumberOfAllFormations, false);
						this._numberOfSpawnedTroops++;
					}
					else if (areaMarkers.Count > num)
					{
						StandingPoint standingPoint = null;
						int num2 = list2.Count - i;
						if (num2 < list.Count / 2 && num2 < 4)
						{
							flag = true;
						}
						if (!flag)
						{
							list.Shuffle<StandingPoint>();
							standingPoint = list.FirstOrDefault<StandingPoint>((StandingPoint point) => !point.IsDeactivated && !point.IsDisabled && !point.HasUser);
						}
						else
						{
							IEnumerable<PatrolArea> enumerable = patrolAreas.Where<PatrolArea>((PatrolArea area) => area.StandingPoints.All<StandingPoint>((StandingPoint point) => !point.HasUser && !point.HasAIMovingTo));
							if (!enumerable.IsEmpty<PatrolArea>())
							{
								foreach (StandingPoint standingPoint2 in enumerable.First<PatrolArea>().StandingPoints)
								{
									if (!standingPoint2.IsDisabled)
									{
										standingPoint = standingPoint2;
										break;
									}
								}
							}
						}
						if (standingPoint != null && !standingPoint.IsDisabled)
						{
							MatrixFrame globalFrame = standingPoint.GameEntity.GetGlobalFrame();
							globalFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
							Agent agent = Mission.Current.SpawnTroop(list2[i], false, false, false, false, 0, 0, false, false, new Vec3?(globalFrame.origin), new Vec2?(globalFrame.rotation.f.AsVec2.Normalized()), "_hideout_bandit", null, FormationClass.NumberOfAllFormations, false);
							this.InitializeBanditAgent(agent, standingPoint, flag, defenderAgentObjects);
							this._numberOfSpawnedTroops++;
							int groupId = ((AnimationPoint)standingPoint).GroupId;
							if (flag)
							{
								goto IL_02CE;
							}
							using (List<StandingPoint>.Enumerator enumerator3 = standingPoint.GameEntity.Parent.GetFirstScriptOfType<UsableMachine>().StandingPoints.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									StandingPoint standingPoint3 = enumerator3.Current;
									int groupId2 = ((AnimationPoint)standingPoint3).GroupId;
									if (groupId == groupId2 && standingPoint3 != standingPoint)
									{
										standingPoint3.SetDisabledAndMakeInvisible(false, false);
									}
								}
								goto IL_02CE;
							}
						}
						num++;
					}
					IL_02CE:;
				}
				foreach (Formation formation in Mission.Current.AttackerTeam.FormationsIncludingEmpty)
				{
					if (formation.CountOfUnits > 0)
					{
						formation.SetMovementOrder(MovementOrder.MovementOrderMove(formation.CachedMedianPosition));
					}
					formation.SetFiringOrder(FiringOrder.FiringOrderHoldYourFire);
					if (Mission.Current.AttackerTeam == Mission.Current.PlayerTeam)
					{
						formation.PlayerOwner = Mission.Current.MainAgent;
					}
				}
			}

			// Token: 0x06000F51 RID: 3921 RVA: 0x00068488 File Offset: 0x00066688
			public void SpawnRemainingTroopsForBossFight(List<MatrixFrame> spawnFrames, int spawnCount, CharacterObject overriddenHideoutBossCharacterObject)
			{
				List<IAgentOriginBase> list = this._troopSupplier.SupplyTroops(spawnCount).ToList<IAgentOriginBase>();
				if (overriddenHideoutBossCharacterObject != null)
				{
					IAgentOriginBase agentOriginBase = list.Find((IAgentOriginBase t) => t.Troop == overriddenHideoutBossCharacterObject);
					MatrixFrame matrixFrame = spawnFrames.FirstOrDefault<MatrixFrame>();
					matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
					Agent agent = Mission.Current.SpawnTroop(agentOriginBase, false, false, false, false, 0, 0, false, false, new Vec3?(matrixFrame.origin), new Vec2?(matrixFrame.rotation.f.AsVec2.Normalized()), "_hideout_bandit", null, FormationClass.NumberOfAllFormations, false);
					this._numberOfSpawnedTroops++;
					AgentFlag agentFlags = agent.GetAgentFlags();
					if (agentFlags.HasAnyFlag(AgentFlag.CanRetreat))
					{
						agent.SetAgentFlags(agentFlags & ~AgentFlag.CanRetreat);
					}
					list.Remove(agentOriginBase);
				}
				for (int i = 0; i < list.Count; i++)
				{
					MatrixFrame matrixFrame2 = spawnFrames.FirstOrDefault<MatrixFrame>();
					matrixFrame2.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
					Agent agent2 = Mission.Current.SpawnTroop(list[i], false, false, false, false, 0, 0, false, false, new Vec3?(matrixFrame2.origin), new Vec2?(matrixFrame2.rotation.f.AsVec2.Normalized()), "_hideout_bandit", null, FormationClass.NumberOfAllFormations, false);
					AgentFlag agentFlags2 = agent2.GetAgentFlags();
					if (agentFlags2.HasAnyFlag(AgentFlag.CanRetreat))
					{
						agent2.SetAgentFlags(agentFlags2 & ~AgentFlag.CanRetreat);
					}
					this._numberOfSpawnedTroops++;
				}
				foreach (Formation formation in Mission.Current.AttackerTeam.FormationsIncludingEmpty)
				{
					if (formation.CountOfUnits > 0)
					{
						formation.SetMovementOrder(MovementOrder.MovementOrderMove(formation.CachedMedianPosition));
					}
					formation.SetFiringOrder(FiringOrder.FiringOrderHoldYourFire);
					if (Mission.Current.AttackerTeam == Mission.Current.PlayerTeam)
					{
						formation.PlayerOwner = Mission.Current.MainAgent;
					}
				}
			}

			// Token: 0x06000F52 RID: 3922 RVA: 0x000686B8 File Offset: 0x000668B8
			private void InitializeBanditAgent(Agent agent, StandingPoint spawnPoint, bool isPatrolling, Dictionary<Agent, HideoutMissionController.UsedObject> defenderAgentObjects)
			{
				UsableMachine usableMachine = (isPatrolling ? spawnPoint.GameEntity.Parent.GetFirstScriptOfType<PatrolArea>() : spawnPoint.GameEntity.Parent.GetFirstScriptOfType<UsableMachine>());
				if (isPatrolling)
				{
					((IDetachment)usableMachine).AddAgent(agent, -1, Agent.AIScriptedFrameFlags.None);
					agent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
				}
				else
				{
					agent.UseGameObject(spawnPoint, -1);
				}
				defenderAgentObjects.Add(agent, new HideoutMissionController.UsedObject(usableMachine, isPatrolling));
				AgentFlag agentFlags = agent.GetAgentFlags();
				agent.SetAgentFlags((agentFlags | AgentFlag.CanGetAlarmed) & ~AgentFlag.CanRetreat);
				agent.GetComponent<CampaignAgentComponent>().CreateAgentNavigator().AddBehaviorGroup<AlarmedBehaviorGroup>()
					.AddBehavior<CautiousBehavior>();
				this.SimulateTick(agent);
			}

			// Token: 0x06000F53 RID: 3923 RVA: 0x0006875C File Offset: 0x0006695C
			private void SimulateTick(Agent agent)
			{
				int num = MBRandom.RandomInt(1, 20);
				for (int i = 0; i < num; i++)
				{
					if (agent.IsUsingGameObject)
					{
						agent.CurrentlyUsedGameObject.SimulateTick(0.1f);
					}
				}
			}

			// Token: 0x06000F54 RID: 3924 RVA: 0x00068796 File Offset: 0x00066996
			public void SetSpawnTroops(bool spawnTroops)
			{
				this.TroopSpawningActive = spawnTroops;
			}

			// Token: 0x06000F55 RID: 3925 RVA: 0x0006879F File Offset: 0x0006699F
			public IEnumerable<IAgentOriginBase> GetAllTroops()
			{
				return this._troopSupplier.GetAllTroops();
			}

			// Token: 0x040007D3 RID: 2003
			private readonly BattleSideEnum _side;

			// Token: 0x040007D4 RID: 2004
			private readonly IMissionTroopSupplier _troopSupplier;

			// Token: 0x040007D5 RID: 2005
			public readonly bool IsPlayerSide;

			// Token: 0x040007D7 RID: 2007
			private int _numberOfSpawnedTroops;
		}

		// Token: 0x020001AD RID: 429
		private class UsedObject
		{
			// Token: 0x06000F56 RID: 3926 RVA: 0x000687AC File Offset: 0x000669AC
			public UsedObject(UsableMachine machine, bool isMachineAITicked)
			{
				this.Machine = machine;
				this.MachineAI = machine.CreateAIBehaviorObject();
				this.IsMachineAITicked = isMachineAITicked;
			}

			// Token: 0x040007D8 RID: 2008
			public readonly UsableMachine Machine;

			// Token: 0x040007D9 RID: 2009
			public readonly UsableMachineAIBase MachineAI;

			// Token: 0x040007DA RID: 2010
			public bool IsMachineAITicked;
		}

		// Token: 0x020001AE RID: 430
		private enum HideoutMissionState
		{
			// Token: 0x040007DC RID: 2012
			NotDecided,
			// Token: 0x040007DD RID: 2013
			WithoutBossFight,
			// Token: 0x040007DE RID: 2014
			InitialFightBeforeBossFight,
			// Token: 0x040007DF RID: 2015
			CutSceneBeforeBossFight,
			// Token: 0x040007E0 RID: 2016
			ConversationBetweenLeaders,
			// Token: 0x040007E1 RID: 2017
			BossFightWithDuel,
			// Token: 0x040007E2 RID: 2018
			BossFightWithAll
		}
	}
}
