using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000A6 RID: 166
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetBoundariesState : GameNetworkMessage
	{
		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x0000BA62 File Offset: 0x00009C62
		// (set) Token: 0x0600069F RID: 1695 RVA: 0x0000BA6A File Offset: 0x00009C6A
		public bool IsOutside { get; private set; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x0000BA73 File Offset: 0x00009C73
		// (set) Token: 0x060006A1 RID: 1697 RVA: 0x0000BA7B File Offset: 0x00009C7B
		public float StateStartTimeInSeconds { get; private set; }

		// Token: 0x060006A2 RID: 1698 RVA: 0x0000BA84 File Offset: 0x00009C84
		public SetBoundariesState()
		{
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0000BA8C File Offset: 0x00009C8C
		public SetBoundariesState(bool isOutside)
		{
			this.IsOutside = isOutside;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0000BA9B File Offset: 0x00009C9B
		public SetBoundariesState(bool isOutside, long stateStartTimeInTicks)
			: this(isOutside)
		{
			this.StateStartTimeInSeconds = (float)stateStartTimeInTicks / 10000000f;
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0000BAB2 File Offset: 0x00009CB2
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteBoolToPacket(this.IsOutside);
			if (this.IsOutside)
			{
				GameNetworkMessage.WriteFloatToPacket(this.StateStartTimeInSeconds, CompressionMatchmaker.MissionTimeCompressionInfo);
			}
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0000BAD8 File Offset: 0x00009CD8
		protected override bool OnRead()
		{
			bool flag = true;
			this.IsOutside = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			if (this.IsOutside)
			{
				this.StateStartTimeInSeconds = GameNetworkMessage.ReadFloatFromPacket(CompressionMatchmaker.MissionTimeCompressionInfo, ref flag);
			}
			return flag;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0000BB0F File Offset: 0x00009D0F
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0000BB17 File Offset: 0x00009D17
		protected override string OnGetLogFormat()
		{
			if (!this.IsOutside)
			{
				return "I am now inside the level boundaries";
			}
			return "I am now outside of the level boundaries";
		}
	}
}
