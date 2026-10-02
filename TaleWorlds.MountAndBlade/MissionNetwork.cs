using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002A4 RID: 676
	public abstract class MissionNetwork : MissionLogic, IUdpNetworkHandler
	{
		// Token: 0x06002575 RID: 9589 RVA: 0x00088F4C File Offset: 0x0008714C
		public override void OnAfterMissionCreated()
		{
			this._missionNetworkMessageHandlerRegisterer = new GameNetwork.NetworkMessageHandlerRegistererContainer();
			this.AddRemoveMessageHandlers(this._missionNetworkMessageHandlerRegisterer);
			this._missionNetworkMessageHandlerRegisterer.RegisterMessages();
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x00088F70 File Offset: 0x00087170
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			GameNetwork.AddNetworkHandler(this);
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x00088F7E File Offset: 0x0008717E
		public override void OnRemoveBehavior()
		{
			GameNetwork.RemoveNetworkHandler(this);
			base.OnRemoveBehavior();
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x00088F8C File Offset: 0x0008718C
		protected virtual void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x00088F8E File Offset: 0x0008718E
		public virtual void OnPlayerConnectedToServer(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x00088F90 File Offset: 0x00087190
		public virtual void OnPlayerDisconnectedFromServer(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x00088F92 File Offset: 0x00087192
		void IUdpNetworkHandler.OnUdpNetworkHandlerTick(float dt)
		{
			this.OnUdpNetworkHandlerTick();
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x00088F9A File Offset: 0x0008719A
		void IUdpNetworkHandler.OnUdpNetworkHandlerClose()
		{
			this.OnUdpNetworkHandlerClose();
			GameNetwork.NetworkMessageHandlerRegistererContainer missionNetworkMessageHandlerRegisterer = this._missionNetworkMessageHandlerRegisterer;
			if (missionNetworkMessageHandlerRegisterer == null)
			{
				return;
			}
			missionNetworkMessageHandlerRegisterer.UnregisterMessages();
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x00088FB2 File Offset: 0x000871B2
		void IUdpNetworkHandler.HandleNewClientConnect(PlayerConnectionInfo clientConnectionInfo)
		{
			this.HandleNewClientConnect(clientConnectionInfo);
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x00088FBB File Offset: 0x000871BB
		void IUdpNetworkHandler.HandleEarlyNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
			this.HandleEarlyNewClientAfterLoadingFinished(networkPeer);
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x00088FC4 File Offset: 0x000871C4
		void IUdpNetworkHandler.HandleNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
			this.HandleNewClientAfterLoadingFinished(networkPeer);
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x00088FCD File Offset: 0x000871CD
		void IUdpNetworkHandler.HandleLateNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
			this.HandleLateNewClientAfterLoadingFinished(networkPeer);
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x00088FD6 File Offset: 0x000871D6
		void IUdpNetworkHandler.HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
			this.HandleNewClientAfterSynchronized(networkPeer);
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x00088FDF File Offset: 0x000871DF
		void IUdpNetworkHandler.HandleLateNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
			this.HandleLateNewClientAfterSynchronized(networkPeer);
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x00088FE8 File Offset: 0x000871E8
		void IUdpNetworkHandler.HandleEarlyPlayerDisconnect(NetworkCommunicator networkPeer)
		{
			this.HandleEarlyPlayerDisconnect(networkPeer);
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x00088FF1 File Offset: 0x000871F1
		void IUdpNetworkHandler.HandlePlayerDisconnect(NetworkCommunicator networkPeer)
		{
			this.HandlePlayerDisconnect(networkPeer);
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x00088FFA File Offset: 0x000871FA
		void IUdpNetworkHandler.OnEveryoneUnSynchronized()
		{
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x00088FFC File Offset: 0x000871FC
		void IUdpNetworkHandler.OnPlayerDisconnectedFromServer(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x00088FFE File Offset: 0x000871FE
		void IUdpNetworkHandler.OnDisconnectedFromServer()
		{
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x00089000 File Offset: 0x00087200
		protected virtual void OnUdpNetworkHandlerTick()
		{
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x00089002 File Offset: 0x00087202
		protected virtual void OnUdpNetworkHandlerClose()
		{
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x00089004 File Offset: 0x00087204
		protected virtual void HandleNewClientConnect(PlayerConnectionInfo clientConnectionInfo)
		{
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x00089006 File Offset: 0x00087206
		protected virtual void HandleEarlyNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x00089008 File Offset: 0x00087208
		protected virtual void HandleNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x0008900A File Offset: 0x0008720A
		protected virtual void HandleLateNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x0008900C File Offset: 0x0008720C
		protected virtual void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x0008900E File Offset: 0x0008720E
		protected virtual void HandleLateNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x00089010 File Offset: 0x00087210
		protected virtual void HandleEarlyPlayerDisconnect(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x00089012 File Offset: 0x00087212
		protected virtual void HandlePlayerDisconnect(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x04000E8A RID: 3722
		private GameNetwork.NetworkMessageHandlerRegistererContainer _missionNetworkMessageHandlerRegisterer;
	}
}
