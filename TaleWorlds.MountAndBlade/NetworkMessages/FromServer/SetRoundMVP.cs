using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000B9 RID: 185
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetRoundMVP : GameNetworkMessage
	{
		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x0000CE35 File Offset: 0x0000B035
		// (set) Token: 0x06000772 RID: 1906 RVA: 0x0000CE3D File Offset: 0x0000B03D
		public NetworkCommunicator MVPPeer { get; private set; }

		// Token: 0x06000773 RID: 1907 RVA: 0x0000CE46 File Offset: 0x0000B046
		public SetRoundMVP(NetworkCommunicator mvpPeer, int mvpCount)
		{
			this.MVPPeer = mvpPeer;
			this.MVPCount = mvpCount;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0000CE5C File Offset: 0x0000B05C
		public SetRoundMVP()
		{
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0000CE64 File Offset: 0x0000B064
		protected override bool OnRead()
		{
			bool flag = true;
			this.MVPPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.MVPCount = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.RoundTotalCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0000CE94 File Offset: 0x0000B094
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.MVPPeer);
			GameNetworkMessage.WriteIntToPacket(this.MVPCount, CompressionBasic.RoundTotalCompressionInfo);
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0000CEB1 File Offset: 0x0000B0B1
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission | MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0000CEB9 File Offset: 0x0000B0B9
		protected override string OnGetLogFormat()
		{
			return "MVP selected as: " + this.MVPPeer.UserName + ".";
		}

		// Token: 0x040001AB RID: 427
		public int MVPCount;
	}
}
