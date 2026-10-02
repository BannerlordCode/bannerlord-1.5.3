using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200004E RID: 78
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class UpdateSelectedTroopIndex : GameNetworkMessage
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000295 RID: 661 RVA: 0x00005375 File Offset: 0x00003575
		// (set) Token: 0x06000296 RID: 662 RVA: 0x0000537D File Offset: 0x0000357D
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000297 RID: 663 RVA: 0x00005386 File Offset: 0x00003586
		// (set) Token: 0x06000298 RID: 664 RVA: 0x0000538E File Offset: 0x0000358E
		public int SelectedTroopIndex { get; private set; }

		// Token: 0x06000299 RID: 665 RVA: 0x00005397 File Offset: 0x00003597
		public UpdateSelectedTroopIndex(NetworkCommunicator peer, int selectedTroopIndex)
		{
			this.Peer = peer;
			this.SelectedTroopIndex = selectedTroopIndex;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x000053AD File Offset: 0x000035AD
		public UpdateSelectedTroopIndex()
		{
		}

		// Token: 0x0600029B RID: 667 RVA: 0x000053B8 File Offset: 0x000035B8
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.SelectedTroopIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.SelectedTroopIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x000053E8 File Offset: 0x000035E8
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteIntToPacket(this.SelectedTroopIndex, CompressionMission.SelectedTroopIndexCompressionInfo);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00005405 File Offset: 0x00003605
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Equipment;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000540C File Offset: 0x0000360C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Update SelectedTroopIndex to: ",
				this.SelectedTroopIndex,
				", on peer: ",
				this.Peer.UserName,
				" with peer-index:",
				this.Peer.Index
			});
		}
	}
}
