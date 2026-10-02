using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000030 RID: 48
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class FindGameAnswerMessage : Message
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00002C38 File Offset: 0x00000E38
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00002C40 File Offset: 0x00000E40
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00002C49 File Offset: 0x00000E49
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00002C51 File Offset: 0x00000E51
		[JsonProperty]
		public string[] SelectedAndEnabledGameTypes { get; private set; }

		// Token: 0x06000109 RID: 265 RVA: 0x00002C5A File Offset: 0x00000E5A
		public FindGameAnswerMessage()
		{
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00002C62 File Offset: 0x00000E62
		public FindGameAnswerMessage(bool successful, string[] selectedAndEnabledGameTypes)
		{
			this.Successful = successful;
			this.SelectedAndEnabledGameTypes = selectedAndEnabledGameTypes;
		}
	}
}
