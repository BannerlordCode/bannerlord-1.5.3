using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200001A RID: 26
	[Serializable]
	public class BuyCosmeticMessageResult : FunctionResult
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00002877 File Offset: 0x00000A77
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x0000287F File Offset: 0x00000A7F
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00002888 File Offset: 0x00000A88
		// (set) Token: 0x060000AB RID: 171 RVA: 0x00002890 File Offset: 0x00000A90
		[JsonProperty]
		public int Gold { get; private set; }

		// Token: 0x060000AC RID: 172 RVA: 0x00002899 File Offset: 0x00000A99
		public BuyCosmeticMessageResult()
		{
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000028A1 File Offset: 0x00000AA1
		public BuyCosmeticMessageResult(bool successful, int gold)
		{
			this.Successful = successful;
			this.Gold = gold;
		}
	}
}
