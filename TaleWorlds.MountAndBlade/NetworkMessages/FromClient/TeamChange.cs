using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200001F RID: 31
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class TeamChange : GameNetworkMessage
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00003400 File Offset: 0x00001600
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00003408 File Offset: 0x00001608
		public bool AutoAssign { get; private set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00003411 File Offset: 0x00001611
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00003419 File Offset: 0x00001619
		public int TeamIndex { get; private set; }

		// Token: 0x060000EE RID: 238 RVA: 0x00003422 File Offset: 0x00001622
		public TeamChange(bool autoAssign, int teamIndex)
		{
			this.AutoAssign = autoAssign;
			this.TeamIndex = teamIndex;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00003438 File Offset: 0x00001638
		public TeamChange()
		{
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00003440 File Offset: 0x00001640
		protected override bool OnRead()
		{
			bool flag = true;
			this.AutoAssign = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			if (!this.AutoAssign)
			{
				this.TeamIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.TeamCompressionInfo, ref flag);
			}
			return flag;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00003477 File Offset: 0x00001677
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteBoolToPacket(this.AutoAssign);
			if (!this.AutoAssign)
			{
				GameNetworkMessage.WriteIntToPacket(this.TeamIndex, CompressionMission.TeamCompressionInfo);
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000349C File Offset: 0x0000169C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000034A4 File Offset: 0x000016A4
		protected override string OnGetLogFormat()
		{
			return "Changed team to: " + this.TeamIndex;
		}
	}
}
