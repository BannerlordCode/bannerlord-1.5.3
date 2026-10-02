using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002F9 RID: 761
	public interface IGameNetworkHandler
	{
		// Token: 0x06002BF6 RID: 11254
		void OnNewPlayerConnect(PlayerConnectionInfo playerConnectionInfo, NetworkCommunicator networkPeer);

		// Token: 0x06002BF7 RID: 11255
		void OnInitialize();

		// Token: 0x06002BF8 RID: 11256
		void OnPlayerConnectedToServer(NetworkCommunicator peer);

		// Token: 0x06002BF9 RID: 11257
		void OnPlayerDisconnectedFromServer(NetworkCommunicator peer);

		// Token: 0x06002BFA RID: 11258
		void OnDisconnectedFromServer();

		// Token: 0x06002BFB RID: 11259
		void OnStartMultiplayer();

		// Token: 0x06002BFC RID: 11260
		void OnStartReplay();

		// Token: 0x06002BFD RID: 11261
		void OnEndMultiplayer();

		// Token: 0x06002BFE RID: 11262
		void OnEndReplay();

		// Token: 0x06002BFF RID: 11263
		void OnHandleConsoleCommand(string command);
	}
}
