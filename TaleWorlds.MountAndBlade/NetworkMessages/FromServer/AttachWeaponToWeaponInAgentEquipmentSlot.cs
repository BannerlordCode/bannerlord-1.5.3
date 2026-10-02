using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200007D RID: 125
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AttachWeaponToWeaponInAgentEquipmentSlot : GameNetworkMessage
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x0000856B File Offset: 0x0000676B
		// (set) Token: 0x06000481 RID: 1153 RVA: 0x00008573 File Offset: 0x00006773
		public MissionWeapon Weapon { get; private set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x0000857C File Offset: 0x0000677C
		// (set) Token: 0x06000483 RID: 1155 RVA: 0x00008584 File Offset: 0x00006784
		public EquipmentIndex SlotIndex { get; private set; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x0000858D File Offset: 0x0000678D
		// (set) Token: 0x06000485 RID: 1157 RVA: 0x00008595 File Offset: 0x00006795
		public int AgentIndex { get; private set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000486 RID: 1158 RVA: 0x0000859E File Offset: 0x0000679E
		// (set) Token: 0x06000487 RID: 1159 RVA: 0x000085A6 File Offset: 0x000067A6
		public MatrixFrame AttachLocalFrame { get; private set; }

		// Token: 0x06000488 RID: 1160 RVA: 0x000085AF File Offset: 0x000067AF
		public AttachWeaponToWeaponInAgentEquipmentSlot(MissionWeapon weapon, int agentIndex, EquipmentIndex slot, MatrixFrame attachLocalFrame)
		{
			this.Weapon = weapon;
			this.AgentIndex = agentIndex;
			this.SlotIndex = slot;
			this.AttachLocalFrame = attachLocalFrame;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x000085D4 File Offset: 0x000067D4
		public AttachWeaponToWeaponInAgentEquipmentSlot()
		{
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x000085DC File Offset: 0x000067DC
		protected override void OnWrite()
		{
			ModuleNetworkData.WriteWeaponReferenceToPacket(this.Weapon);
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.SlotIndex, CompressionMission.ItemSlotCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(this.AttachLocalFrame.origin, CompressionBasic.LocalPositionCompressionInfo);
			GameNetworkMessage.WriteRotationMatrixToPacket(this.AttachLocalFrame.rotation);
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00008634 File Offset: 0x00006834
		protected override bool OnRead()
		{
			bool flag = true;
			this.Weapon = ModuleNetworkData.ReadWeaponReferenceFromPacket(MBObjectManager.Instance, ref flag);
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.SlotIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemSlotCompressionInfo, ref flag);
			Vec3 vec = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.LocalPositionCompressionInfo, ref flag);
			Mat3 mat = GameNetworkMessage.ReadRotationMatrixFromPacket(ref flag);
			if (flag)
			{
				this.AttachLocalFrame = new MatrixFrame(in mat, in vec);
			}
			return flag;
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0000869C File Offset: 0x0000689C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items | MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x000086A4 File Offset: 0x000068A4
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"AttachWeaponToWeaponInAgentEquipmentSlot with name: ",
				(!this.Weapon.IsEmpty) ? this.Weapon.Item.Name : TextObject.GetEmpty(),
				" to SlotIndex: ",
				this.SlotIndex,
				" on agent-index: ",
				this.AgentIndex
			});
		}
	}
}
