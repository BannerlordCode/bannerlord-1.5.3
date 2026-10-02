using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000AA RID: 170
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectAnimationChannelSpeed : GameNetworkMessage
	{
		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x0000BEE6 File Offset: 0x0000A0E6
		// (set) Token: 0x060006D0 RID: 1744 RVA: 0x0000BEEE File Offset: 0x0000A0EE
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x0000BEF7 File Offset: 0x0000A0F7
		// (set) Token: 0x060006D2 RID: 1746 RVA: 0x0000BEFF File Offset: 0x0000A0FF
		public int ChannelNo { get; private set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0000BF08 File Offset: 0x0000A108
		// (set) Token: 0x060006D4 RID: 1748 RVA: 0x0000BF10 File Offset: 0x0000A110
		public float Speed { get; private set; }

		// Token: 0x060006D5 RID: 1749 RVA: 0x0000BF19 File Offset: 0x0000A119
		public SetMissionObjectAnimationChannelSpeed(MissionObjectId missionObjectId, int channelNo, float speed)
		{
			this.MissionObjectId = missionObjectId;
			this.ChannelNo = channelNo;
			this.Speed = speed;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0000BF36 File Offset: 0x0000A136
		public SetMissionObjectAnimationChannelSpeed()
		{
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0000BF40 File Offset: 0x0000A140
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			bool flag2 = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			if (flag)
			{
				this.ChannelNo = (flag2 ? 1 : 0);
			}
			this.Speed = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.AnimationSpeedCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0000BF87 File Offset: 0x0000A187
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteBoolToPacket(this.ChannelNo == 1);
			GameNetworkMessage.WriteFloatToPacket(this.Speed, CompressionBasic.AnimationSpeedCompressionInfo);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0000BFB2 File Offset: 0x0000A1B2
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0000BFBC File Offset: 0x0000A1BC
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set animation speed: ", this.Speed, " on channel: ", this.ChannelNo, " of MissionObject with ID: ", this.MissionObjectId });
		}
	}
}
