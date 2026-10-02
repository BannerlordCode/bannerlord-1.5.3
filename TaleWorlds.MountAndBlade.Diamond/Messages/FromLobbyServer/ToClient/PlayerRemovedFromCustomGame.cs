using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200005B RID: 91
	[Serializable]
	public class PlayerRemovedFromCustomGame : Message
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x000034BC File Offset: 0x000016BC
		// (set) Token: 0x060001D5 RID: 469 RVA: 0x000034C4 File Offset: 0x000016C4
		[JsonProperty]
		public DisconnectType DisconnectType { get; private set; }

		// Token: 0x060001D6 RID: 470 RVA: 0x000034CD File Offset: 0x000016CD
		public PlayerRemovedFromCustomGame()
		{
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000034D5 File Offset: 0x000016D5
		public PlayerRemovedFromCustomGame(DisconnectType disconnectType)
		{
			this.DisconnectType = disconnectType;
		}
	}
}
