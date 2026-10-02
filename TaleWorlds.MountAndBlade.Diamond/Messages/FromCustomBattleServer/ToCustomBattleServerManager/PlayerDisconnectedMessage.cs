using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromCustomBattleServer.ToCustomBattleServerManager
{
	// Token: 0x02000008 RID: 8
	[MessageDescription("CustomBattleServer", "CustomBattleServerManager", false)]
	[Serializable]
	public class PlayerDisconnectedMessage : Message
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002337 File Offset: 0x00000537
		// (set) Token: 0x06000030 RID: 48 RVA: 0x0000233F File Offset: 0x0000053F
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002348 File Offset: 0x00000548
		// (set) Token: 0x06000032 RID: 50 RVA: 0x00002350 File Offset: 0x00000550
		[JsonProperty]
		public DisconnectType Type { get; private set; }

		// Token: 0x06000033 RID: 51 RVA: 0x00002359 File Offset: 0x00000559
		public PlayerDisconnectedMessage()
		{
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002361 File Offset: 0x00000561
		public PlayerDisconnectedMessage(PlayerId playerId, DisconnectType type)
		{
			this.PlayerId = playerId;
			this.Type = type;
		}
	}
}
