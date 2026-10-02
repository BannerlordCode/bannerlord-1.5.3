using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000052 RID: 82
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ChangeCulture : GameNetworkMessage
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x0000583A File Offset: 0x00003A3A
		// (set) Token: 0x060002C6 RID: 710 RVA: 0x00005842 File Offset: 0x00003A42
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x0000584B File Offset: 0x00003A4B
		// (set) Token: 0x060002C8 RID: 712 RVA: 0x00005853 File Offset: 0x00003A53
		public BasicCultureObject Culture { get; private set; }

		// Token: 0x060002C9 RID: 713 RVA: 0x0000585C File Offset: 0x00003A5C
		public ChangeCulture()
		{
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00005864 File Offset: 0x00003A64
		public ChangeCulture(MissionPeer peer, BasicCultureObject culture)
		{
			this.Peer = peer.GetNetworkPeer();
			this.Culture = culture;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000587F File Offset: 0x00003A7F
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteObjectReferenceToPacket(this.Culture, CompressionBasic.GUIDCompressionInfo);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000589C File Offset: 0x00003A9C
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.Culture = (BasicCultureObject)GameNetworkMessage.ReadObjectReferenceFromPacket(MBObjectManager.Instance, CompressionBasic.GUIDCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x000058D6 File Offset: 0x00003AD6
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x000058DE File Offset: 0x00003ADE
		protected override string OnGetLogFormat()
		{
			return "Requested culture: " + this.Culture.Name;
		}
	}
}
