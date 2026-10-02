using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200000F RID: 15
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class RequestPerkChange : GameNetworkMessage
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002921 File Offset: 0x00000B21
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00002929 File Offset: 0x00000B29
		public int PerkListIndex { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00002932 File Offset: 0x00000B32
		// (set) Token: 0x0600005F RID: 95 RVA: 0x0000293A File Offset: 0x00000B3A
		public int PerkIndex { get; private set; }

		// Token: 0x06000060 RID: 96 RVA: 0x00002943 File Offset: 0x00000B43
		public RequestPerkChange(int perkListIndex, int perkIndex)
		{
			this.PerkListIndex = perkListIndex;
			this.PerkIndex = perkIndex;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002959 File Offset: 0x00000B59
		public RequestPerkChange()
		{
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002964 File Offset: 0x00000B64
		protected override bool OnRead()
		{
			bool flag = true;
			this.PerkListIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.PerkListIndexCompressionInfo, ref flag);
			this.PerkIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.PerkIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002998 File Offset: 0x00000B98
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.PerkListIndex, CompressionMission.PerkListIndexCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.PerkIndex, CompressionMission.PerkIndexCompressionInfo);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000029BA File Offset: 0x00000BBA
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Equipment;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000029BF File Offset: 0x00000BBF
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Requesting perk selection in list ", this.PerkListIndex, " change to ", this.PerkIndex });
		}
	}
}
