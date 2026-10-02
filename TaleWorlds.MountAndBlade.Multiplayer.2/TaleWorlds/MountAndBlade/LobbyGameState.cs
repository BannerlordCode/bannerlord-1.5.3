using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200000A RID: 10
	public abstract class LobbyGameState : GameState, IUdpNetworkHandler
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600006D RID: 109 RVA: 0x000039F1 File Offset: 0x00001BF1
		public override bool IsMusicMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000039FC File Offset: 0x00001BFC
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this.StartMultiplayer();
			GameNetwork.AddNetworkHandler(this);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003A10 File Offset: 0x00001C10
		protected override void OnActivate()
		{
			base.OnActivate();
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003A18 File Offset: 0x00001C18
		protected override void OnFinalize()
		{
			base.OnFinalize();
			GameNetwork.RemoveNetworkHandler(this);
			GameNetwork.EndMultiplayer();
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003A2B File Offset: 0x00001C2B
		void IUdpNetworkHandler.OnUdpNetworkHandlerClose()
		{
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003A2D File Offset: 0x00001C2D
		void IUdpNetworkHandler.OnUdpNetworkHandlerTick(float dt)
		{
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003A2F File Offset: 0x00001C2F
		void IUdpNetworkHandler.HandleNewClientConnect(PlayerConnectionInfo clientConnectionInfo)
		{
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003A31 File Offset: 0x00001C31
		void IUdpNetworkHandler.HandleEarlyNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003A33 File Offset: 0x00001C33
		void IUdpNetworkHandler.HandleNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003A35 File Offset: 0x00001C35
		void IUdpNetworkHandler.HandleLateNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003A37 File Offset: 0x00001C37
		void IUdpNetworkHandler.HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003A39 File Offset: 0x00001C39
		void IUdpNetworkHandler.HandleLateNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003A3B File Offset: 0x00001C3B
		void IUdpNetworkHandler.HandleEarlyPlayerDisconnect(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003A3D File Offset: 0x00001C3D
		void IUdpNetworkHandler.HandlePlayerDisconnect(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003A3F File Offset: 0x00001C3F
		void IUdpNetworkHandler.OnEveryoneUnSynchronized()
		{
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003A41 File Offset: 0x00001C41
		void IUdpNetworkHandler.OnPlayerDisconnectedFromServer(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003A43 File Offset: 0x00001C43
		void IUdpNetworkHandler.OnDisconnectedFromServer()
		{
			this.OnDisconnectedFromServer();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003A4B File Offset: 0x00001C4B
		protected virtual void OnDisconnectedFromServer()
		{
		}

		// Token: 0x06000080 RID: 128
		protected abstract void StartMultiplayer();
	}
}
