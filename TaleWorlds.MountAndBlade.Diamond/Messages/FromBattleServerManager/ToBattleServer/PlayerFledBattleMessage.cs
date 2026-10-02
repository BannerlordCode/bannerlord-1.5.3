using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromBattleServerManager.ToBattleServer
{
	// Token: 0x020000E5 RID: 229
	[MessageDescription("BattleServerManager", "BattleServer", true)]
	[Serializable]
	public class PlayerFledBattleMessage : Message
	{
		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x00004EEC File Offset: 0x000030EC
		// (set) Token: 0x06000441 RID: 1089 RVA: 0x00004EF4 File Offset: 0x000030F4
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x06000442 RID: 1090 RVA: 0x00004EFD File Offset: 0x000030FD
		public PlayerFledBattleMessage()
		{
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00004F05 File Offset: 0x00003105
		public PlayerFledBattleMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
