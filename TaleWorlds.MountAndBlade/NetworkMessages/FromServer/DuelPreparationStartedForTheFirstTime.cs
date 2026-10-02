using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000049 RID: 73
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class DuelPreparationStartedForTheFirstTime : GameNetworkMessage
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000261 RID: 609 RVA: 0x00004E9B File Offset: 0x0000309B
		// (set) Token: 0x06000262 RID: 610 RVA: 0x00004EA3 File Offset: 0x000030A3
		public NetworkCommunicator RequesterPeer { get; private set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000263 RID: 611 RVA: 0x00004EAC File Offset: 0x000030AC
		// (set) Token: 0x06000264 RID: 612 RVA: 0x00004EB4 File Offset: 0x000030B4
		public NetworkCommunicator RequesteePeer { get; private set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000265 RID: 613 RVA: 0x00004EBD File Offset: 0x000030BD
		// (set) Token: 0x06000266 RID: 614 RVA: 0x00004EC5 File Offset: 0x000030C5
		public int AreaIndex { get; private set; }

		// Token: 0x06000267 RID: 615 RVA: 0x00004ECE File Offset: 0x000030CE
		public DuelPreparationStartedForTheFirstTime(NetworkCommunicator requesterPeer, NetworkCommunicator requesteePeer, int areaIndex)
		{
			this.RequesterPeer = requesterPeer;
			this.RequesteePeer = requesteePeer;
			this.AreaIndex = areaIndex;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00004EEB File Offset: 0x000030EB
		public DuelPreparationStartedForTheFirstTime()
		{
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00004EF4 File Offset: 0x000030F4
		protected override bool OnRead()
		{
			bool flag = true;
			this.RequesterPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.RequesteePeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.AreaIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.DuelAreaIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00004F32 File Offset: 0x00003132
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.RequesterPeer);
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.RequesteePeer);
			GameNetworkMessage.WriteIntToPacket(this.AreaIndex, CompressionMission.DuelAreaIndexCompressionInfo);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00004F5A File Offset: 0x0000315A
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00004F64 File Offset: 0x00003164
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Duel started between agent with name: ",
				this.RequesteePeer.UserName,
				" and index: ",
				this.RequesteePeer.Index,
				" and agent with name: ",
				this.RequesterPeer.UserName,
				" and index: ",
				this.RequesterPeer.Index
			});
		}
	}
}
