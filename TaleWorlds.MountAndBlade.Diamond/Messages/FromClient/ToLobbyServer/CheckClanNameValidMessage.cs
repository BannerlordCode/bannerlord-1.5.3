using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000084 RID: 132
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class CheckClanNameValidMessage : Message
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000287 RID: 647 RVA: 0x00003C1D File Offset: 0x00001E1D
		// (set) Token: 0x06000288 RID: 648 RVA: 0x00003C25 File Offset: 0x00001E25
		[JsonProperty]
		public string ClanName { get; private set; }

		// Token: 0x06000289 RID: 649 RVA: 0x00003C2E File Offset: 0x00001E2E
		public CheckClanNameValidMessage()
		{
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00003C36 File Offset: 0x00001E36
		public CheckClanNameValidMessage(string clanName)
		{
			this.ClanName = clanName;
		}
	}
}
