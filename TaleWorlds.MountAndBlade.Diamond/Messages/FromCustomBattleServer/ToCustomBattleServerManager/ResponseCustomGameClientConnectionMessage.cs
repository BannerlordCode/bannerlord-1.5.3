using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromCustomBattleServer.ToCustomBattleServerManager
{
	// Token: 0x0200000C RID: 12
	[MessageDescription("CustomBattleServer", "CustomBattleServerManager", false)]
	[Serializable]
	public class ResponseCustomGameClientConnectionMessage : Message
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000065 RID: 101 RVA: 0x000025BC File Offset: 0x000007BC
		// (set) Token: 0x06000066 RID: 102 RVA: 0x000025C4 File Offset: 0x000007C4
		[JsonProperty]
		public PlayerJoinGameResponseDataFromHost[] PlayerJoinData { get; private set; }

		// Token: 0x06000067 RID: 103 RVA: 0x000025CD File Offset: 0x000007CD
		public ResponseCustomGameClientConnectionMessage()
		{
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000025D5 File Offset: 0x000007D5
		public ResponseCustomGameClientConnectionMessage(PlayerJoinGameResponseDataFromHost[] playerJoinData)
		{
			this.PlayerJoinData = playerJoinData;
		}
	}
}
