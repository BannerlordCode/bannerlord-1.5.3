using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000094 RID: 148
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class LoadMission : GameNetworkMessage
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0000A9E5 File Offset: 0x00008BE5
		// (set) Token: 0x060005D7 RID: 1495 RVA: 0x0000A9ED File Offset: 0x00008BED
		public string GameType { get; private set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0000A9F6 File Offset: 0x00008BF6
		// (set) Token: 0x060005D9 RID: 1497 RVA: 0x0000A9FE File Offset: 0x00008BFE
		public string Map { get; private set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0000AA07 File Offset: 0x00008C07
		// (set) Token: 0x060005DB RID: 1499 RVA: 0x0000AA0F File Offset: 0x00008C0F
		public int BattleIndex { get; private set; }

		// Token: 0x060005DC RID: 1500 RVA: 0x0000AA18 File Offset: 0x00008C18
		public LoadMission(string gameType, string map, int battleIndex)
		{
			this.GameType = gameType;
			this.Map = map;
			this.BattleIndex = battleIndex;
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0000AA35 File Offset: 0x00008C35
		public LoadMission()
		{
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0000AA40 File Offset: 0x00008C40
		protected override bool OnRead()
		{
			bool flag = true;
			this.GameType = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.Map = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.BattleIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AutomatedBattleIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0000AA7C File Offset: 0x00008C7C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.GameType);
			GameNetworkMessage.WriteStringToPacket(this.Map);
			GameNetworkMessage.WriteIntToPacket(this.BattleIndex, CompressionMission.AutomatedBattleIndexCompressionInfo);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0000AAA4 File Offset: 0x00008CA4
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0000AAAC File Offset: 0x00008CAC
		protected override string OnGetLogFormat()
		{
			return "Load Mission";
		}
	}
}
