using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200002A RID: 42
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class BarkSelected : GameNetworkMessage
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00003C63 File Offset: 0x00001E63
		// (set) Token: 0x06000154 RID: 340 RVA: 0x00003C6B File Offset: 0x00001E6B
		public int IndexOfBark { get; private set; }

		// Token: 0x06000155 RID: 341 RVA: 0x00003C74 File Offset: 0x00001E74
		public BarkSelected(int indexOfBark)
		{
			this.IndexOfBark = indexOfBark;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00003C83 File Offset: 0x00001E83
		public BarkSelected()
		{
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00003C8C File Offset: 0x00001E8C
		protected override bool OnRead()
		{
			bool flag = true;
			this.IndexOfBark = GameNetworkMessage.ReadIntFromPacket(CompressionMission.BarkIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00003CAE File Offset: 0x00001EAE
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.IndexOfBark, CompressionMission.BarkIndexCompressionInfo);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00003CC0 File Offset: 0x00001EC0
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.None;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00003CC4 File Offset: 0x00001EC4
		protected override string OnGetLogFormat()
		{
			return "FromClient.BarkSelected: " + this.IndexOfBark;
		}
	}
}
