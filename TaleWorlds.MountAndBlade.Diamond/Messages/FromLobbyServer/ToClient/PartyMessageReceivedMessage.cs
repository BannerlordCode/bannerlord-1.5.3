using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000054 RID: 84
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class PartyMessageReceivedMessage : Message
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001BB RID: 443 RVA: 0x000033BC File Offset: 0x000015BC
		// (set) Token: 0x060001BC RID: 444 RVA: 0x000033C4 File Offset: 0x000015C4
		[JsonProperty]
		public string PlayerName { get; private set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001BD RID: 445 RVA: 0x000033CD File Offset: 0x000015CD
		// (set) Token: 0x060001BE RID: 446 RVA: 0x000033D5 File Offset: 0x000015D5
		[JsonProperty]
		public string Message { get; private set; }

		// Token: 0x060001BF RID: 447 RVA: 0x000033DE File Offset: 0x000015DE
		public PartyMessageReceivedMessage()
		{
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000033E6 File Offset: 0x000015E6
		public PartyMessageReceivedMessage(string playerName, string message)
		{
			this.PlayerName = playerName;
			this.Message = message;
		}
	}
}
