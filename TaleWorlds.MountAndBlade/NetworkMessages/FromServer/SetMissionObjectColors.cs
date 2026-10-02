using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000AC RID: 172
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectColors : GameNetworkMessage
	{
		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x0000C0F1 File Offset: 0x0000A2F1
		// (set) Token: 0x060006E6 RID: 1766 RVA: 0x0000C0F9 File Offset: 0x0000A2F9
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x0000C102 File Offset: 0x0000A302
		// (set) Token: 0x060006E8 RID: 1768 RVA: 0x0000C10A File Offset: 0x0000A30A
		public uint Color { get; private set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x0000C113 File Offset: 0x0000A313
		// (set) Token: 0x060006EA RID: 1770 RVA: 0x0000C11B File Offset: 0x0000A31B
		public uint Color2 { get; private set; }

		// Token: 0x060006EB RID: 1771 RVA: 0x0000C124 File Offset: 0x0000A324
		public SetMissionObjectColors(MissionObjectId missionObjectId, uint color, uint color2)
		{
			this.MissionObjectId = missionObjectId;
			this.Color = color;
			this.Color2 = color2;
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0000C141 File Offset: 0x0000A341
		public SetMissionObjectColors()
		{
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0000C14C File Offset: 0x0000A34C
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Color = GameNetworkMessage.ReadUintFromPacket(CompressionBasic.ColorCompressionInfo, ref flag);
			this.Color2 = GameNetworkMessage.ReadUintFromPacket(CompressionBasic.ColorCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0000C18D File Offset: 0x0000A38D
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteUintToPacket(this.Color, CompressionBasic.ColorCompressionInfo);
			GameNetworkMessage.WriteUintToPacket(this.Color2, CompressionBasic.ColorCompressionInfo);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0000C1BA File Offset: 0x0000A3BA
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjects;
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0000C1C2 File Offset: 0x0000A3C2
		protected override string OnGetLogFormat()
		{
			return "Set Colors of MissionObject with ID: " + this.MissionObjectId;
		}
	}
}
