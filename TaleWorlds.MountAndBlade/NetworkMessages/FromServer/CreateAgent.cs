using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000085 RID: 133
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class CreateAgent : GameNetworkMessage
	{
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x00008EDE File Offset: 0x000070DE
		// (set) Token: 0x060004EA RID: 1258 RVA: 0x00008EE6 File Offset: 0x000070E6
		public int AgentIndex { get; private set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x00008EEF File Offset: 0x000070EF
		// (set) Token: 0x060004EC RID: 1260 RVA: 0x00008EF7 File Offset: 0x000070F7
		public int MountAgentIndex { get; private set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x00008F00 File Offset: 0x00007100
		// (set) Token: 0x060004EE RID: 1262 RVA: 0x00008F08 File Offset: 0x00007108
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x00008F11 File Offset: 0x00007111
		// (set) Token: 0x060004F0 RID: 1264 RVA: 0x00008F19 File Offset: 0x00007119
		public BasicCharacterObject Character { get; private set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x00008F22 File Offset: 0x00007122
		// (set) Token: 0x060004F2 RID: 1266 RVA: 0x00008F2A File Offset: 0x0000712A
		public Monster Monster { get; private set; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x00008F33 File Offset: 0x00007133
		// (set) Token: 0x060004F4 RID: 1268 RVA: 0x00008F3B File Offset: 0x0000713B
		public MissionEquipment MissionEquipment { get; private set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x00008F44 File Offset: 0x00007144
		// (set) Token: 0x060004F6 RID: 1270 RVA: 0x00008F4C File Offset: 0x0000714C
		public Equipment SpawnEquipment { get; private set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x00008F55 File Offset: 0x00007155
		// (set) Token: 0x060004F8 RID: 1272 RVA: 0x00008F5D File Offset: 0x0000715D
		public BodyProperties BodyPropertiesValue { get; private set; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x00008F66 File Offset: 0x00007166
		// (set) Token: 0x060004FA RID: 1274 RVA: 0x00008F6E File Offset: 0x0000716E
		public int BodyPropertiesSeed { get; private set; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x00008F77 File Offset: 0x00007177
		// (set) Token: 0x060004FC RID: 1276 RVA: 0x00008F7F File Offset: 0x0000717F
		public bool IsFemale { get; private set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x00008F88 File Offset: 0x00007188
		// (set) Token: 0x060004FE RID: 1278 RVA: 0x00008F90 File Offset: 0x00007190
		public int TeamIndex { get; private set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x00008F99 File Offset: 0x00007199
		// (set) Token: 0x06000500 RID: 1280 RVA: 0x00008FA1 File Offset: 0x000071A1
		public Vec3 Position { get; private set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x00008FAA File Offset: 0x000071AA
		// (set) Token: 0x06000502 RID: 1282 RVA: 0x00008FB2 File Offset: 0x000071B2
		public Vec2 Direction { get; private set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x00008FBB File Offset: 0x000071BB
		// (set) Token: 0x06000504 RID: 1284 RVA: 0x00008FC3 File Offset: 0x000071C3
		public int FormationIndex { get; private set; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x00008FCC File Offset: 0x000071CC
		// (set) Token: 0x06000506 RID: 1286 RVA: 0x00008FD4 File Offset: 0x000071D4
		public bool IsPlayerAgent { get; private set; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x00008FDD File Offset: 0x000071DD
		// (set) Token: 0x06000508 RID: 1288 RVA: 0x00008FE5 File Offset: 0x000071E5
		public uint ClothingColor1 { get; private set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x00008FEE File Offset: 0x000071EE
		// (set) Token: 0x0600050A RID: 1290 RVA: 0x00008FF6 File Offset: 0x000071F6
		public uint ClothingColor2 { get; private set; }

		// Token: 0x0600050B RID: 1291 RVA: 0x00009000 File Offset: 0x00007200
		public CreateAgent(int agentIndex, BasicCharacterObject character, Monster monster, Equipment spawnEquipment, MissionEquipment missionEquipment, BodyProperties bodyPropertiesValue, int bodyPropertiesSeed, bool isFemale, int agentTeamIndex, int agentFormationIndex, uint clothingColor1, uint clothingColor2, int mountAgentIndex, Equipment mountAgentSpawnEquipment, bool isPlayerAgent, Vec3 position, Vec2 direction, NetworkCommunicator peer)
		{
			this.AgentIndex = agentIndex;
			this.MountAgentIndex = mountAgentIndex;
			this.Peer = peer;
			this.Character = character;
			this.Monster = monster;
			this.SpawnEquipment = new Equipment();
			this.MissionEquipment = new MissionEquipment();
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				this.MissionEquipment[equipmentIndex] = missionEquipment[equipmentIndex];
			}
			for (EquipmentIndex equipmentIndex2 = EquipmentIndex.NumAllWeaponSlots; equipmentIndex2 < EquipmentIndex.ArmorItemEndSlot; equipmentIndex2++)
			{
				this.SpawnEquipment[equipmentIndex2] = spawnEquipment.GetEquipmentFromSlot(equipmentIndex2);
			}
			if (this.MountAgentIndex >= 0)
			{
				this.SpawnEquipment[EquipmentIndex.ArmorItemEndSlot] = mountAgentSpawnEquipment[EquipmentIndex.ArmorItemEndSlot];
				this.SpawnEquipment[EquipmentIndex.HorseHarness] = mountAgentSpawnEquipment[EquipmentIndex.HorseHarness];
			}
			else
			{
				this.SpawnEquipment[EquipmentIndex.ArmorItemEndSlot] = default(EquipmentElement);
				this.SpawnEquipment[EquipmentIndex.HorseHarness] = default(EquipmentElement);
			}
			this.BodyPropertiesValue = bodyPropertiesValue;
			this.BodyPropertiesSeed = bodyPropertiesSeed;
			this.IsFemale = isFemale;
			this.TeamIndex = agentTeamIndex;
			this.Position = position;
			this.Direction = direction;
			this.FormationIndex = agentFormationIndex;
			this.ClothingColor1 = clothingColor1;
			this.ClothingColor2 = clothingColor2;
			this.IsPlayerAgent = isPlayerAgent;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00009142 File Offset: 0x00007342
		public CreateAgent()
		{
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0000914C File Offset: 0x0000734C
		protected override bool OnRead()
		{
			bool flag = true;
			this.Character = (BasicCharacterObject)GameNetworkMessage.ReadObjectReferenceFromPacket(MBObjectManager.Instance, CompressionBasic.GUIDCompressionInfo, ref flag);
			this.Monster = (Monster)GameNetworkMessage.ReadObjectReferenceFromPacket(MBObjectManager.Instance, CompressionBasic.GUIDCompressionInfo, ref flag);
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.MountAgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.SpawnEquipment = new Equipment();
			this.MissionEquipment = new MissionEquipment();
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				this.MissionEquipment[equipmentIndex] = ModuleNetworkData.ReadWeaponReferenceFromPacket(MBObjectManager.Instance, ref flag);
			}
			for (EquipmentIndex equipmentIndex2 = EquipmentIndex.NumAllWeaponSlots; equipmentIndex2 < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex2++)
			{
				this.SpawnEquipment.AddEquipmentToSlotWithoutAgent(equipmentIndex2, ModuleNetworkData.ReadItemReferenceFromPacket(MBObjectManager.Instance, ref flag));
			}
			this.IsPlayerAgent = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.BodyPropertiesSeed = ((!this.IsPlayerAgent) ? GameNetworkMessage.ReadIntFromPacket(CompressionBasic.RandomSeedCompressionInfo, ref flag) : 0);
			this.BodyPropertiesValue = GameNetworkMessage.ReadBodyPropertiesFromPacket(ref flag);
			this.IsFemale = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.TeamIndex = GameNetworkMessage.ReadTeamIndexFromPacket(ref flag);
			this.Position = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.PositionCompressionInfo, ref flag);
			this.Direction = GameNetworkMessage.ReadVec2FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref flag).Normalized();
			this.FormationIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			this.ClothingColor1 = GameNetworkMessage.ReadUintFromPacket(CompressionBasic.ColorCompressionInfo, ref flag);
			this.ClothingColor2 = GameNetworkMessage.ReadUintFromPacket(CompressionBasic.ColorCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x000092D0 File Offset: 0x000074D0
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteObjectReferenceToPacket(this.Character, CompressionBasic.GUIDCompressionInfo);
			GameNetworkMessage.WriteObjectReferenceToPacket(this.Monster, CompressionBasic.GUIDCompressionInfo);
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteAgentIndexToPacket(this.MountAgentIndex);
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				ModuleNetworkData.WriteWeaponReferenceToPacket(this.MissionEquipment[equipmentIndex]);
			}
			for (EquipmentIndex equipmentIndex2 = EquipmentIndex.NumAllWeaponSlots; equipmentIndex2 < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex2++)
			{
				ModuleNetworkData.WriteItemReferenceToPacket(this.SpawnEquipment.GetEquipmentFromSlot(equipmentIndex2));
			}
			GameNetworkMessage.WriteBoolToPacket(this.IsPlayerAgent);
			if (!this.IsPlayerAgent)
			{
				GameNetworkMessage.WriteIntToPacket(this.BodyPropertiesSeed, CompressionBasic.RandomSeedCompressionInfo);
			}
			GameNetworkMessage.WriteBodyPropertiesToPacket(this.BodyPropertiesValue);
			GameNetworkMessage.WriteBoolToPacket(this.IsFemale);
			GameNetworkMessage.WriteTeamIndexToPacket(this.TeamIndex);
			GameNetworkMessage.WriteVec3ToPacket(this.Position, CompressionBasic.PositionCompressionInfo);
			GameNetworkMessage.WriteVec2ToPacket(this.Direction, CompressionBasic.UnitVectorCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.FormationIndex, CompressionMission.FormationClassCompressionInfo);
			GameNetworkMessage.WriteUintToPacket(this.ClothingColor1, CompressionBasic.ColorCompressionInfo);
			GameNetworkMessage.WriteUintToPacket(this.ClothingColor2, CompressionBasic.ColorCompressionInfo);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x000093ED File Offset: 0x000075ED
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Agents;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x000093F8 File Offset: 0x000075F8
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Create an agent with index: ",
				this.AgentIndex,
				(this.Peer != null) ? string.Concat(new object[]
				{
					", belonging to peer with Name: ",
					this.Peer.UserName,
					", and peer-index: ",
					this.Peer.Index
				}) : "",
				(this.MountAgentIndex == -1) ? "" : (", owning a mount with index: " + this.MountAgentIndex)
			});
		}
	}
}
