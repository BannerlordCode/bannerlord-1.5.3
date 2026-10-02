using System;
using System.Linq;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002B2 RID: 690
	public class MissionLobbyEquipmentNetworkComponent : MissionNetwork
	{
		// Token: 0x1400004B RID: 75
		// (add) Token: 0x06002644 RID: 9796 RVA: 0x0008B6D4 File Offset: 0x000898D4
		// (remove) Token: 0x06002645 RID: 9797 RVA: 0x0008B70C File Offset: 0x0008990C
		public event MissionLobbyEquipmentNetworkComponent.OnToggleLoadoutDelegate OnToggleLoadout;

		// Token: 0x1400004C RID: 76
		// (add) Token: 0x06002646 RID: 9798 RVA: 0x0008B744 File Offset: 0x00089944
		// (remove) Token: 0x06002647 RID: 9799 RVA: 0x0008B77C File Offset: 0x0008997C
		public event MissionLobbyEquipmentNetworkComponent.OnRefreshEquipmentEventDelegate OnEquipmentRefreshed;

		// Token: 0x06002648 RID: 9800 RVA: 0x0008B7B4 File Offset: 0x000899B4
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			if (!GameNetwork.IsDedicatedServer)
			{
				this._agentVisualSpawnComponent = Mission.Current.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>();
				this._agentVisualSpawnComponent.OnMyAgentVisualSpawned += this.OpenLoadout;
				this._agentVisualSpawnComponent.OnMyAgentSpawnedFromVisual += this.CloseLoadout;
				this._agentVisualSpawnComponent.OnMyAgentVisualRemoved += this.CloseLoadout;
			}
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x0008B824 File Offset: 0x00089A24
		protected override void OnEndMission()
		{
			if (!GameNetwork.IsDedicatedServer)
			{
				this._agentVisualSpawnComponent.OnMyAgentVisualSpawned -= this.OpenLoadout;
				this._agentVisualSpawnComponent.OnMyAgentSpawnedFromVisual -= this.CloseLoadout;
				this._agentVisualSpawnComponent.OnMyAgentVisualRemoved -= this.CloseLoadout;
				this._agentVisualSpawnComponent = null;
			}
			base.OnEndMission();
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x0008B88C File Offset: 0x00089A8C
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsServer)
			{
				registerer.RegisterBaseHandler<RequestTroopIndexChange>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventLobbyEquipmentUpdated));
				registerer.RegisterBaseHandler<TeamInitialPerkInfoMessage>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventTeamInitialPerkInfoMessage));
				registerer.RegisterBaseHandler<RequestPerkChange>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventRequestPerkChange));
				return;
			}
			if (GameNetwork.IsClientOrReplay)
			{
				registerer.RegisterBaseHandler<UpdateSelectedTroopIndex>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventEquipmentIndexUpdated));
				registerer.RegisterBaseHandler<SyncPerksForCurrentlySelectedTroop>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.SyncPerksForCurrentlySelectedTroop));
			}
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x0008B904 File Offset: 0x00089B04
		private void HandleServerEventEquipmentIndexUpdated(GameNetworkMessage baseMessage)
		{
			UpdateSelectedTroopIndex updateSelectedTroopIndex = (UpdateSelectedTroopIndex)baseMessage;
			updateSelectedTroopIndex.Peer.GetComponent<MissionPeer>().SelectedTroopIndex = updateSelectedTroopIndex.SelectedTroopIndex;
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x0008B930 File Offset: 0x00089B30
		private void SyncPerksForCurrentlySelectedTroop(GameNetworkMessage baseMessage)
		{
			SyncPerksForCurrentlySelectedTroop syncPerksForCurrentlySelectedTroop = (SyncPerksForCurrentlySelectedTroop)baseMessage;
			MissionPeer component = syncPerksForCurrentlySelectedTroop.Peer.GetComponent<MissionPeer>();
			for (int i = 0; i < 3; i++)
			{
				component.SelectPerk(i, syncPerksForCurrentlySelectedTroop.PerkIndices[i], component.SelectedTroopIndex);
			}
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x0008B974 File Offset: 0x00089B74
		private bool HandleClientEventLobbyEquipmentUpdated(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			RequestTroopIndexChange requestTroopIndexChange = (RequestTroopIndexChange)baseMessage;
			MissionPeer component = peer.GetComponent<MissionPeer>();
			if (component == null)
			{
				return false;
			}
			SpawnComponent missionBehavior = base.Mission.GetMissionBehavior<SpawnComponent>();
			if (missionBehavior == null)
			{
				return false;
			}
			if (missionBehavior.AreAgentsSpawning() && component.SelectedTroopIndex != requestTroopIndexChange.SelectedTroopIndex)
			{
				if (component.Culture == null || requestTroopIndexChange.SelectedTroopIndex < 0 || MultiplayerClassDivisions.GetMPHeroClasses(component.Culture).Count<MultiplayerClassDivisions.MPHeroClass>() <= requestTroopIndexChange.SelectedTroopIndex)
				{
					component.SelectedTroopIndex = 0;
				}
				else
				{
					component.SelectedTroopIndex = requestTroopIndexChange.SelectedTroopIndex;
				}
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new UpdateSelectedTroopIndex(peer, component.SelectedTroopIndex));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeOtherTeamPlayers, peer);
				if (this.OnEquipmentRefreshed != null)
				{
					this.OnEquipmentRefreshed(component);
				}
			}
			return true;
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x0008BA2C File Offset: 0x00089C2C
		private bool HandleClientEventTeamInitialPerkInfoMessage(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			TeamInitialPerkInfoMessage teamInitialPerkInfoMessage = (TeamInitialPerkInfoMessage)baseMessage;
			MissionPeer component = peer.GetComponent<MissionPeer>();
			if (component == null)
			{
				return false;
			}
			if (base.Mission.GetMissionBehavior<SpawnComponent>() == null)
			{
				return false;
			}
			component.OnTeamInitialPerkInfoReceived(teamInitialPerkInfoMessage.Perks);
			return true;
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x0008BA68 File Offset: 0x00089C68
		private bool HandleClientEventRequestPerkChange(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			RequestPerkChange requestPerkChange = (RequestPerkChange)baseMessage;
			MissionPeer component = peer.GetComponent<MissionPeer>();
			if (component == null)
			{
				return false;
			}
			SpawnComponent missionBehavior = base.Mission.GetMissionBehavior<SpawnComponent>();
			if (missionBehavior == null)
			{
				return false;
			}
			if (component.SelectPerk(requestPerkChange.PerkListIndex, requestPerkChange.PerkIndex, -1) && missionBehavior.AreAgentsSpawning() && this.OnEquipmentRefreshed != null)
			{
				this.OnEquipmentRefreshed(component);
			}
			return true;
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x0008BACC File Offset: 0x00089CCC
		public void PerkUpdated(int perkList, int perkIndex)
		{
			if (GameNetwork.IsServer)
			{
				MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				if (this.OnEquipmentRefreshed != null)
				{
					this.OnEquipmentRefreshed(component);
					return;
				}
			}
			else
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new RequestPerkChange(perkList, perkIndex));
				GameNetwork.EndModuleEventAsClient();
			}
		}

		// Token: 0x06002651 RID: 9809 RVA: 0x0008BB18 File Offset: 0x00089D18
		public void EquipmentUpdated()
		{
			if (GameNetwork.IsServer)
			{
				MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				if (component.SelectedTroopIndex != component.NextSelectedTroopIndex)
				{
					component.SelectedTroopIndex = component.NextSelectedTroopIndex;
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new UpdateSelectedTroopIndex(GameNetwork.MyPeer, component.SelectedTroopIndex));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeOtherTeamPlayers, GameNetwork.MyPeer);
					if (this.OnEquipmentRefreshed != null)
					{
						this.OnEquipmentRefreshed(component);
						return;
					}
				}
			}
			else
			{
				MissionPeer component2 = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new RequestTroopIndexChange(component2.NextSelectedTroopIndex));
				GameNetwork.EndModuleEventAsClient();
			}
		}

		// Token: 0x06002652 RID: 9810 RVA: 0x0008BBAF File Offset: 0x00089DAF
		public void ToggleLoadout(bool isActive)
		{
			if (this.OnToggleLoadout != null)
			{
				this.OnToggleLoadout(isActive);
			}
		}

		// Token: 0x06002653 RID: 9811 RVA: 0x0008BBC5 File Offset: 0x00089DC5
		private void OpenLoadout()
		{
			this.ToggleLoadout(true);
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x0008BBCE File Offset: 0x00089DCE
		private void CloseLoadout()
		{
			this.ToggleLoadout(false);
		}

		// Token: 0x04000EBD RID: 3773
		private MultiplayerMissionAgentVisualSpawnComponent _agentVisualSpawnComponent;

		// Token: 0x02000585 RID: 1413
		// (Invoke) Token: 0x06003E27 RID: 15911
		public delegate void OnToggleLoadoutDelegate(bool isActive);

		// Token: 0x02000586 RID: 1414
		// (Invoke) Token: 0x06003E2B RID: 15915
		public delegate void OnRefreshEquipmentEventDelegate(MissionPeer lobbyPeer);
	}
}
