using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x02000404 RID: 1028
	public abstract class ItemPickupModel : MBGameModel<ItemPickupModel>
	{
		// Token: 0x06003864 RID: 14436
		public abstract float GetItemScoreForAgent(SpawnedItemEntity item, Agent agent);

		// Token: 0x06003865 RID: 14437
		public abstract bool IsItemAvailableForAgent(SpawnedItemEntity item, Agent agent, EquipmentIndex slotToPickUp);

		// Token: 0x06003866 RID: 14438
		public abstract bool IsAgentEquipmentSuitableForPickUpAvailability(Agent agent);
	}
}
