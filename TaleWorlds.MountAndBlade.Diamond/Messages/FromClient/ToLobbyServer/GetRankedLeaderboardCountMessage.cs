using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000A8 RID: 168
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetRankedLeaderboardCountMessage : Message
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000300 RID: 768 RVA: 0x00004109 File Offset: 0x00002309
		// (set) Token: 0x06000301 RID: 769 RVA: 0x00004111 File Offset: 0x00002311
		[JsonProperty]
		public string GameType { get; private set; }

		// Token: 0x06000302 RID: 770 RVA: 0x0000411A File Offset: 0x0000231A
		public GetRankedLeaderboardCountMessage()
		{
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00004122 File Offset: 0x00002322
		public GetRankedLeaderboardCountMessage(string gameType)
		{
			this.GameType = gameType;
		}
	}
}
