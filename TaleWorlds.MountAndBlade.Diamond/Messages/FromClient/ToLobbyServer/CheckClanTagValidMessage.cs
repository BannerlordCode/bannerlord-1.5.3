using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000085 RID: 133
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class CheckClanTagValidMessage : Message
	{
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600028B RID: 651 RVA: 0x00003C45 File Offset: 0x00001E45
		// (set) Token: 0x0600028C RID: 652 RVA: 0x00003C4D File Offset: 0x00001E4D
		[JsonProperty]
		public string ClanTag { get; private set; }

		// Token: 0x0600028D RID: 653 RVA: 0x00003C56 File Offset: 0x00001E56
		public CheckClanTagValidMessage()
		{
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00003C5E File Offset: 0x00001E5E
		public CheckClanTagValidMessage(string clanTag)
		{
			this.ClanTag = clanTag;
		}
	}
}
