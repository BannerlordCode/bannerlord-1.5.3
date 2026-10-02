using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Multiplayer.NetworkComponents;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000008 RID: 8
	public class GameNetworkHandler : IGameNetworkHandler
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00002CF9 File Offset: 0x00000EF9
		void IGameNetworkHandler.OnNewPlayerConnect(PlayerConnectionInfo playerConnectionInfo, NetworkCommunicator networkPeer)
		{
			if (networkPeer != null)
			{
				GameManagerBase.Current.OnPlayerConnect(networkPeer.VirtualPlayer);
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002D0E File Offset: 0x00000F0E
		void IGameNetworkHandler.OnInitialize()
		{
			MultiplayerGameTypes.Initialize();
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002D18 File Offset: 0x00000F18
		void IGameNetworkHandler.OnPlayerConnectedToServer(NetworkCommunicator networkPeer)
		{
			if (Mission.Current != null)
			{
				using (List<MissionBehavior>.Enumerator enumerator = Mission.Current.MissionBehaviors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MissionNetwork missionNetwork;
						if ((missionNetwork = enumerator.Current as MissionNetwork) != null)
						{
							missionNetwork.OnPlayerConnectedToServer(networkPeer);
						}
					}
				}
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002D80 File Offset: 0x00000F80
		void IGameNetworkHandler.OnDisconnectedFromServer()
		{
			if (Mission.Current != null)
			{
				BannerlordNetwork.EndMultiplayerLobbyMission();
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002D90 File Offset: 0x00000F90
		void IGameNetworkHandler.OnPlayerDisconnectedFromServer(NetworkCommunicator networkPeer)
		{
			GameManagerBase.Current.OnPlayerDisconnect(networkPeer.VirtualPlayer);
			if (Mission.Current != null)
			{
				using (List<MissionBehavior>.Enumerator enumerator = Mission.Current.MissionBehaviors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MissionNetwork missionNetwork;
						if ((missionNetwork = enumerator.Current as MissionNetwork) != null)
						{
							missionNetwork.OnPlayerDisconnectedFromServer(networkPeer);
						}
					}
				}
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002E08 File Offset: 0x00001008
		void IGameNetworkHandler.OnStartMultiplayer()
		{
			GameNetwork.AddNetworkComponent<BaseNetworkComponentData>();
			GameNetwork.AddNetworkComponent<BaseNetworkComponent>();
			GameNetwork.AddNetworkComponent<LobbyNetworkComponent>();
			GameNetwork.AddNetworkComponent<MultiplayerPermissionHandler>();
			GameNetwork.AddNetworkComponent<NetworkStatusReplicationComponent>();
			GameManagerBase.Current.OnGameNetworkBegin();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002E32 File Offset: 0x00001032
		void IGameNetworkHandler.OnEndMultiplayer()
		{
			GameManagerBase.Current.OnGameNetworkEnd();
			GameNetwork.DestroyComponent(GameNetwork.GetNetworkComponent<LobbyNetworkComponent>());
			GameNetwork.DestroyComponent(GameNetwork.GetNetworkComponent<NetworkStatusReplicationComponent>());
			GameNetwork.DestroyComponent(GameNetwork.GetNetworkComponent<MultiplayerPermissionHandler>());
			GameNetwork.DestroyComponent(GameNetwork.GetNetworkComponent<BaseNetworkComponent>());
			GameNetwork.DestroyComponent(GameNetwork.GetNetworkComponent<BaseNetworkComponentData>());
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002E70 File Offset: 0x00001070
		void IGameNetworkHandler.OnStartReplay()
		{
			GameNetwork.AddNetworkComponent<BaseNetworkComponentData>();
			GameNetwork.AddNetworkComponent<BaseNetworkComponent>();
			GameNetwork.AddNetworkComponent<LobbyNetworkComponent>();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002E84 File Offset: 0x00001084
		void IGameNetworkHandler.OnEndReplay()
		{
			GameNetwork.DestroyComponent(GameNetwork.GetNetworkComponent<LobbyNetworkComponent>());
			GameNetwork.DestroyComponent(GameNetwork.GetNetworkComponent<BaseNetworkComponent>());
			GameNetwork.DestroyComponent(GameNetwork.GetNetworkComponent<BaseNetworkComponentData>());
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002EA4 File Offset: 0x000010A4
		void IGameNetworkHandler.OnHandleConsoleCommand(string command)
		{
			DedicatedServerConsoleCommandManager.HandleConsoleCommand(command);
		}
	}
}
