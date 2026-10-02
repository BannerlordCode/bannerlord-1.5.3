using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000A9 RID: 169
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectAnimationChannelParameter : GameNetworkMessage
	{
		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x0000BDB6 File Offset: 0x00009FB6
		// (set) Token: 0x060006C4 RID: 1732 RVA: 0x0000BDBE File Offset: 0x00009FBE
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x0000BDC7 File Offset: 0x00009FC7
		// (set) Token: 0x060006C6 RID: 1734 RVA: 0x0000BDCF File Offset: 0x00009FCF
		public int ChannelNo { get; private set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x0000BDD8 File Offset: 0x00009FD8
		// (set) Token: 0x060006C8 RID: 1736 RVA: 0x0000BDE0 File Offset: 0x00009FE0
		public float Parameter { get; private set; }

		// Token: 0x060006C9 RID: 1737 RVA: 0x0000BDE9 File Offset: 0x00009FE9
		public SetMissionObjectAnimationChannelParameter(MissionObjectId missionObjectId, int channelNo, float parameter)
		{
			this.MissionObjectId = missionObjectId;
			this.ChannelNo = channelNo;
			this.Parameter = parameter;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0000BE06 File Offset: 0x0000A006
		public SetMissionObjectAnimationChannelParameter()
		{
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0000BE10 File Offset: 0x0000A010
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			bool flag2 = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			if (flag)
			{
				this.ChannelNo = (flag2 ? 1 : 0);
			}
			this.Parameter = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.AnimationProgressCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0000BE57 File Offset: 0x0000A057
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteBoolToPacket(this.ChannelNo == 1);
			GameNetworkMessage.WriteFloatToPacket(this.Parameter, CompressionBasic.AnimationProgressCompressionInfo);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0000BE82 File Offset: 0x0000A082
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0000BE8C File Offset: 0x0000A08C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set animation parameter: ", this.Parameter, " on channel: ", this.ChannelNo, " of MissionObject with ID: ", this.MissionObjectId });
		}
	}
}
