using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200007C RID: 124
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AttachWeaponToSpawnedWeapon : GameNetworkMessage
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000474 RID: 1140 RVA: 0x00008411 File Offset: 0x00006611
		// (set) Token: 0x06000475 RID: 1141 RVA: 0x00008419 File Offset: 0x00006619
		public MissionWeapon Weapon { get; private set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x00008422 File Offset: 0x00006622
		// (set) Token: 0x06000477 RID: 1143 RVA: 0x0000842A File Offset: 0x0000662A
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x00008433 File Offset: 0x00006633
		// (set) Token: 0x06000479 RID: 1145 RVA: 0x0000843B File Offset: 0x0000663B
		public MatrixFrame AttachLocalFrame { get; private set; }

		// Token: 0x0600047A RID: 1146 RVA: 0x00008444 File Offset: 0x00006644
		public AttachWeaponToSpawnedWeapon(MissionWeapon weapon, MissionObjectId missionObjectId, MatrixFrame attachLocalFrame)
		{
			this.Weapon = weapon;
			this.MissionObjectId = missionObjectId;
			this.AttachLocalFrame = attachLocalFrame;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00008461 File Offset: 0x00006661
		public AttachWeaponToSpawnedWeapon()
		{
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00008469 File Offset: 0x00006669
		protected override void OnWrite()
		{
			ModuleNetworkData.WriteWeaponReferenceToPacket(this.Weapon);
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteVec3ToPacket(this.AttachLocalFrame.origin, CompressionBasic.LocalPositionCompressionInfo);
			GameNetworkMessage.WriteRotationMatrixToPacket(this.AttachLocalFrame.rotation);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x000084A8 File Offset: 0x000066A8
		protected override bool OnRead()
		{
			bool flag = true;
			this.Weapon = ModuleNetworkData.ReadWeaponReferenceFromPacket(MBObjectManager.Instance, ref flag);
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			Vec3 vec = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.LocalPositionCompressionInfo, ref flag);
			Mat3 mat = GameNetworkMessage.ReadRotationMatrixFromPacket(ref flag);
			if (flag)
			{
				this.AttachLocalFrame = new MatrixFrame(in mat, in vec);
			}
			return flag;
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x000084FE File Offset: 0x000066FE
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items | MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00008508 File Offset: 0x00006708
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"AttachWeaponToSpawnedWeapon with name: ",
				(!this.Weapon.IsEmpty) ? this.Weapon.Item.Name : TextObject.GetEmpty(),
				" to MissionObject: ",
				this.MissionObjectId
			});
		}
	}
}
