using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000024 RID: 36
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class ClanInfoChangedMessage : Message
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00002A18 File Offset: 0x00000C18
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x00002A20 File Offset: 0x00000C20
		[JsonProperty]
		public ClanHomeInfo ClanHomeInfo { get; private set; }

		// Token: 0x060000D2 RID: 210 RVA: 0x00002A29 File Offset: 0x00000C29
		public ClanInfoChangedMessage()
		{
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00002A31 File Offset: 0x00000C31
		public ClanInfoChangedMessage(ClanHomeInfo clanHomeInfo)
		{
			this.ClanHomeInfo = clanHomeInfo;
		}
	}
}
