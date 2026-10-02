using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200003A RID: 58
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class TauntSelected : GameNetworkMessage
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001CB RID: 459 RVA: 0x000042C5 File Offset: 0x000024C5
		// (set) Token: 0x060001CC RID: 460 RVA: 0x000042CD File Offset: 0x000024CD
		public int IndexOfTaunt { get; private set; }

		// Token: 0x060001CD RID: 461 RVA: 0x000042D6 File Offset: 0x000024D6
		public TauntSelected(int indexOfTaunt)
		{
			this.IndexOfTaunt = indexOfTaunt;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000042E5 File Offset: 0x000024E5
		public TauntSelected()
		{
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000042F0 File Offset: 0x000024F0
		protected override bool OnRead()
		{
			bool flag = true;
			this.IndexOfTaunt = GameNetworkMessage.ReadIntFromPacket(CompressionMission.TauntIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00004312 File Offset: 0x00002512
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.IndexOfTaunt, CompressionMission.TauntIndexCompressionInfo);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00004324 File Offset: 0x00002524
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.None;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00004328 File Offset: 0x00002528
		protected override string OnGetLogFormat()
		{
			return "FromClient.CheerSelected: " + this.IndexOfTaunt;
		}
	}
}
