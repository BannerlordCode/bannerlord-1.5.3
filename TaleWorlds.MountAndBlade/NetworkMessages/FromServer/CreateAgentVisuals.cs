using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000086 RID: 134
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class CreateAgentVisuals : GameNetworkMessage
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x0000949C File Offset: 0x0000769C
		// (set) Token: 0x06000512 RID: 1298 RVA: 0x000094A4 File Offset: 0x000076A4
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x000094AD File Offset: 0x000076AD
		// (set) Token: 0x06000514 RID: 1300 RVA: 0x000094B5 File Offset: 0x000076B5
		public int VisualsIndex { get; private set; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x000094BE File Offset: 0x000076BE
		// (set) Token: 0x06000516 RID: 1302 RVA: 0x000094C6 File Offset: 0x000076C6
		public BasicCharacterObject Character { get; private set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x000094CF File Offset: 0x000076CF
		// (set) Token: 0x06000518 RID: 1304 RVA: 0x000094D7 File Offset: 0x000076D7
		public Equipment Equipment { get; private set; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x000094E0 File Offset: 0x000076E0
		// (set) Token: 0x0600051A RID: 1306 RVA: 0x000094E8 File Offset: 0x000076E8
		public int BodyPropertiesSeed { get; private set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x000094F1 File Offset: 0x000076F1
		// (set) Token: 0x0600051C RID: 1308 RVA: 0x000094F9 File Offset: 0x000076F9
		public bool IsFemale { get; private set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x00009502 File Offset: 0x00007702
		// (set) Token: 0x0600051E RID: 1310 RVA: 0x0000950A File Offset: 0x0000770A
		public int SelectedEquipmentSetIndex { get; private set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x00009513 File Offset: 0x00007713
		// (set) Token: 0x06000520 RID: 1312 RVA: 0x0000951B File Offset: 0x0000771B
		public int TroopCountInFormation { get; private set; }

		// Token: 0x06000521 RID: 1313 RVA: 0x00009524 File Offset: 0x00007724
		public CreateAgentVisuals(NetworkCommunicator peer, AgentBuildData agentBuildData, int selectedEquipmentSetIndex, int troopCountInFormation = 0)
		{
			this.Peer = peer;
			this.VisualsIndex = agentBuildData.AgentVisualsIndex;
			this.Character = agentBuildData.AgentCharacter;
			this.BodyPropertiesSeed = agentBuildData.AgentEquipmentSeed;
			this.IsFemale = agentBuildData.AgentIsFemale;
			this.Equipment = new Equipment();
			this.Equipment.FillFrom(agentBuildData.AgentOverridenSpawnEquipment, true);
			this.SelectedEquipmentSetIndex = selectedEquipmentSetIndex;
			this.TroopCountInFormation = troopCountInFormation;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0000959A File Offset: 0x0000779A
		public CreateAgentVisuals()
		{
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x000095A4 File Offset: 0x000077A4
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.VisualsIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AgentOffsetCompressionInfo, ref flag);
			this.Character = (BasicCharacterObject)GameNetworkMessage.ReadObjectReferenceFromPacket(MBObjectManager.Instance, CompressionBasic.GUIDCompressionInfo, ref flag);
			this.Equipment = new Equipment();
			bool flag2 = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < (flag2 ? EquipmentIndex.NumEquipmentSetSlots : EquipmentIndex.ArmorItemEndSlot); equipmentIndex++)
			{
				EquipmentElement equipmentElement = ModuleNetworkData.ReadItemReferenceFromPacket(MBObjectManager.Instance, ref flag);
				if (!flag)
				{
					break;
				}
				this.Equipment.AddEquipmentToSlotWithoutAgent(equipmentIndex, equipmentElement);
			}
			this.BodyPropertiesSeed = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.RandomSeedCompressionInfo, ref flag);
			this.IsFemale = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.SelectedEquipmentSetIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.MissionObjectIDCompressionInfo, ref flag);
			this.TroopCountInFormation = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PlayerCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00009678 File Offset: 0x00007878
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteIntToPacket(this.VisualsIndex, CompressionMission.AgentOffsetCompressionInfo);
			GameNetworkMessage.WriteObjectReferenceToPacket(this.Character, CompressionBasic.GUIDCompressionInfo);
			bool flag = this.Equipment[EquipmentIndex.ArmorItemEndSlot].Item != null;
			GameNetworkMessage.WriteBoolToPacket(flag);
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < (flag ? EquipmentIndex.NumEquipmentSetSlots : EquipmentIndex.ArmorItemEndSlot); equipmentIndex++)
			{
				ModuleNetworkData.WriteItemReferenceToPacket(this.Equipment.GetEquipmentFromSlot(equipmentIndex));
			}
			GameNetworkMessage.WriteIntToPacket(this.BodyPropertiesSeed, CompressionBasic.RandomSeedCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.IsFemale);
			GameNetworkMessage.WriteIntToPacket(this.SelectedEquipmentSetIndex, CompressionBasic.MissionObjectIDCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.TroopCountInFormation, CompressionBasic.PlayerCompressionInfo);
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0000972F File Offset: 0x0000792F
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Agents;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00009737 File Offset: 0x00007937
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Create AgentVisuals for peer: ",
				this.Peer.UserName,
				", and with Index: ",
				this.VisualsIndex
			});
		}
	}
}
