using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000018 RID: 24
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class IntermissionVote : GameNetworkMessage
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000AF RID: 175 RVA: 0x0000303C File Offset: 0x0000123C
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00003044 File Offset: 0x00001244
		public int VoteCount { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x0000304D File Offset: 0x0000124D
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x00003055 File Offset: 0x00001255
		public string ItemID { get; private set; }

		// Token: 0x060000B3 RID: 179 RVA: 0x0000305E File Offset: 0x0000125E
		public IntermissionVote(string itemID, int voteCount)
		{
			this.VoteCount = voteCount;
			this.ItemID = itemID;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003074 File Offset: 0x00001274
		public IntermissionVote()
		{
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000307C File Offset: 0x0000127C
		protected override bool OnRead()
		{
			bool flag = true;
			this.ItemID = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.VoteCount = GameNetworkMessage.ReadIntFromPacket(new CompressionInfo.Integer(-1, 1, true), ref flag);
			return flag;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000030AE File Offset: 0x000012AE
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.ItemID);
			GameNetworkMessage.WriteIntToPacket(this.VoteCount, new CompressionInfo.Integer(-1, 1, true));
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000030CE File Offset: 0x000012CE
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000030D6 File Offset: 0x000012D6
		protected override string OnGetLogFormat()
		{
			return string.Format("Intermission vote casted for item with ID: {0} with count: {1}.", this.ItemID, this.VoteCount);
		}
	}
}
