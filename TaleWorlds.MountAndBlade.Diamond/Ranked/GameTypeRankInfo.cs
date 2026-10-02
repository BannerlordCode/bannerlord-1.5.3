using System;

namespace TaleWorlds.MountAndBlade.Diamond.Ranked
{
	// Token: 0x02000161 RID: 353
	[Serializable]
	public class GameTypeRankInfo
	{
		// Token: 0x1700032F RID: 815
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x0000F4E2 File Offset: 0x0000D6E2
		// (set) Token: 0x060009E4 RID: 2532 RVA: 0x0000F4EA File Offset: 0x0000D6EA
		public string GameType { get; private set; }

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x0000F4F3 File Offset: 0x0000D6F3
		// (set) Token: 0x060009E6 RID: 2534 RVA: 0x0000F4FB File Offset: 0x0000D6FB
		public RankBarInfo RankBarInfo { get; private set; }

		// Token: 0x060009E7 RID: 2535 RVA: 0x0000F504 File Offset: 0x0000D704
		public GameTypeRankInfo(string gameType, RankBarInfo rankBarInfo)
		{
			this.GameType = gameType;
			this.RankBarInfo = rankBarInfo;
		}
	}
}
