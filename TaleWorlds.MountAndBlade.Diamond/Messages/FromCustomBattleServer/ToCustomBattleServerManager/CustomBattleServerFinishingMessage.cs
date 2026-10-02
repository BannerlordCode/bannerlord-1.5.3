using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromCustomBattleServer.ToCustomBattleServerManager
{
	// Token: 0x02000004 RID: 4
	[MessageDescription("CustomBattleServer", "CustomBattleServerManager", true)]
	[Serializable]
	public class CustomBattleServerFinishingMessage : Message
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002117 File Offset: 0x00000317
		// (set) Token: 0x0600000E RID: 14 RVA: 0x0000211F File Offset: 0x0000031F
		[JsonProperty]
		public GameLog[] GameLogs { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002128 File Offset: 0x00000328
		// (set) Token: 0x06000010 RID: 16 RVA: 0x00002130 File Offset: 0x00000330
		[JsonProperty]
		public List<BadgeDataEntry> BadgeDataEntries { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002139 File Offset: 0x00000339
		// (set) Token: 0x06000012 RID: 18 RVA: 0x00002141 File Offset: 0x00000341
		[JsonProperty]
		public MultipleBattleResult BattleResult { get; private set; }

		// Token: 0x06000013 RID: 19 RVA: 0x0000214A File Offset: 0x0000034A
		public CustomBattleServerFinishingMessage()
		{
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002152 File Offset: 0x00000352
		public CustomBattleServerFinishingMessage(GameLog[] gameLogs, Dictionary<ValueTuple<PlayerId, string, string>, int> badgeDataDictionary, MultipleBattleResult battleResult)
		{
			this.GameLogs = gameLogs;
			this.BadgeDataEntries = BadgeDataEntry.ToList(badgeDataDictionary);
			this.BattleResult = battleResult;
		}
	}
}
