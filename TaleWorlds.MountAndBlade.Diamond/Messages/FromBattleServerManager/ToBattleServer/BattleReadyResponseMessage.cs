using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromBattleServerManager.ToBattleServer
{
	// Token: 0x020000E0 RID: 224
	[MessageDescription("BattleServerManager", "BattleServer", true)]
	[Serializable]
	public class BattleReadyResponseMessage : FunctionResult
	{
		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x00004DFB File Offset: 0x00002FFB
		// (set) Token: 0x0600042A RID: 1066 RVA: 0x00004E03 File Offset: 0x00003003
		[JsonProperty]
		public bool ShouldReportActivities { get; private set; }

		// Token: 0x0600042B RID: 1067 RVA: 0x00004E0C File Offset: 0x0000300C
		public BattleReadyResponseMessage()
		{
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00004E14 File Offset: 0x00003014
		public BattleReadyResponseMessage(bool shouldReportActivities)
		{
			this.ShouldReportActivities = shouldReportActivities;
		}
	}
}
