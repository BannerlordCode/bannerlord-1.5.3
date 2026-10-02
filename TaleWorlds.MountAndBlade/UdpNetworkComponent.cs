using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200032C RID: 812
	public abstract class UdpNetworkComponent : IUdpNetworkHandler
	{
		// Token: 0x06002E4C RID: 11852 RVA: 0x000B334C File Offset: 0x000B154C
		protected UdpNetworkComponent()
		{
			this._missionNetworkMessageHandlerRegisterer = new GameNetwork.NetworkMessageHandlerRegistererContainer();
			this.AddRemoveMessageHandlers(this._missionNetworkMessageHandlerRegisterer);
			this._missionNetworkMessageHandlerRegisterer.RegisterMessages();
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x000B3376 File Offset: 0x000B1576
		protected virtual void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x000B3378 File Offset: 0x000B1578
		public virtual void OnUdpNetworkHandlerClose()
		{
			GameNetwork.NetworkMessageHandlerRegistererContainer missionNetworkMessageHandlerRegisterer = this._missionNetworkMessageHandlerRegisterer;
			if (missionNetworkMessageHandlerRegisterer != null)
			{
				missionNetworkMessageHandlerRegisterer.UnregisterMessages();
			}
			GameNetwork.NetworkComponents.Remove(this);
		}

		// Token: 0x06002E4F RID: 11855 RVA: 0x000B3397 File Offset: 0x000B1597
		public virtual void OnUdpNetworkHandlerTick(float dt)
		{
		}

		// Token: 0x06002E50 RID: 11856 RVA: 0x000B3399 File Offset: 0x000B1599
		public virtual void HandleNewClientConnect(PlayerConnectionInfo clientConnectionInfo)
		{
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x000B339B File Offset: 0x000B159B
		public virtual void HandleEarlyNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002E52 RID: 11858 RVA: 0x000B339D File Offset: 0x000B159D
		public virtual void HandleNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002E53 RID: 11859 RVA: 0x000B339F File Offset: 0x000B159F
		public virtual void HandleLateNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002E54 RID: 11860 RVA: 0x000B33A1 File Offset: 0x000B15A1
		public virtual void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002E55 RID: 11861 RVA: 0x000B33A3 File Offset: 0x000B15A3
		public virtual void HandleLateNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002E56 RID: 11862 RVA: 0x000B33A5 File Offset: 0x000B15A5
		public virtual void OnEveryoneUnSynchronized()
		{
		}

		// Token: 0x06002E57 RID: 11863 RVA: 0x000B33A7 File Offset: 0x000B15A7
		public void HandleEarlyPlayerDisconnect(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x000B33A9 File Offset: 0x000B15A9
		public virtual void HandlePlayerDisconnect(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x000B33AB File Offset: 0x000B15AB
		public virtual void OnPlayerDisconnectedFromServer(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002E5A RID: 11866 RVA: 0x000B33AD File Offset: 0x000B15AD
		public virtual void OnDisconnectedFromServer()
		{
		}

		// Token: 0x04001248 RID: 4680
		private GameNetwork.NetworkMessageHandlerRegistererContainer _missionNetworkMessageHandlerRegisterer;
	}
}
