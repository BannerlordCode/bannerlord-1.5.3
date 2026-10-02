using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200000B RID: 11
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class DuelRequest : GameNetworkMessage
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002711 File Offset: 0x00000911
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002719 File Offset: 0x00000919
		public int RequestedAgentIndex { get; private set; }

		// Token: 0x0600003C RID: 60 RVA: 0x00002722 File Offset: 0x00000922
		public DuelRequest(int requestedAgentIndex)
		{
			this.RequestedAgentIndex = requestedAgentIndex;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002731 File Offset: 0x00000931
		public DuelRequest()
		{
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000273C File Offset: 0x0000093C
		protected override bool OnRead()
		{
			bool flag = true;
			this.RequestedAgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002759 File Offset: 0x00000959
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.RequestedAgentIndex);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002766 File Offset: 0x00000966
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000276E File Offset: 0x0000096E
		protected override string OnGetLogFormat()
		{
			return "Duel requested from agent with index: " + this.RequestedAgentIndex;
		}
	}
}
