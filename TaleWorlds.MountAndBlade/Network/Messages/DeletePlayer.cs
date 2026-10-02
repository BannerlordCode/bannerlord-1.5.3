using System;

namespace TaleWorlds.MountAndBlade.Network.Messages
{
	// Token: 0x020003C8 RID: 968
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class DeletePlayer : GameNetworkMessage
	{
		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x0600367C RID: 13948 RVA: 0x000E17A6 File Offset: 0x000DF9A6
		// (set) Token: 0x0600367D RID: 13949 RVA: 0x000E17AE File Offset: 0x000DF9AE
		public int PlayerIndex { get; private set; }

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x0600367E RID: 13950 RVA: 0x000E17B7 File Offset: 0x000DF9B7
		// (set) Token: 0x0600367F RID: 13951 RVA: 0x000E17BF File Offset: 0x000DF9BF
		public bool AddToDisconnectList { get; private set; }

		// Token: 0x06003680 RID: 13952 RVA: 0x000E17C8 File Offset: 0x000DF9C8
		public DeletePlayer(int playerIndex, bool addToDisconnectList)
		{
			this.PlayerIndex = playerIndex;
			this.AddToDisconnectList = addToDisconnectList;
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x000E17DE File Offset: 0x000DF9DE
		public DeletePlayer()
		{
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x000E17E6 File Offset: 0x000DF9E6
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.PlayerIndex, CompressionBasic.PlayerCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.AddToDisconnectList);
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x000E1804 File Offset: 0x000DFA04
		protected override bool OnRead()
		{
			bool flag = true;
			this.PlayerIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PlayerCompressionInfo, ref flag);
			this.AddToDisconnectList = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06003684 RID: 13956 RVA: 0x000E1833 File Offset: 0x000DFA33
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers;
		}

		// Token: 0x06003685 RID: 13957 RVA: 0x000E1837 File Offset: 0x000DFA37
		protected override string OnGetLogFormat()
		{
			return "Delete player with index" + this.PlayerIndex;
		}
	}
}
