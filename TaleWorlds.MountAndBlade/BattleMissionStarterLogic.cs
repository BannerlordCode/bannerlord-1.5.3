using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000281 RID: 641
	public class BattleMissionStarterLogic : MissionLogic
	{
		// Token: 0x060023E2 RID: 9186 RVA: 0x00080148 File Offset: 0x0007E348
		public BattleMissionStarterLogic()
		{
		}

		// Token: 0x060023E3 RID: 9187 RVA: 0x00080150 File Offset: 0x0007E350
		public BattleMissionStarterLogic(IMissionTroopSupplier defenderTroopSupplier = null, IMissionTroopSupplier attackerTroopSupplier = null)
		{
		}

		// Token: 0x060023E4 RID: 9188 RVA: 0x00080158 File Offset: 0x0007E358
		public override void AfterStart()
		{
			base.Mission.SetMissionMode(MissionMode.Battle, true);
		}
	}
}
