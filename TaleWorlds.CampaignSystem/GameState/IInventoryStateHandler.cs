using System;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003AF RID: 943
	public interface IInventoryStateHandler
	{
		// Token: 0x060036EE RID: 14062
		void ExecuteLootingScript();

		// Token: 0x060036EF RID: 14063
		void ExecuteSellAllLoot();

		// Token: 0x060036F0 RID: 14064
		void ExecuteBuyConsumableItem();
	}
}
