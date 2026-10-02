using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000032 RID: 50
	[Serializable]
	public class GetAnotherPlayerDataMessageResult : FunctionResult
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00002CA0 File Offset: 0x00000EA0
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00002CA8 File Offset: 0x00000EA8
		[JsonProperty]
		public PlayerData AnotherPlayerData { get; private set; }

		// Token: 0x06000111 RID: 273 RVA: 0x00002CB1 File Offset: 0x00000EB1
		public GetAnotherPlayerDataMessageResult()
		{
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00002CB9 File Offset: 0x00000EB9
		public GetAnotherPlayerDataMessageResult(PlayerData playerData)
		{
			this.AnotherPlayerData = playerData;
		}
	}
}
