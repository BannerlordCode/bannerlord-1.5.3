using System;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000064 RID: 100
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class RejoinRequestRejectedMessage : Message
	{
	}
}
