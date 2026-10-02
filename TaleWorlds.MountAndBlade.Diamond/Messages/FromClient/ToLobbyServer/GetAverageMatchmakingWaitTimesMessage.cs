using System;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000098 RID: 152
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetAverageMatchmakingWaitTimesMessage : Message
	{
	}
}
