using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200000A RID: 10
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class CultureVoteClient : GameNetworkMessage
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000025E5 File Offset: 0x000007E5
		// (set) Token: 0x06000031 RID: 49 RVA: 0x000025ED File Offset: 0x000007ED
		public BasicCultureObject VotedCulture { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000025F6 File Offset: 0x000007F6
		// (set) Token: 0x06000033 RID: 51 RVA: 0x000025FE File Offset: 0x000007FE
		public CultureVoteTypes VotedType { get; private set; }

		// Token: 0x06000034 RID: 52 RVA: 0x00002607 File Offset: 0x00000807
		public CultureVoteClient()
		{
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000260F File Offset: 0x0000080F
		public CultureVoteClient(CultureVoteTypes type, BasicCultureObject culture)
		{
			this.VotedType = type;
			this.VotedCulture = culture;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002625 File Offset: 0x00000825
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.VotedType, CompressionMission.TeamSideCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(MBObjectManager.Instance.GetObjectTypeList<BasicCultureObject>().IndexOf(this.VotedCulture), CompressionBasic.CultureIndexCompressionInfo);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002658 File Offset: 0x00000858
		protected override bool OnRead()
		{
			bool flag = true;
			this.VotedType = (CultureVoteTypes)GameNetworkMessage.ReadIntFromPacket(CompressionMission.TeamSideCompressionInfo, ref flag);
			int num = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.CultureIndexCompressionInfo, ref flag);
			if (flag)
			{
				this.VotedCulture = MBObjectManager.Instance.GetObjectTypeList<BasicCultureObject>()[num];
			}
			return flag;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000026A0 File Offset: 0x000008A0
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000026A8 File Offset: 0x000008A8
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
