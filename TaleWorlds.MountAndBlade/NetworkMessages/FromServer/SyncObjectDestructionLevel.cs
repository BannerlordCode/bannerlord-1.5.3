using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000CF RID: 207
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SyncObjectDestructionLevel : GameNetworkMessage
	{
		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x0000E603 File Offset: 0x0000C803
		// (set) Token: 0x0600087A RID: 2170 RVA: 0x0000E60B File Offset: 0x0000C80B
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x0000E614 File Offset: 0x0000C814
		// (set) Token: 0x0600087C RID: 2172 RVA: 0x0000E61C File Offset: 0x0000C81C
		public int DestructionLevel { get; private set; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x0000E625 File Offset: 0x0000C825
		// (set) Token: 0x0600087E RID: 2174 RVA: 0x0000E62D File Offset: 0x0000C82D
		public int ForcedIndex { get; private set; }

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x0000E636 File Offset: 0x0000C836
		// (set) Token: 0x06000880 RID: 2176 RVA: 0x0000E63E File Offset: 0x0000C83E
		public float BlowMagnitude { get; private set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000881 RID: 2177 RVA: 0x0000E647 File Offset: 0x0000C847
		// (set) Token: 0x06000882 RID: 2178 RVA: 0x0000E64F File Offset: 0x0000C84F
		public Vec3 BlowPosition { get; private set; }

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000883 RID: 2179 RVA: 0x0000E658 File Offset: 0x0000C858
		// (set) Token: 0x06000884 RID: 2180 RVA: 0x0000E660 File Offset: 0x0000C860
		public Vec3 BlowDirection { get; private set; }

		// Token: 0x06000885 RID: 2181 RVA: 0x0000E669 File Offset: 0x0000C869
		public SyncObjectDestructionLevel(MissionObjectId missionObjectId, int destructionLevel, int forcedIndex, float blowMagnitude, Vec3 blowPosition, Vec3 blowDirection)
		{
			this.MissionObjectId = missionObjectId;
			this.DestructionLevel = destructionLevel;
			this.ForcedIndex = forcedIndex;
			this.BlowMagnitude = blowMagnitude;
			this.BlowPosition = blowPosition;
			this.BlowDirection = blowDirection;
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0000E69E File Offset: 0x0000C89E
		public SyncObjectDestructionLevel()
		{
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0000E6A8 File Offset: 0x0000C8A8
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.DestructionLevel = GameNetworkMessage.ReadIntFromPacket(CompressionMission.UsableGameObjectDestructionStateCompressionInfo, ref flag);
			this.ForcedIndex = (GameNetworkMessage.ReadBoolFromPacket(ref flag) ? GameNetworkMessage.ReadIntFromPacket(CompressionBasic.MissionObjectIDCompressionInfo, ref flag) : (-1));
			this.BlowMagnitude = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.UsableGameObjectBlowMagnitude, ref flag);
			this.BlowPosition = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.PositionCompressionInfo, ref flag);
			this.BlowDirection = GameNetworkMessage.ReadVec3FromPacket(CompressionMission.UsableGameObjectBlowDirection, ref flag);
			return flag;
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0000E72C File Offset: 0x0000C92C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteIntToPacket(this.DestructionLevel, CompressionMission.UsableGameObjectDestructionStateCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.ForcedIndex != -1);
			if (this.ForcedIndex != -1)
			{
				GameNetworkMessage.WriteIntToPacket(this.ForcedIndex, CompressionBasic.MissionObjectIDCompressionInfo);
			}
			GameNetworkMessage.WriteFloatToPacket(this.BlowMagnitude, CompressionMission.UsableGameObjectBlowMagnitude);
			GameNetworkMessage.WriteVec3ToPacket(this.BlowPosition, CompressionBasic.PositionCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(this.BlowDirection, CompressionMission.UsableGameObjectBlowDirection);
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0000E7AE File Offset: 0x0000C9AE
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed | MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0000E7B8 File Offset: 0x0000C9B8
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Synchronize DestructionLevel: ",
				this.DestructionLevel,
				" of MissionObject with Id: ",
				this.MissionObjectId,
				(this.ForcedIndex != -1) ? (" (New object will have ID: " + this.ForcedIndex + ")") : ""
			});
		}
	}
}
