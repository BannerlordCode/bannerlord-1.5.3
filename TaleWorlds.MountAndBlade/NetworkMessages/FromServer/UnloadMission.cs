using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000D2 RID: 210
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class UnloadMission : GameNetworkMessage
	{
		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x0000EA1B File Offset: 0x0000CC1B
		// (set) Token: 0x060008A2 RID: 2210 RVA: 0x0000EA23 File Offset: 0x0000CC23
		public bool UnloadingForBattleIndexMismatch { get; private set; }

		// Token: 0x060008A3 RID: 2211 RVA: 0x0000EA2C File Offset: 0x0000CC2C
		public UnloadMission()
		{
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0000EA34 File Offset: 0x0000CC34
		public UnloadMission(bool unloadingForBattleIndexMismatch)
		{
			this.UnloadingForBattleIndexMismatch = unloadingForBattleIndexMismatch;
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0000EA44 File Offset: 0x0000CC44
		protected override bool OnRead()
		{
			bool flag = true;
			this.UnloadingForBattleIndexMismatch = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0000EA61 File Offset: 0x0000CC61
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteBoolToPacket(this.UnloadingForBattleIndexMismatch);
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0000EA6E File Offset: 0x0000CC6E
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0000EA76 File Offset: 0x0000CC76
		protected override string OnGetLogFormat()
		{
			return "Unload Mission";
		}
	}
}
