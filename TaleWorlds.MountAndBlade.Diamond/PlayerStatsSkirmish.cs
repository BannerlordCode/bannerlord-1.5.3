using System;
using Newtonsoft.Json;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000151 RID: 337
	[Serializable]
	public class PlayerStatsSkirmish : PlayerStatsRanked
	{
		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x0000DF14 File Offset: 0x0000C114
		// (set) Token: 0x0600097A RID: 2426 RVA: 0x0000DF1C File Offset: 0x0000C11C
		public int MVPs { get; set; }

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x0000DF25 File Offset: 0x0000C125
		// (set) Token: 0x0600097C RID: 2428 RVA: 0x0000DF2D File Offset: 0x0000C12D
		public int Score { get; set; }

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x0000DF36 File Offset: 0x0000C136
		[JsonIgnore]
		public int AverageScore
		{
			get
			{
				return this.Score / ((base.WinCount + base.LoseCount != 0) ? (base.WinCount + base.LoseCount) : 1);
			}
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0000DF5E File Offset: 0x0000C15E
		public PlayerStatsSkirmish()
		{
			base.GameType = "Skirmish";
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0000DF74 File Offset: 0x0000C174
		public void FillWith(PlayerId playerId, int killCount, int deathCount, int assistCount, int winCount, int loseCount, int forfeitCount, int rating, int ratingDeviation, string rank, bool evaluating, int evaluationMatchesPlayedCount, int mvps, int score)
		{
			base.FillWith(playerId, killCount, deathCount, assistCount, winCount, loseCount, forfeitCount, rating, ratingDeviation, rank, evaluating, evaluationMatchesPlayedCount);
			this.MVPs = mvps;
			this.Score = score;
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x0000DFAC File Offset: 0x0000C1AC
		public void FillWithNewPlayer(PlayerId playerId, int defaultRating, int defaultRatingDeviation)
		{
			this.FillWith(playerId, 0, 0, 0, 0, 0, 0, defaultRating, defaultRatingDeviation, "", true, 0, 0, 0);
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0000DFD1 File Offset: 0x0000C1D1
		public void Update(BattlePlayerStatsSkirmish stats, bool won)
		{
			base.Update(stats, won);
			this.MVPs += stats.MVPs;
			this.Score += stats.Score;
		}
	}
}
