using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromCustomBattleServerManager.ToCustomBattleServer
{
	// Token: 0x02000013 RID: 19
	[MessageDescription("CustomBattleServerManager", "CustomBattleServer", true)]
	[Serializable]
	public class RegisterCustomGameMessageResponseMessage : FunctionResult
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00002714 File Offset: 0x00000914
		// (set) Token: 0x06000087 RID: 135 RVA: 0x0000271C File Offset: 0x0000091C
		[JsonProperty]
		public bool ShouldReportActivities { get; private set; }

		// Token: 0x06000088 RID: 136 RVA: 0x00002725 File Offset: 0x00000925
		public RegisterCustomGameMessageResponseMessage()
		{
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000272D File Offset: 0x0000092D
		public RegisterCustomGameMessageResponseMessage(bool shouldReportActivities)
		{
			this.ShouldReportActivities = shouldReportActivities;
		}
	}
}
