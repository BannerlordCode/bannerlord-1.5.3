using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200008C RID: 140
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class EquipWeaponWithNewEntity : GameNetworkMessage
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x0000A087 File Offset: 0x00008287
		// (set) Token: 0x0600057A RID: 1402 RVA: 0x0000A08F File Offset: 0x0000828F
		public MissionWeapon Weapon { get; private set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x0000A098 File Offset: 0x00008298
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x0000A0A0 File Offset: 0x000082A0
		public EquipmentIndex SlotIndex { get; private set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x0000A0A9 File Offset: 0x000082A9
		// (set) Token: 0x0600057E RID: 1406 RVA: 0x0000A0B1 File Offset: 0x000082B1
		public int AgentIndex { get; private set; }

		// Token: 0x0600057F RID: 1407 RVA: 0x0000A0BA File Offset: 0x000082BA
		public EquipWeaponWithNewEntity(int agentIndex, EquipmentIndex slot, MissionWeapon weapon)
		{
			this.AgentIndex = agentIndex;
			this.SlotIndex = slot;
			this.Weapon = weapon;
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0000A0D7 File Offset: 0x000082D7
		public EquipWeaponWithNewEntity()
		{
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0000A0DF File Offset: 0x000082DF
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			ModuleNetworkData.WriteWeaponReferenceToPacket(this.Weapon);
			GameNetworkMessage.WriteIntToPacket((int)this.SlotIndex, CompressionMission.ItemSlotCompressionInfo);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0000A108 File Offset: 0x00008308
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.Weapon = ModuleNetworkData.ReadWeaponReferenceFromPacket(MBObjectManager.Instance, ref flag);
			this.SlotIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemSlotCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0000A149 File Offset: 0x00008349
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items | MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0000A154 File Offset: 0x00008354
		protected override string OnGetLogFormat()
		{
			if (this.AgentIndex < 0)
			{
				return "Not equipping weapon because there is no agent to equip it to,";
			}
			return string.Concat(new object[]
			{
				"Equip weapon with name: ",
				(!this.Weapon.IsEmpty) ? this.Weapon.Item.Name : TextObject.GetEmpty(),
				" from SlotIndex: ",
				this.SlotIndex,
				" on agent with agent-index: ",
				this.AgentIndex
			});
		}
	}
}
