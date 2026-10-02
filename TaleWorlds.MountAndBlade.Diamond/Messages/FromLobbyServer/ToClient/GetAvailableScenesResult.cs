using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000034 RID: 52
	[Serializable]
	public class GetAvailableScenesResult : FunctionResult
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00002CF6 File Offset: 0x00000EF6
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00002CFE File Offset: 0x00000EFE
		[JsonProperty]
		public AvailableScenes AvailableScenes { get; private set; }

		// Token: 0x06000119 RID: 281 RVA: 0x00002D07 File Offset: 0x00000F07
		public GetAvailableScenesResult()
		{
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00002D0F File Offset: 0x00000F0F
		public GetAvailableScenesResult(AvailableScenes scenes)
		{
			this.AvailableScenes = scenes;
		}
	}
}
