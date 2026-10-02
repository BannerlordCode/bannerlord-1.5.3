using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.MissionSpawnHandlers
{
	// Token: 0x020003D3 RID: 979
	public class CustomSallyOutMissionController : SallyOutMissionController
	{
		// Token: 0x060036FF RID: 14079 RVA: 0x000E3BDA File Offset: 0x000E1DDA
		public CustomSallyOutMissionController(IBattleCombatant defenderBattleCombatant, IBattleCombatant attackerBattleCombatant)
			: base(true)
		{
			this._battleCombatants = new CustomBattleCombatant[]
			{
				(CustomBattleCombatant)defenderBattleCombatant,
				(CustomBattleCombatant)attackerBattleCombatant
			};
		}

		// Token: 0x06003700 RID: 14080 RVA: 0x000E3C01 File Offset: 0x000E1E01
		protected override void GetInitialTroopCounts(out int besiegedTotalTroopCount, out int besiegerTotalTroopCount)
		{
			besiegedTotalTroopCount = this._battleCombatants[0].NumberOfHealthyMembers;
			besiegerTotalTroopCount = this._battleCombatants[1].NumberOfHealthyMembers;
		}

		// Token: 0x040017AB RID: 6059
		private readonly CustomBattleCombatant[] _battleCombatants;
	}
}
