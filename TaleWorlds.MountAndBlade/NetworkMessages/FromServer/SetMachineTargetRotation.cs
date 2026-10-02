using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000A7 RID: 167
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMachineTargetRotation : GameNetworkMessage
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x0000BB2C File Offset: 0x00009D2C
		// (set) Token: 0x060006AA RID: 1706 RVA: 0x0000BB34 File Offset: 0x00009D34
		public MissionObjectId UsableMachineId { get; private set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x0000BB3D File Offset: 0x00009D3D
		// (set) Token: 0x060006AC RID: 1708 RVA: 0x0000BB45 File Offset: 0x00009D45
		public float HorizontalRotation { get; private set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x0000BB4E File Offset: 0x00009D4E
		// (set) Token: 0x060006AE RID: 1710 RVA: 0x0000BB56 File Offset: 0x00009D56
		public float VerticalRotation { get; private set; }

		// Token: 0x060006AF RID: 1711 RVA: 0x0000BB5F File Offset: 0x00009D5F
		public SetMachineTargetRotation(MissionObjectId usableMachineId, float horizontalRotaiton, float verticalRotation)
		{
			this.UsableMachineId = usableMachineId;
			this.HorizontalRotation = horizontalRotaiton;
			this.VerticalRotation = verticalRotation;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0000BB7C File Offset: 0x00009D7C
		public SetMachineTargetRotation()
		{
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0000BB84 File Offset: 0x00009D84
		protected override bool OnRead()
		{
			bool flag = true;
			this.UsableMachineId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.HorizontalRotation = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.HighResRadianCompressionInfo, ref flag);
			this.VerticalRotation = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.HighResRadianCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0000BBC5 File Offset: 0x00009DC5
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.UsableMachineId);
			GameNetworkMessage.WriteFloatToPacket(this.HorizontalRotation, CompressionBasic.HighResRadianCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.VerticalRotation, CompressionBasic.HighResRadianCompressionInfo);
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0000BBF2 File Offset: 0x00009DF2
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0000BBFA File Offset: 0x00009DFA
		protected override string OnGetLogFormat()
		{
			return "Set target rotation of UsableMachine with ID: " + this.UsableMachineId;
		}
	}
}
