using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200001C RID: 28
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class RequestCultureChange : GameNetworkMessage
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x000032EE File Offset: 0x000014EE
		// (set) Token: 0x060000D6 RID: 214 RVA: 0x000032F6 File Offset: 0x000014F6
		public BasicCultureObject Culture { get; private set; }

		// Token: 0x060000D7 RID: 215 RVA: 0x000032FF File Offset: 0x000014FF
		public RequestCultureChange()
		{
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00003307 File Offset: 0x00001507
		public RequestCultureChange(BasicCultureObject culture)
		{
			this.Culture = culture;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00003316 File Offset: 0x00001516
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteObjectReferenceToPacket(this.Culture, CompressionBasic.GUIDCompressionInfo);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00003328 File Offset: 0x00001528
		protected override bool OnRead()
		{
			bool flag = true;
			this.Culture = (BasicCultureObject)GameNetworkMessage.ReadObjectReferenceFromPacket(MBObjectManager.Instance, CompressionBasic.GUIDCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00003354 File Offset: 0x00001554
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000335C File Offset: 0x0000155C
		protected override string OnGetLogFormat()
		{
			return "Requested culture: " + this.Culture.Name;
		}
	}
}
