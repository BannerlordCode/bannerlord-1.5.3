using System;
using Newtonsoft.Json;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200014D RID: 333
	[Serializable]
	public class PlayerStatsCaptain : PlayerStatsRanked
	{
		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000947 RID: 2375 RVA: 0x0000DAB8 File Offset: 0x0000BCB8
		// (set) Token: 0x06000948 RID: 2376 RVA: 0x0000DAC0 File Offset: 0x0000BCC0
		public int CaptainsKilled { get; set; }

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000949 RID: 2377 RVA: 0x0000DAC9 File Offset: 0x0000BCC9
		// (set) Token: 0x0600094A RID: 2378 RVA: 0x0000DAD1 File Offset: 0x0000BCD1
		public int MVPs { get; set; }

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x0000DADA File Offset: 0x0000BCDA
		// (set) Token: 0x0600094C RID: 2380 RVA: 0x0000DAE2 File Offset: 0x0000BCE2
		public int Score { get; set; }

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x0000DAEB File Offset: 0x0000BCEB
		[JsonIgnore]
		public int AverageScore
		{
			get
			{
				if (this.Score / (base.WinCount + base.LoseCount) == 0)
				{
					return 1;
				}
				return base.WinCount + base.LoseCount;
			}
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0000DB12 File Offset: 0x0000BD12
		public PlayerStatsCaptain()
		{
			base.GameType = "Captain";
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0000DB28 File Offset: 0x0000BD28
		public void FillWith(PlayerId playerId, int killCount, int deathCount, int assistCount, int winCount, int loseCount, int forfeitCount, int rating, int ratingDeviation, string rank, bool evaluating, int evaluationMatchesPlayedCount, int captainsKilled, int mvps, int score)
		{
			base.FillWith(playerId, killCount, deathCount, assistCount, winCount, loseCount, forfeitCount, rating, ratingDeviation, rank, evaluating, evaluationMatchesPlayedCount);
			this.CaptainsKilled = captainsKilled;
			this.MVPs = mvps;
			this.Score = score;
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0000DB68 File Offset: 0x0000BD68
		public void FillWithNewPlayer(PlayerId playerId, int defaultRating, int defaultRatingDeviation)
		{
			this.FillWith(playerId, 0, 0, 0, 0, 0, 0, defaultRating, defaultRatingDeviation, "", true, 0, 0, 0, 0);
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0000DB90 File Offset: 0x0000BD90
		public void Update(BattlePlayerStatsCaptain stats, bool won)
		{
			base.Update(stats, won);
			this.CaptainsKilled += stats.CaptainsKilled;
			this.MVPs += stats.MVPs;
			this.Score += stats.Score;
		}
	}
}
