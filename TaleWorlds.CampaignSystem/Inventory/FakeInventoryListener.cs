using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Inventory
{
	// Token: 0x020000DD RID: 221
	public class FakeInventoryListener : InventoryListener
	{
		// Token: 0x0600150E RID: 5390 RVA: 0x000625DF File Offset: 0x000607DF
		public override int GetGold()
		{
			return 0;
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x000625E2 File Offset: 0x000607E2
		public override TextObject GetTraderName()
		{
			return TextObject.GetEmpty();
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x000625E9 File Offset: 0x000607E9
		public override void SetGold(int gold)
		{
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x000625EB File Offset: 0x000607EB
		public override void OnTransaction()
		{
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x000625ED File Offset: 0x000607ED
		public override PartyBase GetOppositeParty()
		{
			return null;
		}
	}
}
