using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000B6 RID: 182
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetPeerTeam : GameNetworkMessage
	{
		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x0000CBAD File Offset: 0x0000ADAD
		// (set) Token: 0x06000754 RID: 1876 RVA: 0x0000CBB5 File Offset: 0x0000ADB5
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x0000CBBE File Offset: 0x0000ADBE
		// (set) Token: 0x06000756 RID: 1878 RVA: 0x0000CBC6 File Offset: 0x0000ADC6
		public int TeamIndex { get; private set; }

		// Token: 0x06000757 RID: 1879 RVA: 0x0000CBCF File Offset: 0x0000ADCF
		public SetPeerTeam(NetworkCommunicator peer, int teamIndex)
		{
			this.Peer = peer;
			this.TeamIndex = teamIndex;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0000CBE5 File Offset: 0x0000ADE5
		public SetPeerTeam()
		{
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0000CBF0 File Offset: 0x0000ADF0
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.TeamIndex = GameNetworkMessage.ReadTeamIndexFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0000CC1B File Offset: 0x0000AE1B
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteTeamIndexToPacket(this.TeamIndex);
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0000CC33 File Offset: 0x0000AE33
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0000CC38 File Offset: 0x0000AE38
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Set Team: ",
				this.TeamIndex,
				" of NetworkPeer with name: ",
				this.Peer.UserName,
				" and peer-index",
				this.Peer.Index
			});
		}
	}
}
