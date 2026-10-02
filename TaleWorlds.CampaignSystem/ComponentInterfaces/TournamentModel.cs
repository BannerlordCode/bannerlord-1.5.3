using System;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001ED RID: 493
	public abstract class TournamentModel : MBGameModel<TournamentModel>
	{
		// Token: 0x06001F80 RID: 8064
		public abstract float GetTournamentStartChance(Town town);

		// Token: 0x06001F81 RID: 8065
		public abstract TournamentGame CreateTournament(Town town);

		// Token: 0x06001F82 RID: 8066
		public abstract float GetTournamentEndChance(TournamentGame tournament);

		// Token: 0x06001F83 RID: 8067
		public abstract int GetNumLeaderboardVictoriesAtGameStart();

		// Token: 0x06001F84 RID: 8068
		public abstract float GetTournamentSimulationScore(CharacterObject character);

		// Token: 0x06001F85 RID: 8069
		public abstract int GetRenownReward(Hero winner, Town town);

		// Token: 0x06001F86 RID: 8070
		public abstract int GetInfluenceReward(Hero winner, Town town);

		// Token: 0x06001F87 RID: 8071
		[return: TupleElementNames(new string[] { "skill", "xp" })]
		public abstract ValueTuple<SkillObject, int> GetSkillXpGainFromTournament(Town town);

		// Token: 0x06001F88 RID: 8072
		public abstract Equipment GetParticipantArmor(CharacterObject participant);

		// Token: 0x06001F89 RID: 8073
		public abstract MBList<ItemObject> GetRegularRewardItems(Town town, int regularRewardMinValue, int regularRewardMaxValue);

		// Token: 0x06001F8A RID: 8074
		public abstract MBList<ItemObject> GetEliteRewardItems(Town town, int regularRewardMinValue, int regularRewardMaxValue);
	}
}
