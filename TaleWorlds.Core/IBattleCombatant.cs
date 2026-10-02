using System;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x0200007E RID: 126
	public interface IBattleCombatant
	{
		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000874 RID: 2164
		TextObject Name { get; }

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000875 RID: 2165
		BattleSideEnum Side { get; }

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000876 RID: 2166
		BasicCultureObject BasicCulture { get; }

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000877 RID: 2167
		BasicCharacterObject General { get; }

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000878 RID: 2168
		Tuple<uint, uint> PrimaryColorPair { get; }

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000879 RID: 2169
		Banner Banner { get; }

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x0600087A RID: 2170
		BattleEnvironment CurrentBattleEnvironment { get; }

		// Token: 0x0600087B RID: 2171
		int GetTacticsSkillAmount();

		// Token: 0x0600087C RID: 2172
		int GetNumberOfMissionReadyTroops();

		// Token: 0x0600087D RID: 2173
		bool IsUnderPlayersCommand(BattleSideEnum playerSide);
	}
}
