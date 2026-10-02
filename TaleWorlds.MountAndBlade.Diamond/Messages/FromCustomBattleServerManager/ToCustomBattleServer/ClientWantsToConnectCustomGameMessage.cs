using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromCustomBattleServerManager.ToCustomBattleServer
{
	// Token: 0x02000010 RID: 16
	[MessageDescription("CustomBattleServerManager", "CustomBattleServer", true)]
	[Serializable]
	public class ClientWantsToConnectCustomGameMessage : Message
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600007D RID: 125 RVA: 0x000026BC File Offset: 0x000008BC
		// (set) Token: 0x0600007E RID: 126 RVA: 0x000026C4 File Offset: 0x000008C4
		[JsonProperty]
		public PlayerJoinGameData[] PlayerJoinGameData { get; private set; }

		// Token: 0x0600007F RID: 127 RVA: 0x000026CD File Offset: 0x000008CD
		public ClientWantsToConnectCustomGameMessage()
		{
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000026D5 File Offset: 0x000008D5
		public ClientWantsToConnectCustomGameMessage(PlayerJoinGameData[] playerJoinGameData)
		{
			this.PlayerJoinGameData = playerJoinGameData;
		}
	}
}
