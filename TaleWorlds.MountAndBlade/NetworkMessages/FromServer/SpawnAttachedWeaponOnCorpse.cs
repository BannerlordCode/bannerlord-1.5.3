using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000C6 RID: 198
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SpawnAttachedWeaponOnCorpse : GameNetworkMessage
	{
		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x0000DA43 File Offset: 0x0000BC43
		// (set) Token: 0x06000800 RID: 2048 RVA: 0x0000DA4B File Offset: 0x0000BC4B
		public int AgentIndex { get; private set; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x0000DA54 File Offset: 0x0000BC54
		// (set) Token: 0x06000802 RID: 2050 RVA: 0x0000DA5C File Offset: 0x0000BC5C
		public int AttachedIndex { get; private set; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x0000DA65 File Offset: 0x0000BC65
		// (set) Token: 0x06000804 RID: 2052 RVA: 0x0000DA6D File Offset: 0x0000BC6D
		public int ForcedIndex { get; private set; }

		// Token: 0x06000805 RID: 2053 RVA: 0x0000DA76 File Offset: 0x0000BC76
		public SpawnAttachedWeaponOnCorpse(int agentIndex, int attachedIndex, int forcedIndex)
		{
			this.AgentIndex = agentIndex;
			this.AttachedIndex = attachedIndex;
			this.ForcedIndex = forcedIndex;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0000DA93 File Offset: 0x0000BC93
		public SpawnAttachedWeaponOnCorpse()
		{
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0000DA9C File Offset: 0x0000BC9C
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.AttachedIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.WeaponAttachmentIndexCompressionInfo, ref flag);
			this.ForcedIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.MissionObjectIDCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0000DADD File Offset: 0x0000BCDD
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket(this.AttachedIndex, CompressionMission.WeaponAttachmentIndexCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.ForcedIndex, CompressionBasic.MissionObjectIDCompressionInfo);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0000DB0A File Offset: 0x0000BD0A
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items;
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0000DB0E File Offset: 0x0000BD0E
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "SpawnAttachedWeaponOnCorpse with agent-index: ", this.AgentIndex, ", and with ID: ", this.ForcedIndex });
		}
	}
}
