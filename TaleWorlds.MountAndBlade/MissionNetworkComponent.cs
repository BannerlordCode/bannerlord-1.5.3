using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002B3 RID: 691
	public sealed class MissionNetworkComponent : MissionNetwork
	{
		// Token: 0x1400004D RID: 77
		// (add) Token: 0x06002656 RID: 9814 RVA: 0x0008BBE0 File Offset: 0x00089DE0
		// (remove) Token: 0x06002657 RID: 9815 RVA: 0x0008BC18 File Offset: 0x00089E18
		public event Action OnMyClientSynchronized;

		// Token: 0x1400004E RID: 78
		// (add) Token: 0x06002658 RID: 9816 RVA: 0x0008BC50 File Offset: 0x00089E50
		// (remove) Token: 0x06002659 RID: 9817 RVA: 0x0008BC88 File Offset: 0x00089E88
		public event Action<NetworkCommunicator> OnClientSynchronizedEvent;

		// Token: 0x0600265A RID: 9818 RVA: 0x0008BCC0 File Offset: 0x00089EC0
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsClientOrReplay)
			{
				registerer.RegisterBaseHandler<CreateFreeMountAgent>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventCreateFreeMountAgentEvent));
				registerer.RegisterBaseHandler<CreateAgent>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventCreateAgent));
				registerer.RegisterBaseHandler<SynchronizeAgentSpawnEquipment>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSynchronizeAgentEquipment));
				registerer.RegisterBaseHandler<CreateAgentVisuals>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventCreateAgentVisuals));
				registerer.RegisterBaseHandler<RemoveAgentVisualsForPeer>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventRemoveAgentVisualsForPeer));
				registerer.RegisterBaseHandler<RemoveAgentVisualsFromIndexForPeer>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventRemoveAgentVisualsFromIndexForPeer));
				registerer.RegisterBaseHandler<ReplaceBotWithPlayer>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventReplaceBotWithPlayer));
				registerer.RegisterBaseHandler<SetWieldedItemIndex>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetWieldedItemIndex));
				registerer.RegisterBaseHandler<SetWeaponNetworkData>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetWeaponNetworkData));
				registerer.RegisterBaseHandler<SetWeaponAmmoData>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetWeaponAmmoData));
				registerer.RegisterBaseHandler<SetWeaponReloadPhase>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetWeaponReloadPhase));
				registerer.RegisterBaseHandler<WeaponUsageIndexChangeMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventWeaponUsageIndexChangeMessage));
				registerer.RegisterBaseHandler<StartSwitchingWeaponUsageIndex>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventStartSwitchingWeaponUsageIndex));
				registerer.RegisterBaseHandler<InitializeFormation>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventInitializeFormation));
				registerer.RegisterBaseHandler<SetSpawnedFormationCount>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetSpawnedFormationCount));
				registerer.RegisterBaseHandler<AddTeam>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventAddTeam));
				registerer.RegisterBaseHandler<TeamSetIsEnemyOf>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventTeamSetIsEnemyOf));
				registerer.RegisterBaseHandler<AssignFormationToPlayer>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventAssignFormationToPlayer));
				registerer.RegisterBaseHandler<ExistingObjectsBegin>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventExistingObjectsBegin));
				registerer.RegisterBaseHandler<ExistingObjectsEnd>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventExistingObjectsEnd));
				registerer.RegisterBaseHandler<ClearMission>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventClearMission));
				registerer.RegisterBaseHandler<CreateMissionObject>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventCreateMissionObject));
				registerer.RegisterBaseHandler<RemoveMissionObject>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventRemoveMissionObject));
				registerer.RegisterBaseHandler<StopPhysicsAndSetFrameOfMissionObject>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventStopPhysicsAndSetFrameOfMissionObject));
				registerer.RegisterBaseHandler<BurstMissionObjectParticles>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventBurstMissionObjectParticles));
				registerer.RegisterBaseHandler<SetMissionObjectVisibility>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetMissionObjectVisibility));
				registerer.RegisterBaseHandler<SetMissionObjectDisabled>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetMissionObjectDisabled));
				registerer.RegisterBaseHandler<SetMissionObjectColors>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetMissionObjectColors));
				registerer.RegisterBaseHandler<SetMissionObjectFrame>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetMissionObjectFrame));
				registerer.RegisterBaseHandler<SetMissionObjectGlobalFrame>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetMissionObjectGlobalFrame));
				registerer.RegisterBaseHandler<SetMissionObjectFrameOverTime>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetMissionObjectFrameOverTime));
				registerer.RegisterBaseHandler<SetMissionObjectGlobalFrameOverTime>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetMissionObjectGlobalFrameOverTime));
				registerer.RegisterBaseHandler<SetMissionObjectAnimationAtChannel>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetMissionObjectAnimationAtChannel));
				registerer.RegisterBaseHandler<SetMissionObjectAnimationChannelParameter>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetMissionObjectAnimationChannelParameter));
				registerer.RegisterBaseHandler<SetMissionObjectAnimationChannelSpeed>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetMissionObjectAnimationChannelSpeed));
				registerer.RegisterBaseHandler<SetMissionObjectAnimationPaused>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetMissionObjectAnimationPaused));
				registerer.RegisterBaseHandler<SetMissionObjectVertexAnimation>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetMissionObjectVertexAnimation));
				registerer.RegisterBaseHandler<SetMissionObjectVertexAnimationProgress>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetMissionObjectVertexAnimationProgress));
				registerer.RegisterBaseHandler<SetMissionObjectImpulse>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetMissionObjectImpulse));
				registerer.RegisterBaseHandler<AddMissionObjectBodyFlags>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventAddMissionObjectBodyFlags));
				registerer.RegisterBaseHandler<RemoveMissionObjectBodyFlags>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventRemoveMissionObjectBodyFlags));
				registerer.RegisterBaseHandler<SetMachineTargetRotation>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetMachineTargetRotation));
				registerer.RegisterBaseHandler<SetUsableMissionObjectIsDeactivated>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetUsableGameObjectIsDeactivated));
				registerer.RegisterBaseHandler<SetUsableMissionObjectIsDisabledForPlayers>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetUsableGameObjectIsDisabledForPlayers));
				registerer.RegisterBaseHandler<SetRangedSiegeWeaponState>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetRangedSiegeWeaponState));
				registerer.RegisterBaseHandler<SetRangedSiegeWeaponAmmo>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetRangedSiegeWeaponAmmo));
				registerer.RegisterBaseHandler<RangedSiegeWeaponChangeProjectile>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventRangedSiegeWeaponChangeProjectile));
				registerer.RegisterBaseHandler<SetStonePileAmmo>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetStonePileAmmo));
				registerer.RegisterBaseHandler<SetSiegeMachineMovementDistance>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetSiegeMachineMovementDistance));
				registerer.RegisterBaseHandler<SetSiegeLadderState>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetSiegeLadderState));
				registerer.RegisterBaseHandler<SetAgentTargetPositionAndDirection>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetAgentTargetPositionAndDirection));
				registerer.RegisterBaseHandler<SetAgentTargetPosition>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetAgentTargetPosition));
				registerer.RegisterBaseHandler<ClearAgentTargetFrame>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventClearAgentTargetFrame));
				registerer.RegisterBaseHandler<AgentTeleportToFrame>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventAgentTeleportToFrame));
				registerer.RegisterBaseHandler<SetSiegeTowerGateState>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetSiegeTowerGateState));
				registerer.RegisterBaseHandler<SetSiegeTowerHasArrivedAtTarget>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetSiegeTowerHasArrivedAtTarget));
				registerer.RegisterBaseHandler<SetBatteringRamHasArrivedAtTarget>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetBatteringRamHasArrivedAtTarget));
				registerer.RegisterBaseHandler<SetPeerTeam>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetPeerTeam));
				registerer.RegisterBaseHandler<SynchronizeMissionTimeTracker>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSyncMissionTimer));
				registerer.RegisterBaseHandler<SetAgentPeer>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetAgentPeer));
				registerer.RegisterBaseHandler<SetAgentIsPlayer>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetAgentIsPlayer));
				registerer.RegisterBaseHandler<SetAgentHealth>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetAgentHealth));
				registerer.RegisterBaseHandler<AgentSetTeam>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventAgentSetTeam));
				registerer.RegisterBaseHandler<SetAgentActionSet>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetAgentActionSet));
				registerer.RegisterBaseHandler<MakeAgentDead>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventMakeAgentDead));
				registerer.RegisterBaseHandler<AgentSetFormation>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventAgentSetFormation));
				registerer.RegisterBaseHandler<AddPrefabComponentToAgentBone>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventAddPrefabComponentToAgentBone));
				registerer.RegisterBaseHandler<SetAgentPrefabComponentVisibility>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetAgentPrefabComponentVisibility));
				registerer.RegisterBaseHandler<UseObject>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventUseObject));
				registerer.RegisterBaseHandler<StopUsingObject>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventStopUsingObject));
				registerer.RegisterBaseHandler<SyncObjectHitpoints>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventHitSynchronizeObjectHitpoints));
				registerer.RegisterBaseHandler<SyncObjectDestructionLevel>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventHitSynchronizeObjectDestructionLevel));
				registerer.RegisterBaseHandler<BurstAllHeavyHitParticles>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventHitBurstAllHeavyHitParticles));
				registerer.RegisterBaseHandler<SynchronizeMissionObject>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSynchronizeMissionObject));
				registerer.RegisterBaseHandler<SpawnWeaponWithNewEntity>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSpawnWeaponWithNewEntity));
				registerer.RegisterBaseHandler<AttachWeaponToSpawnedWeapon>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventAttachWeaponToSpawnedWeapon));
				registerer.RegisterBaseHandler<AttachWeaponToAgent>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventAttachWeaponToAgent));
				registerer.RegisterBaseHandler<SpawnWeaponAsDropFromAgent>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSpawnWeaponAsDropFromAgent));
				registerer.RegisterBaseHandler<SpawnAttachedWeaponOnSpawnedWeapon>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSpawnAttachedWeaponOnSpawnedWeapon));
				registerer.RegisterBaseHandler<SpawnAttachedWeaponOnCorpse>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSpawnAttachedWeaponOnCorpse));
				registerer.RegisterBaseHandler<HandleMissileCollisionReaction>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventHandleMissileCollisionReaction));
				registerer.RegisterBaseHandler<RemoveEquippedWeapon>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventRemoveEquippedWeapon));
				registerer.RegisterBaseHandler<BarkAgent>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventBarkAgent));
				registerer.RegisterBaseHandler<EquipWeaponWithNewEntity>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventEquipWeaponWithNewEntity));
				registerer.RegisterBaseHandler<AttachWeaponToWeaponInAgentEquipmentSlot>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventAttachWeaponToWeaponInAgentEquipmentSlot));
				registerer.RegisterBaseHandler<EquipWeaponFromSpawnedItemEntity>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventEquipWeaponFromSpawnedItemEntity));
				registerer.RegisterBaseHandler<CreateMissile>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventCreateMissile));
				registerer.RegisterBaseHandler<CombatLogNetworkMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventAgentHit));
				registerer.RegisterBaseHandler<ConsumeWeaponAmount>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventConsumeWeaponAmount));
				registerer.RegisterBaseHandler<SetAgentOwningMissionPeer>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSetAgentOwningMissionPeer));
				return;
			}
			if (GameNetwork.IsServer)
			{
				registerer.RegisterBaseHandler<SetFollowedAgent>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventSetFollowedAgent));
				registerer.RegisterBaseHandler<SetMachineRotation>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventSetMachineRotation));
				registerer.RegisterBaseHandler<RequestUseObject>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventRequestUseObject));
				registerer.RegisterBaseHandler<RequestStopUsingObject>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventRequestStopUsingObject));
				registerer.RegisterBaseHandler<ApplyOrder>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventApplyOrder));
				registerer.RegisterBaseHandler<ApplySiegeWeaponOrder>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventApplySiegeWeaponOrder));
				registerer.RegisterBaseHandler<ApplyOrderWithPosition>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventApplyOrderWithPosition));
				registerer.RegisterBaseHandler<ApplyOrderWithFormation>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventApplyOrderWithFormation));
				registerer.RegisterBaseHandler<ApplyOrderWithFormationAndPercentage>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventApplyOrderWithFormationAndPercentage));
				registerer.RegisterBaseHandler<ApplyOrderWithFormationAndNumber>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventApplyOrderWithFormationAndNumber));
				registerer.RegisterBaseHandler<ApplyOrderWithTwoPositions>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventApplyOrderWithTwoPositions));
				registerer.RegisterBaseHandler<ApplyOrderWithMissionObject>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventApplyOrderWithGameEntity));
				registerer.RegisterBaseHandler<ApplyOrderWithAgent>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventApplyOrderWithAgent));
				registerer.RegisterBaseHandler<SelectAllFormations>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventSelectAllFormations));
				registerer.RegisterBaseHandler<SelectAllSiegeWeapons>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventSelectAllSiegeWeapons));
				registerer.RegisterBaseHandler<ClearSelectedFormations>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventClearSelectedFormations));
				registerer.RegisterBaseHandler<SelectFormation>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventSelectFormation));
				registerer.RegisterBaseHandler<SelectSiegeWeapon>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventSelectSiegeWeapon));
				registerer.RegisterBaseHandler<UnselectFormation>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventUnselectFormation));
				registerer.RegisterBaseHandler<UnselectSiegeWeapon>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventUnselectSiegeWeapon));
				registerer.RegisterBaseHandler<DropWeapon>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventDropWeapon));
				registerer.RegisterBaseHandler<TauntSelected>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventCheerSelected));
				registerer.RegisterBaseHandler<BarkSelected>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventBarkSelected));
				registerer.RegisterBaseHandler<AgentVisualsBreakInvulnerability>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventBreakAgentVisualsInvulnerability));
				registerer.RegisterBaseHandler<RequestToSpawnAsBot>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventRequestToSpawnAsBot));
			}
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x0008C4F8 File Offset: 0x0008A6F8
		private Team GetTeamOfPeer(NetworkCommunicator networkPeer)
		{
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			if (component.ControlledAgent == null)
			{
				MBDebug.Print("peer.ControlledAgent == null", 0, Debug.DebugColor.White, 17592186044416UL);
				return null;
			}
			Team team = component.ControlledAgent.Team;
			if (team == null)
			{
				MBDebug.Print("peersTeam == null", 0, Debug.DebugColor.White, 17592186044416UL);
			}
			return team;
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x0008C550 File Offset: 0x0008A750
		private OrderController GetOrderControllerOfPeer(NetworkCommunicator networkPeer)
		{
			Team teamOfPeer = this.GetTeamOfPeer(networkPeer);
			if (teamOfPeer != null)
			{
				return teamOfPeer.GetOrderControllerOf(networkPeer.ControlledAgent);
			}
			MBDebug.Print("peersTeam == null", 0, Debug.DebugColor.White, 17592186044416UL);
			return null;
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x0008C58C File Offset: 0x0008A78C
		private void HandleServerEventSyncMissionTimer(GameNetworkMessage baseMessage)
		{
			SynchronizeMissionTimeTracker synchronizeMissionTimeTracker = (SynchronizeMissionTimeTracker)baseMessage;
			base.Mission.MissionTimeTracker.UpdateSync(synchronizeMissionTimeTracker.CurrentTime);
		}

		// Token: 0x0600265E RID: 9822 RVA: 0x0008C5B8 File Offset: 0x0008A7B8
		private void HandleServerEventSetPeerTeam(GameNetworkMessage baseMessage)
		{
			SetPeerTeam setPeerTeam = (SetPeerTeam)baseMessage;
			MissionPeer component = setPeerTeam.Peer.GetComponent<MissionPeer>();
			component.Team = Mission.MissionNetworkHelper.GetTeamFromTeamIndex(setPeerTeam.TeamIndex);
			if (setPeerTeam.Peer.IsMine)
			{
				base.Mission.PlayerTeam = component.Team;
			}
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x0008C608 File Offset: 0x0008A808
		private void HandleServerEventCreateFreeMountAgentEvent(GameNetworkMessage baseMessage)
		{
			CreateFreeMountAgent createFreeMountAgent = (CreateFreeMountAgent)baseMessage;
			Mission mission = base.Mission;
			EquipmentElement horseItem = createFreeMountAgent.HorseItem;
			EquipmentElement horseHarnessItem = createFreeMountAgent.HorseHarnessItem;
			Vec3 position = createFreeMountAgent.Position;
			Vec2 vec = createFreeMountAgent.Direction;
			vec = vec.Normalized();
			mission.SpawnMonster(horseItem, horseHarnessItem, in position, in vec, createFreeMountAgent.AgentIndex);
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x0008C654 File Offset: 0x0008A854
		private void HandleServerEventCreateAgent(GameNetworkMessage baseMessage)
		{
			CreateAgent createAgent = (CreateAgent)baseMessage;
			BasicCharacterObject character = createAgent.Character;
			NetworkCommunicator peer = createAgent.Peer;
			MissionPeer missionPeer = ((peer != null) ? peer.GetComponent<MissionPeer>() : null);
			Team teamFromTeamIndex = Mission.MissionNetworkHelper.GetTeamFromTeamIndex(createAgent.TeamIndex);
			AgentBuildData agentBuildData = new AgentBuildData(character).MissionPeer(createAgent.IsPlayerAgent ? missionPeer : null).Monster(createAgent.Monster).TroopOrigin(new BasicBattleAgentOrigin(character))
				.Equipment(createAgent.SpawnEquipment)
				.EquipmentSeed(createAgent.BodyPropertiesSeed);
			Vec3 position = createAgent.Position;
			AgentBuildData agentBuildData2 = agentBuildData.InitialPosition(in position);
			Vec2 vec = createAgent.Direction;
			vec = vec.Normalized();
			AgentBuildData agentBuildData3 = agentBuildData2.InitialDirection(in vec).MissionEquipment(createAgent.MissionEquipment).Team(teamFromTeamIndex)
				.Index(createAgent.AgentIndex)
				.MountIndex(createAgent.MountAgentIndex)
				.IsFemale(createAgent.IsFemale)
				.ClothingColor1(createAgent.ClothingColor1)
				.ClothingColor2(createAgent.ClothingColor2);
			Formation formation = null;
			if (teamFromTeamIndex != null && createAgent.FormationIndex >= 0 && !GameNetwork.IsReplay)
			{
				formation = teamFromTeamIndex.GetFormation((FormationClass)createAgent.FormationIndex);
				agentBuildData3.Formation(formation);
			}
			if (createAgent.IsPlayerAgent)
			{
				agentBuildData3.BodyProperties(missionPeer.Peer.BodyProperties);
				agentBuildData3.Age((int)agentBuildData3.AgentBodyProperties.Age);
			}
			else
			{
				agentBuildData3.BodyProperties(BodyProperties.GetRandomBodyProperties(agentBuildData3.AgentRace, agentBuildData3.AgentIsFemale, character.GetBodyPropertiesMin(false), character.GetBodyPropertiesMax(false), (int)agentBuildData3.AgentOverridenSpawnEquipment.HairCoverType, agentBuildData3.AgentEquipmentSeed, character.BodyPropertyRange.HairTags, character.BodyPropertyRange.BeardTags, character.BodyPropertyRange.TattooTags, 0f));
			}
			Banner banner = null;
			if (formation != null)
			{
				if (!string.IsNullOrEmpty(formation.BannerCode))
				{
					if (formation.Banner == null)
					{
						banner = new Banner(formation.BannerCode, teamFromTeamIndex.Color, teamFromTeamIndex.Color2);
						formation.Banner = banner;
					}
					else
					{
						banner = formation.Banner;
					}
				}
			}
			else if (missionPeer != null)
			{
				banner = new Banner(missionPeer.Peer.BannerCode, teamFromTeamIndex.Color, teamFromTeamIndex.Color2);
			}
			agentBuildData3.Banner(banner);
			Agent mountAgent = base.Mission.SpawnAgent(agentBuildData3, false, null, null).MountAgent;
		}

		// Token: 0x06002661 RID: 9825 RVA: 0x0008C898 File Offset: 0x0008AA98
		private void HandleServerEventSynchronizeAgentEquipment(GameNetworkMessage baseMessage)
		{
			SynchronizeAgentSpawnEquipment synchronizeAgentSpawnEquipment = (SynchronizeAgentSpawnEquipment)baseMessage;
			Mission.MissionNetworkHelper.GetAgentFromIndex(synchronizeAgentSpawnEquipment.AgentIndex, false).UpdateSpawnEquipmentAndRefreshVisuals(synchronizeAgentSpawnEquipment.SpawnEquipment);
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x0008C8C4 File Offset: 0x0008AAC4
		private void HandleServerEventCreateAgentVisuals(GameNetworkMessage baseMessage)
		{
			CreateAgentVisuals createAgentVisuals = (CreateAgentVisuals)baseMessage;
			MissionPeer component = createAgentVisuals.Peer.GetComponent<MissionPeer>();
			BattleSideEnum side = component.Team.Side;
			BasicCharacterObject character = createAgentVisuals.Character;
			BasicCultureObject culture = character.Culture;
			BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			BasicCultureObject object2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			MultiplayerBattleColors.MultiplayerCultureColorInfo peerColors = MultiplayerBattleColors.CreateWith(@object, object2).GetPeerColors(component);
			AgentBuildData agentBuildData = new AgentBuildData(character).VisualsIndex(createAgentVisuals.VisualsIndex).Equipment(createAgentVisuals.Equipment).EquipmentSeed(createAgentVisuals.BodyPropertiesSeed)
				.IsFemale(createAgentVisuals.IsFemale)
				.ClothingColor1(peerColors.ClothingColor1Uint)
				.ClothingColor2(peerColors.ClothingColor2Uint);
			if (createAgentVisuals.VisualsIndex == 0)
			{
				agentBuildData.BodyProperties(component.Peer.BodyProperties);
			}
			else
			{
				agentBuildData.BodyProperties(BodyProperties.GetRandomBodyProperties(agentBuildData.AgentRace, agentBuildData.AgentIsFemale, character.GetBodyPropertiesMin(false), character.GetBodyPropertiesMax(false), (int)agentBuildData.AgentOverridenSpawnEquipment.HairCoverType, createAgentVisuals.BodyPropertiesSeed, character.BodyPropertyRange.HairTags, character.BodyPropertyRange.BeardTags, character.BodyPropertyRange.TattooTags, 0f));
			}
			base.Mission.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>().SpawnAgentVisualsForPeer(component, agentBuildData, createAgentVisuals.SelectedEquipmentSetIndex, false, createAgentVisuals.TroopCountInFormation);
			if (agentBuildData.AgentVisualsIndex == 0)
			{
				component.HasSpawnedAgentVisuals = true;
				component.EquipmentUpdatingExpired = false;
			}
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x0008CA38 File Offset: 0x0008AC38
		private void HandleServerEventRemoveAgentVisualsForPeer(GameNetworkMessage baseMessage)
		{
			MissionPeer component = ((RemoveAgentVisualsForPeer)baseMessage).Peer.GetComponent<MissionPeer>();
			base.Mission.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>().RemoveAgentVisuals(component, false);
			component.HasSpawnedAgentVisuals = false;
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x0008CA6F File Offset: 0x0008AC6F
		private void HandleServerEventRemoveAgentVisualsFromIndexForPeer(GameNetworkMessage baseMessage)
		{
			((RemoveAgentVisualsFromIndexForPeer)baseMessage).Peer.GetComponent<MissionPeer>();
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x0008CA84 File Offset: 0x0008AC84
		private void HandleServerEventReplaceBotWithPlayer(GameNetworkMessage baseMessage)
		{
			ReplaceBotWithPlayer replaceBotWithPlayer = (ReplaceBotWithPlayer)baseMessage;
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(replaceBotWithPlayer.BotAgentIndex, false);
			if (agentFromIndex.Formation != null)
			{
				agentFromIndex.Formation.PlayerOwner = agentFromIndex;
			}
			MissionPeer component = replaceBotWithPlayer.Peer.GetComponent<MissionPeer>();
			agentFromIndex.MissionPeer = replaceBotWithPlayer.Peer.GetComponent<MissionPeer>();
			agentFromIndex.Formation = component.ControlledFormation;
			agentFromIndex.Health = (float)replaceBotWithPlayer.Health;
			if (agentFromIndex.MountAgent != null)
			{
				agentFromIndex.MountAgent.Health = (float)replaceBotWithPlayer.MountHealth;
			}
			if (agentFromIndex.Formation != null)
			{
				agentFromIndex.Team.AssignPlayerAsSergeantOfFormation(component, component.ControlledFormation.FormationIndex);
			}
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x0008CB28 File Offset: 0x0008AD28
		private void HandleServerEventSetWieldedItemIndex(GameNetworkMessage baseMessage)
		{
			SetWieldedItemIndex setWieldedItemIndex = (SetWieldedItemIndex)baseMessage;
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(setWieldedItemIndex.AgentIndex, false);
			if (agentFromIndex != null)
			{
				agentFromIndex.SetWieldedItemIndexAsClient(setWieldedItemIndex.IsLeftHand ? Agent.HandIndex.OffHand : Agent.HandIndex.MainHand, setWieldedItemIndex.WieldedItemIndex, setWieldedItemIndex.IsWieldedInstantly, setWieldedItemIndex.IsWieldedOnSpawn, setWieldedItemIndex.MainHandCurrentUsageIndex);
				agentFromIndex.UpdateAgentStats();
			}
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x0008CB7C File Offset: 0x0008AD7C
		private void HandleServerEventSetWeaponNetworkData(GameNetworkMessage baseMessage)
		{
			SetWeaponNetworkData setWeaponNetworkData = (SetWeaponNetworkData)baseMessage;
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(setWeaponNetworkData.AgentIndex, false);
			ItemObject item = agentFromIndex.Equipment[setWeaponNetworkData.WeaponEquipmentIndex].Item;
			WeaponComponentData weaponComponentData = ((item != null) ? item.PrimaryWeapon : null);
			if (weaponComponentData != null)
			{
				if (weaponComponentData.WeaponFlags.HasAnyFlag(WeaponFlags.HasHitPoints))
				{
					agentFromIndex.ChangeWeaponHitPoints(setWeaponNetworkData.WeaponEquipmentIndex, setWeaponNetworkData.DataValue);
					return;
				}
				if (weaponComponentData.IsConsumable)
				{
					agentFromIndex.SetWeaponAmountInSlot(setWeaponNetworkData.WeaponEquipmentIndex, setWeaponNetworkData.DataValue, true);
				}
			}
		}

		// Token: 0x06002668 RID: 9832 RVA: 0x0008CC08 File Offset: 0x0008AE08
		private void HandleServerEventSetWeaponAmmoData(GameNetworkMessage baseMessage)
		{
			SetWeaponAmmoData setWeaponAmmoData = (SetWeaponAmmoData)baseMessage;
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(setWeaponAmmoData.AgentIndex, false);
			if (agentFromIndex.Equipment[setWeaponAmmoData.WeaponEquipmentIndex].CurrentUsageItem.IsRangedWeapon)
			{
				agentFromIndex.SetWeaponAmmoAsClient(setWeaponAmmoData.WeaponEquipmentIndex, setWeaponAmmoData.AmmoEquipmentIndex, setWeaponAmmoData.Ammo);
				return;
			}
			Debug.FailedAssert("Invalid item type.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MissionNetworkComponent.cs", "HandleServerEventSetWeaponAmmoData", 469);
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x0008CC7C File Offset: 0x0008AE7C
		private void HandleServerEventSetWeaponReloadPhase(GameNetworkMessage baseMessage)
		{
			SetWeaponReloadPhase setWeaponReloadPhase = (SetWeaponReloadPhase)baseMessage;
			Mission.MissionNetworkHelper.GetAgentFromIndex(setWeaponReloadPhase.AgentIndex, false).SetWeaponReloadPhaseAsClient(setWeaponReloadPhase.EquipmentIndex, setWeaponReloadPhase.ReloadPhase);
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x0008CCB0 File Offset: 0x0008AEB0
		private void HandleServerEventWeaponUsageIndexChangeMessage(GameNetworkMessage baseMessage)
		{
			WeaponUsageIndexChangeMessage weaponUsageIndexChangeMessage = (WeaponUsageIndexChangeMessage)baseMessage;
			Mission.MissionNetworkHelper.GetAgentFromIndex(weaponUsageIndexChangeMessage.AgentIndex, false).SetUsageIndexOfWeaponInSlotAsClient(weaponUsageIndexChangeMessage.SlotIndex, weaponUsageIndexChangeMessage.UsageIndex);
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x0008CCE4 File Offset: 0x0008AEE4
		private void HandleServerEventStartSwitchingWeaponUsageIndex(GameNetworkMessage baseMessage)
		{
			StartSwitchingWeaponUsageIndex startSwitchingWeaponUsageIndex = (StartSwitchingWeaponUsageIndex)baseMessage;
			Mission.MissionNetworkHelper.GetAgentFromIndex(startSwitchingWeaponUsageIndex.AgentIndex, false).StartSwitchingWeaponUsageIndexAsClient(startSwitchingWeaponUsageIndex.EquipmentIndex, startSwitchingWeaponUsageIndex.UsageIndex, startSwitchingWeaponUsageIndex.CurrentMovementFlagUsageDirection);
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x0008CD1C File Offset: 0x0008AF1C
		private void HandleServerEventInitializeFormation(GameNetworkMessage baseMessage)
		{
			InitializeFormation initializeFormation = (InitializeFormation)baseMessage;
			Mission.MissionNetworkHelper.GetTeamFromTeamIndex(initializeFormation.TeamIndex).GetFormation((FormationClass)initializeFormation.FormationIndex).BannerCode = initializeFormation.BannerCode;
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x0008CD54 File Offset: 0x0008AF54
		private void HandleServerEventSetSpawnedFormationCount(GameNetworkMessage baseMessage)
		{
			SetSpawnedFormationCount setSpawnedFormationCount = (SetSpawnedFormationCount)baseMessage;
			base.Mission.NumOfFormationsSpawnedTeamOne = setSpawnedFormationCount.NumOfFormationsTeamOne;
			base.Mission.NumOfFormationsSpawnedTeamTwo = setSpawnedFormationCount.NumOfFormationsTeamTwo;
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x0008CD8C File Offset: 0x0008AF8C
		private void HandleServerEventAddTeam(GameNetworkMessage baseMessage)
		{
			AddTeam addTeam = (AddTeam)baseMessage;
			Banner banner = (string.IsNullOrEmpty(addTeam.BannerCode) ? null : new Banner(addTeam.BannerCode, addTeam.Color, addTeam.Color2));
			Team team = base.Mission.Teams.Add(addTeam.Side, addTeam.Color, addTeam.Color2, banner, addTeam.IsPlayerGeneral, addTeam.IsPlayerSergeant, true);
			if (addTeam.IsSpectatorTeam)
			{
				base.Mission.SpectatorTeam = team;
			}
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x0008CE10 File Offset: 0x0008B010
		private void HandleServerEventTeamSetIsEnemyOf(GameNetworkMessage baseMessage)
		{
			TeamSetIsEnemyOf teamSetIsEnemyOf = (TeamSetIsEnemyOf)baseMessage;
			Team teamFromTeamIndex = Mission.MissionNetworkHelper.GetTeamFromTeamIndex(teamSetIsEnemyOf.Team1Index);
			Team teamFromTeamIndex2 = Mission.MissionNetworkHelper.GetTeamFromTeamIndex(teamSetIsEnemyOf.Team2Index);
			teamFromTeamIndex.SetIsEnemyOf(teamFromTeamIndex2, teamSetIsEnemyOf.IsEnemyOf);
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x0008CE48 File Offset: 0x0008B048
		private void HandleServerEventAssignFormationToPlayer(GameNetworkMessage baseMessage)
		{
			AssignFormationToPlayer assignFormationToPlayer = (AssignFormationToPlayer)baseMessage;
			MissionPeer component = assignFormationToPlayer.Peer.GetComponent<MissionPeer>();
			component.Team.AssignPlayerAsSergeantOfFormation(component, assignFormationToPlayer.FormationClass);
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x0008CE7A File Offset: 0x0008B07A
		private void HandleServerEventExistingObjectsBegin(GameNetworkMessage baseMessage)
		{
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x0008CE7C File Offset: 0x0008B07C
		private void HandleServerEventExistingObjectsEnd(GameNetworkMessage baseMessage)
		{
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x0008CE7E File Offset: 0x0008B07E
		private void HandleServerEventClearMission(GameNetworkMessage baseMessage)
		{
			base.Mission.ResetMission();
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x0008CE8C File Offset: 0x0008B08C
		private void HandleServerEventCreateMissionObject(GameNetworkMessage baseMessage)
		{
			CreateMissionObject createMissionObject = (CreateMissionObject)baseMessage;
			GameEntity gameEntity = GameEntity.Instantiate(base.Mission.Scene, createMissionObject.Prefab, createMissionObject.Frame, true);
			MissionObject firstScriptOfType = gameEntity.GetFirstScriptOfType<MissionObject>();
			if (firstScriptOfType != null)
			{
				firstScriptOfType.Id = createMissionObject.ObjectId;
				int num = 0;
				using (IEnumerator<GameEntity> enumerator = gameEntity.GetChildren().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MissionObject firstScriptOfType2;
						if ((firstScriptOfType2 = enumerator.Current.GetFirstScriptOfType<MissionObject>()) != null)
						{
							firstScriptOfType2.Id = createMissionObject.ChildObjectIds[num++];
						}
					}
				}
			}
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x0008CF38 File Offset: 0x0008B138
		private void HandleServerEventRemoveMissionObject(GameNetworkMessage baseMessage)
		{
			RemoveMissionObject message = (RemoveMissionObject)baseMessage;
			MissionObject missionObject = base.Mission.MissionObjects.FirstOrDefault<MissionObject>((MissionObject mo) => mo.Id == message.ObjectId);
			if (missionObject == null)
			{
				return;
			}
			missionObject.GameEntity.Remove(82);
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x0008CF88 File Offset: 0x0008B188
		private void HandleServerEventStopPhysicsAndSetFrameOfMissionObject(GameNetworkMessage baseMessage)
		{
			StopPhysicsAndSetFrameOfMissionObject message = (StopPhysicsAndSetFrameOfMissionObject)baseMessage;
			SpawnedItemEntity spawnedItemEntity = (SpawnedItemEntity)base.Mission.MissionObjects.FirstOrDefault<MissionObject>((MissionObject mo) => mo.Id == message.ObjectId);
			MissionObject missionObjectFromMissionObjectId = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(message.ParentId);
			if (spawnedItemEntity == null)
			{
				return;
			}
			spawnedItemEntity.StopPhysicsAndSetFrameForClient(message.Frame, GameEntity.CreateFromWeakEntity((missionObjectFromMissionObjectId != null) ? missionObjectFromMissionObjectId.GameEntity : WeakGameEntity.Invalid));
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x0008D004 File Offset: 0x0008B204
		private void HandleServerEventBurstMissionObjectParticles(GameNetworkMessage baseMessage)
		{
			BurstMissionObjectParticles burstMissionObjectParticles = (BurstMissionObjectParticles)baseMessage;
			(Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(burstMissionObjectParticles.MissionObjectId) as SynchedMissionObject).BurstParticlesSynched(burstMissionObjectParticles.DoChildren);
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x0008D034 File Offset: 0x0008B234
		private void HandleServerEventSetMissionObjectVisibility(GameNetworkMessage baseMessage)
		{
			SetMissionObjectVisibility setMissionObjectVisibility = (SetMissionObjectVisibility)baseMessage;
			Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setMissionObjectVisibility.MissionObjectId).GameEntity.SetVisibilityExcludeParents(setMissionObjectVisibility.Visible);
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x0008D066 File Offset: 0x0008B266
		private void HandleServerEventSetMissionObjectDisabled(GameNetworkMessage baseMessage)
		{
			Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(((SetMissionObjectDisabled)baseMessage).MissionObjectId).SetDisabledAndMakeInvisible(false, false);
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x0008D080 File Offset: 0x0008B280
		private void HandleServerEventSetMissionObjectColors(GameNetworkMessage baseMessage)
		{
			SetMissionObjectColors setMissionObjectColors = (SetMissionObjectColors)baseMessage;
			SynchedMissionObject synchedMissionObject = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setMissionObjectColors.MissionObjectId) as SynchedMissionObject;
			if (synchedMissionObject != null)
			{
				synchedMissionObject.SetTeamColors(setMissionObjectColors.Color, setMissionObjectColors.Color2);
			}
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x0008D0BC File Offset: 0x0008B2BC
		private void HandleServerEventSetMissionObjectFrame(GameNetworkMessage baseMessage)
		{
			SetMissionObjectFrame setMissionObjectFrame = (SetMissionObjectFrame)baseMessage;
			SynchedMissionObject synchedMissionObject = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setMissionObjectFrame.MissionObjectId) as SynchedMissionObject;
			MatrixFrame frame = setMissionObjectFrame.Frame;
			synchedMissionObject.SetFrameSynched(ref frame, true);
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x0008D0F0 File Offset: 0x0008B2F0
		private void HandleServerEventSetMissionObjectGlobalFrame(GameNetworkMessage baseMessage)
		{
			SetMissionObjectGlobalFrame setMissionObjectGlobalFrame = (SetMissionObjectGlobalFrame)baseMessage;
			SynchedMissionObject synchedMissionObject = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setMissionObjectGlobalFrame.MissionObjectId) as SynchedMissionObject;
			MatrixFrame frame = setMissionObjectGlobalFrame.Frame;
			synchedMissionObject.SetGlobalFrameSynched(ref frame, true);
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x0008D124 File Offset: 0x0008B324
		private void HandleServerEventSetMissionObjectFrameOverTime(GameNetworkMessage baseMessage)
		{
			SetMissionObjectFrameOverTime setMissionObjectFrameOverTime = (SetMissionObjectFrameOverTime)baseMessage;
			SynchedMissionObject synchedMissionObject = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setMissionObjectFrameOverTime.MissionObjectId) as SynchedMissionObject;
			MatrixFrame frame = setMissionObjectFrameOverTime.Frame;
			synchedMissionObject.SetFrameSynchedOverTime(ref frame, setMissionObjectFrameOverTime.Duration, true);
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x0008D160 File Offset: 0x0008B360
		private void HandleServerEventSetMissionObjectGlobalFrameOverTime(GameNetworkMessage baseMessage)
		{
			SetMissionObjectGlobalFrameOverTime setMissionObjectGlobalFrameOverTime = (SetMissionObjectGlobalFrameOverTime)baseMessage;
			SynchedMissionObject synchedMissionObject = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setMissionObjectGlobalFrameOverTime.MissionObjectId) as SynchedMissionObject;
			MatrixFrame frame = setMissionObjectGlobalFrameOverTime.Frame;
			synchedMissionObject.SetGlobalFrameSynchedOverTime(ref frame, setMissionObjectGlobalFrameOverTime.Duration, true);
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x0008D19C File Offset: 0x0008B39C
		private void HandleServerEventSetMissionObjectAnimationAtChannel(GameNetworkMessage baseMessage)
		{
			SetMissionObjectAnimationAtChannel setMissionObjectAnimationAtChannel = (SetMissionObjectAnimationAtChannel)baseMessage;
			Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setMissionObjectAnimationAtChannel.MissionObjectId).GameEntity.Skeleton.SetAnimationAtChannel(setMissionObjectAnimationAtChannel.AnimationIndex, setMissionObjectAnimationAtChannel.ChannelNo, setMissionObjectAnimationAtChannel.AnimationSpeed, -1f, 0f);
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x0008D1EC File Offset: 0x0008B3EC
		private void HandleServerEventSetRangedSiegeWeaponAmmo(GameNetworkMessage baseMessage)
		{
			SetRangedSiegeWeaponAmmo setRangedSiegeWeaponAmmo = (SetRangedSiegeWeaponAmmo)baseMessage;
			(Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setRangedSiegeWeaponAmmo.RangedSiegeWeaponId) as RangedSiegeWeapon).SetAmmo(setRangedSiegeWeaponAmmo.AmmoCount);
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x0008D21C File Offset: 0x0008B41C
		private void HandleServerEventRangedSiegeWeaponChangeProjectile(GameNetworkMessage baseMessage)
		{
			RangedSiegeWeaponChangeProjectile rangedSiegeWeaponChangeProjectile = (RangedSiegeWeaponChangeProjectile)baseMessage;
			(Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(rangedSiegeWeaponChangeProjectile.RangedSiegeWeaponId) as RangedSiegeWeapon).ChangeProjectileEntityClient(rangedSiegeWeaponChangeProjectile.Index);
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x0008D24C File Offset: 0x0008B44C
		private void HandleServerEventSetStonePileAmmo(GameNetworkMessage baseMessage)
		{
			SetStonePileAmmo setStonePileAmmo = (SetStonePileAmmo)baseMessage;
			(Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setStonePileAmmo.StonePileId) as StonePile).SetAmmo(setStonePileAmmo.AmmoCount);
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x0008D27C File Offset: 0x0008B47C
		private void HandleServerEventSetRangedSiegeWeaponState(GameNetworkMessage baseMessage)
		{
			SetRangedSiegeWeaponState setRangedSiegeWeaponState = (SetRangedSiegeWeaponState)baseMessage;
			(Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setRangedSiegeWeaponState.RangedSiegeWeaponId) as RangedSiegeWeapon).State = setRangedSiegeWeaponState.State;
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x0008D2AC File Offset: 0x0008B4AC
		private void HandleServerEventSetSiegeLadderState(GameNetworkMessage baseMessage)
		{
			SetSiegeLadderState setSiegeLadderState = (SetSiegeLadderState)baseMessage;
			(Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setSiegeLadderState.SiegeLadderId) as SiegeLadder).State = setSiegeLadderState.State;
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x0008D2DC File Offset: 0x0008B4DC
		private void HandleServerEventSetSiegeTowerGateState(GameNetworkMessage baseMessage)
		{
			SetSiegeTowerGateState setSiegeTowerGateState = (SetSiegeTowerGateState)baseMessage;
			(Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setSiegeTowerGateState.SiegeTowerId) as SiegeTower).State = setSiegeTowerGateState.State;
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x0008D30B File Offset: 0x0008B50B
		private void HandleServerEventSetSiegeTowerHasArrivedAtTarget(GameNetworkMessage baseMessage)
		{
			(Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(((SetSiegeTowerHasArrivedAtTarget)baseMessage).SiegeTowerId) as SiegeTower).HasArrivedAtTarget = true;
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x0008D328 File Offset: 0x0008B528
		private void HandleServerEventSetBatteringRamHasArrivedAtTarget(GameNetworkMessage baseMessage)
		{
			(Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(((SetBatteringRamHasArrivedAtTarget)baseMessage).BatteringRamId) as BatteringRam).HasArrivedAtTarget = true;
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x0008D348 File Offset: 0x0008B548
		private void HandleServerEventSetSiegeMachineMovementDistance(GameNetworkMessage baseMessage)
		{
			SetSiegeMachineMovementDistance setSiegeMachineMovementDistance = (SetSiegeMachineMovementDistance)baseMessage;
			UsableMachine usableMachine = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setSiegeMachineMovementDistance.UsableMachineId) as UsableMachine;
			if (usableMachine != null)
			{
				if (usableMachine is SiegeTower)
				{
					((SiegeTower)usableMachine).MovementComponent.SetDistanceTraveledAsClient(setSiegeMachineMovementDistance.Distance);
					return;
				}
				((BatteringRam)usableMachine).MovementComponent.SetDistanceTraveledAsClient(setSiegeMachineMovementDistance.Distance);
			}
		}

		// Token: 0x06002689 RID: 9865 RVA: 0x0008D3A8 File Offset: 0x0008B5A8
		private void HandleServerEventSetMissionObjectAnimationChannelParameter(GameNetworkMessage baseMessage)
		{
			SetMissionObjectAnimationChannelParameter setMissionObjectAnimationChannelParameter = (SetMissionObjectAnimationChannelParameter)baseMessage;
			MissionObject missionObjectFromMissionObjectId = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setMissionObjectAnimationChannelParameter.MissionObjectId);
			if (missionObjectFromMissionObjectId != null)
			{
				missionObjectFromMissionObjectId.GameEntity.Skeleton.SetAnimationParameterAtChannel(setMissionObjectAnimationChannelParameter.ChannelNo, setMissionObjectAnimationChannelParameter.Parameter);
			}
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x0008D3EC File Offset: 0x0008B5EC
		private void HandleServerEventSetMissionObjectAnimationChannelSpeed(GameNetworkMessage baseMessage)
		{
			SetMissionObjectAnimationChannelSpeed setMissionObjectAnimationChannelSpeed = (SetMissionObjectAnimationChannelSpeed)baseMessage;
			MissionObject missionObjectFromMissionObjectId = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setMissionObjectAnimationChannelSpeed.MissionObjectId);
			if (missionObjectFromMissionObjectId != null)
			{
				missionObjectFromMissionObjectId.GameEntity.Skeleton.SetAnimationSpeedAtChannel(setMissionObjectAnimationChannelSpeed.ChannelNo, setMissionObjectAnimationChannelSpeed.Speed);
			}
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x0008D430 File Offset: 0x0008B630
		private void HandleServerEventSetMissionObjectVertexAnimation(GameNetworkMessage baseMessage)
		{
			SetMissionObjectVertexAnimation setMissionObjectVertexAnimation = (SetMissionObjectVertexAnimation)baseMessage;
			MissionObject missionObjectFromMissionObjectId = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setMissionObjectVertexAnimation.MissionObjectId);
			if (missionObjectFromMissionObjectId != null)
			{
				(missionObjectFromMissionObjectId as VertexAnimator).SetAnimationSynched(setMissionObjectVertexAnimation.BeginKey, setMissionObjectVertexAnimation.EndKey, setMissionObjectVertexAnimation.Speed);
			}
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x0008D470 File Offset: 0x0008B670
		private void HandleServerEventSetMissionObjectVertexAnimationProgress(GameNetworkMessage baseMessage)
		{
			SetMissionObjectVertexAnimationProgress setMissionObjectVertexAnimationProgress = (SetMissionObjectVertexAnimationProgress)baseMessage;
			MissionObject missionObjectFromMissionObjectId = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setMissionObjectVertexAnimationProgress.MissionObjectId);
			if (missionObjectFromMissionObjectId != null)
			{
				(missionObjectFromMissionObjectId as VertexAnimator).SetProgressSynched(setMissionObjectVertexAnimationProgress.Progress);
			}
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x0008D4A4 File Offset: 0x0008B6A4
		private void HandleServerEventSetMissionObjectAnimationPaused(GameNetworkMessage baseMessage)
		{
			SetMissionObjectAnimationPaused setMissionObjectAnimationPaused = (SetMissionObjectAnimationPaused)baseMessage;
			MissionObject missionObjectFromMissionObjectId = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setMissionObjectAnimationPaused.MissionObjectId);
			if (missionObjectFromMissionObjectId != null)
			{
				if (setMissionObjectAnimationPaused.IsPaused)
				{
					missionObjectFromMissionObjectId.GameEntity.PauseSkeletonAnimation();
					return;
				}
				missionObjectFromMissionObjectId.GameEntity.ResumeSkeletonAnimation();
			}
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x0008D4EC File Offset: 0x0008B6EC
		private void HandleServerEventAddMissionObjectBodyFlags(GameNetworkMessage baseMessage)
		{
			AddMissionObjectBodyFlags addMissionObjectBodyFlags = (AddMissionObjectBodyFlags)baseMessage;
			MissionObject missionObjectFromMissionObjectId = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(addMissionObjectBodyFlags.MissionObjectId);
			if (missionObjectFromMissionObjectId != null)
			{
				missionObjectFromMissionObjectId.GameEntity.AddBodyFlags(addMissionObjectBodyFlags.BodyFlags, addMissionObjectBodyFlags.ApplyToChildren);
			}
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x0008D52C File Offset: 0x0008B72C
		private void HandleServerEventRemoveMissionObjectBodyFlags(GameNetworkMessage baseMessage)
		{
			RemoveMissionObjectBodyFlags removeMissionObjectBodyFlags = (RemoveMissionObjectBodyFlags)baseMessage;
			MissionObject missionObjectFromMissionObjectId = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(removeMissionObjectBodyFlags.MissionObjectId);
			if (missionObjectFromMissionObjectId != null)
			{
				missionObjectFromMissionObjectId.GameEntity.RemoveBodyFlags(removeMissionObjectBodyFlags.BodyFlags, removeMissionObjectBodyFlags.ApplyToChildren);
			}
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x0008D56C File Offset: 0x0008B76C
		private void HandleServerEventSetMachineTargetRotation(GameNetworkMessage baseMessage)
		{
			SetMachineTargetRotation setMachineTargetRotation = (SetMachineTargetRotation)baseMessage;
			UsableMachine usableMachine = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setMachineTargetRotation.UsableMachineId) as UsableMachine;
			if (usableMachine != null && usableMachine.PilotAgent != null)
			{
				((RangedSiegeWeapon)usableMachine).AimAtRotation(setMachineTargetRotation.HorizontalRotation, setMachineTargetRotation.VerticalRotation);
			}
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x0008D5B4 File Offset: 0x0008B7B4
		private void HandleServerEventSetUsableGameObjectIsDeactivated(GameNetworkMessage baseMessage)
		{
			SetUsableMissionObjectIsDeactivated setUsableMissionObjectIsDeactivated = (SetUsableMissionObjectIsDeactivated)baseMessage;
			UsableMissionObject usableMissionObject = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setUsableMissionObjectIsDeactivated.UsableGameObjectId) as UsableMissionObject;
			if (usableMissionObject != null)
			{
				usableMissionObject.IsDeactivated = setUsableMissionObjectIsDeactivated.IsDeactivated;
			}
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x0008D5E8 File Offset: 0x0008B7E8
		private void HandleServerEventSetUsableGameObjectIsDisabledForPlayers(GameNetworkMessage baseMessage)
		{
			SetUsableMissionObjectIsDisabledForPlayers setUsableMissionObjectIsDisabledForPlayers = (SetUsableMissionObjectIsDisabledForPlayers)baseMessage;
			UsableMissionObject usableMissionObject = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setUsableMissionObjectIsDisabledForPlayers.UsableGameObjectId) as UsableMissionObject;
			if (usableMissionObject != null)
			{
				usableMissionObject.IsDisabledForPlayers = setUsableMissionObjectIsDisabledForPlayers.IsDisabledForPlayers;
			}
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x0008D61C File Offset: 0x0008B81C
		private void HandleServerEventSetMissionObjectImpulse(GameNetworkMessage baseMessage)
		{
			SetMissionObjectImpulse setMissionObjectImpulse = (SetMissionObjectImpulse)baseMessage;
			MissionObject missionObjectFromMissionObjectId = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setMissionObjectImpulse.MissionObjectId);
			if (missionObjectFromMissionObjectId != null)
			{
				Vec3 position = setMissionObjectImpulse.Position;
				missionObjectFromMissionObjectId.GameEntity.ApplyLocalImpulseToDynamicBody(position, setMissionObjectImpulse.Impulse);
			}
		}

		// Token: 0x06002694 RID: 9876 RVA: 0x0008D658 File Offset: 0x0008B858
		private void HandleServerEventSetAgentTargetPositionAndDirection(GameNetworkMessage baseMessage)
		{
			SetAgentTargetPositionAndDirection setAgentTargetPositionAndDirection = (SetAgentTargetPositionAndDirection)baseMessage;
			Vec2 position = setAgentTargetPositionAndDirection.Position;
			Vec3 direction = setAgentTargetPositionAndDirection.Direction;
			Mission.MissionNetworkHelper.GetAgentFromIndex(setAgentTargetPositionAndDirection.AgentIndex, false).SetTargetPositionAndDirectionSynched(ref position, ref direction);
		}

		// Token: 0x06002695 RID: 9877 RVA: 0x0008D690 File Offset: 0x0008B890
		private void HandleServerEventSetAgentTargetPosition(GameNetworkMessage baseMessage)
		{
			SetAgentTargetPosition setAgentTargetPosition = (SetAgentTargetPosition)baseMessage;
			Vec2 position = setAgentTargetPosition.Position;
			Mission.MissionNetworkHelper.GetAgentFromIndex(setAgentTargetPosition.AgentIndex, false).SetTargetPositionSynched(ref position);
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x0008D6BC File Offset: 0x0008B8BC
		private void HandleServerEventClearAgentTargetFrame(GameNetworkMessage baseMessage)
		{
			Mission.MissionNetworkHelper.GetAgentFromIndex(((ClearAgentTargetFrame)baseMessage).AgentIndex, false).ClearTargetFrame();
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x0008D6D4 File Offset: 0x0008B8D4
		private void HandleServerEventAgentTeleportToFrame(GameNetworkMessage baseMessage)
		{
			AgentTeleportToFrame agentTeleportToFrame = (AgentTeleportToFrame)baseMessage;
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(agentTeleportToFrame.AgentIndex, false);
			agentFromIndex.TeleportToPosition(agentTeleportToFrame.Position);
			Vec2 vec = agentTeleportToFrame.Direction.Normalized();
			agentFromIndex.SetMovementDirection(in vec);
			agentFromIndex.LookDirection = vec.ToVec3(0f);
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x0008D728 File Offset: 0x0008B928
		private void HandleServerEventSetAgentPeer(GameNetworkMessage baseMessage)
		{
			SetAgentPeer setAgentPeer = (SetAgentPeer)baseMessage;
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(setAgentPeer.AgentIndex, true);
			if (agentFromIndex != null)
			{
				NetworkCommunicator peer = setAgentPeer.Peer;
				MissionPeer missionPeer = ((peer != null) ? peer.GetComponent<MissionPeer>() : null);
				agentFromIndex.MissionPeer = missionPeer;
			}
		}

		// Token: 0x06002699 RID: 9881 RVA: 0x0008D768 File Offset: 0x0008B968
		private void HandleServerEventSetAgentIsPlayer(GameNetworkMessage baseMessage)
		{
			SetAgentIsPlayer setAgentIsPlayer = (SetAgentIsPlayer)baseMessage;
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(setAgentIsPlayer.AgentIndex, false);
			if (agentFromIndex.Controller == AgentControllerType.Player != setAgentIsPlayer.IsPlayer)
			{
				if (!agentFromIndex.IsMine)
				{
					agentFromIndex.Controller = AgentControllerType.None;
					return;
				}
				agentFromIndex.Controller = AgentControllerType.Player;
			}
		}

		// Token: 0x0600269A RID: 9882 RVA: 0x0008D7B4 File Offset: 0x0008B9B4
		private void HandleServerEventSetAgentHealth(GameNetworkMessage baseMessage)
		{
			SetAgentHealth setAgentHealth = (SetAgentHealth)baseMessage;
			Mission.MissionNetworkHelper.GetAgentFromIndex(setAgentHealth.AgentIndex, false).Health = (float)setAgentHealth.Health;
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x0008D7E0 File Offset: 0x0008B9E0
		private void HandleServerEventAgentSetTeam(GameNetworkMessage baseMessage)
		{
			AgentSetTeam agentSetTeam = (AgentSetTeam)baseMessage;
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(agentSetTeam.AgentIndex, false);
			MBTeam mbteamFromTeamIndex = Mission.MissionNetworkHelper.GetMBTeamFromTeamIndex(agentSetTeam.TeamIndex);
			agentFromIndex.SetTeam(base.Mission.Teams.Find(mbteamFromTeamIndex), false);
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x0008D824 File Offset: 0x0008BA24
		private void HandleServerEventSetAgentActionSet(GameNetworkMessage baseMessage)
		{
			SetAgentActionSet setAgentActionSet = (SetAgentActionSet)baseMessage;
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(setAgentActionSet.AgentIndex, false);
			AnimationSystemData animationSystemData = agentFromIndex.Monster.FillAnimationSystemData(setAgentActionSet.ActionSet, setAgentActionSet.StepSize, false);
			animationSystemData.NumPaces = setAgentActionSet.NumPaces;
			animationSystemData.MonsterUsageSetIndex = setAgentActionSet.MonsterUsageSetIndex;
			animationSystemData.WalkingSpeedLimit = setAgentActionSet.WalkingSpeedLimit;
			animationSystemData.CrouchWalkingSpeedLimit = setAgentActionSet.CrouchWalkingSpeedLimit;
			agentFromIndex.SetActionSet(ref animationSystemData);
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x0008D898 File Offset: 0x0008BA98
		private void HandleServerEventMakeAgentDead(GameNetworkMessage baseMessage)
		{
			MakeAgentDead makeAgentDead = (MakeAgentDead)baseMessage;
			Mission.MissionNetworkHelper.GetAgentFromIndex(makeAgentDead.AgentIndex, false).MakeDead(makeAgentDead.IsKilled, makeAgentDead.ActionCodeIndex, makeAgentDead.CorpsesToFadeIndex);
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x0008D8D0 File Offset: 0x0008BAD0
		private void HandleServerEventAddPrefabComponentToAgentBone(GameNetworkMessage baseMessage)
		{
			AddPrefabComponentToAgentBone addPrefabComponentToAgentBone = (AddPrefabComponentToAgentBone)baseMessage;
			Mission.MissionNetworkHelper.GetAgentFromIndex(addPrefabComponentToAgentBone.AgentIndex, false).AddSynchedPrefabComponentToBone(addPrefabComponentToAgentBone.PrefabName, addPrefabComponentToAgentBone.BoneIndex);
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x0008D904 File Offset: 0x0008BB04
		private void HandleServerEventSetAgentPrefabComponentVisibility(GameNetworkMessage baseMessage)
		{
			SetAgentPrefabComponentVisibility setAgentPrefabComponentVisibility = (SetAgentPrefabComponentVisibility)baseMessage;
			Mission.MissionNetworkHelper.GetAgentFromIndex(setAgentPrefabComponentVisibility.AgentIndex, false).SetSynchedPrefabComponentVisibility(setAgentPrefabComponentVisibility.ComponentIndex, setAgentPrefabComponentVisibility.Visibility);
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x0008D938 File Offset: 0x0008BB38
		private void HandleServerEventAgentSetFormation(GameNetworkMessage baseMessage)
		{
			AgentSetFormation agentSetFormation = (AgentSetFormation)baseMessage;
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(agentSetFormation.AgentIndex, false);
			Team team = agentFromIndex.Team;
			Formation formation = null;
			if (team != null)
			{
				formation = ((agentSetFormation.FormationIndex >= 0) ? team.GetFormation((FormationClass)agentSetFormation.FormationIndex) : null);
			}
			agentFromIndex.Formation = formation;
		}

		// Token: 0x060026A1 RID: 9889 RVA: 0x0008D984 File Offset: 0x0008BB84
		private void HandleServerEventUseObject(GameNetworkMessage baseMessage)
		{
			UseObject useObject = (UseObject)baseMessage;
			UsableMissionObject usableMissionObject = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(useObject.UsableGameObjectId) as UsableMissionObject;
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(useObject.AgentIndex, false);
			if (usableMissionObject != null)
			{
				usableMissionObject.SetUserForClient(agentFromIndex);
			}
		}

		// Token: 0x060026A2 RID: 9890 RVA: 0x0008D9C0 File Offset: 0x0008BBC0
		private void HandleServerEventStopUsingObject(GameNetworkMessage baseMessage)
		{
			StopUsingObject stopUsingObject = (StopUsingObject)baseMessage;
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(stopUsingObject.AgentIndex, false);
			if (agentFromIndex == null)
			{
				return;
			}
			agentFromIndex.StopUsingGameObject(stopUsingObject.IsSuccessful, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x0008D9F4 File Offset: 0x0008BBF4
		private void HandleServerEventHitSynchronizeObjectHitpoints(GameNetworkMessage baseMessage)
		{
			SyncObjectHitpoints syncObjectHitpoints = (SyncObjectHitpoints)baseMessage;
			MissionObject missionObjectFromMissionObjectId = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(syncObjectHitpoints.MissionObjectId);
			if (missionObjectFromMissionObjectId != null)
			{
				missionObjectFromMissionObjectId.GameEntity.GetFirstScriptOfType<DestructableComponent>().HitPoint = syncObjectHitpoints.Hitpoints;
			}
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x0008DA30 File Offset: 0x0008BC30
		private void HandleServerEventHitSynchronizeObjectDestructionLevel(GameNetworkMessage baseMessage)
		{
			SyncObjectDestructionLevel syncObjectDestructionLevel = (SyncObjectDestructionLevel)baseMessage;
			MissionObject missionObjectFromMissionObjectId = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(syncObjectDestructionLevel.MissionObjectId);
			if (missionObjectFromMissionObjectId == null)
			{
				return;
			}
			missionObjectFromMissionObjectId.GameEntity.GetFirstScriptOfType<DestructableComponent>().SetDestructionLevel(syncObjectDestructionLevel.DestructionLevel, syncObjectDestructionLevel.ForcedIndex, syncObjectDestructionLevel.BlowMagnitude, syncObjectDestructionLevel.BlowPosition, syncObjectDestructionLevel.BlowDirection, false);
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x0008DA88 File Offset: 0x0008BC88
		private void HandleServerEventHitBurstAllHeavyHitParticles(GameNetworkMessage baseMessage)
		{
			MissionObject missionObjectFromMissionObjectId = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(((BurstAllHeavyHitParticles)baseMessage).MissionObjectId);
			if (missionObjectFromMissionObjectId == null)
			{
				return;
			}
			missionObjectFromMissionObjectId.GameEntity.GetFirstScriptOfType<DestructableComponent>().BurstHeavyHitParticles();
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x0008DABC File Offset: 0x0008BCBC
		private void HandleServerEventSynchronizeMissionObject(GameNetworkMessage baseMessage)
		{
			SynchronizeMissionObject synchronizeMissionObject = (SynchronizeMissionObject)baseMessage;
			SynchedMissionObject synchedMissionObject = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(synchronizeMissionObject.MissionObjectId) as SynchedMissionObject;
			ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> recordPair = synchronizeMissionObject.RecordPair;
			synchedMissionObject.OnAfterReadFromNetwork(recordPair, true);
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x0008DAF0 File Offset: 0x0008BCF0
		private void HandleServerEventSpawnWeaponWithNewEntity(GameNetworkMessage baseMessage)
		{
			SpawnWeaponWithNewEntity spawnWeaponWithNewEntity = (SpawnWeaponWithNewEntity)baseMessage;
			MissionObject missionObjectFromMissionObjectId = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(spawnWeaponWithNewEntity.ParentMissionObjectId);
			GameEntity gameEntity = base.Mission.SpawnWeaponWithNewEntityAux(spawnWeaponWithNewEntity.Weapon, spawnWeaponWithNewEntity.WeaponSpawnFlags, spawnWeaponWithNewEntity.Frame, spawnWeaponWithNewEntity.ForcedIndex, missionObjectFromMissionObjectId, spawnWeaponWithNewEntity.HasLifeTime, spawnWeaponWithNewEntity.SpawnedOnACorpse);
			if (!spawnWeaponWithNewEntity.IsVisible)
			{
				gameEntity.SetVisibilityExcludeParents(false);
			}
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x0008DB50 File Offset: 0x0008BD50
		private void HandleServerEventAttachWeaponToSpawnedWeapon(GameNetworkMessage baseMessage)
		{
			AttachWeaponToSpawnedWeapon attachWeaponToSpawnedWeapon = (AttachWeaponToSpawnedWeapon)baseMessage;
			MissionObject missionObjectFromMissionObjectId = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(attachWeaponToSpawnedWeapon.MissionObjectId);
			base.Mission.AttachWeaponWithNewEntityToSpawnedWeapon(attachWeaponToSpawnedWeapon.Weapon, missionObjectFromMissionObjectId as SpawnedItemEntity, attachWeaponToSpawnedWeapon.AttachLocalFrame);
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x0008DB90 File Offset: 0x0008BD90
		private void HandleServerEventAttachWeaponToAgent(GameNetworkMessage baseMessage)
		{
			AttachWeaponToAgent attachWeaponToAgent = (AttachWeaponToAgent)baseMessage;
			MatrixFrame attachLocalFrame = attachWeaponToAgent.AttachLocalFrame;
			Mission.MissionNetworkHelper.GetAgentFromIndex(attachWeaponToAgent.AgentIndex, false).AttachWeaponToBone(attachWeaponToAgent.Weapon, null, attachWeaponToAgent.BoneIndex, ref attachLocalFrame);
		}

		// Token: 0x060026AA RID: 9898 RVA: 0x0008DBCC File Offset: 0x0008BDCC
		private void HandleServerEventHandleMissileCollisionReaction(GameNetworkMessage baseMessage)
		{
			HandleMissileCollisionReaction handleMissileCollisionReaction = (HandleMissileCollisionReaction)baseMessage;
			MissionObject missionObjectFromMissionObjectId = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(handleMissileCollisionReaction.AttachedMissionObjectId);
			base.Mission.HandleMissileCollisionReaction(handleMissileCollisionReaction.MissileIndex, handleMissileCollisionReaction.CollisionReaction, handleMissileCollisionReaction.AttachLocalFrame, handleMissileCollisionReaction.IsAttachedFrameLocal, Mission.MissionNetworkHelper.GetAgentFromIndex(handleMissileCollisionReaction.AttackerAgentIndex, true), Mission.MissionNetworkHelper.GetAgentFromIndex(handleMissileCollisionReaction.AttachedAgentIndex, true), handleMissileCollisionReaction.AttachedToShield, handleMissileCollisionReaction.AttachedBoneIndex, missionObjectFromMissionObjectId, handleMissileCollisionReaction.BounceBackVelocity, handleMissileCollisionReaction.BounceBackAngularVelocity, handleMissileCollisionReaction.ForcedSpawnIndex);
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x0008DC48 File Offset: 0x0008BE48
		private void HandleServerEventSpawnWeaponAsDropFromAgent(GameNetworkMessage baseMessage)
		{
			SpawnWeaponAsDropFromAgent spawnWeaponAsDropFromAgent = (SpawnWeaponAsDropFromAgent)baseMessage;
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(spawnWeaponAsDropFromAgent.AgentIndex, false);
			Vec3 velocity = spawnWeaponAsDropFromAgent.Velocity;
			Vec3 angularVelocity = spawnWeaponAsDropFromAgent.AngularVelocity;
			base.Mission.SpawnWeaponAsDropFromAgentAux(agentFromIndex, spawnWeaponAsDropFromAgent.EquipmentIndex, ref velocity, ref angularVelocity, spawnWeaponAsDropFromAgent.WeaponSpawnFlags, spawnWeaponAsDropFromAgent.ForcedIndex);
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x0008DC9C File Offset: 0x0008BE9C
		private void HandleServerEventSpawnAttachedWeaponOnSpawnedWeapon(GameNetworkMessage baseMessage)
		{
			SpawnAttachedWeaponOnSpawnedWeapon spawnAttachedWeaponOnSpawnedWeapon = (SpawnAttachedWeaponOnSpawnedWeapon)baseMessage;
			SpawnedItemEntity spawnedItemEntity = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(spawnAttachedWeaponOnSpawnedWeapon.SpawnedWeaponId) as SpawnedItemEntity;
			base.Mission.SpawnAttachedWeaponOnSpawnedWeapon(spawnedItemEntity, spawnAttachedWeaponOnSpawnedWeapon.AttachmentIndex, spawnAttachedWeaponOnSpawnedWeapon.ForcedIndex);
		}

		// Token: 0x060026AD RID: 9901 RVA: 0x0008DCDC File Offset: 0x0008BEDC
		private void HandleServerEventSpawnAttachedWeaponOnCorpse(GameNetworkMessage baseMessage)
		{
			SpawnAttachedWeaponOnCorpse spawnAttachedWeaponOnCorpse = (SpawnAttachedWeaponOnCorpse)baseMessage;
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(spawnAttachedWeaponOnCorpse.AgentIndex, false);
			base.Mission.SpawnAttachedWeaponOnCorpse(agentFromIndex, spawnAttachedWeaponOnCorpse.AttachedIndex, spawnAttachedWeaponOnCorpse.ForcedIndex);
		}

		// Token: 0x060026AE RID: 9902 RVA: 0x0008DD18 File Offset: 0x0008BF18
		private void HandleServerEventRemoveEquippedWeapon(GameNetworkMessage baseMessage)
		{
			RemoveEquippedWeapon removeEquippedWeapon = (RemoveEquippedWeapon)baseMessage;
			Mission.MissionNetworkHelper.GetAgentFromIndex(removeEquippedWeapon.AgentIndex, false).RemoveEquippedWeapon(removeEquippedWeapon.SlotIndex);
		}

		// Token: 0x060026AF RID: 9903 RVA: 0x0008DD44 File Offset: 0x0008BF44
		private void HandleServerEventBarkAgent(GameNetworkMessage baseMessage)
		{
			BarkAgent barkAgent = (BarkAgent)baseMessage;
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(barkAgent.AgentIndex, false);
			agentFromIndex.HandleBark(barkAgent.IndexOfBark);
			if (!this._chatBox.IsPlayerMuted(agentFromIndex.MissionPeer.Peer.Id))
			{
				GameTexts.SetVariable("LEFT", agentFromIndex.NameTextObject);
				GameTexts.SetVariable("RIGHT", SkinVoiceManager.VoiceType.MpBarks[barkAgent.IndexOfBark].GetName());
				InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).ToString(), Color.White, "Bark"));
			}
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x0008DDE4 File Offset: 0x0008BFE4
		private void HandleServerEventEquipWeaponWithNewEntity(GameNetworkMessage baseMessage)
		{
			EquipWeaponWithNewEntity equipWeaponWithNewEntity = (EquipWeaponWithNewEntity)baseMessage;
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(equipWeaponWithNewEntity.AgentIndex, false);
			if (agentFromIndex != null)
			{
				MissionWeapon weapon = equipWeaponWithNewEntity.Weapon;
				agentFromIndex.EquipWeaponWithNewEntity(equipWeaponWithNewEntity.SlotIndex, ref weapon);
			}
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x0008DE20 File Offset: 0x0008C020
		private void HandleServerEventAttachWeaponToWeaponInAgentEquipmentSlot(GameNetworkMessage baseMessage)
		{
			AttachWeaponToWeaponInAgentEquipmentSlot attachWeaponToWeaponInAgentEquipmentSlot = (AttachWeaponToWeaponInAgentEquipmentSlot)baseMessage;
			MatrixFrame attachLocalFrame = attachWeaponToWeaponInAgentEquipmentSlot.AttachLocalFrame;
			Mission.MissionNetworkHelper.GetAgentFromIndex(attachWeaponToWeaponInAgentEquipmentSlot.AgentIndex, false).AttachWeaponToWeapon(attachWeaponToWeaponInAgentEquipmentSlot.SlotIndex, attachWeaponToWeaponInAgentEquipmentSlot.Weapon, null, ref attachLocalFrame);
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x0008DE5C File Offset: 0x0008C05C
		private void HandleServerEventEquipWeaponFromSpawnedItemEntity(GameNetworkMessage baseMessage)
		{
			EquipWeaponFromSpawnedItemEntity equipWeaponFromSpawnedItemEntity = (EquipWeaponFromSpawnedItemEntity)baseMessage;
			SpawnedItemEntity spawnedItemEntity = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(equipWeaponFromSpawnedItemEntity.SpawnedItemEntityId) as SpawnedItemEntity;
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(equipWeaponFromSpawnedItemEntity.AgentIndex, true);
			if (agentFromIndex == null)
			{
				return;
			}
			agentFromIndex.EquipWeaponFromSpawnedItemEntity(equipWeaponFromSpawnedItemEntity.SlotIndex, spawnedItemEntity, equipWeaponFromSpawnedItemEntity.RemoveWeapon);
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x0008DEA4 File Offset: 0x0008C0A4
		private void HandleServerEventCreateMissile(GameNetworkMessage baseMessage)
		{
			CreateMissile createMissile = (CreateMissile)baseMessage;
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(createMissile.AgentIndex, false);
			if (createMissile.WeaponIndex != EquipmentIndex.None)
			{
				Vec3 vec = createMissile.Direction * createMissile.Speed;
				base.Mission.OnAgentShootMissile(agentFromIndex, createMissile.WeaponIndex, createMissile.Position, vec, createMissile.Orientation, createMissile.HasRigidBody, createMissile.IsPrimaryWeaponShot, createMissile.MissileIndex);
				return;
			}
			MissionObject missionObjectFromMissionObjectId = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(createMissile.MissionObjectToIgnoreId);
			base.Mission.AddCustomMissile(agentFromIndex, createMissile.Weapon, createMissile.Position, createMissile.Direction, createMissile.Orientation, createMissile.Speed, createMissile.Speed, createMissile.HasRigidBody, missionObjectFromMissionObjectId, createMissile.MissileIndex);
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x0008DF5C File Offset: 0x0008C15C
		private void HandleServerEventAgentHit(GameNetworkMessage baseMessage)
		{
			CombatLogManager.GenerateCombatLog(Mission.MissionNetworkHelper.GetCombatLogDataForCombatLogNetworkMessage((CombatLogNetworkMessage)baseMessage));
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x0008DF70 File Offset: 0x0008C170
		private void HandleServerEventConsumeWeaponAmount(GameNetworkMessage baseMessage)
		{
			ConsumeWeaponAmount consumeWeaponAmount = (ConsumeWeaponAmount)baseMessage;
			(Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(consumeWeaponAmount.SpawnedItemEntityId) as SpawnedItemEntity).ConsumeWeaponAmount(consumeWeaponAmount.ConsumedAmount);
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x0008DFA0 File Offset: 0x0008C1A0
		private void HandleServerEventSetAgentOwningMissionPeer(GameNetworkMessage baseMessage)
		{
			SetAgentOwningMissionPeer setAgentOwningMissionPeer = (SetAgentOwningMissionPeer)baseMessage;
			Agent agent = Mission.Current.FindAgentWithIndex(setAgentOwningMissionPeer.AgentIndex);
			VirtualPlayer peer = setAgentOwningMissionPeer.Peer;
			MissionPeer missionPeer = ((peer != null) ? peer.GetComponent<MissionPeer>() : null);
			agent.SetOwningAgentMissionPeer(missionPeer);
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x0008DFE0 File Offset: 0x0008C1E0
		private bool HandleClientEventSetFollowedAgent(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			SetFollowedAgent setFollowedAgent = (SetFollowedAgent)baseMessage;
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			if (component != null)
			{
				Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(setFollowedAgent.AgentIndex, true);
				component.FollowedAgent = agentFromIndex;
			}
			return true;
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x0008E014 File Offset: 0x0008C214
		private bool HandleClientEventSetMachineRotation(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			SetMachineRotation setMachineRotation = (SetMachineRotation)baseMessage;
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			UsableMachine usableMachine = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(setMachineRotation.UsableMachineId) as UsableMachine;
			if (component.IsControlledAgentActive && usableMachine is RangedSiegeWeapon)
			{
				RangedSiegeWeapon rangedSiegeWeapon = usableMachine as RangedSiegeWeapon;
				if (component.ControlledAgent == rangedSiegeWeapon.PilotAgent && rangedSiegeWeapon.PilotAgent != null)
				{
					rangedSiegeWeapon.AimAtRotation(setMachineRotation.HorizontalRotation, setMachineRotation.VerticalRotation);
				}
			}
			return true;
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x0008E080 File Offset: 0x0008C280
		private bool HandleClientEventRequestUseObject(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			RequestUseObject requestUseObject = (RequestUseObject)baseMessage;
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			UsableMissionObject usableMissionObject = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(requestUseObject.UsableMissionObjectId) as UsableMissionObject;
			if (usableMissionObject != null && component.ControlledAgent != null && component.ControlledAgent.IsActive())
			{
				Vec3 position = component.ControlledAgent.Position;
				Vec3 globalPosition = usableMissionObject.InteractionEntity.GlobalPosition;
				float num;
				if (usableMissionObject is StandingPoint)
				{
					num = usableMissionObject.GetUserFrameForAgent(component.ControlledAgent).Origin.AsVec2.Distance(component.ControlledAgent.Position.AsVec2);
				}
				else
				{
					Vec3 vec;
					Vec3 vec2;
					usableMissionObject.InteractionEntity.GetPhysicsMinMax(true, out vec, out vec2, false);
					float num2 = globalPosition.Distance(vec);
					float num3 = globalPosition.Distance(vec2);
					float num4 = MathF.Max(num2, num3);
					num = globalPosition.Distance(new Vec3(position.x, position.y, position.z + component.ControlledAgent.GetEyeGlobalHeight(), -1f));
					num -= num4;
					num = MathF.Max(num, 0f);
				}
				if (component.ControlledAgent.CurrentlyUsedGameObject != usableMissionObject && component.ControlledAgent.CanReachAndUseObject(usableMissionObject, num * num * 0.9f * 0.9f) && component.ControlledAgent.ObjectHasVacantPosition(usableMissionObject))
				{
					component.ControlledAgent.UseGameObject(usableMissionObject, requestUseObject.UsedObjectPreferenceIndex);
				}
			}
			return true;
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x0008E1F0 File Offset: 0x0008C3F0
		private bool HandleClientEventRequestStopUsingObject(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			RequestStopUsingObject requestStopUsingObject = (RequestStopUsingObject)baseMessage;
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			Agent controlledAgent = component.ControlledAgent;
			if (((controlledAgent != null) ? controlledAgent.CurrentlyUsedGameObject : null) != null)
			{
				component.ControlledAgent.StopUsingGameObject(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
			}
			return true;
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x0008E230 File Offset: 0x0008C430
		private bool HandleClientEventApplyOrder(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			ApplyOrder applyOrder = (ApplyOrder)baseMessage;
			OrderController orderControllerOfPeer = this.GetOrderControllerOfPeer(networkPeer);
			if (orderControllerOfPeer != null)
			{
				orderControllerOfPeer.SetOrder(applyOrder.OrderType);
			}
			return true;
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x0008E25C File Offset: 0x0008C45C
		private bool HandleClientEventApplySiegeWeaponOrder(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			ApplySiegeWeaponOrder applySiegeWeaponOrder = (ApplySiegeWeaponOrder)baseMessage;
			OrderController orderControllerOfPeer = this.GetOrderControllerOfPeer(networkPeer);
			if (orderControllerOfPeer != null)
			{
				orderControllerOfPeer.SiegeWeaponController.SetOrder(applySiegeWeaponOrder.OrderType);
			}
			return true;
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x0008E290 File Offset: 0x0008C490
		private bool HandleClientEventApplyOrderWithPosition(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			ApplyOrderWithPosition applyOrderWithPosition = (ApplyOrderWithPosition)baseMessage;
			OrderController orderControllerOfPeer = this.GetOrderControllerOfPeer(networkPeer);
			if (orderControllerOfPeer != null)
			{
				WorldPosition worldPosition = new WorldPosition(base.Mission.Scene, UIntPtr.Zero, applyOrderWithPosition.Position, false);
				orderControllerOfPeer.SetOrderWithPosition(applyOrderWithPosition.OrderType, worldPosition);
			}
			return true;
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x0008E2DC File Offset: 0x0008C4DC
		private bool HandleClientEventApplyOrderWithFormation(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			ApplyOrderWithFormation message = (ApplyOrderWithFormation)baseMessage;
			Team teamOfPeer = this.GetTeamOfPeer(networkPeer);
			OrderController orderController = ((teamOfPeer != null) ? teamOfPeer.GetOrderControllerOf(networkPeer.ControlledAgent) : null);
			Formation formation = ((teamOfPeer != null) ? teamOfPeer.FormationsIncludingEmpty.SingleOrDefault<Formation>((Formation f) => f.CountOfUnits > 0 && f.Index == message.FormationIndex) : null);
			if (teamOfPeer != null && orderController != null && formation != null)
			{
				orderController.SetOrderWithFormation(message.OrderType, formation);
			}
			return true;
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x0008E350 File Offset: 0x0008C550
		private bool HandleClientEventApplyOrderWithFormationAndPercentage(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			ApplyOrderWithFormationAndPercentage message = (ApplyOrderWithFormationAndPercentage)baseMessage;
			Team teamOfPeer = this.GetTeamOfPeer(networkPeer);
			OrderController orderController = ((teamOfPeer != null) ? teamOfPeer.GetOrderControllerOf(networkPeer.ControlledAgent) : null);
			Formation formation = ((teamOfPeer != null) ? teamOfPeer.FormationsIncludingEmpty.SingleOrDefault<Formation>((Formation f) => f.CountOfUnits > 0 && f.Index == message.FormationIndex) : null);
			float num = (float)message.Percentage * 0.01f;
			if (teamOfPeer != null && orderController != null && formation != null)
			{
				orderController.SetOrderWithFormationAndPercentage(message.OrderType, formation, num);
			}
			return true;
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x0008E3D8 File Offset: 0x0008C5D8
		private bool HandleClientEventApplyOrderWithFormationAndNumber(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			ApplyOrderWithFormationAndNumber message = (ApplyOrderWithFormationAndNumber)baseMessage;
			Team teamOfPeer = this.GetTeamOfPeer(networkPeer);
			OrderController orderController = ((teamOfPeer != null) ? teamOfPeer.GetOrderControllerOf(networkPeer.ControlledAgent) : null);
			Formation formation = ((teamOfPeer != null) ? teamOfPeer.FormationsIncludingEmpty.SingleOrDefault<Formation>((Formation f) => f.CountOfUnits > 0 && f.Index == message.FormationIndex) : null);
			int number = message.Number;
			if (teamOfPeer != null && orderController != null && formation != null)
			{
				orderController.SetOrderWithFormationAndNumber(message.OrderType, formation, number);
			}
			return true;
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x0008E45C File Offset: 0x0008C65C
		private bool HandleClientEventApplyOrderWithTwoPositions(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			ApplyOrderWithTwoPositions applyOrderWithTwoPositions = (ApplyOrderWithTwoPositions)baseMessage;
			OrderController orderControllerOfPeer = this.GetOrderControllerOfPeer(networkPeer);
			if (orderControllerOfPeer != null)
			{
				WorldPosition worldPosition = new WorldPosition(base.Mission.Scene, UIntPtr.Zero, applyOrderWithTwoPositions.Position1, false);
				WorldPosition worldPosition2 = new WorldPosition(base.Mission.Scene, UIntPtr.Zero, applyOrderWithTwoPositions.Position2, false);
				orderControllerOfPeer.SetOrderWithTwoPositions(applyOrderWithTwoPositions.OrderType, worldPosition, worldPosition2);
			}
			return true;
		}

		// Token: 0x060026C2 RID: 9922 RVA: 0x0008E4C8 File Offset: 0x0008C6C8
		private bool HandleClientEventApplyOrderWithGameEntity(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			IOrderable orderable = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(((ApplyOrderWithMissionObject)baseMessage).MissionObjectId) as IOrderable;
			OrderController orderControllerOfPeer = this.GetOrderControllerOfPeer(networkPeer);
			if (orderControllerOfPeer != null)
			{
				orderControllerOfPeer.SetOrderWithOrderableObject(orderable);
			}
			return true;
		}

		// Token: 0x060026C3 RID: 9923 RVA: 0x0008E500 File Offset: 0x0008C700
		private bool HandleClientEventApplyOrderWithAgent(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			ApplyOrderWithAgent applyOrderWithAgent = (ApplyOrderWithAgent)baseMessage;
			OrderController orderControllerOfPeer = this.GetOrderControllerOfPeer(networkPeer);
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(applyOrderWithAgent.AgentIndex, false);
			if (orderControllerOfPeer != null)
			{
				orderControllerOfPeer.SetOrderWithAgent(applyOrderWithAgent.OrderType, agentFromIndex);
			}
			return true;
		}

		// Token: 0x060026C4 RID: 9924 RVA: 0x0008E53B File Offset: 0x0008C73B
		private bool HandleClientEventSelectAllFormations(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			SelectAllFormations selectAllFormations = (SelectAllFormations)baseMessage;
			OrderController orderControllerOfPeer = this.GetOrderControllerOfPeer(networkPeer);
			if (orderControllerOfPeer != null)
			{
				orderControllerOfPeer.SelectAllFormations(false);
			}
			return true;
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x0008E558 File Offset: 0x0008C758
		private bool HandleClientEventSelectAllSiegeWeapons(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			SelectAllSiegeWeapons selectAllSiegeWeapons = (SelectAllSiegeWeapons)baseMessage;
			OrderController orderControllerOfPeer = this.GetOrderControllerOfPeer(networkPeer);
			if (orderControllerOfPeer != null)
			{
				orderControllerOfPeer.SiegeWeaponController.SelectAll();
			}
			return true;
		}

		// Token: 0x060026C6 RID: 9926 RVA: 0x0008E579 File Offset: 0x0008C779
		private bool HandleClientEventClearSelectedFormations(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			ClearSelectedFormations clearSelectedFormations = (ClearSelectedFormations)baseMessage;
			OrderController orderControllerOfPeer = this.GetOrderControllerOfPeer(networkPeer);
			if (orderControllerOfPeer != null)
			{
				orderControllerOfPeer.ClearSelectedFormations();
			}
			return true;
		}

		// Token: 0x060026C7 RID: 9927 RVA: 0x0008E598 File Offset: 0x0008C798
		private bool HandleClientEventSelectFormation(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			SelectFormation message = (SelectFormation)baseMessage;
			Team teamOfPeer = this.GetTeamOfPeer(networkPeer);
			OrderController orderController = ((teamOfPeer != null) ? teamOfPeer.GetOrderControllerOf(networkPeer.ControlledAgent) : null);
			Formation formation = ((teamOfPeer != null) ? teamOfPeer.FormationsIncludingEmpty.SingleOrDefault<Formation>((Formation f) => f.Index == message.FormationIndex && f.CountOfUnits > 0) : null);
			if (teamOfPeer != null && orderController != null && formation != null)
			{
				orderController.SelectFormation(formation);
			}
			return true;
		}

		// Token: 0x060026C8 RID: 9928 RVA: 0x0008E604 File Offset: 0x0008C804
		private bool HandleClientEventSelectSiegeWeapon(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			SelectSiegeWeapon selectSiegeWeapon = (SelectSiegeWeapon)baseMessage;
			Team teamOfPeer = this.GetTeamOfPeer(networkPeer);
			SiegeWeaponController siegeWeaponController = ((teamOfPeer != null) ? teamOfPeer.GetOrderControllerOf(networkPeer.ControlledAgent).SiegeWeaponController : null);
			SiegeWeapon siegeWeapon = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(selectSiegeWeapon.SiegeWeaponId) as SiegeWeapon;
			if (teamOfPeer != null && siegeWeaponController != null && siegeWeapon != null)
			{
				siegeWeaponController.Select(siegeWeapon);
			}
			return true;
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x0008E65C File Offset: 0x0008C85C
		private bool HandleClientEventUnselectFormation(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			UnselectFormation message = (UnselectFormation)baseMessage;
			Team teamOfPeer = this.GetTeamOfPeer(networkPeer);
			OrderController orderController = ((teamOfPeer != null) ? teamOfPeer.GetOrderControllerOf(networkPeer.ControlledAgent) : null);
			Formation formation = ((teamOfPeer != null) ? teamOfPeer.FormationsIncludingEmpty.SingleOrDefault<Formation>((Formation f) => f.CountOfUnits > 0 && f.Index == message.FormationIndex) : null);
			if (teamOfPeer != null && orderController != null && formation != null)
			{
				orderController.DeselectFormation(formation);
			}
			return true;
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x0008E6C8 File Offset: 0x0008C8C8
		private bool HandleClientEventUnselectSiegeWeapon(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			UnselectSiegeWeapon unselectSiegeWeapon = (UnselectSiegeWeapon)baseMessage;
			Team teamOfPeer = this.GetTeamOfPeer(networkPeer);
			SiegeWeaponController siegeWeaponController = ((teamOfPeer != null) ? teamOfPeer.GetOrderControllerOf(networkPeer.ControlledAgent).SiegeWeaponController : null);
			SiegeWeapon siegeWeapon = Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(unselectSiegeWeapon.SiegeWeaponId) as SiegeWeapon;
			if (teamOfPeer != null && siegeWeaponController != null && siegeWeapon != null)
			{
				siegeWeaponController.Deselect(siegeWeapon);
			}
			return true;
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x0008E720 File Offset: 0x0008C920
		private bool HandleClientEventDropWeapon(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			DropWeapon dropWeapon = (DropWeapon)baseMessage;
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			if (((component != null) ? component.ControlledAgent : null) != null && component.ControlledAgent.IsActive())
			{
				component.ControlledAgent.HandleDropWeapon(dropWeapon.IsDefendPressed, dropWeapon.ForcedSlotIndexToDropWeaponFrom);
			}
			return true;
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x0008E770 File Offset: 0x0008C970
		private bool HandleClientEventCheerSelected(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			TauntSelected tauntSelected = (TauntSelected)baseMessage;
			bool flag = false;
			if (networkPeer.ControlledAgent != null)
			{
				networkPeer.ControlledAgent.HandleTaunt(tauntSelected.IndexOfTaunt, false);
				flag = true;
			}
			return flag;
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x0008E7A4 File Offset: 0x0008C9A4
		private bool HandleClientEventBarkSelected(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			BarkSelected barkSelected = (BarkSelected)baseMessage;
			bool flag = false;
			if (networkPeer.ControlledAgent != null)
			{
				networkPeer.ControlledAgent.HandleBark(barkSelected.IndexOfBark);
				flag = true;
			}
			return flag;
		}

		// Token: 0x060026CE RID: 9934 RVA: 0x0008E7D6 File Offset: 0x0008C9D6
		private bool HandleClientEventBreakAgentVisualsInvulnerability(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			AgentVisualsBreakInvulnerability agentVisualsBreakInvulnerability = (AgentVisualsBreakInvulnerability)baseMessage;
			if (base.Mission == null || base.Mission.GetMissionBehavior<SpawnComponent>() == null || networkPeer.GetComponent<MissionPeer>() == null)
			{
				return false;
			}
			base.Mission.GetMissionBehavior<SpawnComponent>().SetEarlyAgentVisualsDespawning(networkPeer.GetComponent<MissionPeer>(), true);
			return true;
		}

		// Token: 0x060026CF RID: 9935 RVA: 0x0008E818 File Offset: 0x0008CA18
		private bool HandleClientEventRequestToSpawnAsBot(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			RequestToSpawnAsBot requestToSpawnAsBot = (RequestToSpawnAsBot)baseMessage;
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			if (component == null)
			{
				return false;
			}
			if (networkPeer.IsSpectator || component.Team == null || component.Team == base.Mission.SpectatorTeam)
			{
				return true;
			}
			if (component.HasSpawnTimerExpired)
			{
				component.WantsToSpawnAsBot = true;
			}
			return true;
		}

		// Token: 0x060026D0 RID: 9936 RVA: 0x0008E870 File Offset: 0x0008CA70
		private void SendExistingObjectsToPeer(NetworkCommunicator networkPeer)
		{
			MBDebug.Print(string.Concat(new object[] { "Sending all existing objects to peer: ", networkPeer.UserName, " with index: ", networkPeer.Index }), 0, Debug.DebugColor.White, 17179869184UL);
			GameNetwork.BeginModuleEventAsServer(networkPeer);
			GameNetwork.WriteMessage(new ExistingObjectsBegin());
			GameNetwork.EndModuleEventAsServer();
			GameNetwork.BeginModuleEventAsServer(networkPeer);
			GameNetwork.WriteMessage(new SynchronizeMissionTimeTracker((float)MissionTime.Now.ToSeconds));
			GameNetwork.EndModuleEventAsServer();
			this.SendTeamsToPeer(networkPeer);
			this.SendTeamRelationsToPeer(networkPeer);
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeersIncludingDisconnectedPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (component != null)
				{
					if (component.Team != null)
					{
						GameNetwork.BeginModuleEventAsServer(networkPeer);
						GameNetwork.WriteMessage(new SetPeerTeam(networkCommunicator, component.Team.TeamIndex));
						GameNetwork.EndModuleEventAsServer();
					}
					if (component.Culture != null)
					{
						GameNetwork.BeginModuleEventAsServer(networkPeer);
						GameNetwork.WriteMessage(new ChangeCulture(component, component.Culture));
						GameNetwork.EndModuleEventAsServer();
					}
				}
			}
			this.SendFormationInformation(networkPeer);
			this.SendAgentsToPeer(networkPeer);
			this.SendSpawnedMissionObjectsToPeer(networkPeer);
			this.SynchronizeMissionObjectsToPeer(networkPeer);
			this.SendMissilesToPeer(networkPeer);
			this.SendTroopSelectionInformation(networkPeer);
			networkPeer.SendExistingObjects(base.Mission);
			GameNetwork.BeginModuleEventAsServer(networkPeer);
			GameNetwork.WriteMessage(new ExistingObjectsEnd());
			GameNetwork.EndModuleEventAsServer();
		}

		// Token: 0x060026D1 RID: 9937 RVA: 0x0008E9E0 File Offset: 0x0008CBE0
		private void SendTroopSelectionInformation(NetworkCommunicator networkPeer)
		{
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeersIncludingDisconnectedPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (component != null && component.SelectedTroopIndex != 0)
				{
					GameNetwork.BeginModuleEventAsServer(networkPeer);
					GameNetwork.WriteMessage(new UpdateSelectedTroopIndex(networkCommunicator, component.SelectedTroopIndex));
					GameNetwork.EndModuleEventAsServer();
				}
			}
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x0008EA54 File Offset: 0x0008CC54
		private void SendTeamsToPeer(NetworkCommunicator networkPeer)
		{
			foreach (Team team in base.Mission.Teams)
			{
				MBDebug.Print(string.Concat(new object[] { "Syncing a team to peer: ", networkPeer.UserName, " with index: ", networkPeer.Index }), 0, Debug.DebugColor.White, 17179869184UL);
				GameNetwork.BeginModuleEventAsServer(networkPeer);
				GameNetwork.WriteMessage(new AddTeam(team.TeamIndex, team.Side, team.Color, team.Color2, (team.Banner != null) ? team.Banner.BannerCode : string.Empty, team.IsPlayerGeneral, team.IsPlayerSergeant, team == base.Mission.Teams.Spectator));
				GameNetwork.EndModuleEventAsServer();
			}
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x0008EB54 File Offset: 0x0008CD54
		private void SendTeamRelationsToPeer(NetworkCommunicator networkPeer)
		{
			int count = base.Mission.Teams.Count;
			for (int i = 0; i < count; i++)
			{
				for (int j = i; j < count; j++)
				{
					Team team = base.Mission.Teams[i];
					Team team2 = base.Mission.Teams[j];
					if (team.IsEnemyOf(team2))
					{
						GameNetwork.BeginModuleEventAsServer(networkPeer);
						GameNetwork.WriteMessage(new TeamSetIsEnemyOf(team.TeamIndex, team2.TeamIndex, true));
						GameNetwork.EndModuleEventAsServer();
					}
				}
			}
		}

		// Token: 0x060026D4 RID: 9940 RVA: 0x0008EBDC File Offset: 0x0008CDDC
		private void SendFormationInformation(NetworkCommunicator networkPeer)
		{
			MBDebug.Print("formations sending begin-", 0, Debug.DebugColor.White, 17179869184UL);
			foreach (Team team in base.Mission.Teams)
			{
				if (team.IsValid && team.Side != BattleSideEnum.None)
				{
					foreach (Formation formation in team.FormationsIncludingEmpty)
					{
						if (!string.IsNullOrEmpty(formation.BannerCode))
						{
							GameNetwork.BeginModuleEventAsServer(networkPeer);
							GameNetwork.WriteMessage(new InitializeFormation(formation, team.TeamIndex, formation.BannerCode));
							GameNetwork.EndModuleEventAsServer();
						}
					}
				}
			}
			if (!networkPeer.IsServerPeer)
			{
				GameNetwork.BeginModuleEventAsServer(networkPeer);
				GameNetwork.WriteMessage(new SetSpawnedFormationCount(base.Mission.NumOfFormationsSpawnedTeamOne, base.Mission.NumOfFormationsSpawnedTeamTwo));
				GameNetwork.EndModuleEventAsServer();
			}
			MBDebug.Print("formations sending end-", 0, Debug.DebugColor.White, 17179869184UL);
		}

		// Token: 0x060026D5 RID: 9941 RVA: 0x0008ED08 File Offset: 0x0008CF08
		private void SendAgentVisualsToPeer(NetworkCommunicator networkPeer, Team peerTeam)
		{
			MBDebug.Print("agentvisuals sending begin-", 0, Debug.DebugColor.White, 17179869184UL);
			foreach (MissionPeer missionPeer in from p in GameNetwork.NetworkPeers
				select p.GetComponent<MissionPeer>() into pr
				where pr != null
				select pr)
			{
				if (missionPeer.Team == peerTeam)
				{
					int amountOfAgentVisualsForPeer = missionPeer.GetAmountOfAgentVisualsForPeer();
					for (int i = 0; i < amountOfAgentVisualsForPeer; i++)
					{
						PeerVisualsHolder visuals = missionPeer.GetVisuals(i);
						IAgentVisual agentVisuals = visuals.AgentVisuals;
						MatrixFrame frame = agentVisuals.GetFrame();
						AgentBuildData agentBuildData = new AgentBuildData(MBObjectManager.Instance.GetObject<BasicCharacterObject>(agentVisuals.GetCharacterObjectID())).MissionPeer(missionPeer).Equipment(agentVisuals.GetEquipment()).VisualsIndex(visuals.VisualsIndex)
							.Team(missionPeer.Team)
							.InitialPosition(in frame.origin);
						Vec2 vec = frame.rotation.f.AsVec2;
						vec = vec.Normalized();
						AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in vec).IsFemale(agentVisuals.GetIsFemale()).BodyProperties(agentVisuals.GetBodyProperties());
						networkPeer.GetComponent<MissionPeer>();
						GameNetwork.BeginModuleEventAsServer(networkPeer);
						GameNetwork.WriteMessage(new CreateAgentVisuals(missionPeer.GetNetworkPeer(), agentBuildData2, missionPeer.SelectedTroopIndex, 0));
						GameNetwork.EndModuleEventAsServer();
					}
				}
			}
			MBDebug.Print("agentvisuals sending end-", 0, Debug.DebugColor.White, 17179869184UL);
		}

		// Token: 0x060026D6 RID: 9942 RVA: 0x0008EEC8 File Offset: 0x0008D0C8
		private void SendAgentsToPeer(NetworkCommunicator networkPeer)
		{
			MBDebug.Print("agents sending begin-", 0, Debug.DebugColor.White, 17179869184UL);
			using (List<Agent>.Enumerator enumerator = base.Mission.AllAgents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Agent agent = enumerator.Current;
					bool isMount = agent.IsMount;
					bool flag = agent.IsAddedAsCorpse();
					AgentState state = agent.State;
					if (!flag && (state == AgentState.Active || ((state == AgentState.Killed || state == AgentState.Unconscious) && (agent.GetAttachedWeaponsCount() > 0 || (!isMount && (agent.GetPrimaryWieldedItemIndex() >= EquipmentIndex.WeaponItemBeginSlot || agent.GetOffhandWieldedItemIndex() >= EquipmentIndex.WeaponItemBeginSlot)) || base.Mission.IsAgentInProximityMap(agent))) || base.Mission.MissilesList.Any<Mission.Missile>((Mission.Missile m) => m.ShooterAgent == agent)))
					{
						if (isMount && agent.RiderAgent == null)
						{
							MBDebug.Print("mount sending " + agent.Index, 0, Debug.DebugColor.White, 17179869184UL);
							GameNetwork.BeginModuleEventAsServer(networkPeer);
							GameNetwork.WriteMessage(new CreateFreeMountAgent(agent, agent.Position, agent.GetMovementDirection()));
							GameNetwork.EndModuleEventAsServer();
							agent.LockAgentReplicationTableDataWithCurrentReliableSequenceNo(networkPeer);
							int attachedWeaponsCount = agent.GetAttachedWeaponsCount();
							for (int i = 0; i < attachedWeaponsCount; i++)
							{
								GameNetwork.BeginModuleEventAsServer(networkPeer);
								GameNetwork.WriteMessage(new AttachWeaponToAgent(agent.GetAttachedWeapon(i), agent.Index, agent.GetAttachedWeaponBoneIndex(i), agent.GetAttachedWeaponFrame(i)));
								GameNetwork.EndModuleEventAsServer();
							}
							if (!agent.IsActive())
							{
								GameNetwork.BeginModuleEventAsServer(networkPeer);
								GameNetwork.WriteMessage(new MakeAgentDead(agent.Index, state == AgentState.Killed, agent.GetCurrentAction(0), -1));
								GameNetwork.EndModuleEventAsServer();
							}
						}
						else if (!isMount)
						{
							MBDebug.Print("human sending " + agent.Index, 0, Debug.DebugColor.White, 17179869184UL);
							Agent agent2 = agent.MountAgent;
							if (agent2 != null && agent2.RiderAgent == null)
							{
								agent2 = null;
							}
							GameNetwork.BeginModuleEventAsServer(networkPeer);
							int index = agent.Index;
							BasicCharacterObject character = agent.Character;
							Monster monster = agent.Monster;
							Equipment spawnEquipment = agent.SpawnEquipment;
							MissionEquipment equipment = agent.Equipment;
							BodyProperties bodyPropertiesValue = agent.BodyPropertiesValue;
							int bodyPropertiesSeed = agent.BodyPropertiesSeed;
							bool isFemale = agent.IsFemale;
							Team team = agent.Team;
							int num = ((team != null) ? team.TeamIndex : (-1));
							Formation formation = agent.Formation;
							int num2 = ((formation != null) ? formation.Index : (-1));
							uint clothingColor = agent.ClothingColor1;
							uint clothingColor2 = agent.ClothingColor2;
							int num3 = ((agent2 != null) ? agent2.Index : (-1));
							Agent mountAgent = agent.MountAgent;
							Equipment equipment2 = ((mountAgent != null) ? mountAgent.SpawnEquipment : null);
							bool flag2 = agent.MissionPeer != null && agent.OwningAgentMissionPeer == null;
							Vec3 position = agent.Position;
							Vec2 movementDirection = agent.GetMovementDirection();
							MissionPeer missionPeer = agent.MissionPeer;
							NetworkCommunicator networkCommunicator;
							if ((networkCommunicator = ((missionPeer != null) ? missionPeer.GetNetworkPeer() : null)) == null)
							{
								MissionPeer owningAgentMissionPeer = agent.OwningAgentMissionPeer;
								networkCommunicator = ((owningAgentMissionPeer != null) ? owningAgentMissionPeer.GetNetworkPeer() : null);
							}
							GameNetwork.WriteMessage(new CreateAgent(index, character, monster, spawnEquipment, equipment, bodyPropertiesValue, bodyPropertiesSeed, isFemale, num, num2, clothingColor, clothingColor2, num3, equipment2, flag2, position, movementDirection, networkCommunicator));
							GameNetwork.EndModuleEventAsServer();
							agent.LockAgentReplicationTableDataWithCurrentReliableSequenceNo(networkPeer);
							if (agent2 != null)
							{
								agent2.LockAgentReplicationTableDataWithCurrentReliableSequenceNo(networkPeer);
							}
							for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
							{
								for (int j = 0; j < agent.Equipment[equipmentIndex].GetAttachedWeaponsCount(); j++)
								{
									GameNetwork.BeginModuleEventAsServer(networkPeer);
									GameNetwork.WriteMessage(new AttachWeaponToWeaponInAgentEquipmentSlot(agent.Equipment[equipmentIndex].GetAttachedWeapon(j), agent.Index, equipmentIndex, agent.Equipment[equipmentIndex].GetAttachedWeaponFrame(j)));
									GameNetwork.EndModuleEventAsServer();
								}
							}
							int num4 = agent.GetAttachedWeaponsCount();
							for (int k = 0; k < num4; k++)
							{
								GameNetwork.BeginModuleEventAsServer(networkPeer);
								GameNetwork.WriteMessage(new AttachWeaponToAgent(agent.GetAttachedWeapon(k), agent.Index, agent.GetAttachedWeaponBoneIndex(k), agent.GetAttachedWeaponFrame(k)));
								GameNetwork.EndModuleEventAsServer();
							}
							if (agent2 != null)
							{
								num4 = agent2.GetAttachedWeaponsCount();
								for (int l = 0; l < num4; l++)
								{
									GameNetwork.BeginModuleEventAsServer(networkPeer);
									GameNetwork.WriteMessage(new AttachWeaponToAgent(agent2.GetAttachedWeapon(l), agent2.Index, agent2.GetAttachedWeaponBoneIndex(l), agent2.GetAttachedWeaponFrame(l)));
									GameNetwork.EndModuleEventAsServer();
								}
							}
							EquipmentIndex primaryWieldedItemIndex = agent.GetPrimaryWieldedItemIndex();
							int num5 = ((primaryWieldedItemIndex != EquipmentIndex.None) ? agent.Equipment[primaryWieldedItemIndex].CurrentUsageIndex : 0);
							GameNetwork.BeginModuleEventAsServer(networkPeer);
							GameNetwork.WriteMessage(new SetWieldedItemIndex(agent.Index, false, true, true, primaryWieldedItemIndex, num5));
							GameNetwork.EndModuleEventAsServer();
							GameNetwork.BeginModuleEventAsServer(networkPeer);
							GameNetwork.WriteMessage(new SetWieldedItemIndex(agent.Index, true, true, true, agent.GetOffhandWieldedItemIndex(), num5));
							GameNetwork.EndModuleEventAsServer();
							MBActionSet mbactionSet = agent.ActionSet;
							if (mbactionSet.IsValid)
							{
								AnimationSystemData animationSystemData = agent.Monster.FillAnimationSystemData(mbactionSet, agent.Character.GetStepSize(), false);
								GameNetwork.BeginModuleEventAsServer(networkPeer);
								GameNetwork.WriteMessage(new SetAgentActionSet(agent.Index, animationSystemData));
								GameNetwork.EndModuleEventAsServer();
								if (!agent.IsActive())
								{
									GameNetwork.BeginModuleEventAsServer(networkPeer);
									GameNetwork.WriteMessage(new MakeAgentDead(agent.Index, state == AgentState.Killed, agent.GetCurrentAction(0), -1));
									GameNetwork.EndModuleEventAsServer();
								}
							}
							else
							{
								mbactionSet = MBActionSet.GetActionSet("as_human_warrior");
								AnimationSystemData animationSystemData2 = agent.Monster.FillAnimationSystemData(mbactionSet, agent.Character.GetStepSize(), false);
								GameNetwork.BeginModuleEventAsServer(networkPeer);
								GameNetwork.WriteMessage(new SetAgentActionSet(agent.Index, animationSystemData2));
								GameNetwork.EndModuleEventAsServer();
								GameNetwork.BeginModuleEventAsServer(networkPeer);
								GameNetwork.WriteMessage(new MakeAgentDead(agent.Index, state == AgentState.Killed, ActionIndexCache.act_death_by_arrow_pelvis, -1));
								GameNetwork.EndModuleEventAsServer();
							}
						}
						else
						{
							MBDebug.Print("agent not sending " + agent.Index, 0, Debug.DebugColor.White, 17179869184UL);
						}
					}
				}
			}
			MBDebug.Print("agents sending end-", 0, Debug.DebugColor.White, 17179869184UL);
		}

		// Token: 0x060026D7 RID: 9943 RVA: 0x0008F5F0 File Offset: 0x0008D7F0
		private void SendSpawnedMissionObjectsToPeer(NetworkCommunicator networkPeer)
		{
			using (List<MissionObject>.Enumerator enumerator = base.Mission.MissionObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MissionObject missionObject = enumerator.Current;
					SpawnedItemEntity spawnedItemEntity;
					if ((spawnedItemEntity = missionObject as SpawnedItemEntity) != null)
					{
						WeakGameEntity gameEntity = spawnedItemEntity.GameEntity;
						if (!gameEntity.Parent.IsValid || !gameEntity.Parent.HasScriptOfType<SpawnedItemEntity>())
						{
							MissionObject missionObject2 = null;
							if (spawnedItemEntity.GameEntity.Parent.IsValid)
							{
								missionObject2 = gameEntity.Parent.GetFirstScriptOfType<MissionObject>();
							}
							MatrixFrame matrixFrame = gameEntity.GetGlobalFrame();
							if (missionObject2 != null)
							{
								matrixFrame = missionObject2.GameEntity.GetGlobalFrame().TransformToLocalNonOrthogonal(in matrixFrame);
							}
							matrixFrame.origin.z = MathF.Max(matrixFrame.origin.z, CompressionBasic.PositionCompressionInfo.GetMinimumValue() + 1f);
							Mission.WeaponSpawnFlags weaponSpawnFlags = spawnedItemEntity.SpawnFlags;
							if (weaponSpawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithPhysics) && !gameEntity.GetPhysicsState())
							{
								weaponSpawnFlags = (weaponSpawnFlags & ~Mission.WeaponSpawnFlags.WithPhysics) | Mission.WeaponSpawnFlags.WithStaticPhysics;
							}
							bool flag = true;
							bool flag2 = !spawnedItemEntity.SpawnedOnACorpse && (!gameEntity.Parent.IsValid || missionObject2 != null);
							GameNetwork.BeginModuleEventAsServer(networkPeer);
							GameNetwork.WriteMessage(new SpawnWeaponWithNewEntity(spawnedItemEntity.WeaponCopy, weaponSpawnFlags, spawnedItemEntity.Id.Id, matrixFrame, (missionObject2 != null) ? missionObject2.Id : MissionObjectId.Invalid, flag2, flag, spawnedItemEntity.SpawnedOnACorpse));
							GameNetwork.EndModuleEventAsServer();
							for (int i = 0; i < spawnedItemEntity.WeaponCopy.GetAttachedWeaponsCount(); i++)
							{
								GameNetwork.BeginModuleEventAsServer(networkPeer);
								GameNetwork.WriteMessage(new AttachWeaponToSpawnedWeapon(spawnedItemEntity.WeaponCopy.GetAttachedWeapon(i), spawnedItemEntity.Id, spawnedItemEntity.WeaponCopy.GetAttachedWeaponFrame(i)));
								GameNetwork.EndModuleEventAsServer();
								if (spawnedItemEntity.WeaponCopy.GetAttachedWeapon(i).Item.ItemFlags.HasAnyFlag(ItemFlags.CanBePickedUpFromCorpse))
								{
									if (!gameEntity.GetChild(i).IsValid)
									{
										Debug.Print(string.Concat(new object[]
										{
											"spawnedItemGameEntity child is null. item: ",
											spawnedItemEntity.WeaponCopy.Item.StringId,
											" attached item: ",
											spawnedItemEntity.WeaponCopy.GetAttachedWeapon(i).Item.StringId,
											" attachment index: ",
											i
										}), 0, Debug.DebugColor.White, 17592186044416UL);
									}
									else if (gameEntity.GetChild(i).GetFirstScriptOfType<SpawnedItemEntity>() == null)
									{
										Debug.Print(string.Concat(new object[]
										{
											"spawnedItemGameEntity child SpawnedItemEntity script is null. item: ",
											spawnedItemEntity.WeaponCopy.Item.StringId,
											" attached item: ",
											spawnedItemEntity.WeaponCopy.GetAttachedWeapon(i).Item.StringId,
											" attachment index: ",
											i
										}), 0, Debug.DebugColor.White, 17592186044416UL);
									}
									GameNetwork.BeginModuleEventAsServer(networkPeer);
									GameNetwork.WriteMessage(new SpawnAttachedWeaponOnSpawnedWeapon(spawnedItemEntity.Id, i, gameEntity.GetChild(i).GetFirstScriptOfType<SpawnedItemEntity>().Id.Id));
									GameNetwork.EndModuleEventAsServer();
								}
							}
						}
					}
					else if (missionObject.CreatedAtRuntime)
					{
						Mission.DynamicallyCreatedEntity dynamicallyCreatedEntity = base.Mission.AddedEntitiesInfo.SingleOrDefault<Mission.DynamicallyCreatedEntity>((Mission.DynamicallyCreatedEntity x) => x.ObjectId == missionObject.Id);
						if (dynamicallyCreatedEntity != null)
						{
							GameNetwork.BeginModuleEventAsServer(networkPeer);
							GameNetwork.WriteMessage(new CreateMissionObject(dynamicallyCreatedEntity.ObjectId, dynamicallyCreatedEntity.Prefab, dynamicallyCreatedEntity.Frame, dynamicallyCreatedEntity.ChildObjectIds));
							GameNetwork.EndModuleEventAsServer();
						}
					}
				}
			}
		}

		// Token: 0x060026D8 RID: 9944 RVA: 0x0008FA00 File Offset: 0x0008DC00
		private void SynchronizeMissionObjectsToPeer(NetworkCommunicator networkPeer)
		{
			using (List<MissionObject>.Enumerator enumerator = base.Mission.MissionObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SynchedMissionObject synchedMissionObject;
					if ((synchedMissionObject = enumerator.Current as SynchedMissionObject) != null)
					{
						GameNetwork.BeginModuleEventAsServer(networkPeer);
						GameNetwork.WriteMessage(new SynchronizeMissionObject(synchedMissionObject));
						GameNetwork.EndModuleEventAsServer();
					}
				}
			}
		}

		// Token: 0x060026D9 RID: 9945 RVA: 0x0008FA70 File Offset: 0x0008DC70
		private void SendMissilesToPeer(NetworkCommunicator networkPeer)
		{
			foreach (Mission.Missile missile in base.Mission.MissilesList)
			{
				Vec3 velocity = missile.GetVelocity();
				float num = velocity.Normalize();
				Mat3 identity = Mat3.Identity;
				identity.f = velocity;
				identity.Orthonormalize();
				GameNetwork.BeginModuleEventAsServer(networkPeer);
				int index = missile.Index;
				int index2 = missile.ShooterAgent.Index;
				EquipmentIndex equipmentIndex = EquipmentIndex.None;
				MissionWeapon weapon = missile.Weapon;
				Vec3 position = missile.GetPosition();
				Vec3 vec = velocity;
				float num2 = num;
				Mat3 mat = identity;
				bool hasRigidBody = missile.GetHasRigidBody();
				MissionObject missionObjectToIgnore = missile.MissionObjectToIgnore;
				GameNetwork.WriteMessage(new CreateMissile(index, index2, equipmentIndex, weapon, position, vec, num2, mat, hasRigidBody, (missionObjectToIgnore != null) ? missionObjectToIgnore.Id : MissionObjectId.Invalid, false));
				GameNetwork.EndModuleEventAsServer();
			}
		}

		// Token: 0x060026DA RID: 9946 RVA: 0x0008FB40 File Offset: 0x0008DD40
		public override void OnPlayerDisconnectedFromServer(NetworkCommunicator networkPeer)
		{
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			if (component != null && component.HasSpawnedAgentVisuals)
			{
				base.Mission.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>().RemoveAgentVisuals(component, false);
				component.HasSpawnedAgentVisuals = false;
			}
		}

		// Token: 0x060026DB RID: 9947 RVA: 0x0008FB78 File Offset: 0x0008DD78
		protected override void HandleEarlyNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
			if (!networkPeer.IsServerPeer)
			{
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					if (networkCommunicator.IsSynchronized || networkCommunicator.JustReconnecting)
					{
						networkCommunicator.VirtualPlayer.SynchronizeComponentsTo(networkPeer.VirtualPlayer);
					}
				}
				foreach (NetworkCommunicator networkCommunicator2 in GameNetwork.DisconnectedNetworkPeers)
				{
					networkCommunicator2.VirtualPlayer.SynchronizeComponentsTo(networkPeer.VirtualPlayer);
				}
			}
			MissionPeer missionPeer = networkPeer.AddComponent<MissionPeer>();
			if (networkPeer.JustReconnecting && missionPeer.Team != null)
			{
				MBAPI.IMBPeer.SetTeam(networkPeer.Index, missionPeer.Team.MBTeam.Index);
			}
			missionPeer.JoinTime = DateTime.Now;
		}

		// Token: 0x060026DC RID: 9948 RVA: 0x0008FC7C File Offset: 0x0008DE7C
		protected override void HandleLateNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
			if (!networkPeer.IsServerPeer)
			{
				this.SendExistingObjectsToPeer(networkPeer);
			}
		}

		// Token: 0x060026DD RID: 9949 RVA: 0x0008FC90 File Offset: 0x0008DE90
		protected override void HandleEarlyPlayerDisconnect(NetworkCommunicator networkPeer)
		{
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			if (component != null)
			{
				Mission mission = base.Mission;
				if (mission != null)
				{
					mission.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>().RemoveAgentVisuals(component, true);
				}
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new RemoveAgentVisualsForPeer(component.GetNetworkPeer()));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				}
				component.HasSpawnedAgentVisuals = false;
			}
		}

		// Token: 0x060026DE RID: 9950 RVA: 0x0008FCE9 File Offset: 0x0008DEE9
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
		}

		// Token: 0x060026DF RID: 9951 RVA: 0x0008FCF4 File Offset: 0x0008DEF4
		protected override void HandlePlayerDisconnect(NetworkCommunicator networkPeer)
		{
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			if (component != null)
			{
				if (component.ControlledAgent != null)
				{
					Agent controlledAgent = component.ControlledAgent;
					Blow blow = new Blow(controlledAgent.Index);
					blow.WeaponRecord = default(BlowWeaponRecord);
					blow.DamageType = DamageTypes.Invalid;
					blow.BaseMagnitude = 10000f;
					blow.WeaponRecord.WeaponClass = WeaponClass.Undefined;
					blow.GlobalPosition = controlledAgent.Position;
					blow.DamagedPercentage = 1f;
					controlledAgent.Die(blow, Agent.KillInfo.Invalid);
				}
				if (base.Mission.AllAgents != null)
				{
					foreach (Agent agent in base.Mission.AllAgents)
					{
						if (agent.MissionPeer == component)
						{
							agent.MissionPeer = null;
						}
						if (agent.OwningAgentMissionPeer == component)
						{
							agent.SetOwningAgentMissionPeer(null);
						}
					}
				}
				if (component.ControlledFormation != null)
				{
					component.ControlledFormation.PlayerOwner = null;
				}
			}
		}

		// Token: 0x060026E0 RID: 9952 RVA: 0x0008FE04 File Offset: 0x0008E004
		public override void OnAddTeam(Team team)
		{
			base.OnAddTeam(team);
			if (GameNetwork.IsServerOrRecorder)
			{
				MBDebug.Print("----------OnAddTeam-", 0, Debug.DebugColor.White, 17592186044416UL);
				MBDebug.Print("Adding a team and sending it to all clients", 0, Debug.DebugColor.White, 17179869184UL);
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new AddTeam(team.TeamIndex, team.Side, team.Color, team.Color2, (team.Banner != null) ? team.Banner.BannerCode : string.Empty, team.IsPlayerGeneral, team.IsPlayerSergeant, team == base.Mission.Teams.Spectator));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
		}

		// Token: 0x060026E1 RID: 9953 RVA: 0x0008FEB6 File Offset: 0x0008E0B6
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._chatBox = Game.Current.GetGameHandler<ChatBox>();
		}

		// Token: 0x060026E2 RID: 9954 RVA: 0x0008FECE File Offset: 0x0008E0CE
		public override void OnClearScene()
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				MBDebug.Print("I am clearing the scene, and sending this message to all clients", 0, Debug.DebugColor.White, 17179869184UL);
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new ClearMission());
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
		}

		// Token: 0x060026E3 RID: 9955 RVA: 0x0008FF04 File Offset: 0x0008E104
		public override void OnMissionTick(float dt)
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				this._accumulatedTimeSinceLastTimerSync += dt;
				if (this._accumulatedTimeSinceLastTimerSync > 2f)
				{
					this._accumulatedTimeSinceLastTimerSync -= 2f;
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SynchronizeMissionTimeTracker((float)MissionTime.Now.ToSeconds));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				}
			}
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionRepresentativeBase component = networkCommunicator.GetComponent<MissionRepresentativeBase>();
				if (component != null)
				{
					component.Tick(dt);
				}
				if (GameNetwork.IsServer && !networkCommunicator.IsServerPeer && !MultiplayerOptions.OptionType.DisableInactivityKick.GetBoolValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions))
				{
					MissionPeer component2 = networkCommunicator.GetComponent<MissionPeer>();
					if (component2 != null)
					{
						component2.TickInactivityStatus();
					}
				}
			}
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x0008FFE4 File Offset: 0x0008E1E4
		protected override void OnEndMission()
		{
			if (GameNetwork.IsServer)
			{
				foreach (MissionPeer missionPeer in VirtualPlayer.Peers<MissionPeer>())
				{
					missionPeer.ControlledAgent = null;
				}
				foreach (Agent agent in base.Mission.AllAgents)
				{
					agent.MissionPeer = null;
				}
			}
			base.OnEndMission();
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x00090088 File Offset: 0x0008E288
		public void OnPeerSelectedTeam(MissionPeer missionPeer)
		{
			this.SendAgentVisualsToPeer(missionPeer.GetNetworkPeer(), missionPeer.Team);
		}

		// Token: 0x060026E6 RID: 9958 RVA: 0x0009009C File Offset: 0x0008E29C
		public void OnClientSynchronized(NetworkCommunicator networkPeer)
		{
			Action<NetworkCommunicator> onClientSynchronizedEvent = this.OnClientSynchronizedEvent;
			if (onClientSynchronizedEvent != null)
			{
				onClientSynchronizedEvent(networkPeer);
			}
			if (networkPeer.IsMine)
			{
				Action onMyClientSynchronized = this.OnMyClientSynchronized;
				if (onMyClientSynchronized == null)
				{
					return;
				}
				onMyClientSynchronized();
			}
		}

		// Token: 0x04000EC2 RID: 3778
		private float _accumulatedTimeSinceLastTimerSync;

		// Token: 0x04000EC3 RID: 3779
		private const float TimerSyncPeriod = 2f;

		// Token: 0x04000EC4 RID: 3780
		private ChatBox _chatBox;
	}
}
