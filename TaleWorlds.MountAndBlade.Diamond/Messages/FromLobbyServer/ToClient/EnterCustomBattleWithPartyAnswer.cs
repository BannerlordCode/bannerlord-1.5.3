using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200002F RID: 47
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class EnterCustomBattleWithPartyAnswer : Message
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00002C10 File Offset: 0x00000E10
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00002C18 File Offset: 0x00000E18
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x06000103 RID: 259 RVA: 0x00002C21 File Offset: 0x00000E21
		public EnterCustomBattleWithPartyAnswer()
		{
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00002C29 File Offset: 0x00000E29
		public EnterCustomBattleWithPartyAnswer(bool successful)
		{
			this.Successful = successful;
		}
	}
}
