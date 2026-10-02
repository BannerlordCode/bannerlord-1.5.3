using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToLobbyServer
{
	// Token: 0x0200006F RID: 111
	[MessageDescription("CustomBattleServerManager", "CustomBattleServerManager", true)]
	[Serializable]
	public class ProcessBadgesAndStatsAfterCustomBattleServerFinishMessage : Message
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0000391D File Offset: 0x00001B1D
		// (set) Token: 0x0600023C RID: 572 RVA: 0x00003925 File Offset: 0x00001B25
		[JsonProperty]
		public List<BadgeDataEntry> BadgeDataEntries { get; private set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0000392E File Offset: 0x00001B2E
		// (set) Token: 0x0600023E RID: 574 RVA: 0x00003936 File Offset: 0x00001B36
		[JsonProperty]
		public PlayerId[] PlayerIds { get; private set; }

		// Token: 0x0600023F RID: 575 RVA: 0x0000393F File Offset: 0x00001B3F
		public ProcessBadgesAndStatsAfterCustomBattleServerFinishMessage()
		{
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00003947 File Offset: 0x00001B47
		public ProcessBadgesAndStatsAfterCustomBattleServerFinishMessage(List<BadgeDataEntry> badgeDataEntries, PlayerId[] playerIds)
		{
			this.BadgeDataEntries = badgeDataEntries;
			this.PlayerIds = playerIds;
		}
	}
}
