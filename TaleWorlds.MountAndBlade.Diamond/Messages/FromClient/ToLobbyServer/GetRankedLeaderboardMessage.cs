using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000A9 RID: 169
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetRankedLeaderboardMessage : Message
	{
		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000304 RID: 772 RVA: 0x00004131 File Offset: 0x00002331
		// (set) Token: 0x06000305 RID: 773 RVA: 0x00004139 File Offset: 0x00002339
		[JsonProperty]
		public string GameType { get; private set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000306 RID: 774 RVA: 0x00004142 File Offset: 0x00002342
		// (set) Token: 0x06000307 RID: 775 RVA: 0x0000414A File Offset: 0x0000234A
		[JsonProperty]
		public int StartIndex { get; private set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000308 RID: 776 RVA: 0x00004153 File Offset: 0x00002353
		// (set) Token: 0x06000309 RID: 777 RVA: 0x0000415B File Offset: 0x0000235B
		[JsonProperty]
		public int Count { get; private set; }

		// Token: 0x0600030A RID: 778 RVA: 0x00004164 File Offset: 0x00002364
		public GetRankedLeaderboardMessage()
		{
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000416C File Offset: 0x0000236C
		public GetRankedLeaderboardMessage(string gameType, int startIndex, int count)
		{
			this.GameType = gameType;
			this.StartIndex = startIndex;
			this.Count = count;
		}
	}
}
