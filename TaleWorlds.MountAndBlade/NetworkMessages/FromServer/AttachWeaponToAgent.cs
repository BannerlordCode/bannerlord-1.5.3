using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200007B RID: 123
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AttachWeaponToAgent : GameNetworkMessage
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x0000825B File Offset: 0x0000645B
		// (set) Token: 0x06000467 RID: 1127 RVA: 0x00008263 File Offset: 0x00006463
		public MissionWeapon Weapon { get; private set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x0000826C File Offset: 0x0000646C
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x00008274 File Offset: 0x00006474
		public int AgentIndex { get; private set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x0000827D File Offset: 0x0000647D
		// (set) Token: 0x0600046B RID: 1131 RVA: 0x00008285 File Offset: 0x00006485
		public sbyte BoneIndex { get; private set; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x0000828E File Offset: 0x0000648E
		// (set) Token: 0x0600046D RID: 1133 RVA: 0x00008296 File Offset: 0x00006496
		public MatrixFrame AttachLocalFrame { get; private set; }

		// Token: 0x0600046E RID: 1134 RVA: 0x0000829F File Offset: 0x0000649F
		public AttachWeaponToAgent(MissionWeapon weapon, int agentIndex, sbyte boneIndex, MatrixFrame attachLocalFrame)
		{
			this.Weapon = weapon;
			this.AgentIndex = agentIndex;
			this.BoneIndex = boneIndex;
			this.AttachLocalFrame = attachLocalFrame;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x000082C4 File Offset: 0x000064C4
		public AttachWeaponToAgent()
		{
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x000082CC File Offset: 0x000064CC
		protected override void OnWrite()
		{
			ModuleNetworkData.WriteWeaponReferenceToPacket(this.Weapon);
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.BoneIndex, CompressionMission.BoneIndexCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(this.AttachLocalFrame.origin, CompressionBasic.LocalPositionCompressionInfo);
			GameNetworkMessage.WriteRotationMatrixToPacket(this.AttachLocalFrame.rotation);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00008324 File Offset: 0x00006524
		protected override bool OnRead()
		{
			bool flag = true;
			this.Weapon = ModuleNetworkData.ReadWeaponReferenceFromPacket(MBObjectManager.Instance, ref flag);
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.BoneIndex = (sbyte)GameNetworkMessage.ReadIntFromPacket(CompressionMission.BoneIndexCompressionInfo, ref flag);
			Vec3 vec = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.LocalPositionCompressionInfo, ref flag);
			Mat3 mat = GameNetworkMessage.ReadRotationMatrixFromPacket(ref flag);
			if (flag)
			{
				this.AttachLocalFrame = new MatrixFrame(in mat, in vec);
			}
			return flag;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0000838D File Offset: 0x0000658D
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items | MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00008398 File Offset: 0x00006598
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"AttachWeaponToAgent with name: ",
				(!this.Weapon.IsEmpty) ? this.Weapon.Item.Name : TextObject.GetEmpty(),
				" to bone index: ",
				this.BoneIndex,
				" on agent agent-index: ",
				this.AgentIndex
			});
		}
	}
}
