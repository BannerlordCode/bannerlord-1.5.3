using System;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004E0 RID: 1248
	public static class InitializeWorkshopAction
	{
		// Token: 0x06004DB1 RID: 19889 RVA: 0x001887C4 File Offset: 0x001869C4
		public static void ApplyByNewGame(Workshop workshop, Hero workshopOwner, WorkshopType workshopType)
		{
			workshop.InitializeWorkshop(workshopOwner, workshopType);
			TextObject textObject;
			TextObject textObject2;
			NameGenerator.Current.GenerateHeroNameAndHeroFullName(workshopOwner, out textObject, out textObject2, true);
			workshopOwner.SetName(textObject2, textObject);
			CampaignEventDispatcher.Instance.OnWorkshopInitialized(workshop);
		}
	}
}
