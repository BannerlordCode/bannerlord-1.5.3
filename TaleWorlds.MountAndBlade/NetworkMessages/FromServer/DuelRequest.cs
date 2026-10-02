using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200004A RID: 74
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class DuelRequest : GameNetworkMessage
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600026D RID: 621 RVA: 0x00004FDE File Offset: 0x000031DE
		// (set) Token: 0x0600026E RID: 622 RVA: 0x00004FE6 File Offset: 0x000031E6
		public int RequesterAgentIndex { get; private set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600026F RID: 623 RVA: 0x00004FEF File Offset: 0x000031EF
		// (set) Token: 0x06000270 RID: 624 RVA: 0x00004FF7 File Offset: 0x000031F7
		public int RequestedAgentIndex { get; private set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000271 RID: 625 RVA: 0x00005000 File Offset: 0x00003200
		// (set) Token: 0x06000272 RID: 626 RVA: 0x00005008 File Offset: 0x00003208
		public TroopType SelectedAreaTroopType { get; private set; }

		// Token: 0x06000273 RID: 627 RVA: 0x00005011 File Offset: 0x00003211
		public DuelRequest(int requesterAgentIndex, int requestedAgentIndex, TroopType selectedAreaTroopType)
		{
			this.RequesterAgentIndex = requesterAgentIndex;
			this.RequestedAgentIndex = requestedAgentIndex;
			this.SelectedAreaTroopType = selectedAreaTroopType;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000502E File Offset: 0x0000322E
		public DuelRequest()
		{
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00005038 File Offset: 0x00003238
		protected override bool OnRead()
		{
			bool flag = true;
			this.RequesterAgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.RequestedAgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.SelectedAreaTroopType = (TroopType)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.TroopTypeCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00005074 File Offset: 0x00003274
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.RequesterAgentIndex);
			GameNetworkMessage.WriteAgentIndexToPacket(this.RequestedAgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.SelectedAreaTroopType, CompressionBasic.TroopTypeCompressionInfo);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000509C File Offset: 0x0000329C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x000050A4 File Offset: 0x000032A4
		protected override string OnGetLogFormat()
		{
			return "Request duel from agent with index: " + this.RequestedAgentIndex;
		}
	}
}
