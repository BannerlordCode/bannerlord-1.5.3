using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000033 RID: 51
	[Serializable]
	public class GetAnotherPlayerStateMessageResult : FunctionResult
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00002CC8 File Offset: 0x00000EC8
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00002CD0 File Offset: 0x00000ED0
		[JsonProperty]
		public AnotherPlayerData AnotherPlayerData { get; private set; }

		// Token: 0x06000115 RID: 277 RVA: 0x00002CD9 File Offset: 0x00000ED9
		public GetAnotherPlayerStateMessageResult()
		{
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00002CE1 File Offset: 0x00000EE1
		public GetAnotherPlayerStateMessageResult(AnotherPlayerState anotherPlayerState, int anotherPlayerExperience)
		{
			this.AnotherPlayerData = new AnotherPlayerData(anotherPlayerState, anotherPlayerExperience);
		}
	}
}
