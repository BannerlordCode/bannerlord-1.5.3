using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000CF RID: 207
	[Serializable]
	public class PSPlayerJoinedToPlayerSessionMessageResult : FunctionResult
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060003CA RID: 970 RVA: 0x00004982 File Offset: 0x00002B82
		// (set) Token: 0x060003CB RID: 971 RVA: 0x0000498A File Offset: 0x00002B8A
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x060003CC RID: 972 RVA: 0x00004993 File Offset: 0x00002B93
		public PSPlayerJoinedToPlayerSessionMessageResult()
		{
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000499B File Offset: 0x00002B9B
		public PSPlayerJoinedToPlayerSessionMessageResult(bool successful)
		{
			this.Successful = successful;
		}
	}
}
