using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200007F RID: 127
	public interface IBattleObserver
	{
		// Token: 0x0600087E RID: 2174
		void TroopNumberChanged(BattleSideEnum side, IBattleCombatant battleCombatant, BasicCharacterObject character, int number = 0, int numberKilled = 0, int numberWounded = 0, int numberRouted = 0, int killCount = 0, int numberReadyToUpgrade = 0);

		// Token: 0x0600087F RID: 2175
		void TroopSideChanged(BattleSideEnum prevSide, BattleSideEnum newSide, IBattleCombatant battleCombatant, BasicCharacterObject character);

		// Token: 0x06000880 RID: 2176
		void HeroSkillIncreased(BattleSideEnum side, IBattleCombatant battleCombatant, BasicCharacterObject heroCharacter, SkillObject skill);

		// Token: 0x06000881 RID: 2177
		void BattleResultsReady();
	}
}
