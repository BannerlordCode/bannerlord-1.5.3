using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000A1 RID: 161
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetPlayerByUsernameAndIdMessage : Message
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002EB RID: 747 RVA: 0x00004039 File Offset: 0x00002239
		// (set) Token: 0x060002EC RID: 748 RVA: 0x00004041 File Offset: 0x00002241
		[JsonProperty]
		public string Username { get; private set; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0000404A File Offset: 0x0000224A
		// (set) Token: 0x060002EE RID: 750 RVA: 0x00004052 File Offset: 0x00002252
		[JsonProperty]
		public int UserId { get; private set; }

		// Token: 0x060002EF RID: 751 RVA: 0x0000405B File Offset: 0x0000225B
		public GetPlayerByUsernameAndIdMessage()
		{
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00004063 File Offset: 0x00002263
		public GetPlayerByUsernameAndIdMessage(string username, int userId)
		{
			this.Username = username;
			this.UserId = userId;
		}
	}
}
