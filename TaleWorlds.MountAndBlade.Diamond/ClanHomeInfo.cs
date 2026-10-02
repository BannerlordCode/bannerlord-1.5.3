using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000106 RID: 262
	[Serializable]
	public class ClanHomeInfo
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x000070F5 File Offset: 0x000052F5
		// (set) Token: 0x06000596 RID: 1430 RVA: 0x000070FD File Offset: 0x000052FD
		[JsonProperty]
		public bool IsInClan { get; private set; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x00007106 File Offset: 0x00005306
		// (set) Token: 0x06000598 RID: 1432 RVA: 0x0000710E File Offset: 0x0000530E
		[JsonProperty]
		public bool CanCreateClan { get; private set; }

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x00007117 File Offset: 0x00005317
		// (set) Token: 0x0600059A RID: 1434 RVA: 0x0000711F File Offset: 0x0000531F
		[JsonProperty]
		public ClanInfo ClanInfo { get; private set; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x00007128 File Offset: 0x00005328
		// (set) Token: 0x0600059C RID: 1436 RVA: 0x00007130 File Offset: 0x00005330
		[JsonProperty]
		public NotEnoughPlayersInfo NotEnoughPlayersInfo { get; private set; }

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x00007139 File Offset: 0x00005339
		// (set) Token: 0x0600059E RID: 1438 RVA: 0x00007141 File Offset: 0x00005341
		[JsonProperty]
		public PlayerNotEligibleInfo[] PlayerNotEligibleInfos { get; private set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x0000714A File Offset: 0x0000534A
		// (set) Token: 0x060005A0 RID: 1440 RVA: 0x00007152 File Offset: 0x00005352
		[JsonProperty]
		public ClanPlayerInfo[] ClanPlayerInfos { get; private set; }

		// Token: 0x060005A1 RID: 1441 RVA: 0x0000715B File Offset: 0x0000535B
		public ClanHomeInfo(bool isInClan, bool canCreateClan, ClanInfo clanInfo, NotEnoughPlayersInfo notEnoughPlayersInfo, PlayerNotEligibleInfo[] playerNotEligibleInfos, ClanPlayerInfo[] clanPlayerInfos)
		{
			this.IsInClan = isInClan;
			this.CanCreateClan = canCreateClan;
			this.ClanInfo = clanInfo;
			this.NotEnoughPlayersInfo = notEnoughPlayersInfo;
			this.PlayerNotEligibleInfos = playerNotEligibleInfos;
			this.ClanPlayerInfos = clanPlayerInfos;
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00007190 File Offset: 0x00005390
		public static ClanHomeInfo CreateInClanInfo(ClanInfo clanInfo, ClanPlayerInfo[] clanPlayerInfos)
		{
			return new ClanHomeInfo(true, false, clanInfo, null, null, clanPlayerInfos);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0000719D File Offset: 0x0000539D
		public static ClanHomeInfo CreateCanCreateClanInfo()
		{
			return new ClanHomeInfo(false, true, null, null, null, null);
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x000071AA File Offset: 0x000053AA
		public static ClanHomeInfo CreateCantCreateClanInfo(NotEnoughPlayersInfo notEnoughPlayersInfo, PlayerNotEligibleInfo[] playerNotEligibleInfos)
		{
			return new ClanHomeInfo(false, false, null, notEnoughPlayersInfo, playerNotEligibleInfos, null);
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x000071B7 File Offset: 0x000053B7
		public static ClanHomeInfo CreateInvalidStateClanInfo()
		{
			return new ClanHomeInfo(false, false, null, null, null, null);
		}
	}
}
