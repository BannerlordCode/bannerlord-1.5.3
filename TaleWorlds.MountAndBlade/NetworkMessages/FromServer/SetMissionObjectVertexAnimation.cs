using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000B3 RID: 179
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectVertexAnimation : GameNetworkMessage
	{
		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x0000C8E1 File Offset: 0x0000AAE1
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x0000C8E9 File Offset: 0x0000AAE9
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x0000C8F2 File Offset: 0x0000AAF2
		// (set) Token: 0x06000734 RID: 1844 RVA: 0x0000C8FA File Offset: 0x0000AAFA
		public int BeginKey { get; private set; }

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x0000C903 File Offset: 0x0000AB03
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x0000C90B File Offset: 0x0000AB0B
		public int EndKey { get; private set; }

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x0000C914 File Offset: 0x0000AB14
		// (set) Token: 0x06000738 RID: 1848 RVA: 0x0000C91C File Offset: 0x0000AB1C
		public float Speed { get; private set; }

		// Token: 0x06000739 RID: 1849 RVA: 0x0000C925 File Offset: 0x0000AB25
		public SetMissionObjectVertexAnimation(MissionObjectId missionObjectId, int beginKey, int endKey, float speed)
		{
			this.MissionObjectId = missionObjectId;
			this.BeginKey = beginKey;
			this.EndKey = endKey;
			this.Speed = speed;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0000C94A File Offset: 0x0000AB4A
		public SetMissionObjectVertexAnimation()
		{
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0000C954 File Offset: 0x0000AB54
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.BeginKey = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AnimationKeyCompressionInfo, ref flag);
			this.EndKey = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AnimationKeyCompressionInfo, ref flag);
			this.Speed = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.VertexAnimationSpeedCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0000C9A7 File Offset: 0x0000ABA7
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteIntToPacket(this.BeginKey, CompressionBasic.AnimationKeyCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.EndKey, CompressionBasic.AnimationKeyCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.Speed, CompressionBasic.VertexAnimationSpeedCompressionInfo);
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0000C9E4 File Offset: 0x0000ABE4
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0000C9EC File Offset: 0x0000ABEC
		protected override string OnGetLogFormat()
		{
			return "Set Vertex Animation on MissionObject with ID: " + this.MissionObjectId;
		}
	}
}
