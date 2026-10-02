using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000021 RID: 33
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class ClanCreationRequestMessage : Message
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00002997 File Offset: 0x00000B97
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x0000299F File Offset: 0x00000B9F
		[JsonProperty]
		public string CreatorPlayerName { get; private set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x000029A8 File Offset: 0x00000BA8
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x000029B0 File Offset: 0x00000BB0
		[JsonProperty]
		public PlayerId CreatorPlayerId { get; private set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x000029B9 File Offset: 0x00000BB9
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x000029C1 File Offset: 0x00000BC1
		[JsonProperty]
		public string ClanName { get; private set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000CA RID: 202 RVA: 0x000029CA File Offset: 0x00000BCA
		// (set) Token: 0x060000CB RID: 203 RVA: 0x000029D2 File Offset: 0x00000BD2
		[JsonProperty]
		public string ClanTag { get; private set; }

		// Token: 0x060000CC RID: 204 RVA: 0x000029DB File Offset: 0x00000BDB
		public ClanCreationRequestMessage()
		{
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000029E3 File Offset: 0x00000BE3
		public ClanCreationRequestMessage(PlayerId creatorPlayerId, string creatorPlayerName, string clanName, string clanTag)
		{
			this.CreatorPlayerId = creatorPlayerId;
			this.CreatorPlayerName = creatorPlayerName;
			this.ClanName = clanName;
			this.ClanTag = clanTag;
		}
	}
}
