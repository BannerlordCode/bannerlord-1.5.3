using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000067 RID: 103
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class ServerStatusMessage : Message
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000215 RID: 533 RVA: 0x00003790 File Offset: 0x00001990
		// (set) Token: 0x06000216 RID: 534 RVA: 0x00003798 File Offset: 0x00001998
		[JsonProperty]
		public ServerStatus ServerStatus { get; private set; }

		// Token: 0x06000217 RID: 535 RVA: 0x000037A1 File Offset: 0x000019A1
		public ServerStatusMessage()
		{
		}

		// Token: 0x06000218 RID: 536 RVA: 0x000037A9 File Offset: 0x000019A9
		public ServerStatusMessage(ServerStatus serverStatus)
		{
			this.ServerStatus = serverStatus;
		}
	}
}
