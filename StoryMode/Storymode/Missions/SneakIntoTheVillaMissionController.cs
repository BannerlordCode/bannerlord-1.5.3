using System;
using System.Collections.Generic;
using SandBox;
using SandBox.Missions;
using SandBox.Missions.AgentBehaviors;
using SandBox.Objects;
using SandBox.Objects.Usables;
using StoryMode.Quests.TutorialPhase;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace Storymode.Missions
{
	// Token: 0x02000004 RID: 4
	public class SneakIntoTheVillaMissionController : MissionLogic
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002058 File Offset: 0x00000258
		// (set) Token: 0x06000004 RID: 4 RVA: 0x0000205F File Offset: 0x0000025F
		public static SneakIntoTheVillaMissionController Instance { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002067 File Offset: 0x00000267
		// (set) Token: 0x06000006 RID: 6 RVA: 0x0000206F File Offset: 0x0000026F
		public SneakIntoTheVillaMissionController.MissionState State { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002078 File Offset: 0x00000278
		// (set) Token: 0x06000008 RID: 8 RVA: 0x00002080 File Offset: 0x00000280
		public Agent HeadmanAgent { get; private set; }

		// Token: 0x06000009 RID: 9 RVA: 0x00002089 File Offset: 0x00000289
		public override void OnMissionTick(float dt)
		{
			if (this._missionEndTimer != null && this._missionEndTimer.Check(false))
			{
				base.Mission.EndMission();
				this._missionEndTimer = null;
			}
			this.CheckTriggers();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000020BC File Offset: 0x000002BC
		public override void OnCreated()
		{
			SneakIntoTheVillaMissionController.Instance = this;
			base.Mission.DoesMissionRequireCivilianEquipment = true;
			this._talkToVillagersQuest = (VillagersInNeed)Campaign.Current.QuestManager.Quests.FirstOrDefaultQ<QuestBase>((QuestBase x) => x is VillagersInNeed);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000211C File Offset: 0x0000031C
		public override void AfterStart()
		{
			base.Mission.SetMissionMode(MissionMode.Stealth, true);
			base.Mission.IsInventoryAccessible = false;
			base.Mission.IsQuestScreenAccessible = true;
			SandBoxHelpers.MissionHelper.SpawnPlayer(false, true, false, false, "");
			MBEquipmentRoster @object = MBObjectManager.Instance.GetObject<MBEquipmentRoster>("stealth_tutorial_set_player");
			Agent.Main.UpdateSpawnEquipmentAndRefreshVisuals(@object.DefaultEquipment);
			this.SpawnStealthAgents();
			this.SpawnHeadman();
			this.InitializeVolumeBoxes();
			base.Mission.GetMissionBehavior<StealthFailCounterMissionLogic>().SetFailTexts(null, new TextObject("{=eJ3iAJ8U}You alerted the bandits. The camp erupts in confusion, but in the darkness you are able to slip away. You watch from a distance as the chaos and noise die down, and you sense that it won't be long before this ill-disciplined gang relaxes their guard, giving you another chance. When you are ready, you can return to Tevea and try again.", null));
			Game.Current.EventManager.RegisterEvent<OnStealthMissionCounterFailedEvent>(new Action<OnStealthMissionCounterFailedEvent>(this.OnCaughtInStealthZone));
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021C5 File Offset: 0x000003C5
		public override void OnAfterMissionLoadingFinished()
		{
			base.Mission.OnInitialSpawnCompleted(BattleSideEnum.None);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021D3 File Offset: 0x000003D3
		public static bool IsStealthTutorialReadyForActivation(SneakIntoTheVillaMissionController.MissionState missionState)
		{
			return Mission.Current != null && SneakIntoTheVillaMissionController.Instance != null && (missionState == SneakIntoTheVillaMissionController.MissionState.Start || missionState <= SneakIntoTheVillaMissionController.Instance.State);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000021FA File Offset: 0x000003FA
		public static bool IsStealthTutorialReadyForCompletion(SneakIntoTheVillaMissionController.MissionState missionState)
		{
			return SneakIntoTheVillaMissionController.Instance != null && missionState < SneakIntoTheVillaMissionController.MissionState.End && SneakIntoTheVillaMissionController.Instance.State > missionState;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002217 File Offset: 0x00000417
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
			SneakIntoTheVillaMissionController.Instance = null;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002225 File Offset: 0x00000425
		protected override void OnEndMission()
		{
			Game.Current.EventManager.UnregisterEvent<OnStealthMissionCounterFailedEvent>(new Action<OnStealthMissionCounterFailedEvent>(this.OnCaughtInStealthZone));
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002242 File Offset: 0x00000442
		private void OnCaughtInStealthZone(OnStealthMissionCounterFailedEvent stealthFailedEvent)
		{
			this._talkToVillagersQuest.OnRescueMissionFailed();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000224F File Offset: 0x0000044F
		private void OnMainAgentIsWounded()
		{
			this._talkToVillagersQuest.OnRescueMissionFailed();
			this._missionEndTimer = new MissionTimer(2f);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000226C File Offset: 0x0000046C
		private void ShowMissionFailedPopup()
		{
			TextObject textObject = new TextObject("{=DM6luo3c}Continue", null);
			InformationManager.ShowInquiry(new InquiryData(new TextObject("{=wQbfWNZO}Mission Failed!", null).ToString(), new TextObject("{=45IBacqS}You are knocked to the ground, but in the confusion and darkness you are able to crawl away. You watch from a distance as the chaos and noise in the hideout die down, and you sense that it won't be long before this ill-disciplined gang relaxes their guard, giving you another chance. When you are ready, you can return to Tevea and try again.", null).ToString(), true, false, textObject.ToString(), null, delegate
			{
				this.OnMainAgentIsWounded();
			}, null, "", 0f, null, null, null), true, false);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000022D4 File Offset: 0x000004D4
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (affectedAgent.IsMainAgent)
			{
				this.ShowMissionFailedPopup();
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000022E4 File Offset: 0x000004E4
		public void OnAfterTalkingToPrisoner()
		{
			GameEntity gameEntity = base.Mission.Scene.FindEntityWithTag("doors_before_convo");
			GameEntity gameEntity2 = base.Mission.Scene.FindEntityWithTag("doors_after_convo");
			gameEntity.SetVisibilityExcludeParents(false);
			gameEntity2.SetVisibilityExcludeParents(true);
			List<GameEntity> list = new List<GameEntity>();
			base.Mission.Scene.GetAllEntitiesWithScriptComponent<Passage>(ref list);
			foreach (GameEntity gameEntity3 in list)
			{
				Passage firstScriptOfType = gameEntity3.GetFirstScriptOfType<Passage>();
				firstScriptOfType.SetEnabled(false);
				firstScriptOfType.PilotStandingPoint.IsDeactivated = false;
			}
			this.AreVisualsDirty = true;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002398 File Offset: 0x00000598
		public override void OnObjectUsed(Agent userAgent, UsableMissionObject usedObject)
		{
			PassageUsePoint passageUsePoint;
			if (userAgent.IsMainAgent && (passageUsePoint = usedObject as PassageUsePoint) != null && passageUsePoint.IsMissionExit)
			{
				this._talkToVillagersQuest.OnHeadmanRescued();
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000023CC File Offset: 0x000005CC
		private void SpawnHeadman()
		{
			GameEntity gameEntity = base.Mission.Scene.FindEntityWithTag("sp_captive");
			this.HeadmanAgent = this.SpawnAgent(this._talkToVillagersQuest.Headman, gameEntity, Team.Invalid);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000240C File Offset: 0x0000060C
		private void SpawnStealthAgents()
		{
			CharacterObject @object = MBObjectManager.Instance.GetObject<CharacterObject>("mountain_bandits_raider");
			List<GameEntity> list = new List<GameEntity>();
			base.Mission.Scene.GetAllEntitiesWithScriptComponent<DynamicPatrolAreaParent>(ref list);
			MBActionSet actionSet = MBGlobals.GetActionSet("as_human_hideout_bandit");
			foreach (GameEntity gameEntity in list)
			{
				foreach (GameEntity gameEntity2 in gameEntity.GetChildren())
				{
					PatrolPoint firstScriptOfType = gameEntity2.GetChild(0).GetFirstScriptOfType<PatrolPoint>();
					if (firstScriptOfType.SpawnGroupTag == "stealth_agent")
					{
						Agent agent = this.SpawnAgent(@object, gameEntity2, base.Mission.PlayerEnemyTeam);
						AgentNavigator agentNavigator = agent.GetComponent<CampaignAgentComponent>().CreateAgentNavigator();
						SandBoxManager.Instance.AgentBehaviorManager.AddStealthAgentBehaviors(agent);
						AnimationSystemData animationSystemData = agent.Monster.FillAnimationSystemData(actionSet, @object.GetStepSize(), false);
						agent.SetActionSet(ref animationSystemData);
						AgentFlag agentFlags = agent.GetAgentFlags();
						agent.SetAgentFlags(agentFlags | AgentFlag.CanGetAlarmed);
						agentNavigator.GetBehaviorGroup<DailyBehaviorGroup>().GetBehavior<PatrolAgentBehavior>().SetDynamicPatrolArea(gameEntity);
						if (firstScriptOfType.GameEntity.HasTag("sp_agent_distraction"))
						{
							this._distractionTargetAgent = agent;
						}
						if (firstScriptOfType.GameEntity.HasTag("sp_agent_stealth_kill"))
						{
							this._stealthKillTargetAgent = agent;
						}
					}
				}
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000025C8 File Offset: 0x000007C8
		private void CheckTriggers()
		{
			if (this.State < SneakIntoTheVillaMissionController.MissionState.End && Agent.Main != null)
			{
				for (SneakIntoTheVillaMissionController.MissionState missionState = this.State + 1; missionState < SneakIntoTheVillaMissionController.MissionState.End; missionState++)
				{
					if (this._volumeBoxes[missionState].IsPointIn(Agent.Main.Position))
					{
						this.State = missionState;
						return;
					}
				}
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000261F File Offset: 0x0000081F
		public bool IsTargetAgentDistracted()
		{
			return this.State == SneakIntoTheVillaMissionController.MissionState.Distraction && (this._distractionTargetAgent == null || this._distractionTargetAgent.IsCautious() || this._distractionTargetAgent.IsAlarmed());
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000264E File Offset: 0x0000084E
		public bool IsTargetAgentKilled()
		{
			return this.State == SneakIntoTheVillaMissionController.MissionState.StealthKill && (this._isStealthAttackComplete || this._stealthKillTargetAgent == null);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002670 File Offset: 0x00000870
		public bool IsMainAgentDraggingTargetBody()
		{
			return this._stealthKillTargetAgent != null && this._stealthKillTargetAgent.IsAddedAsCorpse() && Agent.Main != null && Agent.Main.IsActive() && (Agent.Main.GetScriptedFlags() & Agent.AIScriptedFrameFlags.Drag) == Agent.AIScriptedFrameFlags.Drag;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000026BE File Offset: 0x000008BE
		public override void OnMeleeHit(Agent attacker, Agent victim, bool isCanceled, AttackCollisionData collisionData)
		{
			if (!this._isStealthAttackComplete && this._stealthKillTargetAgent != null && collisionData.IsSneakAttack && victim == this._stealthKillTargetAgent)
			{
				this._isStealthAttackComplete = true;
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000026EC File Offset: 0x000008EC
		private void InitializeVolumeBoxes()
		{
			this._volumeBoxes = new Dictionary<SneakIntoTheVillaMissionController.MissionState, VolumeBox>();
			this._volumeBoxes[SneakIntoTheVillaMissionController.MissionState.Crouch] = base.Mission.Scene.FindEntityWithTag("trigger_volume_crouch").GetFirstScriptOfType<VolumeBox>();
			this._volumeBoxes[SneakIntoTheVillaMissionController.MissionState.WalkSlow] = base.Mission.Scene.FindEntityWithTag("trigger_volume_walk_slowly").GetFirstScriptOfType<VolumeBox>();
			this._volumeBoxes[SneakIntoTheVillaMissionController.MissionState.HideInBushes] = base.Mission.Scene.FindEntityWithTag("trigger_volume_stealthbox").GetFirstScriptOfType<VolumeBox>();
			this._volumeBoxes[SneakIntoTheVillaMissionController.MissionState.HideInBushesEnd] = base.Mission.Scene.FindEntityWithTag("end_trigger_stealthbox").GetFirstScriptOfType<VolumeBox>();
			this._volumeBoxes[SneakIntoTheVillaMissionController.MissionState.Distraction] = base.Mission.Scene.FindEntityWithTag("trigger_volume_distraction").GetFirstScriptOfType<VolumeBox>();
			this._volumeBoxes[SneakIntoTheVillaMissionController.MissionState.DarkZone] = base.Mission.Scene.FindEntityWithTag("trigger_volume_dark_zone").GetFirstScriptOfType<VolumeBox>();
			this._volumeBoxes[SneakIntoTheVillaMissionController.MissionState.DarkZoneEnd] = base.Mission.Scene.FindEntityWithTag("end_trigger_darkness").GetFirstScriptOfType<VolumeBox>();
			this._volumeBoxes[SneakIntoTheVillaMissionController.MissionState.StealthKill] = base.Mission.Scene.FindEntityWithTag("trigger_volume_stealth_kill").GetFirstScriptOfType<VolumeBox>();
			this._volumeBoxes[SneakIntoTheVillaMissionController.MissionState.HideCorpse] = base.Mission.Scene.FindEntityWithTag("trigger_volume_hide_corpse").GetFirstScriptOfType<VolumeBox>();
			this._volumeBoxes[SneakIntoTheVillaMissionController.MissionState.End] = base.Mission.Scene.FindEntityWithTag("trigger_volume_passage").GetFirstScriptOfType<VolumeBox>();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002884 File Offset: 0x00000A84
		private Agent SpawnAgent(CharacterObject character, GameEntity spawnPoint, Team team)
		{
			MatrixFrame globalFrame = spawnPoint.GetGlobalFrame();
			AgentBuildData agentBuildData = new AgentBuildData(character).NoHorses(true).InitialPosition(in globalFrame.origin);
			Vec2 vec = globalFrame.rotation.f.AsVec2;
			vec = vec.Normalized();
			AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in vec).CivilianEquipment(true).Team(team)
				.TroopOrigin(new SimpleAgentOrigin(character, -1, null, default(UniqueTroopDescriptor)));
			return Mission.Current.SpawnAgent(agentBuildData2, false, null, null);
		}

		// Token: 0x04000002 RID: 2
		private Dictionary<SneakIntoTheVillaMissionController.MissionState, VolumeBox> _volumeBoxes = new Dictionary<SneakIntoTheVillaMissionController.MissionState, VolumeBox>();

		// Token: 0x04000003 RID: 3
		private const string FirstDoorId = "doors_before_convo";

		// Token: 0x04000004 RID: 4
		private const string SecondDoorId = "doors_after_convo";

		// Token: 0x04000005 RID: 5
		private const string DistractionAgentSpawnPointId = "sp_agent_distraction";

		// Token: 0x04000006 RID: 6
		private const string StealthKillAgentSpawnPointId = "sp_agent_stealth_kill";

		// Token: 0x04000007 RID: 7
		private const string HeadmanSpawnPoint = "sp_captive";

		// Token: 0x04000008 RID: 8
		private VillagersInNeed _talkToVillagersQuest;

		// Token: 0x04000009 RID: 9
		private MissionTimer _missionEndTimer;

		// Token: 0x0400000A RID: 10
		private Agent _distractionTargetAgent;

		// Token: 0x0400000B RID: 11
		private Agent _stealthKillTargetAgent;

		// Token: 0x0400000C RID: 12
		private bool _isStealthAttackComplete;

		// Token: 0x0400000D RID: 13
		public bool AreVisualsDirty;

		// Token: 0x0200005A RID: 90
		public enum MissionState
		{
			// Token: 0x040001EA RID: 490
			Start,
			// Token: 0x040001EB RID: 491
			Crouch,
			// Token: 0x040001EC RID: 492
			WalkSlow,
			// Token: 0x040001ED RID: 493
			HideInBushes,
			// Token: 0x040001EE RID: 494
			HideInBushesEnd,
			// Token: 0x040001EF RID: 495
			Distraction,
			// Token: 0x040001F0 RID: 496
			DarkZone,
			// Token: 0x040001F1 RID: 497
			DarkZoneEnd,
			// Token: 0x040001F2 RID: 498
			StealthKill,
			// Token: 0x040001F3 RID: 499
			HideCorpse,
			// Token: 0x040001F4 RID: 500
			End
		}
	}
}
