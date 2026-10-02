using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000092 RID: 146
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class InitializeCustomGameMessage : GameNetworkMessage
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x0000A7D8 File Offset: 0x000089D8
		// (set) Token: 0x060005BD RID: 1469 RVA: 0x0000A7E0 File Offset: 0x000089E0
		public bool InMission { get; private set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x0000A7E9 File Offset: 0x000089E9
		// (set) Token: 0x060005BF RID: 1471 RVA: 0x0000A7F1 File Offset: 0x000089F1
		public string GameType { get; private set; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x0000A7FA File Offset: 0x000089FA
		// (set) Token: 0x060005C1 RID: 1473 RVA: 0x0000A802 File Offset: 0x00008A02
		public string Map { get; private set; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x0000A80B File Offset: 0x00008A0B
		// (set) Token: 0x060005C3 RID: 1475 RVA: 0x0000A813 File Offset: 0x00008A13
		public int BattleIndex { get; private set; }

		// Token: 0x060005C4 RID: 1476 RVA: 0x0000A81C File Offset: 0x00008A1C
		public InitializeCustomGameMessage(bool inMission, string gameType, string map, int battleIndex)
		{
			this.InMission = inMission;
			this.GameType = gameType;
			this.Map = map;
			this.BattleIndex = battleIndex;
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0000A841 File Offset: 0x00008A41
		public InitializeCustomGameMessage()
		{
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0000A84C File Offset: 0x00008A4C
		protected override bool OnRead()
		{
			bool flag = true;
			this.InMission = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.GameType = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.Map = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.BattleIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AutomatedBattleIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0000A895 File Offset: 0x00008A95
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteBoolToPacket(this.InMission);
			GameNetworkMessage.WriteStringToPacket(this.GameType);
			GameNetworkMessage.WriteStringToPacket(this.Map);
			GameNetworkMessage.WriteIntToPacket(this.BattleIndex, CompressionMission.AutomatedBattleIndexCompressionInfo);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0000A8C8 File Offset: 0x00008AC8
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0000A8D0 File Offset: 0x00008AD0
		protected override string OnGetLogFormat()
		{
			return "Initialize Custom Game";
		}
	}
}
