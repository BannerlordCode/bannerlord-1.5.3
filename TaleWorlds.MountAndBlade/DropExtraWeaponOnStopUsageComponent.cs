using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000377 RID: 887
	internal class DropExtraWeaponOnStopUsageComponent : UsableMissionObjectComponent
	{
		// Token: 0x06003311 RID: 13073 RVA: 0x000D1520 File Offset: 0x000CF720
		protected internal override void OnUseStopped(Agent userAgent, bool isSuccessful = true)
		{
			if (isSuccessful && !GameNetwork.IsClientOrReplay && !userAgent.Equipment[EquipmentIndex.ExtraWeaponSlot].IsEmpty && !Mission.Current.MissionIsEnding)
			{
				userAgent.Mission.AddTickAction(Mission.MissionTickAction.DropItem, userAgent, 4, 0);
			}
		}
	}
}
