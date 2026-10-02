using System;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x0200009C RID: 156
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetDedicatedCustomServerAuthTokenMessage : Message
	{
	}
}
