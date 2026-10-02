using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000036 RID: 54
	[Serializable]
	public class GetBannerlordIDMessageResult : FunctionResult
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00002D46 File Offset: 0x00000F46
		// (set) Token: 0x06000120 RID: 288 RVA: 0x00002D4E File Offset: 0x00000F4E
		[JsonProperty]
		public string BannerlordID { get; private set; }

		// Token: 0x06000121 RID: 289 RVA: 0x00002D57 File Offset: 0x00000F57
		public GetBannerlordIDMessageResult()
		{
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00002D5F File Offset: 0x00000F5F
		public GetBannerlordIDMessageResult(string bannerlordID)
		{
			this.BannerlordID = bannerlordID;
		}
	}
}
