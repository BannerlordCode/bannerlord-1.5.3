using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200002F RID: 47
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class FinishedLoading : GameNetworkMessage
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00003E57 File Offset: 0x00002057
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00003E5F File Offset: 0x0000205F
		public int BattleIndex { get; private set; }

		// Token: 0x06000179 RID: 377 RVA: 0x00003E68 File Offset: 0x00002068
		public FinishedLoading()
		{
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00003E70 File Offset: 0x00002070
		public FinishedLoading(int battleIndex)
		{
			this.BattleIndex = battleIndex;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00003E80 File Offset: 0x00002080
		protected override bool OnRead()
		{
			bool flag = true;
			this.BattleIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AutomatedBattleIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00003EA2 File Offset: 0x000020A2
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.BattleIndex, CompressionMission.AutomatedBattleIndexCompressionInfo);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00003EB4 File Offset: 0x000020B4
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.General;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00003EB8 File Offset: 0x000020B8
		protected override string OnGetLogFormat()
		{
			return "Finished Loading";
		}
	}
}
