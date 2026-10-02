using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004E6 RID: 1254
	public static class MakePregnantAction
	{
		// Token: 0x06004DCA RID: 19914 RVA: 0x001895DC File Offset: 0x001877DC
		private static void ApplyInternal(Hero mother)
		{
			mother.IsPregnant = true;
			CampaignEventDispatcher.Instance.OnChildConceived(mother);
		}

		// Token: 0x06004DCB RID: 19915 RVA: 0x001895F0 File Offset: 0x001877F0
		public static void Apply(Hero mother)
		{
			MakePregnantAction.ApplyInternal(mother);
		}
	}
}
