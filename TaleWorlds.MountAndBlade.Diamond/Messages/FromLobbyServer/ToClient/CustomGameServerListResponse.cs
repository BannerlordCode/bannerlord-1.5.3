using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200002C RID: 44
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class CustomGameServerListResponse : FunctionResult
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00002BA0 File Offset: 0x00000DA0
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00002BA8 File Offset: 0x00000DA8
		[JsonProperty]
		public AvailableCustomGames AvailableCustomGames { get; private set; }

		// Token: 0x060000F8 RID: 248 RVA: 0x00002BB1 File Offset: 0x00000DB1
		public CustomGameServerListResponse()
		{
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00002BB9 File Offset: 0x00000DB9
		public CustomGameServerListResponse(AvailableCustomGames availableCustomGames)
		{
			this.AvailableCustomGames = availableCustomGames;
		}
	}
}
