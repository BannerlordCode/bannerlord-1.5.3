using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000082 RID: 130
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class ChangeRegionMessage : Message
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600027F RID: 639 RVA: 0x00003BCD File Offset: 0x00001DCD
		// (set) Token: 0x06000280 RID: 640 RVA: 0x00003BD5 File Offset: 0x00001DD5
		[JsonProperty]
		public string Region { get; private set; }

		// Token: 0x06000281 RID: 641 RVA: 0x00003BDE File Offset: 0x00001DDE
		public ChangeRegionMessage()
		{
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00003BE6 File Offset: 0x00001DE6
		public ChangeRegionMessage(string region)
		{
			this.Region = region;
		}
	}
}
