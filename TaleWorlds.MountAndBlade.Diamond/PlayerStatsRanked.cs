using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200014F RID: 335
	[Serializable]
	public class PlayerStatsRanked : PlayerStatsBase
	{
		// Token: 0x170002FD RID: 765
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x0000DCE9 File Offset: 0x0000BEE9
		// (set) Token: 0x0600095F RID: 2399 RVA: 0x0000DCF1 File Offset: 0x0000BEF1
		public int Rating { get; set; }

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x0000DCFA File Offset: 0x0000BEFA
		// (set) Token: 0x06000961 RID: 2401 RVA: 0x0000DD02 File Offset: 0x0000BF02
		public string Rank { get; set; }

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000962 RID: 2402 RVA: 0x0000DD0B File Offset: 0x0000BF0B
		// (set) Token: 0x06000963 RID: 2403 RVA: 0x0000DD13 File Offset: 0x0000BF13
		public bool Evaluating { get; set; }

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000964 RID: 2404 RVA: 0x0000DD1C File Offset: 0x0000BF1C
		// (set) Token: 0x06000965 RID: 2405 RVA: 0x0000DD24 File Offset: 0x0000BF24
		public int EvaluationMatchesPlayedCount { get; set; }

		// Token: 0x06000966 RID: 2406 RVA: 0x0000DD2D File Offset: 0x0000BF2D
		public void FillWith(PlayerId playerId, int killCount, int deathCount, int assistCount, int winCount, int loseCount, int forfeitCount, int rating, int ratingDeviation, string rank, bool evaluating, int evaluationMatchesPlayedCount)
		{
			base.FillWith(playerId, killCount, deathCount, assistCount, winCount, loseCount, forfeitCount);
			this.Rating = rating;
			this.Rank = rank;
			this.Evaluating = evaluating;
			this.EvaluationMatchesPlayedCount = evaluationMatchesPlayedCount;
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0000DD60 File Offset: 0x0000BF60
		public virtual void FillWithNewPlayer(PlayerId playerId, string gameType, int defaultRating, int defaultRatingDeviation)
		{
			this.FillWith(playerId, 0, 0, 0, 0, 0, 0, defaultRating, defaultRatingDeviation, "", true, 0);
		}
	}
}
