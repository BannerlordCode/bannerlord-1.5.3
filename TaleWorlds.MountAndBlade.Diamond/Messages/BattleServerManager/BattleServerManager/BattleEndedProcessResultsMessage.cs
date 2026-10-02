using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.BattleServerManager.BattleServerManager
{
	// Token: 0x020000E9 RID: 233
	[MessageDescription("BattleServerManager", "BattleServerManager", true)]
	[Serializable]
	public class BattleEndedProcessResultsMessage : Message
	{
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x00005184 File Offset: 0x00003384
		// (set) Token: 0x06000478 RID: 1144 RVA: 0x0000518C File Offset: 0x0000338C
		[JsonProperty]
		public BattleResult BattleResult { get; private set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x00005195 File Offset: 0x00003395
		// (set) Token: 0x0600047A RID: 1146 RVA: 0x0000519D File Offset: 0x0000339D
		[JsonProperty]
		public List<BadgeDataEntry> BadgeDateEntries { get; private set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x000051A6 File Offset: 0x000033A6
		// (set) Token: 0x0600047C RID: 1148 RVA: 0x000051AE File Offset: 0x000033AE
		[JsonProperty]
		public string BattleGameType { get; private set; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x000051B7 File Offset: 0x000033B7
		// (set) Token: 0x0600047E RID: 1150 RVA: 0x000051BF File Offset: 0x000033BF
		[JsonProperty]
		public string Region { get; private set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x000051C8 File Offset: 0x000033C8
		// (set) Token: 0x06000480 RID: 1152 RVA: 0x000051D0 File Offset: 0x000033D0
		[TupleElementNames(new string[] { "playerBattleInfo", "shouldSendMessage", "hasLeftGame" })]
		[JsonProperty]
		public List<ValueTuple<PlayerBattleInfo, bool, bool>> PlayersForResults
		{
			[return: TupleElementNames(new string[] { "playerBattleInfo", "shouldSendMessage", "hasLeftGame" })]
			get;
			[param: TupleElementNames(new string[] { "playerBattleInfo", "shouldSendMessage", "hasLeftGame" })]
			private set;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x000051D9 File Offset: 0x000033D9
		public BattleEndedProcessResultsMessage()
		{
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x000051E1 File Offset: 0x000033E1
		public BattleEndedProcessResultsMessage(BattleResult battleResult, List<BadgeDataEntry> badgeDateEntries, string battleGameType, string region, [TupleElementNames(new string[] { "playerBattleInfo", "shouldSendMessage", "hasLeftGame" })] List<ValueTuple<PlayerBattleInfo, bool, bool>> playersForResults)
		{
			this.BattleResult = battleResult;
			this.BadgeDateEntries = badgeDateEntries;
			this.BattleGameType = battleGameType;
			this.Region = region;
			this.PlayersForResults = playersForResults;
		}
	}
}
