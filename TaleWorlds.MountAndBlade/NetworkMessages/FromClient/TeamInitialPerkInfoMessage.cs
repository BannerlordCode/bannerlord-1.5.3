using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000011 RID: 17
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class TeamInitialPerkInfoMessage : GameNetworkMessage
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00002A70 File Offset: 0x00000C70
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00002A78 File Offset: 0x00000C78
		public int[] Perks { get; private set; }

		// Token: 0x06000070 RID: 112 RVA: 0x00002A81 File Offset: 0x00000C81
		public TeamInitialPerkInfoMessage(int[] perks)
		{
			this.Perks = perks;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002A90 File Offset: 0x00000C90
		public TeamInitialPerkInfoMessage()
		{
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00002A98 File Offset: 0x00000C98
		protected override bool OnRead()
		{
			bool flag = true;
			this.Perks = new int[3];
			for (int i = 0; i < 3; i++)
			{
				this.Perks[i] = GameNetworkMessage.ReadIntFromPacket(CompressionMission.PerkIndexCompressionInfo, ref flag);
			}
			return flag;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002AD4 File Offset: 0x00000CD4
		protected override void OnWrite()
		{
			for (int i = 0; i < 3; i++)
			{
				GameNetworkMessage.WriteIntToPacket(this.Perks[i], CompressionMission.PerkIndexCompressionInfo);
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002AFF File Offset: 0x00000CFF
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Equipment;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002B04 File Offset: 0x00000D04
		protected override string OnGetLogFormat()
		{
			return "TeamInitialPerkInfoMessage";
		}
	}
}
