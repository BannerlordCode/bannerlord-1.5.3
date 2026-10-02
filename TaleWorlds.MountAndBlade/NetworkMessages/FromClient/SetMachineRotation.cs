using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000038 RID: 56
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class SetMachineRotation : GameNetworkMessage
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00004125 File Offset: 0x00002325
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x0000412D File Offset: 0x0000232D
		public MissionObjectId UsableMachineId { get; private set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00004136 File Offset: 0x00002336
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x0000413E File Offset: 0x0000233E
		public float HorizontalRotation { get; private set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00004147 File Offset: 0x00002347
		// (set) Token: 0x060001BA RID: 442 RVA: 0x0000414F File Offset: 0x0000234F
		public float VerticalRotation { get; private set; }

		// Token: 0x060001BB RID: 443 RVA: 0x00004158 File Offset: 0x00002358
		public SetMachineRotation(MissionObjectId missionObjectId, float horizontalRotation, float verticalRotation)
		{
			this.UsableMachineId = missionObjectId;
			this.HorizontalRotation = horizontalRotation;
			this.VerticalRotation = verticalRotation;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00004175 File Offset: 0x00002375
		public SetMachineRotation()
		{
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00004180 File Offset: 0x00002380
		protected override bool OnRead()
		{
			bool flag = true;
			this.UsableMachineId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.HorizontalRotation = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.HighResRadianCompressionInfo, ref flag);
			this.VerticalRotation = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.HighResRadianCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x000041C1 File Offset: 0x000023C1
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.UsableMachineId);
			GameNetworkMessage.WriteFloatToPacket(this.HorizontalRotation, CompressionBasic.HighResRadianCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.VerticalRotation, CompressionBasic.HighResRadianCompressionInfo);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000041EE File Offset: 0x000023EE
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000041F6 File Offset: 0x000023F6
		protected override string OnGetLogFormat()
		{
			return "Set rotation of UsableMachine with ID: " + this.UsableMachineId;
		}
	}
}
