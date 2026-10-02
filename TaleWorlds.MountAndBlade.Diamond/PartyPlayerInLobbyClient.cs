using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000129 RID: 297
	public class PartyPlayerInLobbyClient
	{
		// Token: 0x17000270 RID: 624
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x0000BC0C File Offset: 0x00009E0C
		// (set) Token: 0x060007B2 RID: 1970 RVA: 0x0000BC14 File Offset: 0x00009E14
		public PlayerId PlayerId { get; private set; }

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x0000BC1D File Offset: 0x00009E1D
		// (set) Token: 0x060007B4 RID: 1972 RVA: 0x0000BC25 File Offset: 0x00009E25
		public string Name { get; private set; }

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x0000BC2E File Offset: 0x00009E2E
		// (set) Token: 0x060007B6 RID: 1974 RVA: 0x0000BC36 File Offset: 0x00009E36
		public bool WaitingInvitation { get; private set; }

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x0000BC3F File Offset: 0x00009E3F
		// (set) Token: 0x060007B8 RID: 1976 RVA: 0x0000BC47 File Offset: 0x00009E47
		public bool IsPartyLeader { get; private set; }

		// Token: 0x060007B9 RID: 1977 RVA: 0x0000BC50 File Offset: 0x00009E50
		public PartyPlayerInLobbyClient(PlayerId playerId, string name, bool isPartyLeader = false)
		{
			this.PlayerId = playerId;
			this.Name = name;
			this.IsPartyLeader = isPartyLeader;
			this.WaitingInvitation = true;
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0000BC74 File Offset: 0x00009E74
		public void SetAtParty()
		{
			this.WaitingInvitation = false;
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0000BC7D File Offset: 0x00009E7D
		public void SetLeader()
		{
			this.IsPartyLeader = true;
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0000BC86 File Offset: 0x00009E86
		public void SetMember()
		{
			this.IsPartyLeader = false;
		}
	}
}
