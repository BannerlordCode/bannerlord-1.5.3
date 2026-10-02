using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000CE RID: 206
	[Serializable]
	public class PlatformPlayerJoinedToPlayerSessionMessageResult : FunctionResult
	{
		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x0000495A File Offset: 0x00002B5A
		// (set) Token: 0x060003C7 RID: 967 RVA: 0x00004962 File Offset: 0x00002B62
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x060003C8 RID: 968 RVA: 0x0000496B File Offset: 0x00002B6B
		public PlatformPlayerJoinedToPlayerSessionMessageResult()
		{
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00004973 File Offset: 0x00002B73
		public PlatformPlayerJoinedToPlayerSessionMessageResult(bool successful)
		{
			this.Successful = successful;
		}
	}
}
