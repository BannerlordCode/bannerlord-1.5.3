using System;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002EC RID: 748
	public interface ICommunityClientHandler
	{
		// Token: 0x06002B6B RID: 11115
		void OnJoinCustomGameResponse(string address, int port, PlayerJoinGameResponseDataFromHost response);

		// Token: 0x06002B6C RID: 11116
		void OnQuitFromGame();
	}
}
