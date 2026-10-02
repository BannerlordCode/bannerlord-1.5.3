using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200006A RID: 106
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class SystemMessage : Message
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000221 RID: 545 RVA: 0x00003808 File Offset: 0x00001A08
		// (set) Token: 0x06000222 RID: 546 RVA: 0x00003810 File Offset: 0x00001A10
		[JsonProperty]
		public ServerInfoMessage Message { get; private set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000223 RID: 547 RVA: 0x00003819 File Offset: 0x00001A19
		// (set) Token: 0x06000224 RID: 548 RVA: 0x00003821 File Offset: 0x00001A21
		[JsonProperty]
		public List<string> Parameters { get; private set; }

		// Token: 0x06000225 RID: 549 RVA: 0x0000382A File Offset: 0x00001A2A
		public SystemMessage()
		{
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00003832 File Offset: 0x00001A32
		public SystemMessage(ServerInfoMessage message, params string[] arguments)
		{
			this.Message = message;
			this.Parameters = new List<string>(arguments);
		}
	}
}
