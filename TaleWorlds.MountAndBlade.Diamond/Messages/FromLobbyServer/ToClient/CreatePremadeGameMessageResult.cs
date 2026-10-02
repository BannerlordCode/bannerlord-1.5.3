using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200002A RID: 42
	[Serializable]
	public class CreatePremadeGameMessageResult : FunctionResult
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00002B20 File Offset: 0x00000D20
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00002B28 File Offset: 0x00000D28
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x060000EC RID: 236 RVA: 0x00002B31 File Offset: 0x00000D31
		public CreatePremadeGameMessageResult()
		{
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00002B39 File Offset: 0x00000D39
		public CreatePremadeGameMessageResult(bool successful)
		{
			this.Successful = successful;
		}
	}
}
