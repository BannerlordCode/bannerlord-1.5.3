using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x020000AB RID: 171
	public interface ISandBoxMissionManager
	{
		// Token: 0x060013C7 RID: 5063
		IMission OpenTournamentFightMission(string scene, TournamentGame tournamentGame, Settlement settlement, CultureObject culture, bool isPlayerParticipating);

		// Token: 0x060013C8 RID: 5064
		IMission OpenTournamentHorseRaceMission(string scene, TournamentGame tournamentGame, Settlement settlement, CultureObject culture, bool isPlayerParticipating);

		// Token: 0x060013C9 RID: 5065
		IMission OpenTournamentJoustingMission(string scene, TournamentGame tournamentGame, Settlement settlement, CultureObject culture, bool isPlayerParticipating);

		// Token: 0x060013CA RID: 5066
		IMission OpenTournamentArcheryMission(string scene, TournamentGame tournamentGame, Settlement settlement, CultureObject culture, bool isPlayerParticipating);

		// Token: 0x060013CB RID: 5067
		IMission OpenBattleChallengeMission(string scene, IList<Hero> priorityCharsAttacker, IList<Hero> priorityCharsDefender);
	}
}
