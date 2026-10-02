using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200010B RID: 267
	[Serializable]
	public class ClanInfo
	{
		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x0000729D File Offset: 0x0000549D
		// (set) Token: 0x060005BA RID: 1466 RVA: 0x000072A5 File Offset: 0x000054A5
		[JsonProperty]
		public Guid ClanId { get; private set; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x000072AE File Offset: 0x000054AE
		// (set) Token: 0x060005BC RID: 1468 RVA: 0x000072B6 File Offset: 0x000054B6
		[JsonProperty]
		public string Name { get; private set; }

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x000072BF File Offset: 0x000054BF
		// (set) Token: 0x060005BE RID: 1470 RVA: 0x000072C7 File Offset: 0x000054C7
		[JsonProperty]
		public string Tag { get; private set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x000072D0 File Offset: 0x000054D0
		// (set) Token: 0x060005C0 RID: 1472 RVA: 0x000072D8 File Offset: 0x000054D8
		[JsonProperty]
		public string Faction { get; private set; }

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060005C1 RID: 1473 RVA: 0x000072E1 File Offset: 0x000054E1
		// (set) Token: 0x060005C2 RID: 1474 RVA: 0x000072E9 File Offset: 0x000054E9
		[JsonProperty]
		public string Sigil { get; private set; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x000072F2 File Offset: 0x000054F2
		// (set) Token: 0x060005C4 RID: 1476 RVA: 0x000072FA File Offset: 0x000054FA
		[JsonProperty]
		public string InformationText { get; private set; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x00007303 File Offset: 0x00005503
		// (set) Token: 0x060005C6 RID: 1478 RVA: 0x0000730B File Offset: 0x0000550B
		[JsonProperty]
		public ClanPlayer[] Players { get; private set; }

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x00007314 File Offset: 0x00005514
		// (set) Token: 0x060005C8 RID: 1480 RVA: 0x0000731C File Offset: 0x0000551C
		[JsonProperty]
		public ClanAnnouncement[] Announcements { get; private set; }

		// Token: 0x060005C9 RID: 1481 RVA: 0x00007328 File Offset: 0x00005528
		public ClanInfo(Guid clanId, string name, string tag, string faction, string sigil, string information, ClanPlayer[] players, ClanAnnouncement[] announcements)
		{
			this.ClanId = clanId;
			this.Name = name;
			this.Tag = tag;
			this.Faction = faction;
			this.Sigil = sigil;
			this.Players = players;
			this.InformationText = information;
			this.Announcements = announcements;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00007378 File Offset: 0x00005578
		public static ClanInfo CreateUnavailableClanInfo()
		{
			return new ClanInfo(Guid.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, new ClanPlayer[0], new ClanAnnouncement[0]);
		}
	}
}
