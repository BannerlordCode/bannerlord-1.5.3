using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.MissionSpawnHandlers
{
	// Token: 0x020003D4 RID: 980
	public class CustomSiegeMissionSpawnHandler : CustomMissionSpawnHandler
	{
		// Token: 0x06003701 RID: 14081 RVA: 0x000E3C21 File Offset: 0x000E1E21
		public CustomSiegeMissionSpawnHandler(IBattleCombatant defenderBattleCombatant, IBattleCombatant attackerBattleCombatant, bool spawnWithHorses)
		{
			this._battleCombatants = new CustomBattleCombatant[]
			{
				(CustomBattleCombatant)defenderBattleCombatant,
				(CustomBattleCombatant)attackerBattleCombatant
			};
			this._spawnWithHorses = spawnWithHorses;
		}

		// Token: 0x06003702 RID: 14082 RVA: 0x000E3C50 File Offset: 0x000E1E50
		public override void AfterStart()
		{
			int numberOfHealthyMembers = this._battleCombatants[0].NumberOfHealthyMembers;
			int numberOfHealthyMembers2 = this._battleCombatants[1].NumberOfHealthyMembers;
			int num = numberOfHealthyMembers;
			int num2 = numberOfHealthyMembers2;
			this._missionAgentSpawnLogic.SetSpawnHorses(BattleSideEnum.Defender, this._spawnWithHorses);
			this._missionAgentSpawnLogic.SetSpawnHorses(BattleSideEnum.Attacker, this._spawnWithHorses);
			MissionSpawnSettings missionSpawnSettings = CustomMissionSpawnHandler.CreateCustomBattleWaveSpawnSettings();
			this._missionAgentSpawnLogic.InitWithSinglePhase(numberOfHealthyMembers, numberOfHealthyMembers2, num, num2, false, false, in missionSpawnSettings);
		}

		// Token: 0x040017AC RID: 6060
		private CustomBattleCombatant[] _battleCombatants;

		// Token: 0x040017AD RID: 6061
		private bool _spawnWithHorses;
	}
}
