using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Ranked;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000017 RID: 23
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class BattleOverMessage : Message
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000027AC File Offset: 0x000009AC
		// (set) Token: 0x06000096 RID: 150 RVA: 0x000027B4 File Offset: 0x000009B4
		[JsonProperty]
		public int OldExperience { get; private set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000027BD File Offset: 0x000009BD
		// (set) Token: 0x06000098 RID: 152 RVA: 0x000027C5 File Offset: 0x000009C5
		[JsonProperty]
		public int NewExperience { get; private set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000027CE File Offset: 0x000009CE
		// (set) Token: 0x0600009A RID: 154 RVA: 0x000027D6 File Offset: 0x000009D6
		[JsonProperty]
		public List<string> EarnedBadges { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600009B RID: 155 RVA: 0x000027DF File Offset: 0x000009DF
		// (set) Token: 0x0600009C RID: 156 RVA: 0x000027E7 File Offset: 0x000009E7
		[JsonProperty]
		public int GoldGained { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600009D RID: 157 RVA: 0x000027F0 File Offset: 0x000009F0
		// (set) Token: 0x0600009E RID: 158 RVA: 0x000027F8 File Offset: 0x000009F8
		[JsonProperty]
		public RankBarInfo OldInfo { get; private set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00002801 File Offset: 0x00000A01
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00002809 File Offset: 0x00000A09
		[JsonProperty]
		public RankBarInfo NewInfo { get; private set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00002812 File Offset: 0x00000A12
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x0000281A File Offset: 0x00000A1A
		[JsonProperty]
		public BattleCancelReason BattleCancelReason { get; private set; }

		// Token: 0x060000A3 RID: 163 RVA: 0x00002823 File Offset: 0x00000A23
		public BattleOverMessage()
		{
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000282B File Offset: 0x00000A2B
		public BattleOverMessage(int oldExperience, int newExperience, List<string> earnedBadges, int goldGained, BattleCancelReason battleCancelReason = BattleCancelReason.None)
		{
			this.OldExperience = oldExperience;
			this.NewExperience = newExperience;
			this.EarnedBadges = earnedBadges;
			this.GoldGained = goldGained;
			this.BattleCancelReason = battleCancelReason;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00002858 File Offset: 0x00000A58
		public BattleOverMessage(BattleCancelReason battleCancelReason)
		{
			this.BattleCancelReason = battleCancelReason;
		}
	}
}
