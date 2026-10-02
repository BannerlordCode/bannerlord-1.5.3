using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000BF RID: 191
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class ReportPlayerMessage : Message
	{
		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000378 RID: 888 RVA: 0x00004620 File Offset: 0x00002820
		// (set) Token: 0x06000379 RID: 889 RVA: 0x00004628 File Offset: 0x00002828
		[JsonProperty]
		public Guid GameId { get; private set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600037A RID: 890 RVA: 0x00004631 File Offset: 0x00002831
		// (set) Token: 0x0600037B RID: 891 RVA: 0x00004639 File Offset: 0x00002839
		[JsonProperty]
		public PlayerId ReportedPlayerId { get; private set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600037C RID: 892 RVA: 0x00004642 File Offset: 0x00002842
		// (set) Token: 0x0600037D RID: 893 RVA: 0x0000464A File Offset: 0x0000284A
		[JsonProperty]
		public string ReportedPlayerName { get; private set; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600037E RID: 894 RVA: 0x00004653 File Offset: 0x00002853
		// (set) Token: 0x0600037F RID: 895 RVA: 0x0000465B File Offset: 0x0000285B
		[JsonProperty]
		public PlayerReportType Type { get; private set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000380 RID: 896 RVA: 0x00004664 File Offset: 0x00002864
		// (set) Token: 0x06000381 RID: 897 RVA: 0x0000466C File Offset: 0x0000286C
		[JsonProperty]
		public string Message { get; private set; }

		// Token: 0x06000382 RID: 898 RVA: 0x00004675 File Offset: 0x00002875
		public ReportPlayerMessage()
		{
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000467D File Offset: 0x0000287D
		public ReportPlayerMessage(Guid gameId, PlayerId reportedPlayerId, string reportedPlayerName, PlayerReportType type, string message)
		{
			this.GameId = gameId;
			this.ReportedPlayerId = reportedPlayerId;
			this.ReportedPlayerName = reportedPlayerName;
			this.Type = type;
			this.Message = message;
		}
	}
}
