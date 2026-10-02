using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000010 RID: 16
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class RequestTroopIndexChange : GameNetworkMessage
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000066 RID: 102 RVA: 0x000029F8 File Offset: 0x00000BF8
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00002A00 File Offset: 0x00000C00
		public int SelectedTroopIndex { get; private set; }

		// Token: 0x06000068 RID: 104 RVA: 0x00002A09 File Offset: 0x00000C09
		public RequestTroopIndexChange(int selectedTroopIndex)
		{
			this.SelectedTroopIndex = selectedTroopIndex;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002A18 File Offset: 0x00000C18
		public RequestTroopIndexChange()
		{
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002A20 File Offset: 0x00000C20
		protected override bool OnRead()
		{
			bool flag = true;
			this.SelectedTroopIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.SelectedTroopIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002A42 File Offset: 0x00000C42
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.SelectedTroopIndex, CompressionMission.SelectedTroopIndexCompressionInfo);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002A54 File Offset: 0x00000C54
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Equipment;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002A59 File Offset: 0x00000C59
		protected override string OnGetLogFormat()
		{
			return "Requesting selected troop change to " + this.SelectedTroopIndex;
		}
	}
}
