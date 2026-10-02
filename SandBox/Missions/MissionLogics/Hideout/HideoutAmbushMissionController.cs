using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Conversation.MissionLogics;
using SandBox.Missions.MissionLogics.Hideout.Objectives;
using SandBox.Objects.AreaMarkers;
using SandBox.Objects.Usables;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Missions.MissionLogics;

namespace SandBox.Missions.MissionLogics.Hideout
{
	// Token: 0x02000094 RID: 148
	public class HideoutAmbushMissionController : MissionLogic
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x000271EC File Offset: 0x000253EC
		public bool IsReadyForCallTroopsCinematic
		{
			get
			{
				return this._currentHideoutMissionState == HideoutAmbushMissionController.HideoutMissionState.CallTroopsCutSceneState;
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x000271F8 File Offset: 0x000253F8
		public HideoutAmbushMissionController(IMissionTroopSupplier[] suppliers, BattleSideEnum playerSide, int playerTroopCount)
		{
			this._playerSide = playerSide;
			this._playerTroopCount = playerTroopCount;
			this._stealthAreaData = new List<StealthAreaMissionLogic.StealthAreaData>();
			this._waitTimerToChangeStealthModeIntoBattle = null;
			this._currentHideoutMissionState = HideoutAmbushMissionController.HideoutMissionState.NotDecided;
			this._overriddenHideoutBossCharacterObject = null;
			this._suppliers = suppliers;
			IMissionTroopSupplier missionTroopSupplier = this._suppliers[(int)this._playerSide.GetOppositeSide()];
			this._initialHideoutPopulation = missionTroopSupplier.NumTroopsNotSupplied;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00027274 File Offset: 0x00025474
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._battleAgentLogic = base.Mission.GetMissionBehavior<BattleAgentLogic>();
			this._battleEndLogic = base.Mission.GetMissionBehavior<BattleEndLogic>();
			this._battleEndLogic.ChangeCanCheckForEndCondition(false);
			this._stealthAreaMissionLogic = base.Mission.GetMissionBehavior<StealthAreaMissionLogic>();
			StealthAreaMissionLogic stealthAreaMissionLogic = this._stealthAreaMissionLogic;
			stealthAreaMissionLogic.SpawnReinforcementAllyTroopsEvent = (StealthAreaMissionLogic.SpawnReinforcementAllyTroopsDelegate)Delegate.Combine(stealthAreaMissionLogic.SpawnReinforcementAllyTroopsEvent, new StealthAreaMissionLogic.SpawnReinforcementAllyTroopsDelegate(this.SpawnReinforcementAllyTroops));
			this._missionObjectiveLogic = base.Mission.GetMissionBehavior<MissionObjectiveLogic>();
			this._hideoutAmbushBossFightCinematicController = base.Mission.GetMissionBehavior<HideoutAmbushBossFightCinematicController>();
			foreach (StealthAreaUsePoint stealthAreaUsePoint in base.Mission.ActiveMissionObjects.FindAllWithType<StealthAreaUsePoint>())
			{
				this._stealthAreaData.Add(new StealthAreaMissionLogic.StealthAreaData(stealthAreaUsePoint));
			}
			Game.Current.EventManager.RegisterEvent<OnStealthMissionCounterFailedEvent>(new Action<OnStealthMissionCounterFailedEvent>(this.OnStealthMissionCounterFailed));
			base.Mission.GetAgentTroopClass_Override += this.GetHideoutAmbushMissionTroopClass;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00027398 File Offset: 0x00025598
		public override void OnCreated()
		{
			base.OnCreated();
			base.Mission.DoesMissionRequireCivilianEquipment = false;
			CampaignEvents.LocationCharactersAreReadyToSpawnEvent.AddNonSerializedListener(this, new Action<Dictionary<string, int>>(this.LocationCharactersAreReadyToSpawn));
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x000273C4 File Offset: 0x000255C4
		public override void AfterStart()
		{
			base.AfterStart();
			this.InitializeTroops();
			SandBoxHelpers.MissionHelper.SpawnPlayer(false, true, false, false, "");
			Mission.Current.GetMissionBehavior<MissionAgentHandler>().SpawnLocationCharacters(null);
			Agent.Main.SetClothingColor1(4279111698U);
			Agent.Main.SetClothingColor2(4279111698U);
			Agent.Main.UpdateSpawnEquipmentAndRefreshVisuals(Hero.MainHero.StealthEquipment);
			foreach (StealthAreaMissionLogic.StealthAreaData stealthAreaData in this._stealthAreaData)
			{
				foreach (KeyValuePair<StealthAreaMarker, List<Agent>> keyValuePair in stealthAreaData.StealthAreaMarkers)
				{
					this._sentryCount += keyValuePair.Value.Count;
					this._remainingSentryCount += keyValuePair.Value.Count;
				}
			}
			Mission.Current.GetMissionBehavior<StealthFailCounterMissionLogic>().FailCounterSeconds = 15f;
			this._locateTheMainCampObjective = new LocateTheMainCampObjective(base.Mission);
			this._missionObjectiveLogic.StartObjective(this._locateTheMainCampObjective);
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00027510 File Offset: 0x00025710
		public override void OnAfterMissionLoadingFinished()
		{
			base.Mission.OnInitialSpawnCompleted(BattleSideEnum.None);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0002751E File Offset: 0x0002571E
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
			StealthAreaMissionLogic stealthAreaMissionLogic = this._stealthAreaMissionLogic;
			stealthAreaMissionLogic.SpawnReinforcementAllyTroopsEvent = (StealthAreaMissionLogic.SpawnReinforcementAllyTroopsDelegate)Delegate.Remove(stealthAreaMissionLogic.SpawnReinforcementAllyTroopsEvent, new StealthAreaMissionLogic.SpawnReinforcementAllyTroopsDelegate(this.SpawnReinforcementAllyTroops));
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00027550 File Offset: 0x00025750
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._waitTimerToChangeStealthModeIntoBattle != null && this._waitTimerToChangeStealthModeIntoBattle.Check(base.Mission.CurrentTime))
			{
				Agent main = Agent.Main;
				if (main != null && main.IsActive())
				{
					this.ChangeHideoutMissionModeToBattle();
					this._waitTimerToChangeStealthModeIntoBattle = null;
				}
			}
			if (!this._isMissionInitialized)
			{
				Agent main2 = Agent.Main;
				if (main2 != null && main2.IsActive())
				{
					this.InitializeMission();
					this._isMissionInitialized = true;
					return;
				}
			}
			if (this._isMissionInitialized)
			{
				if (!this._troopsInitialized)
				{
					this._troopsInitialized = true;
					foreach (Agent agent in base.Mission.Agents)
					{
						this._battleAgentLogic.OnAgentBuild(agent, null);
					}
				}
				if (!this._battleResolved)
				{
					this.CheckBattleResolved();
					return;
				}
				if (!base.Mission.ForceNoFriendlyFire)
				{
					base.Mission.ForceNoFriendlyFire = true;
				}
			}
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00027660 File Offset: 0x00025860
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			if (this._currentHideoutMissionState < HideoutAmbushMissionController.HideoutMissionState.CutSceneBeforeBossFight && agent.IsHuman && agent.Team == Mission.Current.PlayerEnemyTeam)
			{
				foreach (StealthAreaMissionLogic.StealthAreaData stealthAreaData in this._stealthAreaData)
				{
					foreach (KeyValuePair<StealthAreaMarker, List<Agent>> keyValuePair in stealthAreaData.StealthAreaMarkers)
					{
						if (keyValuePair.Key.IsPositionInRange(agent.Position))
						{
							stealthAreaData.AddAgentToStealthAreaMarker(keyValuePair.Key, agent);
							break;
						}
					}
				}
			}
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00027738 File Offset: 0x00025938
		public override void OnAgentAlarmedStateChanged(Agent agent, Agent.AIStateFlag flag)
		{
			if (agent.IsAlarmed() && this._currentHideoutMissionState == HideoutAmbushMissionController.HideoutMissionState.StealthState)
			{
				this._isClearedAsGhost = false;
			}
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00027754 File Offset: 0x00025954
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (this._clearObjectiveTargetAgents.Contains(affectedAgent))
			{
				this._clearObjectiveTargetAgents.Remove(affectedAgent);
			}
			if (this._currentHideoutMissionState == HideoutAmbushMissionController.HideoutMissionState.StealthState)
			{
				this._isClearedAsGhost = false;
			}
			if (affectorAgent != null && affectorAgent.IsMainAgent)
			{
				this._remainingSentryCount = 0;
				foreach (StealthAreaMissionLogic.StealthAreaData stealthAreaData in this._stealthAreaData)
				{
					foreach (KeyValuePair<StealthAreaMarker, List<Agent>> keyValuePair in stealthAreaData.StealthAreaMarkers)
					{
						if (keyValuePair.Value.Contains(affectedAgent) || keyValuePair.Value.IsEmpty<Agent>())
						{
							stealthAreaData.RemoveAgentFromStealthAreaMarker(keyValuePair.Key, affectedAgent);
						}
						this._remainingSentryCount += keyValuePair.Value.Count;
					}
				}
			}
			if (this._currentHideoutMissionState == HideoutAmbushMissionController.HideoutMissionState.BossFightWithDuel)
			{
				using (List<Agent>.Enumerator enumerator3 = base.Mission.Agents.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Agent agent = enumerator3.Current;
						if (agent != affectedAgent && agent != affectorAgent && agent.IsActive() && agent.GetLookAgent() == affectedAgent)
						{
							agent.SetLookAgent(null);
						}
					}
					return;
				}
			}
			if ((this._currentHideoutMissionState == HideoutAmbushMissionController.HideoutMissionState.StealthState || this._currentHideoutMissionState == HideoutAmbushMissionController.HideoutMissionState.BattleBeforeBossFight) && affectedAgent.IsMainAgent)
			{
				base.Mission.PlayerTeam.PlayerOrderController.SelectAllFormations(false);
				affectedAgent.Formation = null;
				base.Mission.PlayerTeam.PlayerOrderController.SetOrder(OrderType.Retreat);
			}
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00027924 File Offset: 0x00025B24
		protected override void OnEndMission()
		{
			CampaignEventDispatcher.Instance.RemoveListeners(this);
			int num = 0;
			if (this._currentHideoutMissionState == HideoutAmbushMissionController.HideoutMissionState.BossFightWithDuel)
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
			if (!PlayerEncounter.EnemySurrender && num <= 0 && MobileParty.MainParty.MemberRoster.TotalHealthyCount <= 0 && MapEvent.PlayerMapEvent.BattleState == BattleState.None)
			{
				MapEvent.PlayerMapEvent.SetOverrideWinner(base.Mission.PlayerEnemyTeam.Side);
			}
			Game.Current.EventManager.UnregisterEvent<OnStealthMissionCounterFailedEvent>(new Action<OnStealthMissionCounterFailedEvent>(this.OnStealthMissionCounterFailed));
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x000279E8 File Offset: 0x00025BE8
		public override void OnMissionStateFinalized()
		{
			base.Mission.GetAgentTroopClass_Override -= this.GetHideoutAmbushMissionTroopClass;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00027A04 File Offset: 0x00025C04
		public override void OnObjectUsed(Agent userAgent, UsableMissionObject usedObject)
		{
			if (usedObject is StealthAreaUsePoint)
			{
				StealthAreaMissionLogic.StealthAreaData stealthAreaData = null;
				foreach (StealthAreaMissionLogic.StealthAreaData stealthAreaData2 in this._stealthAreaData)
				{
					if (stealthAreaData2.StealthAreaUsePoint == usedObject)
					{
						stealthAreaData = stealthAreaData2;
						break;
					}
				}
				if (stealthAreaData != null)
				{
					this._currentHideoutMissionState = HideoutAmbushMissionController.HideoutMissionState.CallTroopsCutSceneState;
					this._waitTimerToChangeStealthModeIntoBattle = new Timer(base.Mission.CurrentTime, 10f, true);
					this._missionObjectiveLogic.CompleteCurrentObjective();
				}
				List<Agent> list = new List<Agent>();
				foreach (StealthAreaMissionLogic.StealthAreaData stealthAreaData3 in this._stealthAreaData)
				{
					foreach (KeyValuePair<StealthAreaMarker, List<Agent>> keyValuePair in stealthAreaData3.StealthAreaMarkers)
					{
						list.AddRange(keyValuePair.Value);
					}
				}
				foreach (Agent agent in list)
				{
					agent.FadeOut(true, true);
					this._remainingSentryCount--;
				}
				if (this._isClearedAsGhost)
				{
					Campaign.Current.SkillLevelingManager.OnHideoutClearedAsGhost();
				}
			}
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00027B88 File Offset: 0x00025D88
		public void OnStealthMissionCounterFailed(OnStealthMissionCounterFailedEvent obj)
		{
			if (!this._battleResolved)
			{
				Campaign.Current.SkillLevelingManager.OnHideoutMissionEnd(false);
			}
			Campaign.Current.GameMenuManager.SetNextMenu("hideout_after_found_by_sentries");
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00027BB8 File Offset: 0x00025DB8
		public bool IsSideDepleted(BattleSideEnum side)
		{
			bool flag = ((side == BattleSideEnum.Attacker) ? Mission.Current.Teams.Attacker : Mission.Current.Teams.Defender).ActiveAgents.Count == 0;
			if (!flag)
			{
				if (this._playerSide == side)
				{
					if (Agent.Main == null || !Agent.Main.IsActive())
					{
						if (this._currentHideoutMissionState == HideoutAmbushMissionController.HideoutMissionState.BossFightWithDuel || this._currentHideoutMissionState == HideoutAmbushMissionController.HideoutMissionState.BattleBeforeBossFight)
						{
							flag = true;
						}
						else if (this._currentHideoutMissionState == HideoutAmbushMissionController.HideoutMissionState.BossFightWithAll)
						{
							flag = base.Mission.PlayerTeam.ActiveAgents.IsEmpty<Agent>() && !base.Mission.PlayerEnemyTeam.ActiveAgents.IsEmpty<Agent>();
						}
					}
				}
				else if (this._currentHideoutMissionState == HideoutAmbushMissionController.HideoutMissionState.BossFightWithDuel && (this._bossAgent == null || !this._bossAgent.IsActive()))
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00027C8E File Offset: 0x00025E8E
		public void SetOverriddenHideoutBossCharacterObject(CharacterObject characterObject)
		{
			this._overriddenHideoutBossCharacterObject = characterObject;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00027C98 File Offset: 0x00025E98
		public void OnAgentsShouldBeEnabled()
		{
			foreach (Agent agent in Mission.Current.Agents)
			{
				if (agent.IsActive() && agent.IsAIControlled)
				{
					agent.SetIsAIPaused(false);
				}
			}
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00027D00 File Offset: 0x00025F00
		public static void StartBossFightDuelMode()
		{
			Mission mission = Mission.Current;
			HideoutAmbushMissionController hideoutAmbushMissionController = ((mission != null) ? mission.GetMissionBehavior<HideoutAmbushMissionController>() : null);
			if (hideoutAmbushMissionController == null)
			{
				return;
			}
			hideoutAmbushMissionController.StartBossFightDuelModeInternal();
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00027D1D File Offset: 0x00025F1D
		public static void StartBossFightBattleMode()
		{
			Mission mission = Mission.Current;
			HideoutAmbushMissionController hideoutAmbushMissionController = ((mission != null) ? mission.GetMissionBehavior<HideoutAmbushMissionController>() : null);
			if (hideoutAmbushMissionController == null)
			{
				return;
			}
			hideoutAmbushMissionController.StartBossFightBattleModeInternal();
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00027D3C File Offset: 0x00025F3C
		private IAgentOriginBase GetOneEnemyTroopToSpawnInFirstPhase()
		{
			IAgentOriginBase agentOriginBase;
			if (this._allEnemyTroops.Count > 0)
			{
				agentOriginBase = this._allEnemyTroops.GetRandomElement<IAgentOriginBase>();
				this._allEnemyTroops.Remove(agentOriginBase);
			}
			else
			{
				agentOriginBase = this.GetNewRandomEnemyTroop();
			}
			return agentOriginBase;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00027D7C File Offset: 0x00025F7C
		private IAgentOriginBase GetNewRandomEnemyTroop()
		{
			IAgentOriginBase randomElement = this._allEnemyTroopTypesCache.GetRandomElement<IAgentOriginBase>();
			CharacterObject characterObject = (CharacterObject)randomElement.Troop;
			return new PartyAgentOrigin(((PartyGroupAgentOrigin)randomElement).Party, characterObject, -1, default(UniqueTroopDescriptor), false, true);
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00027DBC File Offset: 0x00025FBC
		private void SpawnRemainingTroopsForBossFight(List<MatrixFrame> spawnFrames, int spawnCount)
		{
			int count = this._allEnemyTroops.Count;
			for (int i = 0; i < spawnCount - count; i++)
			{
				this._allEnemyTroops.Add(this.GetNewRandomEnemyTroop());
			}
			if (this._overriddenHideoutBossAgentOrigin != null)
			{
				MatrixFrame matrixFrame = spawnFrames.FirstOrDefault<MatrixFrame>();
				matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
				Agent agent = Mission.Current.SpawnTroop(this._overriddenHideoutBossAgentOrigin, false, false, false, false, 0, 0, false, false, new Vec3?(matrixFrame.origin), new Vec2?(matrixFrame.rotation.f.AsVec2.Normalized()), "_hideout_bandit", null, FormationClass.NumberOfAllFormations, false);
				AgentFlag agentFlags = agent.GetAgentFlags();
				if (agentFlags.HasAnyFlag(AgentFlag.CanRetreat))
				{
					agent.SetAgentFlags(agentFlags & ~AgentFlag.CanRetreat);
				}
			}
			for (int j = 0; j < this._allEnemyTroops.Count; j++)
			{
				MatrixFrame matrixFrame2 = spawnFrames.FirstOrDefault<MatrixFrame>();
				matrixFrame2.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
				Agent agent2 = Mission.Current.SpawnTroop(this._allEnemyTroops[j], false, false, false, false, 0, 0, false, false, new Vec3?(matrixFrame2.origin), new Vec2?(matrixFrame2.rotation.f.AsVec2.Normalized()), "_hideout_bandit", null, FormationClass.NumberOfAllFormations, false);
				AgentFlag agentFlags2 = agent2.GetAgentFlags();
				if (agentFlags2.HasAnyFlag(AgentFlag.CanRetreat))
				{
					agent2.SetAgentFlags(agentFlags2 & ~AgentFlag.CanRetreat);
				}
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

		// Token: 0x060005FA RID: 1530 RVA: 0x00027FCC File Offset: 0x000261CC
		private Agent SpawnAllyAgent(IAgentOriginBase character, GameEntity spawnPoint, Vec3 position)
		{
			MatrixFrame globalFrame = spawnPoint.GetGlobalFrame();
			Agent agent = Mission.Current.SpawnTroop(character, true, false, false, false, 0, 0, true, true, new Vec3?(globalFrame.origin), new Vec2?(globalFrame.rotation.f.AsVec2.Normalized()), null, null, FormationClass.NumberOfAllFormations, false);
			CharacterObject characterObject;
			if ((characterObject = character.Troop as CharacterObject) != null && characterObject.IsHero)
			{
				Equipment stealthEquipment = characterObject.HeroObject.StealthEquipment;
				if (stealthEquipment != null && !stealthEquipment.IsEmpty())
				{
					agent.SetClothingColor1(4279111698U);
					agent.SetClothingColor2(4279111698U);
					agent.UpdateSpawnEquipmentAndRefreshVisuals(stealthEquipment);
				}
			}
			Vec3 randomPositionAroundPoint = Mission.Current.GetRandomPositionAroundPoint(position, 0f, 2f, true);
			WorldPosition worldPosition = new WorldPosition(spawnPoint.Scene, randomPositionAroundPoint);
			agent.SetScriptedPosition(ref worldPosition, true, Agent.AIScriptedFrameFlags.NoAttack | Agent.AIScriptedFrameFlags.Crouch);
			return agent;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x000280A8 File Offset: 0x000262A8
		private void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedUsablePointCount)
		{
			Location locationWithId = Settlement.CurrentSettlement.LocationComplex.GetLocationWithId("hideout_center");
			int num;
			if (unusedUsablePointCount.TryGetValue("stealth_agent_forced", out num))
			{
				locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(this.CreateForcedSentry), Settlement.CurrentSettlement.Culture, LocationCharacter.CharacterRelations.Enemy, num);
			}
			if (unusedUsablePointCount.TryGetValue("stealth_agent", out num))
			{
				int num2 = this._initialHideoutPopulation / 8;
				if (num2 >= 1)
				{
					locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(this.CreateSentry), Settlement.CurrentSettlement.Culture, LocationCharacter.CharacterRelations.Enemy, Math.Min(num2, num));
				}
			}
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00028138 File Offset: 0x00026338
		private LocationCharacter CreateForcedSentry(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			IAgentOriginBase oneEnemyTroopToSpawnInFirstPhase = this.GetOneEnemyTroopToSpawnInFirstPhase();
			CharacterObject characterObject = (CharacterObject)oneEnemyTroopToSpawnInFirstPhase.Troop;
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(characterObject, out num, out num2, "");
			return new LocationCharacter(new AgentData(oneEnemyTroopToSpawnInFirstPhase).Monster(TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(characterObject.Race, "_settlement_slow")).Age(MBRandom.RandomInt(num, num2)), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddStealthAgentBehaviors), "stealth_agent_forced", true, relation, null, false, false, null, false, false, true, null, true);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x000281C8 File Offset: 0x000263C8
		private LocationCharacter CreateSentry(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			IAgentOriginBase oneEnemyTroopToSpawnInFirstPhase = this.GetOneEnemyTroopToSpawnInFirstPhase();
			CharacterObject characterObject = (CharacterObject)oneEnemyTroopToSpawnInFirstPhase.Troop;
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(characterObject, out num, out num2, "");
			return new LocationCharacter(new AgentData(oneEnemyTroopToSpawnInFirstPhase).Monster(TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(characterObject.Race, "_settlement_slow")).Age(MBRandom.RandomInt(num, num2)), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddStealthAgentBehaviors), "stealth_agent", true, relation, null, false, false, null, false, false, true, null, false);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00028258 File Offset: 0x00026458
		private void InitializeMission()
		{
			base.Mission.GetMissionBehavior<MissionConversationLogic>().DisableStartConversation(true);
			base.Mission.SetMissionMode(MissionMode.Stealth, true);
			this._currentHideoutMissionState = HideoutAmbushMissionController.HideoutMissionState.StealthState;
			base.Mission.DeploymentPlan.MakeDefaultDeploymentPlans();
			List<GameEntity> list = new List<GameEntity>();
			Mission.Current.Scene.GetAllEntitiesWithScriptComponent<Chair>(ref list);
			foreach (GameEntity gameEntity in list)
			{
				foreach (StandingPoint standingPoint in gameEntity.GetFirstScriptOfType<Chair>().StandingPoints)
				{
					standingPoint.IsDisabledForPlayers = true;
				}
			}
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00028330 File Offset: 0x00026530
		private void ChangeHideoutMissionModeToBattle()
		{
			this._currentHideoutMissionState = HideoutAmbushMissionController.HideoutMissionState.BattleBeforeBossFight;
			Mission.Current.SetMissionMode(MissionMode.Battle, false);
			foreach (Agent agent in Mission.Current.PlayerTeam.ActiveAgents)
			{
				if (!agent.IsMainAgent)
				{
					agent.ClearTargetFrame();
					agent.DisableScriptedMovement();
				}
			}
			base.Mission.PlayerTeam.PlayerOrderController.SelectAllFormations(false);
			base.Mission.PlayerTeam.PlayerOrderController.SetOrder(OrderType.Charge);
			base.Mission.PlayerEnemyTeam.MasterOrderController.SelectAllFormations(false);
			base.Mission.PlayerEnemyTeam.MasterOrderController.SetOrder(OrderType.Charge);
			foreach (Agent agent2 in base.Mission.PlayerEnemyTeam.ActiveAgents)
			{
				agent2.SetAlarmState(Agent.AIStateFlag.Alarmed);
				this._clearObjectiveTargetAgents.Add(agent2);
			}
			string text = "event:/ui/mission/horns/attack";
			Vec3 position = Agent.Main.Position;
			SoundManager.StartOneShotEvent(text, in position);
			this._clearTheMainCampObjective = new ClearTheMainCampObjective(base.Mission, this._clearObjectiveTargetAgents);
			this._missionObjectiveLogic.StartObjective(this._clearTheMainCampObjective);
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x000284A0 File Offset: 0x000266A0
		private void CheckBattleResolved()
		{
			if (this._currentHideoutMissionState != HideoutAmbushMissionController.HideoutMissionState.NotDecided && this._currentHideoutMissionState != HideoutAmbushMissionController.HideoutMissionState.CutSceneBeforeBossFight && this._currentHideoutMissionState != HideoutAmbushMissionController.HideoutMissionState.ConversationBetweenLeaders)
			{
				if (this.IsSideDepleted(base.Mission.PlayerTeam.Side))
				{
					if (this._currentHideoutMissionState == HideoutAmbushMissionController.HideoutMissionState.BossFightWithDuel)
					{
						this.OnDuelOver(base.Mission.PlayerEnemyTeam.Side);
					}
					Campaign.Current.SkillLevelingManager.OnHideoutMissionEnd(false);
					this._battleEndLogic.ChangeCanCheckForEndCondition(true);
					this._battleResolved = true;
					this._missionObjectiveLogic.CompleteCurrentObjective();
					return;
				}
				if (this.IsSideDepleted(base.Mission.PlayerEnemyTeam.Side))
				{
					if (this._currentHideoutMissionState == HideoutAmbushMissionController.HideoutMissionState.BattleBeforeBossFight || this._currentHideoutMissionState == HideoutAmbushMissionController.HideoutMissionState.StealthState)
					{
						Agent main = Agent.Main;
						if (main != null && main.IsActive())
						{
							if (this._firstPhaseEndTimer == null)
							{
								this._firstPhaseEndTimer = new Timer(base.Mission.CurrentTime, 4f, true);
								Mission.Current.SetMissionMode(MissionMode.CutScene, false);
								return;
							}
							if (this._firstPhaseEndTimer.Check(base.Mission.CurrentTime))
							{
								this._hideoutAmbushBossFightCinematicController.StartCinematic(new HideoutAmbushBossFightCinematicController.OnInitialFadeOutFinished(this.OnInitialFadeOutOver), new Action(this.OnCutSceneOver), 0.4f, 0.2f, 8f, false);
								this._missionObjectiveLogic.CompleteCurrentObjective();
								return;
							}
						}
					}
					else
					{
						if (this._currentHideoutMissionState == HideoutAmbushMissionController.HideoutMissionState.BossFightWithDuel)
						{
							this.OnDuelOver(base.Mission.PlayerTeam.Side);
						}
						Campaign.Current.SkillLevelingManager.OnHideoutMissionEnd(true);
						this._battleEndLogic.ChangeCanCheckForEndCondition(true);
						MapEvent.PlayerMapEvent.SetOverrideWinner(base.Mission.PlayerTeam.Side);
						this._battleResolved = true;
						this._missionObjectiveLogic.CompleteCurrentObjective();
					}
				}
			}
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00028670 File Offset: 0x00026870
		private void InitializeTroops()
		{
			if (this._overriddenHideoutBossCharacterObject == null)
			{
				this._overriddenHideoutBossCharacterObject = Settlement.CurrentSettlement.Culture.BanditBoss;
			}
			IMissionTroopSupplier missionTroopSupplier = this._suppliers[(int)this._playerSide.GetOppositeSide()];
			IEnumerable<IAgentOriginBase> enumerable = missionTroopSupplier.SupplyTroops(missionTroopSupplier.NumTroopsNotSupplied);
			this._overriddenHideoutBossAgentOrigin = enumerable.FirstOrDefault<IAgentOriginBase>((IAgentOriginBase x) => x.Troop == this._overriddenHideoutBossCharacterObject);
			this._allEnemyTroops = enumerable.Where<IAgentOriginBase>(delegate(IAgentOriginBase x)
			{
				CharacterObject characterObject;
				return !x.Troop.IsHero && (characterObject = x.Troop as CharacterObject) != null && characterObject.Culture.BanditBoss != characterObject && characterObject != this._overriddenHideoutBossCharacterObject;
			}).ToList<IAgentOriginBase>();
			this._playerPriorTroops = this._suppliers[(int)this._playerSide].SupplyTroops(this._playerTroopCount).ToList<IAgentOriginBase>();
			this._allEnemyTroopTypesCache = this._allEnemyTroops.DistinctBy<IAgentOriginBase, BasicCharacterObject>((IAgentOriginBase x) => x.Troop).ToList<IAgentOriginBase>();
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00028748 File Offset: 0x00026948
		private MBList<Agent> SpawnReinforcementAllyTroops(StealthAreaMissionLogic.StealthAreaData triggeredStealthAreaData, StealthAreaMarker stealthAreaMarker)
		{
			int count = triggeredStealthAreaData.StealthAreaMarkers.Count;
			StealthAreaMarker[] array = triggeredStealthAreaData.StealthAreaMarkers.Keys.ToArray<StealthAreaMarker>();
			MBList<Agent> mblist = new MBList<Agent>();
			for (int i = 0; i < this._playerPriorTroops.Count; i++)
			{
				if (array[i % count] == stealthAreaMarker)
				{
					IAgentOriginBase agentOriginBase = this._playerPriorTroops[i];
					Agent agent = this.SpawnAllyAgent(agentOriginBase, stealthAreaMarker.ReinforcementAllyGroupSpawnPoint, stealthAreaMarker.WaitPoint.GlobalPosition);
					mblist.Add(agent);
				}
			}
			return mblist;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x000287C8 File Offset: 0x000269C8
		private void SpawnBossAndBodyguards()
		{
			MatrixFrame identity = MatrixFrame.Identity;
			identity.origin = Agent.Main.Position + Agent.Main.LookDirection * -3f;
			int num = (int)MathF.Clamp((float)(this._initialHideoutPopulation / 2), 4f, 20f);
			this.SpawnRemainingTroopsForBossFight(new List<MatrixFrame> { identity }, num);
			this._bossAgent = this.SelectBossAgent();
			this._bossAgent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
			foreach (Agent agent in base.Mission.PlayerEnemyTeam.ActiveAgents)
			{
				if (agent != this._bossAgent)
				{
					agent.WieldInitialWeapons(Agent.WeaponWieldActionType.WithAnimationUninterruptible, Equipment.InitialWeaponEquipPreference.Any);
				}
			}
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x000288A8 File Offset: 0x00026AA8
		private Agent SelectBossAgent()
		{
			Agent agent = null;
			Agent agent2 = null;
			foreach (Agent agent3 in base.Mission.Agents)
			{
				if (agent3.IsHuman && !agent3.Team.IsPlayerAlly)
				{
					if (this._overriddenHideoutBossCharacterObject == null)
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
					}
					else if (agent3.Character == this._overriddenHideoutBossCharacterObject)
					{
						agent = agent3;
						agent2 = agent3;
						break;
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

		// Token: 0x06000605 RID: 1541 RVA: 0x000289C4 File Offset: 0x00026BC4
		private void OnInitialFadeOutOver(ref Agent playerAgent, ref List<Agent> playerCompanions, ref Agent bossAgent, ref List<Agent> bossCompanions, ref float placementPerturbation, ref float placementAngle)
		{
			this._currentHideoutMissionState = HideoutAmbushMissionController.HideoutMissionState.CutSceneBeforeBossFight;
			this._enemyTeam = base.Mission.PlayerEnemyTeam;
			this.SpawnBossAndBodyguards();
			base.Mission.PlayerTeam.SetIsEnemyOf(this._enemyTeam, false);
			if (Agent.Main.IsUsingGameObject)
			{
				Agent.Main.StopUsingGameObject(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
			}
			playerAgent = Agent.Main;
			playerCompanions = base.Mission.Agents.Where<Agent>((Agent x) => x.IsActive() && x.Team == base.Mission.PlayerTeam && x.IsHuman && x.IsAIControlled).ToList<Agent>();
			bossAgent = this._bossAgent;
			bossCompanions = base.Mission.Agents.Where<Agent>((Agent x) => x.IsActive() && x.Team == this._enemyTeam && x.IsHuman && x.IsAIControlled && x != this._bossAgent).ToList<Agent>();
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00028A74 File Offset: 0x00026C74
		private void OnCutSceneOver()
		{
			Mission.Current.SetMissionMode(MissionMode.Battle, false);
			this._currentHideoutMissionState = HideoutAmbushMissionController.HideoutMissionState.ConversationBetweenLeaders;
			MissionConversationLogic missionBehavior = base.Mission.GetMissionBehavior<MissionConversationLogic>();
			missionBehavior.DisableStartConversation(false);
			missionBehavior.StartConversation(this._bossAgent, false, false);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00028AA8 File Offset: 0x00026CA8
		private void OnDuelOver(BattleSideEnum winnerSide)
		{
			if (winnerSide == base.Mission.PlayerTeam.Side && this._duelPhaseAllyAgents != null)
			{
				using (List<Agent>.Enumerator enumerator = this._duelPhaseAllyAgents.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Agent agent = enumerator.Current;
						if (agent.State == AgentState.Active)
						{
							agent.SetTeam(base.Mission.PlayerTeam, true);
						}
					}
					return;
				}
			}
			if (winnerSide == base.Mission.PlayerEnemyTeam.Side && this._duelPhaseBanditAgents != null)
			{
				foreach (Agent agent2 in this._duelPhaseBanditAgents)
				{
					if (agent2.State == AgentState.Active)
					{
						agent2.SetTeam(this._enemyTeam, true);
						agent2.DisableScriptedMovement();
						agent2.ClearTargetFrame();
					}
				}
				foreach (Agent agent3 in this._duelPhaseAllyAgents)
				{
					if (agent3.State == AgentState.Active)
					{
						agent3.SetTeam(base.Mission.PlayerTeam, true);
						agent3.DisableScriptedMovement();
						agent3.ClearTargetFrame();
					}
				}
				foreach (Agent agent4 in base.Mission.PlayerEnemyTeam.ActiveAgents)
				{
					agent4.SetAlarmState(Agent.AIStateFlag.Alarmed);
				}
			}
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00028C58 File Offset: 0x00026E58
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
			this._bossAgent.SetAlarmState(Agent.AIStateFlag.Alarmed);
			this._currentHideoutMissionState = HideoutAmbushMissionController.HideoutMissionState.BossFightWithDuel;
			this._defeatHideoutBossObjective = new DefeatHideoutBossObjective(base.Mission, true);
			this._missionObjectiveLogic.StartObjective(this._defeatHideoutBossObjective);
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00028DDC File Offset: 0x00026FDC
		private void StartBossFightBattleModeInternal()
		{
			base.Mission.GetMissionBehavior<MissionConversationLogic>().DisableStartConversation(true);
			base.Mission.PlayerTeam.SetIsEnemyOf(this._enemyTeam, true);
			this._currentHideoutMissionState = HideoutAmbushMissionController.HideoutMissionState.BossFightWithAll;
			foreach (Agent agent in base.Mission.PlayerEnemyTeam.ActiveAgents)
			{
				agent.SetAlarmState(Agent.AIStateFlag.Alarmed);
			}
			base.Mission.PlayerTeam.PlayerOrderController.SelectAllFormations(false);
			base.Mission.PlayerTeam.PlayerOrderController.SetOrder(OrderType.Charge);
			base.Mission.PlayerEnemyTeam.MasterOrderController.SelectAllFormations(false);
			base.Mission.PlayerEnemyTeam.MasterOrderController.SetOrder(OrderType.Charge);
			this._defeatHideoutBossObjective = new DefeatHideoutBossObjective(base.Mission, false);
			this._missionObjectiveLogic.StartObjective(this._defeatHideoutBossObjective);
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00028EE4 File Offset: 0x000270E4
		private void KillAllSentries()
		{
			List<Agent> list = new List<Agent>();
			foreach (StealthAreaMissionLogic.StealthAreaData stealthAreaData in this._stealthAreaData)
			{
				foreach (KeyValuePair<StealthAreaMarker, List<Agent>> keyValuePair in stealthAreaData.StealthAreaMarkers)
				{
					list.AddRange(keyValuePair.Value);
				}
			}
			foreach (Agent agent in list)
			{
				base.Mission.KillAgentCheat(agent);
			}
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00028FC4 File Offset: 0x000271C4
		private FormationClass GetHideoutAmbushMissionTroopClass(BattleSideEnum battleSide, BasicCharacterObject agentCharacter)
		{
			return agentCharacter.GetFormationClass().DismountedClass();
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00028FD4 File Offset: 0x000271D4
		[CommandLineFunctionality.CommandLineArgumentFunction("kill_all_sentries", "mission")]
		public static string KillAllSentries(List<string> strings)
		{
			string empty = string.Empty;
			if (!CampaignCheats.CheckCheatUsage(ref empty))
			{
				return empty;
			}
			Mission mission = Mission.Current;
			HideoutAmbushMissionController hideoutAmbushMissionController = ((mission != null) ? mission.GetMissionBehavior<HideoutAmbushMissionController>() : null);
			if (hideoutAmbushMissionController != null)
			{
				hideoutAmbushMissionController.KillAllSentries();
				return "Done";
			}
			return "This cheat only works in hideout ambush mission!";
		}

		// Token: 0x0400030F RID: 783
		private const int FirstPhaseEndInSeconds = 4;

		// Token: 0x04000310 RID: 784
		private readonly int _initialHideoutPopulation;

		// Token: 0x04000311 RID: 785
		private bool _troopsInitialized;

		// Token: 0x04000312 RID: 786
		private bool _isMissionInitialized;

		// Token: 0x04000313 RID: 787
		private bool _battleResolved;

		// Token: 0x04000314 RID: 788
		private readonly BattleSideEnum _playerSide;

		// Token: 0x04000315 RID: 789
		private HideoutAmbushMissionController.HideoutMissionState _currentHideoutMissionState;

		// Token: 0x04000316 RID: 790
		private List<Agent> _duelPhaseAllyAgents;

		// Token: 0x04000317 RID: 791
		private List<Agent> _duelPhaseBanditAgents;

		// Token: 0x04000318 RID: 792
		private List<IAgentOriginBase> _allEnemyTroops;

		// Token: 0x04000319 RID: 793
		private List<IAgentOriginBase> _playerPriorTroops;

		// Token: 0x0400031A RID: 794
		private List<IAgentOriginBase> _allEnemyTroopTypesCache;

		// Token: 0x0400031B RID: 795
		private readonly List<StealthAreaMissionLogic.StealthAreaData> _stealthAreaData;

		// Token: 0x0400031C RID: 796
		private Timer _waitTimerToChangeStealthModeIntoBattle;

		// Token: 0x0400031D RID: 797
		private Timer _firstPhaseEndTimer;

		// Token: 0x0400031E RID: 798
		private int _sentryCount;

		// Token: 0x0400031F RID: 799
		private int _remainingSentryCount;

		// Token: 0x04000320 RID: 800
		private bool _isClearedAsGhost = true;

		// Token: 0x04000321 RID: 801
		private BattleAgentLogic _battleAgentLogic;

		// Token: 0x04000322 RID: 802
		private BattleEndLogic _battleEndLogic;

		// Token: 0x04000323 RID: 803
		private HideoutAmbushBossFightCinematicController _hideoutAmbushBossFightCinematicController;

		// Token: 0x04000324 RID: 804
		private StealthAreaMissionLogic _stealthAreaMissionLogic;

		// Token: 0x04000325 RID: 805
		private MissionObjectiveLogic _missionObjectiveLogic;

		// Token: 0x04000326 RID: 806
		private Agent _bossAgent;

		// Token: 0x04000327 RID: 807
		private Team _enemyTeam;

		// Token: 0x04000328 RID: 808
		private CharacterObject _overriddenHideoutBossCharacterObject;

		// Token: 0x04000329 RID: 809
		private IAgentOriginBase _overriddenHideoutBossAgentOrigin;

		// Token: 0x0400032A RID: 810
		private readonly int _playerTroopCount;

		// Token: 0x0400032B RID: 811
		private LocateTheMainCampObjective _locateTheMainCampObjective;

		// Token: 0x0400032C RID: 812
		private ClearTheMainCampObjective _clearTheMainCampObjective;

		// Token: 0x0400032D RID: 813
		private DefeatHideoutBossObjective _defeatHideoutBossObjective;

		// Token: 0x0400032E RID: 814
		private readonly List<Agent> _clearObjectiveTargetAgents = new List<Agent>();

		// Token: 0x0400032F RID: 815
		private readonly IMissionTroopSupplier[] _suppliers;

		// Token: 0x020001A2 RID: 418
		public class TroopData
		{
			// Token: 0x06000F3D RID: 3901 RVA: 0x00067FF5 File Offset: 0x000661F5
			public TroopData(CharacterObject troop, int number)
			{
				this.Troop = troop;
				this.Number = number;
			}

			// Token: 0x040007A9 RID: 1961
			public CharacterObject Troop;

			// Token: 0x040007AA RID: 1962
			public int Number;

			// Token: 0x040007AB RID: 1963
			public int Level;
		}

		// Token: 0x020001A3 RID: 419
		private enum HideoutMissionState
		{
			// Token: 0x040007AD RID: 1965
			NotDecided,
			// Token: 0x040007AE RID: 1966
			StealthState,
			// Token: 0x040007AF RID: 1967
			CallTroopsCutSceneState,
			// Token: 0x040007B0 RID: 1968
			BattleBeforeBossFight,
			// Token: 0x040007B1 RID: 1969
			CutSceneBeforeBossFight,
			// Token: 0x040007B2 RID: 1970
			ConversationBetweenLeaders,
			// Token: 0x040007B3 RID: 1971
			BossFightWithDuel,
			// Token: 0x040007B4 RID: 1972
			BossFightWithAll
		}
	}
}
