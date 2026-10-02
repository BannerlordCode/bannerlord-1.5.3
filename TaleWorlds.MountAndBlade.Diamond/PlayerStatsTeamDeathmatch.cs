using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000152 RID: 338
	[Serializable]
	public class PlayerStatsTeamDeathmatch : PlayerStatsBase
	{
		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000982 RID: 2434 RVA: 0x0000E001 File Offset: 0x0000C201
		// (set) Token: 0x06000983 RID: 2435 RVA: 0x0000E009 File Offset: 0x0000C209
		public int Score { get; set; }

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000984 RID: 2436 RVA: 0x0000E012 File Offset: 0x0000C212
		public float AverageScore
		{
			get
			{
				return (float)this.Score / (float)((base.WinCount + base.LoseCount != 0) ? (base.WinCount + base.LoseCount) : 1);
			}
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0000E03C File Offset: 0x0000C23C
		public PlayerStatsTeamDeathmatch()
		{
			base.GameType = "TeamDeathmatch";
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0000E04F File Offset: 0x0000C24F
		public void FillWith(PlayerId playerId, int killCount, int deathCount, int assistCount, int winCount, int loseCount, int forfeitCount, int score)
		{
			base.FillWith(playerId, killCount, deathCount, assistCount, winCount, loseCount, forfeitCount);
			this.Score = score;
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0000E06C File Offset: 0x0000C26C
		public void FillWithNewPlayer(PlayerId playerId)
		{
			this.FillWith(playerId, 0, 0, 0, 0, 0, 0, 0);
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0000E087 File Offset: 0x0000C287
		public void Update(BattlePlayerStatsTeamDeathmatch stats, bool won)
		{
			base.Update(stats, won);
			this.Score += stats.Score;
		}
	}
}
