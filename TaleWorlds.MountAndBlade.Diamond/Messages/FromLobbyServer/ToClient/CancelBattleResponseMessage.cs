using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200001B RID: 27
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class CancelBattleResponseMessage : Message
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000AE RID: 174 RVA: 0x000028B7 File Offset: 0x00000AB7
		// (set) Token: 0x060000AF RID: 175 RVA: 0x000028BF File Offset: 0x00000ABF
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x060000B0 RID: 176 RVA: 0x000028C8 File Offset: 0x00000AC8
		public CancelBattleResponseMessage()
		{
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000028D0 File Offset: 0x00000AD0
		public CancelBattleResponseMessage(bool successful)
		{
			this.Successful = successful;
		}
	}
}
