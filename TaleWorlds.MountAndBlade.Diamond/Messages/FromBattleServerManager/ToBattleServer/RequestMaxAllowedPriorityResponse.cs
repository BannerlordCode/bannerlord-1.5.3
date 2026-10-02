using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromBattleServerManager.ToBattleServer
{
	// Token: 0x020000E6 RID: 230
	[MessageDescription("BattleServerManager", "BattleServer", true)]
	[Serializable]
	public class RequestMaxAllowedPriorityResponse : FunctionResult
	{
		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x00004F14 File Offset: 0x00003114
		// (set) Token: 0x06000445 RID: 1093 RVA: 0x00004F1C File Offset: 0x0000311C
		[JsonProperty]
		public sbyte Priority { get; private set; }

		// Token: 0x06000446 RID: 1094 RVA: 0x00004F25 File Offset: 0x00003125
		public RequestMaxAllowedPriorityResponse()
		{
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00004F2D File Offset: 0x0000312D
		public RequestMaxAllowedPriorityResponse(sbyte priority)
		{
			this.Priority = priority;
		}
	}
}
