using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200005C RID: 92
	[Serializable]
	public class PlayerRemovedFromMatchmakerGame : Message
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x000034E4 File Offset: 0x000016E4
		// (set) Token: 0x060001D9 RID: 473 RVA: 0x000034EC File Offset: 0x000016EC
		[JsonProperty]
		public DisconnectType DisconnectType { get; private set; }

		// Token: 0x060001DA RID: 474 RVA: 0x000034F5 File Offset: 0x000016F5
		public PlayerRemovedFromMatchmakerGame()
		{
		}

		// Token: 0x060001DB RID: 475 RVA: 0x000034FD File Offset: 0x000016FD
		public PlayerRemovedFromMatchmakerGame(DisconnectType disconnectType)
		{
			this.DisconnectType = disconnectType;
		}
	}
}
