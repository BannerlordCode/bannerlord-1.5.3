using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200010D RID: 269
	[Serializable]
	public class ClanLeaderboardEntry
	{
		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x000073EA File Offset: 0x000055EA
		// (set) Token: 0x060005D2 RID: 1490 RVA: 0x000073F2 File Offset: 0x000055F2
		public Guid ClanId { get; private set; }

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x000073FB File Offset: 0x000055FB
		// (set) Token: 0x060005D4 RID: 1492 RVA: 0x00007403 File Offset: 0x00005603
		public string Name { get; private set; }

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x0000740C File Offset: 0x0000560C
		// (set) Token: 0x060005D6 RID: 1494 RVA: 0x00007414 File Offset: 0x00005614
		public string Tag { get; private set; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x0000741D File Offset: 0x0000561D
		// (set) Token: 0x060005D8 RID: 1496 RVA: 0x00007425 File Offset: 0x00005625
		public string Sigil { get; private set; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x0000742E File Offset: 0x0000562E
		// (set) Token: 0x060005DA RID: 1498 RVA: 0x00007436 File Offset: 0x00005636
		public int WinCount { get; private set; }

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x0000743F File Offset: 0x0000563F
		// (set) Token: 0x060005DC RID: 1500 RVA: 0x00007447 File Offset: 0x00005647
		public int LossCount { get; private set; }

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060005DD RID: 1501 RVA: 0x00007450 File Offset: 0x00005650
		// (set) Token: 0x060005DE RID: 1502 RVA: 0x00007458 File Offset: 0x00005658
		public float Score { get; private set; }

		// Token: 0x060005DF RID: 1503 RVA: 0x00007461 File Offset: 0x00005661
		[JsonConstructor]
		public ClanLeaderboardEntry(Guid clanId, string name, string tag, string sigil, int winCount, int lossCount, float score)
		{
			this.ClanId = clanId;
			this.Name = name;
			this.Tag = tag;
			this.Sigil = sigil;
			this.WinCount = winCount;
			this.LossCount = lossCount;
			this.Score = score;
		}
	}
}
