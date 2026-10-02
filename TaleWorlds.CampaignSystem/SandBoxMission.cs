using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x020000AA RID: 170
	public class SandBoxMission
	{
		// Token: 0x060013C1 RID: 5057 RVA: 0x0005AD16 File Offset: 0x00058F16
		public static IMission OpenTournamentArcheryMission(string scene, TournamentGame tournamentGame, Settlement settlement, CultureObject culture, bool isPlayerParticipating)
		{
			return SandBoxManager.Instance.SandBoxMissionManager.OpenTournamentArcheryMission(scene, tournamentGame, settlement, culture, isPlayerParticipating);
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x0005AD2D File Offset: 0x00058F2D
		public static IMission OpenTournamentFightMission(string scene, TournamentGame tournamentGame, Settlement settlement, CultureObject culture, bool isPlayerParticipating)
		{
			return SandBoxManager.Instance.SandBoxMissionManager.OpenTournamentFightMission(scene, tournamentGame, settlement, culture, isPlayerParticipating);
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x0005AD44 File Offset: 0x00058F44
		public static IMission OpenTournamentHorseRaceMission(string scene, TournamentGame tournamentGame, Settlement settlement, CultureObject culture, bool isPlayerParticipating)
		{
			return SandBoxManager.Instance.SandBoxMissionManager.OpenTournamentHorseRaceMission(scene, tournamentGame, settlement, culture, isPlayerParticipating);
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x0005AD5B File Offset: 0x00058F5B
		public static IMission OpenTournamentJoustingMission(string scene, TournamentGame tournamentGame, Settlement settlement, CultureObject culture, bool isPlayerParticipating)
		{
			return SandBoxManager.Instance.SandBoxMissionManager.OpenTournamentJoustingMission(scene, tournamentGame, settlement, culture, isPlayerParticipating);
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x0005AD72 File Offset: 0x00058F72
		public static IMission OpenBattleChallengeMission(string scene, IList<Hero> priorityCharsAttacker, IList<Hero> priorityCharsDefender)
		{
			return SandBoxManager.Instance.SandBoxMissionManager.OpenBattleChallengeMission(scene, priorityCharsAttacker, priorityCharsDefender);
		}
	}
}
