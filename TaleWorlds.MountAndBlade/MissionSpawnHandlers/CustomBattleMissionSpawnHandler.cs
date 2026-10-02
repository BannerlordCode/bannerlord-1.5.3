using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.MissionSpawnHandlers
{
	// Token: 0x020003D1 RID: 977
	public class CustomBattleMissionSpawnHandler : CustomMissionSpawnHandler
	{
		// Token: 0x060036FA RID: 14074 RVA: 0x000E3B06 File Offset: 0x000E1D06
		public CustomBattleMissionSpawnHandler(CustomBattleCombatant defenderParty, CustomBattleCombatant attackerParty)
		{
			this._defenderParty = defenderParty;
			this._attackerParty = attackerParty;
		}

		// Token: 0x060036FB RID: 14075 RVA: 0x000E3B1C File Offset: 0x000E1D1C
		public override void AfterStart()
		{
			int numberOfHealthyMembers = this._defenderParty.NumberOfHealthyMembers;
			int numberOfHealthyMembers2 = this._attackerParty.NumberOfHealthyMembers;
			int num = numberOfHealthyMembers;
			int num2 = numberOfHealthyMembers2;
			this._missionAgentSpawnLogic.SetSpawnHorses(BattleSideEnum.Defender, true);
			this._missionAgentSpawnLogic.SetSpawnHorses(BattleSideEnum.Attacker, true);
			MissionSpawnSettings missionSpawnSettings = CustomMissionSpawnHandler.CreateCustomBattleWaveSpawnSettings();
			this._missionAgentSpawnLogic.InitWithSinglePhase(numberOfHealthyMembers, numberOfHealthyMembers2, num, num2, true, true, in missionSpawnSettings);
		}

		// Token: 0x040017A8 RID: 6056
		private CustomBattleCombatant _defenderParty;

		// Token: 0x040017A9 RID: 6057
		private CustomBattleCombatant _attackerParty;
	}
}
