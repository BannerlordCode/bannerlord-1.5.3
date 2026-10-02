using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000315 RID: 789
	public interface IOnSpawnPerkEffect
	{
		// Token: 0x06002D55 RID: 11605
		int GetExtraTroopCount();

		// Token: 0x06002D56 RID: 11606
		List<ValueTuple<EquipmentIndex, EquipmentElement>> GetAlternativeEquipments(bool isPlayer, List<ValueTuple<EquipmentIndex, EquipmentElement>> alternativeEquipments, bool getAll = false);

		// Token: 0x06002D57 RID: 11607
		float GetDrivenPropertyBonusOnSpawn(bool isPlayer, DrivenProperty drivenProperty, float baseValue);

		// Token: 0x06002D58 RID: 11608
		float GetHitpoints(bool isPlayer);
	}
}
