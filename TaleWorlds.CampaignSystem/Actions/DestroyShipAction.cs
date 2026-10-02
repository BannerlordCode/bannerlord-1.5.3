using System;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004D2 RID: 1234
	public static class DestroyShipAction
	{
		// Token: 0x06004D6D RID: 19821 RVA: 0x00187924 File Offset: 0x00185B24
		private static void ApplyInternal(Ship ship, DestroyShipAction.ShipDestroyDetail detail)
		{
			PartyBase owner = ship.Owner;
			if (owner != null)
			{
				MobileParty mobileParty = owner.MobileParty;
				if (mobileParty != null)
				{
					mobileParty.SetNavalVisualAsDirty();
				}
			}
			ship.Owner = null;
			CampaignEventDispatcher.Instance.OnShipDestroyed(owner, ship, detail);
		}

		// Token: 0x06004D6E RID: 19822 RVA: 0x00187960 File Offset: 0x00185B60
		public static void Apply(Ship ship)
		{
			DestroyShipAction.ApplyInternal(ship, DestroyShipAction.ShipDestroyDetail.ApplyDefault);
		}

		// Token: 0x06004D6F RID: 19823 RVA: 0x00187969 File Offset: 0x00185B69
		public static void ApplyByDiscard(Ship ship)
		{
			DestroyShipAction.ApplyInternal(ship, DestroyShipAction.ShipDestroyDetail.ApplyByDiscard);
		}

		// Token: 0x020008E3 RID: 2275
		public enum ShipDestroyDetail
		{
			// Token: 0x04002681 RID: 9857
			ApplyDefault,
			// Token: 0x04002682 RID: 9858
			ApplyByDiscard
		}
	}
}
