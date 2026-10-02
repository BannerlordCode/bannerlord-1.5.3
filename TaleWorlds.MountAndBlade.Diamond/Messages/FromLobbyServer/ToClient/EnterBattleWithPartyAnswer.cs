using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200002E RID: 46
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class EnterBattleWithPartyAnswer : Message
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00002BD0 File Offset: 0x00000DD0
		// (set) Token: 0x060000FC RID: 252 RVA: 0x00002BD8 File Offset: 0x00000DD8
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00002BE1 File Offset: 0x00000DE1
		// (set) Token: 0x060000FE RID: 254 RVA: 0x00002BE9 File Offset: 0x00000DE9
		[JsonProperty]
		public string[] SelectedAndEnabledGameTypes { get; private set; }

		// Token: 0x060000FF RID: 255 RVA: 0x00002BF2 File Offset: 0x00000DF2
		public EnterBattleWithPartyAnswer()
		{
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00002BFA File Offset: 0x00000DFA
		public EnterBattleWithPartyAnswer(bool successful, string[] selectedAndEnabledGameTypes)
		{
			this.Successful = successful;
			this.SelectedAndEnabledGameTypes = selectedAndEnabledGameTypes;
		}
	}
}
