using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000CB RID: 203
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class UpdateUsingClanSigil : Message
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060003BB RID: 955 RVA: 0x000048EA File Offset: 0x00002AEA
		// (set) Token: 0x060003BC RID: 956 RVA: 0x000048F2 File Offset: 0x00002AF2
		[JsonProperty]
		public bool IsUsed { get; private set; }

		// Token: 0x060003BD RID: 957 RVA: 0x000048FB File Offset: 0x00002AFB
		public UpdateUsingClanSigil()
		{
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00004903 File Offset: 0x00002B03
		public UpdateUsingClanSigil(bool isUsed)
		{
			this.IsUsed = isUsed;
		}
	}
}
