using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000043 RID: 67
	[Serializable]
	public class GetPublishedLobbyNewsMessageResult : FunctionResult
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00002F4E File Offset: 0x0000114E
		// (set) Token: 0x06000154 RID: 340 RVA: 0x00002F56 File Offset: 0x00001156
		[JsonProperty]
		public PublishedLobbyNewsArticle[] Content { get; private set; }

		// Token: 0x06000155 RID: 341 RVA: 0x00002F5F File Offset: 0x0000115F
		public GetPublishedLobbyNewsMessageResult()
		{
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00002F67 File Offset: 0x00001167
		public GetPublishedLobbyNewsMessageResult(PublishedLobbyNewsArticle[] content)
		{
			this.Content = content;
		}
	}
}
