using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromBattleServerManager.ToBattleServer
{
	// Token: 0x020000E2 RID: 226
	[MessageDescription("BattleServerManager", "BattleServer", true)]
	[Serializable]
	public class FriendlyDamageKickPlayerResponseMessage : Message
	{
		// Token: 0x17000150 RID: 336
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x00004E2B File Offset: 0x0000302B
		// (set) Token: 0x0600042F RID: 1071 RVA: 0x00004E33 File Offset: 0x00003033
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x06000430 RID: 1072 RVA: 0x00004E3C File Offset: 0x0000303C
		public FriendlyDamageKickPlayerResponseMessage()
		{
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00004E44 File Offset: 0x00003044
		public FriendlyDamageKickPlayerResponseMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
