using System;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000019 RID: 25
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class BattleServerLostMessage : Message
	{
	}
}
