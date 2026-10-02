using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.Objects.Usables
{
	// Token: 0x020003B4 RID: 948
	public class RemoveExtraWeaponOnStopUsageComponent : UsableMissionObjectComponent
	{
		// Token: 0x06003602 RID: 13826 RVA: 0x000DEE30 File Offset: 0x000DD030
		protected internal override void OnUseStopped(Agent userAgent, bool isSuccessful = true)
		{
			if (!GameNetwork.IsClientOrReplay && !userAgent.Equipment[EquipmentIndex.ExtraWeaponSlot].IsEmpty && !Mission.Current.MissionIsEnding)
			{
				userAgent.Mission.AddTickActionMT(Mission.MissionTickAction.RemoveEquippedWeapon, userAgent, 4, 0);
			}
		}
	}
}
