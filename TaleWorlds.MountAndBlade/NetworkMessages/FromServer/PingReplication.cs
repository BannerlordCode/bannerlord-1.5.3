using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000D9 RID: 217
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class PingReplication : GameNetworkMessage
	{
		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x0000EF6B File Offset: 0x0000D16B
		// (set) Token: 0x060008E6 RID: 2278 RVA: 0x0000EF73 File Offset: 0x0000D173
		internal NetworkCommunicator Peer { get; private set; }

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x0000EF7C File Offset: 0x0000D17C
		// (set) Token: 0x060008E8 RID: 2280 RVA: 0x0000EF84 File Offset: 0x0000D184
		internal int PingValue { get; private set; }

		// Token: 0x060008E9 RID: 2281 RVA: 0x0000EF8D File Offset: 0x0000D18D
		public PingReplication()
		{
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0000EF95 File Offset: 0x0000D195
		internal PingReplication(NetworkCommunicator peer, int ping)
		{
			this.Peer = peer;
			this.PingValue = ping;
			if (this.PingValue > 1023)
			{
				this.PingValue = 1023;
			}
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0000EFC4 File Offset: 0x0000D1C4
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, true);
			this.PingValue = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PingValueCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0000EFF4 File Offset: 0x0000D1F4
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteIntToPacket(this.PingValue, CompressionBasic.PingValueCompressionInfo);
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0000F011 File Offset: 0x0000D211
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionDetailed;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0000F019 File Offset: 0x0000D219
		protected override string OnGetLogFormat()
		{
			return "PingReplication";
		}

		// Token: 0x04000206 RID: 518
		public const int MaxPingToReplicate = 1023;
	}
}
