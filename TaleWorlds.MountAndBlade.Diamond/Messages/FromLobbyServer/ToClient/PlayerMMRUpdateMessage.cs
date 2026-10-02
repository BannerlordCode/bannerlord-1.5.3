using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Ranked;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000059 RID: 89
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class PlayerMMRUpdateMessage : Message
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00003474 File Offset: 0x00001674
		// (set) Token: 0x060001CE RID: 462 RVA: 0x0000347C File Offset: 0x0000167C
		[JsonProperty]
		public RankBarInfo OldInfo { get; private set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00003485 File Offset: 0x00001685
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x0000348D File Offset: 0x0000168D
		[JsonProperty]
		public RankBarInfo NewInfo { get; private set; }

		// Token: 0x060001D1 RID: 465 RVA: 0x00003496 File Offset: 0x00001696
		public PlayerMMRUpdateMessage()
		{
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000349E File Offset: 0x0000169E
		public PlayerMMRUpdateMessage(RankBarInfo oldInfo, RankBarInfo newInfo)
		{
			this.OldInfo = oldInfo;
			this.NewInfo = newInfo;
		}
	}
}
