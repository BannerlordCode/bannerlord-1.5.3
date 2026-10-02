using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200000E RID: 14
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class RequestChangePreferredTroopType : GameNetworkMessage
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000054 RID: 84 RVA: 0x000028A5 File Offset: 0x00000AA5
		// (set) Token: 0x06000055 RID: 85 RVA: 0x000028AD File Offset: 0x00000AAD
		public TroopType TroopType { get; private set; }

		// Token: 0x06000056 RID: 86 RVA: 0x000028B6 File Offset: 0x00000AB6
		public RequestChangePreferredTroopType(TroopType troopType)
		{
			this.TroopType = troopType;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000028C5 File Offset: 0x00000AC5
		public RequestChangePreferredTroopType()
		{
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000028CD File Offset: 0x00000ACD
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.TroopType, CompressionBasic.TroopTypeCompressionInfo);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000028E0 File Offset: 0x00000AE0
		protected override bool OnRead()
		{
			bool flag = true;
			this.TroopType = (TroopType)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.TroopTypeCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002902 File Offset: 0x00000B02
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionDetailed;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000290A File Offset: 0x00000B0A
		protected override string OnGetLogFormat()
		{
			return "Peer requesting preferred troop type change to " + this.TroopType;
		}
	}
}
