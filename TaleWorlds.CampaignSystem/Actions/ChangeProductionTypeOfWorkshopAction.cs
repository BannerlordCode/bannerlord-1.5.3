using System;
using TaleWorlds.CampaignSystem.Settlements.Workshops;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004C7 RID: 1223
	public static class ChangeProductionTypeOfWorkshopAction
	{
		// Token: 0x06004D3A RID: 19770 RVA: 0x00186D88 File Offset: 0x00184F88
		public static void Apply(Workshop workshop, WorkshopType newWorkshopType, bool ignoreCost = false)
		{
			int num = (ignoreCost ? 0 : Campaign.Current.Models.WorkshopModel.GetConvertProductionCost(newWorkshopType));
			workshop.ChangeWorkshopProduction(newWorkshopType);
			if (num > 0)
			{
				GiveGoldAction.ApplyBetweenCharacters(workshop.Owner, null, num, false);
			}
			CampaignEventDispatcher.Instance.OnWorkshopTypeChanged(workshop);
		}
	}
}
