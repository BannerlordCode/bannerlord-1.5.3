using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200001E RID: 30
	public class CheckClanTagValidResult : FunctionResult
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00002927 File Offset: 0x00000B27
		// (set) Token: 0x060000BA RID: 186 RVA: 0x0000292F File Offset: 0x00000B2F
		[JsonProperty]
		public bool TagExists { get; private set; }

		// Token: 0x060000BB RID: 187 RVA: 0x00002938 File Offset: 0x00000B38
		public CheckClanTagValidResult()
		{
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00002940 File Offset: 0x00000B40
		public CheckClanTagValidResult(bool tagExists)
		{
			this.TagExists = tagExists;
		}
	}
}
