using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.TournamentGames
{
	// Token: 0x020002E6 RID: 742
	public interface ITournamentManager
	{
		// Token: 0x0600284F RID: 10319
		void AddTournament(TournamentGame game);

		// Token: 0x06002850 RID: 10320
		TournamentGame GetTournamentGame(Town town);

		// Token: 0x06002851 RID: 10321
		void OnPlayerJoinMatch(Type gameType);

		// Token: 0x06002852 RID: 10322
		void OnPlayerJoinTournament(Type gameType, Settlement settlement);

		// Token: 0x06002853 RID: 10323
		void OnPlayerWatchTournament(Type gameType, Settlement settlement);

		// Token: 0x06002854 RID: 10324
		void OnPlayerWinMatch(Type gameType);

		// Token: 0x06002855 RID: 10325
		void OnPlayerWinTournament(Type gameType);

		// Token: 0x06002856 RID: 10326
		void InitializeLeaderboardEntry(Hero hero, int initialVictories = 0);

		// Token: 0x06002857 RID: 10327
		void AddLeaderboardEntry(Hero hero);

		// Token: 0x06002858 RID: 10328
		void GivePrizeToWinner(TournamentGame tournament, Hero winner, bool isPlayerParticipated);

		// Token: 0x06002859 RID: 10329
		void DeleteLeaderboardEntry(Hero hero);

		// Token: 0x0600285A RID: 10330
		List<KeyValuePair<Hero, int>> GetLeaderboard();

		// Token: 0x0600285B RID: 10331
		int GetLeaderBoardRank(Hero hero);

		// Token: 0x0600285C RID: 10332
		Hero GetLeaderBoardLeader();

		// Token: 0x0600285D RID: 10333
		void ResolveTournament(TournamentGame tournament, Town town);
	}
}
