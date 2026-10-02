using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200032D RID: 813
	public interface IUdpNetworkHandler
	{
		// Token: 0x06002E5B RID: 11867
		void OnUdpNetworkHandlerClose();

		// Token: 0x06002E5C RID: 11868
		void OnUdpNetworkHandlerTick(float dt);

		// Token: 0x06002E5D RID: 11869
		void HandleNewClientConnect(PlayerConnectionInfo clientConnectionInfo);

		// Token: 0x06002E5E RID: 11870
		void HandleEarlyNewClientAfterLoadingFinished(NetworkCommunicator networkPeer);

		// Token: 0x06002E5F RID: 11871
		void HandleNewClientAfterLoadingFinished(NetworkCommunicator networkPeer);

		// Token: 0x06002E60 RID: 11872
		void HandleLateNewClientAfterLoadingFinished(NetworkCommunicator networkPeer);

		// Token: 0x06002E61 RID: 11873
		void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer);

		// Token: 0x06002E62 RID: 11874
		void HandleLateNewClientAfterSynchronized(NetworkCommunicator networkPeer);

		// Token: 0x06002E63 RID: 11875
		void HandleEarlyPlayerDisconnect(NetworkCommunicator networkPeer);

		// Token: 0x06002E64 RID: 11876
		void HandlePlayerDisconnect(NetworkCommunicator networkPeer);

		// Token: 0x06002E65 RID: 11877
		void OnPlayerDisconnectedFromServer(NetworkCommunicator networkPeer);

		// Token: 0x06002E66 RID: 11878
		void OnDisconnectedFromServer();

		// Token: 0x06002E67 RID: 11879
		void OnEveryoneUnSynchronized();
	}
}
