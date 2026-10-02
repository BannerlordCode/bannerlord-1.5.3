using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200006D RID: 109
	[Serializable]
	public class UpdateUsedCosmeticItemsMessageResult : FunctionResult
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000389D File Offset: 0x00001A9D
		// (set) Token: 0x06000230 RID: 560 RVA: 0x000038A5 File Offset: 0x00001AA5
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x06000231 RID: 561 RVA: 0x000038AE File Offset: 0x00001AAE
		public UpdateUsedCosmeticItemsMessageResult()
		{
		}

		// Token: 0x06000232 RID: 562 RVA: 0x000038B6 File Offset: 0x00001AB6
		public UpdateUsedCosmeticItemsMessageResult(bool successful)
		{
			this.Successful = successful;
		}
	}
}
