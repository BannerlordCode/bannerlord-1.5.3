using System;
using TaleWorlds.CampaignSystem.Settlements.Workshops;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004C5 RID: 1221
	public static class ChangeOwnerOfWorkshopAction
	{
		// Token: 0x06004D31 RID: 19761 RVA: 0x00186A54 File Offset: 0x00184C54
		private static void ApplyInternal(Workshop workshop, Hero newOwner, WorkshopType workshopType, int capital, int cost)
		{
			Hero owner = workshop.Owner;
			workshop.ChangeOwnerOfWorkshop(newOwner, workshopType, capital);
			if (newOwner == Hero.MainHero && cost > 0)
			{
				GiveGoldAction.ApplyBetweenCharacters(newOwner, owner, cost, false);
			}
			if (owner == Hero.MainHero && cost > 0)
			{
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, cost, false);
			}
			CampaignEventDispatcher.Instance.OnWorkshopOwnerChanged(workshop, owner);
		}

		// Token: 0x06004D32 RID: 19762 RVA: 0x00186AAF File Offset: 0x00184CAF
		public static void ApplyByBankruptcy(Workshop workshop, Hero newOwner, WorkshopType workshopType, int cost)
		{
			ChangeOwnerOfWorkshopAction.ApplyInternal(workshop, newOwner, workshopType, Campaign.Current.Models.WorkshopModel.InitialCapital, cost);
		}

		// Token: 0x06004D33 RID: 19763 RVA: 0x00186AD0 File Offset: 0x00184CD0
		public static void ApplyByPlayerBuying(Workshop workshop)
		{
			int costForPlayer = Campaign.Current.Models.WorkshopModel.GetCostForPlayer(workshop);
			ChangeOwnerOfWorkshopAction.ApplyInternal(workshop, Hero.MainHero, workshop.WorkshopType, Campaign.Current.Models.WorkshopModel.InitialCapital, costForPlayer);
		}

		// Token: 0x06004D34 RID: 19764 RVA: 0x00186B1C File Offset: 0x00184D1C
		public static void ApplyByPlayerSelling(Workshop workshop, Hero newOwner, WorkshopType workshopType)
		{
			int costForNotable = Campaign.Current.Models.WorkshopModel.GetCostForNotable(workshop);
			ChangeOwnerOfWorkshopAction.ApplyInternal(workshop, newOwner, workshopType, Campaign.Current.Models.WorkshopModel.InitialCapital, costForNotable);
		}

		// Token: 0x06004D35 RID: 19765 RVA: 0x00186B5C File Offset: 0x00184D5C
		public static void ApplyByDeath(Workshop workshop, Hero newOwner)
		{
			ChangeOwnerOfWorkshopAction.ApplyInternal(workshop, newOwner, workshop.WorkshopType, workshop.Capital, 0);
		}

		// Token: 0x06004D36 RID: 19766 RVA: 0x00186B72 File Offset: 0x00184D72
		public static void ApplyByWar(Workshop workshop, Hero newOwner, WorkshopType workshopType)
		{
			ChangeOwnerOfWorkshopAction.ApplyInternal(workshop, newOwner, workshopType, Campaign.Current.Models.WorkshopModel.InitialCapital, 0);
		}

		// Token: 0x06004D37 RID: 19767 RVA: 0x00186B91 File Offset: 0x00184D91
		public static void ApplyByFree(Workshop workshop, Hero newOwner)
		{
			ChangeOwnerOfWorkshopAction.ApplyInternal(workshop, newOwner, workshop.WorkshopType, Campaign.Current.Models.WorkshopModel.InitialCapital, 0);
		}
	}
}
