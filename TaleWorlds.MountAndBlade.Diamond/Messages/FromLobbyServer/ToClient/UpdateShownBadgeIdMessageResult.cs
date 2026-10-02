using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200006C RID: 108
	[Serializable]
	public class UpdateShownBadgeIdMessageResult : FunctionResult
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600022B RID: 555 RVA: 0x00003875 File Offset: 0x00001A75
		// (set) Token: 0x0600022C RID: 556 RVA: 0x0000387D File Offset: 0x00001A7D
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x0600022D RID: 557 RVA: 0x00003886 File Offset: 0x00001A86
		public UpdateShownBadgeIdMessageResult()
		{
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000388E File Offset: 0x00001A8E
		public UpdateShownBadgeIdMessageResult(bool successful)
		{
			this.Successful = successful;
		}
	}
}
