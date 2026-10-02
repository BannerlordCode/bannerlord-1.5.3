using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200008F RID: 143
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class FlagDominationCapturePointMessage : GameNetworkMessage
	{
		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x0000A20C File Offset: 0x0000840C
		// (set) Token: 0x06000590 RID: 1424 RVA: 0x0000A214 File Offset: 0x00008414
		public int FlagIndex { get; private set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x0000A21D File Offset: 0x0000841D
		// (set) Token: 0x06000592 RID: 1426 RVA: 0x0000A225 File Offset: 0x00008425
		public int OwnerTeamIndex { get; private set; }

		// Token: 0x06000593 RID: 1427 RVA: 0x0000A22E File Offset: 0x0000842E
		public FlagDominationCapturePointMessage()
		{
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0000A236 File Offset: 0x00008436
		public FlagDominationCapturePointMessage(int flagIndex, int ownerTeamIndex)
		{
			this.FlagIndex = flagIndex;
			this.OwnerTeamIndex = ownerTeamIndex;
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0000A24C File Offset: 0x0000844C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.FlagIndex, CompressionMission.FlagCapturePointIndexCompressionInfo);
			GameNetworkMessage.WriteTeamIndexToPacket(this.OwnerTeamIndex);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0000A26C File Offset: 0x0000846C
		protected override bool OnRead()
		{
			bool flag = true;
			this.FlagIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FlagCapturePointIndexCompressionInfo, ref flag);
			this.OwnerTeamIndex = GameNetworkMessage.ReadTeamIndexFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0000A29B File Offset: 0x0000849B
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0000A2A3 File Offset: 0x000084A3
		protected override string OnGetLogFormat()
		{
			return "Flag owner changed.";
		}
	}
}
