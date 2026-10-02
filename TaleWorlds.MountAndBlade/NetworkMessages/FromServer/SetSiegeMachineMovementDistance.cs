using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000BB RID: 187
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetSiegeMachineMovementDistance : GameNetworkMessage
	{
		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x0000CFA5 File Offset: 0x0000B1A5
		// (set) Token: 0x06000784 RID: 1924 RVA: 0x0000CFAD File Offset: 0x0000B1AD
		public MissionObjectId UsableMachineId { get; private set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x0000CFB6 File Offset: 0x0000B1B6
		// (set) Token: 0x06000786 RID: 1926 RVA: 0x0000CFBE File Offset: 0x0000B1BE
		public float Distance { get; private set; }

		// Token: 0x06000787 RID: 1927 RVA: 0x0000CFC7 File Offset: 0x0000B1C7
		public SetSiegeMachineMovementDistance(MissionObjectId usableMachineId, float distance)
		{
			this.UsableMachineId = usableMachineId;
			this.Distance = distance;
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0000CFDD File Offset: 0x0000B1DD
		public SetSiegeMachineMovementDistance()
		{
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0000CFE8 File Offset: 0x0000B1E8
		protected override bool OnRead()
		{
			bool flag = true;
			this.UsableMachineId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Distance = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.PositionCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0000D017 File Offset: 0x0000B217
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.UsableMachineId);
			GameNetworkMessage.WriteFloatToPacket(this.Distance, CompressionBasic.PositionCompressionInfo);
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0000D034 File Offset: 0x0000B234
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0000D03C File Offset: 0x0000B23C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set Movement Distance: ", this.Distance, " of SiegeMachine with ID: ", this.UsableMachineId });
		}
	}
}
