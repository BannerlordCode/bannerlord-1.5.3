using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000054 RID: 84
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class GoldGain : GameNetworkMessage
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000599F File Offset: 0x00003B9F
		// (set) Token: 0x060002DA RID: 730 RVA: 0x000059A7 File Offset: 0x00003BA7
		public List<KeyValuePair<ushort, int>> GoldChangeEventList { get; private set; }

		// Token: 0x060002DB RID: 731 RVA: 0x000059B0 File Offset: 0x00003BB0
		public GoldGain(List<KeyValuePair<ushort, int>> goldChangeEventList)
		{
			this.GoldChangeEventList = goldChangeEventList;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x000059BF File Offset: 0x00003BBF
		public GoldGain()
		{
		}

		// Token: 0x060002DD RID: 733 RVA: 0x000059C8 File Offset: 0x00003BC8
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.GoldChangeEventList.Count - 1, CompressionMission.TdmGoldGainTypeCompressionInfo);
			foreach (KeyValuePair<ushort, int> keyValuePair in this.GoldChangeEventList)
			{
				GameNetworkMessage.WriteIntToPacket((int)keyValuePair.Key, CompressionMission.TdmGoldGainTypeCompressionInfo);
				GameNetworkMessage.WriteIntToPacket(keyValuePair.Value, CompressionMission.TdmGoldChangeCompressionInfo);
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00005A50 File Offset: 0x00003C50
		protected override bool OnRead()
		{
			bool flag = true;
			this.GoldChangeEventList = new List<KeyValuePair<ushort, int>>();
			int num = GameNetworkMessage.ReadIntFromPacket(CompressionMission.TdmGoldGainTypeCompressionInfo, ref flag) + 1;
			for (int i = 0; i < num; i++)
			{
				ushort num2 = (ushort)GameNetworkMessage.ReadIntFromPacket(CompressionMission.TdmGoldGainTypeCompressionInfo, ref flag);
				int num3 = GameNetworkMessage.ReadIntFromPacket(CompressionMission.TdmGoldChangeCompressionInfo, ref flag);
				this.GoldChangeEventList.Add(new KeyValuePair<ushort, int>(num2, num3));
			}
			return flag;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00005AB5 File Offset: 0x00003CB5
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00005ABD File Offset: 0x00003CBD
		protected override string OnGetLogFormat()
		{
			return "Gold change events synced.";
		}
	}
}
