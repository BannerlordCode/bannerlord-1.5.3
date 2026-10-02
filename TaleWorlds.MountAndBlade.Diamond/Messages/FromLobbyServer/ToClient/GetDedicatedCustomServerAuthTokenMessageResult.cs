using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000039 RID: 57
	[Serializable]
	public class GetDedicatedCustomServerAuthTokenMessageResult : FunctionResult
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00002DBE File Offset: 0x00000FBE
		// (set) Token: 0x0600012C RID: 300 RVA: 0x00002DC6 File Offset: 0x00000FC6
		[JsonProperty]
		public string AuthToken { get; private set; }

		// Token: 0x0600012D RID: 301 RVA: 0x00002DCF File Offset: 0x00000FCF
		public GetDedicatedCustomServerAuthTokenMessageResult()
		{
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00002DD7 File Offset: 0x00000FD7
		public GetDedicatedCustomServerAuthTokenMessageResult(string authToken)
		{
			this.AuthToken = authToken;
		}
	}
}
