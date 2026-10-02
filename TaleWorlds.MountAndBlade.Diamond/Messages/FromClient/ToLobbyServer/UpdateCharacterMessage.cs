using System;
using Newtonsoft.Json;
using TaleWorlds.Core;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000C6 RID: 198
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class UpdateCharacterMessage : Message
	{
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x000047DA File Offset: 0x000029DA
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x000047E2 File Offset: 0x000029E2
		[JsonProperty]
		public BodyProperties BodyProperties { get; private set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x000047EB File Offset: 0x000029EB
		// (set) Token: 0x060003A4 RID: 932 RVA: 0x000047F3 File Offset: 0x000029F3
		[JsonProperty]
		public bool IsFemale { get; private set; }

		// Token: 0x060003A5 RID: 933 RVA: 0x000047FC File Offset: 0x000029FC
		public UpdateCharacterMessage()
		{
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00004804 File Offset: 0x00002A04
		public UpdateCharacterMessage(BodyProperties bodyProperties, bool isFemale)
		{
			this.BodyProperties = bodyProperties;
			this.IsFemale = isFemale;
		}
	}
}
