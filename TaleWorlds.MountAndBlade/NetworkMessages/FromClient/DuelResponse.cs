using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200000C RID: 12
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class DuelResponse : GameNetworkMessage
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002785 File Offset: 0x00000985
		// (set) Token: 0x06000043 RID: 67 RVA: 0x0000278D File Offset: 0x0000098D
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002796 File Offset: 0x00000996
		// (set) Token: 0x06000045 RID: 69 RVA: 0x0000279E File Offset: 0x0000099E
		public bool Accepted { get; private set; }

		// Token: 0x06000046 RID: 70 RVA: 0x000027A7 File Offset: 0x000009A7
		public DuelResponse(NetworkCommunicator peer, bool accepted)
		{
			this.Peer = peer;
			this.Accepted = accepted;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000027BD File Offset: 0x000009BD
		public DuelResponse()
		{
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000027C8 File Offset: 0x000009C8
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.Accepted = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000027F3 File Offset: 0x000009F3
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteBoolToPacket(this.Accepted);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000280B File Offset: 0x00000A0B
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002813 File Offset: 0x00000A13
		protected override string OnGetLogFormat()
		{
			return "Duel Response: " + (this.Accepted ? " Accepted" : " Not accepted");
		}
	}
}
