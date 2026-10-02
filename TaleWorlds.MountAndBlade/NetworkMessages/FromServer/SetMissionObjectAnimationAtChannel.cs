using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000A8 RID: 168
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectAnimationAtChannel : GameNetworkMessage
	{
		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x0000BC11 File Offset: 0x00009E11
		// (set) Token: 0x060006B6 RID: 1718 RVA: 0x0000BC19 File Offset: 0x00009E19
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x0000BC22 File Offset: 0x00009E22
		// (set) Token: 0x060006B8 RID: 1720 RVA: 0x0000BC2A File Offset: 0x00009E2A
		public int ChannelNo { get; private set; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x0000BC33 File Offset: 0x00009E33
		// (set) Token: 0x060006BA RID: 1722 RVA: 0x0000BC3B File Offset: 0x00009E3B
		public int AnimationIndex { get; private set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x0000BC44 File Offset: 0x00009E44
		// (set) Token: 0x060006BC RID: 1724 RVA: 0x0000BC4C File Offset: 0x00009E4C
		public float AnimationSpeed { get; private set; }

		// Token: 0x060006BD RID: 1725 RVA: 0x0000BC55 File Offset: 0x00009E55
		public SetMissionObjectAnimationAtChannel(MissionObjectId missionObjectId, int channelNo, int animationIndex, float animationSpeed)
		{
			this.MissionObjectId = missionObjectId;
			this.ChannelNo = channelNo;
			this.AnimationIndex = animationIndex;
			this.AnimationSpeed = animationSpeed;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0000BC7A File Offset: 0x00009E7A
		public SetMissionObjectAnimationAtChannel()
		{
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0000BC84 File Offset: 0x00009E84
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.ChannelNo = (GameNetworkMessage.ReadBoolFromPacket(ref flag) ? 1 : 0);
			this.AnimationIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AnimationIndexCompressionInfo, ref flag);
			this.AnimationSpeed = (GameNetworkMessage.ReadBoolFromPacket(ref flag) ? GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.AnimationSpeedCompressionInfo, ref flag) : 1f);
			return flag;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0000BCEC File Offset: 0x00009EEC
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteBoolToPacket(this.ChannelNo == 1);
			GameNetworkMessage.WriteIntToPacket(this.AnimationIndex, CompressionBasic.AnimationIndexCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.AnimationSpeed != 1f);
			if (this.AnimationSpeed != 1f)
			{
				GameNetworkMessage.WriteFloatToPacket(this.AnimationSpeed, CompressionBasic.AnimationSpeedCompressionInfo);
			}
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0000BD54 File Offset: 0x00009F54
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0000BD5C File Offset: 0x00009F5C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set animation: ", this.AnimationIndex, " on channel: ", this.ChannelNo, " of MissionObject with ID: ", this.MissionObjectId });
		}
	}
}
