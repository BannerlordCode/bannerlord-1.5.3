using System;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000A3 RID: 163
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetPlayerCountInQueue : Message
	{
	}
}
