using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000088 RID: 136
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class CreateFreeMountAgent : GameNetworkMessage
	{
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x00009839 File Offset: 0x00007A39
		// (set) Token: 0x06000532 RID: 1330 RVA: 0x00009841 File Offset: 0x00007A41
		public int AgentIndex { get; private set; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x0000984A File Offset: 0x00007A4A
		// (set) Token: 0x06000534 RID: 1332 RVA: 0x00009852 File Offset: 0x00007A52
		public EquipmentElement HorseItem { get; private set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x0000985B File Offset: 0x00007A5B
		// (set) Token: 0x06000536 RID: 1334 RVA: 0x00009863 File Offset: 0x00007A63
		public EquipmentElement HorseHarnessItem { get; private set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x0000986C File Offset: 0x00007A6C
		// (set) Token: 0x06000538 RID: 1336 RVA: 0x00009874 File Offset: 0x00007A74
		public Vec3 Position { get; private set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x0000987D File Offset: 0x00007A7D
		// (set) Token: 0x0600053A RID: 1338 RVA: 0x00009885 File Offset: 0x00007A85
		public Vec2 Direction { get; private set; }

		// Token: 0x0600053B RID: 1339 RVA: 0x00009890 File Offset: 0x00007A90
		public CreateFreeMountAgent(Agent agent, Vec3 position, Vec2 direction)
		{
			this.AgentIndex = agent.Index;
			this.HorseItem = agent.SpawnEquipment.GetEquipmentFromSlot(EquipmentIndex.ArmorItemEndSlot);
			this.HorseHarnessItem = agent.SpawnEquipment.GetEquipmentFromSlot(EquipmentIndex.HorseHarness);
			this.Position = position;
			this.Direction = direction.Normalized();
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x000098E9 File Offset: 0x00007AE9
		public CreateFreeMountAgent()
		{
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x000098F4 File Offset: 0x00007AF4
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.HorseItem = ModuleNetworkData.ReadItemReferenceFromPacket(Game.Current.ObjectManager, ref flag);
			this.HorseHarnessItem = ModuleNetworkData.ReadItemReferenceFromPacket(Game.Current.ObjectManager, ref flag);
			this.Position = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.PositionCompressionInfo, ref flag);
			this.Direction = GameNetworkMessage.ReadVec2FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00009964 File Offset: 0x00007B64
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			ModuleNetworkData.WriteItemReferenceToPacket(this.HorseItem);
			ModuleNetworkData.WriteItemReferenceToPacket(this.HorseHarnessItem);
			GameNetworkMessage.WriteVec3ToPacket(this.Position, CompressionBasic.PositionCompressionInfo);
			GameNetworkMessage.WriteVec2ToPacket(this.Direction, CompressionBasic.UnitVectorCompressionInfo);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x000099B2 File Offset: 0x00007BB2
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Agents;
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x000099BA File Offset: 0x00007BBA
		protected override string OnGetLogFormat()
		{
			return "Create a mount-agent with index: " + this.AgentIndex;
		}
	}
}
