using System;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x0200007D RID: 125
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class CancelCreatingPremadeGameMessage : Message
	{
	}
}
