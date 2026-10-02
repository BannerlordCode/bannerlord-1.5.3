using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200005F RID: 95
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class MultiplayerIntermissionUsableMapAdded : GameNetworkMessage
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000355 RID: 853 RVA: 0x00006577 File Offset: 0x00004777
		// (set) Token: 0x06000356 RID: 854 RVA: 0x0000657F File Offset: 0x0000477F
		public string MapId { get; private set; }

		// Token: 0x06000357 RID: 855 RVA: 0x00006588 File Offset: 0x00004788
		public MultiplayerIntermissionUsableMapAdded()
		{
			this.CompatibleGameTypes = new List<string>();
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000659B File Offset: 0x0000479B
		public MultiplayerIntermissionUsableMapAdded(string mapId, bool isCompatibleWithAllGameTypes, int compatibleGameTypeCount, List<string> compatibleGameTypes)
		{
			this.MapId = mapId;
			this.IsCompatibleWithAllGameTypes = isCompatibleWithAllGameTypes;
			this.CompatibleGameTypeCount = compatibleGameTypeCount;
			this.CompatibleGameTypes = compatibleGameTypes;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x000065C0 File Offset: 0x000047C0
		protected override bool OnRead()
		{
			bool flag = true;
			this.MapId = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.IsCompatibleWithAllGameTypes = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.CompatibleGameTypeCount = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.IntermissionMapVoteItemCountCompressionInfo, ref flag);
			for (int i = 0; i < this.CompatibleGameTypeCount; i++)
			{
				this.CompatibleGameTypes.Add(GameNetworkMessage.ReadStringFromPacket(ref flag));
			}
			return flag;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00006620 File Offset: 0x00004820
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.MapId);
			GameNetworkMessage.WriteBoolToPacket(this.IsCompatibleWithAllGameTypes);
			GameNetworkMessage.WriteIntToPacket(this.CompatibleGameTypeCount, CompressionBasic.IntermissionMapVoteItemCountCompressionInfo);
			for (int i = 0; i < this.CompatibleGameTypeCount; i++)
			{
				GameNetworkMessage.WriteStringToPacket(this.CompatibleGameTypes[i]);
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00006675 File Offset: 0x00004875
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000667D File Offset: 0x0000487D
		protected override string OnGetLogFormat()
		{
			return "Adding usable map with id: " + this.MapId + ".";
		}

		// Token: 0x040000A2 RID: 162
		public bool IsCompatibleWithAllGameTypes;

		// Token: 0x040000A3 RID: 163
		public int CompatibleGameTypeCount;

		// Token: 0x040000A4 RID: 164
		public List<string> CompatibleGameTypes;
	}
}
