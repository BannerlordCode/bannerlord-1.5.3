using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000069 RID: 105
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SyncGoldsForSkirmish : GameNetworkMessage
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00006FDC File Offset: 0x000051DC
		// (set) Token: 0x060003A4 RID: 932 RVA: 0x00006FE4 File Offset: 0x000051E4
		public VirtualPlayer VirtualPlayer { get; private set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x00006FED File Offset: 0x000051ED
		// (set) Token: 0x060003A6 RID: 934 RVA: 0x00006FF5 File Offset: 0x000051F5
		public int GoldAmount { get; private set; }

		// Token: 0x060003A7 RID: 935 RVA: 0x00006FFE File Offset: 0x000051FE
		public SyncGoldsForSkirmish()
		{
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00007006 File Offset: 0x00005206
		public SyncGoldsForSkirmish(VirtualPlayer peer, int goldAmount)
		{
			this.VirtualPlayer = peer;
			this.GoldAmount = goldAmount;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000701C File Offset: 0x0000521C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteVirtualPlayerReferenceToPacket(this.VirtualPlayer);
			GameNetworkMessage.WriteIntToPacket(this.GoldAmount, CompressionBasic.RoundGoldAmountCompressionInfo);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000703C File Offset: 0x0000523C
		protected override bool OnRead()
		{
			bool flag = true;
			this.VirtualPlayer = GameNetworkMessage.ReadVirtualPlayerReferenceToPacket(ref flag, false);
			this.GoldAmount = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.RoundGoldAmountCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000706C File Offset: 0x0000526C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00007074 File Offset: 0x00005274
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Gold amount set to ",
				this.GoldAmount,
				" for ",
				this.VirtualPlayer.UserName,
				"."
			});
		}
	}
}
