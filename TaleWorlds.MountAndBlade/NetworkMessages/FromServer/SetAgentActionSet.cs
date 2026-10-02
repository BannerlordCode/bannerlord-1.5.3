using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200009D RID: 157
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetAgentActionSet : GameNetworkMessage
	{
		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x0000B15E File Offset: 0x0000935E
		// (set) Token: 0x06000639 RID: 1593 RVA: 0x0000B166 File Offset: 0x00009366
		public int AgentIndex { get; private set; }

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x0000B16F File Offset: 0x0000936F
		// (set) Token: 0x0600063B RID: 1595 RVA: 0x0000B177 File Offset: 0x00009377
		public MBActionSet ActionSet { get; private set; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600063C RID: 1596 RVA: 0x0000B180 File Offset: 0x00009380
		// (set) Token: 0x0600063D RID: 1597 RVA: 0x0000B188 File Offset: 0x00009388
		public int NumPaces { get; private set; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x0000B191 File Offset: 0x00009391
		// (set) Token: 0x0600063F RID: 1599 RVA: 0x0000B199 File Offset: 0x00009399
		public int MonsterUsageSetIndex { get; private set; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x0000B1A2 File Offset: 0x000093A2
		// (set) Token: 0x06000641 RID: 1601 RVA: 0x0000B1AA File Offset: 0x000093AA
		public float WalkingSpeedLimit { get; private set; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x0000B1B3 File Offset: 0x000093B3
		// (set) Token: 0x06000643 RID: 1603 RVA: 0x0000B1BB File Offset: 0x000093BB
		public float CrouchWalkingSpeedLimit { get; private set; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x0000B1C4 File Offset: 0x000093C4
		// (set) Token: 0x06000645 RID: 1605 RVA: 0x0000B1CC File Offset: 0x000093CC
		public float StepSize { get; private set; }

		// Token: 0x06000646 RID: 1606 RVA: 0x0000B1D8 File Offset: 0x000093D8
		public SetAgentActionSet(int agentIndex, AnimationSystemData animationSystemData)
		{
			this.AgentIndex = agentIndex;
			this.ActionSet = animationSystemData.ActionSet;
			this.NumPaces = animationSystemData.NumPaces;
			this.MonsterUsageSetIndex = animationSystemData.MonsterUsageSetIndex;
			this.WalkingSpeedLimit = animationSystemData.WalkingSpeedLimit;
			this.CrouchWalkingSpeedLimit = animationSystemData.CrouchWalkingSpeedLimit;
			this.StepSize = animationSystemData.StepSize;
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0000B23A File Offset: 0x0000943A
		public SetAgentActionSet()
		{
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0000B244 File Offset: 0x00009444
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.ActionSet = GameNetworkMessage.ReadActionSetReferenceFromPacket(CompressionMission.ActionSetCompressionInfo, ref flag);
			this.NumPaces = GameNetworkMessage.ReadIntFromPacket(CompressionMission.NumberOfPacesCompressionInfo, ref flag);
			this.MonsterUsageSetIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.MonsterUsageSetCompressionInfo, ref flag);
			this.WalkingSpeedLimit = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.WalkingSpeedLimitCompressionInfo, ref flag);
			this.CrouchWalkingSpeedLimit = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.WalkingSpeedLimitCompressionInfo, ref flag);
			this.StepSize = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.StepSizeCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0000B2D0 File Offset: 0x000094D0
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteActionSetReferenceToPacket(this.ActionSet, CompressionMission.ActionSetCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.NumPaces, CompressionMission.NumberOfPacesCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.MonsterUsageSetIndex, CompressionMission.MonsterUsageSetCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.WalkingSpeedLimit, CompressionMission.WalkingSpeedLimitCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.CrouchWalkingSpeedLimit, CompressionMission.WalkingSpeedLimitCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.StepSize, CompressionMission.StepSizeCompressionInfo);
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0000B348 File Offset: 0x00009548
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentAnimations;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0000B350 File Offset: 0x00009550
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set ActionSet: ", this.ActionSet, " on agent with agent-index: ", this.AgentIndex });
		}
	}
}
