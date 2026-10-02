using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200008B RID: 139
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class EquipWeaponFromSpawnedItemEntity : GameNetworkMessage
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x00009EF9 File Offset: 0x000080F9
		// (set) Token: 0x0600056C RID: 1388 RVA: 0x00009F01 File Offset: 0x00008101
		public MissionObjectId SpawnedItemEntityId { get; private set; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x00009F0A File Offset: 0x0000810A
		// (set) Token: 0x0600056E RID: 1390 RVA: 0x00009F12 File Offset: 0x00008112
		public EquipmentIndex SlotIndex { get; private set; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x00009F1B File Offset: 0x0000811B
		// (set) Token: 0x06000570 RID: 1392 RVA: 0x00009F23 File Offset: 0x00008123
		public int AgentIndex { get; private set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x00009F2C File Offset: 0x0000812C
		// (set) Token: 0x06000572 RID: 1394 RVA: 0x00009F34 File Offset: 0x00008134
		public bool RemoveWeapon { get; private set; }

		// Token: 0x06000573 RID: 1395 RVA: 0x00009F3D File Offset: 0x0000813D
		public EquipWeaponFromSpawnedItemEntity(int agentIndex, EquipmentIndex slot, MissionObjectId spawnedItemEntityId, bool removeWeapon)
		{
			this.AgentIndex = agentIndex;
			this.SlotIndex = slot;
			this.SpawnedItemEntityId = spawnedItemEntityId;
			this.RemoveWeapon = removeWeapon;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00009F62 File Offset: 0x00008162
		public EquipWeaponFromSpawnedItemEntity()
		{
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00009F6C File Offset: 0x0000816C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteMissionObjectIdToPacket((this.SpawnedItemEntityId.Id >= 0) ? this.SpawnedItemEntityId : MissionObjectId.Invalid);
			GameNetworkMessage.WriteIntToPacket((int)this.SlotIndex, CompressionMission.ItemSlotCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.RemoveWeapon);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00009FC0 File Offset: 0x000081C0
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.SpawnedItemEntityId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.SlotIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemSlotCompressionInfo, ref flag);
			this.RemoveWeapon = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0000A009 File Offset: 0x00008209
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items | MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0000A014 File Offset: 0x00008214
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"EquipWeaponFromSpawnedItemEntity with missionObjectId: ",
				this.SpawnedItemEntityId,
				" to SlotIndex: ",
				this.SlotIndex,
				" on agent-index: ",
				this.AgentIndex,
				" RemoveWeapon: ",
				this.RemoveWeapon.ToString()
			});
		}
	}
}
