using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004B7 RID: 1207
	public static class AddCompanionAction
	{
		// Token: 0x06004CF7 RID: 19703 RVA: 0x00184F95 File Offset: 0x00183195
		private static void ApplyInternal(Clan clan, Hero companion)
		{
			if (companion.CompanionOf != null)
			{
				RemoveCompanionAction.ApplyByFire(companion.CompanionOf, companion);
			}
			companion.CompanionOf = clan;
			CampaignEventDispatcher.Instance.OnNewCompanionAdded(companion);
		}

		// Token: 0x06004CF8 RID: 19704 RVA: 0x00184FBD File Offset: 0x001831BD
		public static void Apply(Clan clan, Hero companion)
		{
			AddCompanionAction.ApplyInternal(clan, companion);
		}
	}
}
