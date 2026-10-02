using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000044 RID: 68
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class FlagDominationMoraleChangeMessage : GameNetworkMessage
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000499D File Offset: 0x00002B9D
		// (set) Token: 0x0600022C RID: 556 RVA: 0x000049A5 File Offset: 0x00002BA5
		public float Morale { get; private set; }

		// Token: 0x0600022D RID: 557 RVA: 0x000049AE File Offset: 0x00002BAE
		public FlagDominationMoraleChangeMessage()
		{
		}

		// Token: 0x0600022E RID: 558 RVA: 0x000049B6 File Offset: 0x00002BB6
		public FlagDominationMoraleChangeMessage(float morale)
		{
			this.Morale = morale;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x000049C5 File Offset: 0x00002BC5
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteFloatToPacket(this.Morale, CompressionMission.FlagDominationMoraleCompressionInfo);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x000049D8 File Offset: 0x00002BD8
		protected override bool OnRead()
		{
			bool flag = true;
			this.Morale = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.FlagDominationMoraleCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x000049FA File Offset: 0x00002BFA
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00004A02 File Offset: 0x00002C02
		protected override string OnGetLogFormat()
		{
			return "Morale synched: " + this.Morale;
		}
	}
}
