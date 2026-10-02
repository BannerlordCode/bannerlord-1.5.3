using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200003B RID: 59
	[Serializable]
	public class GetOtherPlayersStateMessageResult : FunctionResult
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00002E0E File Offset: 0x0000100E
		// (set) Token: 0x06000134 RID: 308 RVA: 0x00002E16 File Offset: 0x00001016
		[JsonProperty]
		public List<ValueTuple<PlayerId, AnotherPlayerData>> States { get; private set; }

		// Token: 0x06000135 RID: 309 RVA: 0x00002E1F File Offset: 0x0000101F
		public GetOtherPlayersStateMessageResult()
		{
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00002E27 File Offset: 0x00001027
		public GetOtherPlayersStateMessageResult(List<ValueTuple<PlayerId, AnotherPlayerData>> states)
		{
			this.States = states;
		}
	}
}
