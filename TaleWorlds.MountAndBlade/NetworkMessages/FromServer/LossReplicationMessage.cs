using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000D8 RID: 216
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class LossReplicationMessage : GameNetworkMessage
	{
		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x0000EEFF File Offset: 0x0000D0FF
		// (set) Token: 0x060008DE RID: 2270 RVA: 0x0000EF07 File Offset: 0x0000D107
		internal int LossValue { get; private set; }

		// Token: 0x060008DF RID: 2271 RVA: 0x0000EF10 File Offset: 0x0000D110
		public LossReplicationMessage()
		{
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0000EF18 File Offset: 0x0000D118
		internal LossReplicationMessage(int lossValue)
		{
			this.LossValue = lossValue;
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0000EF28 File Offset: 0x0000D128
		protected override bool OnRead()
		{
			bool flag = true;
			this.LossValue = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.LossValueCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0000EF4A File Offset: 0x0000D14A
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.LossValue, CompressionBasic.LossValueCompressionInfo);
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0000EF5C File Offset: 0x0000D15C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionDetailed;
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0000EF64 File Offset: 0x0000D164
		protected override string OnGetLogFormat()
		{
			return "LossReplicationMessage";
		}
	}
}
