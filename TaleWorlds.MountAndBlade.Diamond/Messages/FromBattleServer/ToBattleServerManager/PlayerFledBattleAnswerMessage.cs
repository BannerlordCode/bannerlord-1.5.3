using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromBattleServer.ToBattleServerManager
{
	// Token: 0x020000DC RID: 220
	[MessageDescription("BattleServer", "BattleServerManager", false)]
	[Serializable]
	public class PlayerFledBattleAnswerMessage : Message
	{
		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x00004D8B File Offset: 0x00002F8B
		// (set) Token: 0x0600041F RID: 1055 RVA: 0x00004D93 File Offset: 0x00002F93
		[JsonProperty]
		public BattleResult BattleResult { get; private set; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x00004D9C File Offset: 0x00002F9C
		// (set) Token: 0x06000421 RID: 1057 RVA: 0x00004DA4 File Offset: 0x00002FA4
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x00004DAD File Offset: 0x00002FAD
		// (set) Token: 0x06000423 RID: 1059 RVA: 0x00004DB5 File Offset: 0x00002FB5
		[JsonProperty]
		public bool IsAllowedLeave { get; private set; }

		// Token: 0x06000424 RID: 1060 RVA: 0x00004DBE File Offset: 0x00002FBE
		public PlayerFledBattleAnswerMessage()
		{
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00004DC6 File Offset: 0x00002FC6
		public PlayerFledBattleAnswerMessage(PlayerId playerId, BattleResult battleResult, bool isAllowedLeave)
		{
			this.PlayerId = playerId;
			this.BattleResult = battleResult;
			this.IsAllowedLeave = isAllowedLeave;
		}
	}
}
