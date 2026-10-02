using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004DB RID: 1243
	public static class GainRenownAction
	{
		// Token: 0x06004D9F RID: 19871 RVA: 0x0018834F File Offset: 0x0018654F
		private static void ApplyInternal(Hero hero, float gainedRenown, bool doNotNotify)
		{
			if (gainedRenown > 0f)
			{
				hero.Clan.AddRenown(gainedRenown, true);
				CampaignEventDispatcher.Instance.OnRenownGained(hero, (int)gainedRenown, doNotNotify);
			}
		}

		// Token: 0x06004DA0 RID: 19872 RVA: 0x00188374 File Offset: 0x00186574
		public static void Apply(Hero hero, float renownValue, bool doNotNotify = false)
		{
			GainRenownAction.ApplyInternal(hero, renownValue, doNotNotify);
		}
	}
}
