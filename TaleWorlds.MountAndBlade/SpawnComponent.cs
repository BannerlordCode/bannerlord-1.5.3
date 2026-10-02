using System;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002CB RID: 715
	public class SpawnComponent : MissionLogic
	{
		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06002949 RID: 10569 RVA: 0x0009D1D7 File Offset: 0x0009B3D7
		// (set) Token: 0x0600294A RID: 10570 RVA: 0x0009D1DF File Offset: 0x0009B3DF
		public SpawnFrameBehaviorBase SpawnFrameBehavior { get; private set; }

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x0600294B RID: 10571 RVA: 0x0009D1E8 File Offset: 0x0009B3E8
		// (set) Token: 0x0600294C RID: 10572 RVA: 0x0009D1F0 File Offset: 0x0009B3F0
		public SpawningBehaviorBase SpawningBehavior { get; private set; }

		// Token: 0x0600294D RID: 10573 RVA: 0x0009D1F9 File Offset: 0x0009B3F9
		public SpawnComponent(SpawnFrameBehaviorBase spawnFrameBehavior, SpawningBehaviorBase spawningBehavior)
		{
			this.SpawnFrameBehavior = spawnFrameBehavior;
			this.SpawningBehavior = spawningBehavior;
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x0009D20F File Offset: 0x0009B40F
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._missionMultiplayerGameModeBase = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBase>();
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x0009D228 File Offset: 0x0009B428
		public bool AreAgentsSpawning()
		{
			return this.SpawningBehavior.AreAgentsSpawning();
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x0009D235 File Offset: 0x0009B435
		public void SetNewSpawnFrameBehavior(SpawnFrameBehaviorBase spawnFrameBehavior)
		{
			this.SpawnFrameBehavior = spawnFrameBehavior;
			if (this.SpawnFrameBehavior != null)
			{
				this.SpawnFrameBehavior.Initialize();
			}
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x0009D251 File Offset: 0x0009B451
		public void SetNewSpawningBehavior(SpawningBehaviorBase spawningBehavior)
		{
			this.SpawningBehavior = spawningBehavior;
			if (this.SpawningBehavior != null)
			{
				this.SpawningBehavior.Initialize(this);
			}
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x0009D26E File Offset: 0x0009B46E
		protected override void OnEndMission()
		{
			base.OnEndMission();
			this.SpawningBehavior.Clear();
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x0009D281 File Offset: 0x0009B481
		public static void SetSiegeSpawningBehavior()
		{
			Mission.Current.GetMissionBehavior<SpawnComponent>().SetNewSpawnFrameBehavior(new SiegeSpawnFrameBehavior());
			Mission.Current.GetMissionBehavior<SpawnComponent>().SetNewSpawningBehavior(new SiegeSpawningBehavior());
		}

		// Token: 0x06002954 RID: 10580 RVA: 0x0009D2AB File Offset: 0x0009B4AB
		public static void SetFlagDominationSpawningBehavior()
		{
			Mission.Current.GetMissionBehavior<SpawnComponent>().SetNewSpawnFrameBehavior(new FlagDominationSpawnFrameBehavior());
			Mission.Current.GetMissionBehavior<SpawnComponent>().SetNewSpawningBehavior(new FlagDominationSpawningBehavior());
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x0009D2D5 File Offset: 0x0009B4D5
		public static void SetWarmupSpawningBehavior()
		{
			Mission.Current.GetMissionBehavior<SpawnComponent>().SetNewSpawnFrameBehavior(new FFASpawnFrameBehavior());
			Mission.Current.GetMissionBehavior<SpawnComponent>().SetNewSpawningBehavior(new WarmupSpawningBehavior());
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x0009D2FF File Offset: 0x0009B4FF
		public static void SetSpawningBehaviorForCurrentGameType(MultiplayerGameType currentGameType)
		{
			if (currentGameType == MultiplayerGameType.Siege)
			{
				SpawnComponent.SetSiegeSpawningBehavior();
				return;
			}
			if (currentGameType - MultiplayerGameType.Battle > 2)
			{
				return;
			}
			SpawnComponent.SetFlagDominationSpawningBehavior();
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x0009D317 File Offset: 0x0009B517
		public override void AfterStart()
		{
			base.AfterStart();
			this.SetNewSpawnFrameBehavior(this.SpawnFrameBehavior);
			this.SetNewSpawningBehavior(this.SpawningBehavior);
			base.Mission.OnInitialSpawnCompleted(BattleSideEnum.None);
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x0009D343 File Offset: 0x0009B543
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			this.SpawningBehavior.OnTick(dt);
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x0009D358 File Offset: 0x0009B558
		protected void StartSpawnSession()
		{
			this.SpawningBehavior.RequestStartSpawnSession();
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x0009D365 File Offset: 0x0009B565
		public MatrixFrame GetSpawnFrame(Team team, bool hasMount, bool isInitialSpawn = false)
		{
			SpawnFrameBehaviorBase spawnFrameBehavior = this.SpawnFrameBehavior;
			if (spawnFrameBehavior == null)
			{
				return MatrixFrame.Identity;
			}
			return spawnFrameBehavior.GetSpawnFrame(team, hasMount, isInitialSpawn);
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x0009D37F File Offset: 0x0009B57F
		protected void SpawnEquipmentUpdated(MissionPeer lobbyPeer, Equipment equipment)
		{
			if (GameNetwork.IsServer && lobbyPeer != null && this.SpawningBehavior.CanUpdateSpawnEquipment(lobbyPeer) && lobbyPeer.HasSpawnedAgentVisuals)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new EquipEquipmentToPeer(lobbyPeer.GetNetworkPeer(), equipment));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x0009D3BE File Offset: 0x0009B5BE
		public void SetEarlyAgentVisualsDespawning(MissionPeer missionPeer, bool canDespawnEarly = true)
		{
			if (missionPeer != null && this.AllowEarlyAgentVisualsDespawning(missionPeer))
			{
				missionPeer.EquipmentUpdatingExpired = canDespawnEarly;
			}
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x0009D3D3 File Offset: 0x0009B5D3
		public void ToggleUpdatingSpawnEquipment(bool canUpdate)
		{
			this.SpawningBehavior.ToggleUpdatingSpawnEquipment(canUpdate);
		}

		// Token: 0x0600295E RID: 10590 RVA: 0x0009D3E4 File Offset: 0x0009B5E4
		public bool AllowEarlyAgentVisualsDespawning(MissionPeer lobbyPeer)
		{
			MultiplayerClassDivisions.MPHeroClass mpheroClassForPeer = MultiplayerClassDivisions.GetMPHeroClassForPeer(lobbyPeer, false);
			return this._missionMultiplayerGameModeBase.IsClassAvailable(mpheroClassForPeer) && this.SpawningBehavior.AllowEarlyAgentVisualsDespawning(lobbyPeer);
		}

		// Token: 0x0600295F RID: 10591 RVA: 0x0009D415 File Offset: 0x0009B615
		public int GetMaximumReSpawnPeriodForPeer(MissionPeer lobbyPeer)
		{
			return this.SpawningBehavior.GetMaximumReSpawnPeriodForPeer(lobbyPeer);
		}

		// Token: 0x06002960 RID: 10592 RVA: 0x0009D423 File Offset: 0x0009B623
		public override void OnClearScene()
		{
			base.OnClearScene();
			this.SpawningBehavior.OnClearScene();
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x0009D436 File Offset: 0x0009B636
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			this.SpawningBehavior.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);
			this.SpawnFrameBehavior.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);
		}

		// Token: 0x04000FD5 RID: 4053
		private MissionMultiplayerGameModeBase _missionMultiplayerGameModeBase;
	}
}
