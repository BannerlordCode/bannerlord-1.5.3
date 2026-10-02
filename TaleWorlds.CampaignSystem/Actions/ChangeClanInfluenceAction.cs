using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004BF RID: 1215
	public static class ChangeClanInfluenceAction
	{
		// Token: 0x06004D11 RID: 19729 RVA: 0x00185CB5 File Offset: 0x00183EB5
		private static void ApplyInternal(Clan clan, float amount)
		{
			clan.Influence += amount;
			CampaignEventDispatcher.Instance.OnClanInfluenceChanged(clan, amount);
		}

		// Token: 0x06004D12 RID: 19730 RVA: 0x00185CD1 File Offset: 0x00183ED1
		public static void Apply(Clan clan, float amount)
		{
			ChangeClanInfluenceAction.ApplyInternal(clan, amount);
		}
	}
}
