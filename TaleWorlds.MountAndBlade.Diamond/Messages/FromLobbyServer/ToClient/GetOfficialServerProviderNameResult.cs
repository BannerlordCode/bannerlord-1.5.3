using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200003A RID: 58
	[Serializable]
	public class GetOfficialServerProviderNameResult : FunctionResult
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00002DE6 File Offset: 0x00000FE6
		// (set) Token: 0x06000130 RID: 304 RVA: 0x00002DEE File Offset: 0x00000FEE
		[JsonProperty]
		public string Name { get; private set; }

		// Token: 0x06000131 RID: 305 RVA: 0x00002DF7 File Offset: 0x00000FF7
		public GetOfficialServerProviderNameResult()
		{
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00002DFF File Offset: 0x00000FFF
		public GetOfficialServerProviderNameResult(string name)
		{
			this.Name = name;
		}
	}
}
