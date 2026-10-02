using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000150 RID: 336
	[Serializable]
	public class PlayerStatsSiege : PlayerStatsBase
	{
		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x0000DD8C File Offset: 0x0000BF8C
		// (set) Token: 0x0600096A RID: 2410 RVA: 0x0000DD94 File Offset: 0x0000BF94
		public int WallsBreached { get; set; }

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x0000DD9D File Offset: 0x0000BF9D
		// (set) Token: 0x0600096C RID: 2412 RVA: 0x0000DDA5 File Offset: 0x0000BFA5
		public int SiegeEngineKills { get; set; }

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x0000DDAE File Offset: 0x0000BFAE
		// (set) Token: 0x0600096E RID: 2414 RVA: 0x0000DDB6 File Offset: 0x0000BFB6
		public int SiegeEnginesDestroyed { get; set; }

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x0000DDBF File Offset: 0x0000BFBF
		// (set) Token: 0x06000970 RID: 2416 RVA: 0x0000DDC7 File Offset: 0x0000BFC7
		public int ObjectiveGoldGained { get; set; }

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x0000DDD0 File Offset: 0x0000BFD0
		// (set) Token: 0x06000972 RID: 2418 RVA: 0x0000DDD8 File Offset: 0x0000BFD8
		public int Score { get; set; }

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000973 RID: 2419 RVA: 0x0000DDE1 File Offset: 0x0000BFE1
		public int AverageScore
		{
			get
			{
				return this.Score / ((base.WinCount + base.LoseCount != 0) ? (base.WinCount + base.LoseCount) : 1);
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000974 RID: 2420 RVA: 0x0000DE09 File Offset: 0x0000C009
		public int AverageKillCount
		{
			get
			{
				return base.KillCount / ((base.WinCount + base.LoseCount != 0) ? (base.WinCount + base.LoseCount) : 1);
			}
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x0000DE31 File Offset: 0x0000C031
		public PlayerStatsSiege()
		{
			base.GameType = "Siege";
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0000DE44 File Offset: 0x0000C044
		public void FillWith(PlayerId playerId, int killCount, int deathCount, int assistCount, int winCount, int loseCount, int forfeitCount, int wallsBreached, int siegeEngineKills, int siegeEnginesDestroyed, int objectiveGoldGained, int score)
		{
			base.FillWith(playerId, killCount, deathCount, assistCount, winCount, loseCount, forfeitCount);
			this.WallsBreached = wallsBreached;
			this.SiegeEngineKills = siegeEngineKills;
			this.SiegeEnginesDestroyed = siegeEnginesDestroyed;
			this.ObjectiveGoldGained = objectiveGoldGained;
			this.Score = score;
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0000DE80 File Offset: 0x0000C080
		public void FillWithNewPlayer(PlayerId playerId)
		{
			this.FillWith(playerId, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0000DEA0 File Offset: 0x0000C0A0
		public void Update(BattlePlayerStatsSiege stats, bool won)
		{
			base.Update(stats, won);
			this.WallsBreached += stats.WallsBreached;
			this.SiegeEngineKills += stats.SiegeEngineKills;
			this.SiegeEnginesDestroyed += stats.SiegeEnginesDestroyed;
			this.ObjectiveGoldGained += stats.ObjectiveGoldGained;
			this.Score += stats.Score;
		}
	}
}
