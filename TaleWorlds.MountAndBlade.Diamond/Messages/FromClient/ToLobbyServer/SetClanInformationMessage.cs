using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000C5 RID: 197
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class SetClanInformationMessage : Message
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600039D RID: 925 RVA: 0x000047B2 File Offset: 0x000029B2
		// (set) Token: 0x0600039E RID: 926 RVA: 0x000047BA File Offset: 0x000029BA
		[JsonProperty]
		public string Information { get; private set; }

		// Token: 0x0600039F RID: 927 RVA: 0x000047C3 File Offset: 0x000029C3
		public SetClanInformationMessage()
		{
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x000047CB File Offset: 0x000029CB
		public SetClanInformationMessage(string information)
		{
			this.Information = information;
		}
	}
}
