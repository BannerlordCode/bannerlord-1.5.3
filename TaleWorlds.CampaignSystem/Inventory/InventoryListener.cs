using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Inventory
{
	// Token: 0x020000DC RID: 220
	public abstract class InventoryListener
	{
		// Token: 0x06001508 RID: 5384
		public abstract int GetGold();

		// Token: 0x06001509 RID: 5385
		public abstract TextObject GetTraderName();

		// Token: 0x0600150A RID: 5386
		public abstract void SetGold(int gold);

		// Token: 0x0600150B RID: 5387
		public abstract PartyBase GetOppositeParty();

		// Token: 0x0600150C RID: 5388
		public abstract void OnTransaction();
	}
}
