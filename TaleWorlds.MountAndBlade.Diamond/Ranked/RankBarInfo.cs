using System;

namespace TaleWorlds.MountAndBlade.Diamond.Ranked
{
	// Token: 0x02000162 RID: 354
	[Serializable]
	public class RankBarInfo
	{
		// Token: 0x17000331 RID: 817
		// (get) Token: 0x060009E8 RID: 2536 RVA: 0x0000F51A File Offset: 0x0000D71A
		// (set) Token: 0x060009E9 RID: 2537 RVA: 0x0000F522 File Offset: 0x0000D722
		public string RankId { get; set; }

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x060009EA RID: 2538 RVA: 0x0000F52B File Offset: 0x0000D72B
		// (set) Token: 0x060009EB RID: 2539 RVA: 0x0000F533 File Offset: 0x0000D733
		public string PreviousRankId { get; set; }

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x060009EC RID: 2540 RVA: 0x0000F53C File Offset: 0x0000D73C
		// (set) Token: 0x060009ED RID: 2541 RVA: 0x0000F544 File Offset: 0x0000D744
		public string NextRankId { get; set; }

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x060009EE RID: 2542 RVA: 0x0000F54D File Offset: 0x0000D74D
		// (set) Token: 0x060009EF RID: 2543 RVA: 0x0000F555 File Offset: 0x0000D755
		public float ProgressPercentage { get; set; }

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x060009F0 RID: 2544 RVA: 0x0000F55E File Offset: 0x0000D75E
		// (set) Token: 0x060009F1 RID: 2545 RVA: 0x0000F566 File Offset: 0x0000D766
		public int Rating { get; set; }

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x060009F2 RID: 2546 RVA: 0x0000F56F File Offset: 0x0000D76F
		// (set) Token: 0x060009F3 RID: 2547 RVA: 0x0000F577 File Offset: 0x0000D777
		public int RatingToNextRank { get; set; }

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x060009F4 RID: 2548 RVA: 0x0000F580 File Offset: 0x0000D780
		// (set) Token: 0x060009F5 RID: 2549 RVA: 0x0000F588 File Offset: 0x0000D788
		public bool IsEvaluating { get; set; }

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x060009F6 RID: 2550 RVA: 0x0000F591 File Offset: 0x0000D791
		// (set) Token: 0x060009F7 RID: 2551 RVA: 0x0000F599 File Offset: 0x0000D799
		public int EvaluationMatchesPlayed { get; set; }

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x060009F8 RID: 2552 RVA: 0x0000F5A2 File Offset: 0x0000D7A2
		// (set) Token: 0x060009F9 RID: 2553 RVA: 0x0000F5AA File Offset: 0x0000D7AA
		public int TotalEvaluationMatchesRequired { get; set; }

		// Token: 0x060009FA RID: 2554 RVA: 0x0000F5B3 File Offset: 0x0000D7B3
		public RankBarInfo()
		{
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0000F5BC File Offset: 0x0000D7BC
		public RankBarInfo(string rankId, string previousRankId, string nextRankId, float progressPercentage, int rating, int ratingToNextRank, bool isEvaluating, int evaluationMatchesPlayed, int totalEvaluationMatchesRequired)
		{
			this.RankId = rankId;
			this.PreviousRankId = previousRankId;
			this.NextRankId = nextRankId;
			this.ProgressPercentage = progressPercentage;
			this.Rating = rating;
			this.RatingToNextRank = ratingToNextRank;
			this.IsEvaluating = isEvaluating;
			this.EvaluationMatchesPlayed = evaluationMatchesPlayed;
			this.TotalEvaluationMatchesRequired = totalEvaluationMatchesRequired;
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0000F614 File Offset: 0x0000D814
		public static RankBarInfo CreateBarInfo(string rankId, string previousRankId, string nextRankId, float progressPercentage, int rating, int ratingToNextRank)
		{
			return new RankBarInfo(rankId, previousRankId, nextRankId, progressPercentage, rating, ratingToNextRank, false, 0, 0);
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0000F634 File Offset: 0x0000D834
		public static RankBarInfo CreateUnrankedInfo(int matchesPlayed, int totalMatchesRequired)
		{
			return new RankBarInfo("", "", "", 0f, 0, 0, true, matchesPlayed, totalMatchesRequired);
		}
	}
}
