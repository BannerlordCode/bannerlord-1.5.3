using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200014E RID: 334
	[Serializable]
	public class PlayerStatsDuel : PlayerStatsBase
	{
		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x0000DBDE File Offset: 0x0000BDDE
		// (set) Token: 0x06000953 RID: 2387 RVA: 0x0000DBE6 File Offset: 0x0000BDE6
		public int DuelsWon { get; set; }

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x0000DBEF File Offset: 0x0000BDEF
		// (set) Token: 0x06000955 RID: 2389 RVA: 0x0000DBF7 File Offset: 0x0000BDF7
		public int InfantryWins { get; set; }

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000956 RID: 2390 RVA: 0x0000DC00 File Offset: 0x0000BE00
		// (set) Token: 0x06000957 RID: 2391 RVA: 0x0000DC08 File Offset: 0x0000BE08
		public int ArcherWins { get; set; }

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x0000DC11 File Offset: 0x0000BE11
		// (set) Token: 0x06000959 RID: 2393 RVA: 0x0000DC19 File Offset: 0x0000BE19
		public int CavalryWins { get; set; }

		// Token: 0x0600095A RID: 2394 RVA: 0x0000DC22 File Offset: 0x0000BE22
		public PlayerStatsDuel()
		{
			base.GameType = "Duel";
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0000DC35 File Offset: 0x0000BE35
		public void FillWith(PlayerId playerId, int killCount, int deathCount, int assistCount, int winCount, int loseCount, int forfeitCount, int duelsWon, int infantryWins, int archerWins, int cavalryWins)
		{
			base.FillWith(playerId, killCount, deathCount, assistCount, winCount, loseCount, forfeitCount);
			this.DuelsWon = duelsWon;
			this.InfantryWins = infantryWins;
			this.ArcherWins = archerWins;
			this.CavalryWins = cavalryWins;
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0000DC68 File Offset: 0x0000BE68
		public void FillWithNewPlayer(PlayerId playerId)
		{
			this.FillWith(playerId, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0000DC88 File Offset: 0x0000BE88
		public void Update(BattlePlayerStatsDuel stats, bool won)
		{
			base.Update(stats, won);
			this.DuelsWon += stats.DuelsWon;
			this.InfantryWins += stats.InfantryWins;
			this.ArcherWins += stats.ArcherWins;
			this.CavalryWins += stats.CavalryWins;
		}
	}
}
