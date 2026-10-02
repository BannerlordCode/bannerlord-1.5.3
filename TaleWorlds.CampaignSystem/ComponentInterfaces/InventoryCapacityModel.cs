using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C8 RID: 456
	public abstract class InventoryCapacityModel : MBGameModel<InventoryCapacityModel>
	{
		// Token: 0x06001E84 RID: 7812
		public abstract ExplainedNumber CalculateInventoryCapacity(MobileParty mobileParty, bool isCurrentlyAtSea, bool includeDescriptions = false, int additionalManOnFoot = 0, int additionalSpareMounts = 0, int additionalPackAnimals = 0, bool includeFollowers = false);

		// Token: 0x06001E85 RID: 7813
		public abstract int GetItemAverageWeight();

		// Token: 0x06001E86 RID: 7814
		public abstract float GetItemEffectiveWeight(EquipmentElement equipmentElement, MobileParty mobileParty, bool isCurrentlyAtSea, out TextObject description);

		// Token: 0x06001E87 RID: 7815
		public abstract ExplainedNumber CalculateTotalWeightCarried(MobileParty mobileParty, bool isCurrentlyAtSea, bool includeDescriptions = false);
	}
}
