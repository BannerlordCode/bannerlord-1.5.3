using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromBattleServer.ToBattleServerManager
{
	// Token: 0x020000DA RID: 218
	[MessageDescription("BattleServer", "BattleServerManager", false)]
	[Serializable]
	public class NewPlayerResponseMessage : Message
	{
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x00004CDA File Offset: 0x00002EDA
		// (set) Token: 0x0600040F RID: 1039 RVA: 0x00004CE2 File Offset: 0x00002EE2
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x00004CEB File Offset: 0x00002EEB
		// (set) Token: 0x06000411 RID: 1041 RVA: 0x00004CF3 File Offset: 0x00002EF3
		[JsonProperty]
		public PlayerBattleServerInformation PlayerBattleInformation { get; private set; }

		// Token: 0x06000412 RID: 1042 RVA: 0x00004CFC File Offset: 0x00002EFC
		public NewPlayerResponseMessage()
		{
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00004D04 File Offset: 0x00002F04
		public NewPlayerResponseMessage(PlayerId playerId, PlayerBattleServerInformation playerBattleInformation)
		{
			this.PlayerId = playerId;
			this.PlayerBattleInformation = playerBattleInformation;
		}
	}
}
