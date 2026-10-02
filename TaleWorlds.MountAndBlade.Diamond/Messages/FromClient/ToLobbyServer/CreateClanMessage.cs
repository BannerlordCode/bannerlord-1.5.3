using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000088 RID: 136
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class CreateClanMessage : Message
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000294 RID: 660 RVA: 0x00003C9D File Offset: 0x00001E9D
		// (set) Token: 0x06000295 RID: 661 RVA: 0x00003CA5 File Offset: 0x00001EA5
		[JsonProperty]
		public string ClanName { get; private set; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000296 RID: 662 RVA: 0x00003CAE File Offset: 0x00001EAE
		// (set) Token: 0x06000297 RID: 663 RVA: 0x00003CB6 File Offset: 0x00001EB6
		[JsonProperty]
		public string ClanTag { get; private set; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000298 RID: 664 RVA: 0x00003CBF File Offset: 0x00001EBF
		// (set) Token: 0x06000299 RID: 665 RVA: 0x00003CC7 File Offset: 0x00001EC7
		[JsonProperty]
		public string ClanFaction { get; private set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600029A RID: 666 RVA: 0x00003CD0 File Offset: 0x00001ED0
		// (set) Token: 0x0600029B RID: 667 RVA: 0x00003CD8 File Offset: 0x00001ED8
		[JsonProperty]
		public string ClanSigil { get; private set; }

		// Token: 0x0600029C RID: 668 RVA: 0x00003CE1 File Offset: 0x00001EE1
		public CreateClanMessage()
		{
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00003CE9 File Offset: 0x00001EE9
		public CreateClanMessage(string clanName, string clanTag, string clanFaction, string clanSigil)
		{
			this.ClanName = clanName;
			this.ClanTag = clanTag;
			this.ClanFaction = clanFaction;
			this.ClanSigil = clanSigil;
		}
	}
}
