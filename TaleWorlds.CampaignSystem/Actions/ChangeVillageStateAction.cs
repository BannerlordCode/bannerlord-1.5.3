using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004CC RID: 1228
	public static class ChangeVillageStateAction
	{
		// Token: 0x06004D50 RID: 19792 RVA: 0x001871E0 File Offset: 0x001853E0
		private static void ApplyInternal(Village village, Village.VillageStates newState, MobileParty raiderParty)
		{
			Village.VillageStates villageState = village.VillageState;
			if (newState != villageState)
			{
				village.VillageState = newState;
				CampaignEventDispatcher.Instance.OnVillageStateChanged(village, villageState, village.VillageState, raiderParty);
				village.Settlement.Party.SetLevelMaskIsDirty();
			}
		}

		// Token: 0x06004D51 RID: 19793 RVA: 0x00187222 File Offset: 0x00185422
		public static void ApplyBySettingToNormal(Settlement settlement)
		{
			ChangeVillageStateAction.ApplyInternal(settlement.Village, Village.VillageStates.Normal, null);
		}

		// Token: 0x06004D52 RID: 19794 RVA: 0x00187231 File Offset: 0x00185431
		public static void ApplyBySettingToBeingRaided(Settlement settlement, MobileParty raider)
		{
			ChangeVillageStateAction.ApplyInternal(settlement.Village, Village.VillageStates.BeingRaided, raider);
		}

		// Token: 0x06004D53 RID: 19795 RVA: 0x00187240 File Offset: 0x00185440
		public static void ApplyBySettingToBeingForcedForSupplies(Settlement settlement, MobileParty raider)
		{
			ChangeVillageStateAction.ApplyInternal(settlement.Village, Village.VillageStates.ForcedForSupplies, raider);
		}

		// Token: 0x06004D54 RID: 19796 RVA: 0x0018724F File Offset: 0x0018544F
		public static void ApplyBySettingToBeingForcedForVolunteers(Settlement settlement, MobileParty raider)
		{
			ChangeVillageStateAction.ApplyInternal(settlement.Village, Village.VillageStates.ForcedForVolunteers, raider);
		}

		// Token: 0x06004D55 RID: 19797 RVA: 0x0018725E File Offset: 0x0018545E
		public static void ApplyBySettingToLooted(Settlement settlement, MobileParty raider)
		{
			ChangeVillageStateAction.ApplyInternal(settlement.Village, Village.VillageStates.Looted, raider);
		}
	}
}
