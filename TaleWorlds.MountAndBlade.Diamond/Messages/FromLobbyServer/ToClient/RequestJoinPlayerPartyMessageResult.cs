using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000066 RID: 102
	[Serializable]
	public class RequestJoinPlayerPartyMessageResult : FunctionResult
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00003768 File Offset: 0x00001968
		// (set) Token: 0x06000212 RID: 530 RVA: 0x00003770 File Offset: 0x00001970
		[JsonProperty]
		public bool Success { get; private set; }

		// Token: 0x06000213 RID: 531 RVA: 0x00003779 File Offset: 0x00001979
		public RequestJoinPlayerPartyMessageResult()
		{
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00003781 File Offset: 0x00001981
		public RequestJoinPlayerPartyMessageResult(bool success)
		{
			this.Success = success;
		}
	}
}
