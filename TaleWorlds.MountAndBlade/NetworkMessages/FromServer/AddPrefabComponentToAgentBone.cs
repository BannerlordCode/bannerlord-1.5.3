using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000075 RID: 117
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AddPrefabComponentToAgentBone : GameNetworkMessage
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x00007B7B File Offset: 0x00005D7B
		// (set) Token: 0x0600041B RID: 1051 RVA: 0x00007B83 File Offset: 0x00005D83
		public int AgentIndex { get; private set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x00007B8C File Offset: 0x00005D8C
		// (set) Token: 0x0600041D RID: 1053 RVA: 0x00007B94 File Offset: 0x00005D94
		public string PrefabName { get; private set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x00007B9D File Offset: 0x00005D9D
		// (set) Token: 0x0600041F RID: 1055 RVA: 0x00007BA5 File Offset: 0x00005DA5
		public sbyte BoneIndex { get; private set; }

		// Token: 0x06000420 RID: 1056 RVA: 0x00007BAE File Offset: 0x00005DAE
		public AddPrefabComponentToAgentBone(int agentIndex, string prefabName, sbyte boneIndex)
		{
			this.AgentIndex = agentIndex;
			this.PrefabName = prefabName;
			this.BoneIndex = boneIndex;
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00007BCB File Offset: 0x00005DCB
		public AddPrefabComponentToAgentBone()
		{
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00007BD4 File Offset: 0x00005DD4
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.PrefabName = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.BoneIndex = (sbyte)GameNetworkMessage.ReadIntFromPacket(CompressionMission.BoneIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00007C11 File Offset: 0x00005E11
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteStringToPacket(this.PrefabName);
			GameNetworkMessage.WriteIntToPacket((int)this.BoneIndex, CompressionMission.BoneIndexCompressionInfo);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00007C39 File Offset: 0x00005E39
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00007C44 File Offset: 0x00005E44
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Add prefab component: ", this.PrefabName, " on bone with index: ", this.BoneIndex, " on agent with agent-index: ", this.AgentIndex });
		}
	}
}
