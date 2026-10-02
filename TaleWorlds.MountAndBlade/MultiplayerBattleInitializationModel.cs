using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200020A RID: 522
	public class MultiplayerBattleInitializationModel : BattleInitializationModel
	{
		// Token: 0x06001E78 RID: 7800 RVA: 0x00068A13 File Offset: 0x00066C13
		public override List<FormationClass> GetAllAvailableTroopTypes()
		{
			return new List<FormationClass>();
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x00068A1A File Offset: 0x00066C1A
		protected override bool CanPlayerSideDeployWithOrderOfBattleAux()
		{
			return false;
		}
	}
}
