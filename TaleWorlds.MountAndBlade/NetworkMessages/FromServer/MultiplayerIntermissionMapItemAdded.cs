using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200005C RID: 92
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class MultiplayerIntermissionMapItemAdded : GameNetworkMessage
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000339 RID: 825 RVA: 0x0000639C File Offset: 0x0000459C
		// (set) Token: 0x0600033A RID: 826 RVA: 0x000063A4 File Offset: 0x000045A4
		public string MapId { get; private set; }

		// Token: 0x0600033B RID: 827 RVA: 0x000063AD File Offset: 0x000045AD
		public MultiplayerIntermissionMapItemAdded()
		{
		}

		// Token: 0x0600033C RID: 828 RVA: 0x000063B5 File Offset: 0x000045B5
		public MultiplayerIntermissionMapItemAdded(string mapId)
		{
			this.MapId = mapId;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x000063C4 File Offset: 0x000045C4
		protected override bool OnRead()
		{
			bool flag = true;
			this.MapId = GameNetworkMessage.ReadStringFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600033E RID: 830 RVA: 0x000063E1 File Offset: 0x000045E1
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.MapId);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x000063EE File Offset: 0x000045EE
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x000063F6 File Offset: 0x000045F6
		protected override string OnGetLogFormat()
		{
			return "Adding map for voting with id: " + this.MapId + ".";
		}
	}
}
