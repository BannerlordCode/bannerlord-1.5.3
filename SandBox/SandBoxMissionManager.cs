using System;
using System.Collections.Generic;
using SandBox.Tournaments;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;

namespace SandBox
{
	// Token: 0x02000025 RID: 37
	public class SandBoxMissionManager : ISandBoxMissionManager
	{
		// Token: 0x06000119 RID: 281 RVA: 0x00007842 File Offset: 0x00005A42
		public IMission OpenTournamentFightMission(string scene, TournamentGame tournamentGame, Settlement settlement, CultureObject culture, bool isPlayerParticipating)
		{
			return TournamentMissionStarter.OpenTournamentFightMission(scene, tournamentGame, settlement, culture, isPlayerParticipating);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00007850 File Offset: 0x00005A50
		public IMission OpenTournamentHorseRaceMission(string scene, TournamentGame tournamentGame, Settlement settlement, CultureObject culture, bool isPlayerParticipating)
		{
			return TournamentMissionStarter.OpenTournamentHorseRaceMission(scene, tournamentGame, settlement, culture, isPlayerParticipating);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000785E File Offset: 0x00005A5E
		public IMission OpenTournamentJoustingMission(string scene, TournamentGame tournamentGame, Settlement settlement, CultureObject culture, bool isPlayerParticipating)
		{
			return TournamentMissionStarter.OpenTournamentJoustingMission(scene, tournamentGame, settlement, culture, isPlayerParticipating);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000786C File Offset: 0x00005A6C
		public IMission OpenTournamentArcheryMission(string scene, TournamentGame tournamentGame, Settlement settlement, CultureObject culture, bool isPlayerParticipating)
		{
			return TournamentMissionStarter.OpenTournamentArcheryMission(scene, tournamentGame, settlement, culture, isPlayerParticipating);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000787A File Offset: 0x00005A7A
		IMission ISandBoxMissionManager.OpenBattleChallengeMission(string scene, IList<Hero> priorityCharsAttacker, IList<Hero> priorityCharsDefender)
		{
			return TournamentMissionStarter.OpenBattleChallengeMission(scene, priorityCharsAttacker, priorityCharsDefender);
		}
	}
}
