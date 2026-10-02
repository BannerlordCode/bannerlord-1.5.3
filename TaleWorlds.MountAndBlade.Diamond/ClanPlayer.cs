using System;
using Newtonsoft.Json;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200010E RID: 270
	[Serializable]
	public class ClanPlayer
	{
		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060005E0 RID: 1504 RVA: 0x0000749E File Offset: 0x0000569E
		// (set) Token: 0x060005E1 RID: 1505 RVA: 0x000074A6 File Offset: 0x000056A6
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x000074AF File Offset: 0x000056AF
		// (set) Token: 0x060005E3 RID: 1507 RVA: 0x000074B7 File Offset: 0x000056B7
		[JsonProperty]
		public Guid ClanId { get; private set; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x000074C0 File Offset: 0x000056C0
		// (set) Token: 0x060005E5 RID: 1509 RVA: 0x000074C8 File Offset: 0x000056C8
		[JsonProperty]
		public ClanPlayerRole Role { get; private set; }

		// Token: 0x060005E6 RID: 1510 RVA: 0x000074D1 File Offset: 0x000056D1
		public ClanPlayer(PlayerId playerId, Guid clanId, ClanPlayerRole role)
		{
			this.PlayerId = playerId;
			this.ClanId = clanId;
			this.Role = role;
		}
	}
}
