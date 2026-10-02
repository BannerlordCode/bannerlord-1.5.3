using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004F2 RID: 1266
	public static class StartMercenaryServiceAction
	{
		// Token: 0x06004DFA RID: 19962 RVA: 0x0018ACC4 File Offset: 0x00188EC4
		private static void ApplyStart(Clan clan, Kingdom kingdom, int awardMultiplier, StartMercenaryServiceAction.StartMercenaryServiceActionDetails details)
		{
			if (clan.IsUnderMercenaryService)
			{
				EndMercenaryServiceAction.EndByLeavingKingdom(clan);
			}
			clan.MercenaryAwardMultiplier = awardMultiplier;
			clan.Kingdom = kingdom;
			clan.StartMercenaryService();
			if (clan == Clan.PlayerClan)
			{
				Campaign.Current.KingdomManager.PlayerMercenaryServiceNextRenewalDay = Campaign.CurrentTime + 30f * (float)CampaignTime.HoursInDay;
			}
			CampaignEventDispatcher.Instance.OnMercenaryServiceStarted(clan, details);
		}

		// Token: 0x06004DFB RID: 19963 RVA: 0x0018AD28 File Offset: 0x00188F28
		public static void ApplyByDefault(Clan clan, Kingdom kingdom, int awardMultiplier)
		{
			StartMercenaryServiceAction.ApplyStart(clan, kingdom, awardMultiplier, StartMercenaryServiceAction.StartMercenaryServiceActionDetails.ApplyByDefault);
		}

		// Token: 0x020008F3 RID: 2291
		public enum StartMercenaryServiceActionDetails
		{
			// Token: 0x040026CA RID: 9930
			ApplyByDefault
		}
	}
}
