using System;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000056 RID: 86
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class PendingBattleRejoinMessage : Message
	{
	}
}
