using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000045 RID: 69
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class CultureVoteServer : GameNetworkMessage
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000233 RID: 563 RVA: 0x00004A19 File Offset: 0x00002C19
		// (set) Token: 0x06000234 RID: 564 RVA: 0x00004A21 File Offset: 0x00002C21
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000235 RID: 565 RVA: 0x00004A2A File Offset: 0x00002C2A
		// (set) Token: 0x06000236 RID: 566 RVA: 0x00004A32 File Offset: 0x00002C32
		public BasicCultureObject VotedCulture { get; private set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000237 RID: 567 RVA: 0x00004A3B File Offset: 0x00002C3B
		// (set) Token: 0x06000238 RID: 568 RVA: 0x00004A43 File Offset: 0x00002C43
		public CultureVoteTypes VotedType { get; private set; }

		// Token: 0x06000239 RID: 569 RVA: 0x00004A4C File Offset: 0x00002C4C
		public CultureVoteServer()
		{
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00004A54 File Offset: 0x00002C54
		public CultureVoteServer(NetworkCommunicator peer, CultureVoteTypes type, BasicCultureObject culture)
		{
			this.Peer = peer;
			this.VotedType = type;
			this.VotedCulture = culture;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00004A74 File Offset: 0x00002C74
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteIntToPacket((int)this.VotedType, CompressionMission.TeamSideCompressionInfo);
			MBReadOnlyList<BasicCultureObject> objectTypeList = MBObjectManager.Instance.GetObjectTypeList<BasicCultureObject>();
			GameNetworkMessage.WriteIntToPacket((this.VotedCulture == null) ? (-1) : objectTypeList.IndexOf(this.VotedCulture), CompressionBasic.CultureIndexCompressionInfo);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00004AC8 File Offset: 0x00002CC8
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.VotedType = (CultureVoteTypes)GameNetworkMessage.ReadIntFromPacket(CompressionMission.TeamSideCompressionInfo, ref flag);
			int num = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.CultureIndexCompressionInfo, ref flag);
			if (flag)
			{
				MBReadOnlyList<BasicCultureObject> objectTypeList = MBObjectManager.Instance.GetObjectTypeList<BasicCultureObject>();
				this.VotedCulture = ((num < 0) ? null : objectTypeList[num]);
			}
			return flag;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00004B27 File Offset: 0x00002D27
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00004B30 File Offset: 0x00002D30
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Culture ",
				this.VotedCulture.Name,
				" has been ",
				this.VotedType.ToString().ToLower(),
				(this.VotedType == CultureVoteTypes.Ban) ? "ned." : "ed."
			});
		}
	}
}
