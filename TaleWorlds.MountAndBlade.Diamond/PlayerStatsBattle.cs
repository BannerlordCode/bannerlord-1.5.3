using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200014C RID: 332
	[Serializable]
	public class PlayerStatsBattle : PlayerStatsBase
	{
		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x0000DA11 File Offset: 0x0000BC11
		// (set) Token: 0x06000940 RID: 2368 RVA: 0x0000DA19 File Offset: 0x0000BC19
		public int RoundsWon { get; private set; }

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x0000DA22 File Offset: 0x0000BC22
		// (set) Token: 0x06000942 RID: 2370 RVA: 0x0000DA2A File Offset: 0x0000BC2A
		public int RoundsLost { get; private set; }

		// Token: 0x06000943 RID: 2371 RVA: 0x0000DA33 File Offset: 0x0000BC33
		public PlayerStatsBattle()
		{
			base.GameType = "Battle";
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0000DA46 File Offset: 0x0000BC46
		public void FillWith(PlayerId playerId, int killCount, int deathCount, int assistCount, int winCount, int loseCount, int forfeitCount, int roundsWon, int roundsLost)
		{
			base.FillWith(playerId, killCount, deathCount, assistCount, winCount, loseCount, forfeitCount);
			this.RoundsWon = roundsWon;
			this.RoundsLost = roundsLost;
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0000DA6C File Offset: 0x0000BC6C
		public void FillWithNewPlayer(PlayerId playerId)
		{
			this.FillWith(playerId, 0, 0, 0, 0, 0, 0, 0, 0);
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0000DA88 File Offset: 0x0000BC88
		public void Update(BattlePlayerStatsBattle stats, bool won)
		{
			base.Update(stats, won);
			this.RoundsWon += stats.RoundsWon;
			this.RoundsLost += stats.RoundsLost;
		}
	}
}
