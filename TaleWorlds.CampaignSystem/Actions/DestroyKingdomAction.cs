using System;
using System.Linq;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004D0 RID: 1232
	public static class DestroyKingdomAction
	{
		// Token: 0x06004D67 RID: 19815 RVA: 0x00187768 File Offset: 0x00185968
		private static void ApplyInternal(Kingdom destroyedKingdom, bool isKingdomLeaderDeath = false)
		{
			destroyedKingdom.DeactivateKingdom();
			foreach (Clan clan in destroyedKingdom.Clans.ToList<Clan>())
			{
				if (!clan.IsEliminated)
				{
					if (isKingdomLeaderDeath)
					{
						DestroyClanAction.ApplyByClanLeaderDeath(clan);
					}
					else
					{
						DestroyClanAction.Apply(clan);
					}
					destroyedKingdom.RemoveClanInternal(clan);
				}
			}
			Campaign.Current.FactionManager.RemoveFactionsFromCampaignWars(destroyedKingdom);
			CampaignEventDispatcher.Instance.OnKingdomDestroyed(destroyedKingdom);
		}

		// Token: 0x06004D68 RID: 19816 RVA: 0x001877FC File Offset: 0x001859FC
		public static void Apply(Kingdom destroyedKingdom)
		{
			DestroyKingdomAction.ApplyInternal(destroyedKingdom, false);
		}

		// Token: 0x06004D69 RID: 19817 RVA: 0x00187805 File Offset: 0x00185A05
		public static void ApplyByKingdomLeaderDeath(Kingdom destroyedKingdom)
		{
			DestroyKingdomAction.ApplyInternal(destroyedKingdom, true);
		}
	}
}
