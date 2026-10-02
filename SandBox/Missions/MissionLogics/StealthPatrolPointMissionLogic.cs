using System;
using System.Collections.Generic;
using SandBox.CampaignBehaviors;
using SandBox.Missions.AgentBehaviors;
using SandBox.Objects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Objects;
using TaleWorlds.ObjectSystem;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000089 RID: 137
	public class StealthPatrolPointMissionLogic : MissionLogic, IMissionAgentSpawnLogic, IMissionBehavior
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x000237C8 File Offset: 0x000219C8
		public BattleSideEnum PlayerSide
		{
			get
			{
				return BattleSideEnum.None;
			}
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x000237CC File Offset: 0x000219CC
		public StealthPatrolPointMissionLogic()
		{
			this._spawnedEnemyAgentsOnPatrolPoints = new Dictionary<Agent, GameEntity>();
			this._coverAnimalPatrolPoints = new Dictionary<PatrolPoint, Agent>();
			Game.Current.EventManager.RegisterEvent<CheckpointLoadedMissionEvent>(new Action<CheckpointLoadedMissionEvent>(this.OnCheckpointLoadedEvent));
			Game.Current.EventManager.RegisterEvent<LocationCharacterAgentSpawnedMissionEvent>(new Action<LocationCharacterAgentSpawnedMissionEvent>(this.OnLocationCharacterAgentSpawned));
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0002382B File Offset: 0x00021A2B
		protected override void OnEndMission()
		{
			Game.Current.EventManager.UnregisterEvent<CheckpointLoadedMissionEvent>(new Action<CheckpointLoadedMissionEvent>(this.OnCheckpointLoadedEvent));
			Game.Current.EventManager.UnregisterEvent<LocationCharacterAgentSpawnedMissionEvent>(new Action<LocationCharacterAgentSpawnedMissionEvent>(this.OnLocationCharacterAgentSpawned));
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00023864 File Offset: 0x00021A64
		public override void AfterStart()
		{
			base.AfterStart();
			this._checkpointMissionLogic = Mission.Current.GetMissionBehavior<CheckpointMissionLogic>();
			List<GameEntity> list = new List<GameEntity>();
			base.Mission.Scene.GetAllEntitiesWithScriptComponent<DynamicPatrolAreaParent>(ref list);
			this.SpawnCoverAnimals(list);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x000238A8 File Offset: 0x00021AA8
		public void OnLocationCharacterAgentSpawned(LocationCharacterAgentSpawnedMissionEvent locationCharacterAgentSpawnedEvent)
		{
			if (Campaign.Current.GetCampaignBehavior<StealthCharactersCampaignBehavior>() != null)
			{
				LocationCharacter locationCharacter = locationCharacterAgentSpawnedEvent.LocationCharacter;
				Agent agent = locationCharacterAgentSpawnedEvent.Agent;
				GameEntity gameEntity = GameEntity.CreateFromWeakEntity(locationCharacterAgentSpawnedEvent.SpawnedOnGameEntity);
				if (locationCharacter.SpecialTargetTag == "stealth_agent" || locationCharacter.SpecialTargetTag == "stealth_agent_forced" || locationCharacter.SpecialTargetTag == "disguise_default_agent" || locationCharacter.SpecialTargetTag == "disguise_officer_agent" || locationCharacter.SpecialTargetTag == "disguise_shadow_agent" || locationCharacter.SpecialTargetTag == "prison_break_reinforcement_point")
				{
					foreach (string text in gameEntity.GetChild(0).Tags)
					{
						if (!string.IsNullOrEmpty(text))
						{
							agent.AgentVisuals.GetEntity().AddTag(text);
						}
					}
					agent.SetAgentFlags(agent.GetAgentFlags() | AgentFlag.CanGetAlarmed);
					agent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>().GetBehavior<PatrolAgentBehavior>().SetDynamicPatrolArea(gameEntity.Parent);
					this._spawnedEnemyAgentsOnPatrolPoints.Add(agent, gameEntity);
					CheckpointMissionLogic checkpointMissionLogic = this._checkpointMissionLogic;
					if (checkpointMissionLogic == null)
					{
						return;
					}
					checkpointMissionLogic.RegisterAgent(agent);
				}
			}
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x000239E0 File Offset: 0x00021BE0
		public override void OnAgentInteraction(Agent userAgent, Agent agent, sbyte agentBoneIndex)
		{
			base.OnAgentInteraction(userAgent, agent, agentBoneIndex);
			if (userAgent == Agent.Main)
			{
				foreach (KeyValuePair<PatrolPoint, Agent> keyValuePair in this._coverAnimalPatrolPoints)
				{
					if (keyValuePair.Value == agent)
					{
						agent.GetComponent<CoverAnimalAgentComponent>().StartMovement();
					}
				}
			}
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00023A54 File Offset: 0x00021C54
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (affectorAgent != null && affectorAgent.IsMainAgent)
			{
				this._spawnedEnemyAgentsOnPatrolPoints.Remove(affectedAgent);
			}
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00023A70 File Offset: 0x00021C70
		public override bool IsThereAgentAction(Agent userAgent, Agent otherAgent)
		{
			if (userAgent == Agent.Main)
			{
				foreach (KeyValuePair<PatrolPoint, Agent> keyValuePair in this._coverAnimalPatrolPoints)
				{
					if (keyValuePair.Value == otherAgent && !otherAgent.GetComponent<CoverAnimalAgentComponent>().IsMovementStarted)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00023AE4 File Offset: 0x00021CE4
		private void SpawnCoverAnimals(IEnumerable<GameEntity> dynamicPatrolAreas)
		{
			foreach (GameEntity gameEntity in dynamicPatrolAreas)
			{
				if (!gameEntity.GetFirstScriptOfType<DynamicPatrolAreaParent>().IsDisabled)
				{
					foreach (GameEntity gameEntity2 in gameEntity.GetChildren())
					{
						PatrolPoint firstScriptOfType = gameEntity2.GetChild(0).GetFirstScriptOfType<PatrolPoint>();
						if (firstScriptOfType != null && !firstScriptOfType.IsDisabled && !string.IsNullOrEmpty(firstScriptOfType.SpawnGroupTag) && firstScriptOfType.SpawnGroupTag == "cover_cow")
						{
							ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.SpawnGroupTag);
							if (@object == null)
							{
								break;
							}
							if (!this._coverAnimalPatrolPoints.ContainsKey(firstScriptOfType))
							{
								this._coverAnimalPatrolPoints.Add(firstScriptOfType, null);
							}
							MatrixFrame globalFrame = gameEntity2.GetGlobalFrame();
							ItemRosterElement itemRosterElement = new ItemRosterElement(@object, 0, null);
							globalFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
							Mission mission = Mission.Current;
							ItemRosterElement itemRosterElement2 = itemRosterElement;
							ItemRosterElement itemRosterElement3 = default(ItemRosterElement);
							Vec2 asVec = globalFrame.rotation.f.AsVec2;
							Agent agent = mission.SpawnMonster(itemRosterElement2, itemRosterElement3, in globalFrame.origin, in asVec, -1);
							agent.SetAgentExcludeStateForFaceGroupId(1, true);
							AnimalSpawnSettings.CheckAndSetAnimalAgentFlags(gameEntity2, agent);
							this.SimulateAnimalAnimations(agent);
							agent.AddComponent(new CoverAnimalAgentComponent(agent));
							agent.GetComponent<CoverAnimalAgentComponent>().SetDynamicPatrolArea(gameEntity);
							this._coverAnimalPatrolPoints[firstScriptOfType] = agent;
							if (agent.CurrentMortalityState == Agent.MortalityState.Mortal)
							{
								agent.ToggleInvulnerable();
							}
						}
					}
				}
			}
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00023CB8 File Offset: 0x00021EB8
		private void SimulateAnimalAnimations(Agent agent)
		{
			int num = 10 + MBRandom.RandomInt(90);
			for (int i = 0; i < num; i++)
			{
				agent.TickActionChannels(0.1f);
				agent.AgentVisuals.GetSkeleton().TickAnimations(0.1f, agent.AgentVisuals.GetGlobalFrame(), true);
			}
			Vec3 vec = agent.ComputeAnimationDisplacement(0.1f * (float)num);
			if (vec.LengthSquared > 0f)
			{
				agent.TeleportToPosition(agent.Position + vec);
			}
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00023D38 File Offset: 0x00021F38
		public void OnCheckpointLoadedEvent(CheckpointLoadedMissionEvent checkpointLoadedMissionEvent)
		{
			if (checkpointLoadedMissionEvent.LoadedCheckpointUniqueId >= 0)
			{
				string text = "sp_checkpoint_" + checkpointLoadedMissionEvent.LoadedCheckpointUniqueId;
				foreach (KeyValuePair<Agent, GameEntity> keyValuePair in this._spawnedEnemyAgentsOnPatrolPoints)
				{
					foreach (GameEntity gameEntity in keyValuePair.Value.GetChildren())
					{
						GameEntity firstChildEntityWithTag = gameEntity.GetFirstChildEntityWithTag(text);
						if (firstChildEntityWithTag != null)
						{
							keyValuePair.Key.TeleportToPosition(firstChildEntityWithTag.GlobalPosition);
							break;
						}
					}
				}
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00023E08 File Offset: 0x00022008
		public void StartSpawner(BattleSideEnum side)
		{
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00023E0A File Offset: 0x0002200A
		public void StopSpawner(BattleSideEnum side)
		{
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00023E0C File Offset: 0x0002200C
		public bool IsSideSpawnEnabled(BattleSideEnum side)
		{
			return true;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00023E0F File Offset: 0x0002200F
		public bool IsSideDepleted(BattleSideEnum side)
		{
			if (side == BattleSideEnum.Defender)
			{
				return this._spawnedEnemyAgentsOnPatrolPoints.Count <= 0;
			}
			if (side == BattleSideEnum.Attacker)
			{
				Agent main = Agent.Main;
				return main != null && !main.IsActive();
			}
			return false;
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00023E3F File Offset: 0x0002203F
		public float GetReinforcementInterval(BattleSideEnum battleSide = BattleSideEnum.None)
		{
			return 0f;
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00023E46 File Offset: 0x00022046
		public IEnumerable<IAgentOriginBase> GetAllTroopsForSide(BattleSideEnum side)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00023E4D File Offset: 0x0002204D
		public int GetNumberOfPlayerControllableTroops()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00023E54 File Offset: 0x00022054
		public bool GetSpawnHorses(BattleSideEnum side)
		{
			throw new NotImplementedException();
		}

		// Token: 0x040002CC RID: 716
		private const string CoverCowId = "cover_cow";

		// Token: 0x040002CD RID: 717
		private readonly Dictionary<Agent, GameEntity> _spawnedEnemyAgentsOnPatrolPoints;

		// Token: 0x040002CE RID: 718
		private readonly Dictionary<PatrolPoint, Agent> _coverAnimalPatrolPoints;

		// Token: 0x040002CF RID: 719
		private CheckpointMissionLogic _checkpointMissionLogic;
	}
}
